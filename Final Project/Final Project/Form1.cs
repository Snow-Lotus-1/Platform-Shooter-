/*
Jason lu
Jan 12 2018
Final Project
this is a program to show bools, if statements, float, rise & run, custom timers, and the rest of our knowledge over the semester
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Final_Project
{
    public partial class Form1 : Form
    {
        //sound/music
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        WindowsMediaPlayer cut = new WindowsMediaPlayer();
        WindowsMediaPlayer fire = new WindowsMediaPlayer();
        WindowsMediaPlayer dragon = new WindowsMediaPlayer();
        WindowsMediaPlayer gust = new WindowsMediaPlayer();

        //show if win or lose screen is active
        bool screenOn = false;

        //player movement
        bool left = false, right = false;
        bool jump;
        int G = 30;
        int Force;
        float speed = 8;

        //enemy movement
        bool enemyLeft, enemyRight;
        bool enemyJump;
        int EG = 30;
        int EForce;
        int enemyx;
        int enemyy;
        int enemySpeedx = 5;

        //Stop right walk animation
        bool rightWalkStop;

        //Stop left walk animation
        bool leftWalkStop;

        //Stop idle animation
        bool idleStop;

        //Sword bools
        bool rightSword;
        bool leftSword;

        //projectile enemy
        RectangleF projectileBox;
        SizeF projectileSize;
        PointF projectileLocation;
        float projectileX, projectileY;
        float projectileXSpeed, projectileYSpeed;
        bool isProjectileInMotion;
        float rise, run;
        float hypotenuse;
        int PROJECTILE_SPEED = 8;

        //boomerang part 1
        RectangleF boomerangBox;
        SizeF boomerangSize;
        PointF boomerangLocation;
        float boomerangX, boomerangY;
        float boomerangXSpeed, boomerangYSpeed;
        bool isBoomerangInMotion;
        float bRise, bRun;
        float bHypotenuse;
        const int BOOMERANG_SPEED = 6;
        bool throwBoomerang = false;
        bool boomerangReturn;

        // Counter 
        bool keepRunning = false;
        int timeCounter = 0; //counts seconds passed
        int timePassedSinceLastSecond = 0; //this is the fraction of seconds

        //animation
        int animationFrameCounter = 0;

        //graphic info Hp Bar        
        PointF hpLocation;
        SizeF hpSize;
        RectangleF hpBox;
        Font hpFont;
        const string HP_STRING = "HP";
        int Hp = 110;

        //graphic info Hp Bar        
        PointF eHpLocation;
        SizeF eHpSize;
        RectangleF eHpBox;
        Font eHpFont;
        const string EHP_STRING = "HP";
        int EHp = 100;

        //graphic info player
        PointF playerLocation;
        SizeF playerSize;
        RectangleF playerBox;

        //graphic info level
        PointF levelLocation;
        SizeF levelSize;
        RectangleF levelBox;

        //graphic info platform1
        PointF platform1Location;
        SizeF platform1Size;
        RectangleF platform1Box;

        //graphic info platform2
        PointF platform2Location;
        SizeF platform2Size;
        RectangleF platform2Box;

        //graphic info background
        PointF backgroundLocation;
        SizeF backgroundSize;
        RectangleF backgroundBox;

        //graphic info enemy
        PointF enemyLocation;
        SizeF enemySize;
        RectangleF enemyBox;

        //graphic info sword right
        PointF swordRightLocation;
        SizeF swordRightSize;
        RectangleF swordRightBox;

        //graphic info sword left
        PointF swordLeftLocation;
        SizeF swordLeftSize;
        RectangleF swordLeftBox;

        //graphic info HpBar
        PointF hpBarLocation;
        SizeF hpBarSize;
        RectangleF hpBarBox;

        //graphic info enemy HpBar
        PointF eHpBarLocation;
        SizeF eHpBarSize;
        RectangleF eHpBarBox;

        //graphic info start screen
        PointF sScreenLocation;
        SizeF sScreenSize;
        RectangleF sScreenBox;

        //graphic info lose screen
        PointF lScreenLocation;
        SizeF lScreenSize;
        RectangleF lScreenBox;

        //graphic info win screen
        PointF wScreenLocation;
        SizeF wScreenSize;
        RectangleF wScreenBox;

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            SetupGraphics();
            SetUpHP();

            //sound/music          
            player.URL = "myMusicmp3.mp3";
            cut.URL = "Slash.mp3";
            fire.URL = "Shoot.mp3";
            dragon.URL = "Dragon.mp3";
            gust.URL = "Boomerang.mp3";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //quit game
            if (e.KeyCode == Keys.Escape && screenOn == true)
            {
                this.Close();
            }

            //restart game
            if (e.KeyCode == Keys.R && screenOn == true)
            {
                Application.Restart();
                Environment.Exit(0);
            }

            // assign "Space" key to throw boomerang
            if (e.KeyCode == Keys.Space && leftSword == false && rightSword == false)
            {
                throwBoomerang = true;
                CreateNewBoomerang();
            }

            // assign "Right" key to move player right
            if (e.KeyCode == Keys.Right && leftSword == false && rightSword == false)
            {
                right = true;                
            }
            // assign "Left" key to move player left
            else if (e.KeyCode == Keys.Left && leftSword == false && rightSword == false)
            {
                left = true;
            }
            // assign "A" key to make player attack left side
            else if (e.KeyCode == Keys.A && left == false && right == false && rightSword == false && isBoomerangInMotion == false && boomerangReturn == false)
            {
                leftSword = true;               
            }
            // assign "D" key to make player attack right side
            else if (e.KeyCode == Keys.D && left == false && right == false && leftSword == false && isBoomerangInMotion == false && boomerangReturn == false)
            {
                rightSword = true;
            }

            // assign "Up" key to make player jump/move player up
            if (jump != true)
            {
                if (e.KeyCode == Keys.Up)
                {
                    jump = true;
                    Force = G;
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                throwBoomerang = false;               
            }

            if (e.KeyCode == Keys.Right)
            {
                right = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                left = false;
            }
            else if (e.KeyCode == Keys.A)
            {
                leftSword = false;

                if (screenOn == false && left == false && right == false)
                {
                    cut.controls.play();
                }

                // enemy loses hp when sword intersects
                if (swordLeftBox.IntersectsWith(enemyBox))
                {
                    EHp = EHp - 10;

                    // change enemys attack to make game more fun
                    if (EHp <= 70 && EHp >= 50)
                    {
                        //slows enemy but increase fire rate by alot
                        enemySpeedx = 1;
                        PROJECTILE_SPEED = 20;
                    }
                    else if (EHp <= 55 && EHp >= 35)
                    {
                        //bring enemy sts back too avg
                        enemySpeedx = 8;
                        PROJECTILE_SPEED = 8;
                    }                  
                    else if (EHp <= 30)
                    {
                        //enemys stats increase again
                        enemySpeedx = 15;
                        PROJECTILE_SPEED = 15;
                    }

                    if (EHp == 30 || EHp == 70 || EHp == 55)
                    {
                        //tells player that enemy will change attack patterns
                        dragon.controls.play();
                    }

                    // stop projectile if enemy is dead
                    if (EHp <= 0)
                    {
                        System.Threading.Thread.Sleep(500);
                        PROJECTILE_SPEED = 0;
                    }
                }
            }
            else if (e.KeyCode == Keys.D)
            {
                rightSword = false;

                if (screenOn == false && left == false && right == false)
                {
                    cut.controls.play();
                }

                // enemy loses hp when sword intersects
                if (swordRightBox.IntersectsWith(enemyBox))
                {
                    EHp = EHp - 10;

                    // change enemys attack to make game more fun
                    if (EHp <= 70 && EHp >= 50)
                    {
                        enemySpeedx = 1;
                        PROJECTILE_SPEED = 30;
                    }
                    else if (EHp <= 55 && EHp >= 35)
                    {
                        enemySpeedx = 8;
                        PROJECTILE_SPEED = 8;
                    }
                    else if (EHp <= 30)
                    {
                        enemySpeedx = 15;
                        PROJECTILE_SPEED = 15;
                    }

                    if (EHp == 30 || EHp == 70 || EHp == 55)
                    {
                        dragon.controls.play();
                    }

                    // stop projectile if enemy is dead
                    if (EHp <= 0)
                    {
                        System.Threading.Thread.Sleep(500);
                        PROJECTILE_SPEED = 0;
                    }
                }
            }
        }

        private void tmrMove_Tick(object sender, EventArgs e)
        {

        }

        // setup all the game graphics
        void SetupGraphics()
        {
            // used to draw the level (area you can move in) graphics 
            levelSize = new SizeF(1386, 580);
            levelLocation = new PointF(0, 0);
            levelBox = new RectangleF(levelLocation, levelSize);

            // used to draw the player's graphics 
            playerSize = new SizeF(60, 60);
            // start at the top-left part of the screen
            playerLocation = new PointF(0, 0);
            playerBox = new RectangleF(playerLocation, playerSize);

            // used to draw the background graphics 
            backgroundSize = new SizeF(1402, 664);
            backgroundLocation = new PointF(0, 0);
            backgroundBox = new RectangleF(backgroundLocation, backgroundSize);

            // used to draw the platform1 graphics         
            platform1Size = new SizeF(567, 326);
            platform1Location = new PointF(820, 181);
            platform1Box = new RectangleF(platform1Location, platform1Size);

            // used to draw the platform2 graphics
            platform2Size = new SizeF(375, 144);
            platform2Location = new PointF(212, 294);
            platform2Box = new RectangleF(platform2Location, platform2Size);

            // used to draw the enemy's graphics
            enemySize = new SizeF(150, 123);
            enemyLocation = new PointF(1000, 60);
            enemyBox = new RectangleF(enemyLocation, enemySize);
            enemyx = 1000;

            // used to draw the Hp Bar graphics
            hpBarSize = new SizeF(200, 100);
            hpBarLocation = new PointF(0, 0);
            hpBarBox = new RectangleF(hpBarLocation, hpBarSize);

            // used to draw the enemy's Hp Bar graphics
            eHpBarSize = new SizeF(200, 100);
            eHpBarLocation = new PointF(1180, 0);
            eHpBarBox = new RectangleF(eHpBarLocation, eHpBarSize);

            // used to draw the win screen graphics
            wScreenSize = new SizeF(1402, 665);
            wScreenLocation = new PointF(0, 0);
            wScreenBox = new RectangleF(wScreenLocation, wScreenSize);

            // used to draw the lose screen graphics
            lScreenSize = new SizeF(1402, 665);
            lScreenLocation = new PointF(0, 0);
            lScreenBox = new RectangleF(lScreenLocation, lScreenSize);

            // used to draw the start screen graphics
            sScreenSize = new SizeF(1402, 665);
            sScreenLocation = new PointF(0, 0);
            sScreenBox = new RectangleF(sScreenLocation, sScreenSize);

            // used to draw the projectile graphics
            projectileSize = new SizeF(40, 40);
            projectileBox = new RectangleF();
            projectileBox.Size = projectileSize;

            // used to draw boomerang graphics
            boomerangSize = new SizeF(60, 60);
            boomerangBox = new RectangleF();
            boomerangBox.Size = boomerangSize;
        }

        void SetUpSword()
        {
            if (leftSword == true)
            {
                // used to draw the left sword graphics
                swordLeftSize = new SizeF(75, 75);
                swordLeftLocation = new PointF(playerBox.X - 70, playerBox.Y - 40);
                swordLeftBox = new RectangleF(swordLeftLocation, swordLeftSize);
            }
            if (leftSword == false)
            {
                // used to draw the left sword graphics off screen
                swordLeftSize = new SizeF(75, 75);
                swordLeftLocation = new PointF(playerBox.X + 10000, playerBox.Y - 40);
                swordLeftBox = new RectangleF(swordLeftLocation, swordLeftSize);
            }
            if (rightSword == true)
            {
                // used to draw the right sword graphics
                swordRightSize = new SizeF(75, 75);
                swordRightLocation = new PointF(playerBox.X + 55, playerBox.Y - 40);
                swordRightBox = new RectangleF(swordRightLocation, swordRightSize);               
            }
            if (rightSword == false)
            {
                // used to draw the right sword graphics off screen
                swordRightSize = new SizeF(75, 75);
                swordRightLocation = new PointF(playerBox.X - 10000, playerBox.Y - 40);
                swordRightBox = new RectangleF(swordRightLocation, swordRightSize);
            }
        }

        void SetUpHP()
        {
            // used to draw the hp tag above player
            hpLocation = new PointF(playerBox.X + 10, playerBox.Y - 20);
            hpSize = new SizeF(500, 200);
            hpBox = new RectangleF(hpLocation, hpSize);
            hpFont = new Font("ComicSans", 10);

            // used to draw the hp tag above enemy
            eHpLocation = new PointF(enemyBox.X + 10, enemyBox.Y - 20);
            eHpSize = new SizeF(500, 200);
            eHpBox = new RectangleF(eHpLocation, eHpSize);
            eHpFont = new Font("ComicSans", 10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(Properties.Resources.Background, backgroundBox);
            e.Graphics.DrawImage(Properties.Resources.Platform1, platform1Box);
            e.Graphics.DrawImage(Properties.Resources.Platform2, platform2Box);                               

            if (isBoomerangInMotion == true)
            {
                //plays woosh sound so it sounds like boomerang is in the air
                gust.controls.play();

                if (animationFrameCounter >= 0 && animationFrameCounter < 5)
                {
                    e.Graphics.DrawImage(Properties.Resources.BoomerangFrame1, boomerangBox);
                }
                else if (animationFrameCounter >= 10 && animationFrameCounter < 15)
                {
                    e.Graphics.DrawImage(Properties.Resources.BoomerangFrame2, boomerangBox);
                }
                else if (animationFrameCounter >= 15 && animationFrameCounter < 20)
                {
                    e.Graphics.DrawImage(Properties.Resources.BoomerangFrame3, boomerangBox);
                }
                else if (animationFrameCounter >= 25 && animationFrameCounter < 30)
                {
                    e.Graphics.DrawImage(Properties.Resources.BoomerangFrame4, boomerangBox);
                }
                else if (animationFrameCounter >= 30 && animationFrameCounter < 35)
                {
                    e.Graphics.DrawImage(Properties.Resources.BoomerangFrame3, boomerangBox);
                }
                else if (animationFrameCounter >= 40 && animationFrameCounter < 45)
                {
                    e.Graphics.DrawImage(Properties.Resources.BoomerangFrame2, boomerangBox);
                }
            }
                
            // projectile animation
            if (isProjectileInMotion == true)
            {
                if (animationFrameCounter >= 0 && animationFrameCounter < 5)
                {
                    e.Graphics.DrawImage(Properties.Resources.ProjectileFrame1, projectileBox);
                }
                else if (animationFrameCounter >= 5 && animationFrameCounter < 10)
                {
                    e.Graphics.DrawImage(Properties.Resources.ProjectileFrame2, projectileBox);
                }
                else if (animationFrameCounter >= 10 && animationFrameCounter < 15)
                {
                    e.Graphics.DrawImage(Properties.Resources.ProjectileFrame3, projectileBox);
                }
                else if (animationFrameCounter >= 15 && animationFrameCounter < 20)
                {
                    e.Graphics.DrawImage(Properties.Resources.ProjectileFrame2, projectileBox);
                }
            }
          
            // enemy animations           
            if (playerBox.X <= enemyBox.X)//face player
            {
                if (animationFrameCounter >= 0 && animationFrameCounter < 5)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleLeftFrame1, enemyBox);
                }
                else if (animationFrameCounter >= 5 && animationFrameCounter < 10)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleLeftFrame2, enemyBox);
                }
                else if (animationFrameCounter >= 10 && animationFrameCounter < 15)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleLeftFrame3, enemyBox);
                }
                else if (animationFrameCounter >= 15 && animationFrameCounter < 20)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleLeftFrame4, enemyBox);
                }
                else if (animationFrameCounter >= 20 && animationFrameCounter < 25)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleLeftFrame3, enemyBox);
                }
                else if (animationFrameCounter >= 25 && animationFrameCounter < 30)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleLeftFrame2, enemyBox);
                }
            }

            if (playerBox.X >= enemyBox.X)//face player
            {
                if (animationFrameCounter >= 0 && animationFrameCounter < 5)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleRightFrame1, enemyBox);
                }
                else if (animationFrameCounter >= 5 && animationFrameCounter < 10)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleRightFrame2, enemyBox);
                }
                else if (animationFrameCounter >= 10 && animationFrameCounter < 15)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleRightFrame3, enemyBox);
                }
                else if (animationFrameCounter >= 15 && animationFrameCounter < 20)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleRightFrame4, enemyBox);
                }
                else if (animationFrameCounter >= 20 && animationFrameCounter < 25)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleRightFrame3, enemyBox);
                }
                else if (animationFrameCounter >= 25 && animationFrameCounter < 30)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyIdleRightFrame2, enemyBox);
                }
            }

            //drawing the swords
            e.Graphics.DrawImage(Properties.Resources.SwordRight, swordRightBox);
            e.Graphics.DrawImage(Properties.Resources.SwordLeft, swordLeftBox);

            //player animation walk left
            if (left == true)
            {
                if (animationFrameCounter >= 0 && animationFrameCounter < 10 && left == true && right == false)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeftWalkFrame1, playerBox);
                    rightWalkStop = false;
                    idleStop = false;
                    leftWalkStop = true;
                }
                else if (animationFrameCounter >= 10 && animationFrameCounter < 20)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeftWalkFrame2, playerBox);
                    rightWalkStop = false;
                    idleStop = false;
                    leftWalkStop = true;
                }
                else if (animationFrameCounter >= 20 && animationFrameCounter < 30)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeftWalkFrame3, playerBox);
                    rightWalkStop = false;
                    idleStop = false;
                    leftWalkStop = true;
                }
                else if (animationFrameCounter >= 30 && animationFrameCounter < 40)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeftWalkFrame2, playerBox);
                    rightWalkStop = false;
                    idleStop = false;
                    leftWalkStop = true;
                }
            }

            //player animation walk right
            if (right == true)
            {
                if (rightWalkStop == true && leftWalkStop == false && idleStop == false && animationFrameCounter >= 0 && animationFrameCounter < 10 && right == true && left == false)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRightWalkFrame1, playerBox);
                    leftWalkStop = false;
                    idleStop = false;
                    rightWalkStop = true;
                }
                else if (animationFrameCounter >= 10 && animationFrameCounter < 20)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRightWalkFrame2, playerBox);
                    leftWalkStop = false;
                    idleStop = false;
                    rightWalkStop = true;
                }
                else if (animationFrameCounter >= 20 && animationFrameCounter < 30)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRightWalkFrame3, playerBox);
                    leftWalkStop = false;
                    idleStop = false;
                    rightWalkStop = true;
                }
                else if (animationFrameCounter >= 30 && animationFrameCounter < 40)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRightWalkFrame2, playerBox);
                    leftWalkStop = false;
                    idleStop = false;
                    rightWalkStop = true;
                }
            }

            //player animation idle right
            if (right == false && left == false && playerBox.X <= enemyBox.X)
            {
                if (animationFrameCounter >= 0 && animationFrameCounter < 15 && left == false && right == false)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRightIdleFrame1, playerBox);
                    rightWalkStop = false;
                    leftWalkStop = false;

                }
                else if (animationFrameCounter >= 15 && animationFrameCounter < 30 && left == false && right == false)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRightIdleFrame2, playerBox);
                    rightWalkStop = false;
                    leftWalkStop = false;

                }
            }

            //player animation idle left
            if (right == false && left == false && playerBox.X >= enemyBox.X)
            {
                if (animationFrameCounter >= 0 && animationFrameCounter < 15 && left == false && right == false)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeftIdleFrame1, playerBox);
                    rightWalkStop = false;
                    leftWalkStop = false;

                }
                else if (animationFrameCounter >= 15 && animationFrameCounter < 30 && left == false && right == false)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeftIdleFrame2, playerBox);
                    rightWalkStop = false;
                    leftWalkStop = false;

                }
            }

            // draw the moving hp tabs
            e.Graphics.DrawString(HP_STRING + Hp, hpFont, Brushes.Red, hpBox);
            e.Graphics.DrawString(EHP_STRING + EHp, eHpFont, Brushes.Red, eHpBox);

            //Hp bar in corner
            if (Hp == 100)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar1, hpBarBox);
            }

            else if (Hp <= 90 && Hp >= 70)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar2, hpBarBox);
            }

            else if (Hp <= 60 && Hp >= 40)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar3, hpBarBox);
            }

            else if (Hp <= 30 && Hp >= 10)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar4, hpBarBox);
            }
            else if (Hp <= 0)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar5, hpBarBox);
            }

            // enemy hp bar in other corner
            if (EHp == 100)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar1, eHpBarBox);
            }

            else if (EHp <= 95 && EHp >= 70)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar2, eHpBarBox);
            }

            else if (EHp <= 65 && EHp >= 35)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar3, eHpBarBox);
            }

            else if (EHp <= 30 && EHp >= 5)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar4, eHpBarBox);
            }
            else if (EHp <= 0)
            {
                e.Graphics.DrawImage(Properties.Resources.HealthBar5, eHpBarBox);
            }

            //show win screen if player hp is above 10 and enemy hp is lower than or equal too 0
            if (EHp <= 0)
            {
                System.Threading.Thread.Sleep(1000);
                e.Graphics.DrawImage(Properties.Resources.WinScreen, wScreenBox);
                screenOn = true;

                if (screenOn == true)
                {
                    isProjectileInMotion = false;
                }
            }

            //show lose screen if enemy hp is above 10 and player hp is lower than or equal too 0
            if (Hp <= 0)
            {
                System.Threading.Thread.Sleep(1000);
                e.Graphics.DrawImage(Properties.Resources.LoseScreen, lScreenBox);
                screenOn = true;

                if (screenOn == true)
                {
                    isProjectileInMotion = false;
                }
            }

            // show screen at the begining
            if (keepRunning == false)
            {
                e.Graphics.DrawImage(Properties.Resources.StartScreen, sScreenBox);
            }
                        
        }

        private void picPlayer_Click(object sender, EventArgs e)
        {

        }

        //create the projectile with the new shooter location
        void CreateNewProjectile()
        {
            if (isProjectileInMotion == false)
            {
                projectileX = enemyBox.X;
                projectileY = enemyBox.Y;

                //create the start location for the projectile
                projectileLocation = new PointF(projectileX, projectileY);
                // move the projectile box to its start location
                projectileBox.Location = projectileLocation;
                isProjectileInMotion = true;

                //make shooting sound
                if (screenOn == false)
                {
                    fire.controls.play();
                }

                // calculate slope vector from the shooter to the target
                rise = playerBox.Y - enemyBox.Y;
                run = playerBox.X - enemyBox.X;
                hypotenuse = (float)Math.Sqrt(rise * rise + run * run);

                //calulate the speeds for the projectile using slope vector
                projectileXSpeed = run / hypotenuse * PROJECTILE_SPEED;
                projectileYSpeed = rise / hypotenuse * PROJECTILE_SPEED;
            }
        }

        //move the already created projectile
        void MoveProjectile()
        {           
            projectileBox.X = projectileBox.X + projectileXSpeed;
            projectileBox.Y = projectileBox.Y + projectileYSpeed;

            if (projectileBox.IntersectsWith(playerBox) ||
                projectileBox.Y < 0 || projectileBox.Y > ClientSize.Height ||
                projectileBox.X < 0 || projectileBox.X > ClientSize.Width)
            {
                isProjectileInMotion = false;
            }

            if (projectileBox.IntersectsWith(playerBox))
            {
                Hp = Hp - 10;
            }

            if (projectileBox.IntersectsWith(swordRightBox) ||
                projectileBox.Y < 0 || projectileBox.Y > ClientSize.Height ||
                projectileBox.X < 0 || projectileBox.X > ClientSize.Width)
            {
                isProjectileInMotion = false;
            }

            if (projectileBox.IntersectsWith(swordRightBox))
            {
                cut.controls.play();
            }

            if (projectileBox.IntersectsWith(swordLeftBox) ||
                projectileBox.Y < 0 || projectileBox.Y > ClientSize.Height ||
                projectileBox.X < 0 || projectileBox.X > ClientSize.Width)
            {
                isProjectileInMotion = false;                
            }

            if (projectileBox.IntersectsWith(swordLeftBox))
            {
                cut.controls.play();
            }
        }

        void CreateNewBoomerang()
        {
            if (isBoomerangInMotion == false)
            {
                boomerangX = playerBox.X;
                boomerangY = playerBox.Y;

                //create the start location for the boomerang
                boomerangLocation = new PointF(boomerangX, boomerangY);
                // move the boomerang box to its start location
                boomerangBox.Location = boomerangLocation;
                isBoomerangInMotion = true;

                // calculate slope vector from the shooter to the target
                bRise = enemyBox.Y - playerBox.Y;
                bRun = enemyBox.X - playerBox.X;
                bHypotenuse = (float)Math.Sqrt(bRise * bRise + bRun * bRun);

                //calulate the speeds for the boomerang using slope vector
                boomerangXSpeed = bRun / bHypotenuse * BOOMERANG_SPEED;
                boomerangYSpeed = bRise / bHypotenuse * BOOMERANG_SPEED;
            }
        }

        //move the already created boomerang
        void MoveBoomerang()
        {
            if (boomerangReturn == true)
            {
                isBoomerangInMotion = true;

                // calculate slope vector from the shooter to the target
                bRise = playerBox.Y - boomerangBox.Y;
                bRun = playerBox.X - boomerangBox.X;
                bHypotenuse = (float)Math.Sqrt(bRise * bRise + bRun * bRun);

                //calulate the speeds for the boomerang using slope vector
                boomerangXSpeed = bRun / bHypotenuse * BOOMERANG_SPEED;
                boomerangYSpeed = bRise / bHypotenuse * BOOMERANG_SPEED;

                boomerangBox.X = boomerangBox.X + boomerangXSpeed;
                boomerangBox.Y = boomerangBox.Y + boomerangYSpeed;

                if (boomerangBox.IntersectsWith(playerBox))
                {
                    isBoomerangInMotion = false;
                    boomerangReturn = false;
                    gust.controls.stop();
                }
            }

                else if (isBoomerangInMotion == true)
                {                
                boomerangBox.X = boomerangBox.X + boomerangXSpeed;
                boomerangBox.Y = boomerangBox.Y + boomerangYSpeed;

                if (boomerangBox.IntersectsWith(enemyBox) ||
                    boomerangBox.Y < 0 || boomerangBox.Y > ClientSize.Height ||
                    boomerangBox.X < 0 || boomerangBox.X > ClientSize.Width)
                {
                    isBoomerangInMotion = false;
                    boomerangReturn = true;                   
                }

                if (boomerangBox.IntersectsWith(enemyBox))
                {
                    EHp = EHp - 5;
                    cut.controls.play();

                    // change enemys attack to make game more fun
                    if (EHp <= 70 && EHp >= 50)
                    {
                        enemySpeedx = 1;
                        PROJECTILE_SPEED = 30;
                    }
                    else if (EHp <= 55 && EHp >= 35)
                    {
                        enemySpeedx = 8;
                        PROJECTILE_SPEED = 8;
                    }
                    else if (EHp <= 30)
                    {
                        enemySpeedx = 15;
                        PROJECTILE_SPEED = 15;
                    }

                    if (EHp == 30 || EHp == 70 || EHp == 55)
                    {
                        dragon.controls.play();
                    }
                }
            }           
        }
        
        void ControlAnimation()
        {
            animationFrameCounter++; // same as animationFrameCounter = animationFrameCounter + 1

            //reset the animation frame count when reach end of animation 
            if (animationFrameCounter == 20 && isProjectileInMotion == true)
            {
                animationFrameCounter = 0;
            }
            else if (animationFrameCounter == 60)
            {
                animationFrameCounter = 0;
            }
        }

        private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //sound/music
            player.controls.play();
        }

        void MoveEnemy()
        {
            enemyx = enemyx + enemySpeedx;
            enemyBox.Location = new Point(enemyx, enemyy + 60);

            if (enemyx >= this.ClientSize.Width - enemyBox.Width || enemyx <= 0)
            {               
                enemySpeedx = -enemySpeedx;               
            }
        }

        //A custom timer
        void OneSecondTimer()
        {
            keepRunning = true;
            timePassedSinceLastSecond = Environment.TickCount; // <-- the current time

            //keep this timer loop running until keypress
            while (keepRunning == true)
            {               
                SetUpSword();
                SetUpHP();
                MoveEnemy();

                MoveBoomerang();
                               

                //projectile animations
                ControlAnimation();
                MoveProjectile();
                CreateNewProjectile();
               
                //side collision platform 1
                if (playerBox.Right > platform1Box.Left && playerBox.Left < platform1Box.Right - playerBox.Width && playerBox.Bottom < platform1Box.Bottom && playerBox.Bottom > platform1Box.Top)
                {
                    right = false;
                }

                if (playerBox.Left < platform1Box.Right && playerBox.Right > platform1Box.Left + playerBox.Width && playerBox.Bottom < platform1Box.Bottom && playerBox.Bottom > platform1Box.Top)
                {
                    left = false;
                }

                //side collision platform 1 enemy
                if (enemyBox.Right > platform1Box.Left && enemyBox.Left < platform1Box.Right - enemyBox.Width && enemyBox.Bottom < platform1Box.Bottom && enemyBox.Bottom > platform1Box.Top)
                {
                    enemyRight = false;
                }

                if (enemyBox.Left < platform1Box.Right && enemyBox.Right > platform1Box.Left + enemyBox.Width && enemyBox.Bottom < platform1Box.Bottom && enemyBox.Bottom > platform1Box.Top)
                {
                    enemyLeft = false;
                }

                //side collision platform 2
                if (playerBox.Right > platform2Box.Left && playerBox.Left < platform2Box.Right - playerBox.Width && playerBox.Bottom < platform2Box.Bottom && playerBox.Bottom > platform2Box.Top)
                {
                    right = false;
                }

                if (playerBox.Left < platform2Box.Right && playerBox.Right > platform2Box.Left + playerBox.Width && playerBox.Bottom < platform2Box.Bottom && playerBox.Bottom > platform2Box.Top)
                {
                    left = false;
                }

                //side collision platform 2 enemy
                if (enemyBox.Right > platform2Box.Left && enemyBox.Left < platform2Box.Right - enemyBox.Width && enemyBox.Bottom < platform2Box.Bottom && enemyBox.Bottom > platform2Box.Top)
                {
                    right = false;
                }

                if (enemyBox.Left < platform2Box.Right && enemyBox.Right > platform2Box.Left + enemyBox.Width && enemyBox.Bottom < platform2Box.Bottom && enemyBox.Bottom > platform2Box.Top)
                {
                    left = false;
                }

                //move
                if (right == true)
                {
                    playerBox.X += speed;
                }
                if (left == true)
                {
                    playerBox.X -= speed;
                }

                if (jump == true)
                {
                    //Falling if in the air/if the player has jumped
                    playerBox.Y -= Force;
                    Force -= 1;
                }

                //enemy move
                if (enemyRight == true)
                {
                    enemyBox.X += 10;                    
                }
                if (enemyLeft == true)
                {
                    enemyBox.X -= 10;
                }

                if (enemyJump == true)
                {
                    //Falling if in the air/if the enemy has jumped
                    enemyBox.Y -= EForce;
                    EForce -= 1;
                }

                if (playerBox.Y + playerBox.Height >= levelBox.Height)
                {
                    playerBox.Y = levelBox.Height - playerBox.Height; //stop falling at bottom             
                    jump = false;
                }

                if (enemyBox.Y + enemyBox.Height >= levelBox.Height)
                {
                    enemyBox.Y = levelBox.Height - enemyBox.Height; //stop falling at bottom enemy         
                    enemyJump = false;
                }

                if (playerBox.X + playerBox.Width >= levelBox.Width)
                {
                    playerBox.X = levelBox.Width - playerBox.Width; //right border                               
                }

                if (enemyBox.X + enemyBox.Width >= levelBox.Width)
                {
                    enemyBox.X = levelBox.Width - enemyBox.Width; //right border enemy                               
                }
               
                if (playerBox.X < 0) // left border
                {
                    playerBox.X = 0;
                }

                else
                {
                    playerBox.Y += 5; //Falling                  
                }

                if (enemyBox.X < 0) // left border enemy
                {
                    enemyBox.X = 0;
                }

                else
                {
                    enemyBox.Y += 5; //Falling enemy
                }

                //Top Collision platform 1 player

                if (playerBox.Left + playerBox.Width > platform1Box.Left && playerBox.Left + playerBox.Width < platform1Box.Left + platform1Box.Width + playerBox.Width && playerBox.Top + playerBox.Height >= platform1Box.Top && playerBox.Top < platform1Box.Top)
                {
                    jump = false;
                    Force = 0;
                    playerBox.Y = platform1Box.Location.Y - playerBox.Height;
                }

                //Top Collision platform 1 enemy

                if (enemyBox.Left + enemyBox.Width > platform1Box.Left && enemyBox.Left + enemyBox.Width < platform1Box.Left + platform1Box.Width + enemyBox.Width && enemyBox.Top + enemyBox.Height >= platform1Box.Top && enemyBox.Top < platform1Box.Top)
                {
                    enemyJump = false;
                    EForce = 0;
                    enemyBox.Y = platform1Box.Location.Y - enemyBox.Height;
                }

                //Top Collision platform 2

                if (playerBox.Left + playerBox.Width > platform2Box.Left && playerBox.Left + playerBox.Width < platform2Box.Left + platform2Box.Width + playerBox.Width && playerBox.Top + playerBox.Height >= platform2Box.Top && playerBox.Top < platform2Box.Top)
                {
                    jump = false;
                    Force = 0;
                    playerBox.Y = platform2Box.Location.Y - playerBox.Height;
                }

                //Top Collision platform 2 enemy

                if (enemyBox.Left + enemyBox.Width > platform2Box.Left && enemyBox.Left + enemyBox.Width < platform2Box.Left + platform2Box.Width + enemyBox.Width && enemyBox.Top + enemyBox.Height >= platform2Box.Top && enemyBox.Top < platform2Box.Top)
                {
                    enemyJump = false;
                    EForce = 0;
                    enemyBox.Y = platform2Box.Location.Y - enemyBox.Height;
                }

                //Fixing the slow fall platform 1
                if (!(playerBox.Left + playerBox.Width > platform1Box.Left && playerBox.Left + playerBox.Width < platform1Box.Left + platform1Box.Width + playerBox.Width) && playerBox.Top + playerBox.Height >= platform1Box.Top && playerBox.Top < platform1Box.Top)
                {
                    jump = true;
                }

                //Fixing the slow fall platform 1 enemy
                if (!(enemyBox.Left + enemyBox.Width > platform1Box.Left && enemyBox.Left + enemyBox.Width < platform1Box.Left + platform1Box.Width + enemyBox.Width) && enemyBox.Top + enemyBox.Height >= platform1Box.Top && enemyBox.Top < platform1Box.Top)
                {
                    enemyJump = true;
                }

                //Fixing the slow fall platform 2
                if (!(playerBox.Left + playerBox.Width > platform2Box.Left && playerBox.Left + playerBox.Width < platform2Box.Left + platform2Box.Width + playerBox.Width) && playerBox.Top + playerBox.Height >= platform2Box.Top && playerBox.Top < platform2Box.Top)
                {
                    jump = true;
                }

                //Fixing the slow fall platform 2 enemy
                if (!(enemyBox.Left + enemyBox.Width > platform2Box.Left && enemyBox.Left + enemyBox.Width < platform2Box.Left + platform2Box.Width + enemyBox.Width) && enemyBox.Top + enemyBox.Height >= platform2Box.Top && enemyBox.Top < platform2Box.Top)
                {
                    enemyJump = true;
                }

                //Head collision platform 1
                if (playerBox.Left + playerBox.Width > platform1Box.Left && playerBox.Left + playerBox.Width < platform1Box.Left + platform1Box.Width + playerBox.Width && playerBox.Top - platform1Box.Bottom <= 10 && playerBox.Top - platform1Box.Top > -10)
                {
                    Force = -1;
                }

                //Head collision platform 1 enemy
                if (enemyBox.Left + enemyBox.Width > platform1Box.Left && enemyBox.Left + enemyBox.Width < platform1Box.Left + platform1Box.Width + enemyBox.Width && enemyBox.Top - platform1Box.Bottom <= 10 && enemyBox.Top - platform1Box.Top > -10)
                {
                    EForce = -1;
                }

                //Head collision platform 2
                if (playerBox.Left + playerBox.Width > platform2Box.Left && playerBox.Left + playerBox.Width < platform2Box.Left + platform2Box.Width + playerBox.Width && playerBox.Top - platform2Box.Bottom <= 10 && playerBox.Top - platform2Box.Top > -10)
                {
                    Force = -1;
                }

                //Head collision platform 2 enemy
                if (enemyBox.Left + enemyBox.Width > platform2Box.Left && enemyBox.Left + enemyBox.Width < platform2Box.Left + platform2Box.Width + enemyBox.Width && enemyBox.Top - platform2Box.Bottom <= 10 && enemyBox.Top - platform2Box.Top > -10)
                {
                    EForce = -1;
                }
                            
                Refresh();

                // Enviorment.TeckCount - timePassedSinceLastSecond gives how much time
                //has passed since the label was last updated
                if (Environment.TickCount - timePassedSinceLastSecond >= 25)
                {
                    timeCounter++;

                    // change the 
                    timePassedSinceLastSecond = Environment.TickCount;
                }
                Application.DoEvents();
            }
         }

        private void btnStart_Click(object sender, EventArgs e)
        {
                      
        }

        private void lblStart_Click(object sender, EventArgs e)
        {
            lblStart.Hide();           
            OneSecondTimer();   
            dragon.controls.stop();
            gust.controls.stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            keepRunning = false;
        }
     }
}

