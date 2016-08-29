using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Pong
{
    public interface GameInterface
    {
        void update();
        void draw(Graphics g);

        // TODO: add key and mouse handlers
    }
}
