using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class FloorAssigne : System.Web.UI.Page
    {
        DataTable dt;
        DataTable dtFloorlist;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";

            if (!IsPostBack)
            {
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
            }
        }

        private void setPrivilege()
        {
            try
            {

                ViewState["__WriteAction__"] = "1";
                ViewState["__ReadAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                //SetUserType();
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                {

                    DataTable dt = new DataTable();

                    sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='FloorAssigne.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                            ViewState["__ReadAction__"] = "0";
                            btnPrint.Enabled = false;
                            btnPrint.CssClass = "";
                        }
                        if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            ViewState["__WriteAction__"] = "0";                        
                        }
                    }
                    else
                    {
                        ViewState["__ReadAction__"] = "0";
                        btnPrint.Enabled = false;
                        btnPrint.CssClass = "";
                        ViewState["__WriteAction__"] = "0"; 
                        return;
                    }
                }
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.LoadBranch(ddlCompanyList);
                loadRecentloyTransferShiftDepartment();
                loadFloorList();
            }
            catch { }

        }
        private void loadRecentloyTransferShiftDepartment()
        {
            try
            {
                dt = new DataTable();
                if (!chkLoadAllShiftList.Checked) sqlDB.fillDataTable("select  top 20 STId,dptName+' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) '+SftName +' | '+ GName as Title from v_ShiftTransferInfo_DepartmetnList  where  STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' order by STId desc ", dt);
                else sqlDB.fillDataTable("select STId,dptName+' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) '+SftName +' | '+ GName as Title from v_ShiftTransferInfo_DepartmetnList  where  STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' order by STId desc ", dt);
                ddlDepartmentList.DataTextField = "Title";
                ddlDepartmentList.DataValueField = "STId";
                ddlDepartmentList.DataSource = dt;
                ddlDepartmentList.DataBind();
                ddlDepartmentList.Items.Insert(0, new ListItem(" ", "0"));

                int i=0;
                foreach (ListItem item in ddlDepartmentList.Items)
                {
                    if (i%2==0) item.Attributes.Add("style", "color:green");             
                    else item.Attributes.Add("style", "color:red");

                    i++;
                }

            }
            catch { }
        }


        private void loadFloorList()
        {
            try
            {
                sqlDB.fillDataTable("select FId,FName from HRD_Floor where IsActive='True'",dtFloorlist=new DataTable ());
            }
            catch { }
        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Searching();
        }

        protected void gvEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if(e.Row.RowType==DataControlRowType.DataRow)
                {
                    DropDownList ddlFloorList = (e.Row.FindControl("ddlFloorList") as DropDownList);
                    sqlDB.fillDataTable("select FId,FName from HRD_Floor where IsActive='True'", dtFloorlist = new DataTable());
                    ddlFloorList.DataValueField = "FId";
                    ddlFloorList.DataTextField = "FName";
                    ddlFloorList.DataSource = dtFloorlist;
                    ddlFloorList.DataBind();
                    string getFID = (dt.Rows[e.Row.RowIndex]["FId"].ToString() == "") ? "0" : dt.Rows[e.Row.RowIndex]["FId"].ToString();
                    ddlFloorList.SelectedValue = getFID;
                    ddlFloorList.Items.Insert(0, new ListItem("", "0"));

                 
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                }


            }
            catch { }
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
            {
                try
                {
                    if (ViewState["__WriteAction__"].ToString().Equals("0"))
                    {
                        DropDownList lnkDelete = (DropDownList)e.Row.FindControl("ddlFloorList");
                        lnkDelete.Enabled = false;
                    }

                }
                catch { }
            }

        }

        protected void ddlFloorList_SelectedIndexChanged(object sender,EventArgs e)
        {
            try
            {
                DropDownList drp = (DropDownList)sender;
                GridViewRow gv = (GridViewRow)drp.NamingContainer;
                int rIndex = gv.RowIndex;
                DropDownList ddlFloorList = (DropDownList)gvEmpList.Rows[rIndex].FindControl("ddlFloorList");

                SqlCommand cmd = new SqlCommand("Update ShiftTransferInfoDetails set FId='"+ddlFloorList.SelectedItem.Value+"' where SL="+gvEmpList.DataKeys[rIndex].Value.ToString()+"",sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Searching();
        }

        private void Searching()
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("select SL,EmpCardNo,EmpName,DptName,sftName,Fname,FId,Notes from v_ShiftTransferInfoDetails where STId='" + ddlDepartmentList.SelectedItem.Value + "' AND Format(SDate,'dd-MM-yyyy')='" + txtDate.Text.Trim() + "' AND IsWeekend='False' ", dt);
                gvEmpList.DataSource = dt;
                gvEmpList.DataBind();

                lblTotalEmployee.Text = dt.Rows.Count.ToString();
            }
            catch { }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            GenerateReportData();
        }
        private void GenerateReportData() 
        {
            dt = new DataTable();
            sqlDB.fillDataTable("select SftName,SftId,EmpId,EmpName,substring(EmpCardNo,8,12)as EmpCardNo,FName,DptName,Format(SDate,'dd-MM-yyyy')as SDate,Day,EmpType,DsgName,IsWeekend from v_ShiftTransferInfoDetails where STId='" + ddlDepartmentList.SelectedItem.Value + "' AND Format(SDate,'dd-MM-yyyy')='" + txtDate.Text.Trim() + "' ORDER BY SftName, DptName, SDate,EmpCardNo ", dt);
            Session["__ShiftTarnsferReport__"] = dt;
            if (dt.Rows.Count < 1) 
            {
                lblMessage.InnerText = "warning-> Any record are not founded";
                return;
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ShiftTarnsferReport-"+ddlCompanyList.SelectedValue+"');", true);  //Open New Tab for Sever side code
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadRecentloyTransferShiftDepartment();
        }

        protected void chkLoadAllShiftList_CheckedChanged(object sender, EventArgs e)
        {
            loadRecentloyTransferShiftDepartment();
        }
    }
}