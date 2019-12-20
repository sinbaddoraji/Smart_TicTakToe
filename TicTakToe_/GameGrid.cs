using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTakToe_
{
    public partial class GameGrid : UserControl
    {
        private string currentSymbol = "X";
        private string OppositeSymbol => currentSymbol == "X" ? "O" : "X";

        public int CurrentPlayer => currentSymbol == "X" ? 1 : 0;

        public delegate void GridEvent(string curPlayer);
        public GridEvent GridClicked = delegate { };

        private readonly Random r = new Random();

        public bool compPlayFirst = false;

        public string player1Name = "Computer";
        public string player2Name = "player";

        public int player1Score = 0;
        public int player2Score = 0;
        public int draws = 0;

        private int lastPlayedGrid = -1;

        readonly List<int> compPlayedGrids = new List<int>();
        readonly int[] corners = new[] { 1, 3, 7, 9 };

        private string CurrentPlayerStr
        {
            get
            {
                int thePlayer = CurrentPlayer == 1 ? 0 : 1;

                if ((thePlayer == 1 && compPlayFirst) || (thePlayer == 0 && !compPlayFirst))
                    return player1Name;
                else return player2Name;
            }
        }

        private bool Equ(string a, string b, string c) => a == b && b == c && c != "";

        public bool Draw() => GetNextRandomPlayableGrid() == -1;

        public bool Won() 
                => Equ(grid1.Text, grid2.Text, grid3.Text)
                || Equ(grid4.Text, grid5.Text, grid6.Text)
                || Equ(grid7.Text, grid8.Text, grid9.Text)
                || Equ(grid1.Text, grid4.Text, grid7.Text)
                || Equ(grid2.Text, grid5.Text, grid8.Text)
                || Equ(grid3.Text, grid6.Text, grid9.Text)
                || Equ(grid1.Text, grid5.Text, grid9.Text)
                || Equ(grid3.Text, grid5.Text, grid7.Text);

        

        private bool IsValidForWin(string a, string b, string c, string sym)
        {
            int ad = 0;
            if (a == sym) ad++;
            if (b == sym) ad++;
            if (c == sym) ad++;

            return ad > 1;
        }

        private int GetWinningGridIndex(string sym)
        {
            if (sym != "X" && sym != "O")
                return -1;

            if (IsValidForWin(grid1.Text, grid2.Text, grid3.Text, sym))
            {
                if (grid1.Text == "") return 1;
                else if (grid2.Text == "") return 2;
                else if (grid3.Text == "") return 3;
            }
            else if (IsValidForWin(grid4.Text, grid5.Text, grid6.Text, sym))
            {
                if (grid4.Text == "") return 4;
                else if (grid5.Text == "") return 5;
                else if (grid6.Text == "") return 6;
            }
            else if (IsValidForWin(grid7.Text, grid8.Text, grid9.Text, sym))
            {
                if (grid7.Text == "") return 7;
                else if (grid8.Text == "") return 8;
                else if (grid9.Text == "") return 9;
            }
            else if (IsValidForWin(grid1.Text, grid4.Text, grid7.Text, sym))
            {
                if (grid1.Text == "") return 1;
                else if (grid4.Text == "") return 4;
                else if (grid7.Text == "") return 7;
            }
            else if (IsValidForWin(grid2.Text, grid5.Text, grid8.Text, sym))
            {
                if (grid2.Text == "") return 2;
                else if (grid5.Text == "") return 5;
                else if (grid8.Text == "") return 8;
            }
            else if (IsValidForWin(grid3.Text, grid6.Text, grid9.Text, sym))
            {
                if (grid3.Text == "") return 3;
                else if (grid6.Text == "") return 6;
                else if (grid9.Text == "") return 9;
            }
            else if (IsValidForWin(grid1.Text, grid5.Text, grid9.Text, sym))
            {
                if (grid1.Text == "") return 1;
                else if (grid5.Text == "") return 5;
                else if (grid9.Text == "") return 9;
            }
            else if (IsValidForWin(grid3.Text, grid5.Text, grid7.Text, sym))
            {
                if (grid3.Text == "") return 3;
                else if (grid5.Text == "") return 5;
                else if (grid7.Text == "") return 7;
            }

            return -1;
        }

        private int GetNextRandomPlayableGrid()
        {
            foreach (Button grid in panel1.Controls)
            {
                if (grid.Text == "") 
                    return Convert.ToInt32(grid.Name.Trim("grid".ToCharArray()));
            }

            return -1;
        }

        private void Play(int x)
        {
            //Programmatically click on grid
            EventArgs emptyEvent = null;
                 if (x == 1) GameGridClick(grid1, emptyEvent);
            else if (x == 2) GameGridClick(grid2, emptyEvent);
            else if (x == 3) GameGridClick(grid3, emptyEvent);
            else if (x == 4) GameGridClick(grid4, emptyEvent);
            else if (x == 5) GameGridClick(grid5, emptyEvent);
            else if (x == 6) GameGridClick(grid6, emptyEvent);
            else if (x == 7) GameGridClick(grid7, emptyEvent);
            else if (x == 8) GameGridClick(grid8, emptyEvent);
            else if (x == 9) GameGridClick(grid9, emptyEvent);
        }

        public GameGrid()
        {
            InitializeComponent();

            //Initalize play grids
            InitalizeGrids();
            ClearGrid();
            LockGrids(true);
        }

        public void StartGame()
        {
            player1Name = "player 1";
            player2Name = "player 2";

            currentSymbol = "X";

            ClearGrid();
            LockGrids(false);
        }

        public void StartGame(bool pcPlayFirst)
        {
            player1Name = "Computer";
            player2Name = "player";

            currentSymbol = "X";

            ClearGrid();
            LockGrids(false);

            compPlayFirst = pcPlayFirst;
            if (compPlayFirst)
                CompPlay();
        }

        private void InitalizeGrids()
        {
            //Set all grid texts to empty
            foreach (Button gridItem in panel1.Controls)
                gridItem.Click += GameGridClick;
        }

        private void ClearGrid()
        {
            //Set all grid texts to empty
            foreach (Button gridItem in panel1.Controls)
            {
                gridItem.Text = "";
                gridItem.Image = null;
            }
        }

        public void LockGrids(bool doLock)
        {
            //Enable/Disable grids
            foreach (Button gridItem in panel1.Controls)
                gridItem.Enabled = !doLock;
        }

        

        public void CompPlay()
        {
            int playGridIndex = - 1;
            if (compPlayFirst)
            {
                if(compPlayedGrids.Count == 0)
                {
                    playGridIndex = corners[r.Next(0, 4)];
                    goto JustPlay;
                }
                else if (compPlayedGrids.Count == 1)
                {
                    int lastIndex = compPlayedGrids[0];
                    if (lastPlayedGrid != 5)
                    {
                        if (grid1.Text == "")
                            playGridIndex = 1;
                        else if (grid3.Text == "")
                            playGridIndex = 3;
                        else if (grid7.Text == "")
                            playGridIndex = 7;
                        else if (grid9.Text == "")
                            playGridIndex = 9;
                    }
                    else
                    {
                        if (lastIndex == 7)
                            playGridIndex = 3;
                        else if (lastIndex == 3)
                            playGridIndex = 7;
                        else if (lastIndex == 9)
                            playGridIndex = 1;
                        else if (lastIndex == 1)
                            playGridIndex = 9;
                    }
                }
                else if (compPlayedGrids.Count == 2)
                {
                    playGridIndex = GetWinningGridIndex("X");
                    if (playGridIndex != -1) goto JustPlay;
                    else
                    {
                        if (grid1.Text == "")
                            playGridIndex = 1;
                        else if (grid3.Text == "")
                            playGridIndex = 3;
                        else if (grid7.Text == "")
                            playGridIndex = 7;
                        else if (grid9.Text == "")
                            playGridIndex = 9;
                    }
                }
                else if (compPlayedGrids.Count == 3)
                {
                    playGridIndex = GetWinningGridIndex("X");
                    if (playGridIndex == -1)
                    {
                        //Any random thing will do
                        playGridIndex = GetWinningGridIndex("O");
                        if(playGridIndex == -1)
                        {
                            playGridIndex = GetNextRandomPlayableGrid();
                        }
                    }
                }
                else
                {
                    playGridIndex = GetWinningGridIndex("X");
                    if (playGridIndex != -1)
                    {
                        goto JustPlay;
                    }

                    //Any random thing will do
                    playGridIndex = GetWinningGridIndex("O");
                    if (playGridIndex == -1)
                    {
                        playGridIndex = GetNextRandomPlayableGrid();
                    }
                }

            }
            else
            {
                if (compPlayedGrids.Count == 0)
                {
                    if (lastPlayedGrid != 5)
                        playGridIndex = 5;
                    else
                    {
                        playGridIndex = corners[r.Next(0, 4)];
                        goto JustPlay;
                    }
                }
                else if (compPlayedGrids.Count == 1)
                {
                    playGridIndex = GetWinningGridIndex("O");
                    if (playGridIndex != -1)
                    {
                        goto JustPlay;
                    }

                    playGridIndex = GetWinningGridIndex("X");
                    if (playGridIndex != -1)
                    {
                        goto JustPlay;
                    }

                    if (grid2.Text == "")
                        playGridIndex = 2;
                    else if (grid5.Text == "")
                        playGridIndex = 5;
                    else if (grid8.Text == "")
                        playGridIndex = 8;
                }
                else
                {
                    playGridIndex = GetWinningGridIndex("O");
                    if (playGridIndex != -1)
                    {
                        goto JustPlay;
                    }

                    //Any random thing will do
                    playGridIndex = GetWinningGridIndex("X");
                    if (playGridIndex == -1)
                    {
                        playGridIndex = GetNextRandomPlayableGrid();
                    }
                }
            }

        JustPlay:;
            if (playGridIndex != -1)
            {
                compPlayedGrids.Add(playGridIndex);
                Play(playGridIndex);
                return;
            }
        }

        

        private void DrawWinningLine()
        {
            //Horizontal
            if(Equ(grid1.Text, grid2.Text, grid3.Text))
                grid1.Image = grid2.Image = grid3.Image = Properties.Resources.b;
            else if (Equ(grid4.Text, grid5.Text, grid6.Text))
                grid4.Image = grid5.Image = grid6.Image = Properties.Resources.b;
            else if (Equ(grid7.Text, grid8.Text, grid9.Text))
                grid7.Image = grid8.Image = grid9.Image = Properties.Resources.b;

            //Vertical
            else if (Equ(grid1.Text, grid4.Text, grid7.Text))
                grid1.Image = grid4.Image = grid7.Image = Properties.Resources.c;
            else if (Equ(grid2.Text, grid5.Text, grid8.Text))
                grid2.Image = grid5.Image = grid8.Image = Properties.Resources.c;
            else if (Equ(grid3.Text, grid6.Text, grid9.Text))
                grid3.Image = grid6.Image = grid9.Image = Properties.Resources.c;

            //Diagonal
            else if (Equ(grid1.Text, grid5.Text, grid9.Text))
                grid1.Image = grid5.Image = grid9.Image = Properties.Resources.v;
            else if (Equ(grid3.Text, grid5.Text, grid7.Text))
                grid3.Image = grid5.Image = grid7.Image = Properties.Resources.v2;
        }

        private void GameGridClick(object o, EventArgs e)
        {
            GridClicked(CurrentPlayerStr);

            Button gridItem = (Button)o;

            if (gridItem.Text != "" || Won() || GetNextRandomPlayableGrid() == -1) return;

            gridItem.Text = currentSymbol;

            currentSymbol = OppositeSymbol;

            lastPlayedGrid = Convert.ToInt32(gridItem.Name.Trim("grid".ToCharArray()));
           
            

            if (Won())
            {
                LockGrids(true);
                DrawWinningLine();

                string winner;
                int theWinner = CurrentPlayer == 1 ? 0 : 1;

                if (theWinner == 1 && compPlayFirst)
                {
                    winner = player1Name;
                    player1Score++;
                }
                else if (theWinner == 0 && !compPlayFirst)
                {
                    winner = player1Name;
                    player1Score++;
                }
                else
                {
                    winner = player2Name;
                    player2Score++;
                }
                GridClicked(CurrentPlayerStr);

                compPlayedGrids.Clear();
                MessageBox.Show($"{winner} won!");
                return;
            }
            else if (GetNextRandomPlayableGrid() == -1)
            {
                LockGrids(true);
                compPlayedGrids.Clear();
                MessageBox.Show("Draw!");
                draws++;
                GridClicked(CurrentPlayerStr);
                return;
            }


            if (e != null && player1Name == "Computer")
            {
                CompPlay();
            }
        }

    }
}
