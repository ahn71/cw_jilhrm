using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SigmaERP.personnel
{
    public partial class EmployeeAddress : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                ViewState["__IsSave__"] = "";
                classes.commonTask.LoadDistrict(ddlPerCity);
                classes.commonTask.LoadDistrict(ddlPreCity);
                //LoadPreThana(ddlPreThana);
               // LoadPerThana(ddlPerThana);
                LoadEmpAddressDetails();
            }
        }
        
        private void LoadPresentThana(DropDownList dl)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("Select Distinct * From HRDThanaInfo where DstId=" + ddlPreCity.SelectedValue + " order by ThaName", dt);
            dl.DataValueField = "ThaId";
            dl.DataTextField = "ThaName";
            dl.DataSource = dt;
            dl.DataBind();
        }
        private void LoadPermanetThana(DropDownList dl)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("Select * From HRDThanaInfo where DstId=" + ddlPerCity.SelectedValue + " order by ThaName", dt);
            dl.DataValueField = "ThaId";
            dl.DataTextField = "ThaName";
            dl.DataSource = dt;
            dl.DataBind();
        }      
        private void LoadEmpAddressDetails()
        {
            try
            {
                DataTable dta= new DataTable();
                sqlDB.fillDataTable("Select * From Personnel_EmpAddress where EmpId='" + Session["_EmpId_"].ToString() + "'", dta);
                if (dta.Rows.Count > 0)
                {
                    txtPreVillage.Text = dta.Rows[0]["PreVillage"].ToString();
                    txtPreVillageBangla.Text = dta.Rows[0]["PreVillageBangla"].ToString();
                    txtPrePO.Text = dta.Rows[0]["PrePO"].ToString();
                    txtPrePOBangla.Text = dta.Rows[0]["PrePOBangla"].ToString();
                    txtPrePostBox.Text = dta.Rows[0]["PrePostBox"].ToString();
                    ddlPreCity.SelectedValue = dta.Rows[0]["PreCity"].ToString();
                    LoadPresentThana(ddlPreThana);
                    ddlPreThana.SelectedValue = dta.Rows[0]["PreThanaId"].ToString();
                    txtPerVillage.Text = dta.Rows[0]["PerVillage"].ToString();
                    txtPerVillageBangla.Text = dta.Rows[0]["PerVillageBangla"].ToString();
                    txtPerPO.Text = dta.Rows[0]["PerPO"].ToString();
                    txtPerPOBangla.Text = dta.Rows[0]["PerPOBangla"].ToString();
                    txtPerPostBox.Text = dta.Rows[0]["PerPostBox"].ToString();
                    ddlPerCity.SelectedValue = dta.Rows[0]["PerCity"].ToString();
                    LoadPermanetThana(ddlPerThana);
                    ddlPerThana.SelectedValue = dta.Rows[0]["PerThanaId"].ToString();
                    txtMobileNo.Text = dta.Rows[0]["MobileNo"].ToString();
                    txtEmailAddress.Text = dta.Rows[0]["Email"].ToString();
                    btnSaveAddress.Text = "Update";
                }
                sqlDB.fillDataTable("Select * From Personnel_EmergencyContact where EmpId=" + Session["_EmpId_"].ToString() + "", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    txtContactName.Text = dt.Rows[0]["ContactName"].ToString();
                    txtEmpRelation.Text = dt.Rows[0]["EmpRelation"].ToString();
                    txtEmergencyAddress.Text = dt.Rows[0]["EmergencyAddress"].ToString();
                    txtEmergencyPhoneNo.Text = dt.Rows[0]["EmergencyPhoneNo"].ToString();
                    txtJobDescription.Text = dt.Rows[0]["JobDescription"].ToString();
                    ddlGender.Text = dt.Rows[0]["Gender"].ToString();
                    txtAge.Text = dt.Rows[0]["Age"].ToString();
                }
                else 
                {
                    ViewState["__IsSave__"] = "Yes";
                }
            }
            catch { }
        }
        private Boolean saveEmpAddress()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Insert into  Personnel_EmpAddress (EmpId, PreVillage, PreVillageBangla, PrePO, PrePOBangla, PrePostBox, PreThanaId, PreCity, PerVillage, PerVillageBangla, PerPO, PerPOBangla, PerPostBox, PerThanaId, PerCity, MobileNo, Email)  "
                + " values (@EmpId, @PreVillage, @PreVillageBangla, @PrePO, @PrePOBangla, @PrePostBox, @PreThanaId, @PreCity, @PerVillage, @PerVillageBangla, @PerPO, @PerPOBangla, @PerPostBox, @PerThanaId, @PerCity, @MobileNo, @Email) ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", Session["_EmpId_"].ToString());
                cmd.Parameters.AddWithValue("@PreVillage", txtPreVillage.Text.Trim());
                cmd.Parameters.AddWithValue("@PreVillageBangla", txtPreVillageBangla.Text.Trim());
                cmd.Parameters.AddWithValue("@PrePO", txtPrePO.Text.Trim());
                cmd.Parameters.AddWithValue("@PrePOBangla", txtPrePOBangla.Text.Trim());
                cmd.Parameters.AddWithValue("@PrePostBox", txtPrePostBox.Text.Trim());
                cmd.Parameters.AddWithValue("@PreThanaId", ddlPreThana.SelectedValue);
                cmd.Parameters.AddWithValue("@PreCity", ddlPreCity.SelectedValue);
                cmd.Parameters.AddWithValue("@PerVillage", txtPerVillage.Text.Trim());
                cmd.Parameters.AddWithValue("@PerVillageBangla", txtPerVillageBangla.Text.Trim());
                cmd.Parameters.AddWithValue("@PerPO", txtPerPO.Text.Trim());
                cmd.Parameters.AddWithValue("@PerPOBangla", txtPerPOBangla.Text);
                cmd.Parameters.AddWithValue("@PerPostBox", txtPerPostBox.Text.Trim());
                cmd.Parameters.AddWithValue("@PerThanaId", ddlPerThana.SelectedValue);
                cmd.Parameters.AddWithValue("@PerCity", ddlPerCity.SelectedValue);
                cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmailAddress.Text.Trim());

                int result = (int)cmd.ExecuteNonQuery();

                if (result > 0) lblMessage.InnerText = "success->Successfully saved";
                else lblMessage.InnerText = "error->Unable to save";

                return true;

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
                return false;
            }
        }
        private Boolean updateEmpAddress()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("  update Personnel_EmpAddress  Set  PreVillage=@PreVillage, PreVillageBangla=@PreVillageBangla, PrePO=@PrePO, PrePOBangla=@PrePOBangla, PrePostBox=@PrePostBox, PreThanaId=@PreThanaId, PreCity=@PreCity, PerVillage=@PerVillage, PerVillageBangla=@PerVillageBangla, PerPO=@PerPO, PerPOBangla=@PerPOBangla, PerPostBox=@PerPostBox, PerThanaId=@PerThanaId, PerCity=@PerCity, MobileNo=@MobileNo, Email=@Email  where EmpId=@EmpId ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", Session["_EmpId_"].ToString());
                cmd.Parameters.AddWithValue("@PreVillage", txtPreVillage.Text.Trim());
                cmd.Parameters.AddWithValue("@PreVillageBangla", txtPreVillageBangla.Text.Trim());
                cmd.Parameters.AddWithValue("@PrePO", txtPrePO.Text.Trim());
                cmd.Parameters.AddWithValue("@PrePOBangla", txtPrePOBangla.Text.Trim());
                cmd.Parameters.AddWithValue("@PrePostBox", txtPrePostBox.Text.Trim());
                cmd.Parameters.AddWithValue("@PreThanaId", ddlPreThana.SelectedValue);
                cmd.Parameters.AddWithValue("@PreCity", ddlPreCity.SelectedValue);
                cmd.Parameters.AddWithValue("@PerVillage", txtPerVillage.Text.Trim());
                cmd.Parameters.AddWithValue("@PerVillageBangla", txtPerVillageBangla.Text.Trim());
                cmd.Parameters.AddWithValue("@PerPO", txtPerPO.Text.Trim());
                cmd.Parameters.AddWithValue("@PerPOBangla", txtPerPOBangla.Text);
                cmd.Parameters.AddWithValue("@PerPostBox", txtPerPostBox.Text.Trim());
                cmd.Parameters.AddWithValue("@PerThanaId", ddlPerThana.SelectedValue);
                cmd.Parameters.AddWithValue("@PerCity", ddlPerCity.SelectedValue);
                cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmailAddress.Text.Trim());

                cmd.ExecuteNonQuery();

                return true;

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
                return false;
            }
        }
        private Boolean saveEmergencyContact()
        {
            try
            {
                   
                SqlCommand cmd = new SqlCommand("Insert into  Personnel_EmergencyContact (EmpId, ContactName, EmergencyAddress, EmpRelation, EmergencyPhoneNo, JobDescription, Gender, Age)  values (@EmpId, @ContactName, @EmergencyAddress, @EmpRelation, @EmergencyPhoneNo, @JobDescription, @Gender, @Age) ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", Session["_EmpId_"].ToString());
                cmd.Parameters.AddWithValue("@ContactName", txtContactName.Text.Trim());
                cmd.Parameters.AddWithValue("@EmergencyAddress", txtEmergencyAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpRelation", txtEmpRelation.Text.Trim());
                cmd.Parameters.AddWithValue("@EmergencyPhoneNo", txtEmergencyPhoneNo.Text.Trim());
                cmd.Parameters.AddWithValue("@JobDescription", txtJobDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", ddlGender.Text);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text.Trim());

                int result = (int)cmd.ExecuteNonQuery();

                if (result > 0) lblMessage.InnerText = "success->Successfully saved";
                else lblMessage.InnerText = "error->Unable to save";

                return true;

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
                return false;
            }
        }
        private Boolean updateEmergencyContact()
        {
            try
            {
                if (ViewState["__IsSave__"].ToString().Equals("Yes"))
                {
                   
                    if (saveEmergencyContact())
                        return true;
                    else return false;
                } 
                SqlCommand cmd = new SqlCommand("  update Personnel_EmergencyContact  Set ContactName=@ContactName, EmergencyAddress=@EmergencyAddress, EmpRelation=@EmpRelation, EmergencyPhoneNo=@EmergencyPhoneNo, JobDescription=@JobDescription, Gender=@Gender, Age=@Age  where EmpId=@EmpId ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", Session["_EmpId_"].ToString());
                cmd.Parameters.AddWithValue("@ContactName", txtContactName.Text.Trim());
                cmd.Parameters.AddWithValue("@EmergencyAddress", txtEmergencyAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpRelation", txtEmpRelation.Text.Trim());
                cmd.Parameters.AddWithValue("@EmergencyPhoneNo", txtEmergencyPhoneNo.Text.Trim());
                cmd.Parameters.AddWithValue("@JobDescription", txtJobDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", ddlGender.Text);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text.Trim());

                cmd.ExecuteNonQuery();

                return true;

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
                return false;
            }
        }
        
        protected void btnSaveAddress_Click(object sender, EventArgs e)
        {
            if (btnSaveAddress.Text == "Save")
            {
                saveEmpAddress();
                saveEmergencyContact();
                if (Session["_EmpStatus_"] != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindowsClose('/personnel/employee_experience.aspx');", true);  //Open New Tab for Sever side code
                }
            }
            else
            {
                updateEmpAddress();
                updateEmergencyContact();
                ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);  //Close New Tab for Sever side code
            }
        }
        
        protected void btnEmpAddressclose_Click(object sender, EventArgs e)
        {
            Session["_EmpStatus_"] = "";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);  //Close New Tab for Sever side code
        }
        protected void ddlPreCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPresentThana(ddlPreThana);
        }

        protected void ddlPerCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPermanetThana(ddlPerThana);
        }   

        protected void ebntSameAddress_Click(object sender, ImageClickEventArgs e)
        {
            txtPerVillage.Text = txtPreVillage.Text;
            txtPerVillageBangla.Text = txtPreVillageBangla.Text;
            txtPerPO.Text = txtPrePO.Text;
            txtPerPOBangla.Text = txtPrePOBangla.Text;
            txtPerPostBox.Text = txtPrePostBox.Text;
            ddlPerCity.SelectedValue = ddlPreCity.SelectedValue;
            LoadPermanetThana(ddlPerThana);
            ddlPerThana.SelectedValue = ddlPreThana.SelectedValue;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (Session["_EmpStatus_"] != null)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindowsClose('/personnel/employee_experience.aspx');", true);  //Open New Tab for Sever side code
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);
            }
        }
       
      
    }
}