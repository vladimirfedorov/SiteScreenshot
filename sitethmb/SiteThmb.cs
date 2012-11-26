using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace sitethmb
{
    
    public class SiteThmb
    {
        /// <summary>
        /// Internet address of a site
        /// </summary>
        public string Url;

        public string FileName;
        
        /// <summary>
        /// Size of the site image
        /// </summary>
        public Size SiteImageSize;

        /// <summary>
        /// Site screenshot has limited height if set
        /// </summary>
        public bool LimitHeight = true;

        /// <summary>
        /// Site image
        /// </summary>
        public Image SiteImage;

        /// <summary>
        /// Constructor
        /// </summary>
        public SiteThmb()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Url">Internet address of a site</param>
        public SiteThmb(string Url, string FileName, int Width, int Height)
        {
            this.Url = Url;
            this.FileName = FileName;
            this.SiteImageSize = new Size(Width, Height);
        }


        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            // DocumentCombleted is triggered with different statuses, wait for "Complete"
            if (browser.ReadyState != WebBrowserReadyState.Complete) return;

            if (!LimitHeight) browser.Height = browser.Document.Body.ScrollRectangle.Height;
            using (Graphics graphics = browser.CreateGraphics())
            using (Bitmap bitmap = new Bitmap(
                browser.Width, 
                LimitHeight ? browser.Height : browser.Document.Body.ScrollRectangle.Height, 
                graphics))
            {
                Rectangle bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                browser.DrawToBitmap(bitmap, bounds);
                this.SiteImage = bitmap;
                bitmap.Save(this.FileName, ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// Save image of a site
        /// </summary>
        /// <param name="FileName">File name to save to</param>
        public void SaveSiteImage()
        {
            SaveSiteImage(this.Url, this.FileName);
        }
        
        /// <summary>
        /// Save image of a site
        /// </summary>
        /// <param name="Url">Internet address of a site</param>
        /// <param name="FileName">File name to save to</param>
        public void SaveSiteImage(string Url, string FileName)
        {
            this.Url = Url;
            this.FileName = FileName;

            WebBrowser wb = new WebBrowser();
            wb.Size = this.SiteImageSize;
            // wait until page is ready
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);
            wb.ScrollBarsEnabled = true;

            wb.Navigate(Url);
            while (wb.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();

        }

        
    }
}
