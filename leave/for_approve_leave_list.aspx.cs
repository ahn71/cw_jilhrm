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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class for_approve_list : System.Web.UI.Page
    {
        string SqlCmd = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
               
                searchLeaveApplicationForApproved();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
        }
        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {


                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select LvOnlyDpt,LvAuthorityAction,DptId From UserAccount inner join Personnel_EmpCurrentStatus on UserAccount.EmpId=Personnel_EmpCurrentStatus.EmpId where UserAccount.UserId='"+getUserId+"' and UserAccount.isLvAuthority='1' and Personnel_EmpCurrentStatus.IsActive='1' ", dt);
                ViewState["__LvOnlyDpt__"] = dt.Rows[0]["LvOnlyDpt"].ToString();
                ViewState["__LvAuthorityAction__"] = dt.Rows[0]["LvAuthorityAction"].ToString();
                ViewState["__DptId__"] = dt.Rows[0]["DptId"].ToString();

                // below part for supper admin and master admin.there must be controll everythings.remember that super admin not seen another super admin information
                //if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                //{

                    classes.commonTask.LoadBranch(ddlCompanyList);
                    classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ViewState["__CompanyId__"].ToString());
                    return;
                //}
                //else    // below part for admin and viewer.while admin just write info and viewer just see information.its for by default settings
                //{

                //    classes.commonTask.LoadBranch(ddlCompanyList);
                //    classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());
                //    classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ViewState["__CompanyId__"].ToString());

                //    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                //    {
                //        gvForApprovedList.Visible = false; ;
                //        divElementContainer.EnableViewState = false;
                //    }

                //    //  here set privilege by setting master admin or supper admin 
                //    dtSetPrivilege = new DataTable();
                //    sqlDB.fillDataTable("select * from UserPrivilege where PageName='for_approve_leave_list.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);

                //    if (dtSetPrivilege.Rows.Count > 0)
                //    {
                //        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                //        {
                //            gvForApprovedList.Visible = true;
                //            divElementContainer.EnableViewState = true;
                //        }


                //    }


                //}
 
            }
            catch { }
        }

        DataTable dtForApprovedList;
        private void searchLeaveApplicationForApproved()
        {
            try
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000") || ddlCompanyList.SelectedValue.ToString().Equals("")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                dtForApprovedList = new DataTable();
                //if (ddlFindingType.SelectedValue.ToString().Equals("Today"))
                //{

                //    if (ddlShiftName.SelectedIndex == 0 && ddlDepartmentList.SelectedIndex == 0)
                //        sqlDB.fillDataTable("select la.LACode,la.EmpName,la.LeaveName,Format(la.FromDate,'dd-MM-yyyy') as FromDate,Format(la.ToDate,'dd-MM-yyyy') as ToDate,la.TotalDays,Format(la.EntryDate,'dd-MM-yyyy') as EntryDate  from v_Leave_LeaveApplication as la inner join UserAccount on la.LeaveProcessingOrder=UserAccount.LvAuthorityOrder where la.IsApproved='false' AND la.IsProcessessed='false' AND EntryDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND la.CompanyId='" + CompanyId + "' and UserAccount.UserId='" + ViewState["__UserId__"].ToString()+ "' and UserAccount.isLvAuthority='1'", dtForApprovedList = new DataTable());

                //    else if (ddlShiftName.SelectedIndex != 0 && ddlDepartmentList.SelectedIndex == 0)
                //        sqlDB.fillDataTable("select la.LACode,la.EmpName,la.LeaveName,Format(la.FromDate,'dd-MM-yyyy') as FromDate,Format(la.ToDate,'dd-MM-yyyy') as ToDate,la.TotalDays,Format(la.EntryDate,'dd-MM-yyyy') as EntryDate  from v_Leave_LeaveApplication as la inner join UserAccount on la.LeaveProcessingOrder=UserAccount.LvAuthorityOrder where la.IsApproved='false' AND la.IsProcessessed='false' AND EntryDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and UserAccount.UserId='" + ViewState["__UserId__"].ToString() + "' and UserAccount.isLvAuthority='1' " +
                //            " AND la.CompanyId='" + CompanyId + "' AND SftId=" + ddlShiftName.SelectedValue.ToString() + " ", dtForApprovedList = new DataTable());
                //    else if (ddlShiftName.SelectedIndex != 0 && ddlDepartmentList.SelectedIndex != 0)
                //        sqlDB.fillDataTable("select la.LACode,la.EmpName,la.LeaveName,Format(la.FromDate,'dd-MM-yyyy') as FromDate,Format(la.ToDate,'dd-MM-yyyy') as ToDate,la.TotalDays,Format(la.EntryDate,'dd-MM-yyyy') as EntryDate  from v_Leave_LeaveApplication as la inner join UserAccount on la.LeaveProcessingOrder=UserAccount.LvAuthorityOrder where la.IsApproved='false' AND la.IsProcessessed='false' AND EntryDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and UserAccount.UserId='" + ViewState["__UserId__"].ToString() + "' and UserAccount.isLvAuthority='1' " +
                //            " AND la.CompanyId='" + CompanyId + "' AND SftId=" + ddlShiftName.SelectedValue.ToString() + " " +
                //            "AND DptId='" + ddlDepartmentList.SelectedValue.ToString() + "' ", dtForApprovedList = new DataTable());
                //}
                //else
                //{
                //    //if (ddlShiftName.SelectedIndex == 0 && ddlDepartmentList.SelectedIndex == 0)
                //    if (ViewState["__LvOnlyDpt__"].ToString() != "True")
                //        sqlDB.fillDataTable("select Convert(varchar(3),left(la.EmpCardNo,LEN(la.EmpCardNo)-4))+' '+Convert(varchar(10),right(la.EmpCardNo,LEN(la.EmpCardNo)-9)) as EmpCardNo ,la.LACode,la.EmpName,la.LeaveName,Format(la.FromDate,'dd-MM-yyyy') as FromDate,Format(la.ToDate,'dd-MM-yyyy') as ToDate,la.TotalDays,Format(la.EntryDate,'dd-MM-yyyy') as EntryDate  from v_Leave_LeaveApplication as la inner join UserAccount on la.LeaveProcessingOrder=UserAccount.LvAuthorityOrder where la.IsApproved='false' AND la.IsProcessessed='false'   and UserAccount.UserId='" + ViewState["__UserId__"].ToString() + "' and UserAccount.isLvAuthority='1'", dtForApprovedList = new DataTable());

                //    //else if (ddlShiftName.SelectedIndex != 0 && ddlDepartmentList.SelectedIndex == 0)
                //    //    sqlDB.fillDataTable("select la.LACode,la.EmpName,la.LeaveName,Format(la.FromDate,'dd-MM-yyyy') as FromDate,Format(la.ToDate,'dd-MM-yyyy') as ToDate,la.TotalDays,Format(la.EntryDate,'dd-MM-yyyy') as EntryDate  from v_Leave_LeaveApplication as la inner join UserAccount on la.LeaveProcessingOrder=UserAccount.LvAuthorityOrder where la.IsApproved='false' AND la.IsProcessessed='false' and UserAccount.UserId='" + ViewState["__UserId__"].ToString() + "' and UserAccount.isLvAuthority='1' " +
                //    //        " AND la.CompanyId='" + CompanyId + "' AND SftId=" + ddlShiftName.SelectedValue.ToString() + " ", dtForApprovedList = new DataTable());
                //    else
                //        sqlDB.fillDataTable("select Convert(varchar(3),left(la.EmpCardNo,LEN(la.EmpCardNo)-4))+' '+Convert(varchar(10),right(la.EmpCardNo,LEN(la.EmpCardNo)-9)) as EmpCardNo,la.LACode,la.EmpName,la.LeaveName,Format(la.FromDate,'dd-MM-yyyy') as FromDate,Format(la.ToDate,'dd-MM-yyyy') as ToDate,la.TotalDays,Format(la.EntryDate,'dd-MM-yyyy') as EntryDate  from v_Leave_LeaveApplication as la inner join UserAccount on la.LeaveProcessingOrder=UserAccount.LvAuthorityOrder where la.IsApproved='false' AND la.IsProcessessed='false' and UserAccount.UserId='" + ViewState["__UserId__"].ToString() + "' and UserAccount.isLvAuthority='1'" +
                //            " AND la.CompanyId='" + CompanyId + "' " +
                //            "AND DptId='" + ViewState["__DptId__"].ToString() + "' ", dtForApprovedList = new DataTable());
                //}
                SqlCmd = "select la.EmpID,Convert(varchar(3),left(la.EmpCardNo,LEN(la.EmpCardNo)-4))+' '+Convert(varchar(10),right(la.EmpCardNo,LEN(la.EmpCardNo)-9)) as EmpCardNo,la.LACode,la.EmpName,la.LeaveName,Format(la.FromDate,'dd-MM-yyyy') as FromDate,Format(la.ToDate,'dd-MM-yyyy') as ToDate,la.TotalDays,Format(la.EntryDate,'dd-MM-yyyy') as EntryDate  from v_Leave_LeaveApplication as la inner join tblLeaveAuthorityAccessControl ac on la.LeaveProcessingOrder=ac.AuthorityPosition and la.EmpId=ac.EmpID and la.IsApproved='false' AND la.IsProcessessed='false' and ac.AuthorityID='" + ViewState["__UserId__"].ToString() + "'  AND la.CompanyId='" + CompanyId + "' ";
                sqlDB.fillDataTable(SqlCmd, dtForApprovedList = new DataTable());
                if (dtForApprovedList.Rows ==null || dtForApprovedList.Rows.Count==0)
                {
                    gvForApprovedList.DataSource = null;
                    gvForApprovedList.DataBind();
                    divRecordMessage.InnerText = "No Data Available!";
                    divRecordMessage.Visible = true;
                    return;
                }
                gvForApprovedList.DataSource = dtForApprovedList;
                gvForApprovedList.DataBind();
                divRecordMessage.InnerText = "";
                divRecordMessage.Visible = false;
            }
            catch (Exception ex)
            {
               // lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        protected void gvForApprovedList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
               
                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                if (e.CommandName.Equals("Forword"))
                {
                    ViewState["__Predecate__"] = "Forword";
                    Forward(rIndex);
                    saveAprovalLog(rIndex,"0");
                }

                if (e.CommandName.Equals("Yes"))
                {
                    ViewState["__Predecate__"] = "Yes";
                    YesApproved(rIndex);
                    saveAprovalLog(rIndex, "1");

                    
                }
                else if (e.CommandName.Equals("No"))
                {
                    ViewState["__Predecate__"] = "No";
                    saveLeaveApplicationRequest_AsLog(rIndex);
                    NoApproved(rIndex);
                    saveAprovalLog(rIndex, "2");
                    // this function calling for save leave application log
                    
                   
                    
                }
                else if (e.CommandName.Equals("Alter"))
                {
                    
                    TextBox txtFromDate = (TextBox)gvForApprovedList.Rows[rIndex].Cells[2].FindControl("txtFromDate");
                    TextBox txtToDate = (TextBox)gvForApprovedList.Rows[rIndex].Cells[3].FindControl("txtToDate");
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = true;
                    txtFromDate.Style.Add("border-style","solid");
                    txtFromDate.Style.Add("border-color", "#0000ff");

                    txtToDate.Style.Add("border-style", "solid");
                    txtToDate.Style.Add("border-color", "#0000ff");
                }

                else if (e.CommandName.Equals("Calculation"))
                {
                    Button btnForword = (Button)gvForApprovedList.Rows[rIndex].FindControl("btnForword");

                    btnForword.Enabled = false;
                    btnForword.ForeColor = Color.Silver;
                    TextBox txtFromDate = (TextBox)gvForApprovedList.Rows[rIndex].Cells[2].FindControl("txtFromDate");
                    TextBox txtToDate = (TextBox)gvForApprovedList.Rows[rIndex].Cells[3].FindControl("txtToDate");

                    TextBox txtTotalDays = (TextBox)gvForApprovedList.Rows[rIndex].Cells[3].FindControl("txtTotalDays");
                    
                    DataTable dt = new DataTable();

                    string [] getFDays = txtFromDate.Text.Trim().Split('-');
                    string[] getTDays = txtToDate.Text.Trim().Split('-');
                    getFDays[0] = getFDays[2] + "-" + getFDays[1] + "-" + getFDays[0];
                    getTDays[0] = getTDays[2] + "-" + getTDays[1] + "-" + getTDays[0];
                     
                    string CompanyId=(ddlCompanyList.SelectedIndex==0)?ViewState["__CompanyId__"].ToString():ddlCompanyList.SelectedValue.ToString();
                   
                    sqlDB.fillDataTable("select distinct Convert(varchar(11),Offdate,105) as OffDate,Reason from v_AllOffDays where OffDate >='" + getFDays[0] + "' AND OffDate<='" + getTDays[0] + "'  AND CompanyId='" + CompanyId + "' ", dt = new DataTable());
                    ViewState["__WHnumber__"] = dt.Rows.Count;

                    sqlDB.fillDataTable("select DateDiff(Day,'" + getFDays[0] + "','" + getTDays[0] + "') as TotalDays", dt=new DataTable ());

                    txtTotalDays.Text = (int.Parse(dt.Rows[0]["TotalDays"].ToString())+1).ToString();

                }
                else if(e.CommandName.Equals("View"))
                {
                    string LaCode = gvForApprovedList.DataKeys[rIndex].Values[0].ToString();
                    viewLeaveApplication(LaCode);
                }
            }
            catch { }
        }

        

        private void YesApproved(int rIndex)
        {
            try
            {
  	         string [] getColumns={"IsApproved","ApprovedDate","IsProcessessed"};
	         string [] getValues={"1", DateTime.Now.ToString("yyyy-MM-dd"), "1"};
	         if (SQLOperation.forUpdateValue("Leave_LeaveApplication",getColumns,getValues,"LACode",gvForApprovedList.DataKeys[rIndex].Values[0].ToString(),sqlDB.connection)==true)
	         {
                 saveLeaveApplicationRequest_AsLog(rIndex);   // this function calling for save leave application log
                 lblMessage.InnerText = "success->" + gvForApprovedList.Rows[rIndex].Cells[2].Text.ToString() + " approved for " + gvForApprovedList.Rows[rIndex].Cells[1].Text;
                 gvForApprovedList.Rows[rIndex].Visible = false;
	         }

            }
            catch{}
       
        }
        private void Forward(int rIndex)
        {
            try
            {
                DataTable dt=new DataTable();
                //sqlDB.fillDataTable("Select LvAuthorityOrder FROM UserAccount where UserId ='" + ViewState["__UserId__"].ToString()+ "'", dt);
                //string lvaorder = getLeaveAuthority(int.Parse(dt.Rows[0]["LvAuthorityOrder"].ToString())); ;
                string AuthorityPosition = getLeaveAuthority(ViewState["__UserId__"].ToString(), gvForApprovedList.DataKeys[rIndex].Values[1].ToString()); 
                if (AuthorityPosition.Trim() == "") // OD Authority is not found. plz setup for this employee.
                {

                    lblMessage.InnerText = "warning->The higher authority is not found.";
                    return;
                    // 
                }

                string[] getColumns = { "LeaveProcessingOrder" };
                string[] getValues = { AuthorityPosition };
                if (SQLOperation.forUpdateValue("Leave_LeaveApplication", getColumns, getValues, "LACode", gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection) == true)
                {
                   // saveLeaveApplicationRequest_AsLog(rIndex);   // this function calling for save leave application log
                    lblMessage.InnerText = "success->" + gvForApprovedList.Rows[rIndex].Cells[2].Text.ToString() + " forward for " + gvForApprovedList.Rows[rIndex].Cells[1].Text;
                    gvForApprovedList.Rows[rIndex].Visible = false;
                }

            }
            catch { }

        }
        private string getLeaveAuthority(string AuthorityID,string EmpID)
        {
            //try
            //{
            //    DataTable dtLvOrder;
            //    sqlDB.fillDataTable("select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and DptId in(" + ViewState["__DptId__"] + ") and LvAuthorityOrder<"+lvorder+"", dtLvOrder = new DataTable());
            //    if (!dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString().Equals("0"))
            //        return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
            //    else
            //    {
            //        sqlDB.fillDataTable("select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and LvOnlyDpt=0 and LvAuthorityOrder<" + lvorder + "", dtLvOrder = new DataTable());
            //        return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
            //    }
            //}
            //catch { return "0"; }

            try
            {
                DataTable dtLvOrder;
                SqlCmd = "select max(AuthorityPosition) as AuthorityPosition from tblLeaveAuthorityAccessControl where EmpID=" + EmpID + " and AuthorityPosition<(select AuthorityPosition from tblLeaveAuthorityAccessControl where EmpID=" + EmpID + " and AuthorityID= " + AuthorityID + ")";
                sqlDB.fillDataTable(SqlCmd, dtLvOrder = new DataTable());
                if (dtLvOrder == null || dtLvOrder.Rows.Count == 0)
                    return "";
                return dtLvOrder.Rows[0]["AuthorityPosition"].ToString();
            }
            catch (Exception ex) { return ""; }
        }
    

        private void NoApproved(int rIndex)
        {
            try
            {
              

                if (SQLOperation.forDeleteRecordByIdentifier("Leave_LeaveApplication", "LACode", gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection))
                {
                    SQLOperation.forDeleteRecordByIdentifier("Leave_LeaveApplicationDetails", "LACode", gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection); // for clear all leave details

                    SqlCommand cmd = new SqlCommand("update Leave_LeaveApplication_Log set ApprovedDate ='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where LACode=" + gvForApprovedList.DataKeys[rIndex].Values[0].ToString() + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    lblMessage.InnerText = "success->" + gvForApprovedList.Rows[rIndex].Cells[2].Text.ToString() + " not rejected for " + gvForApprovedList.Rows[rIndex].Cells[1].Text.ToString();
                    gvForApprovedList.Rows[rIndex].Visible = false;
                }

            }
            catch { }
        }

        private void saveLeaveApplicationRequest_AsLog(int rIndex)    // this function for save leave application log
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select LACode,LeaveId,Format(FromDate,'dd-MM-yyyy') as FromDate,Format(ToDate,'dd-MM-yyyy') as ToDate,WeekHolydayNo,TotalDays,"+
                    "Remarks,StateStatus,EmpId,IsApproved,Format(ApprovedDate,'dd-MM-yyyy')as ApprovedDate,IsProcessessed,EmpTypeId,Format(PregnantDate,'dd-MM-yyyy') as PregnantDate,"+
                    "Format(PrasaberaDate,'dd-MM-yyyy') as PrasaberaDate,Format(EntryDate,'dd-MM-yyyy') as EntryDate,CompanyId,SftId,DptId,DsgId,EmpStatus,Format(ApplyDate,'dd-MM-yyyy') as ApplyDate,LeaveProcessingOrder " +
                    " from Leave_LeaveApplication where LACode=" + gvForApprovedList.DataKeys[rIndex].Values[0].ToString() + "", dt);

                ViewState["__OldFromDate__"]=dt.Rows[0]["FromDate"].ToString();
                ViewState["__OldToDate__"]=dt.Rows[0]["ToDate"].ToString();

                byte isApproved = (bool.Parse(dt.Rows[0]["IsApproved"].ToString()).Equals(true)) ? (byte)1 : (byte)0;
                byte isProcessed = (bool.Parse(dt.Rows[0]["IsProcessessed"].ToString()).Equals(true)) ? (byte)1 : (byte)0;
                if (isApproved == 1) // if and date are changed then change leave application date and total days changed
                {
                    if (dt.Rows[0]["PregnantDate"].ToString().Length == 10)
                        ChangeEmpTypeForML(dt.Rows[0]["EmpId"].ToString());
                    TextBox txtFromDate = (TextBox)gvForApprovedList.Rows[rIndex].Cells[2].FindControl("txtFromDate");
                    TextBox txtToDate = (TextBox)gvForApprovedList.Rows[rIndex].Cells[3].FindControl("txtToDate");

                    if (!ViewState["__OldFromDate__"].ToString().Equals(txtFromDate.Text.Trim()) || !ViewState["__OldToDate__"].ToString().Equals(txtToDate.Text.Trim()))
                    {
                        SqlCommand cmd = new SqlCommand("insert into Leave_LeaveApplication_Log  select * from  Leave_LeaveApplication where LACode='" + gvForApprovedList.DataKeys[rIndex].Values[0].ToString() + "'", sqlDB.connection);
                        cmd.ExecuteNonQuery().ToString();     
                  
                        TextBox txtTotalDays = (TextBox)gvForApprovedList.Rows[rIndex].Cells[3].FindControl("txtTotalDays");
                        string[] getColumns = { "FromDate", "ToDate", "TotalDays", "WeekHolydayNo" };
                        string[] getValues = { classes.commonTask.ddMMyyyyToyyyyMMdd(txtFromDate.Text.Trim()).ToString(), classes.commonTask.ddMMyyyyToyyyyMMdd(txtToDate.Text.Trim()).ToString(), txtTotalDays.Text.Trim(), ViewState["__WHnumber__"].ToString() };
                        SQLOperation.forUpdateValue("Leave_LeaveApplication", getColumns, getValues, "LACode", gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);

                        saveLeaveDetails(gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), txtFromDate.Text, txtToDate.Text, dt.Rows[0]["EmpId"].ToString());  // for save leave details
                    }
                     
                }
                else 
                {
                    SqlCommand cmd = new SqlCommand("insert into Leave_LeaveApplication_Log  select * from  Leave_LeaveApplication where LACode='" + gvForApprovedList.DataKeys[rIndex].Values[0].ToString() + "'", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    
                }

               
                
            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message);
            }
        }


        private void ChangeEmpTypeForML(string getEmpId)   // change EmpType when any female get any Maternity Leave
        {
            try
            {
                SqlCommand cmd = new SqlCommand("update Personnel_EmpCurrentStatus set EmpStatus='8' where SN=(select Max(SN) from  Personnel_EmpCurrentStatus Where EmpId='" + getEmpId+ "' AND IsActive=1)", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }


        private void saveLeaveDetails(string LACode, string FDates,string TDates,string EmpId)
        {
            try
            {
                SQLOperation.forDeleteRecordByIdentifier("Leave_LeaveApplicationDetails", "LACode", LACode, sqlDB.connection);

                string[] getFDate = FDates.ToString().Split('-');  // dd-MM-yyyy

                string [] getToDate=TDates.ToString().Split('-');

                DateTime dtFromDate = new DateTime(int.Parse(getFDate[2]), int.Parse(getFDate[1]), int.Parse(getFDate[0]));

                DateTime dtToDate = new DateTime(int.Parse(getToDate[2]), int.Parse(getToDate[1]), int.Parse(getToDate[0]));

                while(dtFromDate<=dtToDate)
                {
                    getFDate = dtFromDate.ToString().Split('/'); // now Format m-d-yyyy

                    

                    string Date = getFDate[1] + "-" + getFDate[0] + "-" + getFDate[2]; // convert format in dd-MM-yyyy
                    string[] getColumns = { "LACode", "EmpId", "LeaveDate", "Used" };
                    string[] getValues = { LACode,EmpId, classes.commonTask.ddMMyyyyToyyyyMMdd(Date).ToString(), "0" };
                    SQLOperation.forSaveValue("Leave_LeaveApplicationDetails", getColumns, getValues, sqlDB.connection);
                    dtFromDate=dtFromDate.AddDays(1);

 
                }
               

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
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
            Button btn;
            try
            {
                string EmpID = gvForApprovedList.DataKeys[e.Row.RowIndex].Values[1].ToString();
                string AuthorityAction = commonTask.getLvAuthorityAction(ViewState["__UserId__"].ToString(), EmpID);
                if (AuthorityAction == "")
                {
                    btn = new Button();
                    btn = (Button)e.Row.FindControl("btnForword");
                    btn.Enabled = false;
                    btn.ForeColor = Color.Silver;

                    btn = new Button();
                    btn = (Button)e.Row.FindControl("btnYes");
                    btn.Enabled = false;
                    btn.ForeColor = Color.Silver;

                    btn = new Button();
                    btn = (Button)e.Row.FindControl("btnEdit");
                    btn.Enabled = false;
                    btn.ForeColor = Color.Silver;

                    btn = new Button();
                    btn = (Button)e.Row.FindControl("btnNot");
                    btn.Enabled = false;
                    btn.ForeColor = Color.Silver;

                }
                else
                {
                    ViewState["__LvAuthorityAction__"] = AuthorityAction;
                    if (ViewState["__LvAuthorityAction__"].ToString() != "1" && ViewState["__LvAuthorityAction__"].ToString() != "0")
                    {
                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnForword");
                        btn.Enabled = false;
                        btn.ForeColor = Color.Silver;
                    }
                    if (ViewState["__LvAuthorityAction__"].ToString() != "2" && ViewState["__LvAuthorityAction__"].ToString() != "0")
                    {
                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnYes");
                        btn.Enabled = false;
                        btn.ForeColor = Color.Silver;

                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnEdit");
                        btn.Enabled = false;
                        btn.ForeColor = Color.Silver;

                    }
                }
                

               
            }
            catch { }
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000") || ddlCompanyList.SelectedValue.ToString().Equals("")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                classes.commonTask.LoadShift(ddlShiftName, CompanyId);
                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, CompanyId);

                searchLeaveApplicationForApproved();   // searching operation
            }
            catch { }
        }

        protected void ddlShiftName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000") || ddlCompanyList.SelectedValue.ToString().Equals("")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                classes.commonTask.loadDepartmentListByCompanyAndShift(ddlDepartmentList, CompanyId, ddlShiftName.SelectedValue.ToString());

                searchLeaveApplicationForApproved();   // searching operation
            }
            catch { }
        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                searchLeaveApplicationForApproved();   // searching operation
            }
            catch { }
        }

        protected void ddlFindingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                searchLeaveApplicationForApproved();   // searching operation
            }
            catch { }
        }

        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            try
            {
               
                searchLeaveApplicationForApproved();   // searching operation
            }
            catch { }
        }

        protected void btnCLeave_Click(object sender, EventArgs e)
        { 
        
        }


        //Md.Nayem 
        //Email=xxx@gmail.com
        //purpose : To set value for leave applicaiton report

        private void viewLeaveApplication(string LaCode)
        {
            try
            {
                string getSQLCMD;
                DataTable dt = new DataTable();
                DataTable dtApprovedRejectedDate = new DataTable();
                //getSQLCMD = " with nl as("+
                //    "SELECT LACode,EmpId, format(FromDate,'dd-MM-yyyy') as FromDate,FromDate AS FromDate1, format(ToDate,'dd-MM-yyyy') as ToDate,"+
                //    " TotalDays,  Remarks, LeaveName, DsgName, DptName, CompanyName,format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,"+
                //    " Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,"+
                //    "  EmpName, Address, LvAddress, LvContact, CompanyId, DptId, format(ApplyDate,'dd-MM-yyyy') as ApplyDate,Convert(varchar(3),"+
                //    "left(HEmpCardNo,LEN(HEmpCardNo)-4))+' '+Convert(varchar(10),right(HEmpCardNo,LEN(HEmpCardNo)-9)) HEmpCardNo,HEmpName,HDptName,"+
                //    "HDsgName   FROM dbo.v_Leave_LeaveApplication where LACode=" + LaCode + "),"+
                //    "ll as("+
                //    "select convert(varchar(10), FromDate,105)+' to '+ convert(varchar(10), max(ToDate),105) as LastLeaveDate,LeaveName as LastLeaveNature,TotalDays as LastLeaveDays,EmpID as llEmpID from v_Leave_LeaveApplication " +
                //    " where EmpId=(select EmpId from nl) and  ToDate<(select FromDate1 from nl) group by FromDate,LeaveName,TotalDays,EmpID) " +
                //    "select * from nl left join ll on nl.EmpId=ll.llEmpID";
                getSQLCMD = " SELECT LACode,EmpId, format(FromDate,'dd-MM-yyyy') as FromDate, format(ToDate,'dd-MM-yyyy') as ToDate, TotalDays,"
               + "  Remarks, LeaveName, DsgName, DptName, CompanyName,format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo, "
               + " EmpName, Address, LvAddress, LvContact, CompanyId, DptId, format(ApplyDate,'dd-MM-yyyy') as ApplyDate,HEmpCardNo,HEmpName,HDptName,HDsgName,LastLeaveDate,LastLeaveNature,LastLeaveDays,IsHalfDayLeave "
               + " FROM"
               + " dbo.v_Leave_LeaveApplication"
               + " where LACode=" + LaCode + "";
                sqlDB.fillDataTable(getSQLCMD, dt);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry any payslip are not founded"; return;
                }
                string[] FDate = dt.Rows[0]["FromDate"].ToString().Split('-');
                Session["__Language__"] = "English";
                Session["__LeaveApplication__"] = dt;
                //getSQLCMD = "with lvd as ( select CompanyId, Leaveid, ShortName,count(ShortName) as Amount,case when Sex='Male' then 'm/l'else '' end Sex " +
                //     " from v_Leave_LeaveApplicationDetails where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' and IsApproved=1 and LeaveDate>='" + FDate[2] + "-01-01' and LeaveDate<'" + FDate[2] + "-" + FDate[1] + "-" + FDate[0] + "'" +
                //     " group by CompanyId,Leaveid, ShortName,Sex),pcs as (select case when Sex='Male' then 'm/l'else '' end Sex,CompanyId from v_EmployeeDetails where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' and IsActive=1 ) ,lc as ( select * from tblLeaveConfig where CompanyId=(select CompanyId from pcs)) ," +
                //     " la as(select  LeaveId,TotalDays from Leave_LeaveApplication where LACode=" + LaCode + ")" +
                //     " select lc.ShortName,ISNULL(lvd.Amount,0) as Amount,lc.LeaveDays,lc.CompanyId,lc.LeaveName,( lc.LeaveDays-ISNULL(lvd.Amount,0) )as Remaining,TotalDays Applied from  lc left join lvd on lc.LeaveId=lvd.LeaveId and lc.CompanyId=lvd.CompanyId  left join la on lc.LeaveId=la.LeaveId " +
                //     " where  lc.ShortName not in('sr/l',(select   Sex from pcs))";
                getSQLCMD = "with dCL as(select 'c/l' as ShortName,ISNULL( sum(Days),0) as Deducted from Leave_MonthlyLeaveDeductionRecord where EmpID='" + dt.Rows[0]["EmpId"].ToString() + "' and Month>='" + FDate[2] + "-01-01' and Month<'" + FDate[2] + "-" + FDate[1] + "-" + FDate[0] + "')," +
                   " lvd as ( select CompanyId, Leaveid, ShortName,sum( case when IsHalfDayLeave=1 then 0.5 else 1 end) as Amount,case when Sex='Male' then 'm/l'else '' end Sex " +
                    " from v_Leave_LeaveApplicationDetails where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' and IsApproved=1 and LeaveDate>='" + FDate[2] + "-01-01' and LeaveDate<'" + FDate[2] + "-" + FDate[1] + "-" + FDate[0] + "'" +
                    " group by CompanyId,Leaveid, ShortName,Sex),pcs as (select case when Sex='Male' then 'm/l'else '' end Sex,CompanyId from v_EmployeeDetails where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' and IsActive=1 ) ,lc as ( select * from tblLeaveConfig where CompanyId=(select CompanyId from pcs)) ," +
                    " la as(select  LeaveId,TotalDays from Leave_LeaveApplication where LACode=" + LaCode + ")" +
                    " select lc.ShortName,ISNULL(lvd.Amount,0) as Amount,lc.LeaveDays,lc.CompanyId,lc.LeaveName,( lc.LeaveDays-(ISNULL(lvd.Amount,0)+ISNULL(dCL.Deducted,0)) ) as Remaining,TotalDays Applied, dCL.Deducted from  lc left join lvd on lc.LeaveId=lvd.LeaveId and lc.CompanyId=lvd.CompanyId  left join la on lc.LeaveId=la.LeaveId left join dCL on lvd.ShortName = dCL.ShortName " +
                    " where  lc.ShortName not in('sr/l',(select   Sex from pcs))";
                sqlDB.fillDataTable(getSQLCMD, dt = new DataTable());
                Session["__LeaveCurrentStatus__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveApplication');", true);  //Open New Tab for Sever side code

            }
            catch { }
        }
        private void saveAprovalLog(int rIndex, string Approval) 
        {
            try 
            {
                string[] getColumns = { "LACode", "UserID", "DateTime", "Approval" };
                string[] getValues = { gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), ViewState["__UserId__"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Approval };
                SQLOperation.forSaveValue("Leave_ApprovalLog", getColumns, getValues, sqlDB.connection);
            }
            catch { }
            
              
        }
    }
}