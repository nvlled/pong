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
        public int life = 2;

        public Target(int x, int y)
        {
            pos = new Point(x, y);
        }

        public void draw(Graphics g)
        {
            var brush = Brushes.MediumBlue;
            switch (life)
            {
                case 3: brush = Brushes.MediumAquamarine; break;
                case 2: brush = Brushes.Aquamarine; break;
                case 1: brush = Brushes.Bisque; break;
            }
            g.FillRectangle(brush, getRekt());
        }

        public Rectangle getRekt()
        {
            return new Rectangle(pos, size);
        }
    }
}