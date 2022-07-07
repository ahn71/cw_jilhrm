using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComplexScriptingSystem;
using System.Data.SqlClient;
using adviitRuntimeScripting;
using System.Data;
using System.Web.Services;
using System.Web.Security;
using SigmaERP.classes;

namespace SigmaERP
{
    public partial class Glory : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            try
            {
             
                   
                    HttpCookie getCookies = Request.Cookies["userInfo"];
                    if (getCookies == null || getCookies.Value == "")
                    {
                        Response.Redirect("~/ControlPanel/Login.aspx");
                    
                    }
                    else
                    {

                        UserName.InnerText = getCookies["__getFirstName__"].ToString();
                        ViewState["__getUserId__"] = getCookies["__getUserId__"].ToString();
                        Session["__GetUID__"] = ViewState["__getUserId__"].ToString();
                        Session["__GetLvOnlyDpt__"] = getCookies["__getLvOnlyDpt__"].ToString();
                        Session["__GetODOnlyDpt__"] = getCookies["__getODOnlyDpt__"].ToString();
                        Session["__GetDptId__"] = getCookies["__getDptId__"].ToString();
                        Session["__GetEmpId__"] = getCookies["__getEmpId__"].ToString();
                        Session["__GetUserType__"] = ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString());
                        UserType.InnerText = "  " + ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString());
                        GSName.InnerText = getCookies["__CompanyName__"].ToString();

                        Session["__GetCompanyId__"]=ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                        if (Session["__GetUserType__"].ToString().Equals("User"))
                        {
                            try
                            {
                                
                                
                                string[] path = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
                                string a = path[path.Length - 1].ToString();

                                if (!(a == "Notification.aspx" || a == "default.aspx" || a == "leave_default.aspx" || a == "leave_default.aspx" || a == "attendance_default.aspx"))
                                {
                                if (a ==  "daily_movement.aspx" || a == "monthly_in_out_report.aspx" || a == "out_duty.aspx" || a == "outduty_report.aspx" 
                                    ||  a == "aplication.aspx" || a == "all_leave_list.aspx" ||a== "out_duty_list.aspx" || a == "yearly_leaveStatus_report.aspx")//|| a == "yearly_leaveStatus_report.aspx"
                                    return;                                   
                                    //DataTable dt = new DataTable();
                                    //dt = checkUserPrivilege.PanelWiseUserPrivilege(getCookies["__getUserId__"].ToString(), "", a);
                                    ////dt = checkUserPrivilege.PanelWiseUserPrivilegeByUserTypeAndPage(getCookies["__getUserType__"].ToString(), a);
                                    //if (dt == null || dt.Rows.Count == 0)
                                        Response.Redirect("~/default.aspx");
                                }
                              
                                
                                
                            }
                            catch { Response.Redirect("~/default.aspx"); }
                        }

                        try
                        {
                            if (Session["__SelectedUser__"].ToString() != null) lblSelectUserForChat.Text = Session["__SelectedUser__"].ToString();

                        }
                        catch { }
                    }

                    if (!IsPostBack) loadLoginUserList();

                    try
                    {
                        if (Session["__DCB__"].ToString() == "True") divChatBox.Visible = true;

                        else divChatBox.Visible = false;
                    }
                    catch { }

            }
            catch (Exception ex)
            {
                Response.Redirect("~/ControlPanel/Login.aspx");
            }
        }

        public static string getConnectionString()
        {
            if (System.Environment.MachineName.Equals("NAYEM-PC"))
                return System.Configuration.ConfigurationManager.ConnectionStrings["local1"].ConnectionString;
            else return System.Configuration.ConfigurationManager.ConnectionStrings["local"].ConnectionString;
        }
        public static string getConnectionString2()
        {
           return System.Configuration.ConfigurationManager.ConnectionStrings["local2"].ConnectionString;
        }
        //public static string getConnectionString2()
        //{
        //    if (System.Environment.MachineName.Equals("ROHOL-PC"))
        //        return System.Configuration.ConfigurationManager.ConnectionStrings["local2"].ConnectionString;
        //    return System.Configuration.ConfigurationManager.ConnectionStrings["local3"].ConnectionString;
        //}


        public void btnTextSend_Click(object sender, EventArgs e)
        {
            try
            {
                Session["__TxUserId__"] = ViewState["__getUserId__"].ToString();
                string getTime = DateTime.Now.ToLongTimeString();
                SqlCommand cmd = new SqlCommand("insert into Mail_ChatInfo (Text,TxUserId,RxUserId,CDate,CTime,Status) values ('" + txtChat.Text + "'," + ViewState["__getUserId__"].ToString() + "," + chkLoginUseList.SelectedItem.Value.ToString() + ",'" + convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")) + "','" + getTime + "','0')", sqlDB.connection);
                int result =cmd.ExecuteNonQuery();
                txtChat.Text = "";
                if (result > 0)
                    lblgetStatus.Text ="Successfully send your message";
            }
            catch { }
        }

        private void loadLoginUserList()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select UserId,FirstName from UserAccount where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' AND IsLogin='true' AND UserId !=" + ViewState["__getUserId__"].ToString() + "", dt = new DataTable());
                chkLoginUseList.DataValueField = "UserId";
                chkLoginUseList.DataTextField = "FirstName";
                chkLoginUseList.DataSource = dt;
                chkLoginUseList.DataBind();

            }
            catch { }
        }

        protected void chkLoginUseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblSelectUserForChat.Text = chkLoginUseList.SelectedItem.Text.ToString();
            }
            catch { }
        }

        protected void btnChat_Click(object sender, EventArgs e)
        {
            divChatBox.Visible = true;
            Session["__DCB__"] = "True";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                HttpCookie setCookies = new HttpCookie("userInfo");
                setCookies.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(setCookies);
                FormsAuthentication.SignOut();
                Response.Redirect("~/ControlPanel/Login.aspx",false);
            }
            catch (Exception ex) { }
        }
        protected void btnClose_ChatBox_Click(object sender, EventArgs e)
        {
            divChatBox.Visible = false;
            Session["__DCB__"] = "False";
        }


        

       


        
    }
}