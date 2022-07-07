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
    public partial class seperation_sheet : System.Web.UI.Page
    {
        DataTable dt;
        string CompanyId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.LoadEmpTypeWithAll(rbEmpList);
               // classes.commonTask.loadEmpTye(rbEmpList);
                //classes.commonTask.LoadMonthName(ddlMonthName);
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
                
                
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
                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "seperation_sheet.aspx", ddlCompany, WarningMessage, tblGenerateType, btnpreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                   
                    
                    ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
                    classes.commonTask.LoadInitialShift(ddlShiftList, ddlCompany.SelectedValue);
                    addAllTextInShift();
              classes.commonTask.LoadMonthForSeperation(ddlMonthName, ViewState["__CompanyId__"].ToString());

            }
            catch { }

        }
        private void addAllTextInShift()
        {
            ddlShiftList.Items.RemoveAt(0);
            if (ddlShiftList.Items.Count > 1)
                ddlShiftList.Items.Insert(0, new ListItem("All", "00"));
        }
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstAll, lstSelected);
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
            AddRemoveAll(lstAll, lstSelected);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstSelected, lstAll);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstSelected, lstAll);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }

        protected void btnpreview_Click(object sender, EventArgs e)
        {
            try
            {//-------------------------Validation----------------------
               
           
                if (ddlMonthName.SelectedItem.Text.Trim() == "")
                {
                    lblMessage.InnerText = "warning->Please Select Any Month!";
                    ddlMonthName.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                    return;
                }
                if(lstSelected.Items.Count==0)
                {
                    if (lstAll.Items.Count == 0) 
                    {
                        string EmpType = (rbEmpList.SelectedValue == "All") ? "" : " (" + rbEmpList.SelectedItem.Text + ") ";
                        lblMessage.InnerText = "warning->Any Employees"+EmpType+"Are Not Separated In This Month";
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                        return;
                    }
                    lblMessage.InnerText = "warning->Please Select Any Department!";
                    lstSelected.Focus();
                    return;
                }
             //............................End.................................
                string setPredicate = "", MonthName = "";
                string[] getmonth = ddlMonthName.SelectedValue.Split('-');
                string dataMonthyear = getmonth[1] + "-" + getmonth[0];
                if (getmonth[0].Equals("01"))
                {
                    MonthName = "January " + getmonth[1];
                }
                else if (getmonth[0].Equals("02"))
                {
                    MonthName = "February " + getmonth[1];
                }
                else if (getmonth[0].Equals("03"))
                {
                    MonthName = "March " + getmonth[1];
                }
                else if (getmonth[0].Equals("04"))
                {
                    MonthName = "April " + getmonth[1];
                }
                else if (getmonth[0].Equals("05"))
                {
                    MonthName = "May " + getmonth[1];
                }
                else if (getmonth[0].Equals("06"))
                {
                    MonthName = "June " + getmonth[1];
                }
                else if (getmonth[0].Equals("07"))
                {
                    MonthName = "July " + getmonth[1];
                }
                else if (getmonth[0].Equals("08"))
                {
                    MonthName = "August " + getmonth[1];
                }
                else if (getmonth[0].Equals("09"))
                {
                    MonthName = "September " + getmonth[1];
                }
                else if (getmonth[0].Equals("10"))
                {
                    MonthName = "October " + getmonth[1];
                }
                else if (getmonth[0].Equals("11"))
                {
                    MonthName = "November " + getmonth[1];
                }
                else if (getmonth[0].Equals("12"))
                {
                    MonthName = "December " + getmonth[1];
                }
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

                //string CompanyId="";
                //if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                //{
                //    CompanyId = ddlCompany.SelectedValue;
                //}
                CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;  
                dt = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,SftName,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo ,GrdName,DptName,DsgName,Format(EffectiveDate,'dd-MM-yyyy') as EffectiveDate,EmpStatusName,Remarks,Address From v_SeparationSheet where CompanyId='" + CompanyId + "' and EFMonth='" + ddlMonthName.SelectedValue + "' and DptId " + setPredicate + " and IsActive=1 order by EffectiveDate", dt = new DataTable());
                Session["__SeparationSheet__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=SeparationSheet-" + MonthName + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

        protected void btnDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string setPredicate = "", MonthName = "";
                string[] getmonth = ddlMonthName.SelectedItem.Text.Split('-');
                string dataMonthyear = getmonth[1] + "-" + getmonth[0];
                if (getmonth[0].Equals("01"))
                {
                    MonthName = "January" + getmonth[1];
                }
                else if (getmonth[0].Equals("02"))
                {
                    MonthName = "February" + getmonth[1];
                }
                else if (getmonth[0].Equals("03"))
                {
                    MonthName = "March" + getmonth[1];
                }
                else if (getmonth[0].Equals("04"))
                {
                    MonthName = "April" + getmonth[1];
                }
                else if (getmonth[0].Equals("05"))
                {
                    MonthName = "May" + getmonth[1];
                }
                else if (getmonth[0].Equals("06"))
                {
                    MonthName = "June" + getmonth[1];
                }
                else if (getmonth[0].Equals("07"))
                {
                    MonthName = "July" + getmonth[1];
                }
                else if (getmonth[0].Equals("08"))
                {
                    MonthName = "August" + getmonth[1];
                }
                else if (getmonth[0].Equals("09"))
                {
                    MonthName = "September" + getmonth[1];
                }
                else if (getmonth[0].Equals("10"))
                {
                    MonthName = "October" + getmonth[1];
                }
                else if (getmonth[0].Equals("11"))
                {
                    MonthName = "November" + getmonth[1];
                }
                else if (getmonth[0].Equals("12"))
                {
                    MonthName = "December" + getmonth[1];
                }
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
               
               
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_SeparationSheet where EmpTypeId=1 and DptId " + setPredicate + " and EffectiveDateMonth='" + dataMonthyear + "'and ActiveSalary='True' and IsActive=1 Group by EmpId", dt);
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
                    
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select  Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,convert(varchar(11),DateOfBirth,106)as DateOfBirth,convert(varchar(11),EmpJoiningDate,106)as EmpJoiningDate,PresentAd,PermanentAd From v_EmployeeProfile where SN " + setSn + " order by EmpCardNo", dt = new DataTable());
                }
                Session["__SeparationSheetDetails__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=SeparationSheetDetails-" + MonthName + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

        protected void ddlMonthName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            if (rbEmpList.SelectedValue == "") 
            {
                lblMessage.InnerText = "warning-> Please Select Employee Type!"; return;
            }
            
            lstSelected.Items.Clear();
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            //if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
            //{
            //    CompanyId = ddlCompany.SelectedValue;
            //}
            //else CompanyId = ViewState["__CompanyId__"].ToString();
            classes.commonTask.LoadDepartmentByCompanyInListBoxType(lstAll,rbEmpList.SelectedValue,CompanyId,ddlMonthName.SelectedValue);
            if (lstAll.Items.Count == 0) 
            {
                string Emptype = (rbEmpList.SelectedItem.Text == "All") ? "" : "( " + rbEmpList.SelectedItem.Text + " )";
                lblMessage.InnerText = "warning->Any  Employee " + Emptype + " Are Not Separated In This Month";
                return;
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            lstAll.Items.Clear();
            lstSelected.Items.Clear();
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            classes.commonTask.LoadInitialShift(ddlShiftList, ddlCompany.SelectedValue);
            addAllTextInShift();
            classes.commonTask.LoadMonthForSeperation(ddlMonthName, CompanyId);
        }

        protected void rbEmpList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            if (ddlMonthName.SelectedItem.Text.Trim() != "")
            {
                lstSelected.Items.Clear();
                CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;             
                classes.commonTask.LoadDepartmentByCompanyInListBoxType(lstAll, rbEmpList.SelectedValue, CompanyId, ddlMonthName.SelectedValue);
                if (lstAll.Items.Count == 0)
                {
                   string Emptype= (rbEmpList.SelectedItem.Text=="All")?"":"( "+rbEmpList.SelectedItem.Text+" )";
                   lblMessage.InnerText = "warning->Any  Employee " + Emptype + " Are Not Separated In This Month";
                    return;
                }
            }
        }
    }
}