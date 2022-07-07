using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System.Data;
using System.Data.SqlClient;
using SigmaERP.classes;
using System.Drawing;

namespace SigmaERP.personnel
{
    public partial class separation : System.Web.UI.Page
    {
        DataTable dt;
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
               
                loadSeparationInfo();
               // classes.Employee.LoadEmpCardNo_ForSeperation
                classes.commonTask.loadSeparationType(ddlSeparationType);
                btnDelete.CssClass = "";
                btnDelete.Visible= false;
                if (!classes.commonTask.HasBranch())
                {
                    ddlCompany.Enabled = false;
                    ddlSearchCompany.Enabled = false;
                }

                classes.Employee.LoadEmpCardNo_ForSeperation(ddlEmpCardNo,ddlCompany.SelectedValue);
                load_CurrentSeperationList();
                load_SeperationListForActivation();
                load_SeperationActivation_Log();
            }
        }
        private void setPrivilege()
        {
            try
            {
               
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__G_UserId__"] = getUserId;
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "separation.aspx", ddlCompany, gvSeparationList, btnSave);
                
                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                if (ViewState["__ReadAction__"].ToString().Equals("0"))
                {
                    gvCurrentSeperationList.Visible = false;
                }
              
            
               ddlSearchCompany.DataTextField = "Text";
               ddlSearchCompany.DataValueField = "Value";;
               ddlSearchCompany.DataSource = ddlCompany.Items;
               ddlSearchCompany.DataBind();

               ddlCompanyListActive.DataTextField = "Text";
               ddlCompanyListActive.DataValueField = "Value"; ;
               ddlCompanyListActive.DataSource = ddlCompany.Items;
               ddlCompanyListActive.DataBind();

               ddlCompanyCurrentList.DataTextField = "Text";
               ddlCompanyCurrentList.DataValueField = "Value"; ;
               ddlCompanyCurrentList.DataSource = ddlCompany.Items;
               ddlCompanyCurrentList.DataBind();


               ddlCompanyListActiveLog.DataTextField = "Text";
               ddlCompanyListActiveLog.DataValueField = "Value"; ;
               ddlCompanyListActiveLog.DataSource = ddlCompany.Items;
               ddlCompanyListActiveLog.DataBind();


               ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
               ddlSearchCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
               ddlCompanyCurrentList.SelectedValue = ViewState["__CompanyId__"].ToString();
               ddlCompanyListActive.SelectedValue = ViewState["__CompanyId__"].ToString();
               ddlCompanyListActiveLog.SelectedValue = ViewState["__CompanyId__"].ToString(); 
               

            }
            catch { }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
                        
            if (ddlSeparationType.SelectedValue == "50")
            {
                lblMessage.InnerText = "warning->Please Select Separation Type";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                return;
            }
            if (btnSave.Text.Trim().Equals("Save")) 
            {
                string [] EmpCards = ddlEmpCardNo.SelectedValue.ToString().Split('|');
                ViewState["__G_EmpId__"] = EmpCards[0];
                ViewState["__G_EmpCardNo__"] = EmpCards[1];
                ViewState["__G_EmpTypeId__"] = EmpCards[2];
                saveEmpSeparation();
            }               
            else updateEmpSeparation();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }

        private void saveEmpSeparation()
        {
            try
            {
               // SQLOperation.selectBySetCommandInDatatable("select EmpId from Personnel_EmployeeInfo where EmpCardNo=" + txtEmpCardNo.Text.Trim() + "", dt = new DataTable(), sqlDB.connection);
                cmd = new SqlCommand("insert into Personnel_EmpSeparation (EmpId,EmpCardNo,EffectiveDate,SeparationType,Remarks,EmpTypeId,EntryDate,IsActive,UserId) values (@EmpId,@EmpCardNo,@EffectiveDate,@SeparationType,@Remarks,@EmpTypeId,@EntryDate,@IsActive,@UserId)", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", ViewState["__G_EmpId__"].ToString());
                cmd.Parameters.AddWithValue("@EmpCardNo", ViewState["__G_EmpCardNo__"].ToString());
                cmd.Parameters.AddWithValue("@EffectiveDate",convertDateTime.getCertainCulture(txtEffectiveDate.Text));
                cmd.Parameters.AddWithValue("@SeparationType",ddlSeparationType.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Remarks",txtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpTypeId", ViewState["__G_EmpTypeId__"].ToString());
                cmd.Parameters.AddWithValue("@EntryDate",convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")));
                if (txtEffectiveDate.Text.Trim() == DateTime.Now.ToString("dd-MM-yyyy")) cmd.Parameters.AddWithValue("@IsActive",1);
                else cmd.Parameters.AddWithValue("@IsActive", 0);
                cmd.Parameters.AddWithValue("UserId", ViewState["__G_UserId__"].ToString());
                int result = cmd.ExecuteNonQuery();
                if (result==1)
                {
                  
                    string[] getMonth=txtEffectiveDate.Text.Split('-');
                    SqlCommand delcmd = new SqlCommand("Delete From tblAttendanceRecord where ATTDate>'" + getMonth[2] + "-" + getMonth[1] + "-" + getMonth[0] + "'and EmpId='" + ViewState["__G_EmpId__"].ToString() + "'", sqlDB.connection);
                    delcmd.ExecuteNonQuery();
                    cmd = new SqlCommand("Update Personnel_EmpCurrentStatus set EmpStatus=" + ddlSeparationType.SelectedValue.ToString() + " where EmpId='" + ViewState["__G_EmpId__"].ToString() + "'", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("Update Personnel_EmployeeInfo set EmpStatus=" + ddlSeparationType.SelectedValue.ToString() + " where EmpId='" + ViewState["__G_EmpId__"].ToString() + "'", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    loadSeparationInfo();
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                    {
                        classes.Employee.LoadEmpCardNo_ForSeperation(ddlEmpCardNo, ddlCompany.SelectedValue);
                    }
                    else
                    {
                        classes.Employee.LoadEmpCardNo_ForSeperation(ddlEmpCardNo, ViewState["__CompanyId__"].ToString());
                    }
                    lblMessage.InnerText = "success->Successfully Saved";
                    
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }

        private void updateEmpSeparation()
        {
            try
            {

                cmd = new SqlCommand("update Personnel_EmpSeparation set  EffectiveDate=@EffectiveDate,SeparationType=@SeparationType,Remarks=@Remarks,IsActive=@IsActive where EmpSeparationId=" + ViewState["__SeparationId__"].ToString() + "", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EffectiveDate", convertDateTime.getCertainCulture(txtEffectiveDate.Text));
                cmd.Parameters.AddWithValue("@SeparationType", ddlSeparationType.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                if (txtEffectiveDate.Text.Trim() == DateTime.Now.ToString("dd-MM-yyyy")) cmd.Parameters.AddWithValue("@IsActive", 1);
                else cmd.Parameters.AddWithValue("@IsActive", 0);

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    string[] getMonth = txtEffectiveDate.Text.Split('-');
                    SqlCommand delcmd = new SqlCommand("Delete From tblAttendanceRecord where ATTDate>'" + getMonth[2] + "-" + getMonth[1] + "-" + getMonth[0] + "'and EmpId='" + ViewState["__G_EmpId__"].ToString() + "'", sqlDB.connection);
                    delcmd.ExecuteNonQuery();
                    cmd = new SqlCommand("Update Personnel_EmpCurrentStatus set EmpStatus=" + ddlSeparationType.SelectedValue.ToString() + " where EmpCardNo='" + ViewState["__G_EmpCardNo__"] .ToString()+ "' and EmpTypeId=" + ViewState["__G_EmpTypeId__"].ToString()+ "", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("Update Personnel_EmployeeInfo set EmpStatus=" + ddlSeparationType.SelectedValue.ToString() + " where EmpCardNo='" + ViewState["__G_EmpCardNo__"].ToString() + "' and EmpTypeId=" + ViewState["__G_EmpTypeId__"].ToString().ToString()+ "", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    loadSeparationInfo();
                    classes.Employee.LoadEmpCardNoWithName(ddlEmpCardNo, ViewState["__G_EmpTypeId__"].ToString());
                    lblMessage.InnerText = "success->Successfully Updated";

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    btnSave.Text = "Save";
                    if (ViewState["__WriteAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "css_btn Ptbut";
                    }
                    ddlEmpCardNo.Enabled = true;
                    ddlSeparationType.SelectedIndex = 0;
                    txtEffectiveDate.Text = "";
                    txtRemarks.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                loadSeparationInfo();
            }
        }

        private void loadSeparationInfo()
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select EmpSeparationId,EmpId,EmpCardNo,EmpName,EmpTypeId,convert(varchar(11),EffectiveDate,105) as EffectiveDate,EmpStatusName,EmpType,convert(varchar(11),EntryDate,105) as EntryDate,Remarks  from v_Personnel_EmpSeparation where IsActive='false' ", dt = new DataTable(), sqlDB.connection);
                gvSeparationList.DataSource = dt;
                gvSeparationList.DataBind();
            }
            catch { }
        }      

        protected void gvSeparationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                
                if (e.CommandName == "Alter")
                {
                    string a = gvSeparationList.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values[1].ToString();
                   
                  //  txtEmpCardNo.Text = gvSeparationList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text;
                    txtEffectiveDate.Text = gvSeparationList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text;
                    txtRemarks.Text = gvSeparationList.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values[2].ToString();

                    if (gvSeparationList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text.ToLower().Equals("dismissed")) ddlSeparationType.SelectedValue = "3"; 
                    else if (gvSeparationList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text.ToLower().Equals("resigned")) ddlSeparationType.SelectedValue = "4";
                    else if (gvSeparationList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text.ToLower().Equals("terminate")) ddlSeparationType.SelectedValue = "5";
                    else if (gvSeparationList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text.ToLower().Equals("discharged")) ddlSeparationType.SelectedValue = "6";
                    else if (gvSeparationList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text.ToLower().Equals("unauthorized")) ddlSeparationType.SelectedValue = "7";
                    btnSave.Text = "Update";
                    if (ViewState["__UpdateAction__"].ToString().Equals("1"))
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "css_btn Ptbut";
                    }
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        btnDelete.Visible = true;
                        btnDelete.CssClass = "css_btn Ptbut";
                    }
                    ViewState["__G_EmpId__"] = gvSeparationList.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values[3].ToString();
                    ViewState["__G_EmpCardNo__"] = gvSeparationList.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text;
                    ViewState["__G_EmpTypeId__"] = gvSeparationList.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values[1].ToString();
                    ViewState["__SeparationId__"] = gvSeparationList.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values[0].ToString();
                   // 00000005|AHG2016010004|2
                  //  int aa = ddlEmpCardNo.Items.Count;
                  //  string ddlValue=ViewState["__G_EmpId__"].ToString()+"|"+ViewState["__G_EmpCardNo__"].ToString()+"|"+ViewState["__G_EmpTypeId__"].ToString();
                   // ddlEmpCardNo.SelectedIndex = 1;
                  //  ddlEmpCardNo.SelectedItem.Value = ddlValue;
                  //  ddlEmpCardNo.SelectedValue = ddlValue;
                    ddlEmpCardNo.Enabled = false;
                }
                else if (e.CommandName == "Remove")
                {
                    if (SQLOperation.forDeleteRecordByIdentifier("Personnel_EmpSeparation", "EmpSeparationId", gvSeparationList.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values[0].ToString(), sqlDB.connection))
                    {
                        lblMessage.InnerText = "success-> Successfully Deleted.";
                        gvSeparationList.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Visible = false;
                    }                   
  
                }
                
            }
            catch { }
        }
    
        protected void gvSeparationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                loadSeparationInfo();
            }
            catch { }
            gvSeparationList.PageIndex = e.NewPageIndex;
            Session["pageNumber"] = e.NewPageIndex;
            gvSeparationList.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            btnSave.Text = "Save";
            ddlEmpCardNo.Enabled = true;
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                if(txtSearchEmp.Text.Trim()=="")
                {
                    lblMessage.InnerText = "warning->Please type valid card no.";
                    txtSearchEmp.Focus();
                    return;
                }
                
                SQLOperation.selectBySetCommandInDatatable("select EmpSeparationId,EmpId,EmpCardNo,EmpName,EmpTypeId,convert(varchar(11),EffectiveDate,105) as EffectiveDate,EmpStatusName,EmpType,convert(varchar(11),EntryDate,105) as EntryDate,Remarks  from v_Personnel_EmpSeparation where CompanyId='" + ddlSearchCompany.SelectedValue + "' and EmpCardNo like '%" + txtSearchEmp.Text.Trim() + "' And IsActive='false'", dt = new DataTable(), sqlDB.connection);

                gvSeparationList.DataSource = dt;
                gvSeparationList.DataBind();
                if (dt.Rows.Count == 0)
                    lblMessage.InnerText = "warning->"+txtSearchEmp.Text+" Not Found";
            }
            catch { }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearchEmp.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            
            loadSeparationInfo();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {                
                SqlCommand cmd = new SqlCommand("Delete From Personnel_EmpSeparation where EmpSeparationId=" + ViewState["__SeparationId__"].ToString() + "", sqlDB.connection);
                int result = cmd.ExecuteNonQuery();
                if (result==1)
                {
                    cmd = new SqlCommand("Update Personnel_EmpCurrentStatus set EmpStatus=1 where EmpCardNo='" + ViewState["__G_EmpCardNo__"].ToString() + "' and EmpTypeId=" + ViewState["__G_EmpTypeId__"].ToString() + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("Update Personnel_EmployeeInfo set EmpStatus=1 where EmpCardNo='" + ViewState["__G_EmpCardNo__"].ToString() + "' and EmpTypeId=" + ViewState["__G_EmpTypeId__"].ToString() + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    loadSeparationInfo();
                   
                    lblMessage.InnerText = "success->Successfully Deleted";

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    btnDelete.Visible = false;
                    btnDelete.CssClass = "";
                    ddlEmpCardNo.Enabled = true;
                }
            }
            catch { }
        }

        private void load_CurrentSeperationList()
        {
            try
            {
                string CompanyId = (ddlCompanyCurrentList.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyCurrentList.SelectedValue;
                dt = new DataTable();
                sqlDB.fillDataTable("select EmpId,Substring(EmpCardNo,8,10) as EmpCardNo,EmpName,Convert(varchar,EffectiveDate,100) as  EffectiveDate,EmpStatus,EmpStatusName," +
                    " EmpType,EmpTypeId,Convert(varchar,EntryDate,100) as  EntryDate,(FirstName+' '+LastName) as UserName,Remarks from v_Personnel_EmpSeparation " +
                    " where CompanyId='"+CompanyId+"' and IsActive='True'",dt);               
                gvCurrentSeperationList.DataSource = dt;
                gvCurrentSeperationList.DataBind();
            }
            catch { }
        }

        private void load_SeperationListForActivation()
        {
            try
            {
                string CompanyId = (ddlCompanyListActive.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString(): ddlCompanyListActive.SelectedValue;
                dt = new DataTable();
                if(txtEmpCardNo.Text.Trim().Length==0)
                    sqlDB.fillDataTable(" select  EmpSeparationId,EmpId,Substring(EmpCardNo,8,10) as EmpCardNo,EmpName,EmpType,DptName,DsgName,EmpStatusName, convert(VARCHAR(10),EffectiveDate, 105) AS EffectiveDate, convert(VARCHAR(10), GETDATE(), 105) AS CurrentDate  " +
                    "from v_Personnel_EmpSeparation  "+
                    "where EmpSeparationId in ( select max(EmpSeparationId) from v_Personnel_EmpSeparation  where CompanyId='" + CompanyId + "' group by EmpId) " +
                    "and Empid not In(select EmpId from Personnel_EmpCurrentStatus where CompanyId='" + CompanyId + "' and EmpStatus=1) " +
                    "order by EffectiveDate desc,EmpSeparationId desc", dt);     
                else
                    sqlDB.fillDataTable(" select  EmpSeparationId,EmpId,Substring(EmpCardNo,8,10) as EmpCardNo,EmpName,EmpType,DptName,DsgName,EmpStatusName, convert(VARCHAR(10),EffectiveDate, 105) AS EffectiveDate, convert(VARCHAR(10), GETDATE(), 105) AS CurrentDate  " +
                    "from v_Personnel_EmpSeparation  " +
                    "where EmpSeparationId in ( select max(EmpSeparationId) from v_Personnel_EmpSeparation  where CompanyId='" + CompanyId + "' and EmpCardNo like'%"+txtEmpCardNo.Text.Trim()+"' group by EmpId) " +
                    "and Empid not In(select EmpId from Personnel_EmpCurrentStatus where CompanyId='" + CompanyId + "' and EmpCardNo like'%" + txtEmpCardNo.Text.Trim() + "' and EmpStatus=1) " +
                    "order by EffectiveDate desc,EmpSeparationId desc", dt);     

                gvCurrentSeperationListForActivation.DataSource = dt;
                gvCurrentSeperationListForActivation.DataBind();
            }
            catch { }
        }
        private void load_SeperationActivation_Log()
        {
            try
            {
                string CompanyId = (ddlCompanyListActiveLog.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyListActiveLog.SelectedValue;
                dt = new DataTable();
                if (txtEmpCardNo.Text.Trim().Length == 0)
                    sqlDB.fillDataTable(" select SUBSTRING(EmpCardNo,8,10) as EmpCardNo ,EmpName,EmpType,DptName,DsgName,format(ActiveDate,'dd-MM-yyyy') as ActiveDate,"+
                        "FirstName+' '+LastName as UName,Remark from Personnel_SeparationActivation_Log inner join v_EmployeeDetails on " +
                        "Personnel_SeparationActivation_Log.EmpId=v_EmployeeDetails.EmpId inner join UserAccount "+
                        " on Personnel_SeparationActivation_Log.UserId=UserAccount.UserId "+
                        "where v_EmployeeDetails.CompanyId ='" + CompanyId + "' " +
                        "order by ActiveDate desc", dt);
                else                    
                        sqlDB.fillDataTable(" select SUBSTRING(EmpCardNo,8,10) as EmpCardNo ,EmpName,EmpType,DptName,DsgName,format(ActiveDate,'dd-MM-yyyy') as ActiveDate," +
                            "FirstName+' '+LastName as UName,Remark from Personnel_SeparationActivation_Log inner join v_EmployeeDetails on " +
                            "Personnel_SeparationActivation_Log.EmpId=v_EmployeeDetails.EmpId inner join UserAccount " +
                            " on Personnel_SeparationActivation_Log.UserId=UserAccount.UserId " +
                            "where v_EmployeeDetails.CompanyId ='" + CompanyId + "' and EmpCardNo like'%" + txtCardnoActive.Text.Trim() + "' " +
                            "order by ActiveDate desc", dt);      
                
                gvSeparationActivitionLog.DataSource = dt;
                gvSeparationActivitionLog.DataBind();
            }
            catch { }
        }
        protected void gvSeparationList_RowDataBound(object sender, GridViewRowEventArgs e)
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

                try
                {

                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        Button btnAlter = new Button();
                        btnAlter = (Button)e.Row.FindControl("btnAlter");
                        btnAlter.Enabled = false;
                        btnAlter.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }

        protected void gvCurrentSeperationList_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void ddlCompanyListActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_SeperationListForActivation();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            load_SeperationListForActivation();
        }

        protected void ddlCompanyCurrentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_CurrentSeperationList();
        }

        protected void gvCurrentSeperationListForActivation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                load_SeperationListForActivation();
            }
            catch { }
            gvCurrentSeperationListForActivation.PageIndex = e.NewPageIndex;
            gvCurrentSeperationListForActivation.DataBind();
        }

        protected void gvCurrentSeperationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                load_CurrentSeperationList();
            }
            catch { }
            gvCurrentSeperationList.PageIndex = e.NewPageIndex;
            gvCurrentSeperationList.DataBind();
        }

        protected void gvCurrentSeperationListForActivation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Active") 
            {
                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                string EmpSeparationID = gvCurrentSeperationListForActivation.DataKeys[rIndex].Values[0].ToString();
                string EmpID = gvCurrentSeperationListForActivation.DataKeys[rIndex].Values[1].ToString();
                TextBox txtActiveDate = gvCurrentSeperationListForActivation.Rows[rIndex].FindControl("txtActiveDate") as TextBox;
                TextBox txtRemarks = gvCurrentSeperationListForActivation.Rows[rIndex].FindControl("txtRemarks") as TextBox;
                if (SeparationActivation(EmpID))
                    saveSeparationActivation_Log(EmpID,EmpSeparationID,txtActiveDate.Text.Trim(),txtRemarks.Text.Trim());
                else
                    lblMessage.InnerText="Error-> Unable to Active !";

            }
        }

        private bool SeparationActivation(string EmpId)
        {
            try
            {
                SqlCommand cmd2;
                cmd = new SqlCommand("Update  Personnel_EmployeeInfo set EmpStatus=1 where EmpId='" + EmpId + "'", sqlDB.connection);
                cmd2 = new SqlCommand("Update  Personnel_EmpCurrentStatus set EmpStatus=1 where SN= (select Max(SN) from Personnel_EmpCurrentStatus where EmpId='" + EmpId + "')", sqlDB.connection);
                if (int.Parse(cmd.ExecuteNonQuery().ToString()) == 1 && int.Parse(cmd2.ExecuteNonQuery().ToString())==1)
                    return true;
                else
                    return false;             
                
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                return false;
            }
        }
        
        private void saveSeparationActivation_Log(string EmpId,string SeparationID,string ActiveDate,string Remark)
        {
            try
            {
                
                cmd = new SqlCommand("insert into Personnel_SeparationActivation_Log (EmpId,EmpSeparationId,ActiveDate,Remark,UserId) values (@EmpId,@EmpSeparationId,@ActiveDate,@Remark,@UserId)", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", EmpId);
                cmd.Parameters.AddWithValue("@EmpSeparationId", SeparationID);
                cmd.Parameters.AddWithValue("@ActiveDate", convertDateTime.getCertainCulture(ActiveDate));
                cmd.Parameters.AddWithValue("@Remark", Remark);               
                cmd.Parameters.AddWithValue("UserId", ViewState["__G_UserId__"].ToString());
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    lblMessage.InnerText = "success->Successfully Actived";
                    load_SeperationListForActivation();
                    load_SeperationActivation_Log();
                }                 

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }

        protected void gvSeparationActivitionLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

                     try
            {
                 load_SeperationActivation_Log();
            }
            catch { }
            gvSeparationActivitionLog.PageIndex = e.NewPageIndex;
            gvSeparationActivitionLog.DataBind();
        }

        protected void ddlCompanyListActiveLog_SelectedIndexChanged(object sender, EventArgs e)
        {
             load_SeperationActivation_Log();
        }

        protected void btnSearchLog_Click(object sender, EventArgs e)
        {
            load_SeperationActivation_Log();
        }

  

        protected void gvCurrentSeperationListForActivation_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    if (ViewState["__WriteAction__"].ToString().Equals("0"))
                    {
                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnActive");
                        btn.Enabled = false;
                        btn.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }
        


    }
}