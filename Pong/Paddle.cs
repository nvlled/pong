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
        public Point? lastPos;
        public Size size = new Size(80, 10);

        public int speedCounterMax = 5;
        public int speedCounter = 0;

        public Paddle(int x, int y)
        {
            pos = new Point(x, y);
        }

        public Rectangle getBounds()
        {
            return new Rectangle(pos, size);
        }

        public Point getSpeed()
        {
            if (lastPos == null)
                return new Point(0, 0);
            return pos - (Size)lastPos;
        }

        public void setPos(Point p)
        {
            if (lastPos != null && Math.Abs(lastPos.Value.X) < Math.Abs(p.X))
                lastPos = pos;
            else
                lastPos = pos;
            pos = p;
            speedCounter = speedCounterMax;
        }

        public void update()
        {
            if (speedCounter == 0)
            {
                lastPos = null;
            }
            speedCounter--;
        }

        public void draw(Graphics g)
        {
            g.FillRectangle(Brushes.SteelBlue, new Rectangle(pos, size));
        }
    }
}
