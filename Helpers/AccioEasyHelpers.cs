using System;
using System.Collections.Generic;
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
    }
}
