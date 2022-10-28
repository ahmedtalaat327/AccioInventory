using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccioInventory.UIViews
{
    public partial class LoginView : UserControl
    {
        public static bool verified = false;
        public static bool admin = false;
        private Form parentForm = null;
        public LoginView(Form parent)
        {
            InitializeComponent();

            this.label1.Image = new Bitmap(global::AccioInventory.Properties.Resources.acció_400x400_1, this.label1.Size);


            parentForm = parent;
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "Ahmed" && this.textBox2.Text == "1234")
            {
                verified = true;
                if (this.radioButton1.Checked)
                {
                    admin = true;
                    parentForm.Visible = true;
                }
                else
                {
                    parentForm.Visible = true;
                }
            }

           


        }
    }
}
