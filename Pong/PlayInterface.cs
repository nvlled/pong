using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

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
            gs.ball.pos.X = gs.paddle.pos.X + gs.paddle.size.Width / 2;
            gs.ball.pos.Y = gs.paddle.pos.Y - 15;
            gs.ball.setSpeed(0, -15);
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
                gs.currentLevel.destroy(target);
                ball.speed.Y *= -1;
                if (gs.currentLevel.isCompleted())
                {
                    nextLevel();
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
                ball.speed.Y = (ys / Math.Abs(ys)) * Math.Min(Math.Abs(ys), 30);
            }
            else if (!gs.allowBottom && b.Bottom >= winRect.Bottom)
            {
                gs.gameOver = true;
            }
            else if (b.Bottom >= winRect.Bottom && ball.speed.Y > 0)
            {
                ball.speed.Y *= -1;
            }
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
                    gs.currentLevel = new Level(LevelDefs.levels[++gs.levelNo]);
                };
                gs.currentInterface = fade;
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

            if (gs.gameFinished)
            {
                drawString(g, "well done",
                    100,
                    gs.winRect.Height / 2);
            }
            else if (gs.gameOver)
            {
                drawString(g, "game over",
                    100,
                    gs.winRect.Height / 2);
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
                new Font(FontFamily.GenericMonospace, 12.5f),
                Brushes.Black, new PointF((float)x, (float)y));
        }

    }
}
