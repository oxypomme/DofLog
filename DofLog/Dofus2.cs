using System.Diagnostics;
using System.Drawing;

namespace DofLog
{
    internal class Dofus2 : Dofus
    {
        #region Constructor

        // Coordinates are measured with a resolution of 1920x1040
        public Dofus2(Process pro, Point orig, Size size) : base(pro, orig, size, new Size(1920, 1040))
        {
            logColor = Color.FromArgb(191, 230, 0);
            playColor = Color.FromArgb(191, 230, 0);

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

        #endregion Constructor
    }
}