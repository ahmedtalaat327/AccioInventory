using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;

namespace AccioInventory.DBConnection
{
    public static class Scripts
    {
        /// <summary>
        /// Set server params to check current connection to DB.
        /// </summary>
        /// <param name="dbServerParams">All params to connect to local or remote IP device holding the DB</param>
        /// <returns></returns>
        public static OracleConnection TestConnection(string[] dbServerParams)
        {
            //params
            //[0] = IP = 127.0.0.1
            //[1] = Port = 1521
            //[2] = user = store
            //[3] = pass = store
            //test connectio to oracle
            string oradb = "Data Source =" + dbServerParams[0] + ":" + dbServerParams[1] +" / orcl; User Id = " + dbServerParams[2] +"; password = " + dbServerParams[3]+";";

            OracleConnection conn = new OracleConnection(oradb);
            try
            {
                conn.Open();
              
            }
            catch (Exception orExc)
            {
                MessageBox.Show(orExc.Message, "Database connection error!");
                return null;
            }

            Console.Write("Connected to Oracle" + conn.ServerVersion);
            // Close and Dispose OracleConnection object
            conn.Close();
            conn.Dispose();
            Console.Write("Disconnected");
            return conn;
        }
        /// <summary>
        /// This function mainly made for general purpose to query from any table.
        /// </summary>
        /// <param name="oraConn">Object that holds connection</param>
        /// <param name="tablename">Current table to fetch query from</param>
        /// <param name="choosenFields">Fields to collect all data from</param>
        /// <param name="values">Values to compre with</param>
        /// <param name="oper">Cmpare operations</param>
        /// <returns></returns>
        public static OracleDataReader FetchMyData(OracleConnection oraConn, string tablename, string[] choosenFields, string[] values, string oper,string seper)
        {

            OracleCommand cmd = new OracleCommand();
            string sqlQueryStatement = "select";
            for (int x = 0; x < choosenFields.Length; x++)
            {
                sqlQueryStatement += choosenFields[x] + ",";
            }
            sqlQueryStatement = sqlQueryStatement.Substring(0, sqlQueryStatement.Length - 1);
            sqlQueryStatement += "from" + tablename + "where";

            string select = WherePartQueryTxt(sqlQueryStatement, choosenFields, oper, seper);

            cmd.CommandText = sqlQueryStatement;
            cmd.Connection = oraConn;

            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                return dr;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlOldCommand"></param>
        /// <param name="fields"></param>
        /// <param name="oper"></param>
        /// <param name="seper"></param>
        /// <returns></returns>
        private static string WherePartQueryTxt(string sqlOldCommand, string[] fields,string oper,string seper) {

            string sqlStatement = sqlOldCommand;

            for (int x =0; x < fields.Length; x++) {
                sqlStatement += " " + fields[x] + " " + oper + "? " + seper;
            }

            sqlStatement = sqlStatement.Substring(0, sqlStatement.Length - seper.Length);

            return sqlStatement;
        }
    }
}
