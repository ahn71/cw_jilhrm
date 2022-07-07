using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.attendance
{
    public partial class roster_remove_panel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                classes.commonTask.LoadBranch(ddlCompanyList);
                ddlCompanyList.SelectedIndex = 1;
                classes.RosteringLogic.loadDepartmentListByCompany(ddlDepartmentName, ddlCompanyList.SelectedValue);

            }
        }

        protected void ddlDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                classes.RosteringLogic.loadSubDepartment_ByDeparmemt(ddlDepartmentName.SelectedValue, ddlSubDepartmentName);
            }
            catch { }
        }

        protected void ddlSubDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                classes.RosteringLogic.LoadAssignedShiftList(ddlDepartmentName.SelectedValue, ddlSubDepartmentName.SelectedValue, ddlCompanyList.SelectedItem.Value, ddlShiftList);
            }
            catch { }
        }

        private void LoadRosteredEmployee(bool SearchingByDate)
        {
            try
            {
                DataTable dt = new DataTable();
                if (!SearchingByDate)
                sqlDB.fillDataTable("select distinct EmpId,EmpName,DsgName,GroupOrdering,GId,CustomOrdering  from  v_ShiftTransferInfoDetails where STId='" + ddlShiftList.SelectedValue + "' order by GroupOrdering,GId,CustomOrdering ", dt);
                else
                {
                    string [] Dates=txtRosterDate.Text.Split('-');
                    sqlDB.fillDataTable("select distinct EmpId,EmpName,DsgName,GroupOrdering,GId,CustomOrdering  from  v_ShiftTransferInfoDetails where STId='" + ddlShiftList.SelectedValue + "' AND SDate='"+Dates[2]+"-"+Dates[1]+"-"+Dates[0]+"' order by GroupOrdering,GId,CustomOrdering ", dt);
                }

                    
                gvEmpList.DataSource = dt;
                gvEmpList.DataBind();

                lblRowCount.Text = "Total : " + gvEmpList.Rows.Count.ToString();
            }
            catch { }
        }

        protected void ddlShiftList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRosteredEmployee(false);
        }

        protected void gvEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";

                
            }
        }

        protected void gvEmpList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Remove"))
                {
                    int rIndex=Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteAssignedRoster("", gvEmpList.DataKeys[rIndex].Value.ToString(), true, rIndex);
                }
                else if (e.CommandName.Equals("RemoveBySingleDate"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    TextBox txtAttDate = gvEmpList.Rows[rIndex].FindControl("txtAttDate") as TextBox;
                    if (txtAttDate.Text.Trim().Length < 10)
                    {
                        lblMessage.InnerText = "warning->Please select date to delete";
                        txtAttDate.Focus();
                        return;
                    }
                    string[] SDate = txtAttDate.Text.Trim().Split('-');
                    DeleteAssignedRoster(SDate[2]+"-"+SDate[1]+"-"+SDate[0], gvEmpList.DataKeys[rIndex].Value.ToString(), false,rIndex);
                }
            }
            catch { }
        }

        private void DeleteAssignedRoster(string SDate,string EmpId,bool ForFullRoster,int rIndex)
        {
            try
            {
                SqlCommand cmd;
                if (!ForFullRoster) cmd = new SqlCommand("delete from ShiftTransferInfoDetails where SDate='" + SDate + "' AND EmpId='" + EmpId + "'", sqlDB.connection);
                else cmd = new SqlCommand("delete from ShiftTransferInfoDetails where  EmpId='" + EmpId + "' AND STID='" + ddlShiftList.SelectedValue + "'", sqlDB.connection);
                int result =cmd.ExecuteNonQuery();
                if (result >0)
                {
                    if (ForFullRoster)
                    {
                        gvEmpList.Rows[rIndex].Visible = false;
                    }
                    lblMessage.InnerText = "success->Successfully Deleted";
                }

            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadRosteredEmployee(true);
        }

        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            LoadRosteredEmployee(false);
        }

        protected void btnDeleteFullRoster_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("delete from ShiftTransferInfo where STID='"+ddlShiftList.SelectedValue+"'",sqlDB.connection);
                cmd.ExecuteNonQuery();

                lblMessage.InnerText = "success->Successfully full roster deleted";
                classes.RosteringLogic.LoadAssignedShiftList(ddlDepartmentName.SelectedValue, ddlSubDepartmentName.SelectedValue, ddlCompanyList.SelectedItem.Value, ddlShiftList);
                gvEmpList.DataSource = null;
                gvEmpList.DataBind();
            }
            catch { }
        }
    }
}