
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

        public string DepartmentName { get { return UsersModel.GetDeptName(deptId); } }

        /// <summary>
        /// This function is used by many other classes main goal here to convert Id from [int] to [String] in fastway
        /// we made it static to not create many objects in memory.
        /// </summary>
        /// <param name="deptId">int no that represents PK for real string name</param>
        /// <returns></returns>
        public static string GetDeptName(int deptId)
        {
            var myOpenedTunnel = AccioEasyHelpers.ReadParamsThenConnectToDB(false);

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
