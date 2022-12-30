using AccioInventory.DBConnection;
using AccioInventory.Helpers;
using AccioInventory.Models;
using AccioInventory.UIViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccioInventory.ToolBoxUIViews
{
    public enum UsersPanels
    {
        None,
        addPanel,
        editPanel,
        delPanel
    }
    public partial class UsersToolBoxView : UserControl
    {
        /// <summary>
        /// Save all Departments to load in combobox..
        /// </summary>
        public List<string> AllDepts { get; set; }
        /// <summary>
        /// flag to meausre mouse effect on each panel
        /// </summary>
        private UsersPanels UsersPanelsEffected { get; set; } = UsersPanels.None;
        /// <summary>
        /// hold all three panels
        /// </summary>
        private List<Control> AllPanels { get; set; } = new List<Control>();
        /// <summary>
        /// Constructor
        /// </summary>
        /// /// <summary>
        /// Collect all users in one list actually it's one user depending on id to search..
        /// </summary>
        public List<UsersModel> AllUsers { get; set; }
        public UsersToolBoxView()
        {
            InitializeComponent();

            AllPanels.Add(tableLayoutPanel2);
            AllPanels.Add(tableLayoutPanel3);
            AllPanels.Add(tableLayoutPanel4);
            AllPanels.Add(tableLayoutPanel5);
        }
        /// <summary>
        /// Task func using managed and pooled threads to collect data while using UI 
        /// Loading all deps names to combobox
        /// </summary>
        /// <param name="depsList">current list object</param>
        /// <returns></returns>
        private Task<List<string>> LoadDepsToComboBox(List<string> depsList)
        {
            return Task.Run(() =>
            {
                var myOpenedTunnel = AccioEasyHelpers.ReadParamsThenConnectToDB(false);

                var sqlCMD = Scripts.FetchMyData(myOpenedTunnel, "departments", new string[] { "dept_name" }, new string[] { "dept_id" }, new string[] { "999" }, "!=", "and");


                OracleDataReader dr = sqlCMD.ExecuteReader();


                Application.UseWaitCursor = true;
                Application.DoEvents();
                if (dr.HasRows)
                {


                    while (dr.Read())
                    {
                        depsList.Add(dr["dept_name"].ToString());
                    }
                }

                myOpenedTunnel.Close();
                myOpenedTunnel.Dispose();



                return depsList;
            });
        }
        /// <summary>
        /// Event trigged or fired by dropping down combobox menu / windows
        /// this for add section users
        /// </summary>
        /// <param name="sender">object button to be clicked</param>
        /// <param name="e">event used in fireing clicking up</param>
        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            Task t = null;
            t = new Task(async () =>
            {
                AllDepts = new List<string>();

                AllDepts = await LoadDepsToComboBox(AllDepts);

                var bindingList = new BindingList<string>(AllDepts);
                var source = new BindingSource(bindingList, null);

                //dispacher must be here
                comboBox2.Invoke(new Action(() => { comboBox2.DataSource = source; }));

                if (t.IsCompleted)
                {
                    Cursor.Current = Cursors.Default;
                    Application.UseWaitCursor = false;
                }
            });
            t.Start();
        }
        /// <summary>
        /// Submit button event while adding new User..
        /// </summary>
        /// <param name="sender">object buttton clicked</param>
        /// <param name="e">even t to be used</param>
        private  void button1_Click(object sender, EventArgs e)
        {


            if (comboBox1.SelectedIndex < 0) { MessageBox.Show("Not sufficient inputs here!"); }
            else if (comboBox2.SelectedIndex < 0) { MessageBox.Show("Not sufficient inputs here!"); }
            else if (textBox1.Text.Length <= 0) { MessageBox.Show("Not sufficient inputs here!"); }
            else if (textBox2.Text.Length <= 0) { MessageBox.Show("Not sufficient inputs here!"); }
            else if (textBox3.Text.Length <= 0) { MessageBox.Show("Not sufficient inputs here!"); }
            else if (textBox4.Text.Length <= 0) { MessageBox.Show("Not sufficient inputs here!"); }
            else
            {

                var fullName = textBox1.Text;
                var userName = textBox2.Text;
                var pass = textBox3.Text;
                var telNo = Int32.Parse(textBox4.Text);
                var selectedAuth = comboBox1.SelectedItem.ToString();
                var selectedDept = Int32.Parse(comboBox2.SelectedIndex.ToString());
                DateTime inputDate = Convert.ToDateTime("01/01/2019");

                Task t = null;
                t = new Task(async () =>
                {
                    var rep = await SetNewUser(userName, fullName, pass, inputDate, telNo, selectedAuth, selectedDept);

                    if (t.IsCompleted && rep != -1)
                    {
                        Cursor.Current = Cursors.Default;
                        Application.UseWaitCursor = false;
                        MessageBox.Show("User added successfully !", "Done");
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        Application.UseWaitCursor = false;
                        MessageBox.Show("Something is wrong !", "Didn't happened");
                    }
                });
                t.Start();
            }
        }
        /// <summary>
        /// Real function using tasks 
        /// </summary>
        /// <param name="userName">user name in text box</param>
        /// <param name="fullName">full name in  // //</param>
        /// <param name="pass">password in text box</param>
        /// <param name="inputDate"></param>
        /// <param name="telNo">telephone number</param>
        /// <param name="selectedAuth">user level authority</param>
        /// <param name="selectedDept">user current department</param>
        /// <returns></returns>
        private Task<int> SetNewUser(string userName, string fullName, string pass, DateTime inputDate, int telNo, string selectedAuth, int selectedDept)
        {



            return Task.Run(() =>
            {
                Application.UseWaitCursor = true;
                Application.DoEvents();

                //get highest column id no first...
                var myOpenedTunnel = AccioEasyHelpers.ReadParamsThenConnectToDB(false);
                int id_comu = Scripts.GetHighestNOofRow(myOpenedTunnel,"users","user_id") + 1;

                myOpenedTunnel.Close();
                myOpenedTunnel.Dispose();
                //then make new insertion
                myOpenedTunnel = AccioEasyHelpers.ReadParamsThenConnectToDB(false);
               
                var replyOfOracle = Scripts.InsertMyDataRow(myOpenedTunnel, "users",

                    new string[]
                    {
                    id_comu.ToString(),"'"+userName+"'","'"+fullName+"'","'" + pass + "'",telNo.ToString(),"DATE '"+inputDate.Year.ToString()+"-"+inputDate.Month.ToString()+"-"+inputDate.Day.ToString()+"'","''","'" + selectedAuth + "'" ,selectedDept.ToString()
                    }

                    );

                myOpenedTunnel.Close();
                myOpenedTunnel.Dispose();
                return replyOfOracle;
            });

        }

        /// <summary>
        /// if mouse entered add panel
        /// </summary>
        /// <param name="sender">current panel</param>
        /// <param name="e"></param>
        private void tableLayoutPanel2_MouseEnter(object sender, EventArgs e)
        {
            DisableWhatPanelUsed(sender);
            
            UsersPanelsEffected = UsersPanels.addPanel;
        }

        /// <summary>
        /// if mouse entered edit panel
        /// </summary>
        /// <param name="sender">current panel</param>
        /// <param name="e"></param>
        private void tableLayoutPanel3_MouseEnter(object sender, EventArgs e)
        {
            DisableWhatPanelUsed(sender);
            //due to bug we will activate them manually ..
            label9.Enabled = true;
            textBox5.Enabled = true;
      
            UsersPanelsEffected = UsersPanels.editPanel;
        }
        /// <summary>
        /// if mouse entered delete panel
        /// </summary>
        /// <param name="sender">current panel</param>
        /// <param name="e"></param>
        private void tableLayoutPanel5_MouseEnter(object sender, EventArgs e)
        {
            DisableWhatPanelUsed(sender);
            //due to bug we will activate them manually ..
            label18.Enabled = true;
            textBox10.Enabled = true;

            UsersPanelsEffected = UsersPanels.delPanel;
        }

        /// <summary>
        /// Generic func to disable non-focused panel and re-focus the one we neeed.
        /// </summary>
        /// <param name="sender">panel as controler object</param>
        private void DisableWhatPanelUsed(object sender) {
            AllPanels.ForEach((Control ctrlPan) => {
                if(ctrlPan.Equals ((sender) as Control))
                {
                    AccioEasyHelpers.AllInludedControls(ctrlPan, new List<Control>(new Control[0])).ForEach((Control c) => { c.Enabled = true; });

                }
                else
                {
                    AccioEasyHelpers.AllInludedControls(ctrlPan, new List<Control>(new Control[0])).ForEach((Control c) => { c.Enabled = false; });
                }

            });

        }
        /// <summary>
        /// Search in edit / update section
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, EventArgs e)
        {
            if (textBox5.Text.Length <= 0)
            {
                return;
            }

            Task t = null;
            t = new Task(async () => {
                AllUsers = new List<UsersModel>();

                AllUsers = await AdminstrationView.LoadingUsersFromDB(AllUsers, textBox5.Text, "=");

                if(AllUsers.Count == 1)
                 {
                    //dispatcher must be here ..
                    this.tableLayoutPanel3.Invoke(new Action(() => { 
                        this.textBox6.Text = AllUsers[0].UserName; 
                        this.textBox7.Text = AllUsers[0].FullName;
                        this.textBox8.Text = AllUsers[0].Password;
                        this.textBox9.Text = AllUsers[0].TelNo.ToString();
                        //check which item should be selected in combobox for auth-level
                        for(int i=0;i<this.comboBox3.Items.Count;i++)
                        {
                            if (this.comboBox3.Items[i].ToString() == AllUsers[0].UserAuthLevel)
                            {
                                this.comboBox3.SelectedIndex = i; 
                            }
                        }
                        //this part for departments names
                        //first we laod all depts ids from database 
                        //then represents all names in string usnig <see cref="UsersModel.DepartmentName"/>

                        Task _t = null;
                        _t = new Task(async () =>
                        {
                            AllDepts = new List<string>();

                            AllDepts = await LoadDepsToComboBox(AllDepts);

                            var bindingList = new BindingList<string>(AllDepts);
                            var source = new BindingSource(bindingList, null);

                            //dispacher must be here
                            comboBox4.Invoke(new Action(() => {

                                comboBox4.DataSource = source;
                                this.comboBox4.SelectedIndex = AllUsers[0].DepartmentId;
                            }));

                            if (_t.IsCompleted)
                            {
                                Cursor.Current = Cursors.Default;
                                Application.UseWaitCursor = false;
                            }
                        });
                        _t.Start();

                        //then comapre it with all we got and show the chosen one in combobox

                        this.button3.Enabled = true;
                    }));
                 }
                else
                {
                    //not found any user
                    MessageBox.Show("No ID found recheck it again", "No user with this ID");
                    this.tableLayoutPanel1.Invoke(new Action(() => {/*reset all controls*/
                        this.textBox6.Text = "";
                        this.textBox7.Text = "";
                        this.textBox8.Text = "";
                        this.textBox9.Text = "";
                        
                        this.comboBox3.SelectedIndex = 0;
                        this.comboBox4.SelectedIndex = 0;
                        
                        this.button3.Enabled = false;
                    }));
                 
                }
                if (t.IsCompleted)
                {
                    Cursor.Current = Cursors.Default;
                    Application.UseWaitCursor = false;
                }

            });
            t.Start();
        }
        /// <summary>
        /// Event trigged or fired by dropping down combobox menu / windows
        /// this for update section users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox4_DropDown(object sender, EventArgs e)
        {
            Task t = null;
            t = new Task(async () =>
            {
                AllDepts = new List<string>();

                AllDepts = await LoadDepsToComboBox(AllDepts);

                var bindingList = new BindingList<string>(AllDepts);
                var source = new BindingSource(bindingList, null);

                //dispacher must be here
                comboBox4.Invoke(new Action(() => {
               
                    comboBox4.DataSource = source;
                }));

                if (t.IsCompleted)
                {
                    Cursor.Current = Cursors.Default;
                    Application.UseWaitCursor = false;
                }
            });
            t.Start();
        }
        /// <summary>
        /// update in edit section
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">event</param>
        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
