using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Pong
{
    class Phys
    {
        public float gravity = 1.0f;

        public void moveBall(Ball b)
        {
            b.speed.Y = (int) (b.speed.Y + gravity);
            b.pos += (Size)b.speed;
        }
    }
}
