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
    public partial class employee_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                loadYear();
                setPrivilege();
                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
              
                LoadAllEmployeeList("Select SN, EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpProximityNo From v_EmployeeDetails where EmpStatus in ('1','8') and ActiveSalary='True' and IsActive=1 order by EmpCardNo");
            }
        }
        private void loadYear()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select distinct FORMAT(EmpJoiningDate,'yyyy')as year from v_EmployeeDetails order by year DESC", dt = new DataTable());
                ddlChoseYear.DataTextField = "year";
                ddlChoseYear.DataValueField = "year";
                ddlChoseYear.DataSource = dt;
                ddlChoseYear.DataBind();
                // ddlChoseYear.SelectedIndex = 0;
                ddlChoseYear.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                
                // below part for supper admin and master admin.there must be controll everythings.remember that super admin not seen another super admin information
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                {

                    classes.commonTask.LoadBranch(ddlCompanyList);
                    classes.commonTask.LoadShift(ddlShift, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ViewState["__CompanyId__"].ToString());
                    return;
                }

                else    // below part for admin and viewer.while admin just write info and viewer just see information.its for by default settings
                {

                    classes.commonTask.LoadBranch(ddlCompanyList, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.LoadShift(ddlShift, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ViewState["__CompanyId__"].ToString());

                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                    {
                        //gvLeaveList.Visible = false; ;
                        //divElementContainer.EnableViewState = false;
                    }

                    //  here set privilege by setting master admin or supper admin 

                    /*dtSetPrivilege = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='week_end_list_all.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);

                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            gvLeaveList.Visible = true;
                            divElementContainer.EnableViewState = true;
                        }


                    }
                    */

                }


            }
            catch { }
        }
        private void LoadAllEmployeeList(string cmd)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable(cmd, dt);

                gvEmployeeList.DataSource = dt;
                gvEmployeeList.DataBind();
                    
            }
            catch
            {}
        }
     /*   private void LoadAllEmployeeList(string cmd)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable(cmd, dt);
                int totalRows = dt.Rows.Count;
                string divInfo = "";


                if (totalRows == 0)
                {
                    divInfo = "<div class='noData'>No Employee available</div>";
                    divInfo += "<div class='dataTables_wrapper'><div class='head'></div></div>";
                    divEmployeeList.Controls.Add(new LiteralControl(divInfo));
                    return;
                }
                divInfo = " <table id='tblClassList' class='display'  > ";
                divInfo += "<thead>";
                divInfo += "<tr>";
                divInfo += "<th>Emp ID</th>";
                divInfo += "<th>Emp Name</th>";
                divInfo += "<th>Joining Date</th>";
                divInfo += "<th>Department</th>";
                divInfo += "<th>Designation</th>";
                divInfo += "<th>Shift</th>";
                divInfo += "<th>Shift Start</th>";
                divInfo += "<th>Emp Status</th>";
                divInfo += "<th>Proximity ID</th>";

                divInfo += "<th>Alter</th>";
                divInfo += "<th>Delete</th>";
                divInfo += "</tr>";

                divInfo += "</thead>";

                divInfo += "<tbody>";
                string id = "";

                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    DataTable dtall = new DataTable();
                    sqlDB.fillDataTable("Select EmpID,SN,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatus,EmpProximityNo From v_EmployeeDetails where EmpId='" + dt.Rows[x]["EmpID"].ToString() + "' and SN=" + dt.Rows[x]["SN"].ToString() + "", dtall);
                    if (dtall.Rows.Count > 0)
                    {
                        id = dtall.Rows[0]["EmpID"].ToString();
                        divInfo += "<tr id='r_" + id + "'>";
                        divInfo += "<td >" + dtall.Rows[0]["EmpCardNo"].ToString() + "</td>";
                        divInfo += "<td>" + dtall.Rows[0]["EmpName"].ToString() + "</td>";
                        divInfo += "<td>" + dtall.Rows[0]["EmpJoiningDate"].ToString() + "</td>";
                        divInfo += "<td >" + dtall.Rows[0]["DptName"].ToString() + "</td>";
                        divInfo += "<td>" + dtall.Rows[0]["DsgName"].ToString() + "</td>";
                        divInfo += "<td>" + dtall.Rows[0]["SftName"].ToString() + "</td>";
                        divInfo += "<td>" + dtall.Rows[0]["EmpShiftStartDate"].ToString() + "</td>";
                        divInfo += "<td>" + dtall.Rows[0]["EmpStatus"].ToString() + "</td>";
                        divInfo += "<td>" + dtall.Rows[0]["EmpProximityNo"].ToString() + "</td>";

                        divInfo += "<td>" + "<img src='/Images/datatable/edit.png' class='editImg' title='Alter'  onclick='editEmployee(" + id + ");'  />";
                        divInfo += "<td id='deleterow' style='max-width:30px;min-width:30px;'><img class='delete'  src='/images/action/error.png' title='Delete' onclick='return deleteRow(" + id + " );'/></td>";
                    }
                }

                divInfo += "</tbody>";
                divInfo += "<tfoot>";

                divInfo += "</table>";
                divInfo += "<div class='dataTables_wrapper'><div class='head'></div></div>";
                divEmployeeList.Controls.Add(new LiteralControl(divInfo));
            }
            catch { }
        }
        */
      

        protected void btnAllEmployeeClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goURL('/personnel/employee.aspx')", true);
        }

        protected void gvEmployeeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                SearchingInEmployeeList();  
            }
            catch { }
            gvEmployeeList.PageIndex = e.NewPageIndex;
            gvEmployeeList.DataBind();
        }

        protected void gvEmployeeList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rIndex =int.Parse(e.CommandArgument.ToString());
            string getId = gvEmployeeList.DataKeys[rIndex].Values[0].ToString();
            if (e.CommandName == "Alter")
            {
               //Hiddet = Convert.ToInt32(e.CommandArgument);
                
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goURL('/personnel/employee.aspx?EmpId=" + getId + "&Edit=True')", true);
            }
            else if (e.CommandName == "Delete")
            {

                DataTable dt;
                string EmpCardno = gvEmployeeList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text.ToString();
                sqlDB.fillDataTable("Select EmpCardNo From Personnel_EmpCurrentStatus where EmpId='" + getId + "'", dt = new DataTable());
                //string EmpCardno = dt.Rows[0].ItemArray[0].ToString();
                DeleteEmployee(getId, EmpCardno);
                gvEmployeeList.Rows[rIndex].Visible = false;
            }
        }
        private void DeleteEmployee(string EmpId, string CardNo)
        {
            try
            {
                SqlCommand deletecmd = new SqlCommand("Delete From Personnel_EmployeeInfo where EmpId='" + EmpId + "'", sqlDB.connection);
                int result = (int)deletecmd.ExecuteNonQuery();

                //if (result > 0)
                //{
                //    SearchingInEmployeeList();        
                //}
            }
            catch { }
        }

        protected void btntest_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "confirmDelete();", true);
        }

        protected void gvEmployeeList_RowDataBound(object sender, GridViewRowEventArgs e)
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
        }
        /*Nayem.......
         .............For Searching............*/
        private void SearchingInEmployeeList() 
        {
            try
            {
              
                if (txtFromDate.Text.Trim().Length != 0 || txtToDate.Text.Trim().Length != 0)
                {
                    string[] dates = txtFromDate.Text.Trim().Split('-');
                    ViewState["__FDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    dates = txtToDate.Text.Trim().Split('-');
                    ViewState["__TDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    ddlChoseYear.SelectedIndex = 0;
                }
                if ((txtCardNo.Text.Trim() == "" || txtCardNo.Text.Trim() != "") && ddlCompanyList.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && ((txtToDate.Text.Trim() != "" && txtFromDate.Text.Trim() != "") || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                {
                    lblMessage.InnerText = "warning-> Please, Select Department Name.";
                    return;
                }
                if (ddlCompanyList.SelectedItem.Text.Trim() == "")
                {
                    ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                }
                DataTable dt = new DataTable();
                //1. Search by Company, CardNo.
                if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and EmpCardNo='" + txtCardNo.Text.Trim() + "' order by EmpCardNo", dt = new DataTable());
                //2. Search by Company,Department,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and EmpCardNo='" + txtCardNo.Text.Trim() + "' order by EmpCardNo", dt = new DataTable());
                // 3. Search by Company,Department,Shift  
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && ddlChoseYear.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0)
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' order by EmpCardNo", dt = new DataTable());
                // 4. Search by Company,Department,Shift,CardNo 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo='" + txtCardNo.Text.Trim() + "' order by EmpCardNo", dt = new DataTable());
                //5. Search by Company,Department,Shift,CardNo,From Date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo='" + txtCardNo.Text.Trim() + "'and EmpJoiningDate>='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by EmpCardNo", dt = new DataTable());
                //6. Search by Company,Department,Shift,CardNo,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() != "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo='" + txtCardNo.Text.Trim() + "' and FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by EmpCardNo", dt = new DataTable());
                //7. Search by Company,Department,Shift,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() != "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0)
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by EmpCardNo", dt = new DataTable());
                //8. Search by Company,Department,Shift,From date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() != "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpJoiningDate>='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by EmpCardNo", dt = new DataTable());
                //9. Search by Company, Department
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' order by EmpCardNo", dt = new DataTable());
                //10. Search by Company, CardNo,From date,To date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and EmpJoiningDate >='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' and EmpCardNo='" + txtCardNo.Text.Trim() + "' order by EmpCardNo", dt = new DataTable());
                //11. Search by Company,Department,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && ddlChoseYear.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and  FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by EmpCardNo", dt = new DataTable());
                //12.  Search by Company, Shift
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' order by EmpCardNo", dt = new DataTable());
                //13.  Search by Company, Shift,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo='" + txtCardNo.Text.Trim() + "' order by EmpCardNo", dt = new DataTable());
                //14. Search by Company, Department, FromDate,ToDate
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and EmpJoiningDate >='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by EmpCardNo", dt = new DataTable());
                //15. Search by Company, Department, FromDate,ToDate,Card no.
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and EmpJoiningDate >='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "'and EmpCardNo='" + txtCardNo.Text.Trim() + "' order by EmpCardNo", dt = new DataTable());
                //16. Seardh by Company,FromDate,ToDate
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() == "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "' and EmpJoiningDate >='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by EmpCardNo", dt = new DataTable());
      

                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry,Any record are not founded";
                    gvEmployeeList.DataSource = null;
                    gvEmployeeList.DataBind();
                    return;
                }
                gvEmployeeList.DataSource = dt;
                gvEmployeeList.DataBind();

            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchingInEmployeeList();
        }

        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingInEmployeeList();
        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingInEmployeeList();
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCompanyList.SelectedValue == "0000")
            {
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
            classes.commonTask.LoadShift(ddlShift, ddlCompanyList.SelectedValue.ToString());
            classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue.ToString());
            SearchingInEmployeeList();
        }

        protected void ddlChoseYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            SearchingInEmployeeList();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            allClear();
        }
        private void allClear()
        {
            lblMessage.InnerText = "";
            ddlChoseYear.SelectedIndex = 0;
            ddlCompanyList.SelectedIndex = 0;
            ddlDepartmentList.SelectedIndex = -1;
            ddlShift.SelectedIndex = -1;
            txtToDate.Text = "";
            txtFromDate.Text = "";
            txtCardNo.Text = "";
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllEmployeeList("Select SN, EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpProximityNo From v_EmployeeDetails where EmpStatus in ('1','8') and ActiveSalary='True' and IsActive='1' order by EmpCardNo");
            loadYear();
        }

        //protected void gvEmployeeList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    if ((ddlCompanyList.SelectedItem.Text != "" && ddlShift.SelectedItem.Text != "") || (ddlCompanyList.SelectedItem.Text != "" && txtCardNo.Text.Trim() != "")) SearchingInEmployeeList();
        //    else 
        //    LoadAllEmployeeList("Select SN, EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpProximityNo From v_EmployeeDetails where EmpStatus in ('1','8') and ActiveSalary='True' and IsActive=1 order by EmpCardNo");
        //}

    }
}