using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    class PlayInterface : GameInterface
    {
        GameState gs;

        public PlayInterface(GameState gs)
        {
            this.gs = gs;
        }

        public void readyBall()
        {
            gs.ball.pos = new Point(-10, -10);
            gs.currentInterface = new FnInterface()
            {
                DrawAction = delegate(Graphics g)
                {
                    draw(g);
                    drawString(g, "Get Ready!",
                        gs.winRect.Width / 2 - 50,
                        gs.winRect.Height / 2);
                }
            };
            Timer t = new Timer();
            t.Tick += (s, e) =>
            {
                gs.ball.pos.X = gs.paddle.pos.X + gs.paddle.size.Width / 2;
                gs.ball.pos.Y = gs.paddle.pos.Y - 15;
                gs.ball.setSpeed(0, -15);
                t.Stop();
                gs.currentInterface = this;
            };
            t.Interval = 2000;
            t.Start();
            
        }

        public void update()
        {
            if (gs.pause || gs.gameOver)
                return;
            gs.phys.moveBall(gs.ball);
            detectCollision();
            gs.paddle.update();
        }

        void detectCollision()
        {
            var ball = gs.ball;
            var paddle = gs.paddle;
            var winRect = gs.winRect;
            var b = ball.getBounds();
            var pb = paddle.getBounds();

            if (b.Right >= winRect.Width && ball.speed.X > 0)
                ball.speed.X = -ball.speed.X;
            else if (b.Left <= 0 && ball.speed.X < 0)
                ball.speed.X = Math.Abs(ball.speed.X);

            if (b.Bottom >= winRect.Height || b.Top <= 0)
            {
                ball.speed.Y *= -1;
            }

            Target target = gs.currentLevel.getTarget(b);
            if (target != null)
            {
                ball.speed.Y *= -1;
                target.life--;
                if (target.life <= 0)
                {
                    gs.currentLevel.destroy(target);
                    if (gs.currentLevel.isCompleted())
                    {
                        nextLevel();
                    }
                }
            }
            else if (b.Left >= pb.Left && b.Right <= pb.Right &&
                b.Bottom >= pb.Top && ball.speed.Y > 0)
            {
                ball.pos.Y = pb.Top - 10;

                var ps = paddle.getSpeed();
                var xs = ball.speed.X;

                ball.speed.X = (int)((float)xs * 0.5f + ps.X);
                ball.speed.Y = (int)-((float)ball.speed.Y * 1.5f);
                var ys = ball.speed.Y;
                ball.speed = limitSpeed(ball.speed, gs.ballMaxSpeed);
            }
            else if (!gs.allowBottom && b.Bottom >= winRect.Bottom)
            {
                gs.playerLife--;
                if (gs.playerLife <= 0)
                    gs.gameOver = true;
                else
                {
                    readyBall();
                }
            }
            else if (b.Bottom >= winRect.Bottom && ball.speed.Y > 0)
            {
                ball.speed.Y *= -1;
            }
        }

        Point limitSpeed(Point p, int n)
        {
            if (p.Y != 0)
                p.Y = (p.Y / Math.Abs(p.Y)) * Math.Min(Math.Abs(p.Y), n);
            if (p.X != 0)
                p.X = (p.X / Math.Abs(p.X)) * Math.Min(Math.Abs(p.X), n);
            return p;
        }

        void nextLevel()
        {
            if (gs.levelNo >= LevelDefs.levels.Count() - 1)
            {
                gs.gameFinished = true;
            }
            else
            {
                var intf = new FnInterface()
                {
                    UpdateAction = delegate()
                    {
                        gs.currentInterface = this;
                        readyBall();
                    },
                    DrawAction = draw,
                };
                var fade = new FadeInterface(gs, intf);
                fade.OnDecrease = () =>
                {
                    gs.currentLevel = new Level(gs, LevelDefs.levels[++gs.levelNo]);
                };
                gs.currentInterface = fade;
            }
        }

        public void drawPlayerStatus(Graphics g)
        {
            var r = gs.ball.getBounds();
            r.X = 10;
            r.Y = 10;
            for (int i = 0; i < gs.playerLife; i++)
            {
                g.FillEllipse(gs.ball.brush, r);
                r.X += r.Width + 5;
            }
        }

        public void draw(Graphics g)
        {
            var ball = gs.ball;
            var paddle = gs.paddle;

            g.Clear(Color.DarkGray);
            gs.currentLevel.draw(g);
            ball.draw(g);
            paddle.draw(g);
            drawDebugInfo(g);
            drawPlayerStatus(g);

            if (gs.gameFinished)
            {
                drawString(g, "well done",
                    100,
                    gs.winRect.Height / 2);
            }
            else if (gs.gameOver)
            {
                drawString(g, "game over",
                    gs.winRect.Width/2 - 60,
                    gs.winRect.Height / 2);
                drawString(g, "<click or press any key to play again>",
                    gs.winRect.Width/2 - 200,
                    gs.winRect.Height / 2 + 50);
            }
            else if (gs.pause)
            {
                drawString(g, "<paused>",
                    gs.winRect.Width/2 - 60,
                    gs.winRect.Height / 2 + 50);
            }
        }

        void drawDebugInfo(Graphics g)
        {
            var ball = gs.ball;
            var paddle = gs.paddle;
            if (!gs.showDebugInfo)
                return;
            var rekt = ball.getBounds();
            drawString(g,
                String.Format("ball: pos={0} speed={1}",
                    ball.pos,
                    ball.speed),
                20, 5);
            drawString(g,
                String.Format("paddle: pos={0} speed={1}",
                    paddle.pos,
                    paddle.getSpeed()),
                20, 20);
        }

        void drawString(Graphics g, string s, int x, int y)
        {
            g.DrawString(s,
                new Font(FontFamily.GenericSansSerif, 13.5f),
                Brushes.White, new PointF((float)x, (float)y));
        }

    }
}
