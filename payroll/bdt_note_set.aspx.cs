using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComplexScriptingSystem;
using System.Data;
using adviitRuntimeScripting;
using System.Data.SqlClient;

namespace SigmaERP.hrd
{
    public partial class bdt_note_set : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            if (!IsPostBack)
            {
                setPrivilege();
                loadBDTNote();
            }
        }

        private void setPrivilege()
        {
            try
            {
                ViewState["__WriteAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='bdt_note_set.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                            ViewState["__ReadAction__"] = "0";
                            gvNoteList.Visible = false;
                        }

                        if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            btnSet.CssClass = "";
                            btnSet.Enabled = false;
                            btnReset.CssClass = "";
                            btnReset.Enabled = false;
                            
                        }
                    }
                }
            }
            catch { }
        }

        private void loadBDTNote()
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select SL,Note,Chosen from HRD_BDTNote",dt=new DataTable (),sqlDB.connection);
                gvNoteList.DataSource = dt;
                gvNoteList.DataBind();

            }
            catch { }
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                bool status = false;
                for (byte b = 0; b < gvNoteList.Rows.Count; b++)
                {
                    CheckBox chk = new CheckBox();
                    chk = (CheckBox)gvNoteList.Rows[b].Cells[1].FindControl("SelectCheckBox");
                    if (!chk.Checked)
                    {
                        string[] getColumns = { "Chosen" };
                        string[] getValues = {"0"};
                        status = true;
                        SQLOperation.forUpdateValue("HRD_BDTNote", getColumns, getValues, "SL", gvNoteList.DataKeys[b].Value.ToString(), sqlDB.connection);
                        status = true;
                    }
                }
                if (status) 
                {
                    lblMessage.InnerText = "success->Successfully Setup BDT Note";
                    loadBDTNote();
                }
            }
            catch (Exception ex)
            { 
            
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                SqlCommand cmd = new SqlCommand(" update HRD_BDTNote set Chosen='1'",sqlDB.connection);
                cmd.ExecuteNonQuery();
                lblMessage.InnerText = "success->Successfully Reset BDT Note ";
                loadBDTNote();
                
            }
            catch { }
        }

        protected void gvNoteList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (gvNoteList.Rows.Count <= 0)
                {
                    CheckBox chk = (CheckBox)e.Row.FindControl("SelectCheckBox");
                    chk.Enabled = false;
                }
            }
            catch { }
        }

       

        

        
    }
}