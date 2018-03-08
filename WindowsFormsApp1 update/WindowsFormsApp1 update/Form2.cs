using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1_update
{
    public partial class Form2 : Form
    {
        public String textString = "1";
        public Form2()
        {
            InitializeComponent();
        }
        
        private void detailShow_TextChanged(object sender, EventArgs e)
        {

        }

        public void showDetialShow()
        {
            detailShow.Text = textString;
        }
    }
}
