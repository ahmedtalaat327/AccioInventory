using AccioInventory.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AccioInventory.UIViews
{
    public partial class ConfigView : UserControl
    {
        private string VersionNo { get; set; } = "0.0.0";
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

            ReadPARAMS();

            var photo_path = AccioEasyHelpers.MeExistanceLocation().Substring(0, AccioEasyHelpers.MeExistanceLocation().Length - ("AccioInventory.exe").Length) + "data\\companylogo.png";
            this.label7.Image = new Bitmap(Image.FromFile(photo_path),this.label7.Width,this.label7.Height);

        }
        private void ReadPARAMS()
        {
            //read params from config
            var data = AccioEasyHelpers.ReadTxTFiles(AccioEasyHelpers.MeExistanceLocation().Substring(0, AccioEasyHelpers.MeExistanceLocation().Length - ("AccioInventory.exe").Length) + "data\\params.info");

            var server_adress = AccioEasyHelpers.GetTxTBettwen(data[4], "::", ",");
            var port = AccioEasyHelpers.GetTxTBettwen(data[5], "::", ",");
            var comp = AccioEasyHelpers.GetTxTBettwen(data[6], "::", ",");
            VersionNo = AccioEasyHelpers.GetTxTBettwen(data[7], "::", ",");

            this.textBox1.Text = server_adress;
            this.textBox2.Text = "st*** => only developer can change it contact your seller!";
            this.textBox3.Text = "st***";
            this.textBox4.Text = port;
            this.textBox5.Text = comp;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a an ADMIN page..", "Accio v1.1");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> allData = new List<string>();
            allData.Add(" \n");
            allData.Add("**this file is to initialize the current database connnection to make app connect to its server**\n");
            allData.Add("**don't try to move lines down or up this will miss the whole file just change data values**\n[\n");
            allData.Add($"#server_ip::{this.textBox1.Text},\n");
            allData.Add($"#port::{this.textBox4.Text},\n");
            allData.Add($"#company_name::{this.textBox5.Text},\n");
            allData.Add($"#version::{this.VersionNo},\n]");

            string oneLine = "";
            foreach(string item in allData)
            {
                oneLine += item;
            }

            try
            {

                File.WriteAllText(AccioEasyHelpers.MeExistanceLocation().Substring(0, AccioEasyHelpers.MeExistanceLocation().Length - ("AccioInventory.exe").Length) + "data\\params.info"
                    , oneLine);
                MessageBox.Show("New Configurations has been successfully saved.", "Done");

            }
            catch (Exception)
            {
                MessageBox.Show("Params File not Found!", "Error");
            }


        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            ReadPARAMS();
            MessageBox.Show("Paramas..Loaded..", "Updated");
        }
    }
}
