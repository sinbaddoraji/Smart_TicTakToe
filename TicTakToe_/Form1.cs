using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTakToe_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameGrid1.GridClicked += GridClicked;
        }

        void GridClicked(string curPlayer)
        {
            Text = $"{curPlayer}'s turn";

            label1.Text = $"{gameGrid1.player1Name}:  {gameGrid1.player1Score}";
            label2.Text = $"{gameGrid1.player2Name}:  {gameGrid1.player2Score}";
            label4.Text = $"Draws:  {gameGrid1.draws}";

            if (gameGrid1.Won() || gameGrid1.Draw())
            {
                ExpandPanel();
                button2.Text = "Start Game";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(panel1.Width == 297)
            {
                FoldPanel();
            }
            else
            {
                ExpandPanel();
            }
        }

        private void ExpandPanel()
        {
            splitContainer1.SplitterDistance = 297;
            gameGrid1.Left = (splitContainer1.Panel2.Width - gameGrid1.Width) / 2;
            button1.Text = "<";
            panel2.Show();
            panel3.Show();
        }

        private void FoldPanel()
        {
            splitContainer1.SplitterDistance = 22;
            gameGrid1.Left = (splitContainer1.Panel2.Width - gameGrid1.Width) / 2;
            button1.Text = ">";
            panel2.Hide();
            panel3.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                gameGrid1.StartGame(!checkBox1.Checked);
            else gameGrid1.StartGame();

            button2.Text = "Restart";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Visible = radioButton1.Checked;

            gameGrid1.player1Score = 0;
            gameGrid1.player2Score = 0;
        }
    }
}
