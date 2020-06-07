using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
