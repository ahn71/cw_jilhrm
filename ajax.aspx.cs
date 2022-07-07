using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using adviitRuntimeScripting;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;

namespace SigmaERP
{
    public partial class ajax : System.Web.UI.Page
    {
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sqlDB.connectionString = Glory.getConnectionString();
                sqlDB.connectDB();

                if (Request.QueryString["todo"].Equals("getPunismentAmount")) getPunismentAmountInfo(int.Parse(Request.QueryString["id"].ToString()));
                else if (Request.QueryString["todo"].Equals("getSeparationType")) getSeparationType(int.Parse(Request.QueryString["id"].ToString()));
                else if (Request.QueryString["todo"].Equals("getSalaryIncrementInfo")) getSalaryIncrementInfo(int.Parse(Request.QueryString["id"].ToString()));
                else if ((Request.QueryString["todo"]).Equals("getPromotion")) getPromotionInfo(int.Parse(Request.QueryString["esn"].ToString()));

                string dataId = Request.QueryString["id"];


                if (Request.QueryString["todo"] == "Line")
                {

                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("Select DptId,DptName From v_HRD_Department where DName='" + dataId + "'", dt);

                    for (int x = 0; x < dt.Rows.Count; x++)
                    {
                        var a = Request.QueryString["DptName"].ToString();
                        if (dt.Rows[x]["DptName"].ToString() == Request.QueryString["DptName"])
                            Response.Write("<option value=" + dt.Rows[x]["DptId"].ToString() + " selected='selected'>" + dt.Rows[x]["DptName"].ToString() + "</option>");
                        else Response.Write("<option value=" + dt.Rows[x]["DptId"].ToString() + ">" + dt.Rows[x]["DptName"].ToString() + "</option>");
                    }

                }
                else if (Request.QueryString["todo"] == "Religion")
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("Select RId,RName From HRD_Religion", dt);

                    for (int x = 0; x < dt.Rows.Count; x++)
                    {
                        Response.Write("<option value=" + dt.Rows[x]["RId"].ToString() + ">" + dt.Rows[x]["RName"].ToString() + "</option>");
                    }
                }
                else if (Request.QueryString["todo"] == "PersonalInfo")
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("Select FatherName, MotherName, FatherNameBn, MotherNameBN, MaritialStatus,convert(varchar(11),DateOfBirth,105) as DateOfBirth, PlaceOfBirth, Height, Weight, BloodGroup, Sex, Religion, LastEdQualification, NoOfExperience, Nationality, NationIDCardNo from Personnel_EmpPersonnal  where EmpId=" + dataId + "", dt);
                    if (dt.Rows.Count == 0)
                    {
                        Response.Write(0);
                        return;
                    }
                    string divInfo = "";

                    for (byte i = 0; i < dt.Columns.Count; i++)
                    {
                        divInfo += "" + dt.Columns[i].ColumnName + "_" + dt.Rows[0][i].ToString() + "#*";
                    }
                    Response.Write(divInfo);
                }
                else if (Request.QueryString["todo"] == "EmployeeAddress")
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("Select PresentAd, PresentZipCode, PhoneNo, PermanentAd, PermanentVillage, PermanentPostOffice, PermanentThana, PermanentZilla, PermanentDistrict, PermanentZipCode from Personnel_EmpAddress   where EmpId=" + dataId + "", dt);
                    if (dt.Rows.Count == 0)
                    {
                        Response.Write(0);
                        return;
                    }
                    string divInfo = "";

                    for (byte i = 0; i < dt.Columns.Count; i++)
                    {
                        divInfo += "" + dt.Columns[i].ColumnName + "_" + dt.Rows[0][i].ToString() + "#*";
                    }
                    Response.Write(divInfo);
                }
                else if (Request.QueryString["todo"] == "Personnel_EmergencyContact")
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("Select EmailAddress, ContactName, EmergencyAddress, EmpRelation, EmergencyPhoneNo from Personnel_EmergencyContact where EmpId=" + dataId + "", dt);
                    if (dt.Rows.Count == 0)
                    {
                        Response.Write(0);
                        return;
                    }
                    string divInfo = "";

                    for (byte i = 0; i < dt.Columns.Count; i++)
                    {
                        divInfo += "" + dt.Columns[i].ColumnName + "_" + dt.Rows[0][i].ToString() + "#*";
                    }
                    Response.Write(divInfo);
                }
                else if (Request.QueryString["todo"] == "DeleteEmployee") LoadAllEmployeeList(dataId);
                else if (Request.QueryString["todo"] == "gdept") GetDept(dataId, Request.QueryString["DptName"]);
            }
            catch { }
            
        }
        private void GetDept(string Did,string dptName)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select DptId,DptName From HRD_Department where DId='"+Did+"'", dt);

                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    if (dt.Rows[x]["DptName"].ToString() == dptName)
                    {
                        Response.Write("<option selected='selected'  value=" + dt.Rows[x]["DptId"].ToString() + " >" + dt.Rows[x]["DptName"].ToString() + "</option>");
                    }
                    else
                    {
                        Response.Write("<option value=" + dt.Rows[x]["DptId"].ToString() + ">" + dt.Rows[x]["DptName"].ToString() + "</option>");
                    }
                }
            }
            catch { }
        }

        private void getPunismentAmountInfo(int getId)
        {
            try
            {
                sqlDB.fillDataTable("select EmpPunismentAmount,EmpRemarks from Personnel_EmpPunismentInfo where EmpPunismentId ="+getId+"",dt=new DataTable ());
                Response.Write(dt.Rows[0].ItemArray[0].ToString() + "_" + dt.Rows[0].ItemArray[1].ToString());
            }
            catch { }
        }

        private void getSeparationType(int getId)
        {
            try
            {
                sqlDB.fillDataTable("select Remarks from Personnel_EmpSeparation where EmpSeparationId =" + getId + "", dt = new DataTable());
                Response.Write(dt.Rows[0].ItemArray[0].ToString());

            }
            catch { }
        }

        private void getSalaryIncrementInfo(int getId)
        {
            try
            {
                sqlDB.fillDataTable("select EmpId,EmpCardNo,EmpName,SalaryType,EmpPresentSalary,IncrementAmount,convert(varchar(11),DateofUpdate,105)as DateofUpdate,BasicSalary,MedicalAllownce,FoodAllownce,HouseRent,ConvenceAllownce,OrderRefNo,convert(varchar(11),OrderRefDate,105)as OrderRefDate,EffectiveMonth,Remarks,SN from v_Personnel_EmpCurrentStatus where SN="+getId+"", dt = new DataTable());
                Response.Write(dt.Rows[0]["EmpId"].ToString() + "_" + dt.Rows[0]["EmpCardNo"].ToString() + "_" + dt.Rows[0]["EmpName"].ToString() + "_" + dt.Rows[0]["SalaryType"].ToString() + "_" +
                               dt.Rows[0]["DateofUpdate"].ToString() +"_"+dt.Rows[0]["IncrementAmount"].ToString() + "_" +
                               dt.Rows[0]["BasicSalary"].ToString()+"_"+dt.Rows[0]["MedicalAllownce"].ToString()+"_"+dt.Rows[0]["HouseRent"].ToString()+"_"+
                               dt.Rows[0]["ConvenceAllownce"].ToString()+"_"+ dt.Rows[0]["EmpPresentSalary"].ToString() + "_" +"_"+
                               dt.Rows[0]["OrderRefNo"].ToString() + "_" + dt.Rows[0]["OrderRefDate"].ToString() + "_" + dt.Rows[0]["EffectiveMonth"].ToString() + "_" +
                               dt.Rows[0]["Remarks"].ToString() + "_" + dt.Rows[0]["SN"].ToString() + "_" + dt.Rows[0]["FoodAllownce"].ToString());
            }
            catch { }
        }

        private void getPromotionInfo(int getESN)
        {
            try
            {
                DataTable dtDName = new DataTable();
                DataTable dtEmpType = new DataTable();
                sqlDB.fillDataTable("select EmpId,EmpName,SalaryType,EmpPresentSalary,IncrementAmount,convert(varchar(11),DateofUpdate,105)as DateofUpdate,BasicSalary,MedicalAllownce,HouseRent,ConvenceAllownce,OrderRefNo,convert(varchar(11),OrderRefDate,105) as OrderRefDate,EffectiveMonth,DId,DptId,DptName,DsgId,DsgName,GrdName,TypeOfChange,LnId,FId,GrpId,remarks,EmpTypeId from v_Personnel_EmpCurrentStatus where SN=" + getESN + "", dt = new DataTable());
                sqlDB.fillDataTable("select DName From HRD_Division where DID=" +dt.Rows[0]["DID"].ToString()+"", dtDName);
                sqlDB.fillDataTable("select EmpType from HRD_EmployeeType where EmpTypeId =" + dt.Rows[0]["EmpTypeId"].ToString() + "",dtEmpType);
                string getLnName="0";
                string getFloorName="0";
                string getGrpoupName="0";
                DataTable dtLFG;  //LFG=Line Flor Group 
                if (!dt.Rows[0]["LnId"].ToString().Equals("0"))
                    {
                        sqlDB.fillDataTable("select LnId,LnCode from HRD_Line where LnId=" + dt.Rows[0]["LnId"].ToString() + "", dtLFG = new DataTable());
                        getLnName = dtLFG.Rows[0]["LnCode"].ToString();
                        
                    }
                    else if (!dt.Rows[0]["FId"].ToString().Equals("0"))
                    {
                        sqlDB.fillDataTable("select FId,FCode from HRD_Floor where FId=" + dt.Rows[0]["FId"].ToString() + "", dtLFG = new DataTable());
                        getFloorName = dtLFG.Rows[0]["FCode"].ToString();
                        
                       
                    }
                    else if (!dt.Rows[0]["GrpId"].ToString().Equals("0"))
                    {
                        sqlDB.fillDataTable("select GrpId,GrpName from HRD_GroupInfo where GrpId=" + dt.Rows[0]["GrpId"].ToString() + "", dtLFG = new DataTable());
                        getGrpoupName = dtLFG.Rows[0]["GrpName"].ToString();
                        
                    } 

                Response.Write(dt.Rows[0]["EmpId"].ToString() + "_" + dt.Rows[0]["EmpName"].ToString() + "_" + dt.Rows[0]["SalaryType"].ToString()+"_"+
                               dt.Rows[0]["EmpPresentSalary"].ToString() + "_" + dt.Rows[0]["IncrementAmount"].ToString() + "_" + dt.Rows[0]["DateofUpdate"].ToString()+"_"+
                               dt.Rows[0]["BasicSalary"].ToString() + "_" + dt.Rows[0]["MedicalAllownce"].ToString() + "_" + dt.Rows[0]["HouseRent"].ToString()+"_"+
                               dt.Rows[0]["ConvenceAllownce"].ToString() + "_" + dt.Rows[0]["OrderRefNo"].ToString()+"_"+dt.Rows[0]["OrderRefDate"].ToString()+"_"+
                               dt.Rows[0]["EffectiveMonth"].ToString() + "_" + dt.Rows[0]["DId"].ToString() + "_" + dt.Rows[0]["DptId"].ToString() + "_" + dt.Rows[0]["dptName"].ToString() + "_" +
                               dt.Rows[0]["DsgId"].ToString() + "_" + dt.Rows[0]["DsgName"].ToString() + "_" + dt.Rows[0]["GrdName"].ToString() + "_" + dt.Rows[0]["GrdName"].ToString() + "_" +
                               dt.Rows[0]["TypeOfChange"].ToString()+"_"+dtDName.Rows[0]["DName"].ToString()+"_"+dt.Rows[0]["LnId"].ToString()+"_"+getLnName+"_"+
                               dt.Rows[0]["FId"].ToString() + "_" + getFloorName + "_" + dt.Rows[0]["GrpId"].ToString() + "_" + getGrpoupName + "_" + dt.Rows[0]["remarks"].ToString()+"_"+
                               dtEmpType.Rows[0]["EmpType"].ToString() + "_" + dt.Rows[0]["EmpTypeId"].ToString());
            }
            catch { }
        }
        private void LoadAllEmployeeList(string EmpId)
        {
            try
            {
                SqlCommand deletecmd = new SqlCommand("Delete From Personnel_EmployeeInfo where EmpId="+EmpId+"", sqlDB.connection);
               int result=(int) deletecmd.ExecuteNonQuery();

               Response.Write(result);
            }
            catch { }
        }

    }
}
