using System;
using System.Drawing;
using System.IO;
using System.Windows;

namespace DofLog
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static LogStream logstream = new LogStream(Path.Combine(Environment.CurrentDirectory, "logs.log"));
        internal static Config config = new Config();
        internal static Logger logger = new Logger();

        /// <summary>
        /// Check if a color is nearly the same of another
        /// </summary>
        /// <param name="colorBase">The color that you searched</param>
        /// <param name="colorFound">The color that you find</param>
        /// <param name="tolerance">The tolerance (inclusive), by default 10</param>
        /// <returns>`true` if the color is nearly the same, `false` if doesn't</returns>
        public static bool IsAroundColor(Color colorBase, Color colorFound, int tolerance = 10)
        {
            //logstream.Log(colorFound, "DEBUG");
            if (colorBase.R - tolerance <= colorFound.R && colorFound.R <= colorBase.R + tolerance)
                if (colorBase.G - tolerance <= colorFound.G && colorFound.G <= colorBase.G + tolerance)
                    if (colorBase.B - tolerance <= colorFound.B && colorFound.B <= colorBase.B + tolerance)
                        return true;
            return false;
        }
    }
}