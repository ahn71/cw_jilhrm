using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class ShiftManageRemove : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";

            if(!IsPostBack)
            {

                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
            }
        }

        private void setPrivilege()
        {
            try
            {
                
                ViewState["__DeleteAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                string[] AccessPermission = new string[0];
                
AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyDeleteAction(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "ShiftManageRemove.aspx",ddlCompanyList,gvEmpList,btnDelete);

                ViewState["__ReadAction__"] = AccessPermission[0];             
                ViewState["__DeletAction__"] = AccessPermission[3];                                
                
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                loadRecentlyTransferShiftDepartment(false);     
            }
            catch { }

        }
        private void loadRecentlyTransferShiftDepartment(bool AllShiftRoster)
        {
            try
            {
                dt = new DataTable();
                if (!AllShiftRoster) sqlDB.fillDataTable("select  top 20 STID,Convert(varchar,STId)+'|'+DptId+'|'+ CONVERT(varchar,sftId) as SftId_DptId,dptName+' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) '+SftName +' | '+ GName as Title from v_ShiftTransferInfo_DepartmetnList  where STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' order by STId Desc ", dt);
                else sqlDB.fillDataTable("select STID,Convert(varchar,STId)+'|'+DptId+'|'+ CONVERT(varchar,sftId) as SftId_DptId,dptName+' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) '+SftName +' | '+ GName as Title from v_ShiftTransferInfo_DepartmetnList  where STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' order by STId Desc ", dt);
                ddlDepartmentList.DataTextField = "Title";
                ddlDepartmentList.DataValueField = "SftId_DptId";
                ddlDepartmentList.DataSource = dt;
                ddlDepartmentList.DataBind();
                ddlDepartmentList.Items.Insert(0, new ListItem(" ", "0"));
            }
            catch { }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);  
                if (ddlDepartmentList.SelectedValue == "0")
                {
                    lblMessage.InnerText = "error->Please select a department item";
                    return;
                }
                string[] Sft_Dpt_Id = ddlDepartmentList.SelectedValue.Split('|');
                SqlCommand cmd = new SqlCommand("delete from ShiftTransferInfo where STId=" + Sft_Dpt_Id[0].Trim() + " AND DptId='"+Sft_Dpt_Id[1]+"' AND CompanyId='"+ddlCompanyList.SelectedValue+"' ", sqlDB.connection);
                cmd.ExecuteNonQuery();
                lblMessage.InnerText = "success->Successfully Deleted";
                LoadAllEmployeeList(false);
                if (!chkLoadAllShiftList.Checked) loadRecentlyTransferShiftDepartment(false);
                else loadRecentlyTransferShiftDepartment(true);
            }
            catch { }
        }

        //private void LoadAllEmployeeList()
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        string[] Sft_Dpt_Id = ddlDepartmentList.SelectedValue.Split('|');

        //        sqlDB.fillDataTable("Select Distinct EmpId,EmpCardNo,EmpName,DptName,SftName,EmpType,STId From v_ShiftTransferInfoDetails where DptId ='" + Sft_Dpt_Id[1] + "' AND  SftId=" + Sft_Dpt_Id[2] + " AND STID="+Sft_Dpt_Id[0]+" AND CompanyId='"+ddlCompanyList.SelectedItem.Value+"'  order by EmpCardNo", dt);
        //        gvEmpList.DataSource = dt;
        //        gvEmpList.DataBind();
        //        if (dt.Rows.Count == 0)
        //        {
        //            divRecordMessage.Visible = true;
        //            divRecordMessage.InnerText = " Shift is Empty!";
        //        }
        //        else
        //        {
        //            divRecordMessage.Visible = false;
        //            divRecordMessage.InnerText = " ";
        //          //  divRecordMessage.Visible = false; gvEmpList.Visible = true; lblTotalRow.Text = "Total Employee = " + dt.Rows.Count.ToString();
        //           // lblSelectedRow.Text = "Selected Employee = " + dt.Rows.Count.ToString();
        //        }

        //    }
        //    catch
        //    { }
        //}

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);  
            if (ddlDepartmentList.SelectedValue=="0")
            {
                gvEmpList.DataSource = null;
                gvEmpList.DataBind(); return;
            }
            LoadAllEmployeeList(false);
           
        }

        protected void gvEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);  
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                }
            }
            catch { }
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                        Button btnDeleteBySingleDate = (Button)e.Row.FindControl("btnDeleteBySingleDate");
                        btnDelete.Enabled = false;
                        btnDelete.OnClientClick = "return false";
                        btnDelete.ForeColor = Color.Silver;

                        btnDeleteBySingleDate.Enabled = false;
                        btnDeleteBySingleDate.OnClientClick = "return false";
                        btnDeleteBySingleDate.ForeColor = Color.Silver;
                    }

                }
                catch { }
               
            }
        }

        protected void gvEmpList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //try
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);  
            //    if (e.CommandName=="Remove")
            //    {
            //        string[] Sft_Dpt_Id = ddlDepartmentList.SelectedValue.Split('|');
            //        int rIndex=Convert.ToInt32(e.CommandArgument.ToString());

            //        SqlCommand cmd = new SqlCommand("delete from ShiftTransferInfoDetails where EmpId='"+gvEmpList.DataKeys[rIndex].Value.ToString()+"' AND STId='"+Sft_Dpt_Id[0].Trim()+"' AND DptId='"+Sft_Dpt_Id[1].Trim()+"'",sqlDB.connection);
            //        cmd.ExecuteNonQuery();
            //        gvEmpList.Rows[rIndex].Visible = false;
            //        lblMessage.InnerText = "success->Successfully removed !";
            //        lblTotal.Text = (gvEmpList.Rows.Count-1).ToString();
            //    }

            //}
            //catch { }



            try
            {
                if (e.CommandName.Equals("Remove"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
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
                    DeleteAssignedRoster(SDate[2] + "-" + SDate[1] + "-" + SDate[0], gvEmpList.DataKeys[rIndex].Value.ToString(), false, rIndex);
                }
            }
            catch { }
        }

        protected void gvEmpList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.LoadBranch(ddlCompanyList);
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!chkLoadAllShiftList.Checked) loadRecentlyTransferShiftDepartment(false);
            else loadRecentlyTransferShiftDepartment(true);
        }

        protected void chkLoadAllShiftList_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkLoadAllShiftList.Checked) loadRecentlyTransferShiftDepartment(false);
            else loadRecentlyTransferShiftDepartment(true);
        }
        //-------------------------------------------------------------------
        //private void LoadRosteredEmployee(bool SearchingByDate)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        if (!SearchingByDate)
        //            sqlDB.fillDataTable("select distinct EmpId,EmpName,DsgName,GroupOrdering,GId,CustomOrdering  from  v_ShiftTransferInfoDetails where STId='" + ddlShiftList.SelectedValue + "' order by GroupOrdering,GId,CustomOrdering ", dt);
        //        else
        //        {
        //            string[] Dates = txtRosterDate.Text.Split('-');
        //            sqlDB.fillDataTable("select distinct EmpId,EmpName,DsgName,GroupOrdering,GId,CustomOrdering  from  v_ShiftTransferInfoDetails where STId='" + ddlShiftList.SelectedValue + "' AND SDate='" + Dates[2] + "-" + Dates[1] + "-" + Dates[0] + "' order by GroupOrdering,GId,CustomOrdering ", dt);
        //        }


        //        gvEmpList.DataSource = dt;
        //        gvEmpList.DataBind();

        //        lblRowCount.Text = "Total : " + gvEmpList.Rows.Count.ToString();
        //    }
        //    catch { }
        //}


        private void LoadAllEmployeeList(bool SearchingByDate)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] Sft_Dpt_Id = ddlDepartmentList.SelectedValue.Split('|');
                if (!SearchingByDate)
                    sqlDB.fillDataTable("Select Distinct EmpId,EmpCardNo,EmpName,DptName,SftName,EmpType,STId From v_ShiftTransferInfoDetails where DptId ='" + Sft_Dpt_Id[1] + "' AND  SftId=" + Sft_Dpt_Id[2] + " AND STID=" + Sft_Dpt_Id[0] + " AND CompanyId='" + ddlCompanyList.SelectedItem.Value + "'  order by EmpCardNo", dt);
                else 
                {
                    string[] Dates = txtRosterDate.Text.Split('-');
                    sqlDB.fillDataTable("Select Distinct EmpId,EmpCardNo,EmpName,DptName,SftName,EmpType,STId From v_ShiftTransferInfoDetails where DptId ='" + Sft_Dpt_Id[1] + "' AND  SftId=" + Sft_Dpt_Id[2] + " AND STID=" + Sft_Dpt_Id[0] + " AND CompanyId='" + ddlCompanyList.SelectedItem.Value + "' AND SDate='" + Dates[2] + "-" + Dates[1] + "-" + Dates[0] + "' order by EmpCardNo", dt);
                }
                gvEmpList.DataSource = dt;
                gvEmpList.DataBind();
                lblTotal.Text = gvEmpList.Rows.Count.ToString();
                if (dt.Rows.Count == 0)
                {
                    divRecordMessage.Visible = true;
                    divRecordMessage.InnerText = " Shift is Empty!";
                }
                else
                {
                    divRecordMessage.Visible = false;
                    divRecordMessage.InnerText = " ";
                    //  divRecordMessage.Visible = false; gvEmpList.Visible = true; lblTotalRow.Text = "Total Employee = " + dt.Rows.Count.ToString();
                    // lblSelectedRow.Text = "Selected Employee = " + dt.Rows.Count.ToString();
                }

            }
            catch
            { }
        }
        private void DeleteAssignedRoster(string SDate, string EmpId, bool ForFullRoster, int rIndex)
        {
            try
            {
                SqlCommand cmd;
                string[] Sft_Dpt_Id = ddlDepartmentList.SelectedValue.Split('|');
               //SqlCommand cmd = new SqlCommand("delete from ShiftTransferInfoDetails where EmpId='" + gvEmpList.DataKeys[rIndex].Value.ToString() + "' AND STId='" + Sft_Dpt_Id[0].Trim() + "' AND DptId='" + Sft_Dpt_Id[1].Trim() + "'", sqlDB.connection);
                     
                if (!ForFullRoster) cmd = new SqlCommand("delete from ShiftTransferInfoDetails where SDate='" + SDate + "' AND EmpId='" + EmpId + "'", sqlDB.connection);
                else cmd = new SqlCommand("delete from ShiftTransferInfoDetails where  EmpId='" + EmpId + "'  AND STId='" + Sft_Dpt_Id[0].Trim() + "' AND DptId='" + Sft_Dpt_Id[1].Trim() + "'", sqlDB.connection);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
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
            if (txtRosterDate.Text.Trim().Length < 8)
            {
                lblMessage.InnerText = "warning-> Please select a valid date !"; return;
            }
            LoadAllEmployeeList(true);
        }
        //-------------------------------------------------------------------

        
    }
}