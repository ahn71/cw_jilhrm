using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class roster_missing : System.Web.UI.Page
    {
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
                classes.commonTask.LoadGrouping(ddlGroupList);
            }
        }

        private void setPrivilege()
        {
            try
            {

                ViewState["__DeleteAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyWriteAction(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "roster_missing.aspx",ddlCompanyList,gvEmpList);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];   

                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
               // classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue);
               
            }
            catch { }

        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                classes.commonTask.LoadShiftForSMOperation_WithAll(ddlAssignShift, ddlCompanyList.SelectedValue, ddlDepartmentList.SelectedValue);

            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDate.Text.Trim().Length<10)
                {
                    lblMessage.InnerText = "error->Please select roster date";txtDate.Focus();
                }
                DateTime RosterDate = DateTime.ParseExact(txtDate.Text.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                loadRoster_MissingList(RosterDate);
                classes.commonTask.LoadAssignedShiftList_BySearchTToDate(ddlDepartmentList.SelectedValue, ddlGroupList.SelectedValue, ddlCompanyList.SelectedValue, ddlExistsShiftList, RosterDate.ToString("yyyy-MM-dd"));
            }
            catch { }
        }

        private void loadRoster_MissingList(DateTime RosterDate)
        {
            try
            {
               
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select pes.EmpCardNo,pes.EmpName,pes.DsgName,pes.EmpId,pes.DptId,pes.DsgId,pes.EmpTypeId,pes.GId,pes.EmpType from v_Personnel_EmpCurrentStatus as pes where DptId='"+ddlDepartmentList.SelectedValue+"' And GId='"+ddlGroupList.SelectedValue+"' AND EmpDutyType='Roster' And EmpStatus in(1,8) AND EmpId  " +
                    " not in  (select EmpId from ShiftTransferInfoDetails where DptId='" + ddlDepartmentList.SelectedValue + "' And GId='" + ddlGroupList.SelectedValue + "' AND SDate='" + RosterDate.ToString("yyyy-MM-dd") + "')", dt);

                gvEmpList.DataSource = dt;
                gvEmpList.DataBind();
            }
            catch { }
        }

        protected void gvEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
            }

            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                try
                {
                    if (ViewState["__WriteAction__"].ToString().Equals("0"))
                    {
                        ImageButton btnDelete = (ImageButton)e.Row.FindControl("iAdd");                     
                        btnDelete.Enabled = false; 
                    }

                }
                catch { }

            }
        }

        protected void gvEmpList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("RosterAdd"))
                {
                    chkForDate.Text = string.Format("Just roster for {0}",txtDate.Text.Trim());
                    ddlExistsShiftList.SelectedIndex = 0;
                    chkForFullRoster.Checked = true;
                    chkForDate.Checked = false;
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    ViewState["__EmpId__"] = gvEmpList.DataKeys[rIndex].Values[0];                 
                    ViewState["__DptId__"]= gvEmpList.DataKeys[rIndex].Values[1];
                    ViewState["__DsgId__"] = gvEmpList.DataKeys[rIndex].Values[2];
                    ViewState["__EmpTypeId__"]= gvEmpList.DataKeys[rIndex].Values[3];
                    ViewState["__GId__"] = gvEmpList.DataKeys[rIndex].Values[4];
                    ViewState["__rIndex__"] = rIndex;

                    ModelPopupExtender.Show();

                                                                                                                         
                }
                
            }
            catch { }
        }

        protected void chkForFullRoster_CheckedChanged(object sender, EventArgs e)
        {
            chkForDate.Checked = false;
            chkForFullRoster.Checked = true;
            ModelPopupExtender.Show();
        }

        protected void chkForDate_CheckedChanged(object sender, EventArgs e)
        {
            chkForDate.Checked = true;
            chkForFullRoster.Checked = false;
            ModelPopupExtender.Show();
        }

        protected void ddlGroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                classes.commonTask.loadDepartmentListByCompanyAndGroup(ddlDepartmentList, ddlCompanyList.SelectedValue, ddlGroupList.SelectedValue);
            }
            catch { }
        }

        protected void ddlExistsShiftList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlExistsShiftList.SelectedValue.ToString() == "0") return;
                else
                {
                  if (Assigne_EmpIn_RunningShift())
                    {
                        gvEmpList.Rows[int.Parse(ViewState["__rIndex__"].ToString())].Visible = false;
                        lblMessage.InnerText = "success->Successfully roster assigned";
                    }
                }
            }
            catch { }
        }

        private bool Assigne_EmpIn_RunningShift()
        {
            try
            {
                bool ExecutionResult = false;
                string [] JustDateParts = ddlExistsShiftList.SelectedItem.Text.Trim().Split('|');
                string[] FTDates = JustDateParts[1].Trim().Split('-');              
                if (chkForFullRoster.Checked)
                {
                    int gotMonth = DateTime.ParseExact(FTDates[0].Trim(), "MMM", CultureInfo.InvariantCulture).Month;
                    string Month = (gotMonth.ToString().Length == 1) ? "0" + gotMonth.ToString() : gotMonth.ToString();
                    gotMonth = int.Parse(FTDates[2].Substring(0, 4));

                    DateTime FromDate = DateTime.ParseExact(FTDates[1] + "-" + Month + "-" + gotMonth.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    FTDates[2] = FTDates[2].Trim().Substring(FTDates[2].Trim().LastIndexOf(' '), 4);
                    gotMonth = DateTime.ParseExact(FTDates[2].Trim(), "MMM", CultureInfo.InvariantCulture).Month;
                    Month = (gotMonth.ToString().Length == 1) ? "0" + gotMonth.ToString() : gotMonth.ToString();
                    DateTime ToDate = DateTime.ParseExact(FTDates[3] + "-" + Month + "-" + FTDates[4], "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    while (FromDate <= ToDate)
                    {

                        SqlCommand cmd = new SqlCommand("delete from ShiftTransferInfoDetails where EmpId='" + ViewState["__EmpId__"].ToString() + "' AND Format(SDate,'dd-MM-yyyy')='" + FromDate.ToString("dd-MM-yyyy") + "' AND STId=" + ddlAssignShift.SelectedValue + "", sqlDB.connection);
                        cmd.ExecuteNonQuery();

                        string[] getColumns = { "STId", "SDate", "EmpId", "DptId", "DsgId", "EmpTypeId", "CompanyId", "GId" };
                        string[] getValues = { ddlExistsShiftList.SelectedValue,convertDateTime.getCertainCulture(FromDate.ToString("dd-MM-yyyy")).ToString(),ViewState["__EmpId__"].ToString(),
                                         ViewState["__DptId__"].ToString(),ViewState["__DsgId__"].ToString(),ViewState["__EmpTypeId__"].ToString(),ddlCompanyList.SelectedValue,ViewState["__GId__"].ToString()};
                        SQLOperation.forSaveValue("ShiftTransferInfoDetails", getColumns, getValues, sqlDB.connection);
                        FromDate = FromDate.AddDays(1);
                        ExecutionResult = true;
                    }

                }
                else
                {


                    SqlCommand cmd = new SqlCommand("delete from ShiftTransferInfoDetails where EmpId='" + ViewState["__EmpId__"].ToString() + "' AND Format(SDate,'dd-MM-yyyy')='" + txtDate.Text + "' AND STId=" + ddlAssignShift.SelectedValue + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();

                    string[] getColumns = { "STId", "SDate", "EmpId", "DptId", "DsgId", "EmpTypeId", "CompanyId", "GId" };
                    string[] getValues = { ddlExistsShiftList.SelectedValue,convertDateTime.getCertainCulture(txtDate.Text.Trim()).ToString(),ViewState["__EmpId__"].ToString(),
                                          ViewState["__DptId__"].ToString(),ViewState["__DsgId__"].ToString(),ViewState["__EmpTypeId__"].ToString(),ddlCompanyList.SelectedValue,ViewState["__GId__"].ToString()};
                    SQLOperation.forSaveValue("ShiftTransferInfoDetails", getColumns, getValues, sqlDB.connection);

                    ExecutionResult = true;
                }
                return ExecutionResult;
            }
            catch { return false; }
        }
    }
}