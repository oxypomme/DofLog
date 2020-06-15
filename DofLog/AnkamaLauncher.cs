using System.Diagnostics;
using System.Drawing;

namespace DofLog
{
    internal class AnkamaLauncher : AppInstance
    {
        #region Private Fields

        private readonly Color fbColor = Color.FromArgb(59, 89, 152);
        private readonly Color connectCol = Color.FromArgb(255, 168, 44);
        private readonly Color gamesCol = Color.FromArgb(255, 168, 44);
        private readonly Color startCol = Color.FromArgb(255, 255, 255);
        private readonly Color startRCol = Color.FromArgb(255, 255, 255);
        private readonly Color unlogCol = Color.FromArgb(255, 255, 255);

        #endregion Private Fields

        #region Public Fields

        public new Process[] process { get; set; }

        public Point usernameField;

        public Point fbBtn;
        public Point connectBtn;
        public Point gamesBtn;
        public Point dofusBtn;
        public Point dofusRBtn;
        public Point startBtn;
        public Point profileBtn;
        public Point unlogBtn;

        #endregion Public Fields

        #region Constructor

        public AnkamaLauncher(Process[] pro, Point orig, Size size) : base(orig)
        {
            process = pro;

            usernameField = new Point(
                App.RoundFloat(Origin.X + 325),
                App.RoundFloat(Origin.Y + 190)
            );

            fbBtn = new Point(
                App.RoundFloat(Origin.X + 120),
                App.RoundFloat(Origin.Y + 490)
            );
            connectBtn = new Point(
                App.RoundFloat(Origin.X + 295),
                App.RoundFloat(Origin.Y + 380)
            );
            gamesBtn = new Point(
                App.RoundFloat(Origin.X + 130),
                App.RoundFloat(Origin.Y + 51)
            );
            dofusBtn = new Point(
                App.RoundFloat(Origin.X + 35),
                App.RoundFloat(Origin.Y + 95)
            );
            dofusRBtn = new Point(
                App.RoundFloat(Origin.X + 35),
                App.RoundFloat(Origin.Y + 165)
            );
            startBtn = new Point(
                App.RoundFloat(Origin.X + size.Width - 180),
                App.RoundFloat(Origin.Y + size.Height - 130)
            );
            profileBtn = new Point(
                App.RoundFloat(Origin.X + size.Width - 60),
                App.RoundFloat(Origin.Y + 55)
            );
            unlogBtn = new Point(
                App.RoundFloat(Origin.X + size.Width - 180),
                App.RoundFloat(Origin.Y + 440)
            );
        }

        #endregion Constructor

        #region Public Methods

        public bool IsFbBtn(Color pixelColor) => App.IsAroundColor(fbColor, pixelColor);

        public bool IsConnectBtn(Color pixelColor) => App.IsAroundColor(connectCol, pixelColor);

        public bool IsGamesBtn(Color pixelColor) => App.IsAroundColor(gamesCol, pixelColor);

        public bool IsStartBtn(Color pixelColor) => App.IsAroundColor(startCol, pixelColor);

        public bool IsStartRBtn(Color pixelColor) => App.IsAroundColor(startRCol, pixelColor);

        public bool IsUnlogBtn(Color pixelColor) => App.IsAroundColor(unlogCol, pixelColor);

        #endregion Public Methods
    }
}