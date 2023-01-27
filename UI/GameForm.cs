#nullable enable
using System.ComponentModel;
using Checkers.Libraries;

namespace Checkers.UI;

public class GameForm : Form
{
    public enum MoveState
    {
        Incorrect,
        Continue,
        Kill,
        Correct,
        Player1Win,
        PLayer2Win
    }

    private Cell[] Cells =
    {
        new(), new(), new(), new(), new(), new(), new(), new(),
        new(), new(), new(), new(), new(), new(), new(), new(),
        new(), new(), new(), new(), new(), new(), new(), new(),
        new(), new(), new(), new(), new(), new(), new(), new(),
        new(), new(), new(), new(), new(), new(), new(), new(),
        new(), new(), new(), new(), new(), new(), new(), new(),
        new(), new(), new(), new(), new(), new(), new(), new(),
        new(), new(), new(), new(), new(), new(), new(), new()
    };
        
    public Checker[] AllCheckers =
    {
        new(Checker.CheckerOwner.Player1), new(Checker.CheckerOwner.Player1),
        new(Checker.CheckerOwner.Player1), new(Checker.CheckerOwner.Player1),
        new(Checker.CheckerOwner.Player1), new(Checker.CheckerOwner.Player1),
        new(Checker.CheckerOwner.Player1), new(Checker.CheckerOwner.Player1),
        new(Checker.CheckerOwner.Player1), new(Checker.CheckerOwner.Player1),
        new(Checker.CheckerOwner.Player1), new(Checker.CheckerOwner.Player1),
            
        new(Checker.CheckerOwner.Player2), new(Checker.CheckerOwner.Player2),
        new(Checker.CheckerOwner.Player2), new(Checker.CheckerOwner.Player2),
        new(Checker.CheckerOwner.Player2), new(Checker.CheckerOwner.Player2),
        new(Checker.CheckerOwner.Player2), new(Checker.CheckerOwner.Player2),
        new(Checker.CheckerOwner.Player2), new(Checker.CheckerOwner.Player2),
        new(Checker.CheckerOwner.Player2), new(Checker.CheckerOwner.Player2)
    };
        
    private Label Player1NicknameLabel = new();
    private Label Player2NicknameLabel = new();

    private Label MessageBox = new();

    private Button ReturnToMenuButton = new();
        
    ComponentResourceManager? resources;

    public Player Player1 = new(), Player2 = new();
        
    private int CurrentPressedCellIndex = -1;

    private bool MustMakeKill;

    public enum Players
    {
        Player1 = 0,
        Player2 = 1,
        Jesus = 2
    }

    public Players CurrentPlayer;
        
