using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VAPW_semstralka_ver2
{
    public partial class CustomDialog : Form
    {
        public bool modelConnection { get { return radioButton1.Checked; }} //true - event, false - timer
        
        public CustomDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
