using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace DofLog
{
    public class Logger
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        #region Public Fields

        public string VERSION = "v2.0.0";
        public Config config = new Config();

        #endregion Public Fields

        #region Private Fields

        private const int PAUSE = 100;

        #endregion Private Fields

        #region Constructor

        public Logger()
        {
            config.GenConfig();
        }

        #endregion Constructor

        #region Public Methods

        public Color GetPixel(Point p)
        {
            Rectangle rect = new Rectangle(p, new Size(2, 2));

            Bitmap map = CaptureFromScreen(rect);

            Color c = map.GetPixel(1, 1);

            map.Dispose();

            return c;
        }

        public void LogAccounts(List<Account> accounts)
        {
            if (!accounts.Any())
                throw new ArgumentException();
            var al = new AnkamaLauncher(this);
            var input = new InputSimulator();

            SetForegroundWindow(al.AL_Process.MainWindowHandle);

            void UnlogFromAL(AnkamaLauncher launcher, InputSimulator controller = null)
            {
                if (controller == null)
                    controller = new InputSimulator();

                /* UNLOG FROM AL*/
                SetForegroundWindow(launcher.AL_Process.MainWindowHandle);
                while (!launcher.IsGamesBtn(GetPixel(launcher.gamesBtn)))
                    Thread.Sleep(PAUSE * 2);
                LClickMouseTo(launcher.profileBtn, controller);
                CustomMouseTo(launcher.unlogBtn, controller);
                while (!launcher.IsUnlogBtn(GetPixel(launcher.unlogBtn)))
                    Thread.Sleep(PAUSE * 2);
                controller.Mouse.LeftButtonClick().Sleep(PAUSE);
            }

            Thread.Sleep(PAUSE * 2);

            if (al.IsGamesBtn(GetPixel(al.gamesBtn)))
                UnlogFromAL(al, input);

            /* CONNECT TO AL */
            while (!al.IsFbBtn(GetPixel(al.fbBtn)))
                Thread.Sleep(PAUSE * 2);
            LClickMouseTo(al.usernameField, input);
            WriteLogs(accounts[0], input);
            while (!al.IsConnectBtn(GetPixel(al.connectBtn)))
                Thread.Sleep(PAUSE * 2);
            input.Keyboard.KeyPress(VirtualKeyCode.RETURN).Sleep(PAUSE);

            /* CONNECT TO DOFUS */
            while (!al.IsGamesBtn(GetPixel(al.gamesBtn)))
                Thread.Sleep(PAUSE * 2);
            if (!config.RetroMode)
            {
                LClickMouseTo(al.dofusBtn, input);
            }

            Process[] dofusProcess = Process.GetProcessesByName("dofus");
            var dof = new Dofus();
            while (dofusProcess.Length < accounts.Count)
            {
                SetForegroundWindow(al.AL_Process.MainWindowHandle);

                CustomMouseTo(al.dofusBtn, input);
                while (!al.IsStartBtn(GetPixel(al.startBtn)))
                {
                    SetForegroundWindow(al.AL_Process.MainWindowHandle);
                    Thread.Sleep(PAUSE * 2);
                }
                LClickMouseTo(al.startBtn, input);

                var TMPdofusProcess = Process.GetProcessesByName("dofus");
                if (TMPdofusProcess.Length == dofusProcess.Length)
                    Thread.Sleep(PAUSE * 2);
                else
                    dofusProcess = TMPdofusProcess;
                Thread.Sleep(PAUSE * 2);
            }
            for (int i = 0; i < accounts.Count; i++)
            {
                SetForegroundWindow(dofusProcess[i].MainWindowHandle);
                while (true)
                {
                    Thread.Sleep(PAUSE * 2);
                    SetForegroundWindow(dofusProcess[i].MainWindowHandle);
                    if (dof.IsPlayBtn(GetPixel(dof.playBtn)))
                        break;
                    if (dof.IsLogBtn(GetPixel(dof.logBtn)))
                    {
                        Thread.Sleep(PAUSE * 2);
                        LClickMouseTo(dof.usernameField);
                        WriteLogs(accounts[i], input);
                        input.Keyboard.KeyPress(VirtualKeyCode.RETURN).Sleep(PAUSE);
                    }
                }
            }

            if (!config.StayLog)
                UnlogFromAL(al, input);

            SetForegroundWindow(dofusProcess[0].MainWindowHandle);
        }

        #endregion Public Methods

        #region Private Methods

        private Bitmap CaptureFromScreen(Rectangle rect)
        {
            Bitmap bmpScreenCapture;

            if (rect == Rectangle.Empty)//capture the whole screen
            {
                rect = Screen.PrimaryScreen.Bounds;
            }

            bmpScreenCapture = new Bitmap(rect.Width, rect.Height);

            Graphics p = Graphics.FromImage(bmpScreenCapture);

            p.CopyFromScreen(rect.X,
                     rect.Y,
                     0, 0,
                     rect.Size,
                     CopyPixelOperation.SourceCopy);

            p.Dispose();

            return bmpScreenCapture;
        }

        private void CustomMouseTo(Point p, InputSimulator iS = null)
        {
            if (iS == null)
                iS = new InputSimulator();

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            var X = p.X * 65535 / screenWidth;
            var Y = p.Y * 65535 / screenHeight;

            iS.Mouse.MoveMouseTo(Convert.ToDouble(X), Convert.ToDouble(Y));
        }

        private void LClickMouseTo(Point p, InputSimulator iS = null)
        {
            if (iS == null)
                iS = new InputSimulator();

            CustomMouseTo(p, iS);
            iS.Mouse.LeftButtonClick().Sleep(PAUSE);
        }

        private void WriteLogs(Account account, InputSimulator iS = null)
        {
            if (iS == null)
                iS = new InputSimulator();

            iS.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A)
                .KeyPress(VirtualKeyCode.DELETE).Sleep(PAUSE);
            iS.Keyboard.TextEntry(account.username).Sleep(PAUSE);

            iS.Keyboard.KeyPress(VirtualKeyCode.TAB).Sleep(PAUSE);
            iS.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A)
                .KeyPress(VirtualKeyCode.DELETE).Sleep(PAUSE);
            iS.Keyboard.TextEntry(account.password).Sleep(PAUSE);
        }

        #endregion Private Methods
    }
}