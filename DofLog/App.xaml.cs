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
        public static LogStream logstream;
        internal static Config config;

        public App()
        {
            logstream = new LogStream(Path.Combine(Environment.CurrentDirectory, "logs.log"));

            config = new Config();
            config.GenConfig();
        }

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

        /// <summary>
        /// Round a float and converts it to an int
        /// </summary>
        /// <param name="value">The float</param>
        /// <returns>An int</returns>
        public static int RoundFloat(float value)
        {
            return Convert.ToInt32(Math.Round(value));
        }

        public static void ReloadTheme()
        {
            Current.Resources.MergedDictionaries.Clear();

            var ressource = new ResourceDictionary();
            if (config.RetroMode)
            {
                ressource.Source = new Uri("pack://application:,,,/DofLog;component/Themes/DofusRetro/DofusRetro.xaml", UriKind.RelativeOrAbsolute);
                Current.Resources.MergedDictionaries.Add(ressource);
            }
            else
            {
                ressource.Source = new Uri("pack://application:,,,/DofLog;component/Themes/DofusDark/DofusDark.xaml", UriKind.RelativeOrAbsolute);
                Current.Resources.MergedDictionaries.Add(ressource);
            }

            ressource = new ResourceDictionary();

            ressource.Source = new Uri("pack://application:,,,/DofLog;component/Themes/Dofus.xaml", UriKind.RelativeOrAbsolute);
            Current.Resources.MergedDictionaries.Add(ressource);
        }
    }
}