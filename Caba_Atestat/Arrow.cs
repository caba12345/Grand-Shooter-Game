using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace Caba_Atestat
{
    class Arrow
    {
        public string direction;
        public int arrowLeft;
        public int arrowTop;

        private int speed = 20;
        private PictureBox arrow = new PictureBox();
        private Timer arrowTimer = new Timer();

        public void MakeArrow(Form form)
        {
            arrow.BackColor = Color.Black;
            arrow.Size = new Size(5, 5);
            arrow.Tag = "arrow";
            arrow.Left = arrowLeft;
            arrow.Top = arrowTop;
            arrow.BringToFront();

            form.Controls.Add(arrow);

            arrowTimer.Interval = speed;
            arrowTimer.Tick += new EventHandler(ArrowTimerEvent);
            arrowTimer.Start();

        }
        private void ArrowTimerEvent(object sender, EventArgs e)
        {
            if (direction == "left")
            {
                arrow.Left -= speed;
            }

            if (direction == "right")
            {
                arrow.Left += speed;
            }

            if (direction == "up")
            {
                arrow.Top -= speed;
            }

            if (direction == "down")
            {
                arrow.Top += speed;
            }

            if(arrow.Left<10 || arrow.Left>860 || arrow.Top<10 || arrow.Top>600)
            {
                arrowTimer.Stop();
                arrowTimer.Dispose();
                arrow.Dispose();
                arrowTimer = null;
                arrow = null;
            }

        }
    }
}
