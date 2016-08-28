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
        GameState gs;
        BufferedGraphics grafx;

        public Form1()
        {
            InitializeComponent();
            Cursor.Hide();

            gs = new GameState(ClientRectangle);

            var play = new PlayInterface(gs);
            gs.currentInterface = play;
            gs.currentLevel = new Level(LevelDefs.levels[gs.levelNo]);

            var gitRekt = ClientRectangle;

            var context = BufferedGraphicsManager.Current;
            grafx = context.Allocate(CreateGraphics(), new Rectangle(0, 0, Width, Height));

            play.readyBall();

            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = gs.gameSpeed;
            t.Start();
        }



        void update() {
            gs.currentInterface.update();
        }

        void draw() {
            var g = grafx.Graphics;
            gs.currentInterface.draw(grafx.Graphics);
            grafx.Render(Graphics.FromHwnd(Handle));
        }

        void t_Tick(object sender, EventArgs e)
        {
            update();
            draw();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            var x = e.X;
            var r = gs.paddle.getBounds();
            r.X = x;
            if (r.Right > ClientRectangle.Right)
                x = ClientRectangle.Right - r.Width;

            gs.paddle.setPos(new Point(x, r.Y));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //grafx.Render(e.Graphics);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            gs.pause = !gs.pause;
        }
    }
}