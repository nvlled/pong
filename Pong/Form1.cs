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

        bool pause = false;
        int gameSpeed = 33;

        BufferedGraphics grafx;

        public Form1()
        {
            InitializeComponent();
            Cursor.Hide();

            var gitRekt = ClientRectangle;
            paddle = new Paddle(gitRekt.Width / 2, gitRekt.Height - 80);
            ball = new Ball(gitRekt.Width/2, gitRekt.Height/2);
            ball.setSpeed(10, 15);

            var context = BufferedGraphicsManager.Current;
            grafx =  context.Allocate(CreateGraphics(), new Rectangle(0, 0, Width, Height));

            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = gameSpeed;
            t.Start();
        }


        void update()
        {
            if (pause)
                return;
            ball.move();
            detectCollision();
        }

        void detectCollision()
        {
            var client = ClientRectangle;
            var b = ball.getBounds();
            var pb = paddle.getBounds();

            if (b.Right >= client.Width || b.Left <= 0)
            {
                ball.bounceX();
            }
            if (b.Bottom >= client.Height || b.Top <= 0)
            {
                ball.bounceY();
            }

            if (b.IntersectsWith(pb))
            {
                //ball.bounceX();
                ball.bounceY();
            }
        }

        void draw()
        {
            var g = grafx.Graphics;

            g.Clear(Color.White);
            ball.draw(g);
            paddle.draw(g);
            drawDebugInfo(g);
            
            grafx.Render(Graphics.FromHwnd(Handle));
        }

        void drawDebugInfo(Graphics g)
        {
            var rekt = ball.getBounds();
            drawString(g,
                String.Format("ball pos: X={0}, Y={1}", ball.pos.X, ball.pos.Y),
                20, 5);
            drawString(g,
                String.Format("ball bounds: T={0}, L={1}, R={2}, B={3}",
                rekt.Top, rekt.Left, rekt.Right, rekt.Bottom),
                20, 20);
            drawString(g,
                String.Format("Frame dimension: W={0}, H={1}", Width, Height),
                20, 35);
        }

        void drawString(Graphics g, string s, int x, int y)
        {
            g.DrawString(s,
                new Font(FontFamily.GenericMonospace, 12.5f), 
                Brushes.Black, new PointF((float) x, (float) y));
        }

        void t_Tick(object sender, EventArgs e)
        {
            update();
            draw();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            paddle.pos.X = e.X;
            var r = paddle.getBounds();
            if (r.Right > ClientRectangle.Right)
                paddle.pos.X = ClientRectangle.Right - r.Width;
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