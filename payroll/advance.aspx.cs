using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ComplexScriptingSystem;
using adviitRuntimeScripting;

namespace SigmaERP.payroll
{
    public partial class advance : System.Web.UI.Page
    {
        
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setDefaultColulmns(); setPrivilege();
                ddlCompanyList.SelectedIndex = 1;
                loadExistsAdvance();
                //classes.Employee.LoadEmpCardNoWithNameByCompanyRShift(ddlEmpCardNo,ddlCompanyList.SelectedValue);
                classes.Employee.LoadEmpCardNoForPayroll(ddlEmpCardNo, ViewState["__CompanyId__"].ToString());
                ViewState["AdvanceId"] = "0";
                ViewState["__EmpId__"] = "0";
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
            }
        }

        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                ViewState["__WriteAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                {
                    classes.commonTask.LoadBranch(ddlCompanyList);
                   // classes.commonTask.LoadShift(ddlShiftList, ViewState["__CompanyId__"].ToString());
                    return;
                }
                else
                {
                    dtSetPrivilege = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='advance.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                        {
                            btnSave.Enabled = false;
                            btnDelete.CssClass = "";
                            btnSave.CssClass = "";
                            btnDelete.Enabled = false;

                        }
                        else
                        { 
                        
                        }
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                           
                            gvAdvanceInfo.Visible = false;
                        }

                        if (bool.Parse(dtSetPrivilege.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            btnSave.CssClass = "";
                            btnSave.Enabled = false;


                        }
                    }
                }
            }
            catch { }
        }

        private void setDefaultColulmns()
        {
            try
            {
                dt = new DataTable();
                dt.Columns.Add("Month",typeof(string));
                dt.Columns.Add("Amount",typeof(string));
                dt.Rows.Add(" "," ");
                gvInstallmentDetails.DataSource = dt;
                gvInstallmentDetails.DataBind();
                gvInstallmentDetails.Columns[0].HeaderStyle.Width = 200;
                gvInstallmentDetails.Columns[0].ItemStyle.Width = 200;
                gvInstallmentDetails.Columns[1].HeaderStyle.Width = 120;
            }
            catch { }
        }

       

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            lblMessage.InnerText = "";
            double getInstallmentMonthANDPayment =Math.Round(( double.Parse(txtAdvanceAmount.Text.Trim()))/int.Parse(txtNoOfInstallment.Text.Trim()),0);
            byte getMonth = byte.Parse(txtStartMonth.Text.Substring(0,2));
            dt = new DataTable();
            dt.Columns.Add("Month", typeof(string));
            dt.Columns.Add("Amount", typeof(string));
            int getYear = int.Parse(txtStartMonth.Text.Substring(3,4));
          //  bool status = false;
            for (byte b = 0; b < byte.Parse(txtNoOfInstallment.Text.Trim()); b++)
            {
                dt.Rows.Add(getMonth.ToString() + "-" + getYear.ToString(), getInstallmentMonthANDPayment.ToString());
                getMonth++;
                if ((getMonth) == 13)
                {
                    getYear = getYear + 1;
                    getMonth = 1;
                    //status = true;
                }

              //  if (status) getMonth += 1;
               // else getMonth += b;
                
            }

            gvInstallmentDetails.DataSource = dt;
            gvInstallmentDetails.DataBind();
        }

        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            if (ddlEmpCardNo.SelectedItem.ToString().Trim() == "" &&  txtFindEmployeeCardNo.Text.Trim().Length<8)            
            {
                lblMessage.InnerText = "error->Please select an employee card no";
                return;
            }
            if (gvInstallmentDetails.Rows.Count == 0)
            {
                lblMessage.InnerText = "error->Please add installment month.";
                return;
            }
            
            btnFind_Click(sender, e);
            savePayroll_AdvanceInfo();
        }


        private void savePayroll_AdvanceInfo()
        {
            try
            {
                if (hasExistsAnyAdvanced()) return;

                string AdvanceId = "A" + txtFindEmployeeCardNo.Text + "_" + 12 + "_" + txtStartMonth.Text.Trim() + "_" + txtEntryDate.Text;
                string CompanyId=(ddlCompanyList.SelectedValue.ToString().Equals("0000"))?ViewState["__CompanyId__"].ToString():ddlCompanyList.SelectedValue.ToString();

                try
                {                                   
                    string[] getColumns = { "AdvanceId", "EmpId", "EmpCardNo", "EmpTypeId", "EntryDate", "AdvanceAmount", "InstallmentNo","InstallmentAmount","StartMonth", "PaidStatus", "Notes",
                                          "PaidInstallmentNo","CompanyId"};
                    string[] getValues = {AdvanceId,ViewState["__EmpId__"].ToString(),txtFindEmployeeCardNo.Text,ViewState["__EmpTypeId__"].ToString(), convertDateTime.getCertainCulture(txtEntryDate.Text.Trim()).ToString(),
                                         txtAdvanceAmount.Text.Trim(),txtNoOfInstallment.Text.Trim(),gvInstallmentDetails.Rows[0].Cells[1].Text.Trim(),convertDateTime.getCertainCulture("01-"+txtStartMonth.Text.Trim()).ToString(),"0",txtNotes.Text.Trim(),"0",
                                         CompanyId};
                    if (SQLOperation.forSaveValue("Payroll_AdvanceInfo", getColumns, getValues,sqlDB.connection) == true)
                    {
                        //savePayroll_AdvanceDetails(AdvanceId);
                        lblMessage.InnerText = "success->Successfully Advanced Saved";
                        clearInputBox(); loadExistsAdvance();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.InnerText = "error->"+ex.Message;
                }

            }
            catch (Exception ex)
            { }
        }


        private bool hasExistsAnyAdvanced()
        {
            try
            {
                sqlDB.fillDataTable("select * from Payroll_AdvanceInfo where EmpId=" + ViewState["__EmpId__"].ToString() + " And PaidStatus='0' ", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    lblMessage.InnerText = "error->This " + " " + " has previous advanced due.";
                    return true;
                }
                else return false;
            }
            catch { return true; }
        }
        private void savePayroll_AdvanceDetails(string advanceId)
        {
            try
            {
                for(byte b=0;b<gvInstallmentDetails.Rows.Count;b++) 
                {
                string[] getColumns = { "AdvanceId", "InstallmentMonth", "InstallmentAmount", "PaidStatus"};
                string[] getValues = { advanceId, gvInstallmentDetails.Rows[b].Cells[0].Text.Trim(), gvInstallmentDetails.Rows[b].Cells[1].Text.Trim(),"0"};
                SQLOperation.forSaveValue("Payroll_AdvanceDetails", getColumns, getValues,sqlDB.connection);
                }
            }
            catch (Exception ex)
            {
              
            }
        }

        private void clearInputBox()
        {
            try
            {
                
                txtEntryDate.Text = "";

                txtAdvanceAmount.Text = "";
                txtNoOfInstallment.Text = "";
                txtStartMonth.Text = "";
                txtNotes.Text = "";
                txtFindEmployeeCardNo.Text = "";
                setDefaultColulmns();
                ViewState["AdvanceId"] = "0";
                ViewState["__EmpId__"] = "0";
            }
            catch { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            clearInputBox();
        }


        private void loadExistsAdvance()
        {
            try
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                sqlDB.fillDataTable("select AdvanceId,EmpName,EmpCardNo,AdvanceAmount,InstallmentNo,InstallmentAmount,Format(StartMonth,'MM-yyyy') as StartMonth ,convert(varchar(11),EntryDate,105) as EntryDate ,EmpType from v_Payroll_AdvanceInfo where CompanyId='" + CompanyId + "' AND PaidStatus='false'", dt = new DataTable());
                gvAdvanceInfo.DataSource = dt;
                gvAdvanceInfo.DataBind();
            }
            catch { }
        }

        protected void gvAdvanceInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                loadExistsAdvance();
                gvAdvanceInfo.PageIndex= e.NewPageIndex;
                gvAdvanceInfo.DataBind();

            }
            catch (Exception ex)
            { 
            
            }
        }

        protected void gvAdvanceInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);

                lblMessage.InnerText = "";
                int index=int.Parse(e.CommandArgument.ToString());
                ViewState["AdvanceId"] = gvAdvanceInfo.DataKeys[index].Value.ToString();
            }
            catch { }
            //Delete(e.CommandArgument.ToString());// btnRefresh_Click(sender, e);
        }
        
        private void Delete(string AdvanceId)
        {
            try
            {
                sqlDB.fillDataTable("select * from Payroll_AdvanceSetting where AdvanceId='"+AdvanceId+"'",dt=new DataTable ());
                if (dt.Rows.Count > 0)
                {
                    lblMessage.InnerText = "warning->Sorry,This advanced info is not deleted.Causes " + dt.Rows.Count + " installment is Payed or set";
                    return;
                }
                else
                {
                    SQLOperation.forDeleteRecordByIdentifier("Payroll_AdvanceInfo", "AdvanceId", AdvanceId, sqlDB.connection);
                    
                    loadExistsAdvance();
                    lblMessage.InnerText = "success->Advanced info record is successfully deleted";
                   
                }
                ViewState["AdvanceId"] = "0";
            }
            catch { }
        }

       

        protected void ddlEmpCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            string[] getEmpCard = ddlEmpCardNo.SelectedItem.Text.Split(' ');
            txtFindEmployeeCardNo.Text = getEmpCard[0];
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            lblMessage.InnerText = "";
            if (txtFindEmployeeCardNo.Text.Trim().Length > 8 && txtFindEmployeeCardNo.Text.Trim().Length < 8)
            {
                lblMessage.InnerText = "warning->Please type a valid card number";
                return;
            }
            else
            {
                sqlDB.fillDataTable("Select Max(SN) as SN, EmpName,EmpId,EmpStatus,EmpStatusName,EmpTypeId,DptName From v_Personnel_EmpCurrentStatus where CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' AND EmpCardNo='" + txtFindEmployeeCardNo.Text.Trim() + "' Group by EmpCardNo,EmpId,EmpName,EmpStatus,EmpStatusName,EmpTypeId,DptName ", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["EmpStatus"].ToString().Equals("1") || dt.Rows[0]["EmpStatus"].ToString().Equals("8"))
                    {
                        ViewState["__EmpId__"] = dt.Rows[0]["EmpId"].ToString();
                        ViewState["__EmpTypeId__"] = dt.Rows[0]["EmpTypeId"].ToString();

                        lblMessage.InnerText = "warning->Employee name is " + dt.Rows[0]["EmpName"].ToString()+" | Department : "+dt.Rows[0]["DptName"].ToString();
                    }
                    else lblMessage.InnerText = "warning->" + dt.Rows[0]["EmpName"].ToString() + " is " + dt.Rows[0]["EmpStatusName"].ToString(); 
                }
                else lblMessage.InnerText = "Sorry,This card number is not valid";
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            lblMessage.InnerText = "";
            if (!ViewState["AdvanceId"].ToString().Equals("0"))
            {
                Delete(ViewState["AdvanceId"].ToString());

                loadExistsAdvance();
            }
            else
            {
                lblMessage.InnerText = "error->Please select a record";
            }
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            try
            {
                lblMessage.InnerText = "";

                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                classes.commonTask.LoadShift(ddlShiftList,CompanyId);
                classes.Employee.LoadEmpCardNoForPayroll(ddlEmpCardNo, CompanyId);

                loadExistsAdvance();
            }
            catch { }
        }

        protected void ddlShiftList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();         
            classes.Employee.LoadEmpCardNoForPayroll(ddlEmpCardNo, CompanyId);
            loadExistsAdvance();
        }

        protected void gvAdvanceInfo_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void btnComplain_Click(object sender, EventArgs e)
        {
            Session["__ModuleType__"] = "Payroll";
            Response.Redirect("/mail/complain.aspx");
        }
    }
}