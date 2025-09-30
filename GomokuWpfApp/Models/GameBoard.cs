using System;

namespace GomokuWpfApp.Models;

/// <summary>
/// 提供五子棋棋盤的邏輯運算與狀態管理。
/// </summary>
public class GameBoard
{
    private readonly Stone[,] _cells;
    private int _moveCount;

    public const int DefaultSize = 15;

    public GameBoard(int size = DefaultSize)
    {
        if (size < 5)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "棋盤大小至少需為 5。");
        }

        Size = size;
        _cells = new Stone[size, size];
    }

    /// <summary>
    /// 棋盤的長寬尺寸。
    /// </summary>
    public int Size { get; }

    /// <summary>
    /// 嘗試在指定位置放置一顆棋子。
    /// </summary>
    /// <param name="row">列索引（0 起算）。</param>
    /// <param name="column">行索引（0 起算）。</param>
    /// <param name="stone">準備放置的棋子。</param>
    /// <returns>描述此次落子的結果。</returns>
    public MoveResult PlaceStone(int row, int column, Stone stone)
    {
        if (stone == Stone.None)
        {
            throw new ArgumentException("無效的棋子類型。", nameof(stone));
        }

        if (!IsWithinBounds(row, column))
        {
            return MoveResult.Invalid;
        }

        if (_cells[row, column] != Stone.None)
        {
            return MoveResult.Invalid;
        }

        _cells[row, column] = stone;
        _moveCount++;

        var isWin = HasFiveInARow(row, column);
        var isDraw = !isWin && _moveCount == Size * Size;

        if (isWin)
        {
            return MoveResult.Win;
        }

        if (isDraw)
        {
            return MoveResult.Draw;
        }

        return MoveResult.Valid;
    }

    /// <summary>
    /// 取得指定位置的棋子。
    /// </summary>
    public Stone GetStone(int row, int column)
    {
        if (!IsWithinBounds(row, column))
        {
            throw new ArgumentOutOfRangeException("指定的座標超出棋盤範圍。");
        }

        return _cells[row, column];
    }

    /// <summary>
    /// 重設棋盤。
    /// </summary>
    public void Reset()
    {
        Array.Clear(_cells, 0, _cells.Length);
        _moveCount = 0;
    }

    private bool HasFiveInARow(int row, int column)
    {
        var stone = _cells[row, column];
        if (stone == Stone.None)
        {
            return false;
        }

        return HasFiveInDirection(row, column, 1, 0) ||
               HasFiveInDirection(row, column, 0, 1) ||
               HasFiveInDirection(row, column, 1, 1) ||
               HasFiveInDirection(row, column, 1, -1);
    }

    private bool HasFiveInDirection(int row, int column, int deltaRow, int deltaColumn)
    {
        var stone = _cells[row, column];
        var count = 1;

        count += CountDirection(row, column, deltaRow, deltaColumn, stone);
        count += CountDirection(row, column, -deltaRow, -deltaColumn, stone);

        return count >= 5;
    }

    private int CountDirection(int startRow, int startColumn, int deltaRow, int deltaColumn, Stone stone)
    {
        var count = 0;
        var row = startRow + deltaRow;
        var column = startColumn + deltaColumn;

        while (IsWithinBounds(row, column) && _cells[row, column] == stone)
        {
            count++;
            row += deltaRow;
            column += deltaColumn;
        }

        return count;
    }

    private bool IsWithinBounds(int row, int column) =>
        row >= 0 && row < Size && column >= 0 && column < Size;

    public readonly record struct MoveResult(bool Placed, bool IsWinningMove, bool IsDraw)
    {
        public static MoveResult Invalid => new(false, false, false);
        public static MoveResult Valid => new(true, false, false);
        public static MoveResult Win => new(true, true, false);
        public static MoveResult Draw => new(true, false, true);
    }
}

public enum Stone
{
    None,
    Black,
    White
}
