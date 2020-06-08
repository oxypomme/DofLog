using System;
using System.Diagnostics;
using System.Drawing;

namespace DofLog
{
    internal class Dofus : AppInstance
    {
        #region Private Fields

        private readonly Color logColor = Color.FromArgb(191, 230, 0);
        private readonly Color playColor = Color.FromArgb(191, 230, 0);

        #endregion Private Fields

        #region Public Fields

        public Point usernameField;

        public Point logBtn;
        public Point playBtn;

        #endregion Public Fields

        // Coordinates are measured with a resolution of 1920x1040
        public Dofus(Process pro, Point orig, Size size) : base(pro, orig, size, new Size(1920, 1040))
        {
            usernameField = new Point(
                App.RoundFloat(Origin.X + 915 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 350 * sizeModifier.Y)
            );
            logBtn = new Point(
                App.RoundFloat(Origin.X + 915 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 565 * sizeModifier.Y)
            );
            playBtn = new Point(
                App.RoundFloat(Origin.X + 1150 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 815 * sizeModifier.Y)
            );
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