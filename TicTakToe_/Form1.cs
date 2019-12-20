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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //297
            if(panel1.Width == 297)
            {
                splitContainer1.SplitterDistance = 22;
                gameGrid1.Left = (splitContainer1.Panel2.Width - gameGrid1.Width) / 2;
                button1.Text = ">";
            }
            else
            {
                splitContainer1.SplitterDistance = 297;
                gameGrid1.Left = (splitContainer1.Panel2.Width - gameGrid1.Width) / 2;
                button1.Text = "<";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Restart")
            {
                gameGrid1.StartGame(!checkBox1.Checked);
                button2.Text = "Start Game";
            }
            else
            {
                gameGrid1.StartGame(!checkBox1.Checked);
                button2.Text = "Restart";
            }
            
        }
    }
}
