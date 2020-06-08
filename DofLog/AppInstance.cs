using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofLog
{
    public abstract class AppInstance
    {
        protected static Point Origin;
        protected SizeModifier sizeModifier;

        public Process process;

        protected struct SizeModifier
        {
            public float X;
            public float Y;

            public SizeModifier(int x, int y) : this()
            {
                X = x;
                Y = y;
            }
        }

        public AppInstance(Process pro, Point orig, Size size, Size sizeBase)
        {
            process = pro;
            Origin = orig;

            sizeModifier = new SizeModifier(size.Width / sizeBase.Width, size.Height / sizeBase.Height);
        }

        public AppInstance(Point orig, Size size, Size sizeBase)
        {
            Origin = orig;

            sizeModifier = new SizeModifier(size.Width / sizeBase.Width, size.Height / sizeBase.Height);
        }
    }
}