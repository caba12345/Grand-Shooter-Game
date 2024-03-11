using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caba_Atestat
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, goUp, goDown, gameOver;
        string facing = "up";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int lionSpeed = 3;
        int score = 0;
        Random randNum = new Random();

        List<PictureBox> lionList = new List<PictureBox>();




        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (playerHealth > 1)
            {
                healthBar.Value = playerHealth;
            }
            else
            {
                gameOver = true;
                player.Image = Properties.Resources.dead;
                GameTimer.Stop();
                MessageBox.Show("Ai obtinut scorul de " + score + " puncte. Apasa butonul 'Enter' pentru a juca din nou.");
            }
            txtAmmo.Text = "Munitie: " + ammo;
            txtScore.Text = "Scor: " + score;

            if (goLeft == true && player.Left > 0)
            {
                player.Left -= speed;
            }

            if (goRight == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += speed;
            }

            if (goUp == true && player.Top > 50)
            {
                player.Top -= speed;
            }

            if (goDown == true && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += speed;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;
                    }
                }

                if (x is PictureBox && (string)x.Tag == "lion")
                {
                    if(player.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;
                    }



                    if(x.Left > player.Left)
                    {
                        x.Left -= lionSpeed;
                        ((PictureBox)x).Image = Properties.Resources.lstanga;
                    }
                    if(x.Left < player.Left)
                    {
                        x.Left += lionSpeed;
                        ((PictureBox)x).Image = Properties.Resources.ldreapta;
                    }
                    if (x.Top > player.Top)
                    {
                        x.Top -= lionSpeed;
                        ((PictureBox)x).Image = Properties.Resources.lsus;
                    }
                    if (x.Top < player.Top)
                    {
                        x.Top += lionSpeed;
                        ((PictureBox)x).Image = Properties.Resources.ljos;
                    }
                }

                foreach(Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "arrow" && x is PictureBox && (string)x.Tag == "lion")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++;

                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            lionList.Remove((PictureBox)x);
                            MakeLions();

                        }
                    }

                }

            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(gameOver==true)
            {
                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
                facing = "left";
                player.Image = Properties.Resources.left1;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                facing = "right";
                player.Image = Properties.Resources.right1;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
                facing = "up";
                player.Image = Properties.Resources.up1;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
                facing = "down";
                player.Image = Properties.Resources.down1;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.Space && ammo>0 && gameOver==false)
            {
                ammo--;
                ThrowArrow(facing);

                if (ammo < 1)
                    DropAmmo();
            }

            if(e.KeyCode == Keys.Enter && gameOver==true)
            {
                RestartGame();
            }
        }

        private void ThrowArrow(string direction)
        {
            Arrow throwArrow = new Arrow();
            throwArrow.direction = direction;
            throwArrow.arrowLeft = player.Left + (player.Width / 2);
            throwArrow.arrowTop = player.Top + (player.Height / 2);
            throwArrow.MakeArrow(this);
        }

        private void MakeLions()
        {
            PictureBox lion = new PictureBox();
            lion.Tag = "lion";
            lion.Image = Properties.Resources.ljos;
            lion.Left = randNum.Next(0, 900);
            lion.Top = randNum.Next(0, 800);
            lion.SizeMode = PictureBoxSizeMode.AutoSize;
            lionList.Add(lion);
            this.Controls.Add(lion);
            player.BringToFront();
        }

        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            player.BringToFront();
        }

        private void RestartGame()
        {
            player.Image = Properties.Resources.up1;
            foreach(PictureBox i in lionList)
            {
                this.Controls.Remove(i);
            }

            lionList.Clear();

            for(int i=0; i<4; i++)
            {
                MakeLions();
            }

            goUp = false;
            goDown = false;
            goRight = false;
            goDown = false;
            gameOver = false;

            playerHealth = 100;
            score = 0;
            ammo = 10;

            GameTimer.Start();
        }
    }
}