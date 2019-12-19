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

        private Random r = new Random();

        private int plays = 0;
        public bool compPlayFirst = true;

        int lastPlayedGrid = -1;

        public GameGrid()
        {
            InitializeComponent();

            //Initalize play grids
            InitalizeGrids();
            ClearGrid();
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
                gridItem.Text = "";
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


        JustPlay:;
            if (playGridIndex != -1)
            {
                compPlayedGrids.Add(playGridIndex);
                Play(playGridIndex);
                
            }

            
        }

        private int GetWinningGridIndex(string sym)
        {
            int output = -1;

            if (sym != "X" && sym != "O")
                return -1;

            if(grid1.Text == grid3.Text && grid3.Text == sym && grid2.Text == "")
                return 2;
            else if (grid4.Text == grid6.Text && grid6.Text == sym && grid5.Text == "")
                return 5;
            else if (grid7.Text == grid9.Text && grid9.Text == sym && grid7.Text == "")
                return 8;

            else if (grid1.Text == grid7.Text && grid7.Text == sym && grid4.Text == "")
                return 4;
            else if (grid2.Text == grid8.Text && grid8.Text == sym && grid5.Text == "")
                return 5;
            else if (grid3.Text == grid9.Text && grid9.Text == sym && grid6.Text == "")
                return 6;

            else if (grid1.Text == grid9.Text && grid9.Text == sym && grid5.Text == "")
                return 5;
            else if (grid3.Text == grid7.Text && grid7.Text == sym && grid5.Text == "")
                return 5;

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

        private void GameGridClick(object o, EventArgs e)
        {
            Button gridItem = (Button)o;

            if (gridItem.Text != "") return;

            gridItem.Text = currentSymbol;

            currentSymbol = oppositeSymbol;

            plays++;

            lastPlayedGrid = Convert.ToInt32(gridItem.Name.Trim("grid".ToCharArray()));
            if (e != null)
            {
                CompPlay();
            }
        }

    }
}
