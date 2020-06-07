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

        public static bool IsAroundColor(Color colorBase, Color colorFound)
        {
            //logstream.Log(colorFound, "DEBUG");
            if (colorBase.R - 10 <= colorFound.R && colorFound.R <= colorBase.R + 10)
                if (colorBase.G - 10 <= colorFound.G && colorFound.G <= colorBase.G + 10)
                    if (colorBase.B - 10 <= colorFound.B && colorFound.B <= colorBase.B + 10)
                        return true;
            return false;
        }
    }
}