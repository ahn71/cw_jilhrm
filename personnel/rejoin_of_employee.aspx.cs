using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class rejoin_of_employee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
               // classes.Employee.LoadResignedEmpCardNo(ddlEmployeeCardNo);
                loadRejonEmployeeDetails();
                btnDelete.Enabled = false;
                btnDelete.CssClass = "";
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
           
            
            if (hdnbtnStage.Value.Equals("1"))
            {
                UpdateEmpRejoin();
            }
            else
            {
                if (ddlEmployeeCardNo.SelectedIndex == 0)
                {
                    lblMessage.InnerText = "warning->You must Enter Card No";
                    loadRejonEmployeeDetails();
                    return;
                }
                saveEmpRejoin();
            }
            loadRejonEmployeeDetails();
        }
        private Boolean saveEmpRejoin()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Insert into  Personnel_EmpRejoin (EmpId, EmpCardNo, EffectiveDate, Remarks,EmpTypeId)  values (@EmpId, @EmpCardNo, @EffectiveDate, @Remarks,@EmpTypeId) ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", ddlEmployeeCardNo.SelectedValue);
                cmd.Parameters.AddWithValue("@EmpCardNo", ddlEmployeeCardNo.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@EffectiveDate", convertDateTime.getCertainCulture(txtEffectiveDate.Text.Trim()));
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select EmpJoiningDate from Personnel_EmployeeInfo where EmpId='" + ddlEmployeeCardNo.SelectedValue + "'", dt);
                
                string typeid = "";
                if(rdoEmpType.SelectedIndex==0)
                {
                     typeid="1";
                }
                else
                {
                     typeid="2";
                }
                cmd.Parameters.AddWithValue("@EmpTypeId", typeid);
                cmd.Parameters.AddWithValue("@LastJoiningDate",dt.Rows[0].ItemArray[0].ToString());

                int result = (int)cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    cmd = new SqlCommand("Update Personnel_EmpCurrentStatus set EmpStatus=1 where EmpId='" + ddlEmployeeCardNo.SelectedValue + "' and SN=(Select MAX(SN) as SN From Personnel_EmpCurrentStatus where EmpId='" + ddlEmployeeCardNo.SelectedValue + "')", sqlDB.connection);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("Update Personnel_EmployeeInfo set EmpStatus=@EmpStatus,EmpShiftStartDate=@EmpShiftStartDate,EmpJoiningDate=@EmpJoiningDate,EarnedLeaveEffectedFrom=@EarnedLeaveEffectedFrom where EmpId=" + ddlEmployeeCardNo.SelectedValue + "", sqlDB.connection);
                    cmd.Parameters.AddWithValue("@EmpStatus","1");
                    cmd.Parameters.AddWithValue("@EmpShiftStartDate", convertDateTime.getCertainCulture(txtEffectiveDate.Text.Trim()));
                    cmd.Parameters.AddWithValue("@EmpJoiningDate", convertDateTime.getCertainCulture(txtEffectiveDate.Text.Trim()));
                    cmd.Parameters.AddWithValue("@EarnedLeaveEffectedFrom", convertDateTime.getCertainCulture(txtEffectiveDate.Text.Trim()));
                    cmd.ExecuteNonQuery();

                    rdoEmpType.SelectedIndex = 0;
                    classes.Employee.LoadResignedEmpCardNo(ddlEmployeeCardNo, "1");
                    txtEffectiveDate.Text = "";
                    txtRemarks.Text = "";
                    rdoEmpType.SelectedIndex = 0;
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
        private void UpdateEmpRejoin()
        {
            try
            {

                SqlCommand cmd = new SqlCommand(" update Personnel_EmpRejoin set EffectiveDate=@EffectiveDate, Remarks=@Remarks where EmpRejoinId=@EmpRejoinId ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpRejoinId", hdnUpdate.Value.ToString());
                cmd.Parameters.AddWithValue("@EffectiveDate", convertDateTime.getCertainCulture(txtEffectiveDate.Text.Trim()));
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                int result=(int)cmd.ExecuteNonQuery();
                if (result > 0)
                {
                   
                    cmd = new SqlCommand("Update Personnel_EmployeeInfo set EmpShiftStartDate=@EmpShiftStartDate,EmpJoiningDate=@EmpJoiningDate,EarnedLeaveEffectedFrom=@EarnedLeaveEffectedFrom where EmpId=" + ddlEmployeeCardNo.SelectedValue + "", sqlDB.connection);
                    cmd.Parameters.AddWithValue("@EmpShiftStartDate", convertDateTime.getCertainCulture(txtEffectiveDate.Text.Trim()));
                    cmd.Parameters.AddWithValue("@EmpJoiningDate", convertDateTime.getCertainCulture(txtEffectiveDate.Text.Trim()));
                    cmd.Parameters.AddWithValue("@EarnedLeaveEffectedFrom", convertDateTime.getCertainCulture(txtEffectiveDate.Text.Trim()));
                    cmd.ExecuteNonQuery();
                    hdnbtnStage.Value = "";
                    hdnUpdate.Value = "";
                    txtEffectiveDate.Text = "";
                    txtRemarks.Text = "";
                    lblMessage.InnerText = "success->Successfully Updated";
                }
                else
                {
                    ddlEmployeeCardNo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private void loadRejonEmployeeDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select EmpRejoinId,EmpCardNo,EmpType,convert(varchar(11),EffectiveDate,105) as EffectiveDate, Remarks from v_Personnel_EmpRejoin", dt);

                string divInfo = "";
                int totalRows = dt.Rows.Count;

                divInfo = "<table class='display' >";
                divInfo += "<thead>";
                divInfo += "<tr>";             
                divInfo += "<th style='text-align:left'>Emp Card No</th>";
                divInfo += "<th style='text-align:left'>Emp Type</th>";
                divInfo += "<th class='date'>Effective Date</th>";
                divInfo += "<th>Remarks</th>";
                divInfo += "<th class='numeric' style='max-width:20px;'>Edit</th>";
                divInfo += "</tr>";
                divInfo += "</thead>";

                divInfo += "<tbody>";

                for (int x = 0; x < totalRows; x++)
                {
                    divInfo += "<tr id='r_" + dt.Rows[x]["EmpRejoinId"].ToString() + "'>";
                    divInfo += "<td>" + dt.Rows[x]["EmpCardNo"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["EmpType"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["EffectiveDate"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["Remarks"].ToString() + "</td>";

                    divInfo += "<td class='numeric'><img class='editImg'   src='/Images/datatable/edit.png' title='Edit' onclick='editRejoin(" + dt.Rows[x]["EmpRejoinId"].ToString() + " );'/></td>";

                    divInfo += "</tr>";
                }

                divInfo += "</tbody>";
                divInfo += "</table>";

                divrejoinEmployee.Controls.Add(new LiteralControl(divInfo));
            }
            catch { }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select EmpTypeId From Personnel_EmpRejoin where EmpRejoinId=" + hdnUpdate.Value.ToString() + "", dt);
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Update Personnel_EmpCurrentStatus set EmpStatus='4' where EmpCardNo='" + hdfCardNo.Value.ToString() + "' and EmpTypeId=" + dt.Rows[0]["EmpTypeId"].ToString() + " and SN=(Select MAX(SN) as SN From Personnel_EmpCurrentStatus where EmpCardNo='" + hdfCardNo.Value.ToString() + "' and EmpTypeId=" + dt.Rows[0]["EmpTypeId"].ToString() + ") ", sqlDB.connection);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("Update Personnel_EmployeeInfo set EmpStatus='4' where EmpCardNo='" + hdfCardNo.Value.ToString() + "' and EmpTypeId=" + dt.Rows[0]["EmpTypeId"].ToString() + "", sqlDB.connection);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("Delete From Personnel_EmpRejoin where EmpRejoinId=" + hdnUpdate.Value.ToString() + "", sqlDB.connection);
                int result = (int)cmd.ExecuteNonQuery();
                if (result > 0)
                {
                  

                    txtEffectiveDate.Text = "";
                    txtRemarks.Text = "";
                    rdoEmpType.SelectedIndex = 0;
                    classes.Employee.LoadResignedEmpCardNo(ddlEmployeeCardNo,"1");
                    loadRejonEmployeeDetails();
                    lblMessage.InnerText = "success->Successfully Deleted";
                }
            }
            catch { }
        }

        protected void rdoEmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoEmpType.SelectedIndex == 0)
            {
                classes.Employee.LoadResignedEmpCardNo(ddlEmployeeCardNo, "1");
            }
            else
            {
                classes.Employee.LoadResignedEmpCardNo(ddlEmployeeCardNo, "2");
            }
        }      

    }
}