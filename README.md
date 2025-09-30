# ChatgptCodeTest

此儲存庫現在包含一個使用 WPF 建立的五子棋（Gomoku）應用程式。介面支援 15x15 棋盤、黑白雙方輪流落子、最近一步高亮提示與重新開始的功能。

## 功能摘要
- 15x15 棋盤，支援基本勝負與和局判定。
- 黑白雙方自動輪轉，狀態列會顯示當前輪到哪一方。
- 點擊既有棋子的位置會提示玩家該位置不可再落子。
- 高亮最新落子的位置，方便追蹤棋局進度。
- 一鍵重新開始，快速重置棋局。

## 執行環境
> ⚠️ WPF 僅支援 Windows。請在 Windows 10 以上版本並安裝 .NET 7 SDK（或相容版本）後再開啟專案。

1. 透過 Visual Studio 2022（或更新版本）開啟 `GomokuWpfApp.sln` 解決方案。
2. 使用「啟動」按鈕或 F5 編譯並執行 `GomokuWpfApp` 專案。
3. 若使用命令列，也可以在專案目錄執行：
   ```bash
   dotnet build
   dotnet run
   ```

## 專案結構
```
├── GomokuWpfApp.sln
└── GomokuWpfApp
    ├── App.xaml
    ├── App.xaml.cs
    ├── GomokuWpfApp.csproj
    ├── MainWindow.xaml
    ├── MainWindow.xaml.cs
    └── Models
        └── GameBoard.cs
```

## 遊戲玩法
1. 應用程式啟動後預設由黑方先行。
2. 以滑鼠點擊棋盤上的格子即可落子，五顆相連即獲勝。
3. 棋盤填滿仍無人勝出時視為平手。
4. 任何時候都可使用「重新開始」按鈕清空棋局。
