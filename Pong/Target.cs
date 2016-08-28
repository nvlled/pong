using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pong
{
    public class Target
    {
        public static Size size = new Size(25, 10);
        public Point pos = new Point(0, 0);
        public Brush brush = Brushes.Aquamarine;

        public Target(int x, int y)
        {
            pos = new Point(x, y);
        }

        public void draw(Graphics g)
        {
            g.FillRectangle(brush, getRekt());
        }

        public Rectangle getRekt()
        {
            return new Rectangle(pos, size);
        }
    }
}