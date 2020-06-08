using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace DofLog
{
    internal class AnkamaLauncher : AppInstance
    {
        #region Private Fields

        private readonly Color fbColor = Color.FromArgb(59, 89, 152);
        private readonly Color connectCol = Color.FromArgb(255, 168, 44);
        private readonly Color gamesCol = Color.FromArgb(255, 168, 44);
        private readonly Color startCol = Color.FromArgb(255, 255, 255);
        private readonly Color unlogCol = Color.FromArgb(255, 255, 255);

        #endregion Private Fields

        #region Public Fields

        public new Process[] process { get; set; }

        public Point usernameField;

        public Point fbBtn;
        public Point connectBtn;
        public Point gamesBtn;
        public Point dofusBtn;
        public Point startBtn;
        public Point profileBtn;
        public Point unlogBtn;

        #endregion Public Fields

        #region Constructor

        public AnkamaLauncher(Process[] pro, Point orig, Size size) : base(orig, size, new Size(1200, 720))
        {
            process = pro;

            usernameField = new Point(
                App.RoundFloat(Origin.X + 325 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 190 * sizeModifier.Y)
            );

            fbBtn = new Point(
                App.RoundFloat(Origin.X + 120 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 490 * sizeModifier.Y)
            );
            connectBtn = new Point(
                App.RoundFloat(Origin.X + 295 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 380 * sizeModifier.Y)
            );
            gamesBtn = new Point(
                App.RoundFloat(Origin.X + 130 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 51 * sizeModifier.Y)
            );
            dofusBtn = new Point(
                App.RoundFloat(Origin.X + 35 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 95 * sizeModifier.Y)
            );
            startBtn = new Point(
                App.RoundFloat(Origin.X + 1020 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 590 * sizeModifier.Y)
            );
            profileBtn = new Point(
                App.RoundFloat(Origin.X + 1140 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 55 * sizeModifier.Y)
            );
            unlogBtn = new Point(
                App.RoundFloat(Origin.X + 1020 * sizeModifier.X),
                App.RoundFloat(Origin.Y + 440 * sizeModifier.Y)
            );
        }

        #endregion Constructor

        #region Public Methods

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