using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SigmaERP.classes
{
    public class UserPrivilege
    {

        public static bool UserPrivilegeForUserType(string getEmpCardNo,string getCompanyId,string getUserId)
        {
            try
            {

                DataTable dt = new DataTable();


                sqlDB.fillDataTable("select userId from UserAccount where EmpId=(Select EmpId from v_Personnel_EmpCurrentStatus where EmpCarNo Like '%"+getEmpCardNo+"' AND CompanyId='"+getCompanyId+"') ",dt=new DataTable ());
                if (dt.Rows.Count > 0)
                {
                    if (!getUserId.Equals(dt.Rows[0]["UserId"].ToString())) return false;
                    else return true;
                }
                else return false;
            }
            catch { return false; }
        
        }
    }
}