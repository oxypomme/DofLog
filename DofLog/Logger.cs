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

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        internal struct Rect
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        #region Private Fields

        private const int PAUSE = 100;

        #endregion Private Fields

        #region Public Methods

        public static void LogAccounts(List<Account> accounts)
        {
            //TODO: timeout

            if (!accounts.Any())
                throw new ArgumentException();
            var input = new InputSimulator();

            void SetForegroundWindowAL(AnkamaLauncher launcher)
            {
                foreach (var process in launcher.process)
                {
                    SetForegroundWindow(process.MainWindowHandle);
                }
            }

            void UnlogFromAL(AnkamaLauncher launcher, InputSimulator controller = null)
            {
                if (controller == null)
                    controller = new InputSimulator();

                /* UNLOG FROM AL*/
                App.logstream.Log("Logging off from AL");
                SetForegroundWindowAL(launcher);
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

            // Attenmping to get the AL processes
            var AL_Process = Process.GetProcessesByName("ankama launcher");
            if (AL_Process.Length <= 0)
            {
                // Starting AL
                // TODO : AL is currently bind to DofLog process
                var startInfo = new ProcessStartInfo(App.config.AL_Path);
                startInfo.WorkingDirectory = System.IO.Directory.GetParent(App.config.AL_Path).FullName;
                Process.Start(startInfo);
                AL_Process = Process.GetProcessesByName("ankama launcher");
            }

            var AL_Origin = new Point(0, 0);
            var AL_Size = new Size(0, 0);
            {   // Redefine the origin of AL window
                while (true)
                {
                    foreach (var proc in AL_Process)
                    {
                        var ptr = proc.MainWindowHandle;
                        var procWin = new Rect();
                        GetWindowRect(ptr, ref procWin);
                        if (procWin.Left > AL_Origin.X && procWin.Top > AL_Origin.Y)
                            AL_Origin = new Point(procWin.Left, procWin.Top);
                        if (procWin.Right - procWin.Left > AL_Size.Width && procWin.Bottom - procWin.Top > AL_Size.Height)
                            AL_Size = new Size(procWin.Right - procWin.Left, procWin.Bottom - procWin.Top);
                    }
                    if (AL_Size.Width != 0 && AL_Size.Height != 0)
                        break;
                }

                App.logstream.Log($"AL orig (x:{AL_Origin.X},y: {AL_Origin.Y})", "DEBUG");
                App.logstream.Log($"AL size (x:{AL_Size.Width},y: {AL_Size.Height})", "DEBUG");
            }
            var al = new AnkamaLauncher(AL_Process, AL_Origin, AL_Size);

            SetForegroundWindowAL(al);

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
            App.logstream.Log($"Waiting to detect the game button (x:{al.gamesBtn.X},y:{al.gamesBtn.Y})");
            while (!al.IsGamesBtn(GetPixel(al.gamesBtn)))
                Thread.Sleep(PAUSE * 2);
            App.logstream.Log("Game button found !");
            if (!App.config.RetroMode)
            {
                LClickMouseTo(al.dofusBtn, input);
            }

            var dofusProcess = Process.GetProcessesByName("dofus").ToList();
            App.logstream.Log("Starting all the dofus instances");
            // TODO BUG: AL size can't start
            var dofs = new List<Dofus>();
            while (dofs.Count < accounts.Count)
            {
                SetForegroundWindowAL(al); ;

                CustomMouseTo(al.dofusBtn, input);
                while (!al.IsStartBtn(GetPixel(al.startBtn)))
                {
                    SetForegroundWindowAL(al); ;
                    Thread.Sleep(PAUSE * 2);
                }
                LClickMouseTo(al.startBtn, input);

                var TMPdofusProcess = Process.GetProcessesByName("dofus").ToList();
                if (TMPdofusProcess.Count > dofusProcess.Count)
                    foreach (var proc in TMPdofusProcess)
                        if (!dofusProcess.Any(item => item.Id == proc.Id))
                        {
                            var procWin = new Rect();
                            var size = new Size();
                            var doOnce = true;
                            while (true)
                            {
                                Thread.Sleep(PAUSE * 2);
                                var ptr = proc.MainWindowHandle;
                                GetWindowRect(ptr, ref procWin);

                                if (doOnce)
                                {
                                    App.logstream.Log("Waiting dofus start");
                                    doOnce = false;
                                }

                                size = new Size(procWin.Right - procWin.Left, procWin.Bottom - procWin.Top);
                                if (size.Width != 0 && size.Height != 0)
                                    break;
                            }
                            App.logstream.Log($"Dofus orig (x:{procWin.Left},y: {procWin.Top})", "DEBUG");
                            App.logstream.Log($"Dofus size (x:{size.Width},y: {size.Height})", "DEBUG");

                            var dof = new Dofus(proc, new Point(procWin.Left, procWin.Top), size);
                            dofs.Add(dof);
                            App.logstream.Log($"Dofus count : {dofs.Count}");
                            dofusProcess = TMPdofusProcess;
                        }
            }
            for (int i = 0; i < accounts.Count; i++)
            {
                App.logstream.Log($"Connecting the dofus instance number {i}");
                var dof = dofs[i];
                var conectFound = false;
                var doOnce = true;
                while (true)
                {
                    Thread.Sleep(PAUSE * 2);
                    SetForegroundWindow(dofs[i].process.MainWindowHandle);
                    Thread.Sleep(PAUSE * 4);
                    if (doOnce)
                        App.logstream.Log($"Waiting to detect the connect button (x:{dof.logBtn.X},y:{dof.logBtn.Y})");
                    if (dof.IsLogBtn(GetPixel(dof.logBtn)) && !conectFound)
                    {
                        App.logstream.Log("Connect button found !");
                        Thread.Sleep(PAUSE * 2);
                        LClickMouseTo(dof.usernameField);
                        WriteLogs(accounts[i], input);
                        input.Keyboard.KeyPress(VirtualKeyCode.RETURN).Sleep(PAUSE);
                        App.logstream.Log($"Dofus instance number {i} connected !");
                        conectFound = true;
                        doOnce = true;
                    }
                    if (doOnce)
                    {
                        App.logstream.Log($"or the play button (x:{dof.playBtn.X},y:{dof.playBtn.Y})");
                        doOnce = false;
                    }
                    if (dof.IsPlayBtn(GetPixel(dof.playBtn)))
                    {
                        App.logstream.Log("Play button found ! No need to connect");
                        App.logstream.Log($"Dofus instance number {i} connected !");
                        break;
                    }
                }
            }

            App.logstream.Log($"Waiting to detect the play button (x:{dofs[0].logBtn.X},y:{dofs[0].logBtn.Y})");
            while (!dofs[0].IsPlayBtn(GetPixel(dofs[0].playBtn)))
            {
                Thread.Sleep(PAUSE * 2);
                SetForegroundWindow(dofs[0].process.MainWindowHandle);
            }

            if (!App.config.StayLog)
                UnlogFromAL(al, input);

            SetForegroundWindow(dofs[0].process.MainWindowHandle);

            App.logstream.Log("Connected and ready to play !");
        }

        #endregion Public Methods

        #region Private Methods

        private static Color GetPixel(Point p)
        {
            Rectangle rect = new Rectangle(p, new Size(2, 2));

            Bitmap map = CaptureFromScreen(rect);

            Color c = map.GetPixel(1, 1);

            map.Dispose();

            return c;
        }

        private static Bitmap CaptureFromScreen(Rectangle rect)
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

        private static void CustomMouseTo(Point p, InputSimulator iS = null)
        {
            if (iS == null)
                iS = new InputSimulator();

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            var X = p.X * 65535 / screenWidth;
            var Y = p.Y * 65535 / screenHeight;

            iS.Mouse.MoveMouseTo(Convert.ToDouble(X), Convert.ToDouble(Y));
        }

        private static void LClickMouseTo(Point p, InputSimulator iS = null)
        {
            if (iS == null)
                iS = new InputSimulator();

            CustomMouseTo(p, iS);
            iS.Mouse.LeftButtonClick().Sleep(PAUSE);
        }

        private static void WriteLogs(Account account, InputSimulator iS = null)
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