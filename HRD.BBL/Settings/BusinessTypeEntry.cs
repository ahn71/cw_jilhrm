using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRD.ModelEntities.Models.Settings;
using System.Data.SqlClient;
using adviitRuntimeScripting;
using System.Data;

namespace HRD.BBL.Settings
{
    public class BusinessTypeEntry
    {
        BusinessTypeEntity businessTypeEntity;
        bool result;
        string sql;
        SqlCommand cmd;
        public BusinessTypeEntity SetEntities
        {
            set
            {
                businessTypeEntity = value;
            }
        
        }

        public bool Insert()
        {
            try
            {
                sql = "insert into HRD_BusinessType (BTypeName,IsActive) values ('"+businessTypeEntity.BTypeName+"','"+businessTypeEntity.IsActive+"')";
                cmd = new SqlCommand(sql,sqlDB.connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        
        }
        public bool Update(int BId) 
        {
            try {
                sql = "Update  HRD_BusinessType set BTypeName='" + businessTypeEntity.BTypeName + "', IsActive='" + businessTypeEntity.IsActive + "' where BId="+BId+"";
                cmd = new SqlCommand(sql, sqlDB.connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }
        public bool Delete(int BId) 
        {
            try
            {
                sql = "delete HRD_BusinessType  where BId=" + BId + "";
                cmd = new SqlCommand(sql, sqlDB.connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public List<BusinessTypeEntity> GetBusinessTyeList
        {
            get
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from HRD_BusinessType ",dt);

                List<BusinessTypeEntity> getList = new List<BusinessTypeEntity>();

                if (dt.Rows.Count > 0)
                {
                    getList = (from DataRow dr in dt.Rows
                               select new BusinessTypeEntity
                               {
                                   BId = int.Parse(dr["BId"].ToString()),
                                   BTypeName = dr["BTypeName"].ToString(),
                                   IsActive = bool.Parse(dr["IsActive"].ToString())
                               }).ToList();

                    return getList;
                }
                return null;
            }

            
        }


    }
}
