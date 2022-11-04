using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
