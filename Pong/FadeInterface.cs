using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Pong
{
    class FadeInterface : GameInterface
    {
        GameState gs;
        GameInterface intf;
        public Action OnDecrease { get; set; }
        int dx = 20;

        public FadeInterface(GameState gs, GameInterface intf)
        {
            this.gs = gs;
            this.intf = intf;
            gs.currentInterface = this;
        }

        int radius = 0;
        public void update()
        {
            radius += dx;
            if (radius > gs.winRect.Width)
            {
                dx *= -1;
                if (OnDecrease != null)
                    OnDecrease();
            }
            else if (radius <= 0)
            {
                gs.currentInterface = intf;
            }
        }

        public void draw(Graphics g)
        {
            var r = gs.winRect;
            intf.draw(g);
            g.FillEllipse(Brushes.SeaGreen, r.Width / 2 - radius/2, r.Height / 2 - radius/2,
                radius, radius);
        }
    }
}
