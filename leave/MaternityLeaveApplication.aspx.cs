using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class MaternityLeaveApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.loadMaternityEmpCardNo(ddlCardNo,"1");
            }
        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select  EmpCardNo from v_Leave_LeaveApplication where EmpCardNo like", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            //btnpreview.CssClass = "";

                            //btnpreview.Enabled = false;


                        }
                    }
                }

            }
            catch { }

        }

        protected void rdbWorker_CheckedChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            rdbStaff.Checked = false;
            classes.commonTask.loadMaternityEmpCardNo(ddlCardNo, "1");
        }

        protected void rdbStaff_CheckedChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            rdbWorker.Checked = false;
            classes.commonTask.loadMaternityEmpCardNo(ddlCardNo, "2");
        }
         
        protected void btnFind_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select EmpCardNo from v_Leave_LeaveApplication where EmpCardNo like'%" + txtCardNo.Text.Trim() + "'", dt);
            if(dt.Rows.Count<1)
            {
                lblMessage.InnerText = "warning->Please Type Valid CardNo";
                return;
            }
            for (int k = 0; k < ddlCardNo.Items.Count; k++)
            {
                if ((ddlCardNo.Items[k].Text == dt.Rows[0]["EmpCardNo"].ToString()))
                {
                    ddlCardNo.SelectedIndex = k;
                    break;
                }
            }
            if (ddlCardNo.SelectedItem.Text != dt.Rows[0]["EmpCardNo"].ToString())
            {
                lblMessage.InnerText = "warning->Please Type Valid CardNo";
                return;
            }
        }

        protected void btnMaternityApp_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation() == false) return;
                DataTable dtRunning = new DataTable();
                sqlDB.fillDataTable("Select  SUBSTRING(EmpCardNo,8,12) as EmpCardNo,EmpNameBn,DptNameBn,DsgNameBn,CompanyNameBangla,AddressBangla,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,FORMAT(ToDate,'dd-MM-yyyy') as ToDate,FORMAT(FromDate,'dd-MM-yyyy') as FromDate,FORMAT(PrasaberaDate,'dd-MM-yyyy') as PrasaberaDate,FORMAT(EntryDate,'dd-MM-yyyy') as EntryDate From v_Leave_LeaveApplication where EmpCardNo = '" + ddlCardNo.SelectedValue + "'", dtRunning);
               /* dtRunning.Rows.Clear();
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select EmpId, EmpCardNo,EmpNameBn,DptNameBn,DsgNameBn,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate From v_Personnel_EmpCurrentStatus where SN=" + ddlCardNo.SelectedValue + "", dt);
                DataTable lnCode = new DataTable();
                sqlDB.fillDataTable("Select Max(LACode) as LACode,EmpId From v_Leave_LeaveApplication where EmpId='" + dt.Rows[0].ItemArray[0].ToString() + "' and LeaveName='M/L' group by EmpId", lnCode);
                if (lnCode.Rows.Count > 0)
                {
                    DataTable dtdate = new DataTable();
                    sqlDB.fillDataTable("Select  Format(ToDate,'dd-MM-yyyy') as ToDate,Format(FromDate,'dd-MM-yyyy') as FromDate ,Format(PrasaberaDate,'dd-MM-yyyy') as PrasaberaDate From v_Leave_LeaveApplication where LACode=" + lnCode.Rows[0].ItemArray[0].ToString() + "", dtdate);
                    dtRunning.Columns.Remove("ToDate");
                    dtRunning.Columns.Remove("FromDate");
                    dtRunning.Columns.Remove("PrasaberaDate");
                    dtRunning.Columns.Add("ToDate",typeof(string));
                    dtRunning.Columns.Add("FromDate", typeof(string));
                    dtRunning.Columns.Add("PrasaberaDate", typeof(string));
                    dtRunning.Rows.Add(dt.Rows[0]["EmpCardNo"].ToString(), dt.Rows[0]["EmpNameBn"].ToString(), dt.Rows[0]["DptNameBn"].ToString(), dt.Rows[0]["DsgNameBn"].ToString(), dt.Rows[0]["EmpJoiningDate"].ToString(), dtdate.Rows[0]["ToDate"].ToString(), dtdate.Rows[0]["FromDate"].ToString(), dtdate.Rows[0]["PrasaberaDate"].ToString());
                    //DataView view = new DataView(dtRunning);
                    //DataTable dtSpecificCols = view.ToTable(false, new string[] { "EmpCardNo", "EmpNameBn", "DptNameBn", "DsgNameBn", "EmpJoiningDate", "Format(ToDate,'dd-MM-yyyy') as ToDate", "Format(FromDate,'dd-MM-yyyy')", "Format(PrasaberaDate,'dd-MM-yyyy')" });
                    */
                if (dtRunning.Rows.Count > 0)
                    {
                        Session["__LeaveApplication__"] = dtRunning;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=MaternityLeaveApplication');", true);  //Open New Tab for Sever side code
                    }
                    else
                        lblMessage.InnerText = "warning->No Data Available";
                //}
                //else
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        Session["__LeaveApplication__"] = dt;
                //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveApplication');", true);  //Open New Tab for Sever side code
                //    }
                //    else
                //        lblMessage.InnerText = "warning->No Data Available";
                //}

               
            }
            catch { }
        }

        protected void btnDocCerLetter_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation() == false) return;
                DataTable dtRunning = new DataTable();
                sqlDB.fillDataTable("Select SUBSTRING(EmpCardNo,8,12) as EmpCardNo,EmpNameBn,DptNameBn,DsgNameBn,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,FORMAT(FromDate,'dd-MM-yyyy') as FromDate,Format(PregnantDate,'dd-MM-yyyy') as PregnantDate, FORMAT(PrasaberaDate,'dd-MM-yyyy') as PrasaberaDate,NumberofChild From v_Leave_LeaveApplication where EmpCardNo = '" + ddlCardNo.SelectedValue + "'", dtRunning);
                /*dtRunning.Rows.Clear();
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select EmpId, EmpCardNo,EmpNameBn,DptNameBn,DsgNameBn,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,CONVERT(varchar(10), NumberofChild) as NumberofChild From v_Personnel_EmpCurrentStatus where SN=" + ddlCardNo.SelectedValue + "", dt);
                DataTable lnCode = new DataTable();
                sqlDB.fillDataTable("Select Max(LACode) as LACode,EmpId From v_Leave_LeaveApplication where EmpId='" + dt.Rows[0].ItemArray[0].ToString() + "' and LeaveName='M/L' group by EmpId", lnCode);
                if (lnCode.Rows.Count > 0)
                {
                    DataTable dtdate = new DataTable();
                    sqlDB.fillDataTable("Select  Format(PregnantDate,'dd-MM-yyyy') as PregnantDate,Format(FromDate,'dd-MM-yyyy') as FromDate ,Format(PrasaberaDate,'dd-MM-yyyy') as PrasaberaDate From v_Leave_LeaveApplication where LACode=" + lnCode.Rows[0].ItemArray[0].ToString() + "", dtdate);
                    dtRunning.Columns.Remove("PregnantDate");
                    dtRunning.Columns.Remove("FromDate");
                    dtRunning.Columns.Remove("PrasaberaDate");
                    dtRunning.Columns.Remove("NumberofChild");
                    dtRunning.Columns.Add("NumberofChild", typeof(string));
                    dtRunning.Columns.Add("FromDate", typeof(string));
                    dtRunning.Columns.Add("PregnantDate", typeof(string));
                    dtRunning.Columns.Add("PrasaberaDate", typeof(string));
                   
                    dtRunning.Rows.Add(dt.Rows[0]["EmpCardNo"].ToString(), dt.Rows[0]["EmpNameBn"].ToString(), dt.Rows[0]["DptNameBn"].ToString(), dt.Rows[0]["DsgNameBn"].ToString(), dt.Rows[0]["EmpJoiningDate"].ToString(), dt.Rows[0]["NumberofChild"].ToString(), dtdate.Rows[0]["FromDate"].ToString(), dtdate.Rows[0]["PregnantDate"].ToString(), dtdate.Rows[0]["PrasaberaDate"].ToString());
                    //DataView view = new DataView(dtRunning);
                    //DataTable dtSpecificCols = view.ToTable(false, new string[] { "EmpCardNo", "EmpNameBn", "DptNameBn", "DsgNameBn", "EmpJoiningDate", "Format(ToDate,'dd-MM-yyyy') as ToDate", "Format(FromDate,'dd-MM-yyyy')", "Format(PrasaberaDate,'dd-MM-yyyy')" });
                    */
                    if (dtRunning.Rows.Count > 0)
                    {
                        Session["__DoctorCertificationLetter__"] = dtRunning;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=DoctorCertificationLetter');", true);  //Open New Tab for Sever side code
                    }
                    else
                        lblMessage.InnerText = "warning->No Data Available";
                //}
                //else
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        Session["__DoctorCertificationLetter__"] = dt;
                //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=DoctorCertificationLetter');", true);  //Open New Tab for Sever side code
                //    }
                //    else
                //        lblMessage.InnerText = "warning->No Data Available";
                //}


            }
            catch { }
        }

        protected void btnGrantedDoctor_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation() == false) return;
                DataTable dtRunning = new DataTable();
                /* sqlDB.fillDataTable("Select EmpId, EmpCardNo,EmpNameBn,DptNameBn,DsgNameBn,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,NumberofChild,PregnantDate,FromDate,PrasaberaDate,ToDate From v_Leave_LeaveApplication where SN=" + ddlCardNo.SelectedValue + "", dtRunning);
                 dtRunning.Rows.Clear();
                 DataTable dt = new DataTable();
                 sqlDB.fillDataTable("Select EmpId, EmpCardNo,EmpNameBn,DptNameBn,DsgNameBn,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,CONVERT(varchar(10), NumberofChild) as NumberofChild From v_Personnel_EmpCurrentStatus where SN=" + ddlCardNo.SelectedValue + "", dt);
                 DataTable lnCode = new DataTable();
                 sqlDB.fillDataTable("Select Max(LACode) as LACode,EmpId From v_Leave_LeaveApplication where EmpId='" + dt.Rows[0].ItemArray[0].ToString() + "' and LeaveName='M/L' group by EmpId", lnCode);
               if (lnCode.Rows.Count > 0)
                 {
                     DataTable dtdate = new DataTable();
                     sqlDB.fillDataTable("Select  Format(PregnantDate,'dd-MM-yyyy') as PregnantDate,Format(FromDate,'dd-MM-yyyy') as FromDate ,Format(PrasaberaDate,'dd-MM-yyyy') as PrasaberaDate,Format(ToDate,'dd-MM-yyyy') as ToDate From v_Leave_LeaveApplication where LACode=" + lnCode.Rows[0].ItemArray[0].ToString() + "", dtdate);
                     dtRunning.Columns.Remove("PregnantDate");
                     dtRunning.Columns.Remove("FromDate");
                     dtRunning.Columns.Remove("PrasaberaDate");
                     dtRunning.Columns.Remove("NumberofChild");
                     dtRunning.Columns.Remove("ToDate");
                     dtRunning.Columns.Add("NumberofChild", typeof(string));
                     dtRunning.Columns.Add("FromDate", typeof(string));
                     dtRunning.Columns.Add("PregnantDate", typeof(string));
                     dtRunning.Columns.Add("PrasaberaDate", typeof(string));
                     dtRunning.Columns.Add("ToDate", typeof(string));

                     dtRunning.Rows.Add(dt.Rows[0]["EmpId"].ToString(),dt.Rows[0]["EmpCardNo"].ToString(), dt.Rows[0]["EmpNameBn"].ToString(), dt.Rows[0]["DptNameBn"].ToString(), dt.Rows[0]["DsgNameBn"].ToString(), dt.Rows[0]["EmpJoiningDate"].ToString(), dt.Rows[0]["NumberofChild"].ToString(), dtdate.Rows[0]["FromDate"].ToString(), dtdate.Rows[0]["PregnantDate"].ToString(), dtdate.Rows[0]["PrasaberaDate"].ToString(), dtdate.Rows[0]["ToDate"].ToString());
                     //DataView view = new DataView(dtRunning);
                     //DataTable dtSpecificCols = view.ToTable(false, new string[] { "EmpCardNo", "EmpNameBn", "DptNameBn", "DsgNameBn", "EmpJoiningDate", "Format(ToDate,'dd-MM-yyyy') as ToDate", "Format(FromDate,'dd-MM-yyyy')", "Format(PrasaberaDate,'dd-MM-yyyy')" }); */
                sqlDB.fillDataTable("Select SUBSTRING(EmpCardNo,8,12) as EmpCardNo,EmpId,EmpNameBn,DptNameBn,DsgNameBn,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,FORMAT(FromDate,'dd-MM-yyyy') as FromDate,FORMAT(ToDate,'dd-MM-yyyy') as ToDate,Format(PregnantDate,'dd-MM-yyyy') as PregnantDate, FORMAT(PrasaberaDate,'dd-MM-yyyy') as PrasaberaDate,NumberofChild From v_Leave_LeaveApplication where EmpCardNo = '" + ddlCardNo.SelectedValue + "'", dtRunning);
                    if (dtRunning.Rows.Count > 0)
                    {
                        Session["__Grantedbythedoctor__"] = dtRunning;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=Grantedbythedoctor-"+dtRunning.Rows[0]["EmpId"].ToString()+"');", true);  //Open New Tab for Sever side code
                    }
                    else
                        lblMessage.InnerText = "warning->No Data Available";
                //}
                //else
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        dt.Columns.Add("FromDate", typeof(string));
                        
                //        Session["__Grantedbythedoctor__"] = dt;
                //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=Grantedbythedoctor-"+dt.Rows[0]["EmpId"].ToString()+"');", true);  //Open New Tab for Sever side code
                //    }
                //    else
                //        lblMessage.InnerText = "warning->No Data Available";
                //}
               
                    //Session["__Grantedbythedoctor__"] = dt;
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=Grantedbythedoctor');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

        protected void btnLetterofAuthority_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation() == false) return;
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select SUBSTRING(EmpCardNo,8,12) as EmpCardNo,EmpNameBn,DptNameBn,DsgNameBn,CompanyNameBangla From v_Leave_LeaveApplication where EmpCardNo = '" + ddlCardNo.SelectedValue + "'", dt);
                if (dt.Rows.Count > 0)
                {
                    Session["__LetterofAuthority__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LetterofAuthority');", true);  //Open New Tab for Sever side code
                }
                else
                    lblMessage.InnerText = "warning->No Data Available";
            }
            catch { }
        }
        private Boolean validation()
        {
            try
            {
                lblMessage.InnerText = "";
                if (ddlCardNo.SelectedItem.Text == "Card No")
                {
                    lblMessage.InnerText = "warning->Please Select Valid Card No";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }
    }
}