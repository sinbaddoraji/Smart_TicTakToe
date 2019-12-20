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
        private string oppositeSymbol => currentSymbol == "X" ? "O" : "X";

        public int CurrentPlayer => currentSymbol == "X" ? 1 : 0;

        public delegate void GridEvent(string curPlayer);
        public GridEvent GridClicked = delegate { };

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

        private Random r = new Random();

        private int plays = 0;
        public bool compPlayFirst = false;

        private string player1Name = "Computer";
        private string player2Name = "player";

        private int lastPlayedGrid = -1;

        public GameGrid()
        {
            InitializeComponent();

            //Initalize play grids
            InitalizeGrids();
            ClearGrid();
            LockGrids(true);
        }


        public void StartGame(bool pcPlayFirst)
        {
            currentSymbol = "X";

            ClearGrid();
            LockGrids(false);

            compPlayFirst = pcPlayFirst;
            if (compPlayFirst)
                CompPlay();
        }

        public void SetPlayerName(string name)
        {
            player2Name = name;
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
            grid1.Enabled = !doLock;
            grid2.Enabled = !doLock;
            grid3.Enabled = !doLock;
            grid4.Enabled = !doLock;
            grid5.Enabled = !doLock;
            grid6.Enabled = !doLock;
            grid7.Enabled = !doLock;
            grid8.Enabled = !doLock;
            grid9.Enabled = !doLock;
        }

        List<int> compPlayedGrids = new List<int>();

        public void CompPlay()
        {
            int playGridIndex = - 1;
            if (compPlayFirst)
            {
                if(compPlayedGrids.Count == 0)
                {
                    int[] corners = new[] { 1, 3, 7, 9 };
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
                        int[] corners = new[] { 1, 3, 7, 9 };
                        playGridIndex = corners[r.Next(0, 4)];
                        goto JustPlay;
                    }
                }
                else if (compPlayedGrids.Count == 1)
                {
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
                
            }

            
        }

        private bool Equ(string a, string b, string c)
        {
            return a == b && b == c && c != "";
        }

        public bool Won()
        {
            return Equ(grid1.Text, grid2.Text, grid3.Text)
                || Equ(grid4.Text, grid5.Text, grid6.Text)
                || Equ(grid7.Text, grid8.Text, grid9.Text)

                || Equ(grid1.Text, grid4.Text, grid7.Text)
                || Equ(grid2.Text, grid5.Text, grid8.Text)
                || Equ(grid3.Text, grid6.Text, grid9.Text)

                || Equ(grid1.Text, grid5.Text, grid9.Text)
                || Equ(grid3.Text, grid5.Text, grid7.Text);

        }

        private bool numSym(string a, string b, string c, string sym)
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

            if(numSym(grid1.Text, grid2.Text, grid3.Text, sym))
            {
                if (grid1.Text == "") return 1;
                else if (grid2.Text == "") return 2;
                else if (grid3.Text == "") return 3;
            }
            else if (numSym(grid4.Text, grid5.Text, grid6.Text, sym))
            {
                if (grid4.Text == "") return 4;
                else if (grid5.Text == "") return 5;
                else if (grid6.Text == "") return 6;
            }
            else if (numSym(grid7.Text, grid8.Text, grid9.Text, sym))
            {
                if (grid7.Text == "") return 7;
                else if (grid8.Text == "") return 8;
                else if (grid9.Text == "") return 9;
            }
            else if (numSym(grid1.Text, grid4.Text, grid7.Text, sym))
            {
                if (grid1.Text == "") return 1;
                else if (grid4.Text == "") return 4;
                else if (grid7.Text == "") return 7;
            }
            else if (numSym(grid2.Text, grid5.Text, grid8.Text, sym))
            {
                if (grid2.Text == "") return 2;
                else if (grid5.Text == "") return 5;
                else if (grid8.Text == "") return 8;
            }
            else if (numSym(grid3.Text, grid6.Text, grid9.Text, sym))
            {
                if (grid3.Text == "") return 3;
                else if (grid6.Text == "") return 6;
                else if (grid9.Text == "") return 9;
            }
            else if (numSym(grid1.Text, grid5.Text, grid9.Text, sym))
            {
                if (grid1.Text == "") return 1;
                else if (grid5.Text == "") return 5;
                else if (grid9.Text == "") return 9;
            }
            else if (numSym(grid3.Text, grid5.Text, grid7.Text, sym))
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
                if (grid.Text == "") return Convert.ToInt32(grid.Name.Trim("grid".ToCharArray()));
            }
            return -1;
        }

        private bool Play(int x)
        {
            EventArgs emptyEvent = null;
            switch(x)
            {
                case 1:
                    if(grid1.Text != "")
                        return false;
                    GameGridClick(grid1, emptyEvent);
                    return true;
                case 2:
                    if (grid2.Text != "")
                        return false;
                    GameGridClick(grid2, emptyEvent);
                    return true;
                case 3:
                    if (grid3.Text != "")
                        return false;
                    GameGridClick(grid3, emptyEvent);
                    return true;
                case 4:
                    if (grid4.Text != "")
                        return false;
                    GameGridClick(grid4, emptyEvent);
                    return true;
                case 5:
                    if (grid5.Text != "")
                        return false;
                    GameGridClick(grid5, emptyEvent);
                    return true;
                case 6:
                    if (grid6.Text != "")
                        return false;
                    GameGridClick(grid6, emptyEvent);
                    return true;
                case 7:
                    if (grid7.Text != "")
                        return false;
                    GameGridClick(grid7, emptyEvent);
                    return true;
                case 8:
                    if (grid8.Text != "")
                        return false;
                    GameGridClick(grid8, emptyEvent);
                    return true;
                case 9:
                    if (grid9.Text != "")
                        return false;
                    GameGridClick(grid9, emptyEvent);
                    return true;
            }
            return false; //False -> grod has been played on
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
                grid3.Image = grid5.Image = grid7.Image = Properties.Resources.v;
        }

        private void GameGridClick(object o, EventArgs e)
        {
            GridClicked(CurrentPlayerStr);

            Button gridItem = (Button)o;

            if (gridItem.Text != "" || Won() || GetNextRandomPlayableGrid() == -1) return;

            gridItem.Text = currentSymbol;

            currentSymbol = oppositeSymbol;

            plays++;

            lastPlayedGrid = Convert.ToInt32(gridItem.Name.Trim("grid".ToCharArray()));
            if (e != null)
            {
                CompPlay();
            }
            

            if (Won())
            {
                LockGrids(true);
                DrawWinningLine();

                string winner;
                int theWinner = CurrentPlayer == 1 ? 0 : 1;

                if (theWinner == 1 && compPlayFirst)
                {
                    winner = player1Name;
                }
                else if (theWinner == 0 && !compPlayFirst)
                {
                    winner = player1Name;
                }
                else
                {
                    winner = player2Name;
                }

                MessageBox.Show($"{winner} won!");
            }
            else if (GetNextRandomPlayableGrid() == -1)
            {
                LockGrids(true);
                MessageBox.Show("Draw!");
            }
            
        }

    }
}
