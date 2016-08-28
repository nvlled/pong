using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Pong
{
    class FnInterface : GameInterface
    {
        public Action UpdateAction { get; set; }
        public Action<Graphics> DrawAction { get; set; }

        public void update()
        {
            if (UpdateAction != null)
                UpdateAction();
        }

        public void draw(Graphics g)
        {
            if (DrawAction != null)
                DrawAction(g);
        }
    }
}
