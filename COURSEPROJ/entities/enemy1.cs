using System;
using System.Drawing;
using COURSEPROJ.models;

namespace COURSEPROJ.entities
{
    public class enemy1:steper
    {
        public static short globid = 99;
        public float posx;
        public float posy;
        public short id, speed;
        public double stepsub;
        private bool shooted;
        public float subspeed;
        public Image[] curimg;
        private Random rnd = new Random();
        public float x
        {
            get { return posx; }
            set { posx = value; }
        }
        public float y
        {
            get { return posy; }
            set { posy = value; }
        }
        public short ID
        {
            get { return id; }
        }
        
        public enemy1(short x, short y, ref int[,] MAP)
        {
            curimg = Hero.sprEnemy2_walk;
            shooted = false;
            subspeed = 0;
            speed = 25;
            sub = 0;
            stepsub = 0;
            posx = x;
            posy = y;
            id = ++globid;
            MAP[y / 64, x / 64] = id;
        }

        public void step(ref int[,] MAP, ref Hero hero)
        {
            Form1.stepersimgs[ID % 2048] = this.img;
            stepsub += subspeed;
            sub = (short) stepsub;
            
            short distToPlayer = (short)Math.Sqrt(Math.Pow(hero.posX - this.posx, 2) + Math.Pow(hero.posY - this.posy, 2));
            double angleToPlayer = Hero.findAngle(this.posx, this.posy, hero.posX, hero.posY);
            for (int i = 0; i < distToPlayer; i += 5)
            {
                int scanned = MAP[(short) (this.posy + i * Math.Sin(angleToPlayer)) / 64, (short) (this.posx + i * Math.Cos(angleToPlayer)) / 64];
                if (
                    scanned!=0 & scanned<99
                    )
                {
                    return;
                }
                
            }
            
            if (this.sub % curimg.Length == 1 & this.curimg == Hero.sprEnemy2_shoot&this.shooted==false)
            {
                Form1.stepers.Add(
                    new Bullet(
                        (short)(this.posx+100*Math.Cos(angleToPlayer)+10*Math.Cos(angleToPlayer+90)), 
                        (short)(this.posy+100*Math.Sin(angleToPlayer)+10*Math.Sin(angleToPlayer+90)), 
                        25, 
                        (float)angleToPlayer-(float)Math.PI/180*2,
                        false, ref MAP)
                );
                shooted = true;
            }
            
            if (sub % curimg.Length != 1 | curimg != Hero.sprEnemy2_shoot)
            {
                shooted = false;
            }

            foreach (steper steper_ in Form1.stepers.ToArray())
            {
                if (steper_ is Bullet blt)
                {
                    if (Math.Abs(blt.posx-this.posx)<64&
                        Math.Abs(blt.posy-this.posy)<64)
                    {
                        if (blt.friendly)
                        {
                            if (MAP[(int) y / 64, (int) x / 64] == id)
                            {
                                MAP[(int) y / 64, (int) x / 64] = 0;
                            }

                            Program.disposeObject(this);
                        }
                        else
                        {
                            blt.curimg=Hero.sprBulletExploded;
                        }
                    }
                }
            }

            switch (distToPlayer)
            {
                case >600 and <2500:
                    subspeed = .2f;
                    speed = 15;
                    this.curimg = Hero.sprEnemy2_walk;
                    
                    float dx = (float) (speed * Math.Cos(angleToPlayer));
                    float dy = (float) (speed * Math.Sin(angleToPlayer));
                    MAP[(int) y / 64, (int) x / 64] = 0;
                    if (MAP[
                            ((int) y + Math.Sign(dy) * 16) / 64,
                            ((int) x + Math.Sign(dx) * 16) / 64
                        ] == 0|
                        MAP[
                            ((int) y + Math.Sign(dy) * 16) / 64,
                            ((int) x + Math.Sign(dx) * 16) / 64
                        ]>=100
                        )
                    {
                        this.posy += dy;
                        this.posx += dx;
                    }
                    MAP[(int) y / 64, (int) x / 64] = id;
                    break;
                case <=600:
                    speed = 0;
                    subspeed = .1f;
                    this.curimg = Hero.sprEnemy2_shoot;
                    
                    break;
                case >=2500:
                    speed = 0;
                    subspeed = 0;
                    this.curimg = Hero.sprEnemy2_shoot;
                    stepsub = 0;
                    
                    break;
            }
        }

        public Image img
        {
            get
            {
                return curimg[sub % curimg.Length];
            }
        }
        ~enemy1()
        {
            Console.WriteLine(this.ID+" successfully destroyed");
        }
        public short sub { get; set; }
    }
}