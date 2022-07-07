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
    public partial class shift_roster_remove_single_date_employee : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
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

                ViewState["__DeleteAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                //SetUserType();
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                {

                    DataTable dt = new DataTable();

                    sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='ShiftManageRemove.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                        {
                            ViewState["__DeleteAction__"] = "0";
                            // btnDelete.Enabled = false;
                            // btnDelete.CssClass = "";
                        }
                    }
                    else
                    {
                        ViewState["__DeleteAction__"] = "0";
                        // btnDelete.Enabled = false;
                        // btnDelete.CssClass = "";
                        return;
                    }
                }
                classes.commonTask.LoadBranch(ddlCompanyList);
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue);
                LoadFloorList();
            }
            catch { }

        }

        private void LoadFloorList()
        {
            try
            {
                sqlDB.fillDataTable("select FId,FName from HRD_Floor where IsActive='True' order by FId", dt=new DataTable ());
                ddlFloorList.DataValueField = "FId";
                ddlFloorList.DataTextField = "FName";
                ddlFloorList.DataSource = dt;
                ddlFloorList.DataBind();
                ddlFloorList.Items.Insert(0,new ListItem ("","0"));
            }catch{}
        }
        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            foreach (ListItem item in ddlDepartmentList.Items)
            {
                if (i % 2 == 0) item.Attributes.Add("style", "color:green");
                else item.Attributes.Add("style", "color:red");

                i++;
            }

            loadAssignedShiftList();

        }
        private void loadAssignedShiftList()
        {
            try
            {
                dt = new DataTable();
                if (!chkLoadAllShiftList.Checked) sqlDB.fillDataTable("select  top 50 STID,Convert(varchar,STId)+'|'+DptId+'|'+ CONVERT(varchar,sftId) as SftId_DptId, Format(TFromdate,'dd-MM-yyyy')+' | '+Format(TToDate,'dd-MM-yyyy')+' | '+SftName +' | '+ GName as Title from v_ShiftTransferInfo_DepartmetnList  where STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' AND DptId='" + ddlDepartmentList.SelectedValue + "' order by STId Desc ", dt);
                else sqlDB.fillDataTable("select STID,Convert(varchar,STId)+'|'+DptId+'|'+ CONVERT(varchar,sftId) as SftId_DptId, Format(TFromdate,'dd-MM-yyyy')+' | '+Format(TToDate,'dd-MM-yyyy')+' | '+SftName +' | '+ GName as Title from v_ShiftTransferInfo_DepartmetnList  where STId !='1' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' AND DptId='" + ddlDepartmentList.SelectedValue + "' order by STId Desc ", dt);
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
            int i=0;
            foreach (ListItem item in ddlAssignShift.Items)
            {
                if (i % 2 == 0) item.Attributes.Add("style", "color:green");
                else item.Attributes.Add("style", "color:red");

                i++;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

           if (!ValidationBasket()) return;
           if (!checkDateRange()) return;
           if (!EmpVerifacation()) return;
           if (Validation_EmployeeIsAvailable())
           {
              if (AddEmployeeInRoster())
              {
                  lblMessage.InnerText = "success->Successfully entered in this roster";
                  ddlFloorList.SelectedIndex = 0;
                  txtNotes.Text = "";
                  btnSearch_Click(sender,e);
              }
           }
        }

        bool checkDateRange()
        {
            try
            {
                string[] infos = ddlAssignShift.SelectedItem.Text.Split('|');

                string[] Dates = infos[0].Split('-');
                DateTime FDate = new DateTime(int.Parse(Dates[2]), int.Parse(Dates[1]), int.Parse(Dates[0]));
                Dates = infos[1].Split('-');
                DateTime TDate = new DateTime(int.Parse(Dates[2]), int.Parse(Dates[1]), int.Parse(Dates[0]));

                Dates=txtDate.Text.Split('-');
                DateTime SDate = new DateTime(int.Parse(Dates[2]), int.Parse(Dates[1]), int.Parse(Dates[0]));

                if (SDate>TDate || SDate<FDate)
                {
                    lblMessage.InnerText = "error->Please choose date between of roster date range ";
                    txtDate.Focus();
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        bool Validation_EmployeeIsAvailable()
        {
            try
            {
                string[] infos = ddlAssignShift.SelectedItem.Value.ToString().Split('|');
                ViewState["__STId__"] = infos[0];
                sqlDB.fillDataTable("select SL,EmpName,SftName from v_ShiftTransferInfoDetails where EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' AND Format(SDate,'dd-MM-yyyy') ='" + txtDate.Text + "' AND STId=" + infos [0]+ "", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    lblMessage.InnerText = "error->Sorry " + dt.Rows[0]["EmpName"].ToString() + " is assigned in " + dt.Rows[0]["SftName"].ToString() + " of this date.So,Please select other roster of this date ";
                    return false;
                }
                else return true;
            }
            catch { return false; }
        }

        bool AddEmployeeInRoster()
        {
            try
            {
                if (ddlFloorList.SelectedValue != "0")
                {
                    string[] getColumns = { "STId", "SDate", "EmpId", "DptId", "DsgId", "EmpTypeId", "CompanyId", "IsWeekend", "GId", "FId", "Notes" };
                    string[] getValues = { ViewState["__STId__"].ToString(),convertDateTime.getCertainCulture(txtDate.Text).ToString(),ViewState["__EmpId__"].ToString(),ddlDepartmentList.SelectedItem.Value,
                                          ViewState["__DsgId__"].ToString(),ViewState["__EmpTypeId"].ToString(),ddlCompanyList.SelectedValue,CheckIsOffDay(ViewState["__EmpId__"].ToString(),txtDate.Text).ToString(),
                                         ViewState["__GId__"].ToString(),ddlFloorList.SelectedItem.Value,txtNotes.Text};
                    if (SQLOperation.forSaveValue("ShiftTransferInfoDetails", getColumns, getValues, sqlDB.connection)) return true ;
                }
                else
                {
                    string[] getColumns = { "STId", "SDate", "EmpId", "DptId", "DsgId", "EmpTypeId", "CompanyId", "IsWeekend", "GId","Notes" };
                    string[] getValues = { ViewState["__STId__"].ToString(),convertDateTime.getCertainCulture(txtDate.Text).ToString(),ViewState["__EmpId__"].ToString(),ddlDepartmentList.SelectedItem.Value,
                                          ViewState["__DsgId__"].ToString(),ViewState["__EmpTypeId"].ToString(),ddlCompanyList.SelectedValue,CheckIsOffDay(ViewState["__EmpId__"].ToString(),txtDate.Text).ToString(),
                                         ViewState["__GId__"].ToString(),txtNotes.Text};
                    if (SQLOperation.forSaveValue("ShiftTransferInfoDetails", getColumns, getValues, sqlDB.connection)) return true;
                   
                }
                return false;
            }
            catch { return false; }
        }

        protected void chkLoadAllShiftList_CheckedChanged(object sender, EventArgs e)
        {
            loadAssignedShiftList();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ValidationBasket())
            LoadAllEmployeeList();
        }


        private bool ValidationBasket()
        {
            try
            {
                if (ddlDepartmentList.SelectedValue == "0")
                {
                    lblMessage.InnerText = "error->Please select a department";
                    ddlDepartmentList.Focus();
                    return false; 
                }
                else if (ddlAssignShift.SelectedValue == "0")
                {
                    lblMessage.InnerText = "error->Please select a roster ";
                    ddlAssignShift.Focus();
                    return false;
                }
                else if (txtDate.Text.Trim().Length < 10)
                {
                    lblMessage.InnerText = "error->Select roster date"; txtDate.Focus(); return false;
                }
                return true;
            }
            catch { return false; }
        }

        private void LoadAllEmployeeList()
        {
            try
            {
                DataTable dt = new DataTable();
                string[] Sft_Dpt_Id = ddlAssignShift.SelectedValue.Split('|');

                sqlDB.fillDataTable("Select Distinct EmpId,EmpCardNo,EmpName,DsgName,SftName,EmpType,STId,FName,Notes,DsgId,EmpTypeId,GId,FId From v_ShiftTransferInfoDetails where DptId ='" + Sft_Dpt_Id[1] + "' AND  SftId=" + Sft_Dpt_Id[2] + " AND STID=" + Sft_Dpt_Id[0] + " AND CompanyId='" + ddlCompanyList.SelectedItem.Value + "' AND Format(SDate,'dd-MM-yyyy') ='"+txtDate.Text+"'  order by EmpCardNo", dt);
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

        protected void gvEmpList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if(e.CommandName=="Remove")
                {
                    int rIndex=Convert.ToInt32(e.CommandArgument.ToString());
                    SqlCommand cmd = new SqlCommand("delete from ShiftTransferInfoDetails where Format(SDate,'dd-MM-yyyy')='"+txtDate.Text+"' AND EmpId='"+gvEmpList.DataKeys[rIndex].Values[1].ToString()+"'",sqlDB.connection);
                    cmd.ExecuteNonQuery();

                    gvEmpList.Rows[rIndex].Visible = false;

                    lblMessage.InnerText = "success->Successfully Deleted";
                }
            }
            catch { }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            EmpVerifacation();
        }

        private bool EmpVerifacation()
        {
            try
            {
                sqlDB.fillDataTable("select EmpName,DsgName,DptName,EmpId,GId,DsgId,EmpTypeId from v_Personnel_EmpCurrentStatus where EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "'", dt = new DataTable());
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "error->Sorry any employee are not founded";
                    return false;
                }
                else
                {
                    lblMessage.InnerText = "warning->" + dt.Rows[0]["EmpName"].ToString() + " " + dt.Rows[0]["DptName"].ToString() + " " + dt.Rows[0]["DsgName"].ToString();
                    ViewState["__EmpId__"] = dt.Rows[0]["EmpId"].ToString();
                    ViewState["__GId__"] = dt.Rows[0]["GId"].ToString();
                    ViewState["__DsgId__"] = dt.Rows[0]["DsgId"].ToString();
                    ViewState["__EmpTypeId"] = dt.Rows[0]["EmpTypeId"].ToString();
                    return true;
                }
            }
            catch { return false; }
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

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue;
            classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, CompanyId);
        }
    }
}