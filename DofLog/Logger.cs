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

        #region Private Fields

        private const int PAUSE = 100;

        #endregion Private Fields

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
            var al = new AnkamaLauncher();
            var input = new InputSimulator();

            void SetForegroundWindowAL()
            {
                foreach (var process in al.AL_Process)
                    SetForegroundWindow(process.MainWindowHandle);
            }

            void UnlogFromAL(AnkamaLauncher launcher, InputSimulator controller = null)
            {
                if (controller == null)
                    controller = new InputSimulator();

                /* UNLOG FROM AL*/
                App.logstream.Log("Logging off from AL");
                SetForegroundWindowAL();
                App.logstream.Log($"Waiting to detect the games button (x:{launcher.gamesBtn.X},y:{launcher.gamesBtn.Y})");
                while (!launcher.IsGamesBtn(GetPixel(launcher.gamesBtn)))
                    Thread.Sleep(PAUSE * 2);
                LClickMouseTo(launcher.profileBtn, controller);
                CustomMouseTo(launcher.unlogBtn, controller);
                App.logstream.Log($"Waiting to detect the unlog button (x:{launcher.unlogBtn.X},y:{launcher.unlogBtn.Y})");
                while (!launcher.IsUnlogBtn(GetPixel(launcher.unlogBtn)))
                    Thread.Sleep(PAUSE * 2);
                controller.Mouse.LeftButtonClick().Sleep(PAUSE);
            }

            SetForegroundWindowAL();

            Thread.Sleep(PAUSE * 2);

            /* CONNECT TO AL */
            App.logstream.Log($"Waiting to detect FB button (x:{al.fbBtn.X},y:{al.fbBtn.Y})");
            while (!al.IsFbBtn(GetPixel(al.fbBtn)))
            {
                Thread.Sleep(PAUSE * 2);
                if (al.IsGamesBtn(GetPixel(al.gamesBtn)))
                {
                    UnlogFromAL(al, input);
                    App.logstream.Log("Unlogged from AL");
                }
            }
            App.logstream.Log("FB button found !");
            LClickMouseTo(al.usernameField, input);
            WriteLogs(accounts[0], input);
            App.logstream.Log($"Waiting to detect the log in button (x:{al.connectBtn.X},y:{al.connectBtn.Y})");
            while (!al.IsConnectBtn(GetPixel(al.connectBtn)))
                Thread.Sleep(PAUSE * 2);
            input.Keyboard.KeyPress(VirtualKeyCode.RETURN).Sleep(PAUSE);
            App.logstream.Log("First account connected");

            /* CONNECT TO DOFUS */
            App.logstream.Log($"Waiting to detect the play button (x:{al.gamesBtn.X},y:{al.gamesBtn.Y})");
            while (!al.IsGamesBtn(GetPixel(al.gamesBtn)))
                Thread.Sleep(PAUSE * 2);
            App.logstream.Log("Play button found !");
            if (!App.config.RetroMode)
            {
                LClickMouseTo(al.dofusBtn, input);
            }

            Process[] dofusProcess = Process.GetProcessesByName("dofus");
            App.logstream.Log("Starting all the dofus instances");
            var dof = new Dofus();
            while (dofusProcess.Length < accounts.Count)
            {
                SetForegroundWindowAL(); ;

                CustomMouseTo(al.dofusBtn, input);
                while (!al.IsStartBtn(GetPixel(al.startBtn)))
                {
                    SetForegroundWindowAL(); ;
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
                App.logstream.Log($"Connecting the dofus instance number {i}");
                SetForegroundWindow(dofusProcess[i].MainWindowHandle);
                var DoOnceDetect = false;
                var DoOnceConnec = false;
                while (true)
                {
                    Thread.Sleep(PAUSE * 2);
                    SetForegroundWindow(dofusProcess[i].MainWindowHandle);
                    if (!DoOnceDetect)
                    {
                        App.logstream.Log($"Waiting to detect the connect button (x:{dof.logBtn.X},y:{dof.logBtn.Y})");
                        App.logstream.Log($"or the play button (x:{dof.playBtn.X},y:{dof.playBtn.Y})");
                        DoOnceDetect = true;
                    }
                    if (dof.IsPlayBtn(GetPixel(dof.playBtn)))
                    {
                        App.logstream.Log("Play button found ! No need to connect");
                        break;
                    }
                    if (dof.IsLogBtn(GetPixel(dof.logBtn)))
                    {
                        App.logstream.Log("Connect button found !");
                        Thread.Sleep(PAUSE * 2);
                        LClickMouseTo(dof.usernameField);
                        WriteLogs(accounts[i], input);
                        input.Keyboard.KeyPress(VirtualKeyCode.RETURN).Sleep(PAUSE);
                    }
                    if (!DoOnceConnec)
                    {
                        App.logstream.Log($"Dofus instance number {i} connected !");
                        DoOnceConnec = true;
                    }
                }
            }

            App.logstream.Log($"Waiting to detect the play button (x:{dof.logBtn.X},y:{dof.logBtn.Y})");
            while (!dof.IsPlayBtn(GetPixel(dof.playBtn)))
            {
                Thread.Sleep(PAUSE * 2);
                SetForegroundWindow(dofusProcess[0].MainWindowHandle);
            }

            if (!App.config.StayLog)
                UnlogFromAL(al, input);

            SetForegroundWindow(dofusProcess[0].MainWindowHandle);

            App.logstream.Log("Connected and ready to play !");
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