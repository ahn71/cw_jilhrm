using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ComplexScriptingSystem;

namespace SigmaERP.ControlPanel
{
    public partial class user_privilege : System.Web.UI.Page
    {
        SqlCommand cmd;
        SqlDataAdapter da;
        GridViewRow gvr;
        CheckBox chkReadAction;
        CheckBox chkWriteAction;
        CheckBox chkUpdateAction;
        CheckBox chkDeleteAction;
        CheckBox chkAllAction;
        string getPageName;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {                
                setPrivilege();   
             
                if (Session["__DptId__"]!=null)
                {
                   
                    loadExistsUser();
                    dlExistsUser.SelectedValue = Session["__UserId__"].ToString();

                    Session["__DptId__"] = null;
                    Session["__UserId__"] = null;
                }
             
               
            }
        }
        private void loadCompany()
        {
            try
            {

                DataTable dt = new DataTable();
                SQLOperation.selectBySetCommandInDatatable("select CompanyId,CompanyName from HRD_CompanyInfo", dt, sqlDB.connection);
                ddlCompany.DataTextField = "CompanyName";
                ddlCompany.DataValueField = "CompanyId";
                ddlCompany.DataSource = dt;
                ddlCompany.DataBind();

                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
              //  if (!ViewState["__UserType__"].ToString().Equals("XgWQYNdKrQtMhGZAvmF98Q==")) ddlCompany.Enabled = false;
            }
            catch { }
        }

        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__getUserId__"] = getUserId;
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                {
                    DataTable dt = new DataTable();                
                    sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='user_privilege.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);                    
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            divRightBox.Controls.Clear();
                            divRightBox.Style.Add("color", "red");
                            divRightBox.Style.Add("font-family", "ms");
                            divRightBox.Style.Add("font-size", "70px");
                            divRightBox.Style.Add("font-weight", "Bold");
                            divRightBox.InnerText = "SORRY" + "\n\n" + "You Have Not Any Access Privilege!";
                            return;
                        }

                    }
                    else
                    {
                        divRightBox.Controls.Clear();
                        divRightBox.Style.Add("color", "red");
                        divRightBox.Style.Add("font-family", "ms");
                        divRightBox.Style.Add("font-size", "70px");
                        divRightBox.Style.Add("font-weight", "Bold");
                        divRightBox.InnerText = "SORRY" + "\n\n" + "You Have Not Any Access Privilege!";
                        return;
                    }
                }
                loadCompany();
               // classes.commonTask.loadDepartmentListByCompany(ddlDepartment, ddlCompany.SelectedValue);
              loadExistsUser();               
            }
            catch { }
        
        }

        protected void rbProjectModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dlExistsUser.SelectedValue == "0001" || rbProjectModule.SelectedValue == "0")
                {
                    gvPageList.DataSource = null;
                    gvPageList.DataBind();
                    
                }
                else loadPrivilegeInfo();
                
            }
            catch { }
        }

      

        private void loadExistsUser()
        {
            try
            {
                DataTable dt = new DataTable();
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin"))
                {
                    sqlDB.fillDataTable("select UserId,EmpName as FullName from v_UserAccount where Status='true' AND CompanyId='" + ddlCompany.SelectedValue + "' AND UserType not in ('XgWQYNdKrQtMhGZAvmF98Q==','CwNUgpy/R6vrIxIvnT1Brw==')   ", dt);
                }
               
                else if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                {
                    sqlDB.fillDataTable("select UserId,EmpName as FullName from v_UserAccount where UserId='" + ViewState["__getUserId__"] + "' And Status='true' AND CompanyId='" + ddlCompany.SelectedValue + "' ", dt); // Admin=dXnAt+i7Sr8=
                }

             //   sqlDB.fillDataTable("select UserId,(FirstName +' ' +LastName) as FullName from UserAccount where UserType='Admin'", dt);

                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Any user is not exists in this department";
                }
                else
                {
                    dlExistsUser.DataTextField = "FullName";
                    dlExistsUser.DataValueField = "UserId";
                    dlExistsUser.DataSource = dt;
                    dlExistsUser.DataBind();
                    dlExistsUser.Items.Insert(0, new ListItem(" ", "0001"));
                }
            }
            catch { }
        
        }

        protected void dlExistsUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadModulePrivilegeInfo();

            if (dlExistsUser.SelectedValue == "0001" || rbProjectModule.SelectedValue == "0")           
            loadPrivilegeInfo();
        }
        private void loadModulePrivilegeInfo() 
        {
             DataTable dt = new DataTable();
             sqlDB.fillDataTable("select Settings,Personnel,Leave,Attendance,Payroll,Tools  from   UserPrivilegeByModule where UserId=" + dlExistsUser.SelectedValue.ToString() + "", dt);
                if (dt.Rows.Count > 0)
                {
                   ckblModuleList.Items[0].Selected=  bool.Parse(dt.Rows[0]["Settings"].ToString());
                   ckblModuleList.Items[1].Selected = bool.Parse(dt.Rows[0]["Personnel"].ToString());
                   ckblModuleList.Items[2].Selected = bool.Parse(dt.Rows[0]["Leave"].ToString());
                   ckblModuleList.Items[3].Selected = bool.Parse(dt.Rows[0]["Attendance"].ToString());
                   ckblModuleList.Items[4].Selected = bool.Parse(dt.Rows[0]["Payroll"].ToString());
                   ckblModuleList.Items[5].Selected = bool.Parse(dt.Rows[0]["Tools"].ToString());                       
                   
                }
                else
                    ckblModuleList.ClearSelection();

        }

        private void loadPrivilegeInfo()
        {
            try
            {
                lblMessage.InnerText = " ";
                lblUserType.Text = " ";
                if (dlExistsUser.SelectedValue.ToString() == "0001")
                {                 
                    btnDelete.Visible = false;                 
                }
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from  v_UserPrivilegeList where UserId=" + dlExistsUser.SelectedValue.ToString() + " AND ModuleId=" + rbProjectModule.SelectedValue + "", dt);
                if (dt.Rows.Count == 0)
                {
                    sqlDB.fillDataTable("select ModuleId,PageTitle,ModulePageName,cast(0 As Bit) as ReadAction,cast(0 As Bit) as WriteAction,cast(0 As Bit) as UpdateAction,cast(0 As Bit) as DeleteAction,cast(0 As Bit) as AllAction from ModulePageInfo where ModuleId=" + rbProjectModule.SelectedValue + " Order by Ordering", dt = new DataTable());
                    gvPageList.DataSource = dt;
                    gvPageList.DataBind();

                    SavePageInfoInUserPrivilege(dt);

                    btnDelete.Visible = false;
                }
                else
                {
                    gvPageList.DataSource = dt;
                    gvPageList.DataBind();
                }

                //sqlDB.fillDataTable("select UserType from UserAccount where UserId="+dlExistsUser.SelectedValue.ToString()+"",dt=new DataTable ());
              //  lblUserType.Text ="User : "+ComplexLetters.getEntangledLetters(dt.Rows[0]["UserType"].ToString());
            }
            catch { }
        }

        private void SavePageInfoInUserPrivilege(DataTable DtPageList)
        {
            try
            {
                for(sbyte b=0;b<DtPageList.Rows.Count;b++)
                {
                    cmd = new SqlCommand("insert into UserPrivilege  (UserId,ModuleId,ModulePageName,ReadAction,WriteAction,UpdateAction,DeleteAction,AllAction) values " +
                    "(" + dlExistsUser.SelectedValue + "," + rbProjectModule.SelectedValue + ",'" + DtPageList.Rows[b]["ModulePageName"].ToString() + "',0,0,0,0,0)", sqlDB.connection);
                    int result=cmd.ExecuteNonQuery();

                 
                }
            }
            
            catch { }
        }

        bool saveStatus = false;
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (validationBasket() == false)
            {
                rbProjectModule.SelectedIndex = 0;
                return;
            }
            //saveMasterTransaction();
            //saveReportsPage();
            if (saveStatus)
            {
                lblMessage.InnerText = "success->Successfully Privilege Set";
                rbProjectModule.SelectedIndex = 0;
                loadPrivilegeInfo();
            }
            
        }

        private bool validationBasket()
        {
            try
            {
                bool status=false;
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserType__"].ToString();

                if (getUserId == "Admin")
                {
                    lblMessage.InnerText = "warning->You are not super admin,Supper admin is required for set privilege."; status = false; return false;
                
                }
                if (dlExistsUser.SelectedValue.ToString() == "0001")    // 0001 is a value this is denoted " " string selected as a user 
                {
                    lblMessage.InnerText = "Please chose an user"; return false;
                }
               
                return true;

            }
            catch { return false; }
        
        }
       
        protected void chkReadAction_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvr = ((GridViewRow)((Control)sender).Parent.Parent);
                chkReadAction = (CheckBox)gvr.Cells[1].FindControl("chkReadAction");
                chkWriteAction = (CheckBox)gvr.Cells[1].FindControl("chkWriteAction");
                chkUpdateAction = (CheckBox)gvr.Cells[1].FindControl("chkUpdateAction");
                chkDeleteAction = (CheckBox)gvr.Cells[1].FindControl("chkDeleteAction");
                chkAllAction = (CheckBox)gvr.Cells[1].FindControl("chkAllAction");

                if (chkReadAction.Checked && chkWriteAction.Checked && chkUpdateAction.Checked && chkDeleteAction.Checked)
                {
                    chkAllAction.Checked = true;
                }
                else chkAllAction.Checked = false;

                int index_row = gvr.RowIndex;

                getPageName = gvPageList.DataKeys[index_row].Value.ToString();
             
                cmd = new SqlCommand("Update UserPrivilege set ReadAction='" + chkReadAction.Checked + "',AllAction='"+chkAllAction.Checked+"' where UserId=" + dlExistsUser.SelectedValue + " AND ModulePageName='"+getPageName+"'", sqlDB.connection);
                int result=cmd.ExecuteNonQuery();
             

            }
            catch { }
        }

        protected void chkWriteAction_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                gvr = ((GridViewRow)((Control)sender).Parent.Parent);
                chkReadAction = (CheckBox)gvr.Cells[1].FindControl("chkReadAction");
                chkWriteAction = (CheckBox)gvr.Cells[1].FindControl("chkWriteAction");
                chkUpdateAction = (CheckBox)gvr.Cells[1].FindControl("chkUpdateAction");
                chkDeleteAction = (CheckBox)gvr.Cells[1].FindControl("chkDeleteAction");
                chkAllAction = (CheckBox)gvr.Cells[1].FindControl("chkAllAction");

                if (chkReadAction.Checked && chkWriteAction.Checked && chkUpdateAction.Checked && chkDeleteAction.Checked)
                {
                    chkAllAction.Checked = true;
                }
                else chkAllAction.Checked = false;

                int index_row = gvr.RowIndex;

                getPageName = gvPageList.DataKeys[index_row].Value.ToString();

                cmd = new SqlCommand("Update UserPrivilege set WriteAction='" + chkWriteAction.Checked + "',AllAction='" + chkAllAction.Checked + "' where UserId=" + dlExistsUser.SelectedValue + " AND ModulePageName='" + getPageName + "'", sqlDB.connection);
                int result=cmd.ExecuteNonQuery();
           
            }
            catch { }
        }


        protected void chkUpdateAction_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                gvr = ((GridViewRow)((Control)sender).Parent.Parent);
                chkReadAction = (CheckBox)gvr.Cells[1].FindControl("chkReadAction");
                chkWriteAction = (CheckBox)gvr.Cells[1].FindControl("chkWriteAction");
                chkUpdateAction = (CheckBox)gvr.Cells[1].FindControl("chkUpdateAction");
                chkDeleteAction = (CheckBox)gvr.Cells[1].FindControl("chkDeleteAction");
                chkAllAction = (CheckBox)gvr.Cells[1].FindControl("chkAllAction");

                if (chkReadAction.Checked && chkWriteAction.Checked && chkUpdateAction.Checked && chkDeleteAction.Checked)
                {
                    chkAllAction.Checked = true;
                }
                else chkAllAction.Checked = false;

                int index_row = gvr.RowIndex;

                getPageName = gvPageList.DataKeys[index_row].Value.ToString();

                cmd = new SqlCommand("Update UserPrivilege set UpdateAction='" + chkUpdateAction.Checked + "',AllAction='" + chkAllAction.Checked + "' where UserId=" + dlExistsUser.SelectedValue + " AND ModulePageName='" + getPageName + "'", sqlDB.connection);
                int result = cmd.ExecuteNonQuery();
         
            }
            catch { }
        }

        protected void chkDeleteAction_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                gvr = ((GridViewRow)((Control)sender).Parent.Parent);
                chkReadAction = (CheckBox)gvr.Cells[1].FindControl("chkReadAction");
                chkWriteAction = (CheckBox)gvr.Cells[1].FindControl("chkWriteAction");
                chkUpdateAction = (CheckBox)gvr.Cells[1].FindControl("chkUpdateAction");
                chkDeleteAction = (CheckBox)gvr.Cells[1].FindControl("chkDeleteAction");
                chkAllAction = (CheckBox)gvr.Cells[1].FindControl("chkAllAction");

                if (chkReadAction.Checked && chkWriteAction.Checked && chkUpdateAction.Checked && chkDeleteAction.Checked)
                {
                    chkAllAction.Checked = true;
                }
                else chkAllAction.Checked = false;

                int index_row = gvr.RowIndex;

                getPageName = gvPageList.DataKeys[index_row].Value.ToString();

                cmd = new SqlCommand("Update UserPrivilege set DeleteAction='" + chkDeleteAction.Checked + "',AllAction='" + chkAllAction.Checked + "' where UserId=" + dlExistsUser.SelectedValue + " AND ModulePageName='" + getPageName + "'", sqlDB.connection);
                int result = cmd.ExecuteNonQuery();
         
            }
            catch { }
        }

        protected void chkAllAction_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                gvr = ((GridViewRow)((Control)sender).Parent.Parent);
                int index_row = gvr.RowIndex;


                getPageName = gvPageList.DataKeys[index_row].Value.ToString();

                chkReadAction = (CheckBox)gvr.Cells[1].FindControl("chkReadAction");
                chkWriteAction = (CheckBox)gvr.Cells[1].FindControl("chkWriteAction");
                chkUpdateAction = (CheckBox)gvr.Cells[1].FindControl("chkUpdateAction");
                chkDeleteAction = (CheckBox)gvr.Cells[1].FindControl("chkDeleteAction");
                chkAllAction = (CheckBox)gvr.Cells[1].FindControl("chkAllAction");

                byte Action = (chkAllAction.Checked) ? (byte)1 : (byte)0;
                bool Status = (Action == 1) ? true : false;

                chkReadAction.Checked = Status;
                chkWriteAction.Checked = Status;
                chkUpdateAction.Checked = Status;
                chkDeleteAction.Checked = Status;
                chkAllAction.Checked = Status;

                cmd = new SqlCommand("update UserPrivilege set ReadAction=" + Action + ",WriteAction=" + Action + ",UpdateAction=" + Action + ",DeleteAction=" + Action + ",AllAction=" + Action + " where UserId="+dlExistsUser.SelectedValue+" AND ModulePageName='" + getPageName + "'", sqlDB.connection);
                int result = cmd.ExecuteNonQuery();
           
            }
            catch { }
        }

        protected void chkGenerateAction_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                GridViewRow gvr = ((GridViewRow)((Control)sender).Parent.Parent);
                int index_row = gvr.RowIndex;

                int getSL = Convert.ToInt32(gvPageList.DataKeys[index_row].Value.ToString());

                CheckBox chk = (CheckBox)gvPageList.Rows[index_row].Cells[1].FindControl("chkGenerateAction");

                byte Action = (chk.Checked) ? (byte)1 : (byte)0;

                cmd = new SqlCommand("Update UserPrivilege set GenerateAction=" + Action + " where SL=" + getSL + "", sqlDB.connection);
                int result=cmd.ExecuteNonQuery();
        
            }
            catch { }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
               

                SQLOperation.forDeleteRecordByIdentifier("UserPrivilege", "UserId", dlExistsUser.SelectedValue.ToString(), sqlDB.connection);
               
                gvPageList.DataSource = null;
                gvPageList.DataBind();
               
                btnDelete.Visible = false;
                
            }
            catch { }
        }

        protected void gvPageList_RowDataBound(object sender, GridViewRowEventArgs e)
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
           

            //e.Row.Attributes.Add("onmouseover", "style.backgroundColor='#ffffff'");
            //e.Row.Attributes.Add("onmouseout", "style.backgroundColor=''");

        }

        protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)gvPageList.HeaderRow.FindControl("chkboxSelectAll");
            foreach (GridViewRow row in gvPageList.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkDeleteAction");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadExistsUser();
        }

        protected void ckblModuleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmdDel= new SqlCommand("delete from  UserPrivilegeByModule where UserId='"+dlExistsUser.SelectedValue+"'", sqlDB.connection);
               cmdDel.ExecuteNonQuery();
                cmd = new SqlCommand("insert into UserPrivilegeByModule values(" + dlExistsUser.SelectedValue + "," +boolToInt(ckblModuleList.Items[0].Selected.ToString())+ "," +
                    boolToInt(ckblModuleList.Items[1].Selected.ToString())+ "," +boolToInt(ckblModuleList.Items[2].Selected.ToString()) + "," +
                    boolToInt(ckblModuleList.Items[3].Selected.ToString())+"," + boolToInt(ckblModuleList.Items[4].Selected.ToString()) + "," +
                   boolToInt(ckblModuleList.Items[5].Selected.ToString()) + ")", sqlDB.connection);
                int result = cmd.ExecuteNonQuery();
            }
            catch { }
        }

        private string boolToInt(string value)
        {
            value = (value.Equals("True")) ? "1" : "0";
            return value;
        }
 
    }
}