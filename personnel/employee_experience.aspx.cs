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
    public partial class employee_experience : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                getEmpExperiencelist(Session["_EmpId_"].ToString());
            }
        }
        private void getEmpExperiencelist(string EmpId)
        {
            try
            {
                sqlDB.fillDataTable("Select CompanyName, Designation, Responsibility, YearOfExp,  convert(varchar(11),JoiningDate,105) as JoiningDate,  convert(varchar(11),ResignDate,105) as ResignDate, SpecialQualification, SN from Personnel_EmpExperience where EmpId='" + EmpId + "'", dt = new DataTable());
                int totalRows = dt.Rows.Count;
                string divInfo = "";


                if (totalRows == 0)
                {
                    divInfo = "<div class='noData'>No Experience available</div>";
                    divInfo += "<div class='dataTables_wrapper'><div class='head'></div></div>";
                    divEmpExperience.Controls.Add(new LiteralControl(divInfo));
                    return;
                }

                divInfo = " <table id='tblClassList' class='display'  > ";
                divInfo += "<thead>";
                divInfo += "<tr>";
                divInfo += "<th>Name of Company</th>";
                divInfo += "<th>Designation</th>";
                divInfo += "<th>Responsibility</th>";

                divInfo += "<th>Year of Experience</th>";
                divInfo += "<th>Joining Date</th>";
                divInfo += "<th>Resign Date</th>";
                divInfo += "<th>Special Qualification</th>";
                divInfo += "<th>Edit</th>";
                divInfo += "</tr>";

                divInfo += "</thead>";

                divInfo += "<tbody>";
                string id = "";

                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    id = dt.Rows[x]["SN"].ToString();
                    divInfo += "<tr id='r_" + id + "'>";
                    divInfo += "<td >" + dt.Rows[x]["CompanyName"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["Designation"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["Responsibility"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["YearOfExp"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["JoiningDate"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["ResignDate"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["SpecialQualification"].ToString() + "</td>";

                    divInfo += "<td class='numeric_control' >" + "<img src='/Images/datatable/edit.png' class='editImg'   onclick='editEmpExperience(" + id + ");'  />";
                }

                divInfo += "</tbody>";
                divInfo += "<tfoot>";

                divInfo += "</table>";
                divInfo += "<div class='dataTables_wrapper'><div class='head'></div></div>";
                divEmpExperience.Controls.Add(new LiteralControl(divInfo));
            }
            catch { }
        }
        private Boolean saveEmpExperience()
        {
            try
            {
                System.Data.SqlTypes.SqlDateTime getDate;
                getDate = SqlDateTime.Null;
                SqlCommand cmd = new SqlCommand("Insert into  Personnel_EmpExperience (EmpId, CompanyName, Designation, Responsibility, YearOfExp, JoiningDate, ResignDate, SpecialQualification)  values (@EmpId, @CompanyName, @Designation, @Responsibility, @YearOfExp, @JoiningDate, @ResignDate, @SpecialQualification) ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", Session["_EmpId_"].ToString());
                cmd.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text.Trim());
                cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text.Trim());
                cmd.Parameters.AddWithValue("@Responsibility", txtResponsibility.Text.Trim());
                if (txtYearOfExp.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@YearOfExp", 0);
                }
                else cmd.Parameters.AddWithValue("@YearOfExp", txtYearOfExp.Text.Trim());
                if (txtJoiningDateExperience.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@JoiningDate", getDate);
                }
                else cmd.Parameters.AddWithValue("@JoiningDate", convertDateTime.getCertainCulture(txtJoiningDateExperience.Text.Trim()));
                if (txtResignDate.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@ResignDate", getDate);
                }
                else cmd.Parameters.AddWithValue("@ResignDate", convertDateTime.getCertainCulture(txtResignDate.Text.Trim()));
                cmd.Parameters.AddWithValue("@SpecialQualification", txtSpecialQualification.Text.Trim());

                int result = (int)cmd.ExecuteNonQuery();

                if (result > 0)
                {
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
        private Boolean updateEmpExperience()
        {
            try
            {
                System.Data.SqlTypes.SqlDateTime getDate;
                getDate = SqlDateTime.Null;
                SqlCommand cmd = new SqlCommand(" update Personnel_EmpExperience  Set  CompanyName=@CompanyName, Designation=@Designation, Responsibility=@Responsibility, YearOfExp=@YearOfExp, JoiningDate=@JoiningDate, ResignDate=@ResignDate, SpecialQualification=@SpecialQualification where SN=@SN", sqlDB.connection);
                cmd.Parameters.AddWithValue("@SN",hdfexperience.Value.ToString());
                cmd.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text.Trim());
                cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text.Trim());
                cmd.Parameters.AddWithValue("@Responsibility", txtResponsibility.Text.Trim());
                cmd.Parameters.AddWithValue("@YearOfExp", txtYearOfExp.Text.Trim());
                if (txtJoiningDateExperience.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@JoiningDate", getDate);
                }
                else cmd.Parameters.AddWithValue("@JoiningDate", convertDateTime.getCertainCulture(txtJoiningDateExperience.Text.Trim()));
                if (txtResignDate.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@ResignDate", getDate);
                }
                else cmd.Parameters.AddWithValue("@ResignDate", convertDateTime.getCertainCulture(txtResignDate.Text.Trim()));
                cmd.Parameters.AddWithValue("@SpecialQualification", txtSpecialQualification.Text.Trim());

                cmd.ExecuteNonQuery();

                return true;

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
                return false;
            }
        }
        private void ClearTextExperience()
        {
            txtCompanyName.Text = "";
            txtDesignation.Text = "";
            txtResponsibility.Text = "";
            txtYearOfExp.Text = "";
            txtJoiningDateExperience.Text = "";
            txtResignDate.Text = "";
            txtSpecialQualification.Text = "";
            hdfexperience.Value = "";
          
        }

        protected void btnSaveExperience_Click(object sender, EventArgs e)
        {
            if (hdfexperience.Value.ToString().Length == 0)
            {
                saveEmpExperience();
                getEmpExperiencelist(Session["_EmpId_"].ToString());
                ClearTextExperience();
                //if (Session["_EmpStatus_"] != null)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindowsClose('/personnel/EmployeeEducation.aspx');", true);  //Open New Tab for Sever side code
                //}
            }
            else
            {
                updateEmpExperience();
                getEmpExperiencelist(Session["_EmpId_"].ToString());
                ClearTextExperience();
            }
        }

        protected void btnExperienceClose_Click(object sender, EventArgs e)
        {
            Session["_EmpStatus_"] = "";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);  //Close New Tab for Sever side code
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {

            if (Session["_EmpStatus_"] != null)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindowsClose('/personnel/EmployeeAddress.aspx');", true);  //Open New Tab for Sever side code
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (Session["_EmpStatus_"] != null)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindowsClose('/personnel/EmployeeEducation.aspx');", true);  //Open New Tab for Sever side code
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);
            }
        }
    }
}