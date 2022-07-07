using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRD.ModelEntities.Models.Settings
{
    public class BusinessTypeEntity:IDisposable
    {
        public int BId { get; set;}
        public string BTypeName { get; set; }
        public bool IsActive { get; set; }

        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            disposed = true;
        }
    }
}
