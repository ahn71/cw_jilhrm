using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class promotion_sheet : System.Web.UI.Page
    {
        DataTable dt;
        DataTable dtSetPrivilege;
        string CompanyId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {               
                setPrivilege();
                classes.commonTask.loadEmpTye(rbEmpList);
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
           

        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();


                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "promotion_sheet.aspx", ddlCompany, WarningMessage, tblGenerateType, btnpreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                classes.commonTask.LoadMonthForPromotion(ddlMonthName, ViewState["__CompanyId__"].ToString());
                //-----------------------------------------------------
       

              
               
            }
            catch { }

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

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstAll, lstSelected);
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

        protected void btnpreview_Click(object sender, EventArgs e)
        {
            try
            {
                //-------------------------Validation----------------------
                if(rbEmpList.SelectedValue=="")
                {
                    lblMessage.InnerText = "warning->Please Select Employee Type!";
                     return;
                }
                if (ddlMonthName.SelectedItem.Text.Trim() == "")
                {
                    lblMessage.InnerText = "warning->Please Select Any Month!";
                    ddlMonthName.Focus(); return;
                }
                //if(lstSelected.Items.Count==0)
                //{
                //    if (lstAll.Items.Count == 0) 
                //    {
                //        lblMessage.InnerText = "warning->Any Employees Are Not Separated In This Month";
                //        return;
                //    }
                //    lblMessage.InnerText = "warning->Please Select Any Department!";
                //    lstSelected.Focus();
                //    return;
                //}
             //............................End.................................
                
                 HttpCookie getCookies = Request.Cookies["userInfo"];
                 CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;           
                DataTable dtRunning = new DataTable();
                sqlDB.fillDataTable("Select EmpName,PreGrdName,PreDsgName,PreDptName,GrdName,DsgName,DptName,SubString(EmpCardNo,8,15) as EmpCardNo,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,PreEmpSalary,PreIncrementAmount,EffectiveMonth,IncrementAmount,EmpPresentSalary,CompanyName,SftName,Address From v_Promotion_Increment  where TypeOfChange='p' and EffectiveMonth='" + ddlMonthName.SelectedValue + "' and EmpTypeId=" + rbEmpList.SelectedValue + " and CompanyId='" + CompanyId + "' order by SN", dtRunning);                                                                   
                    Session["__PromotionSheet__"] = dtRunning;
                    if (dtRunning.Rows.Count > 0)
                    {
                        string MonthName = "";
                        string[] getmonth = ddlMonthName.SelectedItem.Text.Split('-');
                        MonthName = getmonth[0].ToUpper() +" "+ getmonth[1];
                        //if (getmonth[0].Equals("01"))
                        //{
                        //    MonthName = "JAN " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("02"))
                        //{
                        //    MonthName = "FEB " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("03"))
                        //{
                        //    MonthName = "MAR " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("04"))
                        //{
                        //    MonthName = "APR " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("05"))
                        //{
                        //    MonthName = "MAY " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("06"))
                        //{
                        //    MonthName = "JUN " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("07"))
                        //{
                        //    MonthName = "JUL " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("08"))
                        //{
                        //    MonthName = "AUG " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("09"))
                        //{
                        //    MonthName = "SEP " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("10"))
                        //{
                        //    MonthName = "OCT " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("11"))
                        //{
                        //    MonthName = "NOV " + getmonth[1].Substring(2, 2);
                        //}
                        //else if (getmonth[0].Equals("12"))
                        //{
                        //    MonthName = "DEC " + getmonth[1].Substring(2, 2);
                        //}
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=PromotionSheet-" + MonthName + "');", true);  //Open New Tab for Sever side code
                    }
                    else
                    {
                        lblMessage.InnerText = "warning->No data Available";
                    }               
                

            }
            catch { }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            classes.commonTask.LoadMonthForPromotion(ddlMonthName, CompanyId);
            
        }
    }
}