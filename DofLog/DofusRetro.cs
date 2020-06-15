using System.Diagnostics;
using System.Drawing;

namespace DofLog
{
    internal class DofusRetro : Dofus
    {
        #region Constructor

        public DofusRetro(Process pro, Point orig, Size size) : base(pro, orig, size, new Size(758, 615))
        {
            logColor = Color.FromArgb(255, 153, 0);
            playColor = Color.FromArgb(255, 153, 0);

            usernameField = new Point(
                App.RoundFloat(Origin.X + 212 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 243 * sizeModifier.Y)
            );
            logBtn = new Point(
                App.RoundFloat(Origin.X + 162 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 376 * sizeModifier.Y)
            );
            playBtn = new Point(
                App.RoundFloat(Origin.X + 392 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 498 * sizeModifier.Y)
            );
        }

        #endregion Constructor
    }
}