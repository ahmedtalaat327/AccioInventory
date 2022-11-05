using AccioInventory.Helpers;
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
    public partial class ConfigView : UserControl
    {
        public ConfigView(Control parentPanel)
        {
            InitializeComponent();

            this.Size = new System.Drawing.Size(parentPanel.Size.Width, parentPanel.Size.Height);

            this.AutoSize = true;
            this.Dock = System.Windows.Forms.DockStyle.Fill;

             
            this.TabIndex = 2;

            this.Resize += (s, e) => {

                
            this.Size = new System.Drawing.Size(parentPanel.Size.Width, parentPanel.Size.Height);
                this.Refresh();

            };

            //read params from config
            var data = AccioEasyHelpers.ReadTxTFiles(AccioEasyHelpers.MeExistanceLocation().Substring(0, AccioEasyHelpers.MeExistanceLocation().Length - ("AccioInventory.exe").Length) + "data\\params.info");

            var server_adress = AccioEasyHelpers.GetTxTBettwen(data[4], "::", ",");
            var port = AccioEasyHelpers.GetTxTBettwen(data[5], "::", ",");

            this.textBox1.Text = server_adress;
            this.textBox2.Text = "store";
            this.textBox3.Text = "store";
            this.textBox4.Text = port;

        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a an ADMIN page..", "Accio v1.1");
        }
    }
}
