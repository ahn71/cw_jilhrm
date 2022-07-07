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
    public partial class EmployeeEducation : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                getEmpEducationlist(Session["_EmpId_"].ToString());
            }
        }
        private void getEmpEducationlist(string EmpId)
        {
            try
            {
                sqlDB.fillDataTable("Select SN, Degree, Year, Institute, Result from Personnel_EmpEducation  where EmpId='" + EmpId + "'", dt = new DataTable());
                int totalRows = dt.Rows.Count;
                string divInfo = "";


                if (totalRows == 0)
                {
                    divInfo = "<div class='noData'>No Education available</div>";
                    divInfo += "<div class='dataTables_wrapper'><div class='head'></div></div>";
                    divEmpEducation.Controls.Add(new LiteralControl(divInfo));
                    return;
                }

                divInfo = " <table id='tblClassList' class='display'  > ";
                divInfo += "<thead>";
                divInfo += "<tr>";
                divInfo += "<th>Degree</th>";
                divInfo += "<th>Year</th>";
                divInfo += "<th>Institute</th>";

                divInfo += "<th>Result</th>";
                divInfo += "<th>Edit</th>";
                divInfo += "</tr>";

                divInfo += "</thead>";

                divInfo += "<tbody>";
                string id = "";

                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    id = dt.Rows[x]["SN"].ToString();
                    divInfo += "<tr id='r_" + id + "'>";
                    divInfo += "<td >" + dt.Rows[x]["Degree"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["Year"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["Institute"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["Result"].ToString() + "</td>";

                    divInfo += "<td class='numeric_control' >" + "<img src='/Images/datatable/edit.png' class='editImg'   onclick='editEducation(" + id + ");'  />";
                }

                divInfo += "</tbody>";
                divInfo += "<tfoot>";

                divInfo += "</table>";
                divInfo += "<div class='dataTables_wrapper'><div class='head'></div></div>";
                divEmpEducation.Controls.Add(new LiteralControl(divInfo));
            }
            catch { }
        }
        private Boolean saveEmpEducation()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Insert into  Personnel_EmpEducation (EmpId, Degree, Year, Institute, Result)  values (@EmpId, @Degree, @Year, @Institute, @Result) ", sqlDB.connection);

                cmd.Parameters.AddWithValue("@EmpId", Session["_EmpId_"].ToString());
                cmd.Parameters.AddWithValue("@Degree", txtDegree.Text.Trim());
                cmd.Parameters.AddWithValue("@Year", txtYear.Text.Trim());
                cmd.Parameters.AddWithValue("@Institute", txtInstitute.Text.Trim());
                cmd.Parameters.AddWithValue("@Result", txtResult.Text.Trim());

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
        private Boolean updateEmpEducation()
        {
            try
            {

                SqlCommand cmd = new SqlCommand(" update Personnel_EmpEducation  Set  Degree=@Degree, Year=@Year, Institute=@Institute, Result=@Result where SN=@SN ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@SN",hdfeducation.Value.ToString());
                cmd.Parameters.AddWithValue("@Degree", txtDegree.Text.Trim());
                cmd.Parameters.AddWithValue("@Year", txtYear.Text.Trim());
                cmd.Parameters.AddWithValue("@Institute", txtInstitute.Text.Trim());
                cmd.Parameters.AddWithValue("@Result", txtResult.Text.Trim());

                cmd.ExecuteNonQuery();

                return true;

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
                return false;
            }
        }
        private void ClearTextEducation()
        {
            txtDegree.Text = "";
            txtYear.Text = "";
            txtInstitute.Text = "";
            txtResult.Text = "";
            hdfeducation.Value = "";
           
        }

        protected void btnSaveEducation_Click(object sender, EventArgs e)
        {
            if (hdfeducation.Value.ToString().Length == 0)
            {
                saveEmpEducation();
                getEmpEducationlist(Session["_EmpId_"].ToString());
                ClearTextEducation();
                ////Session["_EmpStatus_"] = "";
                ////ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);  //Close New Tab for Sever side code
            }
            else
            {
                updateEmpEducation();
                getEmpEducationlist(Session["_EmpId_"].ToString());
                ClearTextEducation();
            }
        }

        protected void btnCloseEmpEducation_Click(object sender, EventArgs e)
        {
            Session["_EmpStatus_"] = "";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);  //Close New Tab for Sever side code
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
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