using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        internal static DateTime currentSelection;

        internal void button1_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value.Date;
            currentSelection = selectedDate;
            MessageBox.Show("Дата выбрана:"+selectedDate.ToString());
            this.Close();
            
        }

         
    }
}
