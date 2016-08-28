using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pong
{
    public class Level
    {
        GameState gs;
        Point pos = new Point(20, 20);
        Point margin = new Point(5, 5);
        string layout;

        List<Target> targets;
        public Level(GameState gs, string layout)
        {
            this.gs = gs;
            targets = new List<Target>();
            this.layout = layout;
            parseLayout();
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

        public void parseLayout()
        {
            int h = layout.Split('\n').Count() * (Target.size.Height + margin.Y);
            int w = layout.Split('\n')
                    .Select(line => line.Trim().Count())
                    .Aggregate((a, b) => a > b ? a : b) 
                     * (Target.size.Width + margin.X);

            // TODO: get wxh of layout
            // center layout according to client size

            int y = (gs.winRect.Height - h) / 4;
            int x;
            foreach (var line in layout.Split('\n'))
            {
                var elems = line.Trim().Skip(1);
                x = (gs.winRect.Width - w) / 2;
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
