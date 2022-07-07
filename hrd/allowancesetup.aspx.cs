using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using adviitRuntimeScripting;
using System.Data;
using System.Data.SqlClient;
using SigmaERP.classes;
using System.Drawing;

namespace SigmaERP.personnel
{
    public partial class allowancesetup : System.Web.UI.Page
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
                loadAllowanceType();               

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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "allowancesetup.aspx", gvAllownceList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

            }
            catch { }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text=="Save") saveAllowance();
            else updateAllowance();
        }
        private void saveAllowance()
        {
            try
            {
                string[] getColumns = { "BasicAllowance", "MedicalAllownce", "ConvenceAllownce","FoodAllownce","HouseRent","StampDeduct", "Year" };
                string[] getValues = {txtBasicFind.Text.Trim(),txtMedical.Text.Trim(),txtConveyance.Text.Trim(),txtFood.Text.Trim(),txtHouseRent.Text.Trim(),txtStampDeduct.Text.Trim(),DateTime.Now.ToString("yyyy")};
                if (SQLOperation.forSaveValue("HRD_AllownceSetting", getColumns, getValues,sqlDB.connection) == true)
                {
                    
                    lblMessage.InnerText = "success->Successfully saved";
                    AllClear();
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    loadAllowanceType();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }

        private void updateAllowance()
        {
            try
            {

                string[] getColumns = { "BasicAllowance", "MedicalAllownce", "ConvenceAllownce","FoodAllownce", "HouseRent", "StampDeduct" };
                string[] getValues = { txtBasicFind.Text.Trim(),txtMedical.Text.Trim(),txtConveyance.Text.Trim(), txtFood.Text.Trim(), txtHouseRent.Text.Trim(),txtStampDeduct.Text.Trim()};
                if (SQLOperation.forUpdateValue("HRD_AllownceSetting", getColumns, getValues, "AllownceId", ViewState["__DstId__"].ToString(), sqlDB.connection) == true)
                {
                   
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    AllClear();
                    lblMessage.InnerText = "success->Successfully Updated";
                    loadAllowanceType();
                }

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }

        private void loadAllowanceType()
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select * from HRD_AllownceSetting where AllownceId =(select MAX(AllownceId) from HRD_AllownceSetting)", dt = new DataTable(), sqlDB.connection);              
                string divInfo = "";
                if (dt.Rows.Count == 0)
                {
                    divInfo = "<div class='noData'></div>";
                    divInfo += "<div class='dataTables_wrapper'>Any Allowance Not assigned<div class='head'></div></div>";
                    divAllowanceType.Controls.Add(new LiteralControl(divInfo));
                    return;
                }
                gvAllownceList.DataSource = dt;
                gvAllownceList.DataBind();

               
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }

        }

        protected void gvAllownceList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["__DstId__"] = gvAllownceList.DataKeys[rIndex].Values[0].ToString();
            if (e.CommandName == "Alter")
            {
                txtBasicFind.Text = gvAllownceList.Rows[rIndex].Cells[0].Text.ToString();
                txtMedical.Text = gvAllownceList.Rows[rIndex].Cells[1].Text.ToString();
                txtConveyance.Text = gvAllownceList.Rows[rIndex].Cells[3].Text.ToString();
                txtFood.Text = gvAllownceList.Rows[rIndex].Cells[2].Text.ToString();
                txtHouseRent.Text = gvAllownceList.Rows[rIndex].Cells[4].Text.ToString();
                txtStampDeduct.Text = gvAllownceList.Rows[rIndex].Cells[5].Text.ToString();
                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "Rbutton";
                }
                btnSave.Text = "Update";
            }           
        }
        private void AllClear() 
        {
            txtBasicFind.Text = "";
            txtConveyance.Text = "";
            txtFood.Text = "";
            txtMedical.Text = "";
            txtHouseRent.Text = "0.0";
            txtStampDeduct.Text = "";
            ViewState["__DstId__"] = "0";
            if (ViewState["__WriteAction__"].Equals("0"))
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
            btnSave.Text = "Save";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            AllClear();
        }

   

        protected void gvAllownceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
             
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        Button lnk= (Button)e.Row.FindControl("btnAlter");
                        lnk.Enabled = false;
                        lnk.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }

    }
}