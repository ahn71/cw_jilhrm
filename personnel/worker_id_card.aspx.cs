using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using adviitRuntimeScripting;
using System.Data;
using System.Data.SqlClient;

namespace SigmaERP.personnel
{
    public partial class worke_id_card : System.Web.UI.Page
    {
        DataTable dt;
        string CompanyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
           
            
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.LoadBranch(ddlBranch);
                ddlBranch.SelectedValue = ViewState["__CompanyId__"].ToString();
                //ddlDepName.Visible = false;
                //ddlEmpCardNo.Visible = false;
                trddldepname.Visible = false;
                trddlempcardno.Visible = false;
                LoadAllWorker();
                if (!classes.commonTask.HasBranch())
                  ddlBranch.Enabled = false;
            }
        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='worke_id_card.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnpreview.CssClass = "";
                            
                            btnpreview.Enabled = false;


                        }
                    }
                }

            }
            catch { }

        }
       
        protected void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rdbDeptWise.Checked = false;
                rdbIndividual.Checked = false;
                trddldepname.Visible = false;
                trddlempcardno.Visible = false;
                workerlist.Visible = true;
                lstSelected.Items.Clear();
                LoadAllWorker();
            }
            catch { }

          
        }

        protected void rdbDeptWise_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rdbAll.Checked = false;
                rdbIndividual.Checked = false;
                trddldepname.Visible = true;
                trddlempcardno.Visible = false;
                workerlist.Visible = true;
                lstAll.Items.Clear();
                lstSelected.Items.Clear();
                CompanyID = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
                classes.commonTask.loadDepartmentListByCompany(ddlDepName, CompanyID);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
               

            }
            catch { }
        }

        protected void rdbIndividual_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rdbAll.Checked = false;
                rdbDeptWise.Checked = false;
                trddldepname.Visible = false;
                trddlempcardno.Visible = true;
                workerlist.Visible = false;
                LoadEmpCardNo(ddlEmpCardNo);
                lstAll.Items.Clear();
                lstSelected.Items.Clear();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            }
            catch { }
        }
        private void LoadAllWorker()
        {
            try
            {
                CompanyID = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
                dt = new DataTable();
                sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo+' [ '+EmpName+' ]' as EmpCardNo  From v_EmployeeDetails where CompanyId='" + CompanyID + "' and EmpTypeId=1 and EmpStatus in ('1','8')" +
                                   " and  ActiveSalary='True' Group by EmpId,EmpCardNo,EmpName,DptCode,CustomOrdering order by DptCode,CustomOrdering", dt);
                lstAll.DataSource = dt;
                lstAll.DataTextField = "EmpCardNo";
                lstAll.DataValueField = "SN";
                lstAll.DataBind();
            }
            catch { }
        }

        private void LoadEmpCardNo(DropDownList dl)
        {
            CompanyID = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
            dt = new DataTable();
            sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId, EmpCardNo +' ['+EmpName+']' as EmpCardNo From v_EmployeeDetails where CompanyId='" + CompanyID + "' and EmpTypeId=1 and EmpStatus in ('1','8') and  ActiveSalary='True' Group by EmpId,EmpCardNo,EmpName,DptCode,CustomOrdering order by DptCode,CustomOrdering", dt);
            dl.DataSource = dt;
            dl.DataTextField = "EmpCardNo";
            dl.DataValueField = "SN";
            dl.DataBind();
        }
        private void LoadDiptwiseCardNo()
        {
            CompanyID = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
            dt = new DataTable();
            sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo+' [ '+EmpName+' ]' as EmpCardNo  From v_EmployeeDetails where CompanyId='" + CompanyID + "' and DptId= '" + ddlDepName.SelectedValue + "' and EmpTypeId=1 and EmpStatus in ('1','8')" +
                                    " and  ActiveSalary='True' Group by EmpId,EmpCardNo,EmpName,CustomOrdering order by CustomOrdering", dt); lstAll.DataSource = dt;
            lstAll.DataSource = dt;
            lstAll.DataTextField = "EmpCardNo";
            lstAll.DataValueField = "SN";
            lstAll.DataBind();
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstAll,lstSelected);
        }
        private void AddRemoveItem(ListBox aSource, ListBox aTarget)
        {

            ListItemCollection licCollection;

            try
            {

                licCollection = new ListItemCollection();
                for (int intCount = 0; intCount < aSource.Items.Count; intCount++)
                {
                    if (aSource.Items[intCount].Selected == true)
                        licCollection.Add(aSource.Items[intCount]);
                }

                for (int intCount = 0; intCount < licCollection.Count; intCount++)
                {
                    aSource.Items.Remove(licCollection[intCount]);
                    aTarget.Items.Add(licCollection[intCount]);
                }

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }
            finally
            {
                licCollection = null;
            }

        }

        private void AddRemoveAll(ListBox aSource, ListBox aTarget)
        {

            try
            {

                foreach (ListItem item in aSource.Items)
                {
                    aTarget.Items.Add(item);
                }
                aSource.Items.Clear();

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }

        }
       
       

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            AddRemoveAll(lstAll,lstSelected);
        }

        protected void ddlDepName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            LoadDiptwiseCardNo();
            lstSelected.Items.Clear();
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            AddRemoveItem(lstSelected,lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            AddRemoveAll(lstSelected,lstAll);
        }

        protected void btnpreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdbAll.Checked == true || rdbDeptWise.Checked == true)
                {
                    if (rdbDeptWise.Checked == true && ddlDepName.SelectedItem.Text.Trim() == "") { lblMessage.InnerText = "warning-> Please select any Department";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true); 
                        return;
                    }
                    if (lstSelected.Items.Count < 1) { lblMessage.InnerText = "warning-> Please select any Card No";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true); 
                        return;
                    }
                    string setPredicate = "";
                    for (int b = 0; b < lstSelected.Items.Count; b++)
                    {

                        if (b == 0 && b == lstSelected.Items.Count - 1)
                        {
                            setPredicate = "in('" + lstSelected.Items[b].Value + "')";
                        }
                        else if (b == 0 && b != lstSelected.Items.Count - 1)
                        {
                            setPredicate += "in ('" + lstSelected.Items[b].Value + "'";
                        }
                        else if (b != 0 && b == lstSelected.Items.Count - 1)
                        {
                            setPredicate += ",'" + lstSelected.Items[b].Value + "')";
                        }
                        else setPredicate += ",'" + lstSelected.Items[b].Value + "'";
                    }
                    DataTable dt = new DataTable();
                    //sqlDB.fillDataTable("Select CompanyNameBangla,AddressBangla,SUBSTRING(EmpCardNo,8,16) as EmpCardNO,EmpNameBn,DptNameBn,DsgNameBn,SftNameBangla,convert(varchar(11),EmpJoiningDate,103) as EmpJoiningDate,FatherNameBn,PerVillageBangla,PerPOBangla,ThaNameBangla,DstBangla,MobileNo,EmpPicture From v_EmployeeDetails where SN " + setPredicate + " and ActiveSalary='True'  order by EmpCardNo", dt);
                    sqlDB.fillDataTable("Select EmpTypeId,CompanyName,Address,EmpCardNO,EmpName,DptName,DsgName,SftNameBangla,Format(EmpJoiningDate,'dd-MMM-yyyy') as EmpJoiningDate,BloodGroup,FORMAT(ExpireDate,'dd-MMM-yyyy') as ExpireDate,SignatureImage,FatherNameBn,PerVillageBangla,PerPOBangla,ThaNameBangla,DstBangla,MobileNo,EmpPicture,CompanyLogo From v_EmployeeDetails where SN " + setPredicate + " and ActiveSalary='True'  order by EmpCardNo", dt);
                    Session["__WorkerID__"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        Session["__ReportView__"] = rblreportview.SelectedValue;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=WorkerIDCard');", true);  //Open New Tab for Sever side code
                    }
                }

                else if (rdbIndividual.Checked == true)
                {
                    if (ddlEmpCardNo.SelectedIndex == -1)
                    {
                        lblMessage.InnerText = "warning->Any Worker Are Not Available!";
                        ddlEmpCardNo.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                        return;
                    }
                    if (ddlEmpCardNo.SelectedItem.Text.Trim() == "")
                    {
                        lblMessage.InnerText = "warning->Please Select Any Employee!";
                        ddlEmpCardNo.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                        return;
                    }
                    DataTable dt = new DataTable();
                    //sqlDB.fillDataTable("Select  CompanyNameBangla,AddressBangla,SUBSTRING(EmpCardNo,8,16) as EmpCardNO,EmpNameBn,DptNameBn,DsgNameBn,SftNameBangla,convert(varchar(11),EmpJoiningDate,103) as EmpJoiningDate,FatherNameBn,PerVillageBangla,PerPOBangla,ThaNameBangla,DstBangla,MobileNo,EmpPicture From v_EmployeeDetails where SN=" + ddlEmpCardNo.SelectedValue + " and ActiveSalary='True'  order by EmpCardNo", dt);
                    sqlDB.fillDataTable("Select EmpTypeId,CompanyName,Address,EmpCardNO,EmpName,DptName,DsgName,SftNameBangla,Format(EmpJoiningDate,'dd-MMM-yyyy') as EmpJoiningDate,BloodGroup,FORMAT(ExpireDate,'dd-MMM-yyyy') as ExpireDate,SignatureImage,FatherNameBn,PerVillageBangla,PerPOBangla,ThaNameBangla,DstBangla,MobileNo,EmpPicture,CompanyLogo From v_EmployeeDetails where SN=" + ddlEmpCardNo.SelectedValue + " and ActiveSalary='True'  order by EmpCardNo", dt);
                    Session["__WorkerID__"] = dt;
                    if(dt.Rows.Count>0)
                    {
                        Session["__ReportView__"] = rblreportview.SelectedValue;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=WorkerIDCard-" + ddlEmpCardNo.SelectedValue + "');", true);  //Open New Tab for Sever side code
                    }
                    
                }

            }
            catch { }
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbAll.Checked == true)
                LoadAllWorker();
            else if (rdbDeptWise.Checked == true)
            {
                lstAll.Items.Clear();
                lstSelected.Items.Clear();
                CompanyID = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
                classes.commonTask.loadDepartmentListByCompany(ddlDepName, CompanyID);
            }
            else if (rdbIndividual.Checked == true)
               LoadEmpCardNo(ddlEmpCardNo);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);


        }

    }
}