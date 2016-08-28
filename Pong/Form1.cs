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
        BufferedGraphics grafx;
        public GameState gs;

        public Form1()
        {
            InitializeComponent();
            Cursor.Hide();
            //this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;

            var context = BufferedGraphicsManager.Current;
            grafx = context.Allocate(CreateGraphics(), new Rectangle(0, 0, Width, Height));
        }

        void update() {
            gs.winRect = ClientRectangle;
            gs.currentInterface.update();
        }

        void draw() {
            var g = grafx.Graphics;
            gs.currentInterface.draw(grafx.Graphics);
            grafx.Render(Graphics.FromHwnd(Handle));
        }

        public void t_Tick(object sender, EventArgs e)
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
            if (gs.gameOver)
            {
                gs.gameOver = false;
                gs.levelNo = 0;
                gs.playerLife = 3;
                var play = new PlayInterface(gs);
                gs.currentInterface = play;
                gs.currentLevel = new Level(gs, LevelDefs.levels[gs.levelNo]);
                play.readyBall();
            }
            else
            {
                gs.pause = !gs.pause;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Clip = Bounds;
        }
    }
}