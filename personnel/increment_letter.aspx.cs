using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace SigmaERP.personnel
{
    public partial class increment_letter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                rdbWorker.Checked = false;
                rdbStaff.Checked = true;
                LoadStaffCardNo();
                UpdateIncrement();
                setPrivilege();
            }
        }
        private void setPrivilege()
        {
            try
            {
                upSuperAdmin.Value = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    upSuperAdmin.Value = "0";
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='increment_letter.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnPreview.CssClass = "";

                            btnPreview.Enabled = false;


                        }
                    }
                }

            }
            catch { }

        }
        private void UpdateIncrement()
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("Select * From Personnel_EmpCurrentStatus where TypeOfChange='i'", dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtR=new DataTable();
                sqlDB.fillDataTable("Select Count(EmpId) as EmpId From Personnel_EmpCurrentStatus where EmpId='"+dt.Rows[i]["EmpId"].ToString()+"' ", dtR);
                if (dtR.Rows[0]["EmpId"].ToString() == "1")
                {
                    SqlCommand cmd = new SqlCommand("Update Personnel_EmpCurrentStatus  set TypeOfChange='s' where EmpId='"+dt.Rows[i]["EmpId"].ToString()+"'", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void rdbStaff_CheckedChanged(object sender, EventArgs e)
        {
            rdbWorker.Checked = false;
            rdbStaff.Checked = true;
            LoadStaffCardNo();
        }

        protected void rdbWorker_CheckedChanged(object sender, EventArgs e)
        {
            rdbWorker.Checked = true;
            rdbStaff.Checked = false;
            LoadWorkerCardNo();
        }
        private void LoadWorkerCardNo()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select Distinct Max(SN) as SN, EmpCardNo From Personnel_EmpCurrentStatus where TypeOfChange='i' and EmpTypeId=1 group by EmpCardNo", dt);
                ddlCardNo.DataSource = dt;
                ddlCardNo.DataTextField = "EmpCardNo";
                ddlCardNo.DataValueField = "SN";
                ddlCardNo.DataBind();
                ddlCardNo.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
        private void LoadStaffCardNo()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select Distinct Max(SN) as SN, EmpCardNo From Personnel_EmpCurrentStatus where TypeOfChange='i' and EmpTypeId=2 group by EmpCardNo", dt);
                ddlCardNo.DataSource = dt;
                ddlCardNo.DataTextField = "EmpCardNo";
                ddlCardNo.DataValueField = "SN";
                ddlCardNo.DataBind();
                ddlCardNo.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            DataTable dt;

            try
            {
                if (rdbStaff.Checked == true)
                {
                    sqlDB.fillDataTable("Select Max(SN) as SN From Personnel_EmpCurrentStatus where SN<" + ddlCardNo.SelectedValue + " and EmpCardNo='" + ddlCardNo.SelectedItem.Text + "' and EmpTypeId=2", dt = new DataTable());
                    string SN = dt.Rows[0]["SN"].ToString();
                    sqlDB.fillDataTable("Select EmpCardNo,EmpName,DsgName,DptName,EmpPresentSalary,SalaryCount From v_Personnel_EmpCurrentStatus where SN=" + SN + " ", dt = new DataTable());
                    DataTable dtP = new DataTable();
                    sqlDB.fillDataTable("Select BasicSalary,OthersAllownce, EmpPresentSalary,EffectiveMonth From v_Personnel_EmpCurrentStatus where SN=" + ddlCardNo.SelectedValue + " ", dtP);
                    Session["__IncrementLetterStaff__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IncrementLetterStaff-" + dtP.Rows[0]["BasicSalary"].ToString() + "-" + dtP.Rows[0]["OthersAllownce"].ToString() + "-" + dtP.Rows[0]["EmpPresentSalary"].ToString() + "-" + dtP.Rows[0]["EffectiveMonth"].ToString() + "-"+upSuperAdmin.Value+"');", true);  //Open New Tab for Sever side code
                }
                else
                {
                    sqlDB.fillDataTable("Select Max(SN) as SN From Personnel_EmpCurrentStatus where SN<" + ddlCardNo.SelectedValue + " and EmpCardNo='" + ddlCardNo.SelectedItem.Text + "' and EmpTypeId=1", dt = new DataTable());
                    string SN = dt.Rows[0]["SN"].ToString();
                    sqlDB.fillDataTable("Select EmpCardNo,EmpNameBn,DsgNameBn,DptNameBn,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,LnCode,EmpPresentSalary From v_Personnel_EmpCurrentStatus where SN=" + SN + " ", dt = new DataTable());
                    Session["__IncrementLetterWorker__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IncrementLetterWorker-" +ddlCardNo.SelectedValue+ "');", true);  //Open New Tab for Sever side code
                }
            }
            catch { }
        }
    }
}