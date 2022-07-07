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

namespace SigmaERP.hrd
{
    public partial class floorConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack) 
            {
                setPrivilege();               
            }
        }


        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "floor_config.aspx", gvFloorList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                LoadFloor();
            }
            catch { }

        }
        private Boolean SaveFloor()
        {
            try
            {               
                string IsActive = (chkIsActive.Checked == true) ? "1" : "0";
                SqlCommand cmd = new SqlCommand("insert into HRD_Floor values(@FName,@FNameBn,@IsActive,@Remarks)", sqlDB.connection);
                cmd.Parameters.AddWithValue("@FName", txtFloor.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@FNameBn", txtFloorBn.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@IsActive", IsActive);
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim().ToString());
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Save")
            {
                if (SaveFloor())
                    lblMessage.InnerText = "success->Saccessfully Saved";
                else lblMessage.InnerText = "error->Unable to Save";
            }
            else {
                if (UpdateFloor())
                    lblMessage.InnerText = "success->Saccessfully Update";
                else lblMessage.InnerText = "error->Unable to Update";
            }
            LoadFloor();
            allClear();
        }
        private Boolean UpdateFloor()
        {
            try
            {         
               
                
                int IsActive = (chkIsActive.Checked == true) ? 1 : 0;
                SqlCommand cmd = new SqlCommand("Update HRD_Floor set FName='" + txtFloor.Text + "',FNameBn='" + txtFloorBn.Text + "', IsActive=" + IsActive + ",Remarks='" + txtRemarks.Text + "' where FId=" + ViewState["__Fid__"].ToString() + "", sqlDB.connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                return false;
            }
        }
        private void LoadFloor()
        {
            try 
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from HRD_Floor", dt);
                gvFloorList.DataSource = dt;
                gvFloorList.DataBind();
            }
            catch{ }

        }

        protected void gvFloorList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                if (e.CommandName.Equals("Alter"))
                {                                  
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    ViewState["__Fid__"] = gvFloorList.DataKeys[rIndex].Values[0].ToString(); 
                    txtFloor.Text = gvFloorList.Rows[rIndex].Cells[0].Text.ToString();
                    txtFloorBn.Text = (gvFloorList.Rows[rIndex].Cells[1].Text.ToString() == "&nbsp;") ? "" : gvFloorList.Rows[rIndex].Cells[1].Text.ToString();
                    txtRemarks.Text = (gvFloorList.Rows[rIndex].Cells[3].Text.ToString() == "&nbsp;")?"":gvFloorList.Rows[rIndex].Cells[3].Text.ToString();
                    if (gvFloorList.Rows[rIndex].Cells[2].Text.ToString().Equals("True")) chkIsActive.Checked = true; else chkIsActive.Checked = false;
                    if (!ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Rbutton";
                    }
                    btnSave.Text = "Update";
                }
                else if (e.CommandName.Equals("Delete"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    SQLOperation.forDeleteRecordByIdentifier("HRD_Floor", "FId", gvFloorList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);                                       
                    lblMessage.InnerText = "success->Successfully Deleted";                    
                }
            }
            catch { }
        }

        protected void gvFloorList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LoadFloor();
        }
        private void allClear()
        {
            txtFloor.Text = "";
            txtFloor.Focus();
            txtFloorBn.Text = "";
            txtRemarks.Text = "";
            chkIsActive.Checked = false;
            if (ViewState["__WriteAction__"].ToString().Equals("0"))
            {

                btnSave.Enabled = false;
                btnSave.CssClass = "";
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.CssClass = "Rbutton";
            }
            btnSave.Text = "Save";
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            allClear();
        }

        protected void gvFloorList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                }
            }
            catch { }
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
            {
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnAlter");
                        lnkDelete.Enabled = false;
                        lnkDelete.CssClass = "";
                    }

                }
                catch { }
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnDelete");
                        lnkDelete.Enabled = false;
                        lnkDelete.CssClass = "";
                        lnkDelete.OnClientClick = "return false";
                    }

                }
                catch { }
            }
        }

        protected void gvFloorList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadFloor();
            gvFloorList.PageIndex = e.NewPageIndex;
            gvFloorList.DataBind();
        }
    }
}