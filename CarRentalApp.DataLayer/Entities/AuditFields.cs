using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.DataLayer.Entities
{
    public class AuditFields
    {
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
