using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ComplexScriptingSystem;

namespace SigmaERP.personnel
{
    public partial class shift_manage_reportByDateRange : System.Web.UI.Page
    {
        DataTable dt;
        DataTable dtSetPrivilege;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";           
            if (!IsPostBack)
            {
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
            }
        }
        private void setPrivilege()
        {
            try
            {

                ViewState["__ReadAction__"] = "1";

                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string getUserId = getCookies["__getUserId__"].ToString();
                string cmpID = ViewState["__CompanyId__"].ToString();
                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                {
                    dtSetPrivilege = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='shift_manage_reportByDateRange.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);
                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            btnPreview.CssClass = "css_btn Ptbut"; btnPreview.Enabled = true;
                            btnSearch.CssClass = "css_btn Ptbut"; btnSearch.Enabled = true;
                        }
                        else
                        {
                            // tblGenerateType.Visible = false;
                            WarningMessage.Visible = true;
                            btnPreview.Enabled = false;
                            btnPreview.CssClass = "";
                            btnSearch.CssClass = ""; btnSearch.Enabled = false;
                        }

                    }
                    else
                    {
                        //tblGenerateType.Visible = false;
                        WarningMessage.Visible = true;
                        btnPreview.Enabled = false;
                        btnPreview.CssClass = "";
                        btnSearch.CssClass = ""; btnSearch.Enabled = false;
                    }
                }
                classes.commonTask.LoadBranch(ddlCompany);
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.loadDepartmentListByCompany(ddlDepartment, ddlCompany.SelectedValue);
            }
            catch { }

        }
        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rblReportType.SelectedValue=="1")
            classes.commonTask.LoadGroupByDepartmentInListBox_ForRoster(lstAll, ddlDepartment.SelectedValue, ddlCompany.SelectedValue);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //-------------------------Validation-----------------------------
            if (ddlDepartment.SelectedValue == "0") { lblMessage.InnerText = "warning->Please select any Department!"; return; }
            if (txtDate.Text == "") { lblMessage.InnerText = "warning->Please select From Date!"; txtDate.Focus(); return; }
            if (txtToDtae.Text == "") { lblMessage.InnerText = "warning->Please select To Date!"; txtToDtae.Focus(); return; }
            //--------------------------------------------------------------------------------------------------------------------
            lstAll.Items.Clear();
            lstSelected.Items.Clear();
          //  classes.commonTask.LoadShiftByDepartmentInListBox(lstAll, ddlDepartment.SelectedValue);
            loadShiftByDepartmentAndDateRange(lstAll, ddlDepartment.SelectedValue, txtDate.Text, txtToDtae.Text);
            if (lstAll.Items.Count<1)
            {
                lblMessage.InnerText = "warning-> Any shift are not founded in this date range!";
                return;
            }
        }

        private void loadShiftByDepartmentAndDateRange(ListBox lb,string DepartmentId,string FDate,string TDate) 
        {
            try
            {
                sqlDB.fillDataTable("select STId,' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) '+SftName as SftName from v_ShiftTransferInfo_DepartmetnList where DptId='" + DepartmentId + "' and (( Convert(datetime,TFromDate,105) between Convert(datetime,'" + FDate + "',105) AND Convert(datetime,'" + TDate + "',105)) or (Convert(datetime,TToDate,105) between Convert(datetime,'" + FDate + "',105) AND Convert(datetime,'" + TDate + "',105)) )", dt = new DataTable());
              
                lb.DataTextField = "SftName";
                lb.DataValueField = "STId";
                lb.DataSource = dt;
                lb.DataBind();
            }
            catch { }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                //-------------------------Validation-----------------------------
                if (ddlDepartment.SelectedValue == "0" && txtCardNo.Text=="") { lblMessage.InnerText = "warning->Please select any Department!"; return; }
                if (txtDate.Text == "") { lblMessage.InnerText = "warning->Please select From Date!"; txtDate.Focus(); return; }
                if (txtToDtae.Text == "") { lblMessage.InnerText = "warning->Please select To Date!"; txtToDtae.Focus(); return; }               
                //-----------------------------------------------------------------
                if (rblReportType.SelectedValue == "0")
                    generateShiftManageReport();
                else generateShifScheduleDetailsReport();
               
            }
            catch { }
        }
        private void generateShiftManageReport() 
        {
            if (lstSelected.Items.Count < 1) { lblMessage.InnerText = "warning->Please select any Shift!"; lstSelected.Focus(); return; }
            string ShiftList = classes.commonTask.getDepartmentList(lstSelected);
            sqlDB.fillDataTable("select SftName+' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) ' as SftName,SftId,EmpId,EmpName,substring(EmpCardNo,8,12)as EmpCardNo,FName,DptName,Format(SDate,'dd-MM-yyyy')as SDate,Day,EmpType,DsgName,IsWeekend from v_ShiftTransferInfoDetails where CompanyId='" + ddlCompany.SelectedValue + "' and STId " + ShiftList + " and DptId='" + ddlDepartment.SelectedValue + "' and  Convert(datetime,SDate,105) between Convert(datetime,'" + txtDate.Text + "',105) AND Convert(datetime,'" + txtToDtae.Text + "',105) ORDER BY SftName, DptName, SDate,EmpCardNo ", dt = new DataTable());
            Session["__ShiftTarnsferReport__"] = dt;
            if (dt == null || dt.Rows.Count < 1)
            {
                lblMessage.InnerText = "warning-> Any record are not founded";
                return;
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ShiftTarnsferReport-" + ddlCompany.SelectedValue + "');", true);  //Open New Tab for Sever side code

        }
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.commonTask.loadDepartmentListByCompany(ddlDepartment, ddlCompany.SelectedValue); 
        }
        private void generateShifScheduleDetailsReport() 
        {
            string[] FDate = txtDate.Text.Trim().Split('-');
            string[] TDate = txtToDtae.Text.Trim().Split('-');
            string sql = "";
            if (txtCardNo.Text == "")
            {
                if (lstSelected.Items.Count < 1) { lblMessage.InnerText = "warning->Please Select Any Group"; return; }
                string GroupList = classes.commonTask.getDepartmentList(lstSelected);
                sql = "SELECT ed.Day, ed.IsWeekend, ed.GId,ed.GName, ed.Notes, Ed.CompanyName,Ed.Address, ED.EmpId,format(ED.SDate,'dd-MM-yyyy') as SDate,ED.DptId,substring (ED.EmpCardNo,8,15) as EmpCardNo,ED.DsgName,Ed.DptName," +
               "ED.EmpName, (SELECT  Case when Es.FName Is NULL then ES.SftName +'  |  ' else ES.SftName+' ['+Es.FName+']'+'  |  ' end  " +
               "FROM v_ShiftTransferInfoDetails ES " +
               "WHERE ES.EmpId = ED.EmpId and ES.SDate=ED.SDate   and ES.DptId=ED.DptId " +
               " order by Es.FId,es.SftId FOR XML PATH(''))[SftName] " +
               "FROM v_ShiftTransferInfoDetails ED " +
               "where  ED.SDate>='" + FDate[2] + "-" + FDate[1] + "-" + FDate[0] + "' and ED.SDate<='" + TDate[2] + " - " + TDate[1] + " - " + TDate[0] + "'  AND ED.DptId='" + ddlDepartment.SelectedValue + "' and Ed.GId "+GroupList+"  " +
               "GROUP BY ED.EmpName,ED.EmpId ,ED.DptId,ED.SDate,ED.EmpCardNo,ED.DsgName,Ed.DptName,Ed.CompanyName,Ed.Address,ed.Notes,ed.GId,ed.GName,ED.FId,ED.SftId,ed.IsWeekend,ed.Day " +
               " order by ed.SDate, ED.FId,ED.SftId,ED.EmpCardNo";
            }
            else 
            {
                if (txtCardNo.Text.Trim().Length < 4)
                {
                    lblMessage.InnerText = "warning-> Please Type Valid Card No (minimum last 4 character)"; return;
                }
                sql = "SELECT ed.Day, ed.IsWeekend,ed.GId,ed.GName,ed.Notes, Ed.CompanyName,Ed.Address,ED.EmpId,format(ED.SDate,'dd-MM-yyyy') as SDate,ED.DptId,substring (ED.EmpCardNo,8,15) as EmpCardNo,ED.DsgName,Ed.DptName," +
             "ED.EmpName, (SELECT  Case when Es.FName Is NULL then ES.SftName +'  |  ' else ES.SftName+' ['+Es.FName+']'+'  |  ' end " +
             "FROM v_ShiftTransferInfoDetails ES " +
             "WHERE ES.EmpId = ED.EmpId and ES.SDate=ED.SDate order by es.FId,es.SftId " +
             "FOR XML PATH(''))[SftName] " +
             "FROM v_ShiftTransferInfoDetails ED " +
             "where  ED.SDate>='" + FDate[2] + "-" + FDate[1] + "-" + FDate[0] + "' and ED.SDate<='" + TDate[2] + " - " + TDate[1] + " - " + TDate[0] + "'  AND ED.EmpCardNo like'%"+txtCardNo.Text+"' " +
             "GROUP BY ED.EmpName,ED.EmpId ,ED.DptId,ED.SDate,ED.EmpCardNo,ED.DsgName,Ed.DptName,Ed.CompanyName,Ed.Address,ed.Notes,ed.GId,ed.GName,ed.IsWeekend,ed.Day order by ED.SDate";
            }          
            sqlDB.fillDataTable(sql, dt = new DataTable());           
            if (dt == null || dt.Rows.Count < 1)
            {
                lblMessage.InnerText = "warning-> Any record are not founded";
                return;
            }
            Session["__ShiftScheduleDetails__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ShiftScheduleDetails');", true);  //Open New Tab for Sever side code

           
        }

        protected void rblReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstAll.Items.Clear();
            lstSelected.Items.Clear();
            if (rblReportType.SelectedValue == "1")
            {
                txtCardNo.Visible = true;
                btnSearch.Visible = false;
                lnkNew.Visible = true;
            }
            else 
            {
                txtCardNo.Text = "";
                txtCardNo.Visible = false;                
                btnSearch.Visible = true;
                lnkNew.Visible = false;
            }
        }
    }
}