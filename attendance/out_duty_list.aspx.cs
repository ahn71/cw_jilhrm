using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.attendance
{
    public partial class out_duty_list : System.Web.UI.Page
    {
        string sql = "";
        string CompanyId = "";
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                Session["__dtClient__"] = "";
                ViewState["__rIndex__"] = "";
                ViewState["__LineORGroupDependency__"] = classes.commonTask.GroupORLineDependency();
                setPrivilege();
                if (ViewState["__LineORGroupDependency__"].ToString().Equals("False"))
                    classes.commonTask.LoadGrouping(ddlGrouping, ViewState["__CompanyId__"].ToString());
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                loadYear();
                //loadOutDuty();
                txtFromDate.Text = "01-" + DateTime.Now.ToString("MM-yyyy");
                txtToDate.Text=DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) +"-" + DateTime.Now.ToString("MM-yyyy");
                SearchingOutDuty();
            }
        }
        private void loadYear()
        {
            try
            {
                sqlDB.fillDataTable("SELECT distinct Year(Date) as Year from tblOutDuty order by Year(Date) desc", dt = new DataTable());
                ddlChoseYear.DataTextField = "Year";
                ddlChoseYear.DataValueField = "Year";
                ddlChoseYear.DataSource = dt;
                ddlChoseYear.DataBind();
                // ddlChoseYear.SelectedIndex = 0;
                ddlChoseYear.Items.Insert(0, new ListItem(string.Empty, "0"));
               // ddlChoseYear.SelectedValue = DateTime.Now.Year.ToString();
            }
            catch { }
        }
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__getUserId__"] = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__ODOnlyDpt__"] = getCookies["__getODOnlyDpt__"].ToString();
                ViewState["__DptId__"] = getCookies["__getDptId__"].ToString();
                ViewState["__EmpId__"] = getCookies["__getEmpId__"].ToString();


                string[] AccessPermission = new string[0];
                Button btnSave = new Button();
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), ViewState["__getUserId__"].ToString(), ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "aplication.aspx", ddlCompanyList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

               

                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin" )
                {
                    
                    classes.commonTask.loadDepartmentByAdminForOutDuty(ddlDepartmentList, ViewState["__CompanyId__"].ToString(), ViewState["__getUserId__"].ToString());
                    if (ViewState["__LineORGroupDependency__"].ToString().Equals("True"))
                    {
                        classes.commonTask.LoadGrouping(ddlGrouping, ViewState["__CompanyId__"].ToString(), ddlDepartmentList.SelectedValue);
                    }                    
                    
                }
                else
                    classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ViewState["__CompanyId__"].ToString());


                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
                {
                    ViewState["__ReadAction__"] = "1";
                    ddlDepartmentList.Enabled = false;
                    txtCardNo.Enabled = false;
                }
                if (ViewState["__ReadAction__"].ToString().Equals("0"))
                {
                    gvOutDuty.Visible = false;
                }
                

            }
            catch { }
        }
        //private void loadOutDuty()
        //{
           
        //    CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
        //    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
        //        sql="select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where EmpId='" + ViewState["__EmpId__"].ToString() + "' and CompanyId='" + CompanyId + "' and Status="+rblApprovedPending.SelectedValue+" order by year(Date) desc,month(Date) desc,date desc";
        //    else if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
        //        sql="select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where CompanyId='" + CompanyId + "'  and (EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpId in(select EmpId from  tblOutDutyAuthorityAccessControl where AuthorityID=" + ViewState["__getUserId__"].ToString() + "))  and Status=" + rblApprovedPending.SelectedValue + " order by year(Date) desc,month(Date) desc,date desc";
        //    else
        //        sql="select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where CompanyId='" + CompanyId + "' and Status=" + rblApprovedPending.SelectedValue + " order by year(Date) desc,month(Date) desc,date desc";
        //    sqlDB.fillDataTable(sql, dt = new DataTable());
        //    gvOutDuty.DataSource = dt;
        //    gvOutDuty.DataBind();

        //}
        private void SearchingOutDuty()
        {

            string EmpId = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("User")) ? " and EmpId='" + ViewState["__EmpId__"].ToString() + "'" : "";

            string AdminCondition = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin")) ? "  and (EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpId in(select EmpId from  tblOutDutyAuthorityAccessControl where AuthorityID=" + ViewState["__getUserId__"].ToString() + ")) " : "";

            //if (ddlCompanyList.SelectedIndex == 0 && txtCardNo.Text.Trim().Length == 0)
            //{
            //    lblMessage.InnerText = "warning-> Please, Select Company Name.";
            //    return;
            //}

            if (txtCardNo.Text.Trim() != "")
            {
                if (txtCardNo.Text.Length < 4)
                { lblMessage.InnerText = "warning-> Please Type Employee Card No Minimum 4 Character!"; return; }
            }
            if (txtFromDate.Text.Trim().Length != 0 || txtToDate.Text.Trim().Length != 0)
            {
                string[] dates = txtFromDate.Text.Trim().Split('-');
                ViewState["__FDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                dates = txtToDate.Text.Trim().Split('-');
                ViewState["__TDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                ddlChoseYear.SelectedIndex = 0;
            }
         
           
            CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
            //0. Search by Company and From date, To Date
            if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0)  && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length == 0)
                sql = "select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where  Status=" + rblApprovedPending.SelectedValue + " and CompanyId='" + CompanyId + "' and Date >='" + ViewState["__FDate__"].ToString() + "' and Date<='" + ViewState["__TDate__"].ToString() + "' "+ EmpId+AdminCondition + "  order by year(Date) desc,month(Date) desc,date desc";
            //1. Search by Company and year
            if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlChoseYear.SelectedIndex > 0 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0)  && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0)
                sql = "select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where  Status=" + rblApprovedPending.SelectedValue + " and CompanyId='" + CompanyId + "' and year(Date)='"+ddlChoseYear.SelectedValue+ "' " + EmpId + AdminCondition + "  order by year(Date) desc,month(Date) desc,date desc";
            //2. Search by Company, CardNo.
            else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0)  && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                sql = "select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where  Status=" + rblApprovedPending.SelectedValue + " and CompanyId='" + CompanyId + "'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' " + EmpId + AdminCondition + "  order by year(Date) desc,month(Date) desc,date desc";
            //3. Search by Company,Department,Card No
            else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                sql = "select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where  Status=" + rblApprovedPending.SelectedValue + " and CompanyId='" + CompanyId + "' and DptId='"+ddlDepartmentList.SelectedValue+"'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' " + EmpId + AdminCondition + "  order by year(Date) desc,month(Date) desc,date desc";
            //4. Search by Company, Department
            else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != ""  && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                sql = "select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where  Status=" + rblApprovedPending.SelectedValue + " and CompanyId='" + CompanyId + "' and DptId='" + ddlDepartmentList.SelectedValue + "' " + EmpId + AdminCondition + "   order by year(Date) desc,month(Date) desc,date desc";
            //5. Search by Company, CardNo, From date,To date
            else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0)
                sql = "select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where  Status=" + rblApprovedPending.SelectedValue + " and CompanyId='" + CompanyId + "' and Date >='" + ViewState["__FDate__"].ToString() + "' and Date<='" + ViewState["__TDate__"].ToString() + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' " + EmpId + AdminCondition + "  order by year(Date) desc,month(Date) desc,date desc";
           //6. Search by Company,Department,Year
            else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != ""  && ddlChoseYear.SelectedItem.Text.Trim() != "")
                sql = "select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where  Status=" + rblApprovedPending.SelectedValue + " and CompanyId='" + CompanyId + "' and DptId='" + ddlDepartmentList.SelectedValue + "' and year(Date)='" + ddlChoseYear.SelectedValue + "' " + EmpId + AdminCondition + "  order by year(Date) desc,month(Date) desc,date desc";
            //7. Search by Company, Department, FromDate,ToDate
            else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                sql = "select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where  Status=" + rblApprovedPending.SelectedValue + " and CompanyId='" + CompanyId + "' and DptId='" + ddlDepartmentList.SelectedValue + "' and Date >='" + ViewState["__FDate__"].ToString() + "' and Date<='" + ViewState["__TDate__"].ToString() + "' " + EmpId + AdminCondition + "  order by year(Date) desc,month(Date) desc,date desc";
            //8. Search by Company, Department, FromDate,ToDate,Card no.
            else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != ""  && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                sql = "select SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,AuthorizedByName from v_tblOutDuty where  Status=" + rblApprovedPending.SelectedValue + " and CompanyId='" + CompanyId + "' and DptId='" + ddlDepartmentList.SelectedValue + "' and Date >='" + ViewState["__FDate__"].ToString() + "' and Date<='" + ViewState["__TDate__"].ToString() + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' " + EmpId + AdminCondition + "   order by year(Date) desc,month(Date) desc,date desc";
            sqlDB.fillDataTable(sql, dt = new DataTable());
            gvOutDuty.DataSource = dt;
            gvOutDuty.DataBind();

        }
        protected void gvOutDuty_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("deleterow"))
            {
                int rIndex = int.Parse(e.CommandArgument.ToString());
                if (SQLOperation.forDeleteRecordByIdentifier("tblOutDuty", "SL", gvOutDuty.DataKeys[rIndex].Value.ToString(), sqlDB.connection) == true)
                {
                    SQLOperation.forDeleteRecordByIdentifier("tblAttendanceRecord", "ODID", gvOutDuty.DataKeys[rIndex].Value.ToString(), sqlDB.connection);
                    gvOutDuty.Rows[rIndex].Visible = false;
                    lblMessage.InnerText = "success-> Successfully Deleted";

                }
            }
            if (e.CommandName.Equals("editRow"))
            {
                int rIndex = int.Parse(e.CommandArgument.ToString());
            }
        }

        protected void gvOutDuty_RowDataBound(object sender, GridViewRowEventArgs e)
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
            try
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblType = (Label)e.Row.FindControl("lblType");
                if (lblType.Text.Trim() == "Training" || lblStatus.Text != "Pending")
                {
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    btnEdit.Enabled = false;
                    btnEdit.CssClass = "";
                    btnEdit.ForeColor = Color.Gray;
                }

                if (lblStatus.Text == "Approved")
                    lblStatus.ForeColor = Color.Green;
                else if (lblStatus.Text == "Rejected")
                    lblStatus.ForeColor = Color.Red;
            }
            catch { }
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("User") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                try
                {
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    if (ViewState["__DeletAction__"].ToString().Equals("0") || lblStatus.Text != "Pending")
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnView");
                        lnkDelete.Enabled = false;
                        lnkDelete.OnClientClick = "return false";
                        lnkDelete.ForeColor = Color.Gray;
                    }

                }
                catch { }

            }
        }

        protected void rblApprovedPending_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingOutDuty();
            //loadOutDuty();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["__LineORGroupDependency__"].ToString().Equals("True"))
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue;
                classes.commonTask.LoadGrouping(ddlGrouping, CompanyId, ddlDepartmentList.SelectedValue);
            }
            SearchingOutDuty();
        }      


        protected void ddlChoseYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingOutDuty();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchingOutDuty();
        }
    }
}