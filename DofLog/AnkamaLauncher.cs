using System.Diagnostics;
using System.Drawing;

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

        public Process AL_Process { get; set; }

        public Point usernameField = new Point(Origin.X + 325, Origin.Y + 190);

        public Point fbBtn = new Point(Origin.X + 120, Origin.Y + 450);
        public Point connectBtn = new Point(360 + 295, 160 + 340);
        public Point gamesBtn = new Point(Origin.X + 130, Origin.Y + 50);
        public Point dofusBtn = new Point(Origin.X + 35, Origin.Y + 95);
        public Point startBtn = new Point(Origin.X + 1020, Origin.Y + 590);
        public Point profileBtn = new Point(Origin.X + 1140, Origin.Y + 55);
        public Point unlogBtn = new Point(Origin.X + 1020, Origin.Y + 440);

        #endregion Public Fields

        #region Constructor

        public AnkamaLauncher(Logger df)
        {
            try
            {
                Process[] process = Process.GetProcessesByName("ankama launcher");
                AL_Process = process[process.Length - 1];
            }
            catch (System.IndexOutOfRangeException)
            {
                try
                {
                    Process.Start(df.config.AL_Path);
                    Process[] process = Process.GetProcessesByName("ankama launcher");
                    AL_Process = process[process.Length - 1];
                }
                catch (System.Exception)
                {
                    throw new System.IO.FileNotFoundException();
                }
            }
        }

        #endregion Constructor

        #region Public Methods

        public bool IsFbBtn(Color pixelColor)
        {
            return pixelColor == fbColor;
        }

        public bool IsConnectBtn(Color pixelColor)
        {
            return pixelColor == connectCol;
        }

        public bool IsGamesBtn(Color pixelColor)
        {
            return pixelColor == gamesCol;
        }

        public bool IsStartBtn(Color pixelColor)
        {
            return pixelColor == startCol;
        }

        public bool IsUnlogBtn(Color pixelColor)
        {
            return pixelColor == unlogCol;
        }

        #endregion Public Methods
    }
}