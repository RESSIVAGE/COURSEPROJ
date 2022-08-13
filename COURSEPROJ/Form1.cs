using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using COURSEPROJ.entities;
using COURSEPROJ.models;
using System.Runtime.InteropServices;


namespace COURSEPROJ
{
   
    
    public partial class Form1 : Form
    {
        public Image testimg;
        public Test tester;
        public Hero hero;
        public double MOUSEX=720/2;
        public double MOUSEY=-1234;
        public static short SURFWIDTH=720;
        public static short SURFHEIGHT=360;
        
        public Image gun = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\fps_gun_100px.png"));
        
        public Image crosshair = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\crosshair72px.png"));
        
        Bitmap surface  = new Bitmap(SURFWIDTH, SURFHEIGHT);
        Bitmap surface2  = new Bitmap(SURFWIDTH, SURFHEIGHT);
        public Bitmap MAP_IMAGE;
        public static int[,] MAP;
        // public static List<steper> enemiesList = new List<steper>();
        // public static steper[] enemies;
        public static List<steper> stepers = new List<steper>();
        public static Pen[] pens = new Pen[255];
        public static int elapsedtimers = 0; 
        public static Image[] stepersimgs = new Image[2048];

        public delegate void updateDelegate(steper obj);
        public event updateDelegate updateEvent;
        
        public Form1()
        {
            for (int i = 0; i < 255; i++)
            {
                pens[i] = new Pen(Color.FromArgb(i+1, 0, 0, 0), 1f);
            }
            MAP_IMAGE = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\map.png"));
            MAP = new int[MAP_IMAGE.Height, MAP_IMAGE.Width];
            //for(y)for(x)MAP[y,x]
            for(short y=0; y<MAP_IMAGE.Width; y++){
                for(short x=0; x<MAP_IMAGE.Height; x++)
                {
                    switch (MAP_IMAGE.GetPixel(x,y).Name)
                    {
                        //стена
                        case "ff000000":
                            MAP[y,x]=2;
                            break;
                        //камень
                        case "ff333941":
                            MAP[y,x]=3;
                            break;
                        //герой
                        case "ff0000ff":
                            hero = new Hero();
                            hero.posX=x*64;
                            hero.posY=y*64;
                            break;
                        //финальная боковая стена
                        case "ff6d758d":
                            MAP[y, x] = 4;
                            break;
                        //финальная горизонтальная стена
                        case "ff4a5462":
                            MAP[y, x] = 5;
                            break;
                        //финальная штука
                        case "fffbf236":
                            MAP[y, x] = 6;
                            break;
                        //enemy1
                        case "ffff0000":
                            stepers.Add(new enemy1(
                                (short)(x*64),
                                (short)(y*64),
                                ref MAP)
                            );
                            break;
                        case "ff008fff":
                            stepers.Add(new enemy2(
                                (short)(x*64),
                                (short)(y*64),
                                ref MAP)
                            );
                            break;
                        case "ffff00ff":
                            stepers.Add(new Bullet((short)(x*64),(short)(y*64),1,1, true, ref MAP));
                            break;
                        default:
                            MAP[y,x]=0;
                            break;
                    }
                }
            }

            // enemies = new steper[enemiesList.Count];
            // foreach (steper enemy in enemiesList)
            // {
            //     enemies[enemy.ID % enemiesList.Count] = enemy;
            // }

            Bitmap surface = new Bitmap(SURFWIDTH, SURFHEIGHT);
            using (Graphics g = Graphics.FromImage(surface)){g.Clear(Color.White);}
            
            Bitmap surface2 = new Bitmap(SURFWIDTH, SURFHEIGHT);
            using (Graphics g = Graphics.FromImage(surface2)){g.Clear(Color.FromArgb(0,0,0,0));}
            
            InitializeComponent();
            this.Width = SURFWIDTH;
            this.Height = SURFHEIGHT;
            
            timer1.Interval = 16;
            timer1.Enabled = true;
            timer1.Elapsed += Update;

            KeyPress += OnPress;

            KeyUp += OnUp;
            
            MouseMove += OnMouseMove;

            MouseClick += OnMouseClick;
            
            Init();
        }
        
        public void Init()
        {
            Cursor.Hide();
            Console.WriteLine(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\test.png"));
            testimg = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "sprites\\test.png"));
            // hero.curImg=testimg;

            for(short y=0; y<MAP_IMAGE.Width; y++){
                for(short x=0; x<MAP_IMAGE.Height; x++)
                {
                    switch (MAP[y,x])
                    {
                        //стена
                        case 2:
                            MAP[y,x]=2;
                            break;

                    }
                }
            }
        
            timer1.Start();
        }
        
        private void OnPaint(object sender, PaintEventArgs e)
        {
            short outw = 720;
            short outh = 360;
            Graphics g = e.Graphics;
            // hero.drawSelf(surface);
            g.InterpolationMode=InterpolationMode.NearestNeighbor;
            g.SmoothingMode=SmoothingMode.None;
            g.CompositingQuality=CompositingQuality.AssumeLinear;
            
            Graphics g2 = Graphics.FromImage(surface);
            g2.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#f4b990")), new Rectangle(0,0, 720, 180));
            g2.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#493c2b")), new Rectangle(0,180, 720, 180));
            hero.watch(surface, surface2, ref MAP);
            
            
            
            g.DrawImage(surface, new Rectangle(0,0, outw, outh), 150,50, 570, 310, GraphicsUnit.Pixel);//нарисовать сурфейс c увеличением
            g.DrawImage(surface2, new Rectangle(0,0, outw, outh), 150,50, 570, 310, GraphicsUnit.Pixel);//нарисовать сурфейс c увеличением
            g.DrawImage(crosshair, new Point(outw/2-48-4, outh/2-48));
            g.DrawImage(gun,new Rectangle(outw-200,outh-175, 200, 150), 0,0,100,100, GraphicsUnit.Pixel);
            
            
            
            /*for (short y = 0; y < MAP.GetLength(0); y++)
            {
                for (short x = 0; x < MAP.GetLength(1); x++)
                {
                    Color col = Color.Gray;
                    if (MAP[y,x]==2){col = Color.Black;}
                    g.FillRectangle(new SolidBrush(col), (float)x*16,(float)y*16+360,10F,10F);
                }
            }
            g.FillRectangle(new SolidBrush(Color.Blue), (float)hero.posX,(float)hero.posY+360,10F,10F);
            g.FillRectangle(new SolidBrush(Color.Red), (float)hero.posX+10*(float)Math.Cos(hero.angle*Math.PI/180),(float)hero.posY+10*(float)Math.Sin(hero.angle*Math.PI/180)+360,10F,10F);*/
            
            
            using (Graphics g_ = Graphics.FromImage(surface)){g_.Clear(Color.White);}//очистить сурфейс
            using (Graphics g_ = Graphics.FromImage(surface2)){g_.Clear(Color.FromArgb(0,0,0,0));}//очистить сурфейс
        }
        
        public void Update(object sender, EventArgs e)
        {
            
            try { hero.step(e); }
            catch { }
            
            foreach (steper st in stepers.ToArray())
            {
                st.step(ref MAP, ref hero);
            }

            Form1.elapsedtimers++;
            Invalidate();
        }

        public void OnPress(object sender, KeyPressEventArgs e)
        {
            hero.handleKeysDown(e);
            Invalidate();
        }
        public void OnMouseClick(object sender, MouseEventArgs e)
        {
            
            switch (e.Button)
            {
                case MouseButtons.Left:
                    steper blt = new Bullet( 
                        (short)(hero.posX+150*Math.Cos(hero.Angle*(Math.PI/180))), 
                        (short)(hero.posY+150*Math.Sin(hero.Angle*(Math.PI/180))), 50, 
                        (float)((hero.Angle-5)*(Math.PI/180)),
                        true, ref MAP);
                    stepers.Add(blt);
                    break;
            }

            
            Invalidate();
        }
        public void OnUp(object sender, KeyEventArgs e)
        {
            hero.handleKeysUp(e);
            Invalidate();
        }
        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            
            double MOUSED=e.X-705/2;
            
            hero.Angle+=(float)MOUSED/2;
            Cursor.Position = new Point(720 / 2+Program.window.Location.X, 360 / 2+Program.window.Location.Y);
            MOUSEX = 720 / 2 + Program.window.Location.X;
        }
    }
}