    private void InitializeForm()
    {
        var resources = new ComponentResourceManager(typeof(GameForm));
            
        Icon = ((Icon)(resources.GetObject("$this.Icon")!));
        Name = "GameForm";
        Text = "DIvanCheckers";
            
        AutoScaleDimensions = new SizeF(18F, 31F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
            
        BackColor = SystemColors.Window;
        Font = new Font("DejaVu Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 204);

        foreach (var cell in Cells)
        {
            Controls.Add(cell);
        }
            
        Controls.Add(Player1NicknameLabel);
        Controls.Add(Player2NicknameLabel);
        
        Controls.Add(MessageBox);
            
        Controls.Add(ReturnToMenuButton);
    }

    private void InitializeBoard()
    {
        var resources = new ComponentResourceManager(typeof(GameForm));

        var checkerIndex = 0;
            
        for (var i = 0; i < Cells.Length; ++i)
        {
            Cells[i].TabStop = false;
            Cells[i].FlatStyle = FlatStyle.Flat;
            Cells[i].FlatAppearance.BorderSize = 0;
                
            Cells[i].Index = i;
            Cells[i].Row = i / 8;
            Cells[i].Col = i % 8;

            Cells[i].Color = i % 2 == i / 8 % 2 ?
                Cell.CellColor.White : Cell.CellColor.Black;

            if (Cells[i].Color == Cell.CellColor.Black && Cells[i].Row <= 2)
            {
                Cells[i].CellCheckerIndex = checkerIndex;
                AllCheckers[checkerIndex].Row = Cells[i].Row;
                AllCheckers[checkerIndex].Col = Cells[i].Col;
                AllCheckers[checkerIndex].CheckerCellIndex = i;
                checkerIndex++;
            }

            if (Cells[i].Color == Cell.CellColor.Black && Cells[i].Row >= 5)
            {
                Cells[i].CellCheckerIndex = checkerIndex;
                AllCheckers[checkerIndex].Row = Cells[i].Row;
                AllCheckers[checkerIndex].Col = Cells[i].Col;
                AllCheckers[checkerIndex].CheckerCellIndex = i;
                checkerIndex++;
            }
                
            Cells[i].Click += CellClickHandler;
                
            Cells[i].Width = 50;
            Cells[i].Height = 50;

            Cells[i].X = 500 + Cells[i].Col * Cells[i].Width;
            Cells[i].Y = 500 - Cells[i].Row * Cells[i].Height;

            Cells[i].Name = string.Concat("FieldButton", i.ToString());
                
            Cells[i].Location = new Point(Cells[i].X, Cells[i].Y);
            Cells[i].Size = new Size(Cells[i].Width, Cells[i].Height);
            Cells[i].TabIndex = 0;
                
            Cells[i].Image = Cells[i].Color == Cell.CellColor.White ?
                (Image)resources.GetObject("WhiteCell.Image")! :
                Cells[i].CellCheckerIndex == -1 ?
                    (Image)resources.GetObject("BlackCell.Image")! :
                    AllCheckers[Cells[i].CellCheckerIndex].Owner == Checker.CheckerOwner.Player1 ?
                        (Image)resources.GetObject("Player1Checker.Image")! :
                        (Image)resources.GetObject("Player2Checker.Image")!;

        }

        MakePlayersCellsClickable(CurrentPlayer);
    }
        
    private void InitializePlayer1NicknameLabel()
    {
        Player1NicknameLabel.Name = "Player1NicknameLabel";
            
        Player1NicknameLabel.Location = new Point(550, 580);
        Player1NicknameLabel.Size = new Size(200, 40);
        Player1NicknameLabel.TabIndex = 0;
            
        Player1NicknameLabel.BackColor = SystemColors.Window;
        Player1NicknameLabel.Text = Player1.Nickname;
    }
        
    private void InitializePlayer2NicknameLabel()
    {
        Player2NicknameLabel.Name = "Player2NicknameLabel";
            
        Player2NicknameLabel.Location = new Point(550, 80);
        Player2NicknameLabel.Size = new Size(200, 40);
        Player2NicknameLabel.TabIndex = 0;
            
        Player2NicknameLabel.BackColor = SystemColors.Window;
        Player2NicknameLabel.Text = Player2.Nickname;
    }

    private void InitializeMessageBox()
    {
        MessageBox.Name = "MessageBox";
            
        MessageBox.Location = new Point(1000, 200);
        MessageBox.Size = new Size(500, 40);
        MessageBox.TabIndex = 0;
            
        MessageBox.BackColor = SystemColors.Window;
        MessageBox.Text = "MessageBox";

        MessageBox.Hide();
    }

    private void InitializeReturnToMenuButton()
    {
        ReturnToMenuButton.Name = "ReturnToMenuButton";
            
        ReturnToMenuButton.Location = new Point(1000, 300);
        ReturnToMenuButton.Size = new Size(300, 50);
        ReturnToMenuButton.TabIndex = 0;
            
        ReturnToMenuButton.BackColor = SystemColors.Menu;
        ReturnToMenuButton.Text = "Вернуться в меню";
        ReturnToMenuButton.Cursor = Cursors.Arrow;
            
        ReturnToMenuButton.Click += ReturnToMainMenuButton_Click;
    }
        
    private void Initialize()
    {
        resources = new ComponentResourceManager(typeof(GameForm));
            
        SuspendLayout();

        InitializeBoard();

        InitializePlayer1NicknameLabel();
        InitializePlayer2NicknameLabel();

        InitializeMessageBox();
        
        InitializeReturnToMenuButton();

        InitializeForm();
            
        ResumeLayout();
    }
        
    public GameForm(string player1Nickname, string player2Nickname)
    {

        Player1.Init(player1Nickname, 12);
        Player2.Init(player2Nickname, 12);

        CurrentPlayer = Players.Player1;
            
        Initialize();
            
        Closing += GameForm_FormClosing;
    }
        
    private static void GameForm_FormClosing(object? sender, CancelEventArgs cancelEventArgs)
    {
        Application.Exit();
    }
        
    private void CellClickHandler(object? sender, EventArgs? eventArgs)
    {
        var pressedCellIndex = ((Cell)sender!).Index;

        if (CurrentPressedCellIndex != -1 && !MustMakeKill && CurrentPressedCellIndex != pressedCellIndex &&
            Cells[CurrentPressedCellIndex].CellCheckerIndex != -1 && Cells[pressedCellIndex].CellCheckerIndex != -1 &&
            AllCheckers[Cells[CurrentPressedCellIndex].CellCheckerIndex].Owner ==
            AllCheckers[Cells[pressedCellIndex].CellCheckerIndex].Owner)
        {
            UnPressCell(CurrentPressedCellIndex);
            PressCell(pressedCellIndex);
            CurrentPressedCellIndex = pressedCellIndex;
            return;
        }

        if (Cells[pressedCellIndex].Cursor != Cursors.Hand)
        {
            return;
        }
            
        if (Cells[pressedCellIndex].CellCheckerIndex == -1)
        {
            UnPressCell(CurrentPressedCellIndex);
                
            var moveState = MakeMove(CurrentPressedCellIndex, pressedCellIndex);

            if (moveState == MoveState.Player1Win)
            {
                EndGame(Players.Player1);
                return;
            }

            if (moveState == MoveState.PLayer2Win)
            {
                EndGame(Players.Player2);
                return;
            }

            if (moveState == MoveState.Incorrect)
            {
                PressCell(CurrentPressedCellIndex);
                return;
            }
                
            if (MustMakeKill && moveState == MoveState.Correct)
            {
                PressCell(CurrentPressedCellIndex);
                return;
            }
                
            if (moveState == MoveState.Kill || moveState == MoveState.Correct)
            {
                MakePossibleCellsUnClickable();
                MakePlayersCellsClickable(CurrentPlayer);
            }

            if (moveState == MoveState.Continue)
            {
                MustMakeKill = true;
            }
            else
            {
                MustMakeKill = false;
                CurrentPressedCellIndex = -1;    
            }
                
            return;
        }
            
        if (AllCheckers[Cells[pressedCellIndex].CellCheckerIndex].Owner.ToString() != CurrentPlayer.ToString())
        {
            return;
        }
            
        if (CurrentPressedCellIndex == pressedCellIndex)
        {
            if (!MustMakeKill)
            {
                UnPressCell(CurrentPressedCellIndex);
                MakePlayersCellsClickable(CurrentPlayer);
                MakePossibleCellsUnClickable();
                CurrentPressedCellIndex = -1;
            }
            return;
        }
            
        CurrentPressedCellIndex = -1;

        MakePossibleCellsClickable();
        MakeOtherCheckersUnClickable(pressedCellIndex);
        PressCell(pressedCellIndex);

        CurrentPressedCellIndex = pressedCellIndex;
    }
        
    private void PutCheckerImage(int cellIndex)
    {
        if (Cells[cellIndex].CellCheckerIndex == -1)
        {
            return;
        }

        var checker = AllCheckers[Cells[cellIndex].CellCheckerIndex]; 
            
        if (checker.Owner == Checker.CheckerOwner.Player1 &&
            checker.Type == Checker.CheckerType.Checker)
        {
            Cells[cellIndex].Image =
                (Image)resources!.GetObject("Player1Checker.Image")!;
        }
        else if (checker.Owner == Checker.CheckerOwner.Player1 &&
                 checker.Type == Checker.CheckerType.Crown)
        {
            Cells[cellIndex].Image =
                (Image)resources!.GetObject("Player1Crown.Image")!;
        }
        else if (checker.Owner == Checker.CheckerOwner.Player2 &&
                 checker.Type == Checker.CheckerType.Checker)
        {
            Cells[cellIndex].Image =
                (Image)resources!.GetObject("Player2Checker.Image")!;
        }
        else
        {
            Cells[cellIndex].Image =
                (Image)resources!.GetObject("Player2Crown.Image")!;
        }
    }

    private void PutActiveCheckerImage(int pressedCellIndex)
    {
        if (Cells[pressedCellIndex].CellCheckerIndex == -1)
        {
            return;
        }
            
        var checker = AllCheckers[Cells[pressedCellIndex].CellCheckerIndex];
            
        if (checker.Owner == Checker.CheckerOwner.Player1 &&
            checker.Type == Checker.CheckerType.Checker)
        {
            Cells[pressedCellIndex].Image =
                (Image)resources!.GetObject("Player1CheckerActive.Image")!;            
        }
        else if (checker.Owner == Checker.CheckerOwner.Player1 &&
                 checker.Type == Checker.CheckerType.Crown)
        {
            Cells[pressedCellIndex].Image =
                (Image)resources!.GetObject("Player1CrownActive.Image")!;
        }
        else if (checker.Owner == Checker.CheckerOwner.Player2 &&
                 checker.Type == Checker.CheckerType.Checker)
        {
            Cells[pressedCellIndex].Image =
                (Image)resources!.GetObject("Player2CheckerActive.Image")!; 
        }
        else
        {
            Cells[pressedCellIndex].Image =
                (Image)resources!.GetObject("Player2CrownActive.Image")!;
        }
    }

    private void UnPressCell(int pressedCellIndex) // Only for Cells with Checkers
    {
        if (Cells[pressedCellIndex].CellCheckerIndex == -1)
        {
            return;
        }
            
        PutCheckerImage(pressedCellIndex);
    }

    private void PressCell(int pressedCellIndex) // Only for Cells with Checkers
    {
        if (Cells[pressedCellIndex].CellCheckerIndex == -1)
        {
            return;
        }

        PutActiveCheckerImage(pressedCellIndex);
    }
        
    private void MakePossibleCellsUnClickable()
    {
        for (var i = 0; i < Cells.Length; ++i)
        {
            if (Cells[i].Color == Cell.CellColor.Black && Cells[i].CellCheckerIndex == -1)
            {
                Cells[i].Cursor = Cursors.Arrow;
            }
        }
    }

    private void MakePossibleCellsClickable()
    {
        for (var i = 0; i < Cells.Length; ++i)
        {
            if (Cells[i].Color == Cell.CellColor.Black && Cells[i].CellCheckerIndex == -1)
            {
                Cells[i].Cursor = Cursors.Hand;
            }
        }
    }

    private void MakeOtherCheckersUnClickable(int pressedCellIndex)
    {
        for (var i = 0; i < Cells.Length; ++i)
        {
            if (Cells[i].CellCheckerIndex != -1 && i != pressedCellIndex)
            {
                Cells[i].Cursor = Cursors.Arrow;
            }
        }
    }
        
    private void MakePlayersCellsClickable(Players currentPlayer)
    {
        for (var i = 0; i < Cells.Length; ++i)
        {
            if (Cells[i].CellCheckerIndex != -1 &&
                AllCheckers[Cells[i].CellCheckerIndex].Owner.ToString() == currentPlayer.ToString())
            {
                Cells[i].Cursor = Cursors.Hand;
            }
        }
    }

    private void MakePlayersCellsUnClickable(Players currentPlayer)
    {
        for (var i = 0; i < Cells.Length; ++i)
        {
            if (Cells[i].CellCheckerIndex != -1 &&
                AllCheckers[Cells[i].CellCheckerIndex].Owner.ToString() == currentPlayer.ToString())
            {
                Cells[i].Cursor = Cursors.Arrow;
            }
        }
    }

    private void RemoveChecker(int cellIndex)
    {
        var checkerIndex = Cells[cellIndex].CellCheckerIndex;
            
        Cells[cellIndex].CellCheckerIndex = -1;
        Cells[cellIndex].Cursor = Cursors.Arrow;
        Cells[cellIndex].Image = (Image)resources!.GetObject("BlackCell.Image")!;
    }

    private void KillChecker(int cellIndex)
    {
        var checkerIndex = Cells[cellIndex].CellCheckerIndex;
            
        RemoveChecker(cellIndex);

        AllCheckers[checkerIndex].Row = -1;
        AllCheckers[checkerIndex].Col = -1;
        AllCheckers[checkerIndex].CheckerCellIndex = -1;

        if (AllCheckers[checkerIndex].Owner == Checker.CheckerOwner.Player1)
        {
            Player1.AliveCheckers--;
        }
        else
        {
            Player2.AliveCheckers--;
        }
    }
        
    private void PutChecker(int fromCellIndex, int toCellIndex)
    {
        var checkerIndex = Cells[fromCellIndex].CellCheckerIndex;

        Cells[toCellIndex].CellCheckerIndex = checkerIndex;

        AllCheckers[checkerIndex].Row = Cells[toCellIndex].Row;
        AllCheckers[checkerIndex].Col = Cells[toCellIndex].Col;
        AllCheckers[checkerIndex].CheckerCellIndex = toCellIndex;

        if (AllCheckers[checkerIndex].Owner == Checker.CheckerOwner.Player1 &&
            AllCheckers[checkerIndex].Row == 7)
        {
            AllCheckers[checkerIndex].Type = Checker.CheckerType.Crown;
        }
            
        if (AllCheckers[checkerIndex].Owner == Checker.CheckerOwner.Player2 &&
            AllCheckers[checkerIndex].Row == 0)
        {
            AllCheckers[checkerIndex].Type = Checker.CheckerType.Crown;
        }
            
        PutCheckerImage(toCellIndex);
    }
        
    private bool CheckPlayerCanMove(Players player)
    {
        for (var i = 0; i < AllCheckers.Length; ++i)
        {
            var aliveChecker = AllCheckers[i];
                
            if (AllCheckers[i].CheckerCellIndex != -1 &&
                aliveChecker.Owner.ToString() == player.ToString() &&
                (AllCheckers[i].Type == Checker.CheckerType.Checker &&
                 (CheckCheckerCanMove(i) || CheckCheckerCanKillMove(i)) || 
                 AllCheckers[i].Type == Checker.CheckerType.Crown &&
                 (CheckCrownCanMove(i) || CheckCrownCanKillMove(i))))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckPlayerCanKillMove(Players player)
    {
        for (var i = 0; i < AllCheckers.Length; ++i)
        {
            var aliveChecker = AllCheckers[i];
                
            if (AllCheckers[i].CheckerCellIndex != -1 &&
                aliveChecker.Owner.ToString() == player.ToString() &&
                (AllCheckers[i].Type == Checker.CheckerType.Checker && CheckCheckerCanKillMove(i) || 
                 AllCheckers[i].Type == Checker.CheckerType.Crown && CheckCrownCanKillMove(i)))
            {
                return true;
            }
        }

        return false;
    }
        
    private bool CheckCheckerCanMove(int checkerIndex)
    {
        var cellIndex = AllCheckers[checkerIndex].CheckerCellIndex;
        var cell = Cells[cellIndex];
            
        for (var dRow = -1; dRow <= 1; ++dRow)
        {
            for (var dCol = -1; dCol <= 1; ++dCol)
            {
                if (Math.Abs(dRow) + Math.Abs(dCol) != 2)
                {
                    continue;
                }

                var nextCellRow = cell.Row + dRow;
                var nextCellCol = cell.Col + dCol;

                if (nextCellRow < 0 || nextCellRow > 7 || nextCellCol < 0 || nextCellCol > 7)
                {
                    continue;
                }

                var nextCellIndex = nextCellRow * 8 + nextCellCol;

                if (CheckCheckerMove(cellIndex, nextCellIndex))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckCheckerCanKillMove(int checkerIndex)
    {
        var cellIndex = AllCheckers[checkerIndex].CheckerCellIndex;
        var cell = Cells[cellIndex];
            
        for (var dRow = -1; dRow <= 1; ++dRow)
        {
            for (var dCol = -1; dCol <= 1; ++dCol)
            {
                if (Math.Abs(dRow) + Math.Abs(dCol) != 2)
                {
                    continue;
                }

                var nextCellRow = cell.Row + 2 * dRow;
                var nextCellCol = cell.Col + 2 * dCol;

                if (nextCellRow < 0 || nextCellRow > 7 || nextCellCol < 0 || nextCellCol > 7)
                {
                    continue;
                }

                var nextCellIndex = nextCellRow * 8 + nextCellCol;

                if (CheckCheckerKillMove(cellIndex, nextCellIndex))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckCrownCanMove(int checkerIndex)
    {
        var cellIndex = AllCheckers[checkerIndex].CheckerCellIndex;
        var cell = Cells[cellIndex];

        for (var dRow = -1; dRow <= 1; ++dRow)
        {
            for (var dCol = -1; dCol <= 1; ++dCol)
            {
                for (var length = 2; length <= 7; ++length)
                {
                    if (Math.Abs(dRow) + Math.Abs(dCol) != 2)
                    {
                        continue;
                    }

                    var nextCellRow = cell.Row + (length - 1) * dRow;
                    var nextCellCol = cell.Col + (length - 1) * dCol;

                    if (nextCellRow < 0 || nextCellRow > 7 || nextCellCol < 0 || nextCellCol > 7)
                    {
                        continue;
                    }

                    var nextCellIndex = nextCellRow * 8 + nextCellCol;

                    if (CheckCrownMove(cellIndex, nextCellIndex))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool CheckCrownCanKillMove(int checkerIndex)
    {
        var cellIndex = AllCheckers[checkerIndex].CheckerCellIndex;
        var cell = Cells[cellIndex];
            
        for (var dRow = -1; dRow <= 1; ++dRow)
        {
            for (var dCol = -1; dCol <= 1; ++dCol)
            {   
                for (var length = 2; length <= 7; ++length)
                {
                    if (Math.Abs(dRow) + Math.Abs(dCol) != 2)
                    {
                        continue;
                    }
                    
                    var nextCellRow = cell.Row + length * dRow;
                    var nextCellCol = cell.Col + length * dCol;

                    if (nextCellRow < 0 || nextCellRow > 7 || nextCellCol < 0 || nextCellCol > 7)
                    {
                        continue;
                    }

                    var nextCellIndex = nextCellRow * 8 + nextCellCol;

                    if (CheckCrownKillMove(cellIndex, nextCellIndex))
                    {
                        return true;
                    }
                }
            }
        }
        
        return false;
    }
    
    private bool CheckCheckerMove(int fromCellIndex, int toCellIndex)
    {
        if (Cells[toCellIndex].CellCheckerIndex != -1)
        {
            return false;
        }
            
        var checker = AllCheckers[Cells[fromCellIndex].CellCheckerIndex];
            
        var deltaRow = Cells[toCellIndex].Row - Cells[fromCellIndex].Row;
        var deltaCol = Cells[toCellIndex].Col - Cells[fromCellIndex].Col;

        if (Math.Abs(deltaRow) != 1 || Math.Abs(deltaCol) != 1)
        {
            return false;
        }

        if (Math.Abs(deltaRow) == 2 && Math.Abs(deltaCol) == 2)
        {
            return false;
        }

        var fromCell = Cells[fromCellIndex];
        var toCell = Cells[toCellIndex];

        if (checker.Owner == Checker.CheckerOwner.Player1 && toCell.Row < fromCell.Row)
        {
            return false;
        }
            
        if (checker.Owner == Checker.CheckerOwner.Player2 && toCell.Row > fromCell.Row)
        {
            return false;
        }
            
        return true;
    }


    private bool CheckCheckerKillMove(int fromCellIndex, int toCellIndex)
    {
        if (Cells[toCellIndex].CellCheckerIndex != -1)
        {
            return false;
        }
            
        var checker = AllCheckers[Cells[fromCellIndex].CellCheckerIndex];
            
        var deltaRow = Cells[toCellIndex].Row - Cells[fromCellIndex].Row;
        var deltaCol = Cells[toCellIndex].Col - Cells[fromCellIndex].Col;

        if (Math.Abs(deltaRow) != 2 || Math.Abs(deltaCol) != 2)
        {
            return false;
        }
            
        var killCellRow = checker.Row + deltaRow / 2;
        var killCellCol = checker.Col + deltaCol / 2;

        var killCellIndex = killCellRow * 8 + killCellCol;

        var killCell = Cells[killCellIndex];

        if (killCell.CellCheckerIndex == -1 ||
            AllCheckers[killCell.CellCheckerIndex].Owner == checker.Owner)
        {
            return false;
        }
            
        return true;
    }
    
    private bool CheckCrownMove(int fromCellIndex, int toCellIndex)
    {
        if (Cells[toCellIndex].CellCheckerIndex != -1)
        {
            return false;
        }
            
        var checker = AllCheckers[Cells[fromCellIndex].CellCheckerIndex];
            
        var deltaRow = Cells[toCellIndex].Row - Cells[fromCellIndex].Row;
        var deltaCol = Cells[toCellIndex].Col - Cells[fromCellIndex].Col;

        if (Math.Abs(deltaRow) != Math.Abs(deltaCol))
        {
            return false;
        }

        var dRow = deltaRow / Math.Abs(deltaRow);
        var dCol = deltaCol / Math.Abs(deltaCol);
            
        var moveLength = Math.Abs(deltaRow);

        var cntBetween = 0;

        for (var length = 1; length < moveLength; ++length)
        {
            var cellRow = checker.Row + length * dRow;
            var cellCol = checker.Col + length * dCol;

            var cellIndex = cellRow * 8 + cellCol;

            var cell = Cells[cellIndex];

            if (cell.CellCheckerIndex == -1)
            {
                continue;
            }

            if (AllCheckers[cell.CellCheckerIndex].Owner == checker.Owner)
            {
                return false;
            }

            cntBetween++;
        }

        if (cntBetween > 1)
        {
            return false;
        }
            
        return true;
    }
    
    private bool CheckCrownKillMove(int fromCellIndex, int toCellIndex)
    {
        if (Cells[toCellIndex].CellCheckerIndex != -1)
        {
            return false;
        }
            
        var checker = AllCheckers[Cells[fromCellIndex].CellCheckerIndex];
            
        var deltaRow = Cells[toCellIndex].Row - Cells[fromCellIndex].Row;
        var deltaCol = Cells[toCellIndex].Col - Cells[fromCellIndex].Col;

        if (Math.Abs(deltaRow) != Math.Abs(deltaCol))
        {
            return false;
        }

        var dRow = deltaRow / Math.Abs(deltaRow);
        var dCol = deltaCol / Math.Abs(deltaCol);
            
        var moveLength = Math.Abs(deltaRow);

        var cntBetween = 0;

        for (var length = 1; length < moveLength; ++length)
        {
            var cellRow = checker.Row + length * dRow;
            var cellCol = checker.Col + length * dCol;

            var cellIndex = cellRow * 8 + cellCol;

            var cell = Cells[cellIndex];

            if (cell.CellCheckerIndex == -1)
            {
                continue;
            }

            if (AllCheckers[cell.CellCheckerIndex].Owner == checker.Owner)
            {
                return false;
            }

            cntBetween++;
        }

        return cntBetween >= 1;
    }

        
    private int GetCheckerKillCellIndex(int fromCellIndex, int toCellIndex)
    {
        var checker = AllCheckers[Cells[fromCellIndex].CellCheckerIndex];
            
        var deltaRow = Cells[toCellIndex].Row - Cells[fromCellIndex].Row;
        var deltaCol = Cells[toCellIndex].Col - Cells[fromCellIndex].Col;

        var killCellRow = checker.Row + deltaRow / 2;
        var killCellCol = checker.Col + deltaCol / 2;

        return killCellRow * 8 + killCellCol;
    }

    private int[] GetCrownKillCellIndexes(int fromCellIndex, int toCellIndex)
    {
        var checker = AllCheckers[Cells[fromCellIndex].CellCheckerIndex];
            
        var deltaRow = Cells[toCellIndex].Row - Cells[fromCellIndex].Row;
        var deltaCol = Cells[toCellIndex].Col - Cells[fromCellIndex].Col;

        if (Math.Abs(deltaRow) != Math.Abs(deltaCol))
        {
            throw new IOException("Error");
        }

        var dRow = deltaRow / Math.Abs(deltaRow);
        var dCol = deltaCol / Math.Abs(deltaCol);
            
        var moveLength = Math.Abs(deltaRow);

        var cellIndexes = new int[] { };

        for (var length = 1; length < moveLength; ++length)
        {
            var cellRow = checker.Row + length * dRow;
            var cellCol = checker.Col + length * dCol;

            var cellIndex = cellRow * 8 + cellCol;

            var cell = Cells[cellIndex];

            if (cell.CellCheckerIndex == -1)
            {
                continue;
            }

            if (AllCheckers[cell.CellCheckerIndex].Owner == checker.Owner)
            {
                break;
            }

            cellIndexes = cellIndexes.Append(cellIndex).ToArray();
        }

        return cellIndexes;
    }

    private MoveState MakeMove(int fromCellIndex, int toCellIndex)
    {
        var checker = AllCheckers[Cells[fromCellIndex].CellCheckerIndex];
            
        if (checker.Type == Checker.CheckerType.Checker &&
            !CheckCheckerMove(fromCellIndex, toCellIndex) &&
            !CheckCheckerKillMove(fromCellIndex, toCellIndex))
        {
            MessageBox.Text = "Вы не можете сделать такой ход!";
            MessageBox.Show();
            
            return MoveState.Incorrect;
        }
            
        if (checker.Type == Checker.CheckerType.Crown &&
            !CheckCrownMove(fromCellIndex, toCellIndex) &&
            !CheckCrownKillMove(fromCellIndex, toCellIndex))
        {
            MessageBox.Text = "Вы не можете сделать такой ход!";
            MessageBox.Show();
            
            return MoveState.Incorrect;
        }

        var killed = false;

        if (checker.Type == Checker.CheckerType.Checker &&
            CheckCheckerKillMove(fromCellIndex, toCellIndex))
        {
            var killCellIndex = GetCheckerKillCellIndex(fromCellIndex, toCellIndex);
                
            KillChecker(killCellIndex);

            killed = true;
            MustMakeKill = false;
        }
        else if (checker.Type == Checker.CheckerType.Crown &&
                 CheckCrownKillMove(fromCellIndex, toCellIndex))
        {
            var killCellIndexes = GetCrownKillCellIndexes(fromCellIndex, toCellIndex);

            foreach (var killCellIndex in killCellIndexes)
            {
                KillChecker(killCellIndex);                
            }
                
            killed = true;
            MustMakeKill = false;
        }
        else if (MustMakeKill)
        {
            MessageBox.Text = "Вы должны срубить еще одну шашку соперника!";
            MessageBox.Show();
            
            return MoveState.Incorrect;
        }

        if (CheckPlayerCanKillMove(CurrentPlayer) && !killed)
        {
            MessageBox.Text = "У Вас есть возможность срубить шашку соперника!";
            MessageBox.Show();
            
            return MoveState.Incorrect;
        }
            
        PutChecker(fromCellIndex, toCellIndex);
        RemoveChecker(fromCellIndex);
            
        MakePossibleCellsUnClickable();

        if (killed &&
            AllCheckers[Cells[toCellIndex].CellCheckerIndex].Type == Checker.CheckerType.Checker &&
            CheckCheckerCanKillMove(Cells[toCellIndex].CellCheckerIndex))
        {   
            MessageBox.Hide();
            
            CellClickHandler(Cells[toCellIndex], null);
            
            return MoveState.Continue;
        }
        
        if (killed &&
            AllCheckers[Cells[toCellIndex].CellCheckerIndex].Type == Checker.CheckerType.Crown &&
            CheckCrownCanKillMove(Cells[toCellIndex].CellCheckerIndex))
        {   
            MessageBox.Hide();
            
            CellClickHandler(Cells[toCellIndex], null);
            
            return MoveState.Continue;
        }

        MakePlayersCellsUnClickable(CurrentPlayer);
        CurrentPlayer = CurrentPlayer == Players.Player1 ? Players.Player2 : Players.Player1;
        MakePlayersCellsClickable(CurrentPlayer);

        if (Player1.AliveCheckers == 0)
        {
            return MoveState.PLayer2Win;
        }

        if (Player2.AliveCheckers == 0)
        {
            return MoveState.Player1Win;
        }

        if (!CheckPlayerCanMove(CurrentPlayer))
        {
            EndGame(Players.Jesus);
        }

        MessageBox.Hide();
        
        return killed ? MoveState.Kill : MoveState.Correct;
    }

    private void EndGame(Players winPlayer)
    {
        if (winPlayer == Players.Jesus)
        {
            MessageBox.Text = "Ничья!";
        }
        else
        {
            MessageBox.Text = string.Concat(winPlayer == Players.Player1 ? Player1.Nickname : Player2.Nickname, " выиграл!");
        }
        
        MessageBox.Show();
        ReturnToMenuButton.Show();
    }

    private void ReturnToMainMenuButton_Click(object? sender, EventArgs eventArgs)
    {
        Hide();
        (new MainMenuForm()).Show();
    }
}