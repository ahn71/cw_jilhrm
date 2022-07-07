using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.All_Report
{
    public partial class help : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            loadHelpFile();
        }

        private void loadHelpFile()
        {
            try
            {
                string FilePath = Server.MapPath("//All Report//GloryUserGide.pdf");
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                }

            }
            catch { }
        }
    }
}