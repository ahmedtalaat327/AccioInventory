using System;
using System.Threading;
using System.Windows.Forms;

namespace AccioInventory
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //check if the thread of the app is already running avoid multiple exe at same time!

            //we should test if data file is there or not if not let there be a message telling the user !
            PragmaChecker.UniqueEXERun();
          
            
        }
    }
    static class PragmaChecker
    {
        public static void UniqueEXERun()
        {
            Mutex mutex = new System.Threading.Mutex(false, "ACCIO_FORM_RUNNING");
            try
            {
                if (mutex.WaitOne(0, false))
                {
                    //run the actual class [represents form main form]
                    Application.Run(new Accio());
                }
                else
                {
                    MessageBox.Show("An instance of the application is already running.");
                }
            }
            finally
            {
                if (mutex != null)
                {
                    mutex.Close();
                    mutex = null;
                }
            }
        }
    }
}
