using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccioInventory
{
    public partial class Accio : Form
    {
        public Accio()
        {
            InitializeComponent();

            this.Controls.Add(SetMyFooter());


            this.Resize += (s, e) => {

                this.tableLayoutPanel1.Location = new Point(3, 27);
                this.tableLayoutPanel1.Size = new System.Drawing.Size(this.Size.Width-22, this.Size.Height-84);


            };

                this.label1.Image = new Bitmap(global::AccioInventory.Properties.Resources.acció_400x400_1, this.label1.Size);

        }

        private Control SetMyFooter()
        {
            Panel footParent = new Panel { BackColor = Color.LightGray, Size = new Size(this.Width, 60), Location = new Point(0, this.Height - 60) };
            this.Resize += (s, e) => {
                footParent.Location = new Point(0, this.Height - 60);
                footParent.Size = new Size(this.Width, 60);
            };
            System.Windows.Forms.Label inf = new System.Windows.Forms.Label { Size = footParent.Size, Location = new Point(5,4), Text = "This project mainly developed by Eng.Ahmed T -- Haam Limited Co. 2022." };
            footParent.Controls.Add(inf);
                return footParent;
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CloseCancel(e);
        }

        public static void CloseCancel(FormClosingEventArgs e)
        {
            const string message = "Are you sure that you would like to logout and exit the SW ?";
            const string caption = "Termination!";
            var result = MessageBox.Show(message, caption,
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question);

            e.Cancel = (result == DialogResult.No);

        }

        
    }
}
