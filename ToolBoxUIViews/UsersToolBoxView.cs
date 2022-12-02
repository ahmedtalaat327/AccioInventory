using AccioInventory.DBConnection;
using AccioInventory.Helpers;
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
        public UsersToolBoxView()
        {
            InitializeComponent();

            AllPanels.Add(tableLayoutPanel2);
            AllPanels.Add(tableLayoutPanel3);
            AllPanels.Add(tableLayoutPanel4);
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
        /// <param name="sender"></param>
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
            UsersPanelsEffected = UsersPanels.editPanel;
        }

        private void DisableWhatPanelUsed(object sender) {
            AllPanels.ForEach((Control ctrlPan) => {
                if(ctrlPan.Equals ((sender) as Control))
                {
                    AccioEasyHelpers.AllInludedControls(ctrlPan).ForEach((Control c) => { c.Enabled = true; });

                }
                else
                {
                    AccioEasyHelpers.AllInludedControls(ctrlPan).ForEach((Control c) => { c.Enabled = false; });
                }

            });

        }

    }
}
