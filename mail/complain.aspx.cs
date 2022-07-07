using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.mail
{
    public partial class complain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                try
                {
                    
                    setPrivilege();
                    if(Session["__forCompose__"].Equals("No"))
                    {
                        ddlModuleType.SelectedValue = Session["__ModuleType__"].ToString();
                        loadSpecificEmployeeOfDepartment();
                    }
                    setNeededControl();
                }
                catch { }
            }
        }

        void setNeededControl()
        {
            try
            {
                if (Session["__forCompose__"].ToString().Equals("Yes"))
                {
                    trCompany.Visible = false;
                    ddlTo.Visible = false;
                    txtTo.Visible = true;
                    h2Title.InnerText = "Compose Mail";
                }

            }
            catch { }
        }

        static DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__getUserId__"] = getUserId.ToString();
                //------------load privilege setting inof from db------
                dtSetPrivilege = new DataTable();
                sqlDB.fillDataTable("select * from UserPrivilege where PageName='aplication.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);
                //-----------------------------------------------------

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                {
                    

                    return;
                }

                else  // for admin and view
                {

                    // trCompanyName.Visible = false;
                   

                    if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                    {
                        
                    }

                    else  // for Viewer
                    {
                        
                    }



                    

                }

            }
            catch { }

        }

        private void loadSpecificEmployeeOfDepartment()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select dptId from v_Personnel_EmpCurrentStatus where EmpId =(select EmpId from UserAccount where UserId=" + ViewState["__getUserId__"].ToString()+ ")",dt);
                sqlDB.fillDataTable(" select FirstName+' '+LastName as FullName,UserId from UserAccount where EmpId in(select EmpId from  v_Personnel_EmpCurrentStatus where dptId='" + dt.Rows[0]["dptId"].ToString() + "') and Status='1' AND UserId !=" + ViewState["__getUserId__"].ToString()+ "", dt = new DataTable());
                ddlTo.DataSource = dt;
                ddlTo.DataTextField = "FullName";
                ddlTo.DataValueField = "UserId";      
                ddlTo.DataBind();

            }
            catch { }
        }

        private void saveComplain()
        {
            try
            {
                DataTable dt = new DataTable();

                sqlDB.fillDataTable("select EmpId from UserAccount where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' AND UserId =" + ViewState["__getUserId__"] + "",dt=new DataTable ());
                string getTime = DateTime.Now.ToLongTimeString();
                string[] getColumns = { "EmpId", "ModuleType", "Subject", "Details", "CDate", "CTime", "CompanyId", "IsRead", "RxUserId" };
                string[] getValues = {dt.Rows[0]["EmpId"].ToString(),ddlModuleType.SelectedValue.ToString(),txtSubject.Text.Trim(),txtBody.Text.Trim(),
                convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")).ToString(),getTime,ViewState["__CompanyId__"].ToString(),"0",ddlTo.SelectedItem.Value.ToString()};

                if (SQLOperation.forSaveValue("Mail_Complain_Info", getColumns, getValues,sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully Submited";
                    InputBoxClear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "";
            }
        }

        void saveComposeMail()
        {
            try
            {
                string[] getCompanyId;   // to set needed user id and companyid
                DataTable dtCompanyInfo=new DataTable ();
                if (txtTo.Text.Trim().Length < 5)
                {
                    lblMessage.InnerText = "error->Please select valid user information."; return;
                }
                else
                {
                    getCompanyId = txtTo.Text.ToString().Split(']');
                    getCompanyId=getCompanyId[1].Split(' ');

                    sqlDB.fillDataTable("select CompanyId from UserAccount where CompanyId='"+getCompanyId[1]+"'",dtCompanyInfo);
                    if (dtCompanyInfo.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "error->Please select valid user information.";
                        return;
                    }
                }
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select EmpId from UserAccount where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' AND UserId =" + ViewState["__getUserId__"] + "", dt = new DataTable());
             
                string getTime = DateTime.Now.ToLongTimeString();
                string[] getColumns = { "EmpId", "Subject", "Details", "CDate", "CTime", "TxCompanyId", "IsRead", "RxCompanyId", "RxUserId" };
                string[] getValues = {dt.Rows[0]["EmpId"].ToString(),txtSubject.Text.Trim(),txtBody.Text.Trim(),
                                     convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")).ToString(),getTime,
                                     ViewState["__CompanyId__"].ToString(),"0",dtCompanyInfo.Rows[0]["CompanyId"].ToString(),getCompanyId[2]};
                if (SQLOperation.forSaveValue("Mail_ComposeMail_Info", getColumns, getValues,sqlDB.connection) == true)
                {
                    txtTo.Text = "";
                    lblMessage.InnerText = "success->Successfully Submited";
                }
            }
            catch (Exception ex)
            {
               
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (inputValidation())
            {
                if (Session["__forCompose__"].ToString().Equals("Yes"))
                {
                    saveComposeMail();
                }

                else saveComplain();
            }
        }

        private bool inputValidation()
        {
            try
            {
                if (txtSubject.Text.Trim().Length < 10)
                {
                    lblMessage.InnerText = "warning-> Minimum of 10 characters required for subject ";
                    txtSubject.Focus();
                    return false;
                }
                if (txtBody.Text.Trim().Length < 20)
                {
                    lblMessage.InnerText = "warning-> Minimum of 20 characters required for details ";
                    txtBody.Focus();
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        private void InputBoxClear()
        {
            try
            {
                txtTo.Text = "";
                txtSubject.Text = "";
                txtBody.Text = "";

            }
            catch { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            InputBoxClear();

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Session["__forCompose__"] = "No";
                Response.Redirect(Session["__PreviousPage__"].ToString());
            }
            catch { }
        }
    }
}