using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Configuration;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using COURSEPROJ.models;
using static COURSEPROJ.Form1;

namespace COURSEPROJ.entities
{
    public class Hero
    {
        public double posX;
        public double posY;
        public float _angle = 90;
        public double speed = 20;
        public short hspeed=0;
        public short vspeed=0;
        public double dirAngle;
        
        public Image curImg;
        
        public Image texStoneWall = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\16px_5.png"));
        
        public Image texStoneWallDamaged = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\16px_4.png"));
        
        public Image texFinalWall = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\16px_7.png"));
        
        public Image texFinalSideWall = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\16px_6.png"));
        
        public Image[] texFinalThingGif =
        {
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName,
                "sprites\\16px_10_1.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName,
                "sprites\\16px_10_2.png"))
        };

        public static Image sprBulletExploded = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\16px_8.png"));
        
        public static Image[] sprEnemy1_walk =
        {
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy1_walk1.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy1_walk2.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy1_walk3.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy1_walk4.png"))
        };
        
        public static Image[] sprEnemy2_walk =
        {
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy2_walk1.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy2_walk2.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy2_walk3.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy2_walk4.png"))
        };
        
        public static Image[] sprEnemy1_shoot =
        {
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy1_shoot1.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy1_shoot2.png")),
        };
        
        public static Image[] sprEnemy2_shoot =
        {
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy2_shoot1.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy2_shoot2.png")),
            new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, 
                "sprites\\spr_enemy2_shoot3.png")),
        };
        
        public static Image sprBullet = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\16px_9.png"));

        public static double findAngle(double x1, double y1, double x2,  double y2)
        {
            return Math.Atan2(
                (y2-y1),(x2-x1)
            );
        }
        
        public float Angle
        {
            get{return _angle;}
            set
            {
                _angle = value%360;
                while (_angle<0){_angle+=360;}
                _angle = _angle%360;
            }
        }
        
        public Hero()
        {
        }
        public void handleKeysDown(KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    this.vspeed=1;
                    break;
                case 'a':
                    this.hspeed=-1;
                    break;
                case 's':
                    this.vspeed=-1;
                    break;
                case 'd':
                    this.hspeed=1;
                    break;
            }
        }
        public void handleKeysUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    this.vspeed=0;
                    break;
                case Keys.A:
                    this.hspeed=0;
                    break;
                case Keys.S:
                    this.vspeed=0;
                    break;
                case Keys.D:
                    this.hspeed=0;
                    break;
                case Keys.Escape:
                    Environment.Exit(0);
                    break;
            }
            
        }
        public void step(EventArgs e)
        {
            if (this.hspeed == this.vspeed & this.vspeed == 0) return;
            double vangle = ((1 - vspeed)/2) * Math.PI ;
            double hangle = Math.PI / 2 * this.hspeed ;
            double newAngDelta = (vangle == 0 ? hangle : vangle);
            if (vangle != 0 & hangle != 0)
            {
                newAngDelta = Math.Atan2(
                    Math.Sin(vangle) + Math.Sin(hangle),
                    Math.Cos(hangle) + Math.Cos(vangle)
                );
            }

            double newAng = (Angle - 10)*Math.PI/180 + newAngDelta;
            // while (newAng > Math.PI * 2) newAng -= Math.PI * 2;
            // while (newAng < 0) newAng += Math.PI * 2;
            double deltaX = this.speed * Math.Cos(newAng);;
            double deltaY = this.speed * Math.Sin(newAng);;
            if (Form1.MAP[(short)(this.posY+Math.Sign(deltaY)*64)/64, (short)(this.posX+Math.Sign(deltaX)*64)/64] == 0 |
                Form1.MAP[(short)(this.posY+Math.Sign(deltaY)*64)/64, (short)(this.posX+Math.Sign(deltaX)*64)/64] >= 100)
            {
                this.posX += deltaX;
                this.posY += deltaY;
            }
        }
        public void drawSelf(Bitmap b)
        {
        }
        public void watch(Bitmap b, Bitmap b2, ref int[,] MAP)
        {
            float[] lens = new float[720];
            short[] flats = new short[720];
            
            bool[] drawed = new bool[1000];
            bool[] met = new bool[stepers.Count];
            
            float[] steperCoords = new float[stepers.Count*2];
            float[] ids = new float[stepers.Count*2];
            short stepi = 0;
            foreach (steper steper_ in stepers)
            {
                steperCoords[stepi * 2] = steper_.x;
                steperCoords[stepi * 2 + 1] = steper_.y;
                ids[stepi] = steper_.ID;
                stepi++;
            }

            float[] getCollision(ref int[,] MAP_, float angleInGrad, double startx, double starty)
            {
                angleInGrad*=(float)Math.PI/180;
                float x_;
                float y_;
                double cos = Math.Cos(angleInGrad);
                double sin = Math.Sin(angleInGrad);

                for (float c = 0f; c<640*20; c+=1f) {
                    x_ = Convert.ToSingle(startx + c*cos);
                    y_ = Convert.ToSingle(starty + c*sin);
                    short spltdx = (short)(x_ / 64);
                    short spltdy = (short)(y_ / 64);
                    
                        if (MAP_[spltdy, spltdx] == 0)
                        {
                            continue;
                        }
                        if (MAP_[spltdy,spltdx]==2|
                            MAP_[spltdy,spltdx]==3|
                            MAP_[spltdy,spltdx]==4|
                            MAP_[spltdy,spltdx]==5|
                            MAP_[spltdy,spltdx]==6) {
                            return new []
                            {
                                x_,
                                y_,
                                Convert.ToSingle(MAP_[spltdy,spltdx])
                            };
                        }
                        
                        for (short i1=0; i1<stepers.Count;i1++)
                        {
                            short i2 = (short)(i1 * 2);
                                if (Math.Abs(steperCoords[i2]-x_)<6 & Math.Abs(steperCoords[i2+1]-y_)<6 & met[i1]==false)
                                {
                                    met[i1] = true;
                                    return new[]
                                    {
                                        steperCoords[i1*2],
                                        steperCoords[i1*2+1],
                                        ids[i1]
                                    };
                                }
                            }
                }
                return new [] {
                    Convert.ToSingle(this.posX),
                    Convert.ToSingle(this.posY), 
                    -1f};
            }
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode=InterpolationMode.NearestNeighbor;
            g.SmoothingMode=SmoothingMode.HighQuality;

            Graphics g2 = Graphics.FromImage(b2);
            g2.InterpolationMode=InterpolationMode.NearestNeighbor;
            g2.SmoothingMode=SmoothingMode.HighQuality;
            
            //for(-fov/2;i<fov/2;i+=width/fov)
            float[] coords;
            
            
            Image tex = texStoneWall;
            // Image spr = sprEnemy1;
            
            for (short i=0; i<720;i++)
            {
                float angleForCoords = Angle-45+((float)i)/8;
                
                
                coords = getCollision(ref MAP, angleForCoords-10, this.posX, this.posY);
                
                byte tex_size = 16;
                float columnHeight;
                
                void drawVertical()
                {
                    
                    // Console.WriteLine(String.Join(",", coords));
                    switch (coords[2])
                    {
                        case 2:
                            tex = texStoneWall;
                            break;
                        case 3:
                            tex = texStoneWallDamaged;
                            break;
                        case 4:
                            tex = texFinalSideWall;
                            break;
                        case 5:
                            tex = texFinalWall;
                            break;
                        case 6:
                            
                            tex = texFinalThingGif[
                                (int)Math.Round(
                                    (Math.Sin(Form1.elapsedtimers/3)+1)/2
                                    )
                            ];
                            break;
                        case >99 and <200:
                            // spr = sprEnemy1;
                            break;
                        case >499 and <600:
                            // spr = sprBullet;
                            break;
                        default:
                            tex = texFinalWall;
                            // spr = texFinalWall;
                            break;
                    }
                    
                    //стена
                    if (coords[2] < 99f)
                    {
                        float len = Convert.ToSingle(Math.Sqrt(Math.Pow(coords[0] - posX, 2.0) +
                                                               Math.Pow(coords[1] - posY, 2.0)));
                        columnHeight =
                            Math.Max(Convert.ToSingle(7500 * 2 / (len * Math.Cos((45 - i / 8) * Math.PI / 180))), 0);

                        short tex_x = (short) (Math.Max(
                            coords[0] / 64 - (short) ((coords[0] + 1) / 64),
                            coords[1] / 64 - (short) ((coords[1] + 1) / 64)
                        ) * tex_size);
                        while (tex_x < 0)
                        {
                            tex_x += tex_size;
                        }



                        if (len < lens[i] | lens[i] == 0)
                        {
                            lens[i] = len;
                        }

                        g.DrawImage(tex, new Rectangle(i, 180 - (short) columnHeight, 1, (short) columnHeight * 2),
                            tex_x,
                            0, 1, tex_size, GraphicsUnit.Pixel);

                        //тень
                        if (lens[i] > 5)
                        {
                            g.DrawLine(Form1.pens[Math.Min((short) (len / 7), (short) 150)],
                                i, 180 - (short) columnHeight, i, 180 + (short) columnHeight);
                        }


                    }

                    //монстр    
                    if (coords[2] > 99f & drawed[(short) (coords[2]) % drawed.Length] == false)
                    {
                        float len_ = Convert.ToSingle(Math.Sqrt(Math.Pow(coords[0] - posX, 2.0) +
                                                               Math.Pow(coords[1] - posY, 2.0)));
                        if (len_ < lens[i] | lens[i] == 0)
                        {
                            lens[i] = len_;
                            
                            flats[i] = (short) coords[2];
                        }

                        drawed[(short) coords[2] % drawed.Length] = true;
                    }
                }

                
                drawVertical();


                
                while (coords[2] > 99f)
                {
                    coords = getCollision(ref MAP, angleForCoords-10, coords[0], coords[1]);
                    drawVertical();
                }
                
            }
            //draw flat objects
            while (!Enumerable.SequenceEqual(flats, new short[720] ))
            {
                float minlen = 0;
                short minflat = 0;
                short mini = 0;
                for (short i = 0; i < 720; i++)
                {
                    if (flats[i]==0){continue;}

                    if (lens[i] > minlen)
                    {
                        minlen = lens[i];
                        minflat = flats[i];
                        mini = i;
                    }
                }

                flats[mini] = 0;
                
                
                byte spr_size = (byte)Form1.stepersimgs[minflat].Width;
                
                
                float columnHeight = Math.Max(Convert.ToSingle(7500 * 2 / (minlen * Math.Cos((45 - mini / 8) * Math.PI / 180))), 0)/32*spr_size;
                
                
                
                short start = (short) Math.Max(0, (int) (mini - columnHeight));
                short end = (short) Math.Min(720, mini + columnHeight);

                ImageAttributes attr = new ImageAttributes();
                if (minflat > 99 & minflat < 200)
                {
                    ColorMatrix colormatrix = new ColorMatrix(new float[][]
                        {
                            new float[] {0, 0, 0, 0, 0}, // red scaling factor of 2
                            new float[] {0, 0, 0, 0, 0}, // green scaling factor of 1
                            new float[] {0, 0, 0, 0, 0}, // blue scaling factor of 1
                            new float[] {0, 0, 0, (float)(Math.Min((short) (minlen / 7), (short) 150)) / 255, 0}, 
                            new float[] {0, 0, 0, 0, 1}
                        }

                    );
                    attr.SetColorMatrix(colormatrix);
                }
                if (minflat > 499 & minflat < 600)
                {

                    // spr = sprBullet;
                }


                short[] toDraw = new short[end - start];
                float[] srcs = new float[end - start];
                for (short j = start; j < end-1; j++)
                {
                    if (minlen < lens[mini])
                    {
                        lens[mini] = minlen;
                    }

                    
                    if (minlen <= lens[j]|minlen <= lens[j+1])
                    {
                        float srcx = ((j - mini + columnHeight) / (columnHeight * 2)) * spr_size;
                        toDraw[j - start] = j;
                        srcs[j - start] = srcx;
                        
                        Rectangle rect = new Rectangle(j, 180 - (short) columnHeight, 1, (short) (columnHeight) * 2);
                        g2.DrawImage(Form1.stepersimgs[minflat%2048],
                            rect,
                            srcx,
                            0, 1, spr_size, GraphicsUnit.Pixel);
                        if (minlen > 70)
                        {
                            g2.DrawImage(Form1.stepersimgs[minflat%2048],
                                rect,
                                j,
                                0, 1, spr_size, GraphicsUnit.Pixel, attr);
                        }
                    }
                     
                }
            }
        }
        
    }
}