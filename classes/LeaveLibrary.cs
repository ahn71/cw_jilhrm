using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SigmaERP.classes
{
    public class LeaveLibrary
    {
        public static string GetLeaveFormSerialNo()
        {
            try
            {
                DataTable dt=new DataTable();
                sqlDB.fillDataTable("select ShortName from HRD_CompanyInfo",dt=new DataTable ());
                string setLFSL=dt.Rows[0]["ShortName"].ToString()+"-";
                dt = new DataTable();
                SQLOperation.selectBySetCommandInDatatable("select Max(convert(int,RIGHT(LeaveFormSLNo,4))) as LeaveFormSLNo from Leave_LeaveApplication "+
                    " where LeaveFormSLNo like '%"+DateTime.Now.Year+"%'",dt,sqlDB.connection);
                if (dt.Rows[0]["LeaveFormSLNo"].ToString().Trim().Length == 0) return setLFSL += DateTime.Now.Year + "-0001";

                int getLFSL = Convert.ToInt32(dt.Rows[0]["LeaveFormSLNo"].ToString()) + 1;
                if (getLFSL.ToString().Length == 1) return  setLFSL += DateTime.Now.Year + "-000"+getLFSL;
                else if (getLFSL.ToString().Length == 2) return  setLFSL += DateTime.Now.Year + "-00"+getLFSL;
                else if (getLFSL.ToString().Length == 3) return  setLFSL += DateTime.Now.Year + "-0" + getLFSL;
                else if (getLFSL.ToString().Length == 4) return setLFSL += DateTime.Now.Year + "-" + getLFSL;

                return null;
            }
            catch { return null; }
        }

        public static void LeaveCount(string AttDate,string LACode)
        {
            try
            {
                SqlCommand cmd; DataTable dt = new DataTable();
                // find Todate of this leave
                sqlDB.fillDataTable("select FORMAT(ToDate,'yyyy-MM-dd') as ToDate,LeaveId,LeaveName,LACode from v_Leave_LeaveApplication where LACode=" + LACode + "", dt);

                // if Todate is equal of current select days then below code is execute
                if (dt.Rows.Count>0)
                if (AttDate.Equals(dt.Rows[0]["ToDate"].ToString()))
                {
                    cmd = new System.Data.SqlClient.SqlCommand("Update Leave_LeaveApplication set IsProcessessed='0' where LACode= " + dt.Rows[0]["LACode"].ToString() + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();

                }

                // for changed used status for leave 
                cmd = new System.Data.SqlClient.SqlCommand("Update Leave_LeaveApplicationDetails set used='1' where LeaveDate='" + AttDate + "' AND LACode=" + dt.Rows[0]["LACode"].ToString() + "", sqlDB.connection);
                cmd.ExecuteNonQuery();               
            }
            catch { }

        }
    }
}