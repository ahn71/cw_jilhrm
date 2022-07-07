using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SigmaERP.personnel
{
    public partial class EmployeePersonal : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                classes.commonTask.LoadRligion(dsReligion);
                classes.commonTask.LoadEducationalQui(ddlLastEdQualification);
                LoadEmpPersonalInfo();
            }
        }
        private void LoadEmpPersonalInfo()
        {
            try
            {
                sqlDB.fillDataTable("Select Personnel_EmpPersonnal.FatherName, Personnel_EmpPersonnal.MotherName, Personnel_EmpPersonnal.FatherNameBn, Personnel_EmpPersonnal.MotherNameBN, Personnel_EmpPersonnal.MaritialStatus,convert(varchar(11),Personnel_EmpPersonnal.DateOfBirth,105) as DateOfBirth, Personnel_EmpPersonnal.PlaceOfBirth, Personnel_EmpPersonnal.Height, Personnel_EmpPersonnal.Weight, Personnel_EmpPersonnal.BloodGroup, Personnel_EmpPersonnal.Sex,  Personnel_EmpPersonnal.NoOfExperience, Personnel_EmpPersonnal.Nationality, Personnel_EmpPersonnal.NationIDCardNo,Personnel_EmpPersonnal.NumberofChild,HRD_Qualification.QName,HRD_Religion.RName from Personnel_EmpPersonnal Left JOIN HRD_Qualification ON Personnel_EmpPersonnal.QId=HRD_Qualification.QId LEFT OUTER JOIN HRD_Religion ON Personnel_EmpPersonnal.RId = HRD_Religion.RId  where Personnel_EmpPersonnal.EmpId='" + Session["_EmpId_"].ToString() + "'", dt = new DataTable());
                if (dt.Rows.Count == 0) return;
                dsBloodGroup.Text = dt.Rows[0]["BloodGroup"].ToString();
                dsDateOfBirth.Text = dt.Rows[0]["DateOfBirth"].ToString();
                dsFatherName.Text = dt.Rows[0]["FatherName"].ToString();
                dsFatherNameBn.Text = dt.Rows[0]["FatherNameBn"].ToString();
                dsHeight.Text = dt.Rows[0]["Height"].ToString();
                ddlLastEdQualification.SelectedItem.Text= dt.Rows[0]["QName"].ToString();
                dsMaritialStatus.Text = dt.Rows[0]["MaritialStatus"].ToString();
                dsMotherName.Text = dt.Rows[0]["MotherName"].ToString();
                dsMotherNameBN.Text = dt.Rows[0]["MotherNameBN"].ToString();
                dsNationality.Text = dt.Rows[0]["Nationality"].ToString();
                dsNationIDCardNo.Text = dt.Rows[0]["NationIDCardNo"].ToString();
                dsNoOfExperience.Text = dt.Rows[0]["NoOfExperience"].ToString();
                dsPlaceOfBirth.Text = dt.Rows[0]["PlaceOfBirth"].ToString();
                
                dsSex.Text = dt.Rows[0]["Sex"].ToString();
                dsWeight.Text = dt.Rows[0]["Weight"].ToString();
                btnSavePersonal.Text = "Update";
                dsReligion.SelectedItem.Text = dt.Rows[0]["RName"].ToString();
                txtNumberofchild.Text = dt.Rows[0]["NumberofChild"].ToString();
            }
            catch { }
        }
        private Boolean saveEmpPersonnal()
        {
            try
            {
                System.Data.SqlTypes.SqlDateTime getDate;
                getDate = SqlDateTime.Null;
                SqlCommand cmd = new SqlCommand("Insert into  Personnel_EmpPersonnal (EmpId, FatherName, MotherName, FatherNameBn, MotherNameBN, MaritialStatus, DateOfBirth,Age, PlaceOfBirth, Height, Weight, BloodGroup, Sex, RId, QId, NoOfExperience, Nationality, NationIDCardNo,NumberofChild)  values (@EmpId, @FatherName, @MotherName, @FatherNameBn, @MotherNameBN, @MaritialStatus, @DateOfBirth,@Age, @PlaceOfBirth, @Height, @Weight, @BloodGroup, @Sex, @RId, @QId, @NoOfExperience, @Nationality, @NationIDCardNo,@NumberofChild) ", sqlDB.connection);

                cmd.Parameters.AddWithValue("@EmpId", Session["_EmpId_"].ToString());
                cmd.Parameters.AddWithValue("@FatherName", dsFatherName.Text.Trim());
                cmd.Parameters.AddWithValue("@MotherName", dsMotherName.Text.Trim());
                cmd.Parameters.AddWithValue("@FatherNameBn", dsFatherNameBn.Text.Trim());
                cmd.Parameters.AddWithValue("@MotherNameBN", dsMotherNameBN.Text.Trim());
                cmd.Parameters.AddWithValue("@MaritialStatus", dsMaritialStatus.Text.Trim());
                if (dsDateOfBirth.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@DateOfBirth", getDate);
                    cmd.Parameters.AddWithValue("@Age", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DateOfBirth", convertDateTime.getCertainCulture(dsDateOfBirth.Text.Trim()));
                    DateTime dof = convertDateTime.getCertainCulture(dsDateOfBirth.Text.Trim());
                    int Year = dof.Year;
                    int Month = dof.Month;
                    int date = dof.Day;
                    TimeSpan TS = DateTime.Now - new DateTime(Year, Month, date);
                    double Years = TS.TotalDays / 365.25;
                    int Age = Convert.ToInt32(Years);
                    cmd.Parameters.AddWithValue("@Age", Age);
                }
                cmd.Parameters.AddWithValue("@PlaceOfBirth", dsPlaceOfBirth.Text.Trim());
                cmd.Parameters.AddWithValue("@Height", dsHeight.Text.Trim());
                cmd.Parameters.AddWithValue("@Weight", dsWeight.Text);
                cmd.Parameters.AddWithValue("@BloodGroup", dsBloodGroup.Text.Trim());
                cmd.Parameters.AddWithValue("@Sex", dsSex.Text.Trim());
                cmd.Parameters.AddWithValue("@RId", dsReligion.SelectedValue);
                cmd.Parameters.AddWithValue("@QId", ddlLastEdQualification.SelectedValue);
                cmd.Parameters.AddWithValue("@NoOfExperience", dsNoOfExperience.Text.Trim());
                cmd.Parameters.AddWithValue("@Nationality", dsNationality.Text.Trim());
                cmd.Parameters.AddWithValue("@NationIDCardNo", dsNationIDCardNo.Text.Trim());
                cmd.Parameters.AddWithValue("@NumberofChild",txtNumberofchild.Text);

                int result = (int)cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    SqlCommand cmd2 = new SqlCommand("Insert Into HRD_ManpowerStatus(EmpId,Male,Female) values(@EmpId,@Male,@Female)", sqlDB.connection);
                    cmd2.Parameters.AddWithValue("@EmpId", Session["_EmpId_"].ToString());
                    if (dsSex.Text.Trim() == "Male")
                    {
                        cmd2.Parameters.AddWithValue("@Male", 1);
                        cmd2.Parameters.AddWithValue("@Female", 0);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@Male", 0);
                        cmd2.Parameters.AddWithValue("@Female", 1);
                    }
                    cmd2.ExecuteNonQuery();
                    lblMessage.InnerText = "success->Successfully saved";
                }
                else lblMessage.InnerText = "error->Unable to save";

                return true;

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
                return false;
            }
        }
        private Boolean updateEmpPersonnal()
        {
            try
            {
                System.Data.SqlTypes.SqlDateTime getDate;
                getDate = SqlDateTime.Null;

                SqlCommand cmd = new SqlCommand(" update Personnel_EmpPersonnal  Set FatherName=@FatherName, MotherName=@MotherName, FatherNameBn=@FatherNameBn, MotherNameBN=@MotherNameBN, MaritialStatus=@MaritialStatus, DateOfBirth=@DateOfBirth,Age=@Age, PlaceOfBirth=@PlaceOfBirth, Height=@Height, Weight=@Weight, BloodGroup=@BloodGroup, Sex=@Sex, RId=@RId, QId=@QId, NoOfExperience=@NoOfExperience, Nationality=@Nationality, NationIDCardNo=@NationIDCardNo,NumberofChild=@NumberofChild where EmpId=@EmpId ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", Session["_EmpId_"].ToString());
                cmd.Parameters.AddWithValue("@FatherName", dsFatherName.Text.Trim());
                cmd.Parameters.AddWithValue("@MotherName", dsMotherName.Text.Trim());
                cmd.Parameters.AddWithValue("@FatherNameBn", dsFatherNameBn.Text.Trim());
                cmd.Parameters.AddWithValue("@MotherNameBN", dsMotherNameBN.Text.Trim());
                cmd.Parameters.AddWithValue("@MaritialStatus", dsMaritialStatus.Text.Trim());
                if (dsDateOfBirth.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@DateOfBirth", getDate);
                    cmd.Parameters.AddWithValue("@Age","");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DateOfBirth", convertDateTime.getCertainCulture(dsDateOfBirth.Text.Trim()));
                    DateTime dof = convertDateTime.getCertainCulture(dsDateOfBirth.Text.Trim());
                    int Year = dof.Year;
                    int Month = dof.Month;
                    int date = dof.Day;
                    TimeSpan TS = DateTime.Now - new DateTime(Year, Month, date);
                    double Years = TS.TotalDays / 365.25;
                    int Age=Convert.ToInt32(Years);
                    cmd.Parameters.AddWithValue("@Age", Age);
                }
                cmd.Parameters.AddWithValue("@PlaceOfBirth", dsPlaceOfBirth.Text.Trim());
                cmd.Parameters.AddWithValue("@Height", dsHeight.Text.Trim());
                if (dsWeight.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@Weight", 0);
                }
                else cmd.Parameters.AddWithValue("@Weight", dsWeight.Text);
                cmd.Parameters.AddWithValue("@BloodGroup", dsBloodGroup.Text.Trim());
                cmd.Parameters.AddWithValue("@Sex", dsSex.Text.Trim());
                cmd.Parameters.AddWithValue("@RId", dsReligion.SelectedValue);
                cmd.Parameters.AddWithValue("@QId", ddlLastEdQualification.SelectedValue);
                if (dsNoOfExperience.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@NoOfExperience", 0);
                }
                else cmd.Parameters.AddWithValue("@NoOfExperience", dsNoOfExperience.Text.Trim());
                cmd.Parameters.AddWithValue("@Nationality", dsNationality.Text.Trim());
                cmd.Parameters.AddWithValue("@NationIDCardNo", dsNationIDCardNo.Text.Trim());
                cmd.Parameters.AddWithValue("@NumberofChild",txtNumberofchild.Text.Trim());

               int result=(int)cmd.ExecuteNonQuery();
               if (result > 0)
               {
                   SqlCommand cmd2 = new SqlCommand("Update HRD_ManpowerStatus set Male=@Male,Female=@Female where EmpId='" + Session["_EmpId_"].ToString() + "'", sqlDB.connection);
                  
                   if (dsSex.Text.Trim() == "Male")
                   {
                       cmd2.Parameters.AddWithValue("@Male", 1);
                       cmd2.Parameters.AddWithValue("@Female", 0);
                   }
                   else
                   {
                       cmd2.Parameters.AddWithValue("@Male", 0);
                       cmd2.Parameters.AddWithValue("@Female", 1);
                   }
                   cmd2.ExecuteNonQuery();
               }

                return true;

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
                return false;
            }
        }

        protected void btndivClose_Click(object sender, EventArgs e)
        {

            Session["_EmpStatus_"] = "";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);  //Close New Tab for Sever side code
            
        }

        protected void btnSavePersonal_Click(object sender, EventArgs e)
        {
            if (btnSavePersonal.Text == "Save")
            {              
                saveEmpPersonnal();
                if (Session["_EmpStatus_"] != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindowsClose('/personnel/EmployeeAddress.aspx');", true);  //Open New Tab for Sever side code
                }
            }
            else
            {
                
                updateEmpPersonnal();
                ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);  //Close New Tab for Sever side code
               
            }
           
        }
    }

}