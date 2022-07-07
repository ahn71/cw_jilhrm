using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class employee_list1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                ViewState["__LineORGroupDependency__"] = classes.commonTask.GroupORLineDependency();
                loadYear();
                setPrivilege();
                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
               // SearchingInEmployeeList();
                if (ViewState["__LineORGroupDependency__"].ToString().Equals("False"))
                    classes.commonTask.LoadGrouping(ddlGrouping, ViewState["__CompanyId__"].ToString());
                LoadAllEmployeeList("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ViewState["__CompanyId__"].ToString() + "' order by DptCode, CustomOrdering");
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                if (Session["IsRedirect"] != null)
                {
                    if (Session["pageNumber"] != null)
                    {
                        gvForApprovedList.PageIndex = (int)Session["pageNumber"];
                        gvForApprovedList.DataBind();
                    }                    
                }
                Session["IsRedirect"] = null;
                Session["pageNumber"] = gvForApprovedList.PageIndex;
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
                ddlChoseYear.Items.Insert(0, new ListItem(string.Empty, ""));
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
                ViewState["__getEmpId__"] = getCookies["__getEmpId__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForList(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "Employee.aspx", ddlCompanyList, gvForApprovedList, btnSearch);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ViewState["__CompanyId__"].ToString());


            }
            catch { }
        }

        private void LoadAllEmployeeList(string cmd)
        {
            try
            {
                DataTable dt = new DataTable();
                if (ViewState["__UserType__"].ToString().Equals("User"))
                {
                    cmd = "";
                    ViewState["__UpdateAction__"] = "0";
                    ViewState["__DeletAction__"] = "0";
                }
                sqlDB.fillDataTable(cmd, dt);

                gvForApprovedList.DataSource = dt;
                gvForApprovedList.DataBind();
                pTotalEmployee.InnerText = "Total Running -> " + dt.Rows.Count.ToString();

            }
            catch
            { }
        }
        /*Nayem.......
       .............For Searching............*/
        private void SearchingInEmployeeList()
        {
            try
            {
                if (ddlCompanyList.SelectedItem.Text.Trim() == "")
                {
                    ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                }
                if (txtCardNo.Text.Trim() != "")
                {
                    if (txtCardNo.Text.Length < 4)
                    { lblMessage.InnerText = "warning-> Please Type Employee Card No Minimum 6 Character!"; return; }
                }
            
                if (txtFromDate.Text.Trim().Length != 0 || txtToDate.Text.Trim().Length != 0)
                {
                    string[] dates = txtFromDate.Text.Trim().Split('-');
                    ViewState["__FDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    dates = txtToDate.Text.Trim().Split('-');
                    ViewState["__TDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    ddlChoseYear.SelectedIndex = 0;
                }
                if (txtCardNo.Text.Trim() == "" && ddlCompanyList.Text.Trim() != "" && ddlShift.SelectedIndex < 1 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && ((txtToDate.Text.Trim() != "" && txtFromDate.Text.Trim() != "") || ddlChoseYear.SelectedItem.Text.Trim() != ""))
                {
                    lblMessage.InnerText = "warning-> Please, Select a Department.";
                    return;
                }
              
                
                DataTable dt = new DataTable();
                // 0.Search by Compnay
                if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0)
                    sqlDB.fillDataTable("Select CompanyId, EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "' order by DptCode, CustomOrdering", dt = new DataTable());
               
                //1. Search by Company, CardNo.
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //2. Search by Company,Department,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' order by DptCode, CustomOrdering", dt = new DataTable());
                // 3. Search by Company,Department,Shift  
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length == 0 && ddlChoseYear.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' order by DptCode, CustomOrdering", dt = new DataTable());
                // 4. Search by Company,Department,Shift,CardNo 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //5. Search by Company,Department,Shift,CardNo,From Date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "'and EmpJoiningDate>='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //6. Search by Company,Department,Shift,CardNo,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' and FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //7. Search by Company,Department,Shift,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //8. Search by Company,Department,Shift,From date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpJoiningDate>='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //9. Search by Company, Department
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //10. Search by Company, CardNo,From date,To date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0 && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and EmpJoiningDate >='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //11. Search by Company,Department,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && ddlChoseYear.SelectedItem.Text.Trim() != "" && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and  FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //12.  Search by Company, Shift
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlDepartmentList.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //13.  Search by Company, Shift,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlDepartmentList.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //14. Search by Company, Department, FromDate,ToDate
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and EmpJoiningDate >='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //15. Search by Company, Department, FromDate,ToDate,Card no.
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and EmpJoiningDate >='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "'and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //16. Seardh by Company,FromDate,ToDate
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() == "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && (ddlGrouping.SelectedIndex == -1 || ddlGrouping.SelectedItem.Text.Trim() == ""))
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ddlCompanyList.SelectedValue + "' and EmpJoiningDate >='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by DptCode, CustomOrdering", dt = new DataTable());

                    //------------------------------------------            
                // 4. Search by Company,Department,Shift,Grouping 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length == 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " order by DptCode,CustomOrdering", dt = new DataTable());

                          // 4. Search by Company,Department,Shift,Grouping ,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' order by DptCode,CustomOrdering", dt = new DataTable());


                 //5. Search by Company,Department,Shift,CardNo,From Date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "'and EmpJoiningDate>='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by DptCode,CustomOrdering", dt = new DataTable());
                //6. Search by Company,Department,Shift,CardNo,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' and FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by DptCode,CustomOrdering", dt = new DataTable());
                //7. Search by Company,Department,Shift,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //8. Search by Company,Department,Shift,From date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpJoiningDate>='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by DptCode,CustomOrdering", dt = new DataTable());


                   // 4. Search by Company,Department,Grouping 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length == 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " order by DptCode,CustomOrdering", dt = new DataTable());

                          // 4. Search by Company,Department,Grouping ,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' order by DptCode,CustomOrdering", dt = new DataTable());


                 //5. Search by Company,Department,CardNo,From Date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "'and EmpJoiningDate>='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by DptCode,CustomOrdering", dt = new DataTable());
                //6. Search by Company,Department,CardNo,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' and FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by DptCode,CustomOrdering", dt = new DataTable());
                //7. Search by Company,Department,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and FORMAT(EmpJoiningDate,'yyyy')='" + ddlChoseYear.SelectedValue + "' order by DptCode, CustomOrdering", dt = new DataTable());
                //8. Search by Company,Department,From date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("Select CompanyId,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and EmpJoiningDate>='" + ViewState["__FDate__"].ToString() + "' and EmpJoiningDate<='" + ViewState["__TDate__"].ToString() + "' order by DptCode,CustomOrdering", dt = new DataTable());


                //-----------------------------------------
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No data available!";
                    gvForApprovedList.DataSource = null;
                    gvForApprovedList.DataBind();
                    return;
                }
                gvForApprovedList.DataSource = dt;
                gvForApprovedList.DataBind();

            }
            catch { }
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
        protected void gvForApprovedList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                
                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                string getId = gvForApprovedList.DataKeys[rIndex].Values[0].ToString();
                string CompanyId = gvForApprovedList.DataKeys[rIndex].Values[1].ToString();
              
                //if (e.CommandName == "Edit")
                //{
                //    Response.Redirect("/personnel/employee.aspx?CompanyId=" + CompanyId + "&EmpId=" + getId + "&Edit=True");
                //    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goURL('/personnel/employee.aspx?EmpId=" + getId + "&Edit=True')", true);
                //}
                if (e.CommandName == "Edit")
                {
                    Response.Redirect("/personnel/employee.aspx?EmpId=" + getId + "&CompanyId=" + CompanyId + "&Edit=True &Transfer=False");                   
                }
                if (e.CommandName == "Transfer")
                {
                    Response.Redirect("/personnel/employee.aspx?EmpId=" + getId + "&CompanyId=" + CompanyId + "&Edit=False &Transfer=True");                    
                }
                else if (e.CommandName == "Remove")
                {
                    DataTable dt;
                    //string EmpCardno = gvEmployeeList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text.ToString();
                    //sqlDB.fillDataTable("Select EmpCardNo From Personnel_EmpCurrentStatus where EmpId='" + getId + "'", dt = new DataTable());
                    ////string EmpCardno = dt.Rows[0].ItemArray[0].ToString();
                    DeleteEmployee(getId);
                    gvForApprovedList.Rows[rIndex].Visible = false;
                }
            }
            catch { }
        }
        private void DeleteEmployee(string EmpId)
        {
            try
            {
                SqlCommand deletecmd = new SqlCommand("Delete From Personnel_EmployeeInfo where EmpId='" + EmpId + "'", sqlDB.connection);
                int result = (int)deletecmd.ExecuteNonQuery();
               if (result > 0)
                {
                    lblMessage.InnerText = "success-> Successfully Deleted.";       
                }
            }
            catch { }
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCompanyList.SelectedValue == "0000")
            {
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
           // classes.commonTask.LoadShift(ddlShift, ddlCompanyList.SelectedValue.ToString());
            classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue.ToString());
          //  SearchingInEmployeeList();
        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["__LineORGroupDependency__"].ToString().Equals("True"))
            {
               string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue;
                classes.commonTask.LoadGrouping(ddlGrouping, CompanyId, ddlDepartmentList.SelectedValue);
            }
            classes.commonTask.LoadInitialShiftByDepartment(ddlShift, ddlCompanyList.SelectedValue, ddlDepartmentList.SelectedValue);
            SearchingInEmployeeList();
        }

        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingInEmployeeList();
        }

        protected void ddlChoseYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCardNo.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            SearchingInEmployeeList();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchingInEmployeeList();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            allClear();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllEmployeeList("Select CompanyId,EmpId,EmpCardNo,EmpName,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DptName,DsgName,SftName, convert(varchar(11),EmpShiftStartDate,105) as EmpShiftStartDate,EmpStatusName,EmpType From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and CompanyId='" + ViewState["__CompanyId__"].ToString() + "' order by EmpCardNo"); 
            loadYear();
            allClear();
        }

        protected void gvForApprovedList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SearchingInEmployeeList();
            }
            catch { }
            gvForApprovedList.PageIndex = e.NewPageIndex;
            Session["pageNumber"] = e.NewPageIndex;
            gvForApprovedList.DataBind();
        }

        protected void gvForApprovedList_RowDataBound(object sender, GridViewRowEventArgs e)
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
                Button btn ;
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnView");
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
                        btn = (Button)e.Row.FindControl("btnEdit");
                        btn.Enabled = false;
                        btn.ForeColor = Color.Silver;
                    }

                }
                catch { }
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnTransfer");
                        btn.Enabled = false;
                        btn.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }

        protected void ddlGrouping_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingInEmployeeList();
        }

        [WebMethod]       
        public static  object LoadEmpInfo()
        {
            //employee_list1 emplst = new employee_list1();
            //emplst.SearchingInEmployeeList();
            return 1;
        }
    }
}