using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP
{
    public partial class ModifyPage : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();            

        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            int p = 0;
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("SELECT * FROM Personnel_EmpAddress ", dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string Eid = dt.Rows[i]["EmpId"].ToString();
                DataRow[] d = dt.Select("EmpId='"+Eid+"'");
                if (d.Count()>1)
                {
                    SqlCommand cmd = new SqlCommand("Delete FROM Personnel_EmpAddress where SN=(Select MAX(SN) SN FROM Personnel_EmpAddress where EmpID='" + Eid + "')", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    break;
                }
               
            }
        }
    }
}