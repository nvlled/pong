using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Pong
{
    class Ball
    {
        public Point pos;
        public Point speed;

        public int size { get; set; }
        public Ball(int x, int y)
        {
            size = 25;
            pos = new Point(x, y);
            speed = new Point(0, 0);
        }

        public void setSpeed(int x, int y)
        {
            speed.X = x;
            speed.Y = y;
        }

        public Rectangle getBounds()
        {
            return new Rectangle(pos, new Size(size, size));
        }

        public void move()
        {
            pos.X += speed.X;
            pos.Y += speed.Y;
        }

        public void bounceX()
        {
            speed.X *= -1;
        }

        public void bounceY()
        {
            speed.Y *= -1;
        }

        public void draw(Graphics g)
        {
            var b = getBounds();
            //g.DrawRectangle(new Pen(Brushes.Salmon), b);
            g.FillEllipse(Brushes.OrangeRed, b);
        }
    }
}