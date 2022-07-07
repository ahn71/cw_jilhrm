using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class resignation_letter : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                LoadWorkerCardNo(ddlCardNo);
            }
        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='resignation_letter.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnPrintpreview.CssClass = "";

                            btnPrintpreview.Enabled = false;


                        }
                    }
                }

            }
            catch { }

        }

        protected void rdbWorker_CheckedChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            rdbStaff.Checked = false;
            LoadWorkerCardNo(ddlCardNo);
        }

        protected void rdbStaff_CheckedChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            rdbWorker.Checked = false;
            LoadStaffCardNo(ddlCardNo);
        } 

        protected void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                for (int k = 0; k < ddlCardNo.Items.Count; k++)
                {
                    if (ddlCardNo.Items[k].Text == txtCardNo.Text)
                    {
                        ddlCardNo.SelectedIndex = k;
                        break;
                    }
                }
                if (ddlCardNo.SelectedItem.Text != txtCardNo.Text)
                {
                    lblMessage.InnerText = "warning->Please Type Valid CardNo";
                    return;
                }
            }
            catch { }
        }
        private void LoadWorkerCardNo(DropDownList dl)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo From v_EmployeeProfile where EmpTypeId=1 and EmpStatus in ('1','8') and ActiveSalary='True' Group by EmpId,EmpCardNo order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "SN";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem("Card No", "0"));
            }
            catch { }
        }
        private void LoadStaffCardNo(DropDownList dl)
        {
            dt = new DataTable();
            sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo From v_EmployeeDetails where EmpTypeId=2 and EmpStatus in ('1','8') and ActiveSalary='True' Group by EmpId,EmpCardNo order by EmpCardNo", dt);
            dl.DataSource = dt;
            dl.DataTextField = "EmpCardNo";
            dl.DataValueField = "SN";
            dl.DataBind();
            dl.Items.Insert(0, new ListItem("Card No", "0"));
        }

        protected void btnPrintpreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation() == false) return;
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select EmpCardNo,EmpNameBn,DptNameBn,DsgNameBn,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate From v_Personnel_EmpCurrentStatus where SN=" + ddlCardNo.SelectedValue + "", dt);
                if (dt.Rows.Count > 0)
                {
                    Session["__ResignationLetter__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ResignationLetter');", true);  //Open New Tab for Sever side code
                }
                else
                    lblMessage.InnerText = "warning->No Data Available";
            }
            catch { }
        }
        private Boolean validation()
        {
            try
            {
                lblMessage.InnerText = "";
                if (ddlCardNo.SelectedItem.Text == "Card No")
                {
                    lblMessage.InnerText = "warning->Please Select Valid Card No";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }
    }
}