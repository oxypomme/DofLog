using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace DofLog
{
    internal class AnkamaLauncher
    {
        //TODO: En fonction de la taille de l'écran

        #region Private Fields

        private static Point Origin = new Point(360, 160);

        private readonly Color fbColor = Color.FromArgb(59, 89, 152);
        private readonly Color connectCol = Color.FromArgb(255, 168, 44);
        private readonly Color gamesCol = Color.FromArgb(255, 168, 44);
        private readonly Color startCol = Color.FromArgb(255, 255, 255);
        private readonly Color unlogCol = Color.FromArgb(255, 255, 255);

        #endregion Private Fields

        #region Public Fields

        public Process[] AL_Process { get; set; }

        public Point usernameField = new Point(Origin.X + 325, Origin.Y + 190);

        public Point fbBtn = new Point(Origin.X + 120, Origin.Y + 490);
        public Point connectBtn = new Point(Origin.X + 295, Origin.Y + 380);
        public Point gamesBtn = new Point(Origin.X + 130, Origin.Y + 51);
        public Point dofusBtn = new Point(Origin.X + 35, Origin.Y + 95);
        public Point startBtn = new Point(Origin.X + 1020, Origin.Y + 590);
        public Point profileBtn = new Point(Origin.X + 1140, Origin.Y + 55);
        public Point unlogBtn = new Point(Origin.X + 1020, Origin.Y + 440);

        #endregion Public Fields

        #region Constructor

        public AnkamaLauncher()
        {
            AL_Process = Process.GetProcessesByName("ankama launcher");
            if (AL_Process.Length <= 0)
            {
                var startInfo = new ProcessStartInfo(App.config.AL_Path);
                startInfo.WorkingDirectory = Directory.GetParent(App.config.AL_Path).FullName;
                Process.Start(startInfo);
                AL_Process = Process.GetProcessesByName("ankama launcher");
            }
        }

        #endregion Constructor

        #region Public Methods

        public void RecalcCoord(Point orig)
        {
            Origin = orig;

            usernameField = new Point(Origin.X + 325, Origin.Y + 190);

            fbBtn = new Point(Origin.X + 120, Origin.Y + 490);
            connectBtn = new Point(Origin.X + 295, Origin.Y + 380);
            gamesBtn = new Point(Origin.X + 130, Origin.Y + 51);
            dofusBtn = new Point(Origin.X + 35, Origin.Y + 95);
            startBtn = new Point(Origin.X + 1020, Origin.Y + 590);
            profileBtn = new Point(Origin.X + 1140, Origin.Y + 55);
            unlogBtn = new Point(Origin.X + 1020, Origin.Y + 440);
        }

        public bool IsFbBtn(Color pixelColor)
        {
            return App.IsAroundColor(fbColor, pixelColor);
        }

        public bool IsConnectBtn(Color pixelColor)
        {
            return App.IsAroundColor(connectCol, pixelColor);
        }

        public bool IsGamesBtn(Color pixelColor)
        {
            return App.IsAroundColor(gamesCol, pixelColor);
        }

        public bool IsStartBtn(Color pixelColor)
        {
            return App.IsAroundColor(startCol, pixelColor);
        }

        public bool IsUnlogBtn(Color pixelColor)
        {
            return App.IsAroundColor(unlogCol, pixelColor);
        }

        #endregion Public Methods
    }
}