using AccioInventory.DBConnection;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace AccioInventory.Helpers
{
    public static class AccioEasyHelpers
    {
        /// <summary>
        /// Get any controlers by name in parent controle
        /// </summary>
        /// <param name="ParentCntl"></param>
        /// <param name="NameToSearch"></param>
        /// <returns></returns>
        public static Control GetControlByName(Control ParentCntl, string NameToSearch)
        {
            if (ParentCntl.Name == NameToSearch)
                return ParentCntl;

            foreach (Control ChildCntl in ParentCntl.Controls)
            {
                Control ResultCntl = GetControlByName(ChildCntl, NameToSearch);
                if (ResultCntl != null)
                    return ResultCntl;
            }
            return null;
        }
        /// <summary>
        /// Get relative location to me executaive application...
        /// </summary>
        /// <returns></returns>
        public static string MeExistanceLocation()
        {
            return System.Reflection.Assembly.GetEntryAssembly().Location;
        }
        public static string[] ReadTxTFiles(string pathToFile)
        {
            List<string> data = new List<string>();


            data.AddRange(System.IO.File.ReadAllLines(pathToFile));



            return data.ToArray();
        }

        public static string GetTxTBettwen(string txt, string first, string last)
        {

            StringBuilder sb = new StringBuilder(txt);
            int pos1 = txt.IndexOf(first) + first.Length;
            int len = (txt.Length) - pos1;

            string reminder = txt.Substring(pos1, len);


            int pos2 = reminder.IndexOf(last) - last.Length + 1;




            return reminder.Substring(0, pos2);



        }

        /// <summary>
        /// Test connectivity to database 
        /// </summary>
        /// <param name="autoclose">show if automatic connection needs to be closed or not</param>
        /// <returns></returns>
        public static OracleConnection ReadParamsThenConnectToDB(bool autoclose)
        {
            //read params from config
            var data = AccioEasyHelpers.ReadTxTFiles(AccioEasyHelpers.MeExistanceLocation().Substring(0, AccioEasyHelpers.MeExistanceLocation().Length - ("AccioInventory.exe").Length) + "data\\params.info");

            var server_adress = AccioEasyHelpers.GetTxTBettwen(data[4], "::", ",");
            var port = AccioEasyHelpers.GetTxTBettwen(data[5], "::", ",");

            return Scripts.TestConnection(new[] { server_adress, port, "store", "store" }, autoclose);
        }
        /// <summary>
        /// Will return all controls
        /// </summary>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public static List<Control> AllInludedControls(Control ParentCntl)
        {

            List<Control> allCNTRL = new List<Control>(new Control[0]);

            foreach (Control ChildCntl in ParentCntl.Controls)
            {
                allCNTRL.Add(ChildCntl);

                
                if (ChildCntl.HasChildren && ChildCntl.GetType() == typeof(TableLayoutPanel))
                {
                    allCNTRL.AddRange(AllInludedControls(ChildCntl));
                }

          
            }


            return allCNTRL;

        }
         
    }
}
