using System;
using System.Diagnostics;
using System.Drawing;

namespace DofLog
{
    internal abstract class Dofus : AppInstance
    {
        #region Private Fields

        protected Color logColor;
        protected Color playColor;

        #endregion Private Fields

        #region Public Fields

        public Point usernameField;

        public Point logBtn;
        public Point playBtn;

        #endregion Public Fields

        #region Constructor

        // Coordinates are measured with a resolution of 1920x1040
        public Dofus(Process pro, Point orig, Size size, Size sizeBase) : base(pro, orig, size, sizeBase)
        { }

        #endregion Constructor

        #region Public Methods

        public bool IsLogBtn(Color pixelColor) => App.IsAroundColor(logColor, pixelColor);

        public bool IsPlayBtn(Color pixelColor) => App.IsAroundColor(playColor, pixelColor);

        #endregion Public Methods
    }
}