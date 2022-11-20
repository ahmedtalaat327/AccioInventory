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

namespace AccioInventory.ToolBoxUIViews
{
    public partial class UsersToolBoxView : UserControl
    {
        public List<string> AllDepts { get; set; }
        public UsersToolBoxView()
        {
            InitializeComponent();

          

              
        }

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

        private async void button1_Click(object sender, EventArgs e)
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
                var rep = await SetNewUser(userName,fullName,pass,inputDate,telNo,selectedAuth,selectedDept);

                if (t.IsCompleted)
                {
                    Cursor.Current = Cursors.Default;
                    Application.UseWaitCursor = false;
                    MessageBox.Show("Useer added successfully !","Done");
                }
            });
            t.Start();
        }

        private Task<int> SetNewUser(string userName, string fullName,string pass,DateTime inputDate,int telNo,string selectedAuth,int selectedDept)
        {
           

            
            return Task.Run(() =>
            {
                var myOpenedTunnel = AccioEasyHelpers.ReadParamsThenConnectToDB(false);
                Application.UseWaitCursor = true;
                Application.DoEvents();
                var replyOfOracle = Scripts.InsertMyDataRow(myOpenedTunnel, "users",
                   
                    new string[]
                    {
                    "3","'"+userName+"'","'"+fullName+"'","'" + pass + "'",telNo.ToString(),"DATE '"+inputDate.Year.ToString()+"-"+inputDate.Month.ToString()+"-"+inputDate.Day.ToString()+"'","''","'" + selectedAuth + "'" ,selectedDept.ToString()
                    }
                    
                    );

                myOpenedTunnel.Close();
                myOpenedTunnel.Dispose();
                return replyOfOracle;
            });

         }
    }
}
