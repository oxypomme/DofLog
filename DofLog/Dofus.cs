using System.Drawing;

namespace DofLog
{
    internal class Dofus
    {
        //TODO: En fonction de la taille de l'écran
        //TODO: En fonction de la taille de la fenêtre

        #region Private Fields

        private static Point Origin = new Point(0, 0);

        private readonly Color logColor = Color.FromArgb(191, 231, 0);
        private readonly Color playColor = Color.FromArgb(191, 230, 0);

        #endregion Private Fields

        #region Public Fields

        public Point usernameField = new Point(Origin.X + 915, Origin.Y + 350);

        public Point logBtn = new Point(Origin.X + 915, Origin.Y + 565);
        public Point playBtn = new Point(Origin.X + 1150, Origin.Y + 815);

        #endregion Public Fields

        #region Public Methods

        public bool IsLogBtn(Color pixelColor)
        {
            return pixelColor == logColor;
        }

        public bool IsPlayBtn(Color pixelColor)
        {
            return pixelColor == playColor;
        }

        #endregion Public Methods
    }
}