using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class bonud_monyh_setup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";

            if (!IsPostBack)
            {
                setPrivilege();
                // classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpTypeList);
                classes.Payroll.loadBonusType(dlSelectBonusYearAndType, ViewState["__CompanyId__"].ToString());
                if (!classes.commonTask.HasBranch())
                    ddlComapnyList.Enabled = false;
                ddlComapnyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
        }

        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                
                
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                {
                    classes.commonTask.LoadBranch(ddlComapnyList);
                    return;
                }
                else
                {
                    ddlComapnyList.Enabled = false;
                    classes.commonTask.LoadBranch(ddlComapnyList, ViewState["__CompanyId__"].ToString());
                    if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                    {
                        btnDelete.Enabled = false;
                        //dlSelectBonusYearAndType.Enabled = false;
                    }
                    else
                    {
                        btnSet.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    dtSetPrivilege = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='bonus_monyh_setup.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "",dtSetPrivilege);
                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            gvBonusMonthList.Visible = true;
                            dlSelectBonusYearAndType.Enabled = true;
                        }
                        else gvBonusMonthList.Visible = false;
                        if (bool.Parse(dtSetPrivilege.Rows[0]["WriteAction"].ToString()).Equals(true))
                        {
                            gvBonusMonthList.Visible = true;
                            btnSet.Enabled = true;
                        }
                        else btnSet.Enabled = false;

                        if (bool.Parse(dtSetPrivilege.Rows[0]["DeleteAction"].ToString()).Equals(true))
                        {

                            btnDelete.Enabled = true;
                        }
                    }

                }

            }
            catch { }

        }

       

       

        protected void dlSelectBonusYearAndType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

             //   divPagelist.Visible = false;
                lblMessage.InnerText = "";
                if (dlSelectBonusYearAndType.SelectedValue.ToString() == "" || dlSelectBonusYearAndType.SelectedValue.ToString() == "0")
                {
                    gvBonusMonthList.DataSource = null;
                    gvBonusMonthList.DataBind();
                    tblGenreateType.Visible = false;
                    lblStatus.Text = " "; 
                    return;
                }
                lblStatus.Text = "Unsetuped Bonus Month Info";
                tblGenreateType.Visible = true;
                DataTable dt = new DataTable();
                dt.Columns.Add("SL", typeof(int));
                dt.Columns.Add("SlabType",typeof(string));
                dt.Columns.Add("EquivalentDays", typeof(string));
                dt.Columns.Add("Chosen",typeof(bool));
                dt.Columns.Add("Percentage",typeof(string));
                if (chekHasRecord() == true) return;   // verify already setuped ?
                else
                {
                   // divPagelist.Visible = true;
                    int EquivalentDays = 0;
                    
                    for (byte b =12; b > 0; b--)
                    {
                       

                        bool chosen = false; int percentage = 0;
                        if (b == 12)
                        { 
                        chosen = true; percentage = 100;
                        }                       
                        else if (b == 9)
                        {
                           chosen = true; percentage = 75;
                        }
                        
                        else if (b == 6)
                        {
                            chosen = true; percentage = 50;
                        }                       
                        else if (b == 3)
                        {
                          chosen = true; percentage = 25;
                        }
                        

                        EquivalentDays = (30 * b);
                        dt.Rows.Add(b, b.ToString() + " Months", EquivalentDays, chosen,percentage);
                       


                    }
                    gvBonusMonthList.DataSource = dt;
                    gvBonusMonthList.DataBind();
                    lblStatus.Text = "Unsetuped Bonus Month Info";
                
                }
            }
            catch { }
        }

        private bool chekHasRecord()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from Payroll_BonusMonthSetup where BId=" + dlSelectBonusYearAndType.SelectedItem.Value.ToString() + "", dt=new DataTable ());
                if (dt.Rows.Count > 0)
                {
                   // divPagelist.Visible = true;
                    gvBonusMonthList.DataSource = dt;
                    gvBonusMonthList.DataBind();
                    lblStatus.Text = "Setuped Bonus Month Info";
                    if (dt.Rows[0]["GenerateOn"].ToString() == "Basic Salary") rblGenerateType.SelectedIndex = 1;
                    else rblGenerateType.SelectedIndex = 0;
                    return true;
                }
                else return false;
            }
            catch { return false; }
        
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                bool status = false;
                if (dlSelectBonusYearAndType.SelectedValue.ToString() == "0") return;
                SQLOperation.forDeleteRecordByIdentifier("Payroll_BonusMonthSetup", "BId", dlSelectBonusYearAndType.SelectedValue.ToString(), sqlDB.connection);
                for (byte b = 0; b < gvBonusMonthList.Rows.Count; b++)
                {
                    CheckBox chk = new CheckBox();
                    chk = (CheckBox)gvBonusMonthList.Rows[b].Cells[2].FindControl("chkChosen");
                    TextBox txtPercentage = (TextBox)gvBonusMonthList.Rows[b].Cells[3].FindControl("txtPercentage");
                    TextBox txtEquivalentDays = (TextBox)gvBonusMonthList.Rows[b].FindControl("txtEquivalentDays");

                    bool chosen = (chk.Checked) ? true : false;
                    string[] getColumns = {"BId","SlabType", "EquivalentDays", "Chosen", "Percentage", "BonusType", "GenerateOn" };
                    string[] getValues = { dlSelectBonusYearAndType.SelectedValue.ToString(),gvBonusMonthList.Rows[b].Cells[0].Text, txtEquivalentDays.Text.Trim(), chosen.ToString(), txtPercentage.Text,dlSelectBonusYearAndType.SelectedItem.ToString(),rblGenerateType.SelectedItem.ToString() };
                    if (SQLOperation.forSaveValue("Payroll_BonusMonthSetup", getColumns, getValues,sqlDB.connection) == true)
                    {
                        status = true;
                    }
                }

                if (status) lblMessage.InnerText = "success->Successfully Bonus Month Setup";
            }
            catch (Exception ex)
            {
              
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            SQLOperation.forDeleteRecordByIdentifier("Payroll_BonusMonthSetup", "BonusType",dlSelectBonusYearAndType.SelectedItem.ToString(),sqlDB.connection);
            gvBonusMonthList.DataSource = null;
            gvBonusMonthList.DataBind();
        }

        protected void ddlComapnyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                classes.Payroll.loadBonusType(dlSelectBonusYearAndType, ddlComapnyList.SelectedValue);
                gvBonusMonthList.DataSource = null;
                gvBonusMonthList.DataBind();
            }
            catch { }
        }

        protected void tc1_ActiveTabChanged(object sender, EventArgs e)
        {
           loadSetupedInfo();
        }

        private void loadSetupedInfo()
        {
            try
            {
                DataTable dt=new DataTable ();
                sqlDB.fillDataTable("select distinct BId,BonusType,GenerateOn,convert(varchar(10), SetupedDate,105) as SetupedDate, convert(varchar(10), SetupedDate,120) as SetupedDate1 from Payroll_BonusMonthSetup order by convert(varchar(10), SetupedDate,120) desc", dt);
                gvSetupedList.DataSource = dt;
                gvSetupedList.DataBind();

            }
            catch { }
        }

        protected void gvSetupedList_RowDataBound(object sender, GridViewRowEventArgs e)
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
        }

     

    }
}