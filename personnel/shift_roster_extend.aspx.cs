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
    public partial class shift_roster_extend : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";

            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = 100;
            ProgressBar1.BackColor = System.Drawing.Color.Blue;
            ProgressBar1.ForeColor = Color.White;
            ProgressBar1.Height = new Unit(20);
            ProgressBar1.Width = new Unit(500);
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "FloorAssigne.aspx", ddlCompanyList, gvEmpList, btnExtend);

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
                classes.commonTask.loadGroupByDepartment_Company(ddlGrouopList, ddlCompanyList.SelectedValue, ddlDepartmentList.SelectedValue);
               
            }
            catch { }
        }


        private void loadAssignedShiftList()
        {
            try
            {
                dt = new DataTable();
                if (!chkLoadAllShiftList.Checked) sqlDB.fillDataTable("select  top 50 STID,Convert(varchar,STId)+'|'+DptId+'|'+ CONVERT(varchar,sftId) as SftId_DptId, Format(TFromdate,'dd-MM-yyyy')+' | '+Format(TToDate,'dd-MM-yyyy')+' | '+SftName +' | '+GName as Title from v_ShiftTransferInfo_DepartmetnList  where STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' AND DptId='" + ddlDepartmentList.SelectedValue + "' AND GID='"+ddlGrouopList.SelectedValue.ToString()+"' order by STId Desc ", dt);
                else sqlDB.fillDataTable("select STID,Convert(varchar,STId)+'|'+DptId+'|'+ CONVERT(varchar,sftId) as SftId_DptId, Format(TFromdate,'dd-MM-yyyy')+' | '+Format(TToDate,'dd-MM-yyyy')+' | '+SftName +' | '+GName as Title from v_ShiftTransferInfo_DepartmetnList  where STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' AND DptId='" + ddlDepartmentList.SelectedValue + "' AND GID='" + ddlGrouopList.SelectedValue.ToString() + "' order by STId Desc ", dt);
                ddlAssignShift.DataTextField = "Title";
                ddlAssignShift.DataValueField = "SftId_DptId";
                ddlAssignShift.DataSource = dt;
                ddlAssignShift.DataBind();
                ddlAssignShift.Items.Insert(0, new ListItem(" ", "0"));

                int i = 0;
                foreach (ListItem item in ddlAssignShift.Items)
                {
                    if (i % 2 == 0) item.Attributes.Add("style", "color:green");
                    else item.Attributes.Add("style", "color:red");

                    i++;
                }
            }
            catch { }
        }

        protected void ddlAssignShift_SelectedIndexChanged(object sender, EventArgs e)
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
            lblTotal.Text = gvEmpList.Rows.Count.ToString();

        }

        private void LoadAllEmployeeList()
        {
            try
            {
                DataTable dt = new DataTable();
                string[] Sft_Dpt_Id = ddlAssignShift.SelectedValue.Split('|');

                sqlDB.fillDataTable("Select Distinct EmpId,EmpCardNo,EmpName,DsgName,SftName,EmpType,STId,FName,Notes,DsgId,EmpTypeId,GId,FId From v_ShiftTransferInfoDetails where DptId ='" + Sft_Dpt_Id[1] + "' AND  SftId=" + Sft_Dpt_Id[2] + " AND STID=" + Sft_Dpt_Id[0] + " AND CompanyId='" + ddlCompanyList.SelectedItem.Value + "'  order by EmpCardNo", dt);
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
                    //  divRecordMessage.Visible = false; gvEmpList.Visible = true; lblTotalRow.Text = "Total Employee = " + dt.Rows.Count.ToString();
                    // lblSelectedRow.Text = "Selected Employee = " + dt.Rows.Count.ToString();
                }

            }
            catch
            { }
        }

        protected void gvEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
            }
        }

        protected void chkLoadAllShiftList_CheckedChanged(object sender, EventArgs e)
        {
            loadAssignedShiftList();
        }

        protected void btnExtend_Click(object sender, EventArgs e)
        {
            if (rbRosterType.SelectedIndex==-1)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                lblErrorMessage.Text = "Please select roster type ."; 
                rbRosterType.Focus(); return;
            }
            else if (txtDate.Text.Trim().Length<10)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                lblErrorMessage.Text = "Please select extend date."; return;
            }

            string[] Dates = ddlAssignShift.SelectedItem.Text.Split('|');
            
            Extend_Schedule(Dates[1]);
            if (gvEmpList.Rows.Count > 0)
            {
               // divRecordMessage.Visible = true;
                
               // gvEmpList.Visible = false;
                ProgressBar1.Value = 0;

                if (rbRosterType.SelectedValue.ToString() == "Extend")
                {
                    string SelectRoste=ddlAssignShift.SelectedValue;
                    string[] getColumns = { "TToDate" };
                    string[] getValues = { convertDateTime.getCertainCulture(txtDate.Text).ToString() };
                    SQLOperation.forUpdateValue("ShiftTransferInfo", getColumns, getValues, "STID", gvEmpList.DataKeys[0].Values[0].ToString(), sqlDB.connection);
                    divRecordMessage.InnerText = "Successfully roster " + ddlAssignShift.SelectedItem + " extend  "+Dates[0]+" to " + txtDate.Text;
                    lblErrorMessage.Text = "Successfully Roster Extend";
                   
                    loadAssignedShiftList();
                    ddlAssignShift.SelectedValue = SelectRoste;
                    LoadAllEmployeeList();
                    
                }
                else
                {

                    divRecordMessage.InnerText = "Successfully Shift Roster Created of " + Dates[2]+ " from " + txtDate.Text+" to "+txtDate.Text;
                    lblErrorMessage.Text = "Successfully Roster Created";
                   
                    loadAssignedShiftList();
                    ddlAssignShift.SelectedIndex = 1; // new shift all time stay in top
                    LoadAllEmployeeList();
                }
                
            }
            
        }
     
        private void Extend_Schedule(string fromDate)
        {
            try
            {
                //-------------------------------------------------------------------------------------------------------------------
                if (rbRosterType.SelectedItem.Value.Equals("Create"))
                {
                    if (!saveShiftTransfer())
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                        lblErrorMessage.Text = "Sorry new shift roster not created.";
                        return;
                    }
                    else
                    {
                        sqlDB.fillDataTable("select Max(STId) as STId from ShiftTransferInfo", dt = new DataTable());
                        ViewState["__STID__"] = dt.Rows[0]["STId"].ToString();
                        fromDate = txtDate.Text;
                    }
                }
                else
                {
                    ViewState["__STID__"] = gvEmpList.DataKeys[0].Values[0].ToString();  // reason STID are same for all employee of roster
                }
                //-------------------------------------------------------------------------------------------------------------------

                int v;
                for (int i = 0; i < gvEmpList.Rows.Count; i++)
                {
                    string[] FromDates =  fromDate.Trim().Split('-');
                    string[] ToDates = txtDate.Text.Trim().Split('-');
                    DateTime FromDate = new DateTime(int.Parse(FromDates[2]), int.Parse(FromDates[1]), int.Parse(FromDates[0]));
                    DateTime ToDate = new DateTime(int.Parse(ToDates[2]), int.Parse(ToDates[1]), int.Parse(ToDates[0]));

                    if (!rbRosterType.SelectedItem.Value.Equals("Create")) FromDate = FromDate.AddDays(1);

                    v = i + 1;
                    ProgressBar1.Value = (100 * v / gvEmpList.Rows.Count);

                    string Notes = (gvEmpList.Rows[i].Cells[5].Text.Trim() == "" || gvEmpList.Rows[i].Cells[5].Text.Trim() == "&nbsp;") ? "" : gvEmpList.Rows[i].Cells[5].Text.Trim();
                   
                    while (FromDate <= ToDate)
                    {
                        FromDates = FromDate.ToString().Split('/');

                        FromDates[1] = (FromDates[1].Trim().Length == 1) ? "0" + FromDates[1] : FromDates[1];
                        FromDates[0] = (FromDates[0].Trim().Length == 1) ? "0" + FromDates[0] : FromDates[0];



                        string SDate = FromDates[1] + "-" + FromDates[0] + "-" + FromDates[2].Substring(0, 4);

                        SqlCommand cmd = new SqlCommand("delete from ShiftTransferInfoDetails where EmpId='" + gvEmpList.DataKeys[i].Values[1].ToString() + "' AND Format(SDate,'dd-MM-yyyy')='" + SDate + "' AND STId=" + gvEmpList.DataKeys[i].Values[0].ToString() + "", sqlDB.connection);
                        cmd.ExecuteNonQuery();
                       
                        if (gvEmpList.DataKeys[i].Values[5].ToString().Trim().Length != 0)
                        {
                            string[] getColumns = { "STId", "SDate", "EmpId", "DptId", "DsgId", "EmpTypeId", "CompanyId", "IsWeekend", "GId", "FId", "Notes" };
                            string[] getValues = { ViewState["__STID__"].ToString(),convertDateTime.getCertainCulture(SDate).ToString(),gvEmpList.DataKeys[i].Values[1].ToString(),
                                         ddlDepartmentList.SelectedValue,gvEmpList.DataKeys[i].Values[2].ToString(),gvEmpList.DataKeys[i].Values[3].ToString(),ddlCompanyList.SelectedValue,CheckIsOffDay(gvEmpList.DataKeys[i].Values[1].ToString(),SDate).ToString(),
                                         gvEmpList.DataKeys[i].Values[4].ToString(),gvEmpList.DataKeys[i].Values[5].ToString(),Notes};
                            SQLOperation.forSaveValue("ShiftTransferInfoDetails", getColumns, getValues, sqlDB.connection);
                        }
                        else
                        {
                            string[] getColumns = { "STId", "SDate", "EmpId", "DptId", "DsgId", "EmpTypeId", "CompanyId", "IsWeekend", "GId","Notes" };
                            string[] getValues = { ViewState["__STID__"].ToString(),convertDateTime.getCertainCulture(SDate).ToString(),gvEmpList.DataKeys[i].Values[1].ToString(),
                                         ddlDepartmentList.SelectedValue,gvEmpList.DataKeys[i].Values[2].ToString(),gvEmpList.DataKeys[i].Values[3].ToString(),ddlCompanyList.SelectedValue,CheckIsOffDay(gvEmpList.DataKeys[i].Values[1].ToString(),SDate).ToString(),
                                         gvEmpList.DataKeys[i].Values[4].ToString(),Notes};
                            SQLOperation.forSaveValue("ShiftTransferInfoDetails", getColumns, getValues, sqlDB.connection);
                        }
                        FromDate = FromDate.AddDays(1);

                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        private bool saveShiftTransfer()
        {
            try
            {
                string[] getSTD = ddlAssignShift.SelectedValue.ToString().Split('|');
                sqlDB.fillDataTable("select SftId from ShiftTransferInfo where STID=" + getSTD [0]+ "", dt = new DataTable());
                string[] getColumns = { "SftId", "TFromDate", "TToDate", "DptId", "CompanyId", "GId" };
                string[] getValues = {dt.Rows[0]["SftId"].ToString(),convertDateTime.getCertainCulture(txtDate.Text.Trim()).ToString(),
                                     convertDateTime.getCertainCulture(txtDate.Text.Trim()).ToString(),ddlDepartmentList.SelectedItem.Value.ToString(),ddlCompanyList.SelectedValue,ddlGrouopList.SelectedValue};
                if (SQLOperation.forSaveValue("ShiftTransferInfo", getColumns, getValues, sqlDB.connection) == true) return true;
                else return false;


            }
            catch (Exception ex)
            {
                return false;
            }
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

        protected void ddlGrouopList_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvEmpList.DataSource = null;
            gvEmpList.DataBind();
            loadAssignedShiftList();
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue;
            classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, CompanyId);
        }
    }


}