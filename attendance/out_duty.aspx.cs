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

namespace SigmaERP.attendance
{
    public partial class out_duty : System.Web.UI.Page
    {
        DataTable dtClient;
        string sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();


            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                Session["__dtClient__"] = "";
                ViewState["__rIndex__"] = "";
               // loadClientList();
                setPrivilege();                
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");               
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
        }


        static DataTable dtSetPrivilege;

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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), ViewState["__getUserId__"].ToString(), ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "aplication.aspx", ddlCompanyList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                ViewState["__IsODAuthority__"] = commonTask.IsODAuthority(ViewState["__CompanyId__"].ToString());

                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
                {
                    ViewState["__ReadAction__"] = "1";
                    findEmpInfo();
                    trEmpCardNo.Visible = false;
                   
                }
                if (ViewState["__ReadAction__"].ToString().Equals("0"))
                {
                    gvOutDuty.Visible = false;
                }
                loadOutDuty();

            }
            catch { }
        }
        DataTable dt;
        string CompanyId;
        private void findEmpInfo() 
        {
            try
            {
               
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
                {
                    CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                   
                    sqlDB.fillDataTable("select EmpId,EmpName,CompanyName,DptName,DsgName,EmpTypeId,DptId,DsgId,SftId from v_Personnel_EmpCurrentStatus where EmpId='" + ViewState["__EmpId__"].ToString() + "' AND IsActive='1' AND  EmpStatus in ('1','8') ",  dt = new DataTable());

                }
                else 
                {
                    if (txtEmpCardNo.Text.Trim().Length < 4)
                    {
                        lblMessage.InnerText = "warning-> Please type valid Card No !";
                        txtEmpCardNo.Focus();
                        return;
                    }
                    CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                    //if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin" && ViewState["__ODOnlyDpt__"].ToString().Equals("True"))
                    //    sqlDB.fillDataTable("select EmpId,EmpName,CompanyName,DptName,DsgName,EmpTypeId,DptId,DsgId,SftId from v_Personnel_EmpCurrentStatus where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like '%" + txtEmpCardNo.Text.Trim() + "'" +
                    //             " AND IsActive='1' AND  EmpStatus in ('1','8')  " +
                    //             " AND CompanyId='" + CompanyId + "' and DptID='"+ ViewState["__DptId__"] .ToString()+ "' ",  dt = new DataTable());
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
                        sqlDB.fillDataTable("select EmpId,EmpName,CompanyName,DptName,DsgName,EmpTypeId,DptId,DsgId,SftId from v_Personnel_EmpCurrentStatus where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like '%" + txtEmpCardNo.Text.Trim() + "'" +
                                 " AND IsActive='1' AND  EmpStatus in ('1','8')  " +
                                 " AND CompanyId='" + CompanyId + "' and ( EmpId='"+ ViewState["__EmpId__"] .ToString()+ "' or EmpID in(select Distinct EmpID from tblOutDutyAuthorityAccessControl where AuthorityID="+ ViewState["__getUserId__"].ToString()+ "))",  dt = new DataTable());
                    else
                        sqlDB.fillDataTable("select EmpId,EmpName,CompanyName,DptName,DsgName,EmpTypeId,DptId,DsgId,SftId from v_Personnel_EmpCurrentStatus where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like '%" + txtEmpCardNo.Text.Trim() + "'" +
                                 " AND IsActive='1' AND  EmpStatus in ('1','8')  " +
                                 " AND CompanyId='" + CompanyId + "' ", dt = new DataTable());
                }
                

              
                if (dt== null || dt.Rows.Count==0) 
                {
                    lblMessage.InnerText = "error->Please type valid employee card no";
                    divFindInfo.InnerText = "";
                }
                else
                {
                    divFindInfo.Style.Add("Color", "Green");
                    divFindInfo.InnerText = "Name: " + dt.Rows[0]["EmpName"].ToString() + " , Designation : " + dt.Rows[0]["DsgName"].ToString() + " , Department: " + dt.Rows[0]["DptName"].ToString();
                    ViewState["__EmpId__"] = dt.Rows[0]["EmpId"].ToString();
                    ViewState["__DptId__"] = dt.Rows[0]["DptId"].ToString();

                }
            }
            catch { }
        }


        protected void btnFindEmpInfo_Click(object sender, EventArgs e)
        {
            findEmpInfo();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Update")
            {
                if (!checkIsPending(ViewState["__ODID__"].ToString()))
                {
                    lblMessage.InnerText = "warning-> The approved/rejectd out duty record can't be updated.";
                    return;
                }
                if (!checkClientList())
                {
                    lblMessage.InnerText = "warning-> First add client info then save";
                    return;
                }
                updateOutDuty(ViewState["__ODID__"].ToString());
                deleteOutDutyDetails(ViewState["__ODID__"].ToString());
                saveOutDutyDetails(ViewState["__ODID__"].ToString());
                loadOutDuty();
                lblMessage.InnerText = "success-> Successfully Update.";
                clearAll();
            }
            else
            {
                if (txtDate.Text.Trim().Length < 8)
                {
                    lblMessage.InnerText = "warning-> Please select date !";
                    txtDate.Focus();
                    return;
                }
                if (divFindInfo.InnerText == "")
                {
                    lblMessage.InnerText = "warning-> Please Find any Employee by Card No ! "; btnFindEmpInfo.Focus(); return;
                }
                if (rblDutyType.SelectedValue == "0")
                    saveOutDuty();
                else
                    saveTraining();
            }
           
        }
        private bool Validation() 
        {
            try 
            {
                sqlDB.fillDataTable("select EmpId, case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status,Date from v_tblOutDuty where  Status<>2 and EmpId='" + ViewState["__EmpId__"].ToString() + "' and convert(varchar(10),Date,105)='" + txtDate.Text.Trim() + "'", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    lblMessage.InnerText = "warning-> This date already applied and " + dt.Rows[0]["Status"].ToString();
                    return false;
                }
                return true;
            }
            catch { return false; }
           
        }
        private void saveTraining()
        {
            try
            {
                if (!Validation()) return;// this validation use to check existing record
                string ODProcessingOrder = "0";
                string Status ="0";
                
                if (bool.Parse(ViewState["__IsODAuthority__"].ToString()))// if this company use OD appoval system
                {
                    ODProcessingOrder = getODAuthority();
                    if (ODProcessingOrder.Trim() == "") // OD Authority is not found. plz setup for this employee.
                    {
                        
                        lblMessage.InnerText = "warning-> You are not able to apply for Out Duty. Please contact with Authority.";
                        return;
                        // 
                    }
                    else if (ODProcessingOrder.Trim() == "-1")// This Employee is allowed for direct approval
                    {
                        Status = "1";
                    }
                }
                else
                    Status = "1";


                
                     string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

            

                    string[] getColumns = { "EmpId", "Date", "Type", "Remark", "Status", "Processing", "AppliedBy","AppiedDate", "StraightFromHome" , "StraightToHome" };
                    string[] getValues = { ViewState["__EmpId__"].ToString(), commonTask.ddMMyyyyToyyyyMMdd(txtDate.Text.Trim()).ToString(),
                                             rblDutyType.SelectedValue,txtRemark.Text.Trim(),Status,ODProcessingOrder,ViewState["__getUserId__"].ToString(),
                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),ckbStraightFromHome.Checked.ToString(),ckbStraightToHome.Checked.ToString()};
                  if (SQLOperation.forSaveValue("tblOutDuty", getColumns, getValues, sqlDB.connection) == true)
                   {
                        loadOutDuty();
                        lblMessage.InnerText = "success-> Successfully saved.";
                        clearAll();

                }

                }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private bool checkClientList()
        {
            try
            {
                dtClient = new DataTable();
                dtClient = (DataTable)Session["__dtClient__"];
                if (dtClient != null && dtClient.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }
        private void saveOutDuty()
        {
            try
            {
                if (!checkClientList())
                {
                    lblMessage.InnerText = "warning-> First add client info then save";
                    return;
                }
                if (!Validation()) return;// this validation use to check existing record
                
              
                string ODProcessingOrder = "0";
                string Status = "0";

                if (bool.Parse(ViewState["__IsODAuthority__"].ToString()))// if this company use OD appoval system
                {
                    ODProcessingOrder = getODAuthority();
                    if (ODProcessingOrder.Trim() == "") // OD Authority is not found. plz setup for this employee.
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('You are not able to apply for Out Duty. Please contact with Authority.');", true);
                        return;
                        // 
                    }
                    else if (ODProcessingOrder.Trim() == "-1")// This Employee is allowed for direct approval
                    {
                        Status = "1";
                    }
                }
                else
                    Status = "1";
               
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();


                sql = "insert into tblOutDuty (EmpId, Date, Type, Remark, Status, Processing, AppliedBy, AppiedDate,StraightFromHome, StraightToHome) values('" +
                    ViewState["__EmpId__"].ToString()+"','"+ commonTask.ddMMyyyyToyyyyMMdd(txtDate.Text.Trim()).ToString() + "','"+ rblDutyType.SelectedValue +"'"+
                    ",'',"+ Status + ","+ ODProcessingOrder + ",'"+ ViewState["__getUserId__"].ToString() + "','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+ckbStraightFromHome.Checked.ToString()+ "','" + ckbStraightToHome.Checked.ToString() + "');" +
                    " SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(sql, sqlDB.connection);
                int ODID=int.Parse(cmd.ExecuteScalar().ToString());
                if (ODID>0)
                {
                    saveOutDutyDetails(ODID.ToString());
                    loadOutDuty();
                    lblMessage.InnerText = "success-> Successfully saved.";
                    clearAll();

                }

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private void updateOutDuty(string ODID)
        {
            try
            {
               

                sql = "update tblOutDuty set StraightFromHome='"+ckbStraightFromHome.Checked.ToString() + "',StraightToHome='"+ckbStraightToHome.Checked.ToString()+"' where SL ="+ ODID;
                SqlCommand cmd = new SqlCommand(sql, sqlDB.connection);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private void saveOutDutyDetails(string ODID )
        {
            try
            {
                dtClient = new DataTable();
                try
                {
                    dtClient = (DataTable)Session["__dtClient__"];
                }
                catch { }
                if (dtClient != null && dtClient.Rows.Count > 0)
                {
                    for (int i = 0; i < dtClient.Rows.Count; i++)
                    {
                        string[] getColumns = { "ODID", "ClientName", "InTime", "OutTime", "Purpose" };
                        string[] getValues = { ODID,dtClient.Rows[i]["ClientName"].ToString().Trim(),
                            dtClient.Rows[i]["InTime"].ToString().Trim(),dtClient.Rows[i]["OutTime"].ToString().Trim(),
                        dtClient.Rows[i]["Purpose"].ToString().Trim()};
                        SQLOperation.forSaveValue("tblOutdutyDetails", getColumns, getValues, sqlDB.connection);
                        
                    }
                }
                   

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private bool checkIsPending(string ODID)
        {
            try
            {

                sqlDB.fillDataTable("select SL from tblOutDuty where Status=0 and SL="+ODID, dt = new DataTable());
                if (dt != null && dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void deleteOutDutyDetails(string ODID)
        {
            try
            {
                
                SQLOperation.forDeleteRecordByIdentifier("tblOutdutyDetails", "ODID", ODID, sqlDB.connection);

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private string getODAuthority()
        {
            try
            {
                DataTable dtLvOrder;
                sql = "select case when IsDirectApprove=1 then -1 else   max(AuthorityPosition) end as AuthorityPosition from tblOutDutyAuthorityAccessControl where EmpID='" + ViewState["__EmpId__"].ToString() + "' group by IsDirectApprove";
                sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                if (dtLvOrder == null || dtLvOrder.Rows.Count == 0)
                    return "";
                return dtLvOrder.Rows[0]["AuthorityPosition"].ToString();

                sql = "select ISNULL(max(ODAuthorityOrder),0) ODAuthorityOrder from v_UserAccountforLeave where isODAuthority=1 and DptId in(" + ViewState["__DptId__"].ToString() + ") and EmpId ='"+ ViewState["__EmpId__"] .ToString()+ "'";
                sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                if (!dtLvOrder.Rows[0]["ODAuthorityOrder"].ToString().Equals("0"))
                    sql = "select ISNULL(max(ODAuthorityOrder),0) ODAuthorityOrder from v_UserAccountforLeave where isODAuthority=1 and DptId in(" + ViewState["__DptId__"].ToString() + ") and ODAuthorityOrder<"+ dtLvOrder.Rows[0]["ODAuthorityOrder"].ToString() + "";
                else
                    sql = "select ISNULL(max(ODAuthorityOrder),0) ODAuthorityOrder from v_UserAccountforLeave where isODAuthority=1 and DptId in(" + ViewState["__DptId__"].ToString() + ") ";
                sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                if (!dtLvOrder.Rows[0]["ODAuthorityOrder"].ToString().Equals("0"))
                    return dtLvOrder.Rows[0]["ODAuthorityOrder"].ToString();
                else
                {
                    sql = "select ISNULL(max(ODAuthorityOrder),0) ODAuthorityOrder from v_UserAccountforLeave where isODAuthority=1 and  ODOnlyDpt=0 and EmpId ='" + ViewState["__EmpId__"].ToString() + "'";
                    sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                    if (!dtLvOrder.Rows[0]["ODAuthorityOrder"].ToString().Equals("0"))
                        sql = "select ISNULL(max(ODAuthorityOrder),0) ODAuthorityOrder from v_UserAccountforLeave where isODAuthority=1 and  ODOnlyDpt=0 and ODAuthorityOrder<" + dtLvOrder.Rows[0]["ODAuthorityOrder"].ToString() + "";
                    else
                        sql = "select ISNULL(max(ODAuthorityOrder),0) ODAuthorityOrder from v_UserAccountforLeave where isODAuthority=1 and  ODOnlyDpt=0";
                    sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                    return dtLvOrder.Rows[0]["ODAuthorityOrder"].ToString();
                }
            }
            catch { return ""; }
        }
        private void loadOutDuty() 
        {
            CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
             if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
                 sqlDB.fillDataTable("select ISNULL(StraightFromHome,0) as StraightFromHome ,ISNULL(StraightToHome,0) as StraightToHome, SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome from v_tblOutDuty where EmpId='" + ViewState["__EmpId__"].ToString() + "' and CompanyId='" + CompanyId + "' and Status=0 order by year(Date) desc,month(Date) desc,date desc", dt = new DataTable());
            //else if(ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin" && ViewState["__ODOnlyDpt__"].ToString().Equals("True"))
            //    sqlDB.fillDataTable("select ISNULL(StraightFromHome,0) as StraightFromHome ,ISNULL(StraightToHome,0) as StraightToHome,SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome from v_tblOutDuty where CompanyId='" + CompanyId + "' and DptId='"+ ViewState["__DptId__"].ToString()+ "' and Status=0 order by year(Date) desc,month(Date) desc,date desc", dt = new DataTable());
            else if(ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
                sqlDB.fillDataTable("select ISNULL(StraightFromHome,0) as StraightFromHome ,ISNULL(StraightToHome,0) as StraightToHome,SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome from v_tblOutDuty where CompanyId='" + CompanyId + "' and   ( EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpID in(select Distinct EmpID from tblOutDutyAuthorityAccessControl where AuthorityID=" + ViewState["__getUserId__"].ToString() + ")) and Status=0 order by year(Date) desc,month(Date) desc,date desc", dt = new DataTable());
            else
                sqlDB.fillDataTable("select ISNULL(StraightFromHome,0) as StraightFromHome ,ISNULL(StraightToHome,0) as StraightToHome,SL,EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DptName,DsgName,convert(varchar(10),Date,105) as Date,case  when Status=0 then 'Pending' when Status=1 then 'Approved' when Status=2 then 'Rejected'  end as Status ,Type,case when Type=0 then 'Out Duty' else 'Training' end as TypeName,Remark,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome from v_tblOutDuty where CompanyId='" + CompanyId + "' and Status=0 order by year(Date) desc,month(Date) desc,date desc", dt = new DataTable());

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

                txtDate.Text = ((Label)gvOutDuty.Rows[rIndex].FindControl("lblDate")).Text;
                txtEmpCardNo.Text = ((Label)gvOutDuty.Rows[rIndex].FindControl("lblEmpCode")).Text;
                findEmpInfo();
                ViewState["__ODID__"] = gvOutDuty.DataKeys[rIndex].Values[0].ToString();
                string a = gvOutDuty.DataKeys[rIndex].Values[1].ToString();
                string b = gvOutDuty.DataKeys[rIndex].Values[2].ToString();
                ckbStraightFromHome.Checked = (a == "True") ? true:false ;
                ckbStraightToHome.Checked= (b == "True") ? true : false;

                sqlDB.fillDataTable("select ClientName,InTime,OutTime,Purpose from tblOutdutyDetails where ODID="+ ViewState["__ODID__"].ToString(), dtClient = new DataTable());
                bindToClientList();
                btnSave.Text = "Update";
                pnlInputUnit.Enabled = false;
                rblDutyType.SelectedValue = "0";
                pnlClientInfo.Visible = true;
                gvClient.Visible = true;
                btnAdd.Visible = true;

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
                if (lblType.Text.Trim() == "Training" || lblStatus.Text != "Pending" ) 
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
                    if (ViewState["__DeletAction__"].ToString().Equals("0")||lblStatus.Text!="Pending")
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

       
        private void addClientToList()
        {
            dtClient = new DataTable();
            try
            {
                dtClient = (DataTable)Session["__dtClient__"];
            }
            catch { }
           
            if (dtClient == null || dtClient.Rows.Count==0)
            {
                dtClient = new DataTable();
                dtClient.Columns.Add("ClientName", typeof(string));
                dtClient.Columns.Add("InTime", typeof(string));
                dtClient.Columns.Add("OutTime", typeof(string));
                dtClient.Columns.Add("Purpose", typeof(string));
            }
            DateTime InTime =DateTime .Parse("2019-01-01 "+ddlInHur.SelectedValue+":"+txtInMin.Text+":00 "+ddlInTimeAMPM.SelectedValue);
            DateTime OutTime = DateTime.Parse("2019-01-01 " + ddlOutHur.SelectedValue + ":" + txtOutMin.Text + ":00 " + ddlOutTimeAMPM.SelectedValue);
            dtClient.Rows.Add(new Object[] { txtClientName.Text.Trim(), InTime.ToString("HH:mm:ss"),
            OutTime.ToString("HH:mm:ss"), txtRemark.Text.Trim()});
            bindToClientList();
            clearClientUnit();
        }
        private void bindToClientList()
        {
           
            Session["__dtClient__"] = dtClient;
            gvClient.DataSource = dtClient;
            gvClient.DataBind();
        }
        private void removeRowFromClientList(int rIndex)
        {
            dtClient = new DataTable();
            try
            {
                dtClient = (DataTable)Session["__dtClient__"];
            }
            catch { }
            dtClient.Rows.RemoveAt(rIndex);
            bindToClientList();
            ViewState["__rIndex__"] = "";
        }
        
        protected void rblDutyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblDutyType.SelectedValue == "0")
            {
                pnlClientInfo.Visible = true;
                gvClient.Visible = true;
                btnAdd.Visible = true;
            }
            else
            {
                pnlClientInfo.Visible = false;
                gvClient.Visible = false;
                btnAdd.Visible = false;
            } 
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtClientName.Text.Trim() == "")
            {
                lblMessage.InnerText = "warning-> Please, Enter Client Name.";
                txtClientName.Focus();
                return;
            }
            if (ddlInHur.SelectedValue==" " || txtInMin.Text.Trim()=="")
            {
                lblMessage.InnerText = "warning-> Please,Enter Valid In Time.";
               
                return;
            }
            if (ddlOutHur.SelectedValue == " " || txtOutMin.Text.Trim() == "")
            {
                lblMessage.InnerText = "warning-> Please,Enter Valid Out Time.";               
                return;
            }
            if (!ViewState["__rIndex__"].ToString().Equals(""))
                removeRowFromClientList(int.Parse(ViewState["__rIndex__"].ToString()));
            addClientToList();
        }

        protected void gvClient_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "removeClientRow")
            {
                int rIndex = int.Parse(e.CommandArgument.ToString());
                removeRowFromClientList(rIndex);
            }
            if (e.CommandName == "editClientRow")
            {
                int rIndex = int.Parse(e.CommandArgument.ToString());
                ViewState["__rIndex__"] = rIndex.ToString();
                txtClientName.Text = ((Label)gvClient.Rows[rIndex].FindControl("lblClientName")).Text.Trim();
                string InTime = ((Label)gvClient.Rows[rIndex].FindControl("lblInTime")).Text.Trim();
                string OutTime = ((Label)gvClient.Rows[rIndex].FindControl("lblOutTime")).Text.Trim();
                string lblPurpose = ((Label)gvClient.Rows[rIndex].FindControl("lblPurpose")).Text.Trim();
                txtRemark.Text = lblPurpose;
                DateTime _InTime = DateTime.Parse("2019-01-01 " + InTime);
                DateTime _OutTime = DateTime.Parse("2019-01-01 "+OutTime);
                ddlInHur.SelectedValue = _InTime.ToString("hh");
                txtInMin.Text = _InTime.ToString("mm");
                ddlInTimeAMPM.SelectedValue= _InTime.ToString("tt");

                ddlOutHur.SelectedValue = _OutTime.ToString("hh");
                txtOutMin.Text = _OutTime.ToString("mm");
                ddlOutTimeAMPM.SelectedValue = _OutTime.ToString("tt");
            }
        }

        protected void gvClient_RowDataBound(object sender, GridViewRowEventArgs e)
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
        private void clearAll()
        {
            Session["__dtClient__"] = "";
            ViewState["__ODID__"] = "";
            dtClient = new DataTable();
            bindToClientList();
            ViewState["__rIndex__"] = "";
            clearClientUnit();
            btnSave.Text = "Save";
            pnlInputUnit.Enabled = true;
        }
        private void clearClientUnit()
        {
            txtClientName.Text = "";
            ddlInHur.SelectedValue = " ";
            ddlOutHur.SelectedValue = " ";
            txtInMin.Text = "00";
            txtOutMin.Text = "00";
            txtRemark.Text = "";
        }
    }
}