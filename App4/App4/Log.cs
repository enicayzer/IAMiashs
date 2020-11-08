using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms.PlatformConfiguration;

namespace App4
{
    public class Log
    {
        String CurrentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        String Current = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public void writeLog()
        {
            Console.WriteLine("Couou " + CurrentFolder);
            Console.WriteLine("Couou___ " + Current);
        }
    }
}
