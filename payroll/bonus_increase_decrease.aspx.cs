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

namespace SigmaERP.payroll
{
    public partial class bonus_decrease : System.Web.UI.Page
    {
        string CompanyId;
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
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                loadBounsByBounsType();

                ViewState["__IsSuccessEditStatus__"] = "True";
            }
        }

        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];

                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
               
                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                {
                  
                    classes.commonTask.LoadBranch(ddlCompanyList);                   
                  //  classes.commonTask.LoadShift(ddlShiftList, ViewState["__CompanyId__"].ToString());

                }
                else
                {
                   
                    dtSetPrivilege = new DataTable();                    
                    classes.commonTask.LoadBranch(ddlCompanyList, ViewState["__CompanyId__"].ToString());
                   // classes.commonTask.LoadShift(ddlShiftList, ViewState["__CompanyId__"].ToString());

                    if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                    {
                      
                    }

                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='aplication.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);

                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                           
                        }

                    }
                }
                CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                classes.Payroll.loadBonusTypeByCompany(ddlBonusType, CompanyId);
                addAllTextInShift();

            }
            catch { }
        }
        private void addAllTextInShift()
        {
           // if (ddlShiftList.Items.Count > 2)
             //   ddlShiftList.Items.Insert(1, new ListItem("All", "00"));
        }
        private void loadBounsByBounsType()
        {
            try
            {
                string getSQLCMD;              
             
                string CompanyList = (ddlCompanyList.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
             
                if (txtEmpCardNo.Text.Trim().Length == 0)
                {
                    getSQLCMD = "SELECT SL,v_Payroll_YearlyBonusSheet.EmpCardNo, v_Payroll_YearlyBonusSheet.PresentSalary,v_Payroll_YearlyBonusSheet.BasicSalary,v_Payroll_YearlyBonusSheet.BonusAmount," +
                              "v_Payroll_YearlyBonusSheet.Percentage, v_Payroll_YearlyBonusSheet.EmpName,v_Payroll_YearlyBonusSheet.DptName,v_Payroll_YearlyBonusSheet.DsgName," +
                              " FORMAT(v_Payroll_YearlyBonusSheet.EmpJoiningDate ,'dd-MM-yyyy') as EmpJoiningDate, v_Payroll_YearlyBonusSheet.CompanyName, v_Payroll_YearlyBonusSheet.SftName" +
                              " FROM   dbo.v_Payroll_YearlyBonusSheet"
                             + " where"
                            + " CompanyId in(" + CompanyList + ") AND BId='" + ddlBonusType.SelectedItem.Value.ToString() + "' AND BonusAmount !=0"
                            + " ORDER BY"
                             + " v_Payroll_YearlyBonusSheet.CompanyName, v_Payroll_YearlyBonusSheet.SftName, v_Payroll_YearlyBonusSheet.DptName";
                }
                else
                {
                    getSQLCMD = "SELECT SL, v_Payroll_YearlyBonusSheet.EmpCardNo, v_Payroll_YearlyBonusSheet.PresentSalary,v_Payroll_YearlyBonusSheet.BasicSalary,v_Payroll_YearlyBonusSheet.BonusAmount," +
                              "v_Payroll_YearlyBonusSheet.Percentage, v_Payroll_YearlyBonusSheet.EmpName,v_Payroll_YearlyBonusSheet.DptName,v_Payroll_YearlyBonusSheet.DsgName," +
                              " FORMAT(v_Payroll_YearlyBonusSheet.EmpJoiningDate ,'dd-MM-yyyy') as EmpJoiningDate, v_Payroll_YearlyBonusSheet.CompanyName, v_Payroll_YearlyBonusSheet.SftName" +
                              " FROM  dbo.v_Payroll_YearlyBonusSheet"
                             + " where"
                            + " CompanyId in(" + CompanyList + ") AND BId='" + ddlBonusType.SelectedItem.Value.ToString() + "' AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' AND BonusAmount !=0"
                            + " ORDER BY"
                             + " v_Payroll_YearlyBonusSheet.CompanyName, v_Payroll_YearlyBonusSheet.SftName, v_Payroll_YearlyBonusSheet.DptName";
                }
                DataTable dt = new DataTable();
                sqlDB.fillDataTable(getSQLCMD,dt);
                if (dt.Rows.Count > 0)
                {
                    gvBonusList.DataSource = dt;
                    gvBonusList.DataBind();
                }
                else
                {
                    gvBonusList.DataSource = null;
                    gvBonusList.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                //classes.commonTask.LoadShift(ddlShiftList, CompanyId);
                addAllTextInShift();
                classes.Payroll.loadBonusTypeByCompany(ddlBonusType, CompanyId);    
               loadBounsByBounsType();       
            }
            catch { }
        }
        protected void ddlShiftList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
              loadBounsByBounsType();
            }
            catch { }
        }
        protected void ddlBonusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                loadBounsByBounsType();
            }
            catch { }
        }
        protected void gvBonusList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                TextBox txtAddBonusAmount = (TextBox)gvBonusList.Rows[rIndex].Cells[8].FindControl("txtInDe_Crease");

                if (e.CommandName.ToString().Equals("Increase"))
                {

                    Button btn = (Button)gvBonusList.Rows[rIndex].Cells[9].FindControl("btnIncrease");
                    if (btn.Text == "Increase")
                    {
                        if (ViewState["__IsSuccessEditStatus__"].ToString() == "False")
                        {
                            lblMessage.InnerText = "error->Please must be complete previous edit status !";
                            return;
                        }
                        else
                        {
                            btn.Text = "Ok";
                            txtAddBonusAmount.Style.Add("border-style", "solid");
                            txtAddBonusAmount.Style.Add("border-color", "#0000ff");
                            txtAddBonusAmount.Enabled = true;
                            ViewState["__IsSuccessEditStatus__"] = "False";
                        }
                    }
                    else
                    {
                        double getNewBouns = Math.Round(double.Parse(gvBonusList.Rows[rIndex].Cells[6].Text.ToString()) + double.Parse(txtAddBonusAmount.Text), 0);
                        gvBonusList.Rows[rIndex].Cells[6].Text = getNewBouns.ToString();

                        btn.Text = "Increase";
                        txtAddBonusAmount.Style.Add("border-style", "solid");
                        txtAddBonusAmount.Style.Add("border-color", "gray");
                        txtAddBonusAmount.Enabled = false;
                        ViewState["__IsSuccessEditStatus__"] = "True";

                        updateBouns("Increase",getNewBouns.ToString(),gvBonusList.DataKeys[rIndex].Value.ToString());
                    }

                }
                else
                {
                    Button btn = (Button)gvBonusList.Rows[rIndex].Cells[9].FindControl("btnDecrease");
                    if (btn.Text == "Decrease")
                    {
                        if (ViewState["__IsSuccessEditStatus__"].ToString() == "False")
                        {
                            lblMessage.InnerText = "error->Please must be complete previous edit status !";
                            return;
                        }
                        else
                        {
                            btn.Text = "Ok";
                            txtAddBonusAmount.Style.Add("border-style", "solid");
                            txtAddBonusAmount.Style.Add("border-color", "#0000ff");
                            txtAddBonusAmount.Enabled = true;
                            ViewState["__IsSuccessEditStatus__"] = "False";
                        }
                    }
                    else
                    {

                        if (double.Parse(txtAddBonusAmount.Text) > double.Parse(gvBonusList.Rows[rIndex].Cells[6].Text.ToString()))
                        {
                            lblMessage.InnerText = "warning->Please type correct amount !";
                            return;
                        }
                        double getNewBouns = Math.Round(double.Parse(gvBonusList.Rows[rIndex].Cells[6].Text.ToString()) - double.Parse(txtAddBonusAmount.Text), 0);
                        
                        gvBonusList.Rows[rIndex].Cells[6].Text = getNewBouns.ToString();

                        btn.Text = "Decrease";
                        txtAddBonusAmount.Style.Add("border-style", "solid");
                        txtAddBonusAmount.Style.Add("border-color", "gray");
                        txtAddBonusAmount.Enabled = false;
                        ViewState["__IsSuccessEditStatus__"] = "True";

                        updateBouns("Decrease", getNewBouns.ToString(), gvBonusList.DataKeys[rIndex].Value.ToString());
                    }
                }
            }
            catch { }
        }
        private void updateBouns(string status,string bonusAmount,string SL)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("update Payroll_YearlyBonusSheet set BonusAmount=" + bonusAmount + ",EditStatus='"+status+"' where SL=" + SL + "", sqlDB.connection);
                int result = cmd.ExecuteNonQuery();
                lblMessage.InnerText = "success->Successfully Bonus " + status+"d";
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadBounsByBounsType();
        }

        protected void gvBonusList_RowDataBound(object sender, GridViewRowEventArgs e)
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