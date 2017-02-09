using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeatExchangerGUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string value { get; set; }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',')
            {

            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            value = textBox1.Text;
            if (value != null)
            {
                textBox1.Clear();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nie wpisano wartości.");
            }
        }
    }
}
