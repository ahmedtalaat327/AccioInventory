using AccioInventory.DBConnection;
using AccioInventory.Helpers;
using AccioInventory.Models;
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
    public partial class AdminstrationView : UserControl
    {

      public  List<UsersModel> AllUsers  { get; set; }

    public AdminstrationView(Control parentPanel)
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
           
        }
        /// <summary>
        /// Test connectivity to database 
        /// </summary>
        /// <param name="autoclose">show if automatic connection needs to be closed or not</param>
        /// <returns></returns>
        private OracleConnection TestConn(bool autoclose)
        {
            //read params from config
            var data = AccioEasyHelpers.ReadTxTFiles(AccioEasyHelpers.MeExistanceLocation().Substring(0, AccioEasyHelpers.MeExistanceLocation().Length - ("AccioInventory.exe").Length) + "data\\params.info");

            var server_adress = AccioEasyHelpers.GetTxTBettwen(data[4], "::", ",");
            var port = AccioEasyHelpers.GetTxTBettwen(data[5], "::", ",");

            return Scripts.TestConnection(new[] { server_adress, port, "store", "store" }, autoclose);
        }
        private void tabControl1_Click(object sender, EventArgs e)
        {

            dataGridView2.AutoGenerateColumns = true;
            Task t = null;
            t = new Task( async () =>  { 
                AllUsers = new List<UsersModel>();

              //  if (!t.IsCompleted)
                //    MessageBox.Show("Loading", "Please wait!");
                

            AllUsers = await (LoadingUsersFromDB(AllUsers));


                var bindingList = new BindingList<UsersModel>(AllUsers);
                var source = new BindingSource(bindingList, null);

                //dispacher must be here
                dataGridView2.Invoke(new Action(() => {dataGridView2.DataSource = source; }));
                

             


            });
            t.Start();



        }
        private Task<List<UsersModel>> LoadingUsersFromDB(List<UsersModel> myList)
        {
            return Task.Run(() => { 
            var myOpenedTunnel = TestConn(false);

            var sqlCMD = Scripts.FetchMyData(myOpenedTunnel, "users", new string[] { "user_id", "user_full_name", "user_password", "user_auth", "user_full_name" }, new string[] { "user_id", "user_auth" }, new string[] { "999", "'power'" }, "!=", "and");


            OracleDataReader dr = sqlCMD.ExecuteReader();
            if (dr.HasRows)
            {


                while (dr.Read())
                {
                        myList.Add(new UsersModel() { Id = Int32.Parse(dr["user_id"].ToString()), FullName = dr["user_full_name"].ToString() });

                }
            }

            myOpenedTunnel.Close();
            myOpenedTunnel.Dispose();

            return myList;
            });
        }
    }
     
}
