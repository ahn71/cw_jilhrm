using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP
{
    public partial class Tools_Nested : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDBBackup_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseBackupRestore.DatabaseBackup("Glory", "D:\\DatabaseBank\\Glory" + DateTime.Now.Month + "." + DateTime.Now.Year + "", sqlDB.connection);
            }
            catch { }
        }

       
    }
}