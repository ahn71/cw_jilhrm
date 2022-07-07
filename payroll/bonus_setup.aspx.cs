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
using System.Drawing;
using SigmaERP.classes;

namespace SigmaERP.payroll
{
    public partial class bonus_setup : System.Web.UI.Page
    {
        DataTable dt;
       static DataTable dtSetPrivilege;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                loadBounsInfo();
                loadReligionForBonusGenerate();
                ViewState["__BonusName__"] = "";
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
        }

        //DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                hfWriteAction.Value = "True";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                {
                    classes.commonTask.LoadBranch(ddlCompanyName);
                    return;
                }
                else
                {
                    ddlCompanyName.Enabled = false;
                    classes.commonTask.LoadBranch(ddlCompanyName, ViewState["__CompanyId__"].ToString());
                    if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                    {
                        divBonusList.Visible = false;
                        //gvBonusSetupList.Visible = false;
                      
                    }
                    else
                    {
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;

                    }
                    dtSetPrivilege = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='bonus_setup.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);
                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            divBonusList.Visible = true;
                            //gvBonusSetupList.Visible = true;
                        }
                        else divBonusList.Visible = false;
                        if (bool.Parse(dtSetPrivilege.Rows[0]["WriteAction"].ToString()).Equals(true))
                        {
                            btnSave.Enabled = true;
                            btnSave.CssClass = "Pbutton";
                        }
                        else 
                        {
                            btnSave.Enabled = false;
                            btnSave.CssClass = "";
                            //btnSave.CssClass = "";
                            //btnSave.Enabled = false;
                            hfWriteAction.Value = "False";
                        }
                        if (bool.Parse(dtSetPrivilege.Rows[0]["UpdateAction"].ToString()).Equals(true) || bool.Parse(dtSetPrivilege.Rows[0]["DeleteAction"].ToString()).Equals(true))
                            divBonusList.Visible = true;
                        
                        //if (bool.Parse(dtSetPrivilege.Rows[0]["UpdateAction"].ToString()).Equals(false))
                        //{

                        //    hfUpdateAction.Value = "False";

                        //    if (ViewState["__WriteAction__"].ToString() == "1")
                        //    {

                        //        btnSave.Enabled = true;
                        //    }
                        //    else
                        //    {

                        //        btnSave.CssClass = "";
                        //        btnSave.Enabled = false;
                        //    }
                        //}
                        //if (bool.Parse(dtSetPrivilege.Rows[0]["DeleteAction"].ToString()).Equals(false))
                        //{
                        //    ViewState["__DeletAction__"] = "0";
                        //}
                    }

                }

            }
            catch { }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(rblGenerateByReligion.SelectedItem==null || rblGenerateByReligion.SelectedItem.Text=="")
            {
                lblMessage.InnerText = "warning-> Please select a Religion.";
                return;
            }
            if (btnSave.Text.Equals("Save")) saveBounsType();
            else updateBonus(ViewState["__BID__"].ToString());
        }

        private void saveBounsType()
        {
            try
            {
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                byte getStatus =(rblStatus.SelectedValue.ToString().Equals("1")) ?(byte) 1 :(byte) 0;
                string[] getColumns = { "CompanyId", "BonusName", "PaymentDate", "ConfigDate", "Status", "CalculationDate","RId" };
                string[] getValues =  { CompanyId,txtBonusName.Text.Trim(),commonTask.ddMMyyyyToyyyyMMdd(txtPaymentDate.Text.Trim()), commonTask.ddMMyyyyToyyyyMMdd(txtConfigDate.Text.Trim()),
                                      getStatus.ToString(),commonTask.ddMMyyyyToyyyyMMdd(txtCalculationDate.Text.Trim()),rblGenerateByReligion.SelectedValue};
                if (SQLOperation.forSaveValue("Payroll_BonusSetup", getColumns, getValues,sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success-> Successfully Saved";
                    InputBoxClear();
                    loadBounsInfo();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->"+ex.Message;
            }
        }

        private void updateBonus(string BID)
        {
            try
            {
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                byte getStatus = (rblStatus.SelectedValue.ToString().Equals("1")) ? (byte)1 : (byte)0;
                string[] getColumns = { "CompanyId", "BonusName", "PaymentDate", "ConfigDate", "Status", "CalculationDate","RId" };
                string[] getValues = {CompanyId,txtBonusName.Text.Trim(),commonTask.ddMMyyyyToyyyyMMdd(txtPaymentDate.Text.Trim()), commonTask.ddMMyyyyToyyyyMMdd(txtConfigDate.Text.Trim()),
                                      getStatus.ToString(),commonTask.ddMMyyyyToyyyyMMdd(txtCalculationDate.Text.Trim()),rblGenerateByReligion.SelectedValue };
                if (SQLOperation.forUpdateValue("Payroll_BonusSetup", getColumns, getValues, "BId",BID,sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success-> Successfully Updated";
                    loadBounsInfo();
                    InputBoxClear();
                    if (hfWriteAction.Value == "False")
                    {
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void loadBounsInfo()
        {
            try
            {
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                sqlDB.fillDataTable("select pbs.BId,pbs.BonusName,Format(pbs.CalculationDate,'dd-MM-yyyy') as CalculationDate,pbs.CompanyId,Format(pbs.ConfigDate,'dd-MM-yyyy') as ConfigDate ,Format(pbs.PaymentDate,'dd-MM-yyyy') as PaymentDate,pbs.RId,pbs.Status, case when  pbs.RId=0 then 'All' else hr.RName  end as Religion " +
                    "from Payroll_BonusSetup pbs Left   join HRD_Religion as HR  on pbs.RId=HR.RId where CompanyId='" + CompanyId + "' order by convert(varchar(10), CalculationDate,120) desc", dt = new DataTable());

                gvBonusSetupList.DataSource = dt;
                gvBonusSetupList.DataBind();

               
            }
            catch (Exception ex)
            { 
            
            }
        }

        

        protected void gvBonusSetupList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                if (e.CommandName.Equals("Alter"))
                {
                    ddlCompanyName.SelectedValue = gvBonusSetupList.DataKeys[rIndex].Values[1].ToString();
                    txtBonusName.Text = gvBonusSetupList.Rows[rIndex].Cells[0].Text;
                    txtPaymentDate.Text = gvBonusSetupList.Rows[rIndex].Cells[2].Text;
                    if (bool.Parse(gvBonusSetupList.DataKeys[rIndex].Values[2].ToString()).Equals(true)) rblStatus.SelectedIndex = 0;
                    else rblStatus.SelectedIndex = 1;
                    txtConfigDate.Text = gvBonusSetupList.Rows[rIndex].Cells[3].Text;
                    txtCalculationDate.Text = gvBonusSetupList.Rows[rIndex].Cells[4].Text;
                    ViewState["__BID__"] = gvBonusSetupList.DataKeys[rIndex].Values[0].ToString();
                    rblGenerateByReligion.SelectedValue = gvBonusSetupList.DataKeys[rIndex].Values[3].ToString();
                    btnSave.Enabled = true;
                    btnSave.Text = "Update";
                    btnSave.CssClass = "Pbutton";
                    
                }
                else if (e.CommandName.Equals("Delete"))
                {
                    if (SQLOperation.forDeleteRecordByIdentifier("Payroll_BonusSetup", "BId", gvBonusSetupList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection))
                    {
                        lblMessage.InnerText = "success->Successfully Deleted";

                        InputBoxClear();

                    }
                }
            }
            catch { }
        }

        private void InputBoxClear()
        {
            try
            {
                ViewState["__BonusName__"] = "";
                btnSave.Text = "Save";
                //ddlCompanyName.SelectedIndex = 0;
                txtBonusName.Text = "";
                txtCalculationDate.Text = "";
                txtPaymentDate.Text = "";
                txtConfigDate.Text = "";
                rblStatus.SelectedIndex = -1;
                rblGenerateByReligion.SelectedIndex = -1;

            }
            catch { }
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                loadBounsInfo();
               
            }
            catch { }
        }

        protected void gvBonusSetupList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            loadBounsInfo();

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            InputBoxClear();
        }

        protected void gvBonusSetupList_RowDataBound(object sender, GridViewRowEventArgs e)
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



            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
            {

                return;
            }
            try 
            {
                if (dtSetPrivilege.Rows.Count <= 0)
                {
                    try
                    {
                        Button btnDel = (Button)e.Row.FindControl("btnDelete");
                        btnDel.Enabled = false;
                        btnDel.ForeColor = Color.Black;
                        btnDel.OnClientClick = "return false";
                    }
                    catch { }
                    try
                    {
                            Button btnAlt = (Button)e.Row.FindControl("btnAlter");
                            btnAlt.Enabled = false;
                            btnAlt.ForeColor = Color.Black;
                    }
                    catch { }
                    return;
                }
            }
            catch { }
            try
            {
                if (bool.Parse(dtSetPrivilege.Rows[0]["DeleteAction"].ToString()).Equals(false))
                {
                    Button btnDel = (Button)e.Row.FindControl("btnDelete");
                    btnDel.Enabled = false;
                    btnDel.ForeColor = Color.Black;
                    btnDel.OnClientClick = "return false";
                }
               
            }
            catch { }
            try
            {
                if (bool.Parse(dtSetPrivilege.Rows[0]["UpdateAction"].ToString()).Equals(false))
                {
                    Button btnAlt = (Button)e.Row.FindControl("btnAlter");
                    btnAlt.Enabled = false;
                    btnAlt.ForeColor = Color.Black;
                    //Button btn = (Button)e.Row.FindControl("btnAlter");
                    //btn.Enabled = false;    
                }
            }
            catch { }
           
            
        }

        private void loadReligionForBonusGenerate()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select RId,RName from HRD_Religion ", dt);
                rblGenerateByReligion.DataTextField = "RName";
                rblGenerateByReligion.DataValueField = "RId";
                rblGenerateByReligion.DataSource = dt;
                rblGenerateByReligion.DataBind();
                //rblGenerateByReligion.Items.Insert(0,new ListItem("All","0"));
            }
            catch { }
        }

       
        protected void rblGenerateByReligion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                /*
                if (ViewState["__BonusName__"].ToString().Trim().Length == 0) ViewState["__BonusName__"] = txtBonusName.Text;

                if (txtBonusName.Text.Trim() == "")
                {
                    lblMessage.InnerText = "error->Please first time type bonus name then chose religion !";
                    rblGenerateByReligion.SelectedIndex = -1;
                    return;
                }

                txtBonusName.Text = "";
                txtBonusName.Text = ViewState["__BonusName__"].ToString() + "_" + rblGenerateByReligion.SelectedItem.Text.ToString()+"("+rblGenerateByReligion.SelectedValue+")";
           
                 */
            }
            catch { }
        }
    }
}