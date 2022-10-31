using AccioInventory.DBConnection;
using AccioInventory.Helpers;
using Oracle.ManagedDataAccess.Client;
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

        private string key { get; set; } = "null";
        

        private System.Windows.Forms.Timer myTimer_showReaction = new System.Windows.Forms.Timer();

        public LoginView(Form parent)
        {
            InitializeComponent();

            this.label1.Image = new Bitmap(global::AccioInventory.Properties.Resources.acció_400x400_1, this.label1.Size);


            parentForm = parent;

            //test oracle db connection
            if (Scripts.TestConnection(new[] { "127.0.0.1", "1521", "store", "store" },true) == null)
                Environment.Exit(0);
        

         }

        private void button1_Click(object sender, EventArgs e)
        {
            var myOpenedTunnel = Scripts.TestConnection(new[] { "127.0.0.1", "1521", "store", "store" });
            var fetchedData = Scripts.FetchMyData(myOpenedTunnel, "users", new string[] { "user_name", "user_password" }, new string[] {"user_id"},new string[] {"999"},"!=","and");

            while (fetchedData.Read())
            {
                if (this.textBox1.Text == fetchedData["user_name"].ToString() && this.textBox2.Text == fetchedData["user_password"].ToString())
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
                    myOpenedTunnel.Close();
                    myOpenedTunnel.Dispose();

                    var userNameLabel = (Label)AccioEasyHelpers.GetControlByName(parentForm, "holderUser");
                    userNameLabel.Text = "Logged as: " + this.textBox1.Text;
                    userNameLabel.ForeColor = Color.Gray;
                    break;
                }
                else
                {
                    this.label6.Text = "Faild to login";
                    this.label6.ForeColor = Color.Red;

                    myTimer_showReaction.Tick += new EventHandler((ob, ev) =>
                    {


                        if (this.label6.Text.StartsWith("Fail"))
                        {
                            // Restarts the timer and increments the counter.


                            this.label6.Text = "...";
                            this.label6.ForeColor = Color.Black;
                            myTimer_showReaction.Stop();
                        }

                    });

                    // Sets the timer interval to 5 seconds.
                    myTimer_showReaction.Interval = 5000;
                    myTimer_showReaction.Start();
                }
            }

           


        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //hide all pass text
          


        }
    }
}
