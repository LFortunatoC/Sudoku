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
    public partial class frmNumbers : Form
    {
        List<int> lstshowBtns = new List<int>();
        Button[] buttonArray = new Button[9];
        public string ReturnValue { get; set; }

        public frmNumbers(List<int> lstButtons)
        {
            lstshowBtns = lstButtons;
            InitializeComponent();

        }

 
        public void EnteredValue(string value)
        {
            this.ReturnValue = value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmNumbers_Load(object sender, EventArgs e)
        {
            const int btnSize = 20;
            const int padding = 5;

            int horizontal = padding;
            int vertical = padding;
            int size_h =0;
            int size_v= 0;
            
            Color btnColor = new Color();
            btnColor = Color.LightSeaGreen;
            int count = 0;
            foreach (int num in lstshowBtns)
            {
                buttonArray[count] = new Button();
                buttonArray[count].FlatStyle = FlatStyle.Popup;
                buttonArray[count].Size = new Size(btnSize, btnSize);
                buttonArray[count].Location = new Point(horizontal, vertical);
                buttonArray[count].BackColor = btnColor;
                buttonArray[count].Text = num.ToString();
                buttonArray[count].Click += Buttons_Click;
                horizontal += btnSize;
                this.Controls.Add(buttonArray[count]);
                count++;
                if (horizontal > (btnSize*5))
                {
                    vertical = btnSize + padding;
                    horizontal = padding;
                    size_h = (btnSize*5)+ padding;
                }
                else if(size_h < ((btnSize * 5) + padding)) { size_h += btnSize; }
            }
            size_v= vertical+btnSize+padding;
            size_h += padding;
            buttonArray[count] = new Button();
            buttonArray[count].FlatStyle = FlatStyle.Popup;
            buttonArray[count].Size = new Size(btnSize, btnSize);
            buttonArray[count].Location = new Point(horizontal, vertical);
            buttonArray[count].BackColor = btnColor;
            buttonArray[count].Text = "X";
            buttonArray[count].Click += Buttons_Click;
            if(count< 5)
            {
                size_h += btnSize+padding;
            }
            this.Controls.Add(buttonArray[count]);
            this.Size = new Size(size_h, size_v);
        }

        private void Buttons_Click(object sender, EventArgs e)
        {

            Button btn = sender as Button;
            EnteredValue(btn.Text);
        }
    }
}
