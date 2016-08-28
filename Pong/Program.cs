using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var form = new Pong();
            var gs = new GameState(form.ClientRectangle);
            form.gs = gs;

            var play = new PlayInterface(gs);
            gs.currentInterface = play;
            gs.currentLevel = new Level(gs, LevelDefs.levels[gs.levelNo]);

            var gitRekt = form.ClientRectangle;

            play.readyBall();

            Timer t = new Timer();
            t.Tick += form.t_Tick;
            t.Interval = gs.gameSpeed;
            t.Start();

            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(form);
        }
    }
}
