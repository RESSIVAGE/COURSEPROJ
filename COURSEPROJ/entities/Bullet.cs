using System;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using COURSEPROJ.models;

namespace COURSEPROJ.entities
{
    public class Bullet:steper
    {
        public static short globid = 499;
        public short posx;
        public short posy;
        public Image curimg;
        public float dposx;
        public float dposy;
        public short wait4Death = 0;
        public bool friendly;
        public short id;
        public float x
        {
            get { return dposx; }
            set { dposx = value; }
        }

        public short speed { get; set; }
        public float angle { get; set; }
        public float y
        {
            get { return dposy; }
            set { dposy = value; }
        }
        public short ID
        {
            get { return id; }
        }

        public Image img
        {
            get { return this.curimg; }
        }

        public Bullet(short x, short y, short speed, float angle, bool friendly, ref int[,] MAP)
        {
            curimg = Hero.sprBullet;
            posx = x;
            posy = y;
            dposx = x;
            dposy = y;
            id = ++globid;
            
            MAP[y / 64, x / 64] = id;
            
            this.speed = speed;
            this.angle = angle;
            Form1.stepersimgs[ID % 2048] = this.img;
            wait4Death = 0;
            this.friendly = friendly;
        }

        public void step(ref int[,] MAP, ref Hero hero)
        {
            Form1.stepersimgs[ID % 2048] = this.img;
            if (curimg == Hero.sprBulletExploded)
            {
                if (wait4Death == 5)
                {
                    
                    
                    Program.disposeObject(this);
                    return;
                }
                MAP[(int) y / 64, (int) x / 64] = id;
                
                wait4Death++;
                return;
                
            }
            
            float dx = (float)(speed*Math.Cos(angle));
            float dy = (float)(speed*Math.Sin(angle));
            MAP[(int)y / 64, (int)x / 64] = 0;
            if ((y + dy) / 64 >= MAP.GetLength(1) |
                (x + dx) / 64 >= MAP.GetLength(0) |
                (y + dy) / 64 < 0 |
                (x + dx) / 64 < 0|
                speed<5)
            {
                curimg = Hero.sprBulletExploded;
                return;
            }
            if (MAP[((int)y + (short)(Math.Sign(dy)*speed)) / 64, ((int)x+(short)(Math.Sign(dx)*speed)) / 64] >= 100 | 
                MAP[((int)y + (short)(Math.Sign(dy)*speed)) / 64, ((int)x+(short)(Math.Sign(dx)*speed)) / 64] == 0)
            {
                this.dposy += dy;
                this.dposx += dx;
                this.posx = (short) this.dposx;
                this.posy = (short) this.dposy;
            }
            else
            {
                speed = (short)(speed / 1.5)  ;
                dx = (float)(speed*Math.Cos(angle));
                dy = (float)(speed*Math.Sin(angle));
                if (MAP[((int)y + (short)(Math.Sign(dy)*speed)) / 64, ((int)x+(short)(Math.Sign(dx)*speed)) / 64] >= 100 | 
                    MAP[((int)y + (short)(Math.Sign(dy)*speed)) / 64, ((int)x+(short)(Math.Sign(dx)*speed)) / 64] == 0)
                {
                    this.dposy += dy/2;
                    this.dposx += dx/2;
                    this.posx = (short) this.dposx;
                    this.posy = (short) this.dposy;
                }
                else
                {
                    curimg = Hero.sprBulletExploded;
                    return;
                }
            }


            MAP[(int) y / 64, (int) x / 64] = id;
        }
        public short sub { get; set; }
        ~Bullet()
        {
            Console.WriteLine(this.ID+" successfully destroyed");
        } 
    }
}