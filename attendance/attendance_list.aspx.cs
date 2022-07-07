using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System.Drawing;

namespace SigmaERP.attendance
{
    public partial class attendance_list : System.Web.UI.Page
    {
        DataTable dt;
       static  DataTable dtSetPrivilege;
        static byte  searchStatus; 
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            
            if (!IsPostBack)
            {
                ViewState["__LineORGroupDependency__"] = classes.commonTask.GroupORLineDependency();
                //string script = "$(document).ready(function () { $('[id*=btnForGet]').click(); });";
                //ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);

                setPrivilege();
                if (ViewState["__LineORGroupDependency__"].ToString().Equals("False"))
                    classes.commonTask.LoadGrouping(ddlGrouping, ViewState["__CompanyId__"].ToString());
                loadAttendanceList();
                //SearchAttendanceList();               
                loadYear();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
        }

        //DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForList(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "attendance.aspx", ddlCompanyList, gvAttendanceList, btnSearch);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];   
                ViewState["__DeletAction__"] = AccessPermission[3];
                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentName, ViewState["__CompanyId__"].ToString());
            }
            catch { }
        }

        private void loadAttendanceList()
        {
            try
            {
                searchStatus = 1;   // 1 means all load at first time
                sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where Year='" + DateTime.Now.Year + "' and CompanyId='" + ViewState["__CompanyId__"].ToString() + "' order by AttDate desc", dt = new DataTable());
                if(dt.Rows.Count<1)
                {
                    gvAttendanceList.DataSource = null;
                    gvAttendanceList.DataBind();
                    divRecordMessage.InnerText = "Any Attendance Record Are Not Founded!";
                    divRecordMessage.Visible = true;
                }
                gvAttendanceList.DataSource = dt;
                gvAttendanceList.DataBind();
            }
            catch { }
        }

       
        private void loadYear()
        {
            try
            {
                sqlDB.fillDataTable("select distinct Year from v_tblAttendanceRecord",dt=new DataTable ());
                ddlChoseYear.DataTextField = "Year";
                ddlChoseYear.DataValueField = "Year";
                ddlChoseYear.DataSource = dt;
                ddlChoseYear.DataBind();
                ddlChoseYear.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

        private void searchByEmpCardNo_Others()
        {
            
        }
        protected void btnSearchByFromDateToDate_Click(object sender, EventArgs e)
        {
            searchByDateRange();
        }
        private void searchByDateRange()
        {
            
        }
        protected void gvAttendanceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (searchStatus == (byte)1) loadAttendanceList();
                else if (searchStatus == (byte)2) searchByEmpCardNo_Others();
                else searchByDateRange();

                gvAttendanceList.PageIndex = e.NewPageIndex;
                gvAttendanceList.DataBind();
            }
            catch { }
        }

        protected void gvAttendanceList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    lblMessage.InnerText = "";
                    string [] getValus = e.CommandArgument.ToString().Split(',');
                   

                    SqlCommand cmd = new SqlCommand("delete from tblAttendanceRecord where EmpId='"+getValus[0]+"' And Attdate='"+getValus[2].Substring(6,4)+"-"+getValus[2].Substring(3,2)+"-"+getValus[2].Substring(0,2)+"'",sqlDB.connection);
                    int result=cmd.ExecuteNonQuery();
                    if (result >0)
                    {
                        ChangeLeaveStatusByeEmpIdAndDate(getValus[0],getValus[3],getValus[2].Substring(6, 4) + "-" + getValus[2].Substring(3, 2) + "-" + getValus[2].Substring(0, 2));
                        lblMessage.InnerText = "success->Successfully Deleted";
                        if (searchStatus == (byte)1) loadAttendanceList();
                        else if (searchStatus == (byte)2) searchByEmpCardNo_Others();
                        else searchByDateRange();
                    }
                }

                if (e.CommandName.Equals("Alter"))
                {
                    lblMessage.InnerText = "";
                    int index = Convert.ToInt32(e.CommandArgument.ToString());
                    string EmpId = gvAttendanceList.DataKeys[index].Values[0].ToString();
                    string EmpCardNo = gvAttendanceList.DataKeys[index].Values[1].ToString();
                    string AttDate = gvAttendanceList.DataKeys[index].Values[2].ToString();
                    string AttStatus = gvAttendanceList.DataKeys[index].Values[3].ToString();
                    string InTime = gvAttendanceList.DataKeys[index].Values[4].ToString();
                    string OutTime = gvAttendanceList.DataKeys[index].Values[5].ToString();
                    string EmpType = gvAttendanceList.DataKeys[index].Values[6].ToString();
                    string EmpName = gvAttendanceList.DataKeys[index].Values[7].ToString();
                    string StateStatus = gvAttendanceList.DataKeys[index].Values[8].ToString();
                 

                    Response.Redirect("/attendance/attendance.aspx?eid_cn_at=" + EmpId + "_" + EmpCardNo + "_" + AttDate + "_" + AttStatus + "_" + InTime + "_" + OutTime + "_" + EmpType + "_" + EmpName + "_" + StateStatus + "");
                }
            }
            catch { }
        }

        private void ChangeLeaveStatusByeEmpIdAndDate(string EmpId,string AttStatus,string AttDate)
        {
            try
            {
                if (AttStatus.Equals("lv"))
                {
                    sqlDB.fillDataTable("select LACode from Leave_LeaveApplicationDetails where LeaveDate='"+AttDate+"' AND EmpId='"+EmpId+"'",dt=new DataTable ());
                    if (dt.Rows.Count > 0)
                    {
                        SqlCommand cmd = new SqlCommand("Update Leave_LeaveApplicationDetails set Used='0' where EmpId='"+EmpId+"' AND LeaveDate='"+AttDate+"'",sqlDB.connection);
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("Update Leave_LeaveApplication set IsProcessessed='0' where LACode=" + dt.Rows[0]["LACode"].ToString() + " AND EmpId='" + EmpId + "' AND ToDate='"+AttDate+"'",sqlDB.connection);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        protected void gvAttendanceList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (searchStatus == (byte)1) loadAttendanceList();
                else if (searchStatus == (byte)2) searchByEmpCardNo_Others();
                else searchByDateRange();
            }
            catch { }
        }

        protected void btnForGet_Click(object sender, EventArgs e)
        {
            loadAttendanceList();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchAttendanceList();
        }
        private void SearchAttendanceList()
        {
            try
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                lblMessage.InnerText = "";
                //if (ddlCompanyList.SelectedIndex == 0 && txtCardNo.Text.Trim().Length == 0)
                //{
                //    lblMessage.InnerText = "warning-> Please, Select Company Name.";
                //    return;
                //}
                if (txtCardNo.Text.Trim() != "") 
                {
                    if (txtCardNo.Text.Length < 6)
                    { lblMessage.InnerText = "warning-> Please Type Employee Card No Minimum 6 Character!"; return; }
                }
                if (txtCardNo.Text.Trim() == ""  && ddlCompanyList.Text.Trim() != "" && ddlShift.SelectedIndex<1 && (ddlDepartmentName.SelectedIndex == -1 || ddlDepartmentName.SelectedIndex == 0) && ((txtToDate.Text.Trim() != "" && txtFromDate.Text.Trim() != "") || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                {
                    lblMessage.InnerText = "warning-> Please, Select a Department.";
                    return;
                }



                if (txtFromDate.Text.Trim().Length != 0 || txtToDate.Text.Trim().Length != 0)
                {
                    string[] dates = txtFromDate.Text.Trim().Split('-');
                    ViewState["__FDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    dates = txtToDate.Text.Trim().Split('-');
                    ViewState["__TDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    ddlChoseYear.SelectedIndex=0;
                }
                if (ddlCompanyList.SelectedItem.Text.Trim() == "")
                {
                    ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                }
                string ListType = (rblAttListType.SelectedValue == "0") ? "" : " and OutDuty="+rblAttListType.SelectedValue+"";
                //0. Search by Company
                if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentName.SelectedIndex == -1 || ddlDepartmentName.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length== 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord  where CompanyId='" + ddlCompanyList.SelectedValue + "' " + ListType + " order by AttDate desc", dt = new DataTable());
           
                 //1. Search by Company, Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentName.SelectedIndex == -1 || ddlDepartmentName.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord  where CompanyId='" + ddlCompanyList.SelectedValue + "'and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  //2. Search by Company,Department,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord  where CompanyId='" + ddlCompanyList.SelectedValue + "' and DptId='" + ddlDepartmentName.SelectedValue + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  //3. Search by Company,Shift,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentName.SelectedIndex == -1 || ddlDepartmentName.SelectedIndex == 0) && ddlShift.SelectedIndex>0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord  where CompanyId='" + ddlCompanyList.SelectedValue + "' and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                     //4. Search by Company,Department,Shift  
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtCardNo.Text.Trim().Length == 0 && ddlChoseYear.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                     //5. Search by Company,Department,Shift,CardNo 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                    //6. Search by Company,Department,Shift,CardNo,From Date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "'and ATTDate>='" + ViewState["__FDate__"].ToString() + "' and ATTDate<='" + ViewState["__TDate__"].ToString() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                     //7. Search by Company,Department,Shift,CardNo,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' and Year='" + ddlChoseYear.SelectedValue + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                    //8. Search by Company,Department,Shift,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'and Year='" + ddlChoseYear.SelectedValue + "'  " + ListType + " order by AttDate desc", dt = new DataTable());
                    //9. Search by Company,Department,From date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length == 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "' and ATTDate>='" + ViewState["__FDate__"].ToString() + "' and ATTDate<='" + ViewState["__TDate__"].ToString() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  //10. Search by Company,Department,From date,To Date,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "' and ATTDate>='" + ViewState["__FDate__"].ToString() + "' and ATTDate<='" + ViewState["__TDate__"].ToString() + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  
                  //11. Search by Company,Department,Shift,From date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and ATTDate>='" + ViewState["__FDate__"].ToString() + "' and ATTDate<='" + ViewState["__TDate__"].ToString() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  // 12. Search by Company, Department
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "' " + ListType + " order by AttDate desc", dt = new DataTable());

                      //13.  Search by Company, Shift
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlDepartmentName.SelectedItem.Text.Trim() == "" && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                   //14. Search by Company, CardNo,From date,To date
                else if (ddlCompanyList.SelectedIndex > 0 && (ddlDepartmentName.SelectedIndex == -1 || ddlDepartmentName.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and ATTDate >='" + ViewState["__FDate__"].ToString() + "' and ATTDate<='" + ViewState["__TDate__"].ToString() + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                   //15. Search by Company,Department,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == "") && ddlChoseYear.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and Year='" + ddlChoseYear.SelectedValue + "'  " + ListType + " order by AttDate desc", dt = new DataTable());


                //------------------------------Search By Line or Group------------------------------------------------

                // 4. Search by Company,Department,Shift,Grouping 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length == 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  " + ListType + " order by AttDate desc", dt = new DataTable());

                            // 4. Search by Company,Department,Shift,Grouping ,CardNo
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex ==0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' " + ListType + " order by AttDate desc", dt = new DataTable());


                   //5. Search by Company,Department,Shift,CardNo,From Date,To Date,Grouping
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' and ATTDate>='" + ViewState["__FDate__"].ToString() + "' and ATTDate<='" + ViewState["__TDate__"].ToString() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  //6. Search by Company,Department,Shift,CardNo,Year,Grouping
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' and Year='" + ddlChoseYear.SelectedValue + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  //7. Search by Company,Department,Shift,Year,Grouping
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and Year='" + ddlChoseYear.SelectedValue + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  //8. Search by Company,Department,Shift,From date,To Date,Grouping
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and ATTDate>='" + ViewState["__FDate__"].ToString() + "' and ATTDate<='" + ViewState["__TDate__"].ToString() + "' " + ListType + " order by AttDate desc", dt = new DataTable());


                     // 4. Search by Company,Department,Grouping 
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length == 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " " + ListType + " order by AttDate desc", dt = new DataTable());

                            // 4. Search by Company,Department,Grouping ,CardNo
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' " + ListType + " order by AttDate desc", dt = new DataTable());


                   //5. Search by Company,Department,CardNo,From Date,To Date,Grouping
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "'and ATTDate>='" + ViewState["__FDate__"].ToString() + "' and ATTDate<='" + ViewState["__TDate__"].ToString() + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  //6. Search by Company,Department,CardNo,Year,Grouping
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' and Year='" + ddlChoseYear.SelectedValue + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  //7. Search by Company,Department,Year,Grouping
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and Year='" + ddlChoseYear.SelectedValue + "' " + ListType + " order by AttDate desc", dt = new DataTable());
                  //8. Search by Company,Department,From date,To Date,Grouping
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentName.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) EmpCardNo,MonthId,Format(AttDate,'dd-MM-yyyy') as AttDate,AttStatus,AttManual,InTime,OutTime,EmpType,EmpName,StateStatus from v_tblAttendanceRecord where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentName.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and ATTDate>='" + ViewState["__FDate__"].ToString() + "' and ATTDate<='" + ViewState["__TDate__"].ToString() + "' " + ListType + " order by AttDate desc", dt = new DataTable());

                //------------------------------------------------------------------------------------------------
                    
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry,Any record are not founded";
                    gvAttendanceList.DataSource = null;
                    gvAttendanceList.DataBind();
                    return;
                }
                gvAttendanceList.DataSource = dt;
                gvAttendanceList.DataBind();

            }
            catch { }
        }
        

        protected void btnClear_Click(object sender, EventArgs e)
        {
            allClear();
        }

        private void allClear()
        {
            lblMessage.InnerText = "";
            ddlChoseYear.SelectedIndex = 0;
           // ddlCompanyList.SelectedIndex = 0;
           // ddlDivisionName.SelectedIndex = -1;
            ddlDepartmentName.SelectedIndex = -1;
            ddlShift.SelectedIndex = -1;
            ddlGrouping.SelectedIndex = -1;
            txtToDate.Text = "";
            txtFromDate.Text = "";
            txtCardNo.Text = "";
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
              //  classes.commonTask.loadDivision(ddlDivisionName, ddlCompanyList.SelectedValue.ToString(), "Admin");
                //classes.commonTask.LoadShift(ddlShift, ddlCompanyList.SelectedValue.ToString(), "Admin");
                if (ddlCompanyList.SelectedValue=="0000")
                {
                    ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                }
                classes.commonTask.LoadShift(ddlShift, ddlCompanyList.SelectedValue.ToString());
                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentName, ddlCompanyList.SelectedValue.ToString());
                
                gvAttendanceList.DataSource = null;
                gvAttendanceList.DataBind();
                SearchAttendanceList();

            }
            catch { }
        }

        protected void ddlDivisionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["__LineORGroupDependency__"].ToString().Equals("True"))
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue;
                classes.commonTask.LoadGrouping(ddlGrouping, CompanyId, ddlDepartmentName.SelectedValue);
            }
            classes.commonTask.LoadShiftWithoutInitialByDepartment(ddlShift, ddlCompanyList.SelectedValue, ddlDepartmentName.SelectedValue);
            SearchAttendanceList();
        }

        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchAttendanceList();
        }
        protected void ddlGrouping_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchAttendanceList();
        }
      

        protected void ddlChoseYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            SearchAttendanceList();
        }

        protected void gvAttendanceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                }
            }
            catch { }

            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                Button btn;
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnRemove");
                        btn.Enabled = false;
                        btn.OnClientClick = "return false";
                        btn.ForeColor = Color.Silver;
                    }

                }
                catch { }
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnAlter");
                        btn.Enabled = false;
                        btn.ForeColor = Color.Silver;
                    }

                }
                catch { }

            }

       
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            loadAttendanceList();
            loadYear();
            allClear();
        }

        protected void rblAttListType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchAttendanceList();
        }

      

     
    }
}