using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace SigmaERP.personnel
{
    public partial class punishment : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            if (!IsPostBack)
            {
                loadPunismentInfo();
                classes.commonTask.loadEmpCardNo(ddlEmpCardNo);
                classes.Employee.loadEmpPunismentType(ddlPunismentType);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            if (hfSaveStatus.Value.ToString().Equals("Save")) savePunisment();
            else updatePunisment();
        }

        public void savePunisment()
        {
            try
            {
                if (PunishmentValidation() == false)
                {
                    loadPunismentInfo();
                    return;
                }

                SQLOperation.selectBySetCommandInDatatable("select EmpId from Personnel_EmployeeInfo where EmpCardNo=" + txtEmpCardNo.Text.Trim() + "", dt = new DataTable(), sqlDB.connection);

                SqlCommand cmd = new SqlCommand("insert into Personnel_EmpPunismentInfo (EmpId,EmpCardNo,EmpPunismentOrderRef,EmpPunismentOrderRefDate,PtId,EmpPunismentAmount,EmpRemarks,EmpPunismentStaus) values (@EmpId,@EmpCardNo,@EmpPunismentOrderRef,@EmpPunismentOrderRefDate,@PtId,@EmpPunismentAmount,@EmpRemarks,@EmpPunismentStaus) ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", dt.Rows[0]["EmpId"].ToString());
                cmd.Parameters.AddWithValue("@EmpCardNo",txtEmpCardNo.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpPunismentOrderRef", txtPunismetnOrderRef.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpPunismentOrderRefDate", convertDateTime.getCertainCulture(txtPunismetnOrderRefDate.Text));
                cmd.Parameters.AddWithValue("@PtId", ddlPunismentType.SelectedValue);
                cmd.Parameters.AddWithValue("@EmpPunismentAmount", txtAmount.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpRemarks", txtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpPunismentStaus", "1");
                int result = cmd.ExecuteNonQuery();
                if (result==1)
                {
                    lblMessage.InnerText = "success->Successfully Saved";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    loadPunismentInfo();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }

        public void updatePunisment()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("update  Personnel_EmpPunismentInfo set EmpPunismentOrderRef=@EmpPunismentOrderRef,EmpPunismentOrderRefDate=@EmpPunismentOrderRefDate,EmpPunismentAmount=@EmpPunismentAmount,EmpRemarks=@EmpRemarks,EmpPunismentStaus=@EmpPunismentStaus where EmpPunismentId="+hfPunismentId.Value+"",sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpPunismentOrderRef",txtPunismetnOrderRef.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpPunismentOrderRefDate", convertDateTime.getCertainCulture(txtPunismetnOrderRefDate.Text));
                cmd.Parameters.AddWithValue("@EmpPunismentAmount",txtAmount.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpRemarks",txtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@EmpPunismentStaus","1");
                int result = cmd.ExecuteNonQuery();
                if (result==1)
                {
                    lblMessage.InnerText = "success->Successfully Updated";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    loadPunismentInfo();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "warning->"+ex.Message;
            }
        }
        private Boolean PunishmentValidation()
        {
            try
            {
               
                sqlDB.fillDataTable("Select EmpPunismentStaus From Personnel_EmpPunismentInfo where EmpCardNo="+txtEmpCardNo.Text+" and PtId="+ddlPunismentType.SelectedValue+"", dt = new DataTable());
                if (dt.Rows.Count == 0) return true;
                if (dt.Rows[0]["EmpPunismentStaus"].ToString() == "True")
                {
                    lblMessage.InnerText = "warning->Already "+ddlPunismentType.SelectedItem.Text+" Punishment Continue";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        private void loadPunismentInfo()
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select EmpPunismentId,EmpId,EmpCardNo,EmpPunismentOrderRef,convert(varchar(11),EmpPunismentOrderRefDate,105) as EmpPunismentOrderRefDate,PtName  from v_Personnel_EmpPunismentInfo where EmpPunismentStaus='true'", dt = new DataTable(), sqlDB.connection);

                int totalRows = dt.Rows.Count;
                string divInfo = "";

                if (totalRows == 0)
                {
                    divInfo = "<div class='noData'>Punisment not assigne </div>";
                    divInfo += "<div class='dataTables_wrapper'><div class='head'></div></div>";
                    divPunismentList.Controls.Add(new LiteralControl(divInfo));
                    return;
                }

                divInfo = " <table id='tblDesignationList' class='display'  > ";
                divInfo += "<thead>";
                divInfo += "<tr>";
                divInfo += "<th>Card No</th>";
                divInfo += "<th>Order Reference</th>";
                divInfo += "<th>Order Reference Date</th>";
                divInfo += "<th>Punisment Type </th>";
                divInfo += "<th>Edit</th>";
                divInfo += "</tr>";

                divInfo += "</thead>";

                divInfo += "<tbody>";
                string id = "";

                for (int x = 0; x < dt.Rows.Count; x++)
                {

                    id = dt.Rows[x]["EmpPunismentId"].ToString();
                    divInfo += "<tr id='r_" + id + "'>";
                    divInfo += "<td>" + dt.Rows[x]["EmpCardNo"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["EmpPunismentOrderRef"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["EmpPunismentOrderRefDate"].ToString() + "</td>";
                    divInfo += "<td>" + dt.Rows[x]["PtName"].ToString() + "</td>";
                    divInfo += "<td class='numeric_control' >" + "<img src='/Images/gridImages/edit.png' class='editImg'   onclick='editFeesType(" + id + ");'  />";
                }

                divInfo += "</tbody>";
                divInfo += "<tfoot>";

                divInfo += "</table>";
                divInfo += "<div class='dataTables_wrapper'><div class='head'></div></div>";

                divPunismentList.Controls.Add(new LiteralControl(divInfo));

            }
            catch { }
        }

       
    }
}