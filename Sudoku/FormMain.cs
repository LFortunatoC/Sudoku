using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class FormMain : Form
    {
        Button[,] buttonArray = new Button[9, 9];
        Sudoku mySudoku = new Sudoku();

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int horizontal = 10;
            int vertical = 20;
           
            Color btnColor = new Color();
            btnColor= Color.WhiteSmoke;
            for(int row = 0; row < 9; row++ )
            {
                for (int column = 0; column < 9; column++)
                {
                    buttonArray[row,column] = new Button();
                    buttonArray[row, column].FlatStyle = FlatStyle.Popup;
                    buttonArray[row,column].Size = new Size(40, 40);
                    buttonArray[row,column].Location = new Point(horizontal, vertical);
                    buttonArray[row,column].BackColor = btnColor;
                    buttonArray[row, column].Tag = row + "," + column;
                    buttonArray[row, column].Enabled = false;
                    buttonArray[row, column].Click += Buttons_Click;
                   
                    if ((column == 2) || column == 5)
                    {
                        if (btnColor == Color.WhiteSmoke)
                            btnColor = Color.Silver;
                        else
                            btnColor = Color.WhiteSmoke;
                    }
                    horizontal += 40;//vertical += 40;
                    this.Controls.Add(buttonArray[row, column]);
                }
                horizontal = 10;
                vertical += 40;
                if ((row == 2) || row == 5)
                {
                    if (btnColor == Color.WhiteSmoke)
                        btnColor = Color.Silver;
                    else
                        btnColor = Color.WhiteSmoke;
                }

            }

            comboBoxLevel.SelectedIndex = 0;
        }

        private void Buttons_Click(object sender, EventArgs e)
        {

            Button btn = sender as Button;
            string[] indexes = btn.Tag.ToString().Split(',');
            int row = Convert.ToInt32(btn.Tag.ToString().Substring(0, 1));
            int column = Convert.ToInt32(btn.Tag.ToString().Substring(2, 1));
            //in indexes[0] you've got the i index and in indexes[1] the j index
            Console.WriteLine(indexes[0] + "," + indexes[1]);
            List<int> lstPossibleNums = new List<int>();
            lstPossibleNums= mySudoku.GetPossibleNums(row, column);
            frmNumbers frmNumber = new frmNumbers(lstPossibleNums);
            frmNumber.StartPosition = FormStartPosition.Manual;
            frmNumber.Location = new Point(this.Location.X + buttonArray[row, column].Location.X, this.Location.Y + buttonArray[row, column].Location.Y);

            var result = frmNumber.ShowDialog();
            if (result == DialogResult.OK)
            {
                string val = frmNumber.ReturnValue;            //values preserved after close

                if (val == "X")
                {
                    buttonArray[row, column].Text = "";
                    mySudoku.EnterValue(0, row, column);
                }
                else
                {
                    buttonArray[row, column].Text = val;
                    mySudoku.EnterValue(Convert.ToInt32(val), row, column);
                    if(mySudoku.IsGameFinished()==true)
                    {
                        MessageBox.Show("You did it! \n You beat Sudoku", "Congrats!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        PrepareNewGame();
                    }
                }
            }
            frmNumber.Dispose();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            int gameLevel = comboBoxLevel.SelectedIndex>=0? comboBoxLevel.SelectedIndex:0;
            mySudoku.GenerateGame(gameLevel);
            LoadGame(mySudoku);
            buttonRestart.Enabled = true;
            buttonSolve.Enabled = true;
        }

        private void LoadGame(Sudoku sudoku)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    buttonArray[row, col].Text = sudoku.GameSet[row, col].ToString();
                    if (buttonArray[row, col].Text != "0")
                    {
                        buttonArray[row, col].Font= new Font(buttonArray[row, col].Font, FontStyle.Bold);
                        buttonArray[row, col].ForeColor = Color.DarkBlue;
                        buttonArray[row, col].Enabled = false;
                    }
                    else
                    {
                        buttonArray[row, col].Text = "";
                        buttonArray[row, col].Font = new Font(buttonArray[row, col].Font, FontStyle.Regular);
                        buttonArray[row, col].ForeColor = Color.Gray;
                        buttonArray[row, col].Enabled = true;
                    }
                }
            }
        }

        private void Solve(Sudoku sudoku)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    buttonArray[row, col].Text = sudoku.GameAnswer[row, col].ToString();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Solve(mySudoku);
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            LoadGame(mySudoku);
        }

        private void PrepareNewGame()
        {
            for(int row=0;row<9;row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    buttonArray[row, col].Text = "";
                    buttonArray[row, col].Enabled = false;
                }
            }
            buttonSolve.Enabled = false;
            buttonRestart.Enabled = false;
        }
    }
}