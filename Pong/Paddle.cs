using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pong
{
    class Paddle
    {
        public Point pos;
        public Size size = new Size(70, 20);

        public Paddle(int x, int y)
        {
            pos = new Point(x, y);
        }

        public Rectangle getBounds()
        {
            return new Rectangle(pos, size);
        }

        public void draw(Graphics g)
        {
            g.FillRectangle(Brushes.SaddleBrown, new Rectangle(pos, size));
        }
    }
}
