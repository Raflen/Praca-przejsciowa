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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        eNTU model = new eNTU();
        Form2 otherForm = new Form2();

        #region textBox_KeyPress

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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',')
            {

            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',')
            {

            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',')
            {

            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',')
            {

            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',')
            {

            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',')
            {

            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',')
            {

            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        #endregion

        #region buttons

        private void button1_Click(object sender, EventArgs e)
        {
            if (setValues())
            {
                if (check())
                {
                    display();
                }
            }
                        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = listBox1.GetItemText(listBox1.SelectedItem);
            int thermal_conductivity = int.Parse(text.Split(' ').Last());
            model.thermal_conductivity = thermal_conductivity;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            otherForm.ShowDialog();
            try
            {
                int thermal_conductivity = int.Parse(otherForm.value);
                model.thermal_conductivity = thermal_conductivity;
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Wpisano niepoprawną wartość.");
            }
        }
        #endregion

        #region methods

        private bool setValues()
        {
            bool flag = true;
            try
            {
                model.v_c = double.Parse(textBox1.Text);
                model.t_ci = double.Parse(textBox2.Text);
                model.t_hi = double.Parse(textBox3.Text);
                model.v_h = double.Parse(textBox4.Text);
                model.exchanger_height = double.Parse(textBox5.Text);
                model.exchanger_diam = double.Parse(textBox6.Text);
                model.pipe_thickness = double.Parse(textBox7.Text) / 1000;
                model.pipe_OD = double.Parse(textBox8.Text) / 1000;
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Nie podano wszyskich wartości początkowych.");
                flag = false;              
            }           

            return flag;
        }

        private bool check()
        {
            bool flag = true;

            if (model.thermal_conductivity != 0)
            {
                model.calculations();
            }
            else
            {
                MessageBox.Show("Nie wybrano materiału.");
                flag = false;    
            }

            return flag;
        }

        private void display()
        {
            textBox9.Text = Math.Round(model.q, 2).ToString();
            textBox10.Text = Math.Round(model.t_ce, 2).ToString();
            textBox11.Text = Math.Round(model.t_he, 2).ToString();
        }

        #endregion

        
    }
}
