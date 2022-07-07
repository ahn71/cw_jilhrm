using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRD.ModelEntities.Models.Settings;
using HRD.BBL.Settings;
using adviitRuntimeScripting;
using System.Data;
using SigmaERP.classes;
using ComplexScriptingSystem;
using System.Drawing;

namespace SigmaERP.hrd
{
    public partial class business_type : System.Web.UI.Page
    {
        BusinessTypeEntry businessTypeEntry;
        bool result;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                DataBindForView();
                txtBusinessType.Focus();
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "business_type.aspx", gvBusinessType, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
              
            }
            catch { }

        }
        private BusinessTypeEntity GetBusinessTypeEntityData()
        {
            try
            {
                BusinessTypeEntity businessTypeEntity = new BusinessTypeEntity();
                businessTypeEntity.BId =int.Parse(ViewState["__BId__"].ToString());
                businessTypeEntity.BTypeName = txtBusinessType.Text;
                businessTypeEntity.IsActive = (chkIsActive.Checked) ? true : false;

                return businessTypeEntity;
            }
            catch { return null; }
        }

        private void saveBusinessType()//......For Save......
        {
            ViewState["__BId__"] = "0";
            using (BusinessTypeEntity businessTypeEntity = GetBusinessTypeEntityData())
            {
                
                if (businessTypeEntry == null) businessTypeEntry = new BusinessTypeEntry();
                businessTypeEntry.SetEntities = businessTypeEntity;
                result = businessTypeEntry.Insert();
             
                if (result)
                {
                    DataBindForView();
                    lblMessage.InnerText = "success-> Successfully Saved.";
                }
                else
                {
                    lblMessage.InnerText = "warning-> Somthing is worng!"; return;
                }
            }
        }
        private void updateBusinessType() // .........For Updat............
        {
            ViewState["__BId__"] = "0";
            using (BusinessTypeEntity businessTypeEntity = GetBusinessTypeEntityData())
            {

                if (businessTypeEntry == null) businessTypeEntry = new BusinessTypeEntry();
                businessTypeEntry.SetEntities = businessTypeEntity;
                result = businessTypeEntry.Update(int.Parse(ViewState["__getBId__"].ToString()));
                if (result)
                {
                    DataBindForView();
                    lblMessage.InnerText = "success-> Successfully Updated.";
                }
                else
                {
                    lblMessage.InnerText = "warning-> Somthing is worng!"; return;
                }
            }
         
            //if (result)
            //{
            //    DataBindForView();
            //    lblMessage.InnerText = "success-> Successfully Saved.";
            //}
            //else
            //{
            //    lblMessage.InnerText = "warning-> Somthing is worng!"; return;
            //}
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(txtBusinessType.Text.Trim().Length==0)
            {
                lblMessage.InnerText = "warning-> Please Enter Business Type Name.";
                txtBusinessType.Focus();
                return;
            }
            if (btnSave.Text == "Update") updateBusinessType(); else saveBusinessType();
            allClear();
        }

        private void DataBindForView()
        {
            try
            {
                if (businessTypeEntry == null) businessTypeEntry = new BusinessTypeEntry();
                List<BusinessTypeEntity> getBusinessTypeList = businessTypeEntry.GetBusinessTyeList;                
                gvBusinessType.DataSource = getBusinessTypeList;
                gvBusinessType.DataBind();
            }
            catch { }
        }

        protected void gvBusinessType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rIndext =int.Parse(e.CommandArgument.ToString());
            ViewState["__getBId__"] = gvBusinessType.DataKeys[rIndext].Values[0].ToString();
           // ViewState["__getThaId__"] = gvLateDeductionTypeList.DataKeys[rIndex].Values[0].ToString();
            if (businessTypeEntry == null) businessTypeEntry = new BusinessTypeEntry();
            if (e.CommandName == "Alter")
            {
                //ddlDistrict.SelectedValue = gvLateDeductionTypeList.DataKeys[rIndex].Values[1].ToString();
                //txtThanaName.Text = gvLateDeductionTypeList.Rows[rIndex].Cells[1].Text.ToString();
                txtBusinessType.Text = gvBusinessType.Rows[rIndext].Cells[0].Text.ToString();
                if (gvBusinessType.Rows[rIndext].Cells[1].Text.ToString().Equals("True")) chkIsActive.Checked = true; else chkIsActive.Checked = false;
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
            else if (e.CommandName == "Delete")
            {
                if (deleteValidation(ViewState["__getBId__"].ToString()))
                    {
                result = businessTypeEntry.Delete(int.Parse(ViewState["__getBId__"].ToString()));
                if (result)
                {
                    //gvBusinessType.Rows[rIndext].Visible = false;
                    lblMessage.InnerText = "success-> Successfully Deleted.";
                }
                    }
                 else
                    lblMessage.InnerText = "error->Warning! Can't delete this BusinessType. It is used in a Company.";
            }
        }

        protected void gvBusinessType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataBindForView();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            allClear();            
        }
        private void allClear() 
        {
            txtBusinessType.Text = "";
            txtBusinessType.Focus();
            chkIsActive.Checked = false;
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
        }
        private bool deleteValidation(string BTypeId)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select BusinessType from HRD_CompanyInfo  where BusinessType=" + BTypeId + "", dt);
            if (dt.Rows.Count > 0)
                return false;
            else return true;
        }

        protected void gvBusinessType_RowDataBound(object sender, GridViewRowEventArgs e)
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
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnDelete");
                        lnkDelete.Enabled = false;
                        lnkDelete.OnClientClick = "return false";
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnAlter");
                        lnkDelete.Enabled = false;
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }
        
    }
}