using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using SigmaERP.classes;

namespace SigmaERP.hrd
{
    public partial class CompanyInfo : System.Web.UI.Page
    {
        static string imageName;
        string HeadOfficeId;     
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            divMsg.InnerText = "";
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                
                setPrivilege();
                loadCompanyInfoInfo();
                LoadCompanyId();
                LoadBusinessType();
                ViewState["__BranchType__"] = classes.commonTask.HasBranch();
                if (ViewState["__BranchType__"].ToString().Equals("False"))
                {                    
                    if (gvCompanyInfo.Rows.Count > 0)
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    rblOfficeType.Enabled = false;
                }
            }
        }
        private void LoadBusinessType() 
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from HRD_BusinessType where IsActive='1'", dt);
                ddlBusinessType.DataValueField = "BId";     
                ddlBusinessType.DataTextField = "BTypeName";                          
                ddlBusinessType.DataSource = dt;
                ddlBusinessType.DataBind();
                ddlBusinessType.Items.Insert(0, new ListItem(string.Empty, "0")); 
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private void setPrivilege()
        {
            try
            {
                //upupdate.Value = "1";               
                //upSave.Value = "1";
                //ViewState["__WriteAction__"] = "1";
                //ViewState["__DeletAction__"] = "1";
                //ViewState["__ReadAction__"] = "1";
                //ViewState["__UpdateAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                 ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "CompanyInfo.aspx", gvCompanyInfo, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
              
            }
            catch { }
        }
        private void LoadCompanyId()
        {
            try
            {
                string SL = "", ID="";;
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select Max(CompanyId) as CompanyId From HRD_CompanyInfo", dt);
                if (dt.Rows[0]["CompanyId"].ToString() == "")
                {
                    SL = "0001";
                }
                ID = (int.Parse(dt.Rows[0]["CompanyId"].ToString())).ToString();
                if (ID.Length == 1) SL = "000" + (int.Parse(ID) + 1);
                else if (ID.Length == 2) SL = "00" + (int.Parse(ID) + 1);
                else if (ID.Length == 3) SL = "0" + (int.Parse(ID) + 1);
                else if (ID.Length == 4) SL = (int.Parse(ID) + 1).ToString();
                txtCompanyId.Text = SL;
            }
            catch { }
        }
        private void AlterCompanyInfo(int ID)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select ID, CompanyId,CompanyType, CompanyName,HeadOfficeId, CompanyNameBangla, Address, AddressBangla, Country, Telephone, Fax, DefaultCurrency, BusinessType, MultipleBranch, Comments, CompanyLogo,StartCardNo,Weekend,ShortName,CardNoType,FlatCode,CardNoDigits,AttMachineName from HRD_CompanyInfo where ID=" + ID + " ", dt);
                if (dt.Rows.Count == 0)
                {
                    if (upSave.Value == "0")
                    {
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;
                    }
                    return;
                }
                hdfID.Value = dt.Rows[0]["ID"].ToString();
                txtCompanyId.Text = dt.Rows[0]["CompanyId"].ToString();
               // rblOfficeType.SelectedValue = dt.Rows[0]["CompanyType"].ToString();
                if (dt.Rows[0]["CompanyType"].ToString() != "True") //for Branch
                {
                    rblOfficeType.SelectedValue = "0";
                    loadHeadOffice();
                    trHeadOffice.Visible = true;
                    ddlHeadOffice.SelectedValue = dt.Rows[0]["HeadOfficeId"].ToString();
                }
                else { rblOfficeType.SelectedValue = "1"; trHeadOffice.Visible = false; }                              
                txtCompanyName.Text = dt.Rows[0]["CompanyName"].ToString();
                txtCompanyNameBangla.Text = dt.Rows[0]["CompanyNameBangla"].ToString();
                txtAddress.Text = dt.Rows[0]["Address"].ToString();
                txtAddressBangla.Text = dt.Rows[0]["AddressBangla"].ToString();
                txtCountry.Text = dt.Rows[0]["Country"].ToString();
                txtTelephone.Text = dt.Rows[0]["Telephone"].ToString();
                txtFax.Text = dt.Rows[0]["Fax"].ToString();
                ddlDefaultCurrency.Text= dt.Rows[0]["DefaultCurrency"].ToString();
                ddlBusinessType.SelectedValue = dt.Rows[0]["BusinessType"].ToString();               
                ddlMultipleBranch.Text = dt.Rows[0]["MultipleBranch"].ToString();
                txtComments.Text = dt.Rows[0]["Comments"].ToString();
                imageName = dt.Rows[0]["CompanyLogo"].ToString();
                string url = @"/EmployeeImages/CompanyLogo/" + Path.GetFileName(dt.Rows[0]["CompanyLogo"].ToString());
                imgProfile.ImageUrl = url;
                txtShortName.Text = dt.Rows[0]["ShortName"].ToString();
                ddlWeekend.Text = dt.Rows[0]["Weekend"].ToString();
                ddlCardNoDigit.SelectedValue = dt.Rows[0]["CardNoDigits"].ToString();
                txtStartCardNo.Text = dt.Rows[0]["StartCardNo"].ToString();
                ddlMachine.SelectedValue = dt.Rows[0]["AttMachineName"].ToString();
                if (dt.Rows[0]["CardNoType"].ToString().Equals("True"))
                {
                    rblCardNoType.SelectedValue = "1";
                    txtFladCode.Text = "99";
                    txtFladCode.Visible = false;
                    txtStartCardNo.Style.Add("Width", "97%");
                    tdFladCode.InnerText = "Start Card No";
                }
                else 
                {
                    rblCardNoType.SelectedValue = "0";

                    txtFladCode.Text = dt.Rows[0]["FlatCode"].ToString().Equals("") ? "99" : dt.Rows[0]["FlatCode"].ToString();
                    txtFladCode.Visible = true;
                    txtStartCardNo.Style.Add("Width", "71%");
                    tdFladCode.InnerText = "Flat Code";
                }                
                if (upupdate.Value == "0")
                {
                    btnSave.Text = "Update";
                    btnSave.CssClass = "";
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Text = "Update";
                    btnSave.CssClass = "Rbutton";
                    btnSave.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private void loadHeadOffice()
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("Select  CompanyId, CompanyName from HRD_CompanyInfo where CompanyType='1' ", dt);
            if (dt.Rows.Count < 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Warning();", true);
                //divMsg.InnerText = "First Set a Head Office!";
                //divMsg.Style.Add("color", "Red");
                rblOfficeType.SelectedValue = "1";
                //trHeadOffice.Visible = false;
                return;
            }
            ddlHeadOffice.DataValueField = "CompanyId";
            ddlHeadOffice.DataTextField = "CompanyName";
            ddlHeadOffice.DataSource = dt;
            ddlHeadOffice.DataBind();
            ddlHeadOffice.Items.Insert(0, new ListItem(string.Empty,"0000"));
            trHeadOffice.Visible = true;
                
        }
        private void loadCompanyInfoInfo()
        {
            try
            {
                DataTable dt = new DataTable();
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                {
                    sqlDB.fillDataTable("Select ID, CompanyId, CompanyName, CompanyNameBangla, Address, AddressBangla, Country, Telephone, Fax, DefaultCurrency, BTypeName,  MultipleBranch,  Comments, CompanyLogo,StartCardNo,ComType,AttMachineName from v_HRD_CompanyInfo ", dt);
                }
                else
                {
                    sqlDB.fillDataTable("Select ID, CompanyId, CompanyName, CompanyNameBangla, Address, AddressBangla, Country, Telephone, Fax, DefaultCurrency, BTypeName,  MultipleBranch,  Comments, CompanyLogo,StartCardNo,ComType,AttMachineName from v_HRD_CompanyInfo where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' ", dt);
                }
                gvCompanyInfo.DataSource = dt;
                gvCompanyInfo.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private Boolean saveCompanyInfo()
        {
            try
            {
                System.Data.SqlTypes.SqlDateTime getDate;
                getDate = SqlDateTime.Null;
                SqlCommand cmd = new SqlCommand("Insert into  HRD_CompanyInfo (CompanyId, CompanyType, HeadOfficeId, CompanyName, CompanyNameBangla, Address, AddressBangla, Country, Telephone, Fax, DefaultCurrency, BusinessType, MultipleBranch, Comments, CompanyLogo,StartCardNo,Weekend,ShortName,CardNoType,FlatCode,CardNoDigits,AttMachineName)  values (@CompanyId,@CompanyType,@HeadOfficeId, @CompanyName, @CompanyNameBangla, @Address, @AddressBangla, @Country, @Telephone, @Fax, @DefaultCurrency, @BusinessType,  @MultipleBranch, @Comments, @CompanyLogo,@StartCardNo,@Weekend,@ShortName,@CardNoType,@FlatCode,@CardNoDigits,@AttMachineName) ", sqlDB.connection);

                cmd.Parameters.AddWithValue("@CompanyId", txtCompanyId.Text.Trim());
                cmd.Parameters.AddWithValue("@CompanyType", rblOfficeType.SelectedValue);
                HeadOfficeId = (rblOfficeType.SelectedValue!="0") ? txtCompanyId.Text.Trim() : ddlHeadOffice.SelectedValue;
                cmd.Parameters.AddWithValue("@HeadOfficeId", HeadOfficeId.ToString());
                cmd.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text.Trim());
                cmd.Parameters.AddWithValue("@CompanyNameBangla", txtCompanyNameBangla.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@AddressBangla", txtAddressBangla.Text.Trim());
                cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                cmd.Parameters.AddWithValue("@Telephone", txtTelephone.Text.Trim());
                cmd.Parameters.AddWithValue("@Fax", txtFax.Text.Trim());
                cmd.Parameters.AddWithValue("@DefaultCurrency", ddlDefaultCurrency.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@BusinessType", ddlBusinessType.Text.Trim());               
                if(ddlMultipleBranch.Text=="Yes")
                {
                     cmd.Parameters.AddWithValue("@MultipleBranch",1 );
                }
                else
                {
                     cmd.Parameters.AddWithValue("@MultipleBranch",0 );
                }              
                cmd.Parameters.AddWithValue("@Comments", txtComments.Text.Trim());
                if(FileUpload1.HasFile==true)
                {
                    cmd.Parameters.AddWithValue("@CompanyLogo", "logo" + txtCompanyId.Text.Trim() + ".PNG");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CompanyLogo","" );
                }
                cmd.Parameters.AddWithValue("@StartCardNo", txtStartCardNo.Text);
                cmd.Parameters.AddWithValue("@Weekend", ddlWeekend.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@ShortName", txtShortName.Text.ToUpper());
                cmd.Parameters.AddWithValue("@CardNoType", rblCardNoType.SelectedValue);
                if(rblCardNoType.SelectedValue=="0")
                cmd.Parameters.AddWithValue("@FlatCode",txtFladCode.Text.Trim() );
                else cmd.Parameters.AddWithValue("@FlatCode", 0);
                cmd.Parameters.AddWithValue("@CardNoDigits", ddlCardNoDigit.SelectedValue);
                cmd.Parameters.AddWithValue("@AttMachineName", ddlMachine.SelectedValue);
                int result = (int)cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    saveImg();
                    AllClear();
                    ScriptManager.RegisterStartupScript(this.Page,Page.GetType(), "call me", "Success();", true);
                    return true;
                }
                else
                {
                    divMsg.InnerText = "Unable to save";
                    return false;
                }

                

            }
            catch (Exception ex)
            {
                divMsg.InnerText = "error->" + ex.Message;
                return false;
            }
        }
        private void saveImg()
        {
            try
            {
                
                string filename = "logo" + txtCompanyId.Text.Trim() + ".PNG";
                FileUpload1.SaveAs(Server.MapPath("/EmployeeImages/CompanyLogo/" + filename));
               


            }
            catch { }
        }
        private Boolean updateCompanyInfo()
        {
            try
            {
                System.Data.SqlTypes.SqlDateTime getDate;
                getDate = SqlDateTime.Null;
                if (FileUpload1.HasFile == true)
                {
                    SqlCommand cmd = new SqlCommand(" update HRD_CompanyInfo  Set CompanyId=@CompanyId, CompanyType=@CompanyType, HeadOfficeId=@HeadOfficeId, CompanyName=@CompanyName, CompanyNameBangla=@CompanyNameBangla, Address=@Address, AddressBangla=@AddressBangla, Country=@Country, Telephone=@Telephone, Fax=@Fax, DefaultCurrency=@DefaultCurrency, BusinessType=@BusinessType, MultipleBranch=@MultipleBranch, Comments=@Comments, CompanyLogo=@CompanyLogo,StartCardNo=@StartCardNo,Weekend=@Weekend,ShortName=@ShortName,CardNoType=@CardNoType,FlatCode=@FlatCode,CardNoDigits=@CardNoDigits,AttMachineName=@AttMachineName where ID=@ID ", sqlDB.connection);
                    cmd.Parameters.AddWithValue("@ID", hdfID.Value.ToString());
                    cmd.Parameters.AddWithValue("@CompanyId", txtCompanyId.Text.Trim());
                    cmd.Parameters.AddWithValue("@CompanyType", rblOfficeType.SelectedValue);
                    HeadOfficeId = (rblOfficeType.SelectedValue!= "0") ? txtCompanyId.Text.Trim() : ddlHeadOffice.SelectedValue;
                    cmd.Parameters.AddWithValue("@HeadOfficeId", HeadOfficeId.ToString());
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text.Trim());
                    cmd.Parameters.AddWithValue("@CompanyNameBangla", txtCompanyNameBangla.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@AddressBangla", txtAddressBangla.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@Telephone", txtTelephone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Fax", txtFax.Text.Trim());
                    cmd.Parameters.AddWithValue("@DefaultCurrency", ddlDefaultCurrency.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@BusinessType", ddlBusinessType.Text.Trim());
                    if (ddlMultipleBranch.Text == "Yes")
                    {
                        cmd.Parameters.AddWithValue("@MultipleBranch", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MultipleBranch", 0);
                    }
                    cmd.Parameters.AddWithValue("@Comments", txtComments.Text.Trim());
                    if (FileUpload1.HasFile == true)
                    {
                        cmd.Parameters.AddWithValue("@CompanyLogo","logo"+txtCompanyId.Text.Trim()+".PNG");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CompanyLogo", "");
                    }
                    cmd.Parameters.AddWithValue("@StartCardNo", txtStartCardNo.Text);
                    cmd.Parameters.AddWithValue("@Weekend", ddlWeekend.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@ShortName", txtShortName.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@CardNoType", rblCardNoType.SelectedValue);
                    if (rblCardNoType.SelectedValue == "0")
                        cmd.Parameters.AddWithValue("@FlatCode", txtFladCode.Text.Trim());
                    else cmd.Parameters.AddWithValue("@FlatCode", 0);

                    cmd.Parameters.AddWithValue("@CardNoDigits", ddlCardNoDigit.SelectedValue);
                    cmd.Parameters.AddWithValue("@AttMachineName", ddlMachine.SelectedValue);

                   /* cmd.Parameters.AddWithValue("@ID", hdfID.Value.ToString());
                    cmd.Parameters.AddWithValue("@CompanyId", txtCompanyId.Text.Trim());
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text.Trim());
                    cmd.Parameters.AddWithValue("@CompanyNameBangla", txtCompanyNameBangla.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@AddressBangla", txtAddressBangla.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@Telephone", txtTelephone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Fax", txtFax.Text.Trim());
                    cmd.Parameters.AddWithValue("@DefaultCurrency", ddlDefaultCurrency.Text.Trim());
                    cmd.Parameters.AddWithValue("@BusinessType", ddlBusinessType.Text.Trim());
                    //if (txtFinancialYearForm.Text.ToString().Length == 0)
                    //{
                    //    cmd.Parameters.AddWithValue("@FinancialYearForm", getDate);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@FinancialYearForm", convertDateTime.getCertainCulture(txtFinancialYearForm.Text.Trim()));
                    //}
                    //if (txtFinancialYearTo.Text.ToString().Length == 0)
                    //{
                    //    cmd.Parameters.AddWithValue("@FinancialYearTo", getDate);
                    //}
                    //else cmd.Parameters.AddWithValue("@FinancialYearTo", convertDateTime.getCertainCulture(txtFinancialYearTo.Text.Trim()));
                    if (ddlMultipleBranch.Text == "Yes")
                    {
                        cmd.Parameters.AddWithValue("@MultipleBranch", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MultipleBranch", 0);
                    }
                    //if (ddlUserAccessControl.Text == "Yes")
                    //{
                    //    cmd.Parameters.AddWithValue("@UserAccessControl", 1);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@UserAccessControl", 0);
                    //}
                    cmd.Parameters.AddWithValue("@Comments", txtComments.Text.Trim());
                   
                    cmd.Parameters.AddWithValue("@CompanyLogo", FileUpload1.FileName);
                    cmd.Parameters.AddWithValue("@StartCardNo", txtStartCardNo.Text);
                    cmd.Parameters.AddWithValue("@Weekend", ddlWeekend.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@ShortName",txtShortName.Text);
                    */

                    int result = (int)cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        if (imageName != "")
                        {
                            System.IO.File.Delete(Request.PhysicalApplicationPath + "/EmployeeImages/CompanyLogo/" + "logo"+txtCompanyId.Text.Trim()+".PNG");
                        }
                        //string filename = Path.GetFileName(FileUpload1.PostedFile.FileName)+txtCompanyId.Text.Trim();
                        string filename = "logo" + txtCompanyId.Text.Trim()+".PNG";                      
                        FileUpload1.SaveAs(Server.MapPath("/EmployeeImages/CompanyLogo/" + filename));

                        //string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                        //FileUpload1.SaveAs(Server.MapPath("/EmployeeImages/CompanyLogo/" + filename));    //Save images into Images folder
                        loadCompanyInfoInfo();
                        AllClear();              
                      
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "updSuccess();", true);
                        return true;
                    }
                     else
                {
                    lblMessage.InnerText = "error->Unable to save";
                    return false;
                }

                }
                else if (FileUpload1.HasFile == false)
                {
                    SqlCommand cmd = new SqlCommand("  update HRD_CompanyInfo  Set CompanyId=@CompanyId, CompanyType=@CompanyType, HeadOfficeId=@HeadOfficeId, CompanyName=@CompanyName, CompanyNameBangla=@CompanyNameBangla, Address=@Address, AddressBangla=@AddressBangla, Country=@Country, Telephone=@Telephone, Fax=@Fax, DefaultCurrency=@DefaultCurrency, BusinessType=@BusinessType, MultipleBranch=@MultipleBranch, Comments=@Comments, StartCardNo=@StartCardNo,Weekend=@Weekend,ShortName=@ShortName,CardNoType=@CardNoType,FlatCode=@FlatCode,CardNoDigits=@CardNoDigits,AttMachineName=@AttMachineName where ID=@ID  ", sqlDB.connection);
                    cmd.Parameters.AddWithValue("@ID", hdfID.Value.ToString());
                    cmd.Parameters.AddWithValue("@CompanyId", txtCompanyId.Text.Trim());
                    cmd.Parameters.AddWithValue("@CompanyType", rblOfficeType.SelectedValue);
                    HeadOfficeId = (rblOfficeType.SelectedValue != "0") ? txtCompanyId.Text.Trim() : ddlHeadOffice.SelectedValue;
                    cmd.Parameters.AddWithValue("@HeadOfficeId", HeadOfficeId.ToString());
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text.Trim());
                    cmd.Parameters.AddWithValue("@CompanyNameBangla", txtCompanyNameBangla.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@AddressBangla", txtAddressBangla.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@Telephone", txtTelephone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Fax", txtFax.Text.Trim());
                    cmd.Parameters.AddWithValue("@DefaultCurrency", ddlDefaultCurrency.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@BusinessType", ddlBusinessType.Text.Trim());


                  /*  cmd.Parameters.AddWithValue("@ID", hdfID.Value.ToString());
                    cmd.Parameters.AddWithValue("@CompanyId", txtCompanyId.Text.Trim());
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text.Trim());
                    cmd.Parameters.AddWithValue("@CompanyNameBangla", txtCompanyNameBangla.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@AddressBangla", txtAddressBangla.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@Telephone", txtTelephone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Fax", txtFax.Text.Trim());
                    cmd.Parameters.AddWithValue("@DefaultCurrency", ddlDefaultCurrency.SelectedItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@BusinessType", ddlBusinessType.Text.Trim());  */
                    //if (txtFinancialYearForm.Text.ToString().Length == 0)
                    //{
                    //    cmd.Parameters.AddWithValue("@FinancialYearForm", getDate);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@FinancialYearForm", convertDateTime.getCertainCulture(txtFinancialYearForm.Text.Trim()));
                    //}
                    //if (txtFinancialYearTo.Text.ToString().Length == 0)
                    //{
                    //    cmd.Parameters.AddWithValue("@FinancialYearTo", getDate);
                    //}
                    //else cmd.Parameters.AddWithValue("@FinancialYearTo", convertDateTime.getCertainCulture(txtFinancialYearTo.Text.Trim()));
                    if (ddlMultipleBranch.Text == "Yes")
                    {
                        cmd.Parameters.AddWithValue("@MultipleBranch", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MultipleBranch", 0);
                    }
                    //if (ddlUserAccessControl.Text == "Yes")
                    //{
                    //    cmd.Parameters.AddWithValue("@UserAccessControl", 1);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@UserAccessControl", 0);
                    //}
                    cmd.Parameters.AddWithValue("@Comments", txtComments.Text.Trim());
                    cmd.Parameters.AddWithValue("@StartCardNo", txtStartCardNo.Text);
                    cmd.Parameters.AddWithValue("@Weekend", ddlWeekend.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@ShortName", txtShortName.Text.ToUpper());

                    cmd.Parameters.AddWithValue("@CardNoType", rblCardNoType.SelectedValue);
                    if (rblCardNoType.SelectedValue == "0")
                        cmd.Parameters.AddWithValue("@FlatCode", txtFladCode.Text.Trim());
                    else cmd.Parameters.AddWithValue("@FlatCode", 0);
                    cmd.Parameters.AddWithValue("@CardNoDigits", ddlCardNoDigit.SelectedValue);
                    cmd.Parameters.AddWithValue("@AttMachineName",ddlMachine.SelectedValue);
                    int result = (int)cmd.ExecuteNonQuery();

                    if (result > 0)
                    {                     
                        loadCompanyInfoInfo();
                        AllClear();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "updSuccess();", true);
                        return true;
                    }
                    else
                    {
                        divMsg.InnerText = "error->Unable to save";
                        return false;
                    }
                }
                return true;
               
            }
            catch (Exception ex)
            {
                divMsg.InnerText = "error->" + ex.Message;
                divMsg.Style.Add("color","Red");
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Save")
            {
                saveCompanyInfo();
            }
            else
            {
                updateCompanyInfo();
            }
            loadCompanyInfoInfo();
        }

        protected void gvCompanyInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
              
                //GridViewRow gvrow 

                int index = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Alter")
                {


                    string ID = gvCompanyInfo.DataKeys[index].Values[0].ToString();
                    AlterCompanyInfo(int.Parse(ID));
                    if (deleteValidation(gvCompanyInfo.DataKeys[index].Values[1].ToString()))
                    {
                       
                        rblCardNoType.Enabled = true;
                        ddlCardNoDigit.Enabled = true;
                        txtStartCardNo.Enabled = true;
                    }
                    else 
                    {
                        rblCardNoType.Enabled = false;
                        ddlCardNoDigit.Enabled = false;
                        txtStartCardNo.Enabled = false;
                    }

                 
                }
                else if (e.CommandName == "Delete")
                {

                    if (deleteValidation(gvCompanyInfo.DataKeys[index].Values[1].ToString()))
                    {
                        Delete(Convert.ToInt32(gvCompanyInfo.DataKeys[index].Values[0].ToString()));
                    }
                    else
                        lblMessage.InnerText = "error->Warning! Can't delete this Company.";

                   
                }
            }
            catch { }
        }
        private void Delete(int ID)
        {
            try
            {
               
                SqlCommand cmd = new SqlCommand("Delete From HRD_CompanyInfo where ID="+ID+"", sqlDB.connection);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "delSuccess();", true);                
                AllClear();
                loadCompanyInfoInfo();
                    
               
            }
            catch { }
        }
        private void AllClear()
        {
            try
            {
                txtAddress.Text = "";
                txtAddressBangla.Text = "";
                txtComments.Text = "";
                txtCompanyId.Text = "";
                txtCompanyName.Text = "";
                txtCompanyNameBangla.Text = "";
                txtCountry.Text = "";
                rblOfficeType.SelectedValue = "1";
                trHeadOffice.Visible = false;
                ddlBusinessType.SelectedValue = "0";
                // ddlDefaultCurrency.SelectedItem.Text = "";
                txtFax.Text = "";
                //txtFinancialYearForm.Text = "";
                //txtFinancialYearTo.Text = "";
                txtTelephone.Text = "";
                txtStartCardNo.Text = "";
                txtShortName.Text = "";
                btnSave.Text = "Save";
                    imgProfile.ImageUrl = "~/images/profileImages/Logo.png";
                    LoadCompanyId();
                    if (ViewState["__BranchType__"].ToString().Equals("False") && gvCompanyInfo.Rows.Count > 0)
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    

            }
            catch { }
        }

        protected void gvCompanyInfo_RowDataBound(object sender, GridViewRowEventArgs e)
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


            try
            {
                if (ViewState["__DeletAction__"].ToString().Equals("0"))
                {
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                    btnDelete.Enabled = false;
                    btnDelete.OnClientClick = "return false";
                    btnDelete.ForeColor = Color.Silver; 

                }

            }
            catch { }
            try
            {
                if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                {
                    Button btnAlter = (Button)e.Row.FindControl("btnAlter");
                    btnAlter.Enabled = false;
                    btnAlter.ForeColor = Color.Silver;
                    
                }

            }
            catch { }
        
        }

        protected void gvCompanyInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                loadCompanyInfoInfo();
                gvCompanyInfo.PageIndex = e.NewPageIndex;
                gvCompanyInfo.DataBind();
            }
            catch { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            AllClear();
        }

        protected void gvCompanyInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            loadCompanyInfoInfo();
        }

        protected void rblOfficeType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (rblOfficeType.SelectedValue == "0") loadHeadOffice();
            else trHeadOffice.Visible = false;
        }

        protected void rblCardNoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblCardNoType.SelectedValue == "1")
            {
                txtFladCode.Visible = false;
                txtStartCardNo.Style.Add("Width", "97%");
                tdFladCode.InnerText = "Start Card No";
            }
            else 
            {
                txtFladCode.Visible = true;
                txtStartCardNo.Style.Add("Width", "71%");
                tdFladCode.InnerText = "Flat Code";
            }
        }
        private bool deleteValidation(string CompanyId)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("Select CompanyId from Personnel_EmpCurrentStatus  where CompanyId=" + CompanyId + "", dt);
            if (dt.Rows.Count > 0)
                return false;
            else return true;
        }
    }
}