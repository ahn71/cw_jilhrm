using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class festival_bonus : System.Web.UI.Page
    {

        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpTypeList,"hasMnu");
                classes.commonTask.loadEmpTypeInRadioButtonList(rblSelectEmployeeType);
                classes.commonTask.loadDivision(ddlDivisionName);
                loadMonthInf();
            }
        }


        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='festival_bonus.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnPreview.CssClass = "";
                            btnPreview.Enabled = false;
                        }
                    }
                    btnBankBonusList.Visible = false;
                }
            }
            catch { }
        }

        private void loadMonthInf()
        {
            try
            {

                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select distinct BonusType,Year from v_Payroll_YearlyBonusSheet order by year desc ", dt);
                ddlMonthID.DataTextField = "BonusType";
                ddlMonthID.DataValueField = "BonusType";
                ddlMonthID.DataSource = dt;
                ddlMonthID.DataBind();
                ddlMonthID.Items.Insert(0, new ListItem(" ", "0"));
            }
            catch { }

        }



        protected void rbEmpTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!rbEmpTypeList.SelectedValue.ToString().Equals("50"))
            {
                ddlCardNo.Enabled = false;
                rblSelectEmployeeType.Visible = false;
            }
            else
            {
                ddlCardNo.Enabled = true;
                rblSelectEmployeeType.Visible = true;
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            ViewState["__BBL__"] = "";
            if (rbEmpTypeList.SelectedValue.ToString().Equals("50")) { }
            else WorkerSalarySheet();
        }

        private void WorkerSalarySheet()
        {
            try
            {
                string setPredicate = "";
                //   string[] getMonth = ddlMonthID.SelectedValue.Split('-');
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

                if (ViewState["__BBL__"].ToString() == "From Bank ")
                {
                    if (!ddlCardNo.Enabled)
                        sqlDB.fillDataTable("Select EmpCardNo,BasicSalary,BonusAmount,Percentage,EmpName,DptName,LnCode,PresentSalary,DptId,LnId,Convert(varchar(11),generateDate,105) as generateDate,BonusType,EmpType,DsgName,Convert(varchar(11),EmpJoiningDate,110) as EmpJoiningDate,SalaryCount from v_Payroll_YearlyBonusSheet group by DsgName,EmpCardNo,BasicSalary,BonusAmount,Percentage,EmpName,DptName,LnCode,PresentSalary,DptId,LnId,generateDate,BonusType,EmpType,EmpJoiningDate,SalaryCount having bonustype='" + ddlMonthID.SelectedItem.ToString() + "' AND EmpType='" + rbEmpTypeList.SelectedItem.ToString() + "' AND SalaryCount in ('Bank') AND DptId "+setPredicate+" order by dptId,LnId,EmpCardNo", dt = new DataTable());
                    else sqlDB.fillDataTable("Select EmpCardNo,BasicSalary,BonusAmount,Percentage,EmpName,DptName,LnCode,PresentSalary,DptId,LnId,Convert(varchar(11),generateDate,105) as generateDate,BonusType,EmpType,DsgName,Convert(varchar(11),EmpJoiningDate,110) as EmpJoiningDate,SalaryCount from v_Payroll_YearlyBonusSheet group by DsgName,EmpCardNo,BasicSalary,BonusAmount,Percentage,EmpName,DptName,LnCode,PresentSalary,DptId,LnId,generateDate,BonusType,EmpType,EmpJoiningDate,SalaryCount having bonustype='" + ddlMonthID.SelectedItem.ToString() + "' AND EmpType='" + rbEmpTypeList.SelectedItem.ToString() + "' AND SalaryCount in ('Bank') AND EmpCardNo='"+ddlCardNo.SelectedValue.ToString()+"' AND DptId "+setPredicate+" order by dptId,LnId,EmpCardNo", dt = new DataTable());
                
                }
                else
                {
                    if (!ddlCardNo.Enabled)
                        sqlDB.fillDataTable("Select EmpCardNo,BasicSalary,BonusAmount,Percentage,EmpName,DptName,LnCode,PresentSalary,DptId,LnId,Convert(varchar(11),generateDate,105) as generateDate,BonusType,EmpType,DsgName,Convert(varchar(11),EmpJoiningDate,110) as EmpJoiningDate,SalaryCount from v_Payroll_YearlyBonusSheet group by DsgName,EmpCardNo,BasicSalary,BonusAmount,Percentage,EmpName,DptName,LnCode,PresentSalary,DptId,LnId,generateDate,BonusType,EmpType,EmpJoiningDate,SalaryCount having bonustype='" + ddlMonthID.SelectedItem.ToString() + "' AND EmpType='" + rbEmpTypeList.SelectedItem.ToString() + "' AND SalaryCount not in ('Bank') AND DptId " + setPredicate + " order by dptId,LnId,EmpCardNo", dt = new DataTable());
                    else sqlDB.fillDataTable("Select EmpCardNo,BasicSalary,BonusAmount,Percentage,EmpName,DptName,LnCode,PresentSalary,DptId,LnId,Convert(varchar(11),generateDate,105) as generateDate,BonusType,EmpType,DsgName,Convert(varchar(11),EmpJoiningDate,110) as EmpJoiningDate,SalaryCount from v_Payroll_YearlyBonusSheet group by DsgName,EmpCardNo,BasicSalary,BonusAmount,Percentage,EmpName,DptName,LnCode,PresentSalary,DptId,LnId,generateDate,BonusType,EmpType,EmpJoiningDate,SalaryCount having bonustype='" + ddlMonthID.SelectedItem.ToString() + "' AND EmpType='" + rbEmpTypeList.SelectedItem.ToString() + "' AND SalaryCount not in ('Bank') AND EmpCardNo='" + ddlCardNo.SelectedValue.ToString() + "' AND DptId " + setPredicate + " order by dptId,LnId,EmpCardNo", dt = new DataTable());
                
                }
               
                Session["__WorkerBonusSheet__"] = dt;
             
                if (rbLanguage.SelectedValue.ToString().Equals("0"))
                {
                    Session["__Language__"] = "Bangal";
                }
                else if (rbLanguage.SelectedValue.ToString().Equals("1"))
                {
                    Session["__Language__"] = "English";
                }

                //string getType = new String(ddlMonthID.SelectedItem.ToString().Where(Char.IsLetter).ToArray());

                if (dt.Rows.Count>=0) 

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=WorkerBonusSheet-"+ddlMonthID.SelectedItem.ToString()+"-"+rbEmpTypeList.SelectedItem.ToString()+"');", true);  //Open New Tab for Sever side code
            }
            catch (Exception ex)
            { 
            
            }
        }


        protected void rblSelectEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.Employee.LoadEmpCardNoWithName(ddlCardNo, rblSelectEmployeeType.SelectedValue);
        }

        protected void btnBankBonusList_Click(object sender, EventArgs e)
        {
            if (rbEmpTypeList.SelectedValue.ToString() == "2")
            {
                ViewState["__BBL__"] = "From Bank ";
                WorkerSalarySheet();
            }
            else
            {
                lblMessage.InnerText = "warning->This Option for only staff";
            
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstAll, lstSelected);
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
            AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstSelected, lstAll);
        }

        

        private void LoadDepartment(string divisionId, ListBox lst)
        {
            try
            {
                dt = new DataTable();

                sqlDB.fillDataTable("SELECT DptId, DptName FROM HRD_Department where DId=" + divisionId + "", dt);

                lst.DataValueField = "DptId";
                lst.DataTextField = "DptName";
                lst.DataSource = dt;
                lst.DataBind();
            }
            catch { }
        }

        protected void ddlDivisionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartment(ddlDivisionName.SelectedValue, lstAll);
        }

       
        
    }
}