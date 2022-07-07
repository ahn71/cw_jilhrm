using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace SigmaERP.attendance
{
    public partial class daily_attendance : System.Web.UI.Page
    {
      
        string strParam = string.Empty;
       
        
        DataTable DTLocal;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnaddall.Click += new System.EventHandler(this.btnaddall_Click);
            this.btnadditem.Click += new System.EventHandler(this.btnadditem_Click);
            this.btnremoveitem.Click += new System.EventHandler(this.btnremoveitem_Click);
            this.btnremoveall.Click += new System.EventHandler(this.btnremoveall_Click);
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);

            this.Load += new System.EventHandler(this.Page_Load);
            if (!IsPostBack)
            {
                
                //LoadDropDown();
            }
        }
       
        

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (rdoDept.SelectedValue == "0")
            {
                string strUrl = "ReportViewer.aspx?RepName=DailyAttendanceReport&ATTStatus=A&ATTDate='" + dptDate.Text.Trim() + "'&DpCode='0'&DP=" + rdoDept.SelectedItem.Value + "";
                Response.Redirect(strUrl);
            }
            else
            {
                string data1 = string.Empty;
                for (int i = 0; i < lstSelectedEmployees.Items.Count; i++)
                {
                    string data2 = lstSelectedEmployees.Items[i].Text + "','";
                    string aa = lstSelectedEmployees.SelectedItem.Value;// ddlDepartment.SelectedValue;
                    data1 += data2;
                }
                string departments = data1.Substring(0, data1.Count() - 3);
                string strUrl = "ReportViewer.aspx?RepName=DailyAttendanceReport&ATTStatus=A&ATTDate='" + dptDate.Text.Trim() + "'&DpCode='" + departments + "'&DP=" + rdoDept.SelectedItem.Value + "";
                Response.Redirect(strUrl);
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

        private void btnaddall_Click(object sender, System.EventArgs e)
        {
            AddRemoveAll(lstEmployees, lstSelectedEmployees);

        }

        private void btnadditem_Click(object sender, System.EventArgs e)
        {
            AddRemoveItem(lstEmployees, lstSelectedEmployees);
        }

        private void btnremoveitem_Click(object sender, System.EventArgs e)
        {
            AddRemoveItem(lstSelectedEmployees, lstEmployees);
        }

        private void btnremoveall_Click(object sender, System.EventArgs e)
        {
            AddRemoveAll(lstSelectedEmployees, lstEmployees);
        }

        

       

        

        

        
    }
}