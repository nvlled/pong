using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class Form1 : Form
    {
        Ball ball;
        Paddle paddle;
        Phys phys = new Phys();

        int levelNo = 0;
        Level currentLevel;

        bool allowBottom = true;
        bool showDebugInfo = false;
        bool gameOver = false;
        bool pause = false;
        bool gameFinished = false;
        int gameSpeed = 33;

        BufferedGraphics grafx;

        public Form1()
        {
            InitializeComponent();
            Cursor.Hide();

            currentLevel = new Level(LevelDefs.levels[levelNo]);

            var gitRekt = ClientRectangle;
            paddle = new Paddle(gitRekt.Width / 2, gitRekt.Height - 30);
            ball = new Ball(paddle.pos.X + paddle.size.Width/2, paddle.pos.Y-15);
            ball.setSpeed(0, -15);

            var context = BufferedGraphicsManager.Current;
            grafx = context.Allocate(CreateGraphics(), new Rectangle(0, 0, Width, Height));

            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = gameSpeed;
            t.Start();
        }

        void update()
        {
            if (pause || gameOver)
                return;
            phys.moveBall(ball);
            detectCollision();
            paddle.update();
        }

        void detectCollision()
        {
            var client = ClientRectangle;
            var b = ball.getBounds();
            var pb = paddle.getBounds();

            if (b.Right >= client.Width && ball.speed.X > 0)
                ball.speed.X = -ball.speed.X;
            else if (b.Left <= 0 && ball.speed.X < 0)
                ball.speed.X = Math.Abs(ball.speed.X);

            if (b.Bottom >= client.Height || b.Top <= 0)
            {
                ball.speed.Y *= -1;
            }

            Target target = currentLevel.getTarget(b);
            if (target != null)
            {
                currentLevel.destroy(target);
                ball.speed.Y *= -1;
                if (currentLevel.isCompleted())
                {
                    currentLevel = new Level(LevelDefs.levels[++levelNo]);
                    if (levelNo >= LevelDefs.levels.Count())
                        gameFinished = true;
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
            else if (!allowBottom && b.Bottom >= client.Bottom)
            {
                gameOver = true;
            }
            else if (b.Bottom >= client.Bottom && ball.speed.Y > 0)
            {
                ball.speed.Y *= -1;
            }
        }

        void draw()
        {
            var g = grafx.Graphics;

            g.Clear(Color.DarkGray);
            currentLevel.draw(g);
            ball.draw(g);
            paddle.draw(g);
            drawDebugInfo(g);

            if (gameFinished)
            {
                drawString(g, "well done",
                    100,
                    ClientRectangle.Height / 2);
            }
            else if (gameOver)
            {
                drawString(g, "game over",
                    100,
                    ClientRectangle.Height / 2);
            }

            grafx.Render(Graphics.FromHwnd(Handle));
        }

        void drawDebugInfo(Graphics g)
        {
            if (!showDebugInfo)
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

        void t_Tick(object sender, EventArgs e)
        {
            update();
            draw();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            var x = e.X;
            var r = paddle.getBounds();
            r.X = x;
            if (r.Right > ClientRectangle.Right)
                x = ClientRectangle.Right - r.Width;

            paddle.setPos(new Point(x, r.Y));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //grafx.Render(e.Graphics);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            pause = !pause;
        }
    }
}