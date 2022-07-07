using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;

namespace SigmaERP.personnel
{
    public partial class man_power_status : System.Web.UI.Page
    {
        DataTable dt;
        string CompanyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyy.Enabled = false;        
            }
        }
        private void setPrivilege()
        {
            try
            {
                ViewState["__WriteAction__"] = "1";
                ViewState["__DeletAction__"] = "1";
                ViewState["__ReadAction__"] = "1";
                ViewState["__UpdateAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "man_power_status.aspx", ddlCompanyy, WarningMessage, tblGenerateType, btnpreview);
                ViewState["__ReadAction__"] = AccessPermission[0];      
            
                ddlCompanyy.Items.RemoveAt(0);
                ddlCompanyy.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.LoadInitialShift(ddlShift, ddlCompanyy.SelectedValue);
                addAllTextInShift();
                classes.commonTask.LoadDepartment(ddlCompanyy.SelectedValue, lstAll);
            }
            catch { }

        }
        private void addAllTextInShift()
        {
            ddlShift.Items.RemoveAt(0);
            if (ddlShift.Items.Count > 1)
                ddlShift.Items.Insert(0, new ListItem("All", "00"));
        }
        private void LoadDepartment(string CompnayId,ListBox lst)
        {
            try
            {
                 dt = new DataTable();

                 sqlDB.fillDataTable("SELECT DptId, DptName FROM HRD_Department where CompanyId=" + CompnayId + "", dt);

                 lst.DataValueField = "DptId";
                 lst.DataTextField = "DptName";
                 lst.DataSource = dt;
                 lst.DataBind();
            }
            catch { }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstAll,lstSelected);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }
        private void AddRemoveItem(ListBox aSource, ListBox aTarget)
        {

            ListItemCollection licCollection;

            try
            {

                licCollection = new ListItemCollection();
                for (int intCount = 0; intCount < aSource.Items.Count; intCount++)
                {
                    if (aSource.Items[intCount].Selected == true)
                        licCollection.Add(aSource.Items[intCount]);
                }

                for (int intCount = 0; intCount < licCollection.Count; intCount++)
                {
                    aSource.Items.Remove(licCollection[intCount]);
                    aTarget.Items.Add(licCollection[intCount]);
                }

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }
            finally
            {
                licCollection = null;
            }

        }

        private void AddRemoveAll(ListBox aSource, ListBox aTarget)
        {

            try
            {

                foreach (ListItem item in aSource.Items)
                {
                    aTarget.Items.Add(item);
                }
                aSource.Items.Clear();

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }

        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstAll,lstSelected);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstSelected,lstAll);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstSelected,lstAll);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }

        protected void btnpreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlShift.SelectedValue == "0") { lblMessage.InnerText = "warning-> Please select any shift !"; 
                    ddlShift.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                    return;
                }
                if (lstSelected.Items.Count < 1) {
                    lblMessage.InnerText = "warning-> Please select any Department !"; 
                    lstSelected.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                    return;
                }
                string setPredicate = "";
                for (byte b = 0; b < lstSelected.Items.Count; b++)
                {

                    if (b == 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate = "in('" + lstSelected.Items[b].Value + "')";
                    }
                    else if (b == 0 && b != lstSelected.Items.Count - 1)
                    {
                        setPredicate += "in ('" + lstSelected.Items[b].Value + "'";
                    }
                    else if (b != 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate += ",'" + lstSelected.Items[b].Value + "')";
                    }
                    else setPredicate += ",'" + lstSelected.Items[b].Value + "'";
                }
                string shiftlist=(ddlShift.SelectedValue=="00")?"":" and SftId='"+ddlShift.SelectedValue+"'";
                string EmpTypeID = rblEmpType.SelectedValue.Equals("All") ? "" : " and EmpTypeId="+rblEmpType.SelectedValue+"";
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_ManPowerStatus where  DptId " + setPredicate + " " + shiftlist + " " + EmpTypeID + " and EmpStatus in('1','8') and ActiveSalary='True' and IsActive=1 Group by EmpId", dt);
                if (dt == null || dt.Rows.Count == 0) 
                { 
                    lblMessage.InnerText = "warning-> Any records are not founded !";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                    return;
                }
                string setSn = "", setEmpId = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0 && i == dt.Rows.Count - 1)
                    {
                        setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId = "in('" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else if (i == 0 && i != dt.Rows.Count - 1)
                    {
                        setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += "in ('" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                    else if (i != 0 && i == dt.Rows.Count - 1)
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                }
                dt = new DataTable();
                sqlDB.fillDataTable("Select * from v_ManPowerStatus where SN "+setSn+"", dt);
                Session["__ManPowerStatus__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ManPowerStatus-"+rblEmpType.SelectedItem.Text+"');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpId,Sex From v_EmployeeProfile", dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand("Insert Into HRD_ManpowerStatus(EmpId,Male,Female) values(@EmpId,@Male,@Female)", sqlDB.connection);
                    cmd.Parameters.AddWithValue("@EmpId", dt.Rows[i]["EmpId"].ToString());
                    if (dt.Rows[i]["Sex"].ToString() == "Male")
                    {
                        cmd.Parameters.AddWithValue("@Male", 1);
                        cmd.Parameters.AddWithValue("@Female", 0);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Male", 0);
                        cmd.Parameters.AddWithValue("@Female", 1);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        protected void ddlCompanyy_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompanyID=(ddlCompanyy.SelectedValue.ToString().Equals("0000"))?ViewState["__CompanyId__"].ToString(): ddlCompanyy.SelectedValue;
            classes.commonTask.LoadInitialShift(ddlShift, ddlCompanyy.SelectedValue);
            addAllTextInShift();
            lstSelected.Items.Clear();
           classes.commonTask.LoadDepartment(ddlCompanyy.SelectedValue, lstAll);
           ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }
    }
}