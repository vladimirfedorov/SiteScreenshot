using System;
using System.Text;
using sitethmb;
using System.Reflection;
using System.IO;

namespace SiteScreenshot
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // SiteScreenshot http://www.sitename.com/page.html 1280 1024 ./image.jpg
            // SiteScreenshot http://www.sitename.com/page.html 1280 ./image.jpg

            if (args.Length == 3)
                CreateSiteScreenshot(args[0], args[1], "1", false, args[2]);
            else if (args.Length == 4)
                CreateSiteScreenshot(args[0], args[1], args[2], true, args[3]);
            else
                ShowInfo();
        
        }

        /// <summary>
        /// Show help
        /// </summary>
        static void ShowInfo()
        {
            Console.WriteLine("Params format: ");
            Console.WriteLine("  SiteScreenshot URL Width [HeightLimit] OutputFileName");
            Console.WriteLine("Examples:");
            Console.WriteLine("  SiteScreenshot http://www.sitename.com/page.html 1280 1024 ./image.jpg");
            Console.WriteLine("  SiteScreenshot http://www.sitename.com/page.html 1280 ./image.jpg");
        }

        /// <summary>
        /// Create a site screenshot
        /// </summary>
        /// <param name="Url">page address</param>
        /// <param name="sWidth">Screenshot width</param>
        /// <param name="sHeight">Screenshot height</param>
        /// <param name="Limit">Don't use sHeight if false</param>
        /// <param name="FileName">File name to save to</param>
        static void CreateSiteScreenshot(string Url, string sWidth, string sHeight, bool Limit, string FileName)
        {
            int Width=0, Height=0;
            if (!int.TryParse(sWidth, out Width)) { Console.WriteLine("Wrong width: " + sWidth); return; }
            if (!int.TryParse(sHeight, out Height)) { Console.WriteLine("Wrong height: " + sHeight); return; }
            
            string outfilename = 
                Environment.ExpandEnvironmentVariables(
                    Path.GetFullPath(FileName)); ;

            SiteThmb thmb = new SiteThmb(Url, outfilename, Width, Height);
            thmb.LimitHeight = Limit;
            thmb.SaveSiteImage();

        }
    }
}
