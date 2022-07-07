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
    public partial class increment_sheet : System.Web.UI.Page
    {
        string CompanyId = "";
        DataTable dt;
        DataTable dtSetPrivilege;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.LoadEmpType(rbEmpList);
                //classes.commonTask.LoadMonthName(ddlMonthName);
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();               
            }
            lblMessage.InnerText = "";

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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "increment_sheet.aspx", ddlCompany, WarningMessage, tblGenerateType, btnpreview,btnPreviewDetails);
                ViewState["__ReadAction__"] = AccessPermission[0];

                classes.commonTask.LoadMonthForIncreament(ddlMonthName, ViewState["__CompanyId__"].ToString());
                classes.Employee.LoadEmpCardIncPro(ddlCardNo, "i", ViewState["__CompanyId__"].ToString());
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
                CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;   
              //  HttpCookie getCookies = Request.Cookies["userInfo"];
                //if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                //{
                //    CompanyID = ddlCompany.SelectedValue;
                //}
                //else
                //{
                //    CompanyID = ViewState["__CompanyId__"].ToString();
                //}
                if (rbEmpList.SelectedValue == "00")
                {
                    DataTable dtRunning = new DataTable();
                    if (ddlCardNo.SelectedValue == "0")
                    {
                        sqlDB.fillDataTable("Select EmpName,GrdName,DsgName,DptName,SftName,SubString(EmpCardNo,8,15) as EmpCardNo,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,PreEmpSalary,PreIncrementAmount,Format(Convert(datetime,'01-'+EffectiveMonth,105),'MMM-yyyy') as EffectiveMonth,IncrementAmount,EmpPresentSalary,CompanyName,Remarks,Address,OrderRefNo From v_Promotion_Increment  where TypeOfChange='i' and CompanyId='" + CompanyId + "' order by SN", dtRunning);
                    }
                    else
                    {
                        sqlDB.fillDataTable("Select EmpName,GrdName,DsgName,DptName,SftName,SubString(EmpCardNo,8,15) as EmpCardNo,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,PreEmpSalary,PreIncrementAmount,Format(Convert(datetime,'01-'+EffectiveMonth,105),'MMM-yyyy') as EffectiveMonth,IncrementAmount,EmpPresentSalary,CompanyName,Remarks,Address,OrderRefNo From v_Promotion_Increment  where TypeOfChange='i' and CompanyId='" + CompanyId + "' and EmpId='"+ddlCardNo.SelectedValue+"' order by SN", dtRunning);
                    }
                    Session["__IndivisualIncrementSheet__"] = dtRunning;
                    if (dtRunning.Rows.Count > 0)
                    {                       
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IndivisualIncrementSheet');", true);  //Open New Tab for Sever side code
                    }
                    else
                    {
                        lblMessage.InnerText = "warning->No data Available";
                    }
                }
                else
                { 
                    DataTable dtRunning = new DataTable();
                    if (rbEmpList.SelectedValue == "0")
                    {
                        sqlDB.fillDataTable("Select EmpName,GrdName,DsgName,DptName,SftName,SubString(EmpCardNo,8,15) as EmpCardNo,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,PreEmpSalary,PreIncrementAmount,EffectiveMonth,IncrementAmount,EmpPresentSalary,CompanyName,Remarks,Address From v_Promotion_Increment  where TypeOfChange='i' and EffectiveMonth='" + ddlMonthName.SelectedValue + "' and CompanyId='" + CompanyId + "' order by SN", dtRunning);
                    }
                    else
                    {
                        sqlDB.fillDataTable("Select EmpName,GrdName,DsgName,DptName,SftName,SubString(EmpCardNo,8,15) as EmpCardNo,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,PreEmpSalary,PreIncrementAmount,EffectiveMonth,IncrementAmount,EmpPresentSalary,CompanyName,Remarks,Address From v_Promotion_Increment  where TypeOfChange='i' and EffectiveMonth='" + ddlMonthName.SelectedValue + "' and EmpTypeId=" + rbEmpList.SelectedValue + " and CompanyId='" + CompanyId + "' order by SN", dtRunning);
                    }
                    Session["__IncrementSheet__"] = dtRunning;
                    if (dtRunning.Rows.Count > 0)
                    {
                        string MonthName = "";
                        string[] getmonth = ddlMonthName.SelectedItem.Text.Split('-');
                        MonthName = getmonth[0].ToUpper() + " " + getmonth[1];                   
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IncrementSheet-" + MonthName + "');", true);  //Open New Tab for Sever side code
                    }
                    else
                    {
                        lblMessage.InnerText = "warning->No data Available";
                    }
                }
            }
            catch { }
        }       

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            classes.commonTask.LoadMonthForIncreament(ddlMonthName, CompanyId);
            classes.Employee.LoadEmpCardIncPro(ddlCardNo, "i", CompanyId);
        }

        protected void btnPreviewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
                //  HttpCookie getCookies = Request.Cookies["userInfo"];
                //if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                //{
                //    CompanyID = ddlCompany.SelectedValue;
                //}
                //else
                //{
                //    CompanyID = ViewState["__CompanyId__"].ToString();
                //}  
              if (rbEmpList.SelectedValue == "00")
              {
                  DataTable dtRunning = new DataTable();
                  if (ddlCardNo.SelectedValue != "0")
                  {
                      sqlDB.fillDataTable("Select EmpName,GrdName,DsgName,DptName,SftName,SubString(EmpCardNo,8,15) as EmpCardNo,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,PreEmpSalary,PreIncrementAmount,Format(Convert(datetime,'01-'+EffectiveMonth,105),'MMM-yyyy') as EffectiveMonth,IncrementAmount,EmpPresentSalary,CompanyName,Remarks,Address,PreBasicSalary,BasicSalary,PreOthersAllownce,OthersAllownce,PreMedicalAllownce,MedicalAllownce,PreFoodAllownce,FoodAllownce,PreHouseRent,HouseRent,OrderRefNo From v_Promotion_Increment  where TypeOfChange='i' and EmpId='"+ddlCardNo.SelectedValue+"' and CompanyId='" + CompanyId + "' order by SN", dtRunning);
                  }
                  else
                  {
                      sqlDB.fillDataTable("Select EmpName,GrdName,DsgName,DptName,SftName,SubString(EmpCardNo,8,15) as EmpCardNo,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,PreEmpSalary,PreIncrementAmount,Format(Convert(datetime,'01-'+EffectiveMonth,105),'MMM-yyyy') as EffectiveMonth,IncrementAmount,EmpPresentSalary,CompanyName,Remarks,Address,PreBasicSalary,BasicSalary,PreOthersAllownce,OthersAllownce,PreMedicalAllownce,MedicalAllownce,PreFoodAllownce,FoodAllownce,PreHouseRent,HouseRent,OrderRefNo From v_Promotion_Increment  where TypeOfChange='i' and CompanyId='" + CompanyId + "' order by SN", dtRunning);
                  }
                  Session["__IndIncrementSheetDetails__"] = dtRunning;
                  if (dtRunning.Rows.Count > 0)
                  {                      
                      ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IndIncrementSheetDetails');", true);  //Open New Tab for Sever side code
                  }
                  else
                  {
                      lblMessage.InnerText = "warning->No data Available";
                  }
              }
              else
              {
                  DataTable dtRunning = new DataTable();
                  if (rbEmpList.SelectedValue == "0")
                  {
                      sqlDB.fillDataTable("Select EmpName,GrdName,DsgName,DptName,SftName,SubString(EmpCardNo,8,15) as EmpCardNo,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,PreEmpSalary,PreIncrementAmount,EffectiveMonth,IncrementAmount,EmpPresentSalary,CompanyName,Remarks,Address,PreBasicSalary,BasicSalary,PreOthersAllownce,OthersAllownce,PreMedicalAllownce,MedicalAllownce,PreFoodAllownce,FoodAllownce,PreHouseRent,HouseRent From v_Promotion_Increment  where TypeOfChange='i' and EffectiveMonth='" + ddlMonthName.SelectedValue + "'  and CompanyId='" + CompanyId + "' order by SN", dtRunning);
                  }
                  else
                  {
                      sqlDB.fillDataTable("Select EmpName,GrdName,DsgName,DptName,SftName,SubString(EmpCardNo,8,15) as EmpCardNo,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,PreEmpSalary,PreIncrementAmount,EffectiveMonth,IncrementAmount,EmpPresentSalary,CompanyName,Remarks,Address,PreBasicSalary,BasicSalary,PreOthersAllownce,OthersAllownce,PreMedicalAllownce,MedicalAllownce,PreFoodAllownce,FoodAllownce,PreHouseRent,HouseRent From v_Promotion_Increment  where TypeOfChange='i' and EffectiveMonth='" + ddlMonthName.SelectedValue + "' and EmpTypeId=" + rbEmpList.SelectedValue + " and CompanyId='" + CompanyId + "' order by SN", dtRunning);
                  }
                  Session["__IncrementSheetDetails__"] = dtRunning;
                  if (dtRunning.Rows.Count > 0)
                  {
                      string MonthName = "";
                      string[] getmonth = ddlMonthName.SelectedItem.Text.Split('-');
                      MonthName = getmonth[0].ToUpper() + " " + getmonth[1];
                      ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IncrementSheetDetails-" + MonthName + "');", true);  //Open New Tab for Sever side code
                  }
                  else
                  {
                      lblMessage.InnerText = "warning->No data Available";
                  }
              }



            }
            catch { }
        }

        protected void rbEmpList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbEmpList.SelectedValue == "00")
                {
                    trCard.Visible = true;
                    trMonth.Visible = false;
                }
                else
                {
                    trCard.Visible = false;
                    trMonth.Visible = true;
                }
            }
            catch { }
        }
    }
}