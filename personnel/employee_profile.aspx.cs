using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ComplexScriptingSystem;
using SigmaERP.classes;

namespace SigmaERP.personnel
{
    public partial class employee_profile : System.Web.UI.Page
    {
        DataTable dt;
        string CompanyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                divindivisual.Visible = false;
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                if (!classes.commonTask.HasBranch())
                    ddlBranch.Enabled = false;
                
            }
            
        }
        private void setPrivilege()
        {
            try
            {
                upSuperAdmin.Value = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "employee_profile.aspx", ddlBranch, WarningMessage, tblGenerateType, btnPrintpreview);
              
               
                ViewState["__ReadAction__"] = AccessPermission[0];
                ddlBranch.SelectedValue = ViewState["__CompanyId__"].ToString();
                //if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                //{
                //    //For supper admin & Master admin
                //    tblGenerateType.Visible = true;
                //    classes.commonTask.LoadBranch(ddlBranch);
                //    ddlBranch.SelectedValue = ViewState["__CompanyId__"].ToString();
                //    ViewState["__IsUserType__"] = "1";
                //    return;
                //}
                //else
                //{
                //    ViewState["__IsUserType__"] = "0";
                //    upSuperAdmin.Value = "0";
                //    DataTable dtSetPrivilege = new DataTable();
                //    sqlDB.fillDataTable("select * from UserPrivilege where PageName='employee_profile.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);
                //    //if (dt.Rows.Count > 0)
                //    //{
                //    //    if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                //    //    {
                //    //        btnPrintpreview.CssClass = "";
                //    //        btnPrintpreview.Enabled = false;
                //    //    }
                //    //}
                //    if (dtSetPrivilege.Rows.Count > 0)
                //    {
                //        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                //        {
                //            btnPrintpreview.CssClass = "css_btn Ptbut"; btnPrintpreview.Enabled = true; 
                //        }
                //        else
                //        {
                //            tblGenerateType.Visible = false;
                //            WarningMessage.Visible = true;
                //            btnPrintpreview.CssClass = ""; btnPrintpreview.Enabled = false;
                            
                //            // mainDiv.Style.Add("Pointer-event", "none");
                //        }

                //    }
                //    else
                //    {
                //        tblGenerateType.Visible = false;
                //        WarningMessage.Visible = true;
                //        btnPrintpreview.CssClass = ""; btnPrintpreview.Enabled = false;                       
                //        // mainDiv.Style.Add("Pointer-event", "none");
                //    }
                //}

            }
            catch { }

        }

        protected void btnPrintpreview_Click(object sender, EventArgs e)
        {
            
            if (rdball.Checked == true)
            {
              
                    sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeProfile where  EmpStatus in ('1','8') and IsActive=1 and CompanyId='"+ddlBranch.SelectedValue+"' Group by EmpId", dt=new DataTable());
              
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
                sqlDB.fillDataTable("Select  EmpId, CompanyId,FatherName,MotherName,EmpCompanyName,Address,Telephone,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,BasicSalary,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,Age,Sex,RName,NationIDCardNo,EmpAccountNo,SalaryCount,GrdName,Type,WagesType,BankName,EmpType,SftName,PreCity,PrePostBox,PerCity,MobileNo,Email,ContactName,EmpRelation,JobDescription,Gender,FamilyAge,BloodGroup,EmpPicture,Nationality,MaritialStatus,PresentAd,PermanentAd,MobileNo,EmpPresentSalary  From v_EmployeeProfile where SN " + setSn + " order by DptCode,CustomOrdering", dt);
                Session["__EmployeeProfile__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeProfile-" + rblReportStructure.SelectedValue + "');", true);  //Open New Tab for Sever side code
            }
            else if (rdbindividual.Checked == true)
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select  EmpId, CompanyId,FatherName,MotherName,EmpCompanyName,Address,Telephone,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,BasicSalary,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,Age,Sex,RName,NationIDCardNo,EmpAccountNo,SalaryCount,GrdName,Type,WagesType,BankName,EmpType,SftName,PreCity,PrePostBox,PerCity,MobileNo,Email,ContactName,EmpRelation,JobDescription,Gender,FamilyAge,BloodGroup,EmpPicture,Nationality,MaritialStatus,PresentAd,PermanentAd,MobileNo,EmpPresentSalary   From v_EmployeeProfile where SN=" + ddlCardNo.SelectedValue + " and IsActive=1", dt);
                Session["__EmployeeProfile__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeProfile-"+rblReportStructure.SelectedValue+"');", true);  //Open New Tab for Sever side code
            }
           // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeProfile');", true);  //Open New Tab for Sever side code
        }

        protected void rdball_CheckedChanged(object sender, EventArgs e)
        {
           
            divindivisual.Visible = false;
            rdbindividual.Checked = false;         
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);      
        }

        protected void rdbindividual_CheckedChanged(object sender, EventArgs e)
        {            
            rdball.Checked = false;
            divindivisual.Visible = true;
            CompanyID = (ddlBranch.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
            classes.commonTask.LoadEmpCardNoByEmpType(ddlCardNo, CompanyID, rblEmpType.SelectedValue);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }       

        protected void btnOldProfile_Click(object sender, EventArgs e)
        {           
            if (rdball.Checked == true)
            {
                dt = new DataTable();
             
                    sqlDB.fillDataTable("Select Min(SN) as SN,EmpId From  v_EmployeeProfile where  EmpStatus in ('1','8') and IsActive='1' and CompanyId='" + ddlBranch.SelectedValue + "' Group by EmpId", dt=new DataTable());

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
                sqlDB.fillDataTable("Select EmpPresentSalary, EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName,convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,BasicSalary,PresentAd,PresentZipCode,PermanentAd,PermanentDistrict,PermanentZipCode,convert(varchar(11),DateOfBirth,106) as DateOfBirth,Age,Sex,MaritialStatus,Weight,Height,Religion,Nationality,NationIDCardNo,EmpAccountNo,MedicalAllownce,FoodAllownce,ConvenceAllownce,HouseRent,OthersAllownce,EmpTypeId,NumberofChild,SalaryCount,EmpPicture,DstName,PerDstName From v_EmployeeProfile where SN " + setSn + " order by EmpCardNo", dt);
                Session["__EmployeeProfile__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeProfile-" + upSuperAdmin.Value + "');", true);  //Open New Tab for Sever side code
            }
            else if (rdbindividual.Checked == true)
            {               
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpPresentSalary, EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName,convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,BasicSalary,PresentAd,PresentZipCode,PermanentAd,PermanentDistrict,PermanentZipCode,convert(varchar(11),DateOfBirth,106) as DateOfBirth,Age,Sex,MaritialStatus,Weight,Height,Religion,Nationality,NationIDCardNo,EmpAccountNo,MedicalAllownce,FoodAllownce,ConvenceAllownce,HouseRent,OthersAllownce,EmpTypeId,NumberofChild,SalaryCount,EmpPicture,DstName,PerDstName From v_EmployeeProfile where SN=(Select MIN(SN) as SN From v_EmployeeProfile where EmpCardNo='" + ddlCardNo.SelectedItem.Text + "') and ActiveSalary='True'", dt);
                Session["__EmployeeProfile__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeProfile-" + upSuperAdmin.Value + "');", true);  //Open New Tab for Sever side code
            }

        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdball.Checked = true;           
            divindivisual.Visible = false;
            rdbindividual.Checked = false;           
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }

        protected void rblEmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompanyID = (ddlBranch.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString(): ddlBranch.SelectedValue;
            classes.commonTask.LoadEmpCardNoByEmpType(ddlCardNo, CompanyID, rblEmpType.SelectedValue);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }
    }
}