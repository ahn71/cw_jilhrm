using adviitRuntimeScripting;
using ComplexScriptingSystem;
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
    public partial class for_approve_shortleave_list : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            
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
                sqlDB.fillDataTable("Select LvOnlyDpt,LvAuthorityAction,DptId From UserAccount inner join Personnel_EmpCurrentStatus on UserAccount.EmpId=Personnel_EmpCurrentStatus.EmpId where UserAccount.UserId='" + getUserId + "' and UserAccount.isLvAuthority='1' and Personnel_EmpCurrentStatus.IsActive='1' ", dt);
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

                //    classes.commonTask.LoadBranch(ddlCompanyList, ViewState["__CompanyId__"].ToString());
                    
                //    classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());

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

                if (ViewState["__LvOnlyDpt__"].ToString() != "1")
                    sqlDB.fillDataTable("Select SrLvID,v_Leave_ShortLeave.EmpId,EmpCardNo,v_Leave_ShortLeave.CompanyId,EmpName,FORMAT(LvDate,'dd-MM-yyyy') as LvDate,FromTime,ToTime,LvTime,Remarks,v_Leave_ShortLeave.DptId  from v_Leave_ShortLeave inner join UserAccount on v_Leave_ShortLeave.LeaveProcessingOrder=UserAccount.LvAuthorityOrder " +
                            " where v_Leave_ShortLeave.LvStatus='0'   and UserAccount.UserId='" + ViewState["__UserId__"].ToString() + "' and UserAccount.isLvAuthority='1'  Order by year(LvDate)desc, Month(LvDate)desc,LvDate desc", dtForApprovedList = new DataTable());

                    else
                    sqlDB.fillDataTable("Select SrLvID,v_Leave_ShortLeave.EmpId,EmpCardNo,v_Leave_ShortLeave.CompanyId,EmpName,FORMAT(LvDate,'dd-MM-yyyy') as LvDate,FromTime,ToTime,LvTime,Remarks,v_Leave_ShortLeave.DptId  from v_Leave_ShortLeave inner join UserAccount on v_Leave_ShortLeave.LeaveProcessingOrder=UserAccount.LvAuthorityOrder where v_Leave_ShortLeave.LvStatus='0' " +
                            " AND v_Leave_ShortLeave.CompanyId='" + CompanyId + "' AND DptId='" + ViewState["__DptId__"].ToString() + "' and UserAccount.UserId='" + ViewState["__UserId__"].ToString() + "' and UserAccount.isLvAuthority='1'  Order by year(LvDate)desc, Month(LvDate)desc,LvDate desc", dtForApprovedList = new DataTable());                  
                if (dtForApprovedList.Rows ==null || dtForApprovedList.Rows.Count==0)
                {
                    gvForApprovedList.DataSource = null;
                    gvForApprovedList.DataBind();
                    divRecordMessage.InnerText = "Any Leave Approved Request Are Not Founded !";
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
                lblMessage.InnerText = "";
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
                    NoApproved(rIndex);  // this function calling for save leave application log
                    saveAprovalLog(rIndex, "2");
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

        private void Forward(int rIndex)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select LvAuthorityOrder FROM UserAccount where UserId ='" + ViewState["__UserId__"].ToString() + "'", dt);
                string lvaorder = getLeaveAuthority(int.Parse(dt.Rows[0]["LvAuthorityOrder"].ToString())); ;

                string[] getColumns = { "LeaveProcessingOrder" };
                string[] getValues = { lvaorder };
                if (SQLOperation.forUpdateValue("Leave_ShortLeave", getColumns, getValues, "SrLvID", gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection) == true)
                {
                    //saveLeaveApplicationRequest_AsLog(rIndex);   // this function calling for save leave application log
                    lblMessage.InnerText = "success->" + gvForApprovedList.Rows[rIndex].Cells[2].Text.ToString() + " approved for " + gvForApprovedList.Rows[rIndex].Cells[1].Text;
                    gvForApprovedList.Rows[rIndex].Visible = false;
                }

            }
            catch { }

        }
        private string getLeaveAuthority(int lvorder)
        {
            try
            {
                DataTable dtLvOrder;
                sqlDB.fillDataTable("select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where DptId in(" + ViewState["__DptId__"] + ") and LvAuthorityOrder<" + lvorder + "", dtLvOrder = new DataTable());
                if (!dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString().Equals("0"))
                    return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
                else
                {
                    sqlDB.fillDataTable("select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where  LvOnlyDpt=0 and LvAuthorityOrder<" + lvorder + "", dtLvOrder = new DataTable());
                    return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
                }
            }
            catch { return "0"; }
        }

        private void YesApproved(int rIndex)
        {
            try
            {
                string[] getColumns = { "LvStatus" };
	         string [] getValues={"1"};
             if (SQLOperation.forUpdateValue("Leave_ShortLeave", getColumns, getValues, "SrLvID", gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection) == true)
	         {
                // saveLeaveApplicationRequest_AsLog(rIndex);   // this function calling for save leave application log
                 lblMessage.InnerText = "success->" + gvForApprovedList.Rows[rIndex].Cells[2].Text.ToString() + " approved for " + gvForApprovedList.Rows[rIndex].Cells[1].Text;
                 gvForApprovedList.Rows[rIndex].Visible = false;
	         }

            }
            catch{}
       
        }

        private void NoApproved(int rIndex)
        {
            try
            {


                string[] getColumns = { "LvStatus" };
                string[] getValues = { "2" };
                if (SQLOperation.forUpdateValue("Leave_ShortLeave", getColumns, getValues, "SrLvID", gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection) == true)
                {
                    // saveLeaveApplicationRequest_AsLog(rIndex);   // this function calling for save leave application log
                    lblMessage.InnerText = "success->" + gvForApprovedList.Rows[rIndex].Cells[2].Text.ToString() + " approved for " + gvForApprovedList.Rows[rIndex].Cells[1].Text;
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
                    "Format(PrasaberaDate,'dd-MM-yyyy') as PrasaberaDate,Format(EntryDate,'dd-MM-yyyy') as EntryDate,CompanyId,SftId,DptId,DsgId,EmpStatus " +
                    " from Leave_LeaveApplication where LACode=" + gvForApprovedList.DataKeys[rIndex].Values[0].ToString() + "", dt);

                ViewState["__OldFromDate__"]=dt.Rows[0]["FromDate"].ToString();
                ViewState["__OldToDate__"]=dt.Rows[0]["ToDate"].ToString();

                byte isApproved = (bool.Parse(dt.Rows[0]["IsApproved"].ToString()).Equals(true)) ? (byte)1 : (byte)0;
                byte isProcessed = (bool.Parse(dt.Rows[0]["IsProcessessed"].ToString()).Equals(true)) ? (byte)1 : (byte)0;


                if (dt.Rows[0]["PregnantDate"].ToString().Length ==10) // 10 Length menas=dd-MM-yyyy this type length
                {
                    if (isApproved == 1)   // while leave nature is maternity and already this leave are approved.
                    {
                        string[] getColumns = { "LACode", "LeaveId", "FromDate", "ToDate", "WeekHolydayNo", "TotalDays", "Remarks", "StateStatus","IsApproved",
                                               "ApprovedDate", "IsProcessessed", "EmpTypeId", "PregnantDate", "PrasaberaDate", "EntryDate", "CompanyId", "SftId","EmpId","ApprovedRejected","DptId","DsgId","EmpStatus" };
                        string[] getValues = {dt.Rows[0]["LACode"].ToString(), dt.Rows[0]["LeaveId"].ToString(), convertDateTime.getCertainCulture(dt.Rows[0]["FromDate"].ToString()).ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["ToDate"].ToString()).ToString(),dt.Rows[0]["WeekHolydayNo"].ToString(),
                                             dt.Rows[0]["TotalDays"].ToString(),dt.Rows[0]["Remarks"].ToString(),dt.Rows[0]["StateStatus"].ToString(),isApproved.ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["ApprovedDate"].ToString()).ToString(),isProcessed.ToString(),
                                             dt.Rows[0]["EmpTypeId"].ToString(),convertDateTime.getCertainCulture(dt.Rows[0]["PregnantDate"].ToString()).ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["PrasaberaDate"].ToString()).ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["EntryDate"].ToString()).ToString(),
                                             dt.Rows[0]["CompanyId"].ToString(),dt.Rows[0]["SftId"].ToString(),dt.Rows[0]["EmpId"].ToString(),"Approved",
                                             dt.Rows[0]["DptId"].ToString(),dt.Rows[0]["DsgId"].ToString(),dt.Rows[0]["EmpStatus"].ToString()};
                        SQLOperation.forSaveValue("Leave_LeaveApplication_Log", getColumns, getValues, sqlDB.connection);
                      
                        ChangeEmpTypeForML(dt.Rows[0]["EmpId"].ToString());  // for employee current status changed when leave type are maternity leave 
                    }
                    else // while leave nature is maternity and  leave are not approved
                    {
                        string[] getColumns = { "LACode", "LeaveId", "FromDate", "ToDate", "WeekHolydayNo", "TotalDays", "Remarks", "StateStatus", "IsApproved",
                                               "IsProcessessed", "EmpTypeId", "PregnantDate", "PrasaberaDate", "EntryDate", "CompanyId", "SftId","EmpId","ApprovedRejected","DptId","DsgId","EmpStatus" };
                        string[] getValues = {dt.Rows[0]["LACode"].ToString(), dt.Rows[0]["LeaveId"].ToString(), convertDateTime.getCertainCulture(dt.Rows[0]["FromDate"].ToString()).ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["ToDate"].ToString()).ToString(),dt.Rows[0]["WeekHolydayNo"].ToString(),
                                             dt.Rows[0]["TotalDays"].ToString(),dt.Rows[0]["Remarks"].ToString(),dt.Rows[0]["StateStatus"].ToString(),isApproved.ToString(),
                                             isProcessed.ToString(),
                                             dt.Rows[0]["EmpTypeId"].ToString(),convertDateTime.getCertainCulture(dt.Rows[0]["PregnantDate"].ToString()).ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["PrasaberaDate"].ToString()).ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["EntryDate"].ToString()).ToString(),
                                             dt.Rows[0]["CompanyId"].ToString(),dt.Rows[0]["SftId"].ToString(),dt.Rows[0]["EmpId"].ToString(),"Rejected",
                                             dt.Rows[0]["DptId"].ToString(),dt.Rows[0]["DsgId"].ToString(),dt.Rows[0]["EmpStatus"].ToString()};
                        SQLOperation.forSaveValue("Leave_LeaveApplication_Log", getColumns, getValues, sqlDB.connection);
                    }
                }
                else  // here count every leave without maternity leave type,so here just considering approved status for approved date
                {
                    if (isApproved == 1)
                    {
                        string[] getColumns = { "LACode", "LeaveId", "FromDate", "ToDate", "WeekHolydayNo", "TotalDays", "Remarks", "StateStatus", "IsApproved",
                                               "ApprovedDate", "IsProcessessed", "EmpTypeId","EntryDate", "CompanyId", "SftId","EmpId","ApprovedRejected","DptId","DsgId","EmpStatus" };
                        string[] getValues = {dt.Rows[0]["LACode"].ToString(), dt.Rows[0]["LeaveId"].ToString(), convertDateTime.getCertainCulture(dt.Rows[0]["FromDate"].ToString()).ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["ToDate"].ToString()).ToString(),dt.Rows[0]["WeekHolydayNo"].ToString(),
                                             dt.Rows[0]["TotalDays"].ToString(),dt.Rows[0]["Remarks"].ToString(),dt.Rows[0]["StateStatus"].ToString(),isApproved.ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["ApprovedDate"].ToString()).ToString(),isProcessed.ToString(),
                                             dt.Rows[0]["EmpTypeId"].ToString(),                          
                                             convertDateTime.getCertainCulture(dt.Rows[0]["EntryDate"].ToString()).ToString(),
                                             dt.Rows[0]["CompanyId"].ToString(),dt.Rows[0]["SftId"].ToString(),dt.Rows[0]["EmpId"].ToString(),"Approved",
                                             dt.Rows[0]["DptId"].ToString(),dt.Rows[0]["DsgId"].ToString(),dt.Rows[0]["EmpStatus"].ToString()};
                        SQLOperation.forSaveValue("Leave_LeaveApplication_Log", getColumns, getValues, sqlDB.connection);
                    }
                    else
                    {
                        string[] getColumns = { "LACode", "LeaveId", "FromDate", "ToDate", "WeekHolydayNo", "TotalDays", "Remarks", "StateStatus", "IsApproved",
                                               "IsProcessessed", "EmpTypeId","EntryDate", "CompanyId", "SftId","EmpId","ApprovedRejected","DptId","DsgId","EmpStatus" };
                        string[] getValues = {dt.Rows[0]["LACode"].ToString(), dt.Rows[0]["LeaveId"].ToString(), convertDateTime.getCertainCulture(dt.Rows[0]["FromDate"].ToString()).ToString(),
                                             convertDateTime.getCertainCulture(dt.Rows[0]["ToDate"].ToString()).ToString(),dt.Rows[0]["WeekHolydayNo"].ToString(),
                                             dt.Rows[0]["TotalDays"].ToString(),dt.Rows[0]["Remarks"].ToString(),dt.Rows[0]["StateStatus"].ToString(),isApproved.ToString(),
                                             isProcessed.ToString(),
                                             dt.Rows[0]["EmpTypeId"].ToString(),                          
                                             convertDateTime.getCertainCulture(dt.Rows[0]["EntryDate"].ToString()).ToString(),
                                             dt.Rows[0]["CompanyId"].ToString(),dt.Rows[0]["SftId"].ToString(),dt.Rows[0]["EmpId"].ToString(),"Rejected",
                                             dt.Rows[0]["DptId"].ToString(),dt.Rows[0]["DsgId"].ToString(),dt.Rows[0]["EmpStatus"].ToString()};

                        SQLOperation.forSaveValue("Leave_LeaveApplication_Log", getColumns, getValues, sqlDB.connection);
                    }
                
                }


                if (isApproved==1) // if and date are changed then change leave application date and total days changed
                {
                    TextBox txtFromDate = (TextBox)gvForApprovedList.Rows[rIndex].Cells[2].FindControl("txtFromDate");
                    TextBox txtToDate = (TextBox)gvForApprovedList.Rows[rIndex].Cells[3].FindControl("txtToDate");

                    if (!ViewState["__OldFromDate__"].ToString().Equals(txtFromDate.Text.Trim()) || !ViewState["__OldToDate__"].ToString().Equals(txtToDate.Text.Trim()))
                    {
                        TextBox txtTotalDays = (TextBox)gvForApprovedList.Rows[rIndex].Cells[3].FindControl("txtTotalDays");

                        string[] getColumns = { "FromDate", "ToDate", "TotalDays", "WeekHolydayNo" };
                        string[] getValues = { convertDateTime.getCertainCulture(txtFromDate.Text.Trim()).ToString(), convertDateTime.getCertainCulture(txtToDate.Text.Trim()).ToString(), txtTotalDays.Text.Trim(), ViewState["__WHnumber__"].ToString()};
                        SQLOperation.forUpdateValue("Leave_LeaveApplication", getColumns, getValues, "LACode", gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);

                        saveLeaveDetails(gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), txtFromDate.Text, txtToDate.Text, dt.Rows[0]["EmpId"].ToString());  // for save leave details
                    }
                }

                if (ViewState["__Predecate__"].ToString().Equals("No")) NoApproved(rIndex);
                
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
                    string[] getValues = { LACode,EmpId, convertDateTime.getCertainCulture(Date).ToString(), "0" };
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
                }


            }
            catch { }
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

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
                lblMessage.InnerText = "";
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                classes.commonTask.loadDepartmentListByCompanyAndShift(ddlDepartmentList, CompanyId, ddlShiftName.SelectedValue.ToString());

                searchLeaveApplicationForApproved();   // searching operation
            }
            catch { }
        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                searchLeaveApplicationForApproved();   // searching operation
            }
            catch { }
        }

        protected void ddlFindingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                searchLeaveApplicationForApproved();   // searching operation
            }
            catch { }
        }

        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
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
                getSQLCMD = " SELECT SrLvID,EmpId, EmpName, format(LvDate,'dd-MM-yyyy') as LvDate, FromTime, ToTime, LvTime, CompanyName, Address, DsgName, Remarks"                   
                    + " FROM"
                    + " v_Leave_ShortLeave"
                    + " where SrLvID=" + LaCode + "";
                sqlDB.fillDataTable(getSQLCMD, dt);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry any payslip are not founded"; return;
                }
                Session["__Language__"] = "English";
                Session["__ShortLeaveApplication__"] = dt;               
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ShortLeaveApplication');", true);  //Open New Tab for Sever side code

            }
            catch { }
        }

        private void saveAprovalLog(int rIndex, string Approval)
        {
            try
            {
                string[] getColumns = { "LACode", "UserID", "DateTime", "Approval", "DptId" };
                string[] getValues = { gvForApprovedList.DataKeys[rIndex].Values[0].ToString(), ViewState["__UserId__"].ToString(), DateTime.Now.ToString(), Approval, gvForApprovedList.DataKeys[rIndex].Values[1].ToString() };
                SQLOperation.forSaveValue("Leave_ApprovalLog", getColumns, getValues, sqlDB.connection);
            }
            catch { }


        }
    }
}