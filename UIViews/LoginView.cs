using AccioInventory.DBConnection;
using AccioInventory.Helpers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AccioInventory.UIViews
{
    public partial class LoginView : UserControl
    {
        public static bool verified = false;
        public static bool admin = false;
        private static string loginAuth = "user";
        private Form parentForm = null;

        
        

        private System.Windows.Forms.Timer myTimer_showReaction = new System.Windows.Forms.Timer();

        public LoginView(Form parent)
        {
            InitializeComponent();

            this.label1.Image = new Bitmap(global::AccioInventory.Properties.Resources.acció_400x400_1, this.label1.Size);


            parentForm = parent;

            

            //test oracle db connection
            if (AccioEasyHelpers.ReadParamsThenConnectToDB(true) == null)
                Environment.Exit(0);


            //read params from config
            var data = AccioEasyHelpers.ReadTxTFiles(AccioEasyHelpers.MeExistanceLocation().Substring(0, AccioEasyHelpers.MeExistanceLocation().Length - ("AccioInventory.exe").Length) + "data\\params.info");

            var cpm_name = AccioEasyHelpers.GetTxTBettwen(data[6], "::", ",");

            this.label5.Text = cpm_name;
         }
      

        private void button1_Click(object sender, EventArgs e)
        {
            var myOpenedTunnel = AccioEasyHelpers.ReadParamsThenConnectToDB(false);

            var sqlCMD = Scripts.FetchMyData(myOpenedTunnel, "users", new string[] { "user_name", "user_password","user_auth","user_full_name" }, new string[] {"user_id","user_auth"},new string[] {"999","'power'"},"!=","and");

            OracleDataReader dr = sqlCMD.ExecuteReader();
            if (dr.HasRows)
            {
                
            
                while (dr.Read())
                {
                    if (this.textBox1.Text == dr["user_name"].ToString() && this.textBox2.Text == dr["user_password"].ToString()
                       && loginAuth == dr["user_auth"].ToString() ) 
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

                        //read params from config
                        var data = AccioEasyHelpers.ReadTxTFiles(AccioEasyHelpers.MeExistanceLocation().Substring(0, AccioEasyHelpers.MeExistanceLocation().Length - ("AccioInventory.exe").Length) + "data\\params.info");

                        var cpm_name = AccioEasyHelpers.GetTxTBettwen(data[6], "::", ",");

                        var userNameLabel = (Label)AccioEasyHelpers.GetControlByName(parentForm, "holderUser");
                        userNameLabel.Text = "Logged as: " + dr["user_full_name"].ToString()+" for "+ cpm_name;
                        userNameLabel.ForeColor = Color.Gray;

                        if (admin)
                        {
                            var button_Adminstration = (Button)AccioEasyHelpers.GetControlByName(parentForm, "adminstrationButton");
                            button_Adminstration.Enabled = true;

                            var button_Configs = (Button)AccioEasyHelpers.GetControlByName(parentForm, "configsButton");
                            button_Configs.Enabled = true;

                            var auth_Sign = (Label)AccioEasyHelpers.GetControlByName(parentForm, "holdAuth");
                            auth_Sign.Text = "admin";
                        }
                        else
                        {
                            var auth_Sign_ = (Label)AccioEasyHelpers.GetControlByName(parentForm, "holdAuth");
                            auth_Sign_.Text = "user";
                        }
                        myOpenedTunnel.Close();
                        myOpenedTunnel.Dispose();
                        break;
                    }
                    else
                    {
                        this.label6.Text = "Failed to login";
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
            else
            {
                this.label6.Text = "Failed to login";
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

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //hide all pass text
          


        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(this.radioButton1.Checked)
            {
                admin = true;
                loginAuth = "admin";
            }
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                admin = false;
                loginAuth = "user";
            }
        }
    }
}
