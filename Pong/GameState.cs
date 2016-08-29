using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Pong
{
    public class GameState
    {
        public GameState(Rectangle r) {
            winRect = r;
            paddle = new Paddle(r.Width / 2, r.Height - 30);
            ball = new Ball(0, 0);
        }

        public Ball ball;
        public Paddle paddle;
        public Phys phys = new Phys();
        public GameInterface currentInterface;
        //public Stack<GameInterface> stack = new Stack<GameInterface>();

        public int levelNo = 0;
        public Level currentLevel;

        public bool allowBottom = false;
        public bool showDebugInfo = false;
        public bool gameOver = false;
        public bool pause = false;
        public bool gameFinished = false;
        public int gameSpeed = 33;

        public int ballMaxSpeed = 25;
        public static int startPlayerLife = 5;
        public int playerLife = startPlayerLife;

        public Rectangle winRect;
    }
}
