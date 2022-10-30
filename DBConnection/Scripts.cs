using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;

namespace AccioInventory.DBConnection
{
    public static class Scripts
    {

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
    }
}
