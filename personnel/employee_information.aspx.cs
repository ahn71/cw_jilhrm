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
    public partial class employee_information : System.Web.UI.Page
    {
        DataTable dt;
        static DataTable dtSetPrivilege;
        string CompanyId;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sqlDB.connectionString = Glory.getConnectionString();
                sqlDB.connectDB();
                lblMessage.InnerText = "";
                if (!IsPostBack)
                {
                    classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                    setPrivilege();
                    HttpCookie getCookies = Request.Cookies["userInfo"];
                    //ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                    trFdate.Visible = false;
                    trTdate.Visible = false;
                    trStatus.Visible = false;
                    trDistrict.Visible = false;
                    trReligion.Visible = false;
                    ddlEmpStatus.Items.Insert(0, new ListItem("All", "0"));
                    if (!classes.commonTask.HasBranch())
                        ddlCompany.Enabled = false;
                    ddlCardNo.Items.Insert(0,new ListItem("","0"));

                }
            }
            catch { }
        }
        private void setPrivilege()
        {
            try
            {
                

                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string getUserId = getCookies["__getUserId__"].ToString();

                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "employee_information.aspx", ddlCompany, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
              
                string cmpID = ViewState["__CompanyId__"].ToString();

                trCompany.Visible = true;

                classes.commonTask.LoadEmpCardNoByEmpType(ddlCardNo, ViewState["__CompanyId__"].ToString(), rblEmpType.SelectedValue);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                classes.commonTask.loadDepartmentListByCompany(ddlDepartment, ViewState["__CompanyId__"].ToString());

                ddlDepartment.Items.RemoveAt(0);
                addAllTextInDepartment();
                LoadDesignation(ViewState["__CompanyId__"].ToString(), ddlDepartment.SelectedValue, lstAll);
                
            }
            catch { }
            

        }
        private void addAllTextInDepartment()
        {
            if (ddlDepartment.Items.Count > 1)
                ddlDepartment.Items.Insert(0, new ListItem("All", "0000"));
        }      
        protected void chkjoiningdate_CheckedChanged(object sender, EventArgs e)
        {
          
                trStatus.Visible = true;
                trFdate.Visible = false;
                trTdate.Visible = false;
           
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (rblLanguage.SelectedValue == "EN")
                ShowEmpList();
            else
                ShowEmpListBangla();
        }
        private void ShowEmpListBangla()
        {
            try
            {
                string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId=" + rblEmpType.SelectedValue + " ";
                

                string ReportType = "";
                CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
                string DepartmentID = (ddlDepartment.SelectedValue.Equals("0000")) ? "" : " and DptId='" + ddlDepartment.SelectedValue + "'";
                if (rdblSearchEmployee.SelectedItem.Text == "Basic")
                {
                    ReportType = "BasicInfo";
                    string sqlCmd = "";
                    if (ddlCardNo.SelectedValue == "0")
                    {

                        if (ddlDepartment.SelectedValue.Equals("0000"))
                            sqlCmd = "Select EmpId, CompanyId,FatherNameBn as FatherName,MotherNameBn as MotherName,CompanyNameBangla EmpCompanyName,AddressBangla Address,Telephone,  Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpNameBn EmpName,DsgNameBn DsgName ,DptNameBn DptName,DptId,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,BasicSalary,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,Age,Sex,RName,NationIDCardNo,EmpAccountNo,SalaryCount,Type,WagesType,BankName,EmpType,PreCity,PrePostBox,PerCity,MobileNo,Email,ContactName,EmpRelation,JobDescription,Gender,FamilyAge,BloodGroup,EmpPicture,Nationality,MaritialStatus,PresentAd,PermanentAd,MobileNo,EmpPresentSalary,RealProximityNo,EmpTypeId,EmpTypeBn,OverTime,GrdNameBangla GrdName From v_EmployeeProfile where CompanyId='" + CompanyId + "'   " + EmpTypeID + "  order by convert(int, DptCode),convert(int, DptId),CustomOrdering";
                        else
                            sqlCmd = "Select EmpId, CompanyId,FatherNameBn as FatherName,MotherNameBn as MotherName,CompanyNameBangla EmpCompanyName,AddressBangla Address,Telephone,  Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpNameBn EmpName,DsgNameBn DsgName ,DptNameBn DptName,DptId,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,BasicSalary,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,Age,Sex,RName,NationIDCardNo,EmpAccountNo,SalaryCount,Type,WagesType,BankName,EmpType,PreCity,PrePostBox,PerCity,MobileNo,Email,ContactName,EmpRelation,JobDescription,Gender,FamilyAge,BloodGroup,EmpPicture,Nationality,MaritialStatus,PresentAd,PermanentAd,MobileNo,EmpPresentSalary,RealProximityNo,EmpTypeId,EmpTypeBn,OverTime,GrdNameBangla GrdName From v_EmployeeProfile where CompanyId='" + CompanyId + "'    and DptId ='" + ddlDepartment.SelectedValue + "' " + EmpTypeID + "  order by convert(int, DptCode),convert(int, DptId),CustomOrdering";


                    }
                    else
                    {
                        //if (txtCardNo.Text.Length < 4)
                        //{
                        //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                        //    txtCardNo.Focus(); return;
                        //}
                        sqlCmd = "Select EmpId, CompanyId,FatherName,MotherName,EmpCompanyName,Address,Telephone,  Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName, DptId,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,BasicSalary,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,Age,Sex,RName,NationIDCardNo,EmpAccountNo,SalaryCount,GrdName,Type,WagesType,BankName,EmpType,SftName,PreCity,PrePostBox,PerCity,MobileNo,Email,ContactName,EmpRelation,JobDescription,Gender,FamilyAge,BloodGroup,EmpPicture,Nationality,MaritialStatus,PresentAd,PermanentAd,MobileNo,EmpPresentSalary From v_EmployeeProfile where CompanyId='" + CompanyId + "' and SN=" + ddlCardNo.SelectedValue + " ";
                    }
                    dt = new DataTable();
                    sqlDB.fillDataTable(sqlCmd, dt);
                }
                else if (rdblSearchEmployee.SelectedItem.Text == "Designation")
                {
                    ReportType = "Designation";
                    if (ddlCardNo.SelectedValue == "0")
                    {
                        if (lstSelected.Items.Count < 1)
                        {
                            lblMessage.InnerText = "warning->Please Select Any Designation!"; lstSelected.Focus();
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                            return;
                        }
                        string setPredicate = "";
                        for (int b = 0; b < lstSelected.Items.Count; b++)
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
                        sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1 and DsgId " + setPredicate + " and CompanyId='" + CompanyId + "'    " + EmpTypeID + "   Group by EmpId", dt);
                        string setSn = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i == 0 && i == dt.Rows.Count - 1)
                            {
                                setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else if (i == 0 && i != dt.Rows.Count - 1)
                            {
                                setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                            else if (i != 0 && i == dt.Rows.Count - 1)
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                        }
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpNameBn EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyNameBangla CompanyName,DptNameBn DptName,DsgNameBn DsgName,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,RNameBn RName,Sex,DstBangla DstName,AddressBangla Address,CompanyId,DptId,DsgId  From v_EmployeeDetails where IsActive=1 and DsgId " + setPredicate + " and SN " + setSn + " order by convert(int, DptCode),convert(int, DptId),convert(int,DsgId),CustomOrdering", dt);
                    }
                    else
                    {
                        //if (txtCardNo.Text.Length < 4)
                        //{
                        //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                        //    txtCardNo.Focus();
                        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true); 
                        //    return;                        

                        //}
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where CompanyId='" + CompanyId + "' and IsActive=1 and SN=" + ddlCardNo.SelectedValue + "", dt);


                    }
                }

        //------------------------Start----------------------------

                else if (rdblSearchEmployee.SelectedItem.Text == "District")    //  Purpose: Districtwise Employee List Report
                {                                                              //   Updated By Nayem.
                    ReportType = "District";
                    if (ddlCardNo.SelectedValue == "0")
                    {
                        dt = new DataTable();

                        if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                        {
                            if (ddlCompany.SelectedValue == "0000" && ddlDistrict.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1    " + EmpTypeID + "  " + DepartmentID + "  Group by EmpId", dt);
                            }
                            else if (ddlCompany.SelectedValue == "0000" && ddlDistrict.SelectedValue != "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1    " + DepartmentID + " and PerCity=" + ddlDistrict.SelectedValue + " " + EmpTypeID + "    Group by EmpId", dt);
                            }
                            else if (ddlCompany.SelectedValue != "0000" && ddlDistrict.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1 and  CompanyId='" + ddlCompany.SelectedValue + "'    " + DepartmentID + " " + EmpTypeID + "   Group by EmpId", dt);
                            }
                            else
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1 and PerCity=" + ddlDistrict.SelectedValue + " and CompanyId='" + ddlCompany.SelectedValue + "'    " + DepartmentID + " " + EmpTypeID + "   Group by EmpId", dt);
                            }
                        }
                        else
                        {
                            if (ddlDistrict.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1 and CompanyId='" + ViewState["__CompanyId__"].ToString() + "'     " + EmpTypeID + " " + DepartmentID + "   Group by EmpId", dt);
                            }
                            else if (ddlDistrict.SelectedValue != "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1 and PerCity=" + ddlDistrict.SelectedValue + "  and CompanyId='" + ViewState["__CompanyId__"].ToString() + "'    " + EmpTypeID + "  " + DepartmentID + "   Group by EmpId", dt);
                            }
                        }


                        string setSn = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i == 0 && i == dt.Rows.Count - 1)
                            {
                                setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else if (i == 0 && i != dt.Rows.Count - 1)
                            {
                                setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                            else if (i != 0 && i == dt.Rows.Count - 1)
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                        }
                        dt = new DataTable();
                        if (ddlDistrict.SelectedValue == "0000")
                        {
                            sqlDB.fillDataTable("Select EmpNameBn EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyNameBangla CompanyName,DptNameBn DptName,DsgNameBn DsgName,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,RNameBn RName,Sex,DstBangla DstName,AddressBangla Address,CompanyId,DptId,DsgId  From v_EmployeeDetails where IsActive=1  and SN " + setSn + " order by CustomOrdering", dt);
                        }
                        else
                        {
                            sqlDB.fillDataTable("Select EmpNameBn EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyNameBangla CompanyName,DptNameBn DptName,DsgNameBn DsgName,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,RNameBn RName,Sex,DstBangla DstName,AddressBangla Address,CompanyId,DptId,DsgId From v_EmployeeDetails where IsActive=1 and PerCity=" + ddlDistrict.SelectedValue + "  and SN " + setSn + " order by CustomOrdering", dt);
                        }
                    }
                    else
                    {
                        //if (txtCardNo.Text.Length < 4)
                        //{
                        //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                        //    txtCardNo.Focus(); return;

                        //}
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where CompanyId='" + CompanyId + "' and IsActive=1 and SN=" + ddlCardNo.SelectedValue + "", dt);


                    }

                }

                else if (rdblSearchEmployee.SelectedItem.Text == "Joining")
                {
                    ReportType = "Designation";
                    string[] Fromdate = dtpFromDate.Text.Split('-');
                    string[] Todate = dtpTodate.Text.Split('-');
                    if (ddlCardNo.SelectedValue == "0")
                    {
                        dt = new DataTable();
                        if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                        {
                            if (ddlCompany.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1    and EmpJoiningDate between '" + Fromdate[2] + "-" + Fromdate[1] + "-" + Fromdate[0] + "' and '" + Todate[2] + "-" + Todate[1] + "-" + Todate[0] + "'  " + EmpTypeID + " " + DepartmentID + "  Group by EmpId", dt);
                            }
                            else
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1    and EmpJoiningDate between '" + Fromdate[2] + "-" + Fromdate[1] + "-" + Fromdate[0] + "' and '" + Todate[2] + "-" + Todate[1] + "-" + Todate[0] + "' and CompanyId='" + ddlCompany.SelectedValue + "'  " + EmpTypeID + " " + DepartmentID + "  Group by EmpId", dt);
                            }
                        }
                        else
                        {
                            sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1    and EmpJoiningDate between '" + Fromdate[2] + "-" + Fromdate[1] + "-" + Fromdate[0] + "' and '" + Todate[2] + "-" + Todate[1] + "-" + Todate[0] + "' and CompanyId='" + ViewState["__CompanyId__"] + "'  " + EmpTypeID + " " + DepartmentID + "  Group by EmpId", dt);
                        }
                        string setSn = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i == 0 && i == dt.Rows.Count - 1)
                            {
                                setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else if (i == 0 && i != dt.Rows.Count - 1)
                            {
                                setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                            else if (i != 0 && i == dt.Rows.Count - 1)
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                        }
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpNameBn EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyNameBangla CompanyName,DptNameBn DptName,DsgNameBn DsgName,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,RNameBn RName,Sex,DstBangla DstName,AddressBangla Address,CompanyId,DptId,DsgId  From v_EmployeeDetails where IsActive=1 and SN " + setSn + " order by convert(int, DptCode),convert(int, DptId),convert(int,DsgId),CustomOrdering", dt);
                    }
                    else
                    {
                        //if (txtCardNo.Text.Length < 4)
                        //{
                        //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                        //    txtCardNo.Focus();
                        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true); 
                        //    return;                           
                        //}
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where CompanyId='" + CompanyId + "' and IsActive=1 and SN=" + ddlCardNo.SelectedValue + "", dt);

                    }
                }
                else if (rdblSearchEmployee.SelectedItem.Text == "Religion") // purpose: Religionwise Employee List Report
                {
                    ReportType = "Religion";
                    if (ddlCardNo.SelectedValue == "0")
                    {
                        dt = new DataTable();
                        if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                        {
                            if (ddlCompany.SelectedValue == "0000" && ddlReligion.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1      " + EmpTypeID + " " + DepartmentID + "  Group by EmpId", dt);
                            }
                            else if (ddlCompany.SelectedValue == "0000" && ddlReligion.SelectedValue != "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1    and RId=" + ddlReligion.SelectedValue + "  " + EmpTypeID + " " + DepartmentID + "  Group by EmpId", dt);
                            }
                            else if (ddlCompany.SelectedValue != "0000" && ddlReligion.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1     and CompanyId='" + ddlCompany.SelectedValue + "'  " + EmpTypeID + " " + DepartmentID + "  Group by EmpId", dt);
                            }
                            else
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1    and RId=" + ddlReligion.SelectedValue + " and CompanyId='" + ddlCompany.SelectedValue + "'  " + EmpTypeID + " " + DepartmentID + "  Group by EmpId", dt);
                            }
                        }
                        else
                        {
                            if (ddlReligion.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1     and CompanyId='" + ViewState["__CompanyId__"] + "'  " + EmpTypeID + " " + DepartmentID + "  Group by EmpId", dt);
                            }
                            else
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where IsActive=1    and RId=" + ddlReligion.SelectedValue + " and CompanyId='" + ViewState["__CompanyId__"] + "' " + EmpTypeID + " " + DepartmentID + "   Group by EmpId", dt);
                            }
                        }
                        string setSn = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i == 0 && i == dt.Rows.Count - 1)
                            {
                                setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else if (i == 0 && i != dt.Rows.Count - 1)
                            {
                                setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                            else if (i != 0 && i == dt.Rows.Count - 1)
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                        }
                        dt = new DataTable();
                        if (ddlReligion.SelectedValue == "0000")
                        {
                            sqlDB.fillDataTable("Select EmpNameBn EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyNameBangla CompanyName,DptNameBn DptName,DsgNameBn DsgName,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,RNameBn RName,Sex,DstBangla DstName,AddressBangla Address,CompanyId,DptId,DsgId  From v_EmployeeDetails where IsActive=1 and SN " + setSn + " order by CustomOrdering", dt);
                        }
                        else
                        {
                            sqlDB.fillDataTable("Select EmpNameBn EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyNameBangla CompanyName,DptNameBn DptName,DsgNameBn DsgName,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,RNameBn RName,Sex,DstBangla DstName,AddressBangla Address,CompanyId,DptId,DsgId  From v_EmployeeDetails where IsActive=1 and RId=" + ddlReligion.SelectedValue + " and SN " + setSn + " order by CustomOrdering", dt);
                        }
                    }
                    else
                    {
                        //if (txtCardNo.Text.Length < 4)
                        //{
                        //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                        //    txtCardNo.Focus();
                        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                        //    return;

                        //}
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where CompanyId='" + CompanyId + "' and IsActive=1 and SN=" + ddlCardNo.SelectedValue + "", dt);

                    }
                }
                //-------------------------------------------END------------------------------------------------------

                if (dt.Rows.Count > 0)
                {
                    Session["__EmpInformationBangla__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmpInformationBangla-" + ReportType + "');", true);  //Open New Tab for Sever side code
                }
                else
                {
                    lblMessage.InnerText = "warning-> Any Employees Are Not Available!";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                }
            }
            catch { }
        }
        private void ShowEmpList()
        {
            try
            {
                string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId="+rblEmpType.SelectedValue+" ";
                
                string ReportType = "";
                CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
                string DepartmentID=(ddlDepartment.SelectedItem.Text.ToString().Equals("All"))?"":" and DptId='"+ddlDepartment.SelectedValue+"'";
                if (rdblSearchEmployee.SelectedItem.Text == "Basic")
                {
                    ReportType = "BasicInfo";
                   string sqlCmd="";
                   if (ddlCardNo.SelectedValue=="0")
                   {
                    
                           if (ddlDepartment.SelectedItem.Text.ToString().Equals("All"))
                               sqlCmd = "Select EmpId, CompanyId,FatherName,MotherName,EmpCompanyName,Address,Telephone,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName,DptId,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,BasicSalary,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,Age,Sex,RName,NationIDCardNo,EmpAccountNo,SalaryCount,GrdName,Type,WagesType,BankName,EmpType,SftName,PreCity,PrePostBox,PerCity,MobileNo,Email,ContactName,EmpRelation,JobDescription,Gender,FamilyAge,BloodGroup,EmpPicture,Nationality,MaritialStatus,PresentAd,PermanentAd,MobileNo,EmpPresentSalary,RealProximityNo,EmpTypeId,EmpType,OverTime From v_EmployeeProfile where EmpStatus in('1','8') And IsActive=1 AND CompanyId='" + CompanyId + "' " + EmpTypeID + " Order by  DptCode,DptId,CustomOrdering";
                           else
                               sqlCmd = "Select EmpId, CompanyId,FatherName,MotherName,EmpCompanyName,Address,Telephone,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName, DptId,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,BasicSalary,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,Age,Sex,RName,NationIDCardNo,EmpAccountNo,SalaryCount,GrdName,Type,WagesType,BankName,EmpType,SftName,PreCity,PrePostBox,PerCity,MobileNo,Email,ContactName,EmpRelation,JobDescription,Gender,FamilyAge,BloodGroup,EmpPicture,Nationality,MaritialStatus,PresentAd,PermanentAd,MobileNo,EmpPresentSalary,RealProximityNo,EmpTypeId,EmpType,OverTime From v_EmployeeProfile where EmpStatus in('1','8') And IsActive=1 AND CompanyId='" + CompanyId + "' and DptId ='" + ddlDepartment.SelectedValue + "' " + EmpTypeID + " Order by DptCode,DptId,CustomOrdering";
                      
                       
                   }
                   else 
                   {
                       //if (txtCardNo.Text.Length < 4)
                       //{
                       //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                       //    txtCardNo.Focus(); return;
                       //}
                       sqlCmd = "Select EmpId, CompanyId,FatherName,MotherName,EmpCompanyName,Address,Telephone, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName, DptId,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,BasicSalary,Format(DateOfBirth,'dd-MM-yyyy') as DateOfBirth,Age,Sex,RName,NationIDCardNo,EmpAccountNo,SalaryCount,GrdName,Type,WagesType,BankName,EmpType,SftName,PreCity,PrePostBox,PerCity,MobileNo,Email,ContactName,EmpRelation,JobDescription,Gender,FamilyAge,BloodGroup,EmpPicture,Nationality,MaritialStatus,PresentAd,PermanentAd,MobileNo,EmpPresentSalary,RealProximityNo From v_EmployeeProfile where EmpStatus in('1','8') AND CompanyId='" + CompanyId + "' and SN=" + ddlCardNo.SelectedValue + " ";
                   }
                    dt = new DataTable();
                    sqlDB.fillDataTable(sqlCmd, dt);
                }
               else if (rdblSearchEmployee.SelectedItem.Text == "Designation")
                {
                    ReportType = "Designation";
                    if (ddlCardNo.SelectedValue == "0")
                    {
                        if (lstSelected.Items.Count < 1) 
                        { 
                            lblMessage.InnerText = "warning->Please Select Any Designation!"; lstSelected.Focus();
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
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and DsgId " + setPredicate + " and CompanyId='" + CompanyId + "' " + EmpTypeID + "  Group by EmpId", dt);
                        string setSn = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i == 0 && i == dt.Rows.Count - 1)
                            {
                                setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else if (i == 0 && i != dt.Rows.Count - 1)
                            {
                                setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                            else if (i != 0 && i == dt.Rows.Count - 1)
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                            }
                            else
                            {
                                setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                            }
                        }
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and DsgId " + setPredicate + " and SN " + setSn + " order by CustomOrdering", dt);
                    }
                    else 
                    {
                        //if (txtCardNo.Text.Length < 4)
                        //{
                        //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                        //    txtCardNo.Focus();
                        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true); 
                        //    return;                        
                           
                        //}
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND CompanyId='" + CompanyId + "' and IsActive=1 and SN=" + ddlCardNo.SelectedValue + "", dt);
                   

                    }
                }
        
       //------------------------Start----------------------------

                else if (rdblSearchEmployee.SelectedItem.Text == "District")    //  Purpose: Districtwise Employee List Report
                {                                                              //   Updated By Nayem.
                    ReportType = "District";
                    if (ddlCardNo.SelectedValue=="0")
                    {
                    dt = new DataTable();
               
                        if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                        {
                            if (ddlCompany.SelectedValue == "0000" && ddlDistrict.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails EmpStatus in('1','8') AND where IsActive=1 " + EmpTypeID + "  " + DepartmentID + " Group by EmpId", dt);
                            }
                            else if (ddlCompany.SelectedValue == "0000" && ddlDistrict.SelectedValue != "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 " + DepartmentID + " and PerCity=" + ddlDistrict.SelectedValue + " " + EmpTypeID + "   Group by EmpId", dt);
                            }
                            else if (ddlCompany.SelectedValue != "0000" && ddlDistrict.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and  CompanyId='" + ddlCompany.SelectedValue + "' " + DepartmentID + " " + EmpTypeID + "  Group by EmpId", dt);
                            }
                            else
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and PerCity=" + ddlDistrict.SelectedValue + " and CompanyId='" + ddlCompany.SelectedValue + "' " + DepartmentID + " " + EmpTypeID + "  Group by EmpId", dt);
                            }
                        }
                        else
                        {
                            if (ddlDistrict.SelectedValue == "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and CompanyId='" + ViewState["__CompanyId__"].ToString() + "'  " + EmpTypeID + " " + DepartmentID + " Group by EmpId", dt);
                            }
                            else if (ddlDistrict.SelectedValue != "0000")
                            {
                                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and PerCity=" + ddlDistrict.SelectedValue + "  and CompanyId='" + ViewState["__CompanyId__"].ToString() + "' " + EmpTypeID + "  " + DepartmentID + "  Group by EmpId", dt);
                            }
                        }
                    
                  
                    string setSn = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0 && i == dt.Rows.Count - 1)
                        {
                            setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        }
                        else if (i == 0 && i != dt.Rows.Count - 1)
                        {
                            setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        }
                        else if (i != 0 && i == dt.Rows.Count - 1)
                        {
                            setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        }
                        else
                        {
                            setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        }
                    }
                    dt = new DataTable();
                    if (ddlDistrict.SelectedValue == "0000")
                    {
                        sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1  and SN " + setSn + " order by CustomOrdering", dt);
                    }
                    else
                    {
                        sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and PerCity=" + ddlDistrict.SelectedValue + "  and SN " + setSn + " order by CustomOrdering", dt);
                    }
                }
                    else
                    {
                        //if (txtCardNo.Text.Length < 4)
                        //{
                        //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                        //    txtCardNo.Focus(); return;
                          
                        //}
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND CompanyId='" + CompanyId + "' and IsActive=1 and SN=" + ddlCardNo.SelectedValue + "", dt);


                    }

                }
    
                else if (rdblSearchEmployee.SelectedItem.Text == "Joining")
                {
                    ReportType = "Designation";
                    string[] Fromdate = dtpFromDate.Text.Split('-');
                    string[] Todate = dtpTodate.Text.Split('-');
                    if (ddlCardNo.SelectedValue=="0") { 
                    dt = new DataTable();
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin")||ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                    {
                        if (ddlCompany.SelectedValue == "0000")
                        {
                            sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and EmpJoiningDate between '" + Fromdate[2] + "-" + Fromdate[1] + "-" + Fromdate[0] + "' and '" + Todate[2] + "-" + Todate[1] + "-" + Todate[0] + "'  " + EmpTypeID + " " + DepartmentID + " Group by EmpId", dt);
                        }
                        else
                        {
                            sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and EmpJoiningDate between '" + Fromdate[2] + "-" + Fromdate[1] + "-" + Fromdate[0] + "' and '" + Todate[2] + "-" + Todate[1] + "-" + Todate[0] + "' and CompanyId='" + ddlCompany.SelectedValue + "'  " + EmpTypeID + " " + DepartmentID + " Group by EmpId", dt);
                        }
                    }
                    else
                    {
                        sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and EmpJoiningDate between '" + Fromdate[2] + "-" + Fromdate[1] + "-" + Fromdate[0] + "' and '" + Todate[2] + "-" + Todate[1] + "-" + Todate[0] + "' and CompanyId='" + ViewState["__CompanyId__"] + "'  " + EmpTypeID + " " + DepartmentID + " Group by EmpId", dt);
                    }
                    string setSn = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0 && i == dt.Rows.Count - 1)
                        {
                            setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        }
                        else if (i == 0 && i != dt.Rows.Count - 1)
                        {
                            setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        }
                        else if (i != 0 && i == dt.Rows.Count - 1)
                        {
                            setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        }
                        else
                        {
                            setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        }
                    }
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and SN " + setSn + " order by CustomOrdering", dt);
                    }
                    else
                    {
                        //if (txtCardNo.Text.Length < 4)
                        //{
                        //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                        //    txtCardNo.Focus();
                        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true); 
                        //    return;                           
                        //}
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND CompanyId='" + CompanyId + "' and IsActive=1 and SN=" + ddlCardNo.SelectedValue + "", dt);

                    }
                }
                else if (rdblSearchEmployee.SelectedItem.Text == "Religion") // purpose: Religionwise Employee List Report
                {
                    ReportType = "Religion";
                    if (ddlCardNo.SelectedValue=="0") { 
                    dt = new DataTable();
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                    {
                        if (ddlCompany.SelectedValue == "0000" && ddlReligion.SelectedValue == "0000")
                        {
                            sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1   " + EmpTypeID + " " + DepartmentID + " Group by EmpId", dt);
                        }
                        else if (ddlCompany.SelectedValue == "0000" && ddlReligion.SelectedValue != "0000")
                        {
                            sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and RId=" + ddlReligion.SelectedValue + "  " + EmpTypeID + " " + DepartmentID + " Group by EmpId", dt);
                        }
                        else if (ddlCompany.SelectedValue != "0000" && ddlReligion.SelectedValue == "0000")
                        {
                            sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1  and CompanyId='" + ddlCompany.SelectedValue + "'  " + EmpTypeID + " " + DepartmentID + " Group by EmpId", dt);
                        }
                        else
                        {
                            sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and RId=" + ddlReligion.SelectedValue + " and CompanyId='" + ddlCompany.SelectedValue + "'  " + EmpTypeID + " " + DepartmentID + " Group by EmpId", dt);
                        }
                    }
                    else
                    {
                        if (ddlReligion.SelectedValue == "0000")
                        {
                            sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1  and CompanyId='" + ViewState["__CompanyId__"] + "'  " + EmpTypeID + " " + DepartmentID + " Group by EmpId", dt);
                        }
                        else
                        {
                            sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and RId=" + ddlReligion.SelectedValue + " and CompanyId='" + ViewState["__CompanyId__"] + "' " + EmpTypeID + " " + DepartmentID + "  Group by EmpId", dt);
                        }
                    }                
                    string setSn = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0 && i == dt.Rows.Count - 1)
                        {
                            setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        }
                        else if (i == 0 && i != dt.Rows.Count - 1)
                        {
                            setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        }
                        else if (i != 0 && i == dt.Rows.Count - 1)
                        {
                            setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        }
                        else
                        {
                            setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        }
                    }
                    dt = new DataTable();
                    if (ddlReligion.SelectedValue == "0000")
                    {
                        sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and SN " + setSn + " order by CustomOrdering", dt);
                    }
                    else
                    {
                        sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND IsActive=1 and RId=" + ddlReligion.SelectedValue + " and SN " + setSn + " order by CustomOrdering", dt);
                    }
                    }
                    else
                    {
                        //if (txtCardNo.Text.Length < 4)
                        //{
                        //    lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card No !";
                        //    txtCardNo.Focus();
                        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                        //    return;
                            
                        //}
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,EmpType,CompanyName,DptName,DsgName,Convert(varchar(11),EmpJoiningDate,106) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,106) as DateOfBirth,RName,Sex,DstName,Address  From v_EmployeeDetails where EmpStatus in('1','8') AND CompanyId='" + CompanyId + "' and IsActive=1 and SN=" + ddlCardNo.SelectedValue + "", dt);

                    }
                }
                //-------------------------------------------END------------------------------------------------------

                if (dt.Rows.Count > 0)
                {
                    Session["__EmpInformation__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmpInformation-" + ReportType + "');", true);  //Open New Tab for Sever side code
                }
                else
                {
                    lblMessage.InnerText = "warning-> Any Employees Are Not Available!";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                }
            }
            catch { }
        }


        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstAll.DataSource = "";
            lstAll.DataBind();
            lstSelected.DataSource = "";
            lstSelected.DataBind();
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            LoadDesignation(CompanyId, ddlDepartment.SelectedValue, lstAll);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }
        private void LoadDesignation(string cid,string dptid, ListBox lst)
        {
            try
            {
                DataTable dt = new DataTable();
                if (dptid == "0000" && cid=="0000")  sqlDB.fillDataTable("SELECT DsgId, DsgName FROM HRD_Designation", dt);
                else if (dptid == "0000" && cid != "0000") sqlDB.fillDataTable("SELECT DsgId, DsgName FROM v_HRD_Designation where CompanyId='"+cid+"'", dt);
                else sqlDB.fillDataTable("SELECT DsgId, DsgName FROM HRD_Designation where DptId=" + dptid + "", dt);
                lst.DataValueField = "DsgId";
                lst.DataTextField = "DsgName";
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
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            AddRemoveItem(lstAll, lstSelected);
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            AddRemoveAll(lstSelected, lstAll);
        }

        protected void rdblSearchEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);

            if (rdblSearchEmployee.SelectedItem.Text == "Basic")
            {
                LoadBasicInfo();
            }
            else if (rdblSearchEmployee.SelectedItem.Text == "Designation")
            {
              LoadDesignation();
            }
            else if (rdblSearchEmployee.SelectedItem.Text == "District")
            {
                LoadDistrict();
            }
            else if (rdblSearchEmployee.SelectedItem.Text == "Joining")
            {
                LoadJoiningDate();
            }
            else if (rdblSearchEmployee.SelectedItem.Text == "Religion")
            {
                LoadReligion();
            }
        }
        private void LoadBasicInfo()
        {
            try
            {
                trFdate.Visible = false;
                trTdate.Visible = false;
                trStatus.Visible = false;
                trDistrict.Visible = false;
                //trDpt.Visible = true;
                divdesignation.Visible = false;
                trReligion.Visible = false;            
            }
            catch { }
        }
        private void LoadDesignation()
        {
            try
            {
                trFdate.Visible = false;
                trTdate.Visible = false;
                trStatus.Visible = false;
                trDistrict.Visible = false;
                //trDpt.Visible = true;
                divdesignation.Visible = true;
                trReligion.Visible = false;
                lstAll.DataSource = "";
                lstAll.DataBind();
                lstSelected.DataSource = "";
                lstSelected.DataBind();
                CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
                classes.commonTask.loadDepartmentListByCompany(ddlDepartment, CompanyId);
                ddlDepartment.Items.RemoveAt(0);
                addAllTextInDepartment(); 
               LoadDesignation(ddlCompany.SelectedValue, ddlDepartment.SelectedValue, lstAll);              
            }
            catch { }
        }
        private void LoadDistrict()
        {
            try
            {
                trFdate.Visible = false;
                trTdate.Visible = false;
                trStatus.Visible = false;
                trDistrict.Visible = true;
                //trDpt.Visible = false;
                divdesignation.Visible = false;
                trReligion.Visible = false;
                classes.commonTask.LoadDistrict(ddlDistrict);
                ddlDistrict.Items.RemoveAt(0);
                ddlDistrict.Items.Insert(0,new ListItem("All","0000"));
            }
            catch { }
        }

        private void LoadJoiningDate()
        {
            try
            {
                trFdate.Visible = true;
                trTdate.Visible = true;
                trStatus.Visible = false;
                trDistrict.Visible = false;
                //trDpt.Visible = false;
                divdesignation.Visible = false;
                trReligion.Visible = false;
                DateTime today =new DateTime();
                today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpFromDate.Text = today.ToString("dd-MM-yyyy");
                dtpTodate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
            catch { }
        }
        private void LoadReligion()
        {
            try
            {
                trFdate.Visible = false;
                trTdate.Visible = false;
                trStatus.Visible = false;
                trDistrict.Visible = false;
                //trDpt.Visible = false;
                divdesignation.Visible = false;
                trReligion.Visible = true;
                classes.commonTask.LoadRligion(ddlReligion);
                ddlReligion.Items.Insert(0,new ListItem("All","0000"));
            }
            catch { }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                lstAll.DataSource = "";
                lstAll.DataBind();
                lstSelected.DataSource = "";
                lstSelected.DataBind();
                CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
                classes.commonTask.LoadEmpCardNoByEmpType(ddlCardNo, CompanyId, rblEmpType.SelectedValue);
                ddlCardNo.Items.Insert(0, new ListItem("", "0"));
                classes.commonTask.loadDepartmentListByCompany(ddlDepartment,CompanyId);
                ddlDepartment.Items.RemoveAt(0);
                addAllTextInDepartment();
                LoadDesignation(CompanyId, ddlDepartment.SelectedValue, lstAll);
               // classes.commonTask.LoadDepartmentByCompanyInListBox(CompanyId, lstAll);
                //ddlDepartment.Items.Insert(0,new ListItem("All","0000"));                
                LoadDesignation(CompanyId, ddlDepartment.SelectedValue, lstAll);
            }
            catch { }
        }

        protected void rblEmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            classes.commonTask.LoadEmpCardNoByEmpType(ddlCardNo, CompanyId, rblEmpType.SelectedValue);
            ddlCardNo.Items.Insert(0, new ListItem("", "0"));
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }
    }
}