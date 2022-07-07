
using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ComplexScriptingSystem;
using System.Drawing;
using SigmaERP.classes;


namespace SigmaERP.attendance
{
    public partial class attendance_summary : System.Web.UI.Page
    {
        DataTable dtSetPrivilege;
        string CompanyId = "";
        GridView gvattsummary;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sqlDB.connectionString = Glory.getConnectionString();
                sqlDB.connectDB();
                lblMessage.InnerText = "";
                if (!IsPostBack)
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    setPrivilege();
                    //if (!classes.ServerTimeZone.IsBDTZone()) txtFromDate.Text = dptDate.Text = classes.ServerTimeZone.GetBangladeshNowDate();
                    //else txtFromDate.Text = dptDate.Text= DateTime.Now.ToString("dd-MM-yyyy");
                    if (!classes.commonTask.HasBranch())
                        ddlCompanyName.Enabled = false;
                   
                    

                   
                }
            }
            catch { }
        }
        private void setPrivilege()
        {
            try
            {
                ViewState["__WriteAction__"] = "1";
                ViewState["__DeletAction__"] = "1";
                ViewState["__ReadAction__"] = "1";
                ViewState["__UpdateAction__"] = "1";


                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserId__"] = getCookies["__getUserId__"].ToString();

                string[] AccessPermission = new string[0];              
                 AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "attendance_summary.aspx", ddlCompanyName, WarningMessage, btnPrintPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                if (ViewState["__ReadAction__"].ToString().Equals("0")) 
                {
                    btnSearch.Enabled = false;
                    btnSearch.CssClass = "";
                    tblGenerateType.Visible = false;
                }
                classes.commonTask.LoadShiftNameByCompany(ViewState["__CompanyId__"].ToString(), ddlShiftName);
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();

                

            }
            catch { }

        }
        protected void rblShiftNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            CompanyId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue;
            
           
        }      
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchSummaries();
        }

        private void searchSummaries()
        {
            //string[] Tdate = dptDate.Text.Split('-');
            string[] Fdate = txtFromDate.Text.Split('-');           
            // ViewState["__ShiftID__"] = dtselect.Rows[0]["SftId"].ToString();

           

            string DepartmentList = "";



            //if (rblDptType.SelectedValue == "1") DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
            Session["__SelectedDepartmentList__"] = DepartmentList;

            if (ddlCompanyName.SelectedValue != "00")
            {
                // classes.DailyAttendanceProcessing DAP = new classes.DailyAttendanceProcessing();
                //    DAP.GetRequiredInfo(ViewState["__CompanyId__"].ToString(),ddlShiftName.SelectedValue,classes.ServerTimeZone.GetBangladeshNowDate(), ViewState["__UserId__"].ToString());
                LoadAttenSummary();
            }
            else
            {
                //   classes.DailyAttendanceProcessing DAP = new classes.DailyAttendanceProcessing();
                //   DAP.GetRequiredInfo(ViewState["__CompanyId__"].ToString(), ddlShiftName.SelectedValue, classes.ServerTimeZone.GetBangladeshNowDate(), ViewState["__UserId__"].ToString());
                LoadAllCompanyAtt();
            }
        }

        private void LoadAttenSummary()
        {
            try
            {

                string[] Tdate = txtFromDate.Text.Split('-');// dptDate.Text.Split('-');
                string[] Fdate = txtFromDate.Text.Split('-');
                string ToDate, FromDate;
                ToDate = Tdate[2] + "-" + Tdate[1] + "-" + Tdate[0];
                FromDate = Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0];
              
                string ShiftName = (ddlShiftName.SelectedValue == "0") ? "" : " and SftName='" + ddlShiftName.SelectedValue + "' ";
                gvattsummary = new GridView();
                gvattsummary.AutoGenerateColumns = false;
                DataTable dt = new DataTable();
                if (rblReportType.SelectedValue == "0") 
                {
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                    {
                        sqlDB.fillDataTable("Select DptId,DptName as Department,sum(A) as Absent,sum(P) as Present,sum(Lv) as Leave,sum(L) as Late,sum(A)+sum(P)+sum(Lv)+sum(L) as Total From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate='" + FromDate + "' " + ShiftName + "  group by DptId,DptName", dt);
                        //else sqlDB.fillDataTable("Select DptId,DptName as Department,sum(A) as Absent,sum(P) as Present,sum(Lv) as Leave,sum(L) as Late,sum(A)+sum(P)+sum(Lv)+sum(L) as Total From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and SftId=" + ddlShiftName.SelectedValue + " and EmpStatus in (1,8) and ATTDate='" + FromDate + "'  and DptId='" + ViewState["__DptId__"].ToString() + "' group by DptId,DptName ", dt);

                    }
                    else
                    {


                        sqlDB.fillDataTable("Select DptId,DptName as Department,sum(A) as Absent,sum(P) as Present,sum(Lv) as Leave,sum(L) as Late,sum(A)+sum(P)+sum(Lv)+sum(L) as Total From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate='" + FromDate + "' " + ShiftName + " group by DptId,CONVERT(int,DptCode),DptName order by CONVERT(int,DptCode)", dt);
                         
                    }
                }
                else
                {
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                    {
                        sqlDB.fillDataTable("Select DptId,DptName as Department,sum(A) as Absent,sum(P) as Present,sum(Lv) as Leave,sum(L) as Late,sum(A)+sum(P)+sum(Lv)+sum(L) as Total From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate>='" + FromDate + "' and ATTDate<='" + ToDate + "' and DptId='" + ViewState["__DptId__"].ToString() + "' " + ShiftName + " group by DptId,DptName", dt);
                        //else sqlDB.fillDataTable("Select DptId,DptName as Department,sum(A) as Absent,sum(P) as Present,sum(Lv) as Leave,sum(L) as Late,sum(A)+sum(P)+sum(Lv)+sum(L) as Total From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and SftId=" + ddlShiftName.SelectedValue + " and EmpStatus in (1,8) and ATTDate>='" + FromDate + "' and ATTDate<='" + ToDate + "' and DptId='" + ViewState["__DptId__"].ToString() + "' group by DptId,DptName ", dt);

                    }
                    else
                    {
                        sqlDB.fillDataTable("Select DptId,DptName as Department,sum(A) as Absent,sum(P) as Present,sum(Lv) as Leave,sum(L) as Late,sum(A)+sum(P)+sum(Lv)+sum(L) as Total From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate>='" + FromDate + "' and ATTDate<='" + ToDate + "' " + ShiftName + " group by DptId,CONVERT(int,DptCode),DptName order by CONVERT(int,DptCode)", dt);
                       
                    }
                }
                if(dt.Rows.Count<1)
                {
                    lblMessage.InnerText = "warning->Any Record Are Not Founded.";
                    return;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    TemplateField tf = new TemplateField();
                    tf.HeaderTemplate = new DynamicGridViewURLTemplate(dt.Columns[i].ColumnName.ToString(), dt.Columns[i].ColumnName.ToString(), DataControlRowType.Header, "0", ddlCompanyName.SelectedValue, txtFromDate.Text, txtFromDate.Text, rblPrintType.SelectedValue);
                    tf.ItemTemplate = new DynamicGridViewURLTemplate(dt.Columns[i].ColumnName.ToString(), dt.Columns[i].ColumnName.ToString(), DataControlRowType.DataRow,"0", ddlCompanyName.SelectedValue, txtFromDate.Text, txtFromDate.Text, rblPrintType.SelectedValue);
                    gvattsummary.Columns.Add(tf);
                }
                object absent = dt.Compute("sum(Absent)", "");
                object present = dt.Compute("sum(Present)", "");
                object Leave = dt.Compute("sum(Leave)", "");
                object Late = dt.Compute("sum(Late)", "");
                object Total = dt.Compute("sum(Total)", "");
                dt.Rows.Add("0", "Total", absent.ToString() , present.ToString() , Leave.ToString()  , Late.ToString() , Total.ToString() );
                
                gvattsummary.DataSource = dt;
                gvattsummary.DataBind();              
                gvattsummary.Width = 755;
                gvattsummary.CssClass = "gvdisplay";
                gvattsummary.HeaderStyle.CssClass = "header";
                
                //gvattsummary.HeaderRow.CssClass = "";
                gvattsummary.Columns[0].Visible = false;
                //gvattsummary.Columns[2].Visible = false;


                divsummary.Controls.Add(gvattsummary);
                
                gvattsummary.Rows[dt.Rows.Count - 1].Enabled = false;
                gvattsummary.Rows[dt.Rows.Count - 1].ForeColor = Color.Black;

            }
            catch { }
        }
        public class DynamicGridViewURLTemplate : ITemplate
        {
            string _ColNameText;
            string _ColNameURL;
            DataControlRowType _rowType;
            string sftid;
            string cid;
            string date;
            string fdate;
            string PrintType;

            public DynamicGridViewURLTemplate(string ColNameText, string ColNameURL, DataControlRowType RowType,string sid,string comid,string adate,string fromdate,string PType)
            {
                 
                _ColNameText = ColNameText;
                _rowType = RowType;
                _ColNameURL = ColNameURL;
                sftid = sid;
                cid = comid;
                date = adate;
                fdate = fromdate;
                PrintType = PType;
            }
            public void InstantiateIn(System.Web.UI.Control container)
            {
                switch (_rowType)
                {
                    case DataControlRowType.Header:
                        Literal lc = new Literal();
                        lc.Text = "<b>" + _ColNameURL + "</b>";
                        container.Controls.Add(lc);
                        break;
                    case DataControlRowType.DataRow:
                        HyperLink hpl = new HyperLink();
                        hpl.Target = "_blank";
                        hpl.DataBinding += new EventHandler(this.hpl_DataBind);
                        container.Controls.Add(hpl);
                        break;
                    default:
                        break;
                }
            }

            private void hpl_DataBind(Object sender, EventArgs e)
            {
                HyperLink hpl = (HyperLink)sender;
                GridViewRow row = (GridViewRow)hpl.NamingContainer;
                string rowItemText = DataBinder.Eval(row.DataItem, _ColNameText).ToString();
                string rIndex = row.RowIndex.ToString();
              //  string rCount = ViewState["TotalRow"].ToString();
                
                //hpl.NavigateUrl = DataBinder.Eval(row.DataItem, _ColNameURL).ToString();
                hpl.NavigateUrl = "";
                if (rowItemText != "0")
                {



                    hpl.NavigateUrl = "~/All Report/Report.aspx?for=AttSummary-" + DataBinder.Eval(row.DataItem, "DptId").ToString() + "-" + cid + "-" + DataBinder.Eval(row.DataItem, "DptId").ToString() + "-" + _ColNameText + "-" + fdate + "-" + fdate + "-" + PrintType + "";
                    hpl.Text = "<div class=\"Post\"><div class=\"PostTitle\">" + rowItemText + "</div></div>";
                }
                else hpl.Text = rowItemText;
            }
        }

        protected void gvSeparationList_RowCommand(object sender, GridViewCommandEventArgs e)
        { 
        
        }

        private void LoadAllCompanyAtt()
        {
            try
            {
                string[] Fdate = txtFromDate.Text.Split('-');
                string[] Tdate = txtFromDate.Text.Split('-');
                DataTable dtselect = new DataTable();
                sqlDB.fillDataTable("Select SftId,SftName,CompanyId,CompanyName From v_v_DailyAttendanceSummary where ATTDate >='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "'and ATTDate <='" + Tdate[2] + "-" + Tdate[1] + "-" + Tdate[0] + "' group by SftId,CompanyId,SftName,CompanyName", dtselect);
                for (int m = 0; m < dtselect.Rows.Count; m++)
                {
                    GridView gvattsummary = new GridView();
                    gvattsummary.AutoGenerateColumns = false;
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("Select DptId,DptName as Department,sum(A) as Absent,sum(P) as Present,sum(Lv) as Leave,sum(L) as Late,sum(A)+sum(P)+sum(Lv)+sum(L) as Total From v_v_DailyAttendanceSummary where CompanyId='" + dtselect.Rows[m]["CompanyId"].ToString() + "' and SftId=" + dtselect.Rows[m]["SftId"].ToString() + " and ATTDate >='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "'and ATTDate <='" + Tdate[2] + "-" + Tdate[1] + "-" + Tdate[0] + "' group by DptId,DptName ", dt);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        TemplateField tf = new TemplateField();
                        tf.HeaderTemplate = new DynamicGridViewURLTemplate1(dt.Columns[i].ColumnName.ToString(), dt.Columns[i].ColumnName.ToString(), DataControlRowType.Header, dtselect.Rows[m]["SftId"].ToString(), dtselect.Rows[m]["CompanyId"].ToString(),txtFromDate.Text,rblPrintType.SelectedValue);
                        tf.ItemTemplate = new DynamicGridViewURLTemplate1(dt.Columns[i].ColumnName.ToString(), dt.Columns[i].ColumnName.ToString(), DataControlRowType.DataRow, dtselect.Rows[m]["SftId"].ToString(), dtselect.Rows[m]["CompanyId"].ToString(), txtFromDate.Text, rblPrintType.SelectedValue);
                        gvattsummary.Columns.Add(tf);
                    }
                    gvattsummary.DataSource = dt;
                    gvattsummary.DataBind();
                    gvattsummary.Width = 572;
                    gvattsummary.CssClass = "display";
                    gvattsummary.HeaderStyle.CssClass = "header";
                    gvattsummary.Columns[0].Visible = false;
                    gvattsummary.Columns[2].Visible = false;
                    string divInfo = "";
                    divInfo = " <table > ";
                    divInfo += "<thead>";
                    divInfo += "<tr>";
                    divInfo += "<td style='font:bold'>Company Name:</td>";
                    divInfo += "<td style='width:150px'>" + dtselect.Rows[m]["CompanyName"].ToString() + "</td>";
                    divInfo += "<td style='font:bold'>Shift:</td>";
                    divInfo += "<td style='width:70px'>" + dtselect.Rows[m]["SftName"].ToString() + "</td>";
                    divInfo += "</tr>";

                    divInfo += "</thead>";
                    divInfo += "<tfoot>";

                    divInfo += "</table>";
                    divsummary.Controls.Add(new LiteralControl(divInfo));
                    divsummary.Controls.Add(gvattsummary);
                }

            }
            catch { }
        }

        public class DynamicGridViewURLTemplate1 : ITemplate
        {
            string _ColNameText;
            string _ColNameURL;
            DataControlRowType _rowType;
            string sid;
            string cid;
            string date;
            string PrintType;

            public DynamicGridViewURLTemplate1(string ColNameText, string ColNameURL, DataControlRowType RowType,string sftid,string comid,string cdate,string ptrintType)
            {
                _ColNameText = ColNameText;
                _rowType = RowType;
                _ColNameURL = ColNameURL;
                sid = sftid;
                cid = comid;
                date = cdate;
                PrintType = ptrintType;
            }
            public void InstantiateIn(System.Web.UI.Control container)
            {
                switch (_rowType)
                {
                    case DataControlRowType.Header:
                        Literal lc = new Literal();
                        lc.Text = "<b>" + _ColNameURL + "</b>";
                        container.Controls.Add(lc);
                        break;
                    case DataControlRowType.DataRow:
                        HyperLink hpl = new HyperLink();
                        hpl.Target = "_blank";
                        hpl.DataBinding += new EventHandler(this.hpl_DataBind);
                        container.Controls.Add(hpl);
                        break;
                    default:
                        break;
                }
            }

            private void hpl_DataBind(Object sender, EventArgs e)
            {
                HyperLink hpl = (HyperLink)sender;
                GridViewRow row = (GridViewRow)hpl.NamingContainer;
                string a = row.DataItem.ToString();
                //hpl.NavigateUrl = DataBinder.Eval(row.DataItem, _ColNameURL).ToString();
                hpl.NavigateUrl = "";
                hpl.NavigateUrl = "~/All Report/Report.aspx?for=AttSummary-" + sid + "-" + cid + "-" + DataBinder.Eval(row.DataItem, "DptId").ToString() + "-" + _ColNameText + "-" + date + "";
                hpl.Text = "<div class=\"Post\"><div class=\"PostTitle\">" + DataBinder.Eval(row.DataItem, _ColNameText).ToString() + "</div></div>";
            }
        }
        //protected override void Render(HtmlTextWriter writer) 
        //{
        //    try
        //    {
        //        string lastSubCategory = String.Empty;
        //        Table gridTable = (Table)gvattsummary.Controls[0];
        //        foreach (GridViewRow gvr in gvattsummary.Rows)
        //        {
        //            HiddenField hfSubCategory = gvr.FindControl("hfSubCategory") as
        //                                        HiddenField;
        //            string currSubCategory = hfSubCategory.Value;
        //            if (lastSubCategory.CompareTo(currSubCategory) != 0)
        //            {
        //                int rowIndex = gridTable.Rows.GetRowIndex(gvr);
        //                // Add new group header row
        //                GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex,
        //                    DataControlRowType.DataRow, DataControlRowState.Normal);
        //                TableCell headerCell = new TableCell();
        //                headerCell.ColumnSpan = gvattsummary.Columns.Count;
        //                headerCell.Text = string.Format("{0}:{1}", "SubCategory",
        //                                                currSubCategory);
        //                headerCell.CssClass = "GroupHeaderRowStyle";
        //                // Add header Cell to header Row, and header Row to gridTable
        //                headerRow.Cells.Add(headerCell);
        //                gridTable.Controls.AddAt(rowIndex, headerRow);
        //                // Update lastValue
        //                lastSubCategory = currSubCategory;
        //            }
        //        }
        //        base.Render(writer);
        //    }
        //    catch { }
        //}


        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCompanyName.SelectedValue == "00")
            {
                //ddlShiftName.Enabled = false;
                divDepartment.Visible = false;
                //ddlShiftName.Items.Clear();
            }
            else
            {
                //ddlShiftName.Enabled = true;
                divDepartment.Visible = false;
                //classes.commonTask.LoadShift(ddlShiftName, ddlCompanyName.SelectedValue, 0);
            }
            classes.commonTask.LoadShiftNameByCompany(ddlCompanyName.SelectedValue, ddlShiftName);
        }

        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            if (txtCardNo.Text.Length > 0 && ddlCompanyName.SelectedValue != "00") //Individual Employee.
            {
                if (txtCardNo.Text.Length < 4) // validation For Individual Employee
                {
                    lblMessage.InnerText = "warning->Please Type Minimum 6 Character of CardNo !";
                    txtCardNo.Focus();
                    return;
                }
                //-------------------- Data processing-----------------------------
              //  classes.DailyAttendanceProcessing DAP = new classes.DailyAttendanceProcessing();
               // DAP.GetRequiredInfo(ViewState["__CompanyId__"].ToString(), ddlShiftName.SelectedValue, DateTime.Now.ToString("dd-MM-yyyy"), ViewState["__UserId__"].ToString());
                //-------------------------------------------------
                SpeacificEmpDailyAttRpt();
                return;
            }    
           
            if (ddlCompanyName.SelectedValue != "00") // Individual Company.
            {
                //-------------------- Data processing-----------------------------
               // classes.DailyAttendanceProcessing DAP = new classes.DailyAttendanceProcessing();
               // DAP.GetRequiredInfo(ViewState["__CompanyId__"].ToString(), ddlShiftName.SelectedValue, DateTime.Now.ToString("dd-MM-yyyy"), ViewState["__UserId__"].ToString());
                //-------------------------------------------------
                PrintIndivisualComSummary();
            }
            else  // All Comapany.
            {
                //-------------------- Data processing-----------------------------
               // classes.DailyAttendanceProcessing DAP = new classes.DailyAttendanceProcessing();
              //  DAP.GetRequiredInfo(ViewState["__CompanyId__"].ToString(), ddlShiftName.SelectedValue, DateTime.Now.ToString("dd-MM-yyyy"), ViewState["__UserId__"].ToString());
                //-------------------------------------------------
                PrintAllComSummary();
            }
            
        }
        //..........................Start..............................................
        /*Md.Abid Hasan (Nayem)
         Email  :nayem.optimal@gmail.com
         Purpose: For Speacific Epmloyee's Attendance Report Preview
         */
        private void SpeacificEmpDailyAttRpt() 
        {
            string SqlCmd = "";
            string[] Fdate = txtFromDate.Text.Split('-');
            string FromDate = Fdate[2] + " - " + Fdate[1] + " - " + Fdate[0];
            string[] Tdate = txtFromDate.Text.Split('-');
            string ToDate = Tdate[2] + " - " + Tdate[1] + " - " + Tdate[0];
            DataTable dt = new DataTable();
            if (txtFromDate.Text == txtFromDate.Text) // For Single Date
            {
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                 SqlCmd = " SELECT Format(v_tblAttendanceRecord.ATTDate,'dd-MM-yyyy') as AttDate, v_tblAttendanceRecord.ATTStatus, v_tblAttendanceRecord.StateStatus, v_tblAttendanceRecord.EmpName,v_tblAttendanceRecord.DptName, "
                    + "v_tblAttendanceRecord.DsgName, v_tblAttendanceRecord.SftName,format(v_tblAttendanceRecord.EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate, v_tblAttendanceRecord.EmpCardNo,"
                    + "v_tblAttendanceRecord.CompanyName, v_tblAttendanceRecord.Address, v_tblAttendanceRecord.Sex, v_tblAttendanceRecord.RName,v_tblAttendanceRecord.MobileNo"
                    + " FROM v_tblAttendanceRecord"
                    + " where EmpCardNo like '%" + txtCardNo.Text + "' and CompanyId='" + ddlCompanyName.SelectedValue + "' and DptId='" + ViewState["__DptId__"].ToString() + "' and EmpStatus in (1,8) and ATTDate='" + ToDate + "'";
                else
                    SqlCmd = " SELECT Format(v_tblAttendanceRecord.ATTDate,'dd-MM-yyyy') as AttDate, v_tblAttendanceRecord.ATTStatus, v_tblAttendanceRecord.StateStatus, v_tblAttendanceRecord.EmpName,v_tblAttendanceRecord.DptName, "
                   + "v_tblAttendanceRecord.DsgName, v_tblAttendanceRecord.SftName,format(v_tblAttendanceRecord.EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate, v_tblAttendanceRecord.EmpCardNo,"
                   + "v_tblAttendanceRecord.CompanyName, v_tblAttendanceRecord.Address, v_tblAttendanceRecord.Sex, v_tblAttendanceRecord.RName,v_tblAttendanceRecord.MobileNo"
                   + " FROM v_tblAttendanceRecord"
                   + " where EmpCardNo like '%" + txtCardNo.Text + "' and CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate='" + ToDate + "'";
                sqlDB.fillDataTable(SqlCmd, dt);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning-> Any Record Are Not Founded!";
                    return;
                }
                Session["__SpeacificEmpAttSummary__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=SpeacificEmpAttSummary');", true);  //Open New Tab for Sever side code
            }
            else // For Muliple Date
            {
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                SqlCmd = " SELECT Format(v_tblAttendanceRecord.ATTDate,'dd-MM-yyyy') as AttDate, v_tblAttendanceRecord.ATTStatus, v_tblAttendanceRecord.StateStatus, v_tblAttendanceRecord.EmpName,v_tblAttendanceRecord.DptName, "
                    + "v_tblAttendanceRecord.DsgName, v_tblAttendanceRecord.SftName, format(v_tblAttendanceRecord.EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate, v_tblAttendanceRecord.EmpCardNo,"
                    + "v_tblAttendanceRecord.CompanyName, v_tblAttendanceRecord.Address, v_tblAttendanceRecord.Sex, v_tblAttendanceRecord.RName,v_tblAttendanceRecord.MobileNo"
                    + " FROM v_tblAttendanceRecord"
                    + " where EmpCardNo like '%" + txtCardNo.Text + "' and CompanyId='" + ddlCompanyName.SelectedValue + "' and DptId='" + ViewState["__DptId__"].ToString() + "' and EmpStatus in (1,8) and ATTDate>='" + FromDate + "' and ATTDate<='" + ToDate + "'";
                else
                    SqlCmd = " SELECT Format(v_tblAttendanceRecord.ATTDate,'dd-MM-yyyy') as AttDate, v_tblAttendanceRecord.ATTStatus, v_tblAttendanceRecord.StateStatus, v_tblAttendanceRecord.EmpName,v_tblAttendanceRecord.DptName, "
                    + "v_tblAttendanceRecord.DsgName, v_tblAttendanceRecord.SftName, format(v_tblAttendanceRecord.EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate, v_tblAttendanceRecord.EmpCardNo,"
                    + "v_tblAttendanceRecord.CompanyName, v_tblAttendanceRecord.Address, v_tblAttendanceRecord.Sex, v_tblAttendanceRecord.RName,v_tblAttendanceRecord.MobileNo"
                    + " FROM v_tblAttendanceRecord"
                    + " where EmpCardNo like '%" + txtCardNo.Text + "' and CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate>='" + FromDate + "' and ATTDate<='" + ToDate + "'";               
                sqlDB.fillDataTable(SqlCmd, dt);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning-> Any record are not founded.";
                    return;
                }
                Session["__SpeacificAttSummaryMultiDate__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=SpeacificAttSummaryMultiDate');", true);  //Open New Tab for Sever side code
           
            }



        }       

        //...............................End.............................................
        private void PrintIndivisualComSummary()// For Individsual company
        {
            try
            {

                string[] Fdate = txtFromDate.Text.Split('-');
                string[] Tdate = txtFromDate.Text.Split('-');
                string ShiftName = (ddlShiftName.SelectedValue == "0") ? "" : " and SftName='" + ddlShiftName.SelectedValue + "' ";
                DataTable dt = new DataTable();
                if (rblReportType.SelectedValue == "0")
                {
                   
                        if(rblDptORGrp.SelectedValue=="Dpt")
                        sqlDB.fillDataTable("Select CompanyName,Address,DptName,sum(A) as A,sum(P) as P,sum(Lv) as Lv,sum(L) as L From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate ='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "' "+ShiftName+"  group by DptId,CONVERT(int,DptCode),DptName,CompanyName,Address order by CONVERT(int,DptCode)", dt);
                        else
                            sqlDB.fillDataTable("Select CompanyName,Address,GName as DptName,sum(A) as A,sum(P) as P,sum(Lv) as Lv,sum(L) as L From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate ='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "' " + ShiftName + " group by GId,GName,DptId,CompanyName,Address order by Convert(int,DptId),CONVERT(int,GId)", dt);
                        //else sqlDB.fillDataTable("Select DptName,CompanyName,Address,SftId, SftName,sum(A) as A,sum(P) as P,sum(Lv) as Lv,sum(L) as L From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and SftId=" + ddlShiftName.SelectedValue + " and ATTDate ='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "' and DptId='" + ViewState["__DptId__"].ToString() + "' group by DptId,CONVERT(int,DptCode),DptName,CompanyName,Address,SftId,SftName order by CONVERT(int,DptCode) ", dt);

                 
                }
                else 
                {
                    if (rblDptORGrp.SelectedValue == "Dpt")
                        sqlDB.fillDataTable("Select CompanyName,Address,DptName,sum(A) as A,sum(P) as P,sum(Lv) as Lv,sum(L) as L From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate ='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "' " + ShiftName + "  group by DptId,CONVERT(int,DptCode),DptName,CompanyName,Address order by CONVERT(int,DptCode)", dt);
                    else
                        sqlDB.fillDataTable("Select CompanyName,Address,GName as DptName,sum(A) as A,sum(P) as P,sum(Lv) as Lv,sum(L) as L From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate ='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "' " + ShiftName + "  group by GId,GName,DptId,CompanyName,Address order by Convert(int,DptId),CONVERT(int,GId)", dt);
                    //if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                    //{
                    //   sqlDB.fillDataTable("Select CompanyName,Address,DptName,SftId,SftName,sum(A) as A,sum(P) as P,sum(Lv) as Lv,sum(L) as L From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and ATTDate ='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "'  group by DptId,CONVERT(int,DptCode),DptName,CompanyName,Address,SftId,SftName order by CONVERT(int,DptCode)", dt);
                    //    //else sqlDB.fillDataTable("Select DptName,CompanyName,Address,SftId, SftName,sum(A) as A,sum(P) as P,sum(Lv) as Lv,sum(L) as L From v_v_DailyAttendanceSummary where CompanyId='" + ddlCompanyName.SelectedValue + "' and EmpStatus in (1,8) and SftId=" + ddlShiftName.SelectedValue + " and ATTDate ='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "' and DptId='" + ViewState["__DptId__"].ToString() + "' group by DptId,CONVERT(int,DptCode),DptName,CompanyName,Address,SftId,SftName order by CONVERT(int,DptCode) ", dt);

                    //}
                  
                }
                if (dt.Rows.Count > 0)
                {
                    Session["__AllAttSummary__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=AllAttSummary-" + txtFromDate.Text.Trim() +"-"+rblDptORGrp.SelectedValue+"');", true);  //Open New Tab for Sever side code
                }
                else lblMessage.InnerText = "warning-> Any Record Are Not Founded!";
            }
            catch { }
        }
        private void PrintAllComSummary() // For All company
        {
            try
            {
                string[] Fdate = txtFromDate.Text.Split('-');
                string[] Tdate = txtFromDate.Text.Split('-');
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select DptName,CompanyName,Address,SftName,sum(A) as A,sum(P) as P,sum(Lv) as Lv,sum(L) as L From v_v_DailyAttendanceSummary where   ATTDate >='" + Fdate[2] + "-" + Fdate[1] + "-" + Fdate[0] + "'and ATTDate <='" + Tdate[2] + "-" + Tdate[1] + "-" + Tdate[0] + "' group by DptName,CompanyName,Address,SftName ", dt);
                if (dt.Rows.Count > 0)
                {
                    Session["__AllAttSummary__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=AllAttSummary-" + txtFromDate.Text +"');", true);  //Open New Tab for Sever side code
                }
            }
            catch { }
        }
     
        //.........................................................
        /*Md.Abid Hasan (Nayem)
         Email: nayem.optimal@gmail.com
         */
      
        //............................End........................................................
        protected void Button1_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }

        protected void rblPrintType_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchSummaries();
        }

        }

     
    }
