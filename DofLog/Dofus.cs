using System.Diagnostics;
using System.Drawing;

namespace DofLog
{
    internal class Dofus
    {
        //TODO: En fonction de la taille de l'écran
        //TODO: En fonction de la taille de la fenêtre

        #region Private Fields

        private static Point Origin = new Point(0, 0);

        private readonly Color logColor = Color.FromArgb(191, 230, 0);
        private readonly Color playColor = Color.FromArgb(191, 230, 0);

        #endregion Private Fields

        #region Public Fields

        public Point usernameField = new Point(Origin.X + 915, Origin.Y + 350);

        public Point logBtn = new Point(Origin.X + 915, Origin.Y + 565);
        public Point playBtn = new Point(Origin.X + 1150, Origin.Y + 815);

        public Process process;

        #endregion Public Fields

        public Dofus(Process pro, Point orig)
        {
            process = pro;
            Origin = orig;

            usernameField = new Point(Origin.X + 915, Origin.Y + 350);
            logBtn = new Point(Origin.X + 915, Origin.Y + 565);
            playBtn = new Point(Origin.X + 1150, Origin.Y + 815);
        }

        #region Public Methods

        public bool IsLogBtn(Color pixelColor)
        {
            return App.IsAroundColor(logColor, pixelColor);
        }

        public bool IsPlayBtn(Color pixelColor)
        {
            return App.IsAroundColor(playColor, pixelColor);
        }

        #endregion Public Methods
    }
}