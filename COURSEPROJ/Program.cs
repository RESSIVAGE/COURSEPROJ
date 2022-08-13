using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using COURSEPROJ.models;

namespace COURSEPROJ
{
    
    public static class Program
    {
        public static void disposeObject(steper obj)
        {
            for (short i=0; i<Form1.stepers.Count;i++)
            {
                if (Form1.stepers[i] == null)
                {
                    continue;
                }

                if (Form1.stepers[i].ID == obj.ID)
                {
                    Form1.stepers.RemoveAt(i);
                }
            }
            GC.Collect();
        }

        public static Form1 window;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            window = new Form1();
            Application.Run(window);
            
        }
    } 
}