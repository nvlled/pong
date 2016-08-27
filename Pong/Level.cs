using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pong
{
    class Level
    {
        Point pos = new Point(20, 20);
        Point margin = new Point(5, 5);

        List<Target> targets;
        public Level(string layout)
        {
            targets = new List<Target>();
            parseLayout(layout);
        }

        public Target getTarget(Rectangle r)
        {
            foreach (var t in targets)
            {
                if (t.getRekt().IntersectsWith(r))
                    return t;
            }
            return null;
        }

        public void destroy(Target t)
        {
            targets.Remove(t);
        }

        public void draw(Graphics g)
        {
            foreach (var t in targets)
            {
                t.draw(g);
            }
        }

        public bool isCompleted()
        {
            return targets.Count() == 0;
        }

        private void parseLayout(string layout)
        {
            int y = 20;
            foreach (var line in layout.Split('\n'))
            {
                var elems = line.Trim().Skip(1);
                int x = 20;
                foreach (var c in elems)
                {
                    if (c != ' ')
                        targets.Add(new Target(x, y));
                    x += Target.size.Width + margin.X ;
                }
                y += Target.size.Height + margin.Y;
            }
        }
    }
}
