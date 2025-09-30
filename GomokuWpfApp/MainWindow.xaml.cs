using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GomokuWpfApp.Models;

namespace GomokuWpfApp;

public partial class MainWindow : Window
{
    private readonly GameBoard _gameBoard = new();
    private readonly Button[,] _boardButtons;
    private bool _isBlackTurn = true;
    private bool _isGameOver;
    private Button? _lastMoveButton;

    public MainWindow()
    {
        InitializeComponent();
        _boardButtons = new Button[_gameBoard.Size, _gameBoard.Size];
        InitializeBoard();
        UpdateStatusMessage();
    }

    private void InitializeBoard()
    {
        BoardGrid.Rows = _gameBoard.Size;
        BoardGrid.Columns = _gameBoard.Size;
        BoardGrid.Children.Clear();

        for (int row = 0; row < _gameBoard.Size; row++)
        {
            for (int column = 0; column < _gameBoard.Size; column++)
            {
                var button = new Button
                {
                    Style = (Style)FindResource("BoardCellButtonStyle"),
                    Tag = new CellPosition(row, column)
                };

                button.Click += HandleCellClick;
                _boardButtons[row, column] = button;
                BoardGrid.Children.Add(button);
            }
        }
    }

    private void HandleCellClick(object sender, RoutedEventArgs e)
    {
        if (_isGameOver)
        {
            return;
        }

        if (sender is not Button button || button.Tag is not CellPosition position)
        {
            return;
        }

        var stone = _isBlackTurn ? Stone.Black : Stone.White;
        var moveResult = _gameBoard.PlaceStone(position.Row, position.Column, stone);

        if (!moveResult.Placed)
        {
            ShowInvalidMoveMessage();
            return;
        }

        UpdateCellVisual(position.Row, position.Column, stone);
        HighlightLastMove(button);

        if (moveResult.IsWinningMove)
        {
            _isGameOver = true;
            BoardGrid.IsEnabled = false;
            StatusTextBlock.Text = $"{GetPlayerName(stone)}獲勝！";
            return;
        }

        if (moveResult.IsDraw)
        {
            _isGameOver = true;
            BoardGrid.IsEnabled = false;
            StatusTextBlock.Text = "平手！棋盤已滿。";
            return;
        }

        _isBlackTurn = !_isBlackTurn;
        UpdateStatusMessage();
    }

    private void HighlightLastMove(Button button)
    {
        if (_lastMoveButton != null)
        {
            _lastMoveButton.Background = (Brush)FindResource("BoardCellBackgroundBrush");
        }

        button.Background = (Brush)FindResource("BoardCellHighlightBrush");
        _lastMoveButton = button;
    }

    private void UpdateCellVisual(int row, int column, Stone stone)
    {
        var button = _boardButtons[row, column];

        if (stone == Stone.None)
        {
            button.Content = null;
            return;
        }

        var ellipse = new Ellipse
        {
            Width = 28,
            Height = 28,
            Fill = stone == Stone.Black ? Brushes.Black : Brushes.White,
            Stroke = stone == Stone.Black
                ? Brushes.Black
                : new SolidColorBrush(Color.FromRgb(64, 64, 64)),
            StrokeThickness = 1.5
        };

        button.Content = ellipse;
    }

    private void ShowInvalidMoveMessage()
    {
        var playerName = _isBlackTurn ? "黑方" : "白方";
        StatusTextBlock.Text = $"{playerName}：此位置已有棋子，請重新選擇。";
    }

    private void UpdateStatusMessage()
    {
        var playerName = _isBlackTurn ? "黑方" : "白方";
        StatusTextBlock.Text = $"{playerName}的回合";
    }

    private static string GetPlayerName(Stone stone) => stone switch
    {
        Stone.Black => "黑方",
        Stone.White => "白方",
        _ => ""
    };

    private void NewGameButton_Click(object sender, RoutedEventArgs e)
    {
        _gameBoard.Reset();
        ResetBoardVisuals();
        _isGameOver = false;
        _isBlackTurn = true;
        BoardGrid.IsEnabled = true;
        UpdateStatusMessage();
    }

    private void ResetBoardVisuals()
    {
        for (int row = 0; row < _gameBoard.Size; row++)
        {
            for (int column = 0; column < _gameBoard.Size; column++)
            {
                var button = _boardButtons[row, column];
                button.Content = null;
                button.Background = (Brush)FindResource("BoardCellBackgroundBrush");
            }
        }

        _lastMoveButton = null;
    }

    private readonly record struct CellPosition(int Row, int Column);
}
