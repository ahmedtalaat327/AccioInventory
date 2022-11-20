using AccioInventory.DBConnection;
using AccioInventory.Helpers;
using AccioInventory.Models;
using AccioInventory.ToolBoxUIViews;
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

        private static bool UsersToolBoxVisible { get; set; } = false;

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

            this.tabPage4.Paint += (g,d) => {
                if (!UsersToolBoxVisible)
                {
                    AdminstrationView.UsersToolBoxVisible = true;

                    Form miniVisibleToolsBoxUsers = new Form();
                    miniVisibleToolsBoxUsers.Size = new Size(390, 750);
                    miniVisibleToolsBoxUsers.StartPosition = FormStartPosition.Manual;
                    miniVisibleToolsBoxUsers.Location = new Point(5, 10);
                    miniVisibleToolsBoxUsers.Text = "Users Control Panel";
                    miniVisibleToolsBoxUsers.Icon = null;
                    miniVisibleToolsBoxUsers.ShowIcon = false;

                    UsersToolBoxView usersToolBoxView = new UsersToolBoxView();
                    var oulineSize = usersToolBoxView.Size;
                    miniVisibleToolsBoxUsers.Size = new Size(oulineSize.Width + 15, oulineSize.Height + 10);
                    miniVisibleToolsBoxUsers.Controls.Add(usersToolBoxView);
                    miniVisibleToolsBoxUsers.Visible = true;
                    miniVisibleToolsBoxUsers.TopMost = true;
                    miniVisibleToolsBoxUsers.FormClosed += (o, p) => { AdminstrationView.UsersToolBoxVisible = false; };
                    miniVisibleToolsBoxUsers.MaximizeBox = false;
                    miniVisibleToolsBoxUsers.Resize += (w,o) => { miniVisibleToolsBoxUsers.Size = new Size(oulineSize.Width + 15, oulineSize.Height + 10); };

                }
            };
           
        }
       
        private void tabControl1_Click(object sender, EventArgs e)
        {

            dataGridView2.AutoGenerateColumns = true;
            Task t = null;
            t = new Task( async () =>  { 
                AllUsers = new List<UsersModel>();

             
                    
                

               AllUsers = await (LoadingUsersFromDB(AllUsers));


                var bindingList = new BindingList<UsersModel>(AllUsers);
                var source = new BindingSource(bindingList, null);

                //dispacher must be here
                dataGridView2.Invoke(new Action(() => {dataGridView2.DataSource = source; }));



                if (t.IsCompleted)
                {
                    Cursor.Current = Cursors.Default;
                    Application.UseWaitCursor = false;
                }

            });
            t.Start();



        }
        private Task<List<UsersModel>> LoadingUsersFromDB(List<UsersModel> myList)
        {
            return Task.Run(() => { 
            var myOpenedTunnel = AccioEasyHelpers.ReadParamsThenConnectToDB(false);

            var sqlCMD = Scripts.FetchMyData(myOpenedTunnel, "users", new string[] { "user_id", "user_full_name", "user_password", "user_auth", "user_full_name","dept_id" }, new string[] { "user_id", "user_auth" }, new string[] { "999", "'power'" }, "!=", "and");


            OracleDataReader dr = sqlCMD.ExecuteReader();
            if (dr.HasRows)
            {


                while (dr.Read())
                {
                        myList.Add(new UsersModel() { Id = Int32.Parse(dr["user_id"].ToString()), FullName = dr["user_full_name"].ToString() ,DepartmentId = Int32.Parse(dr["dept_id"].ToString()) });

                }
            }

            myOpenedTunnel.Close();
            myOpenedTunnel.Dispose();

                Application.UseWaitCursor = true;
                Application.DoEvents();
         

            return myList;
            });
        }
    }
     
}
