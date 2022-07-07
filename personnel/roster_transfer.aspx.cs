using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
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
    public partial class roster_transfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            if (!IsPostBack)
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


                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();


                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "roster_transfer.aspx", ddlCompanyList, gvEmpList, btnSubmit);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue);

            }
            catch { }

        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                classes.commonTask.loadGroupByDepartment_Company(ddlGroupList, ddlCompanyList.SelectedValue, ddlDepartmentList.SelectedValue);
                classes.commonTask.LoadShiftForSMOperation(ddlNewShift, ddlCompanyList.SelectedValue, ddlDepartmentList.SelectedValue);

            }
            catch { }
        }

        protected void ddlCurrentShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            if (ddlDepartmentList.SelectedValue == "0")
            {
                gvEmpList.DataSource = null;
                gvEmpList.DataBind(); return;
            }
            divRecordMessage.Visible = false;
            if (!ViewState["__ReadAction__"].ToString().Equals("0"))
                gvEmpList.Visible = true;
            LoadAllEmployeeList();
          //  lblTotal.Text = gvEmpList.Rows.Count.ToString();
        }
        private void LoadAllEmployeeList()
        {
            try
            {
                DataTable dt = new DataTable();
                string[] Sft_Dpt_Id = ddlCurrentShift.SelectedValue.Split('|');

                sqlDB.fillDataTable("Select Distinct EmpId,EmpCardNo,EmpName,DsgName,SftName,EmpType,EntryDate,FId,Notes,Sftid,CAST(1 AS bit) AS Status  From v_ShiftTransferInfoDetails where DptId ='" + Sft_Dpt_Id[1] + "' AND  SftId=" + Sft_Dpt_Id[2] + " AND STID=" + Sft_Dpt_Id[0] + " AND CompanyId='" + ddlCompanyList.SelectedItem.Value + "' and GID='"+ddlGroupList.SelectedValue+"'  order by EmpCardNo", dt);
                gvEmpList.DataSource = dt;
                gvEmpList.DataBind();
                if (dt.Rows.Count == 0)
                {
                    divRecordMessage.Visible = true;
                    divRecordMessage.InnerText = " Shift is Empty!";
                }
                else
                {
                    divRecordMessage.Visible = false;
                    divRecordMessage.InnerText = " ";
                   
                }

            }
            catch
            { }
        }

        protected void ddlGroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvEmpList.DataSource = null;
            gvEmpList.DataBind();
            loadAssignedShiftList();
        }
        private void loadAssignedShiftList()
        {
            try
            {
            DataTable    dt = new DataTable();
            string Top = (chksftAll.Checked) ? "" : " Top(5) ";
                //if (!chkLoadAllShiftList.Checked) sqlDB.fillDataTable("select  top 50 STID,Convert(varchar,STId)+'|'+DptId+'|'+ CONVERT(varchar,sftId) as SftId_DptId, Format(TFromdate,'dd-MM-yyyy')+' | '+Format(TToDate,'dd-MM-yyyy')+' | '+SftName +' | '+GName as Title from v_ShiftTransferInfo_DepartmetnList  where STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' AND DptId='" + ddlDepartmentList.SelectedValue + "' AND GID='" + ddlGrouopList.SelectedValue.ToString() + "' order by STId Desc ", dt);
                //else
               sqlDB.fillDataTable("select " + Top + " STID,Convert(varchar,STId)+'|'+DptId+'|'+ CONVERT(varchar,sftId) as SftId_DptId, Format(TFromdate,'dd-MM-yyyy')+' | '+Format(TToDate,'dd-MM-yyyy')+' | '+SftName +' | '+GName as Title from v_ShiftTransferInfo_DepartmetnList  where STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' AND DptId='" + ddlDepartmentList.SelectedValue + "' AND GID='" + ddlGroupList.SelectedValue.ToString() + "' order by STId Desc ", dt);
                ddlCurrentShift.DataTextField = "Title";
                ddlCurrentShift.DataValueField = "SftId_DptId";
                ddlCurrentShift.DataSource = dt;
                ddlCurrentShift.DataBind();
                ddlCurrentShift.Items.Insert(0, new ListItem(" ", "0"));

                int i = 0;
                foreach (ListItem item in ddlCurrentShift.Items)
                {
                    if (i % 2 == 0) item.Attributes.Add("style", "color:green");
                    else item.Attributes.Add("style", "color:red");

                    i++;
                }
            }
            catch { }
        }

        protected void chkHeaderChosen_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                CheckBox chk;
                CheckBox chkHeader = (CheckBox)gvEmpList.HeaderRow.FindControl("chkHeaderChosen");
                if (chkHeader.Checked)
                {
                    foreach (GridViewRow gvr in gvEmpList.Rows)
                    {
                        chk = (CheckBox)gvr.Cells[2].FindControl("chkChosen");
                        chk.Checked = true;
                    }
                    lblSelectedRow.Text = "Selected Employee = " + gvEmpList.Rows.Count;
                }
                else
                {
                    foreach (GridViewRow gvr in gvEmpList.Rows)
                    {
                        chk = (CheckBox)gvr.Cells[2].FindControl("chkChosen");
                        chk.Checked = false;
                    }
                    lblSelectedRow.Text = "Selected Employee = 0";
                }
            }
            catch { }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            if (!InputValidationBasket()) return;      // for required validation
            if (!CheckAnyEmployeeIsSelected()) return; // for checking any employee is selected ?
            shiftTransfer();
            
        }
        private void shiftTransfer()
        {
            try
            {
                CheckBox chk = new CheckBox();
                bool result = false;
                DataTable dtSTId = new DataTable();              
                    if (!CheckAllreadyAssigned()) return;
                    if (!saveShiftTransfer()) return;
                    else
                    {
                        sqlDB.fillDataTable("select Max(STId) as STId from ShiftTransferInfo", dtSTId);
                    }
        

                int v;
                for (int i = 0; i < gvEmpList.Rows.Count; i++)
                {

                    v = i + 1;
                    ProgressBar1.Value = (100 * v / gvEmpList.Rows.Count);
                    System.Threading.Thread.Sleep(50);
                    chk = (CheckBox)gvEmpList.Rows[i].Cells[2].FindControl("chkChosen");
                    DropDownList ddl = (DropDownList)gvEmpList.Rows[i].Cells[2].FindControl("ddlFloorList");
                    TextBox txtNotes = (TextBox)gvEmpList.Rows[i].Cells[3].FindControl("txtRemaks");

                    if (chk.Checked) saveShiftTransferDetails(dtSTId.Rows[0]["STId"].ToString(), gvEmpList.DataKeys[i].Values[0].ToString(), ddl.SelectedValue, txtNotes.Text);

                }
                divRecordMessage.Visible = true;
                divRecordMessage.InnerText = "Successfully shift assigned in " + ddlNewShift.SelectedItem.Text + "";
                gvEmpList.Visible = false;
                AllClear();

            }
            catch { }

        }

        private bool saveShiftTransfer()
        {
            try
            {

                string[] getColumns = { "SftId", "TFromDate", "TToDate", "DptId", "CompanyId", "GID" };
                string[] getValues = {ddlNewShift.SelectedValue,convertDateTime.getCertainCulture(txtFromDate.Text.Trim()).ToString(),
                                     convertDateTime.getCertainCulture(txtToDate.Text.Trim()).ToString(),ddlDepartmentList.SelectedItem.Value.ToString(),ddlCompanyList.SelectedValue,ddlGroupList.SelectedItem.Value.ToString()};
                if (SQLOperation.forSaveValue("ShiftTransferInfo", getColumns, getValues, sqlDB.connection) == true) return true;
                else return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void saveShiftTransferDetails(string STId, string EmpId, string FId, string Notes)
        {
            try
            {

                string[] FromDates = txtFromDate.Text.Trim().Split('-');
                string[] ToDates = txtToDate.Text.Trim().Split('-');
                DateTime FromDate = new DateTime(int.Parse(FromDates[2]), int.Parse(FromDates[1]), int.Parse(FromDates[0]));
                DateTime ToDate = new DateTime(int.Parse(ToDates[2]), int.Parse(ToDates[1]), int.Parse(ToDates[0]));

                string[] GetDsgID_EmpTypeId_GId = DsgId_EmpTypeId(EmpId).Split('|');

                while (FromDate <= ToDate)
                {
                    FromDates = FromDate.ToString().Split('/');

                    FromDates[1] = (FromDates[1].Trim().Length == 1) ? "0" + FromDates[1] : FromDates[1];
                    FromDates[0] = (FromDates[0].Trim().Length == 1) ? "0" + FromDates[0] : FromDates[0];



                    string SDate = FromDates[1] + "-" + FromDates[0] + "-" + FromDates[2].Substring(0, 4);

                    SqlCommand cmd = new SqlCommand("delete from ShiftTransferInfoDetails where EmpId='" + EmpId + "' AND Format(SDate,'dd-MM-yyyy')='" + SDate + "' AND STId=" + STId + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();

                    string[] getColumns = { "STId", "SDate", "EmpId", "DptId", "DsgId", "EmpTypeId", "CompanyId", "IsWeekend", "GId", "FId", "Notes" };
                    string[] getValues = { STId,convertDateTime.getCertainCulture(SDate).ToString(),EmpId,
                                         ddlDepartmentList.SelectedValue,GetDsgID_EmpTypeId_GId[0],GetDsgID_EmpTypeId_GId[1],ddlCompanyList.SelectedValue,CheckIsOffDay(EmpId,SDate).ToString(),
                                         GetDsgID_EmpTypeId_GId[2],FId,Notes};
                    SQLOperation.forSaveValue("ShiftTransferInfoDetails", getColumns, getValues, sqlDB.connection);
                    FromDate = FromDate.AddDays(1);

                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }
        private string DsgId_EmpTypeId(string EmpId)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select DsgId,EmpTypeId,GID from Personnel_EmpCurrentStatus where SN = (Select Max (SN) from Personnel_EmpCurrentStatus where EmpId='" + EmpId + "' AND EmpStatus in ('1','8'))", dt);
                return dt.Rows[0]["DsgId"].ToString() + '|' + dt.Rows[0]["EmpTypeId"].ToString() + '|' + dt.Rows[0]["GId"].ToString();
            }
            catch { return null; }
        }
        private byte CheckIsOffDay(string EmpId, string HWDate)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select SL From tblHolydayWeekentEmployeeWiseDetails where EmpId='" + EmpId + "' AND Format(HWDate,'dd-MM-yyyy')='" + HWDate + "'", dt);
                if (dt.Rows.Count > 0) return 1;
                else return 0;
            }
            catch { return 0; }
        }
        void AllClear()
        {
            try
            {
                //  ddlDepartmentList.SelectedIndex = 0;
                ddlCurrentShift.SelectedIndex = 0;
                ddlNewShift.SelectedIndex = 0;
                lblTotalRow.Text = "";
                lblSelectedRow.Text = "";
                ProgressBar1.Value = 0;
                Response.Redirect("../personnel/roster_transfer.aspx");
            }
            catch { }
        }
        private bool CheckAllreadyAssigned()
        {
            try
            {

                DataTable dtSTInfo = new DataTable();

                string[] TFromDate = txtFromDate.Text.Trim().Split('-');
                string[] ToDate = txtToDate.Text.Trim().Split('-');
                TFromDate[0] = TFromDate[2] + "-" + TFromDate[1] + "-" + TFromDate[0];
                ToDate[0] = ToDate[2] + "-" + ToDate[1] + "-" + ToDate[0];

                sqlDB.fillDataTable("select Max(Format(SDate,'dd-MM-yyyy')) as SDate from v_ShiftTransferInfoDetails where DptId='" + ddlDepartmentList.SelectedItem.Value + "' AND SftId =" + ddlNewShift.SelectedItem.Value + " AND SDate >='" + TFromDate[0] + "' AND SDate <='" + ToDate[0] + "' AND GId='" + ddlGroupList.SelectedValue + "' ", dtSTInfo);
                if (dtSTInfo.Rows.Count > 0 && dtSTInfo.Rows[0]["SDate"].ToString().Trim() != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);﻿
                    lblErrorMessage.Text = "Already this shift are assigned for " + dtSTInfo.Rows[0]["SDate"].ToString() + System.Environment.NewLine + ".Now you can modify,for modify must be checkd shift modify option.";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }
        private bool CheckAnyEmployeeIsSelected()
        {
            try
            {
                CheckBox chkb = gvEmpList.HeaderRow.FindControl("chkHeaderChosen") as CheckBox;
                if (chkb.Checked) return true;
                else
                {
                    foreach (GridViewRow gr in gvEmpList.Rows)
                    {
                        if ((gr.FindControl("chkChosen") as CheckBox).Checked) return true;
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                lblErrorMessage.Text = "Any employee is not selected ! ";
                return false;
            }
            catch { return false; }
        }
        private bool InputValidationBasket()
        {
            try
            {
                if (txtFromDate.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "change me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select valid from date";
                    ddlNewShift.SelectedIndex = 0;
                    txtFromDate.Focus(); return false;
                }
                if (txtToDate.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select valid to date";
                    ddlNewShift.SelectedIndex = 0;
                    txtToDate.Focus();
                    return false;
                }
                if (ddlNewShift.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select New Shift!";
                    ddlNewShift.Focus(); return false;
                }
                return true;
            }
            catch { return false; }
        }

        protected void chksftAll_CheckedChanged(object sender, EventArgs e)
        {
            loadAssignedShiftList();
        }

      
    }
}