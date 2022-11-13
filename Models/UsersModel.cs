
using AccioInventory.DBConnection;
using AccioInventory.Helpers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel;

namespace AccioInventory.Models
{
    /// <summary>
    /// Each model class represents a datatype usually used to create lists.
    /// </summary>
    public class UsersModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; } 
        public int TelNo { get; set; } 
        public DateTime LastSeen { get; set; } 
        public string UserInSession { get; set; } 
        public string UserAuthLevel { get; set; }


        private int deptId = 0;
        [Browsable(false)]
        public int DepartmentId { get { return deptId; } set { deptId = value; } }

        public string DepartmentName { get { return GetDeptName(deptId); } }


        private OracleConnection TestConn(bool autoclose)
        {
            //read params from config
            var data = AccioEasyHelpers.ReadTxTFiles(AccioEasyHelpers.MeExistanceLocation().Substring(0, AccioEasyHelpers.MeExistanceLocation().Length - ("AccioInventory.exe").Length) + "data\\params.info");

            var server_adress = AccioEasyHelpers.GetTxTBettwen(data[4], "::", ",");
            var port = AccioEasyHelpers.GetTxTBettwen(data[5], "::", ",");

            return Scripts.TestConnection(new[] { server_adress, port, "store", "store" }, autoclose);
        }
        private string GetDeptName(int deptId)
        {
            var myOpenedTunnel = TestConn(false);

            var sqlCMD = Scripts.FetchMyData(myOpenedTunnel, "departments", new string[] { "dept_name" }, new string[] { "dept_id" }, new string[] { $"{deptId}" }, "=", "and");


            OracleDataReader dr = sqlCMD.ExecuteReader();

            string Dep = "No Name";

            if (dr.HasRows)
            {


                while (dr.Read())
                {
                    Dep = dr["dept_name"].ToString();
                }
            }

            myOpenedTunnel.Close();
            myOpenedTunnel.Dispose();

            return Dep;
        }
    }
}
