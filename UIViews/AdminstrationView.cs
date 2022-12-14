using AccioInventory.DBConnection;
using AccioInventory.Helpers;
using AccioInventory.Models;
using AccioInventory.ToolBoxUIViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccioInventory.UIViews
{
    public partial class AdminstrationView : UserControl
    {
        /// <summary>
        /// Collect all users in one list
        /// </summary>
        public  List<UsersModel> AllUsers  { get; set; }
        /// <summary>
        /// Flag to determines if the window is visible or not?
        /// </summary>
        private static bool UsersToolBoxVisible { get; set; } = false;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentPanel">Panel to hold all UI controls</param>
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
        /// <summary>
        /// Event fires when the tabe is clicked
        /// </summary>
        /// <param name="sender">object clicked</param>
        /// <param name="e">event used in clicking</param>
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
        /// <summary>
        /// Task to load all users
        /// </summary>
        /// <param name="myList">current list object</param>
        /// <returns></returns>
        public static Task<List<UsersModel>> LoadingUsersFromDB(List<UsersModel> myList,string conditionIDNo = "999",string conditionOperator = "!=")
        {

            return Task.Run(() => { 
            var myOpenedTunnel = AccioEasyHelpers.ReadParamsThenConnectToDB(false);

            var sqlCMD = Scripts.FetchMyData(myOpenedTunnel, "users", new string[] { "user_id", "user_name","user_full_name", "user_password", "user_tel","user_seen_date","user_session","user_auth","dept_id" }, new string[] { "user_id" }, new string[] { conditionIDNo }, conditionOperator, "and");


            OracleDataReader dr = sqlCMD.ExecuteReader();

            if (dr.HasRows)
            {
                  

                while (dr.Read())
                {
                        myList.Add(new UsersModel() { Id = Int32.Parse(dr["user_id"].ToString()),
                            UserName = dr["user_name"].ToString(),
                            FullName = dr["user_full_name"].ToString(),
                            Password = dr["user_password"].ToString(),
                            TelNo = Int32.Parse(dr["user_tel"].ToString()),
                            LastSeen = (DateTime)dr["user_seen_date"],
                            UserInSession= dr["user_session"].ToString(),
                            UserAuthLevel = dr["user_auth"].ToString(),
                            DepartmentId = Int32.Parse(dr["dept_id"].ToString()) }) ; 
                         
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
