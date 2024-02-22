using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.DataLayer.Entities
{
    public class Payment:AuditFields
    {
        public int PaymentId { get; set; }
        public DateTime TransactionDateTime { get; set; }

        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }

        //navigation properties

        public Reservation Reservation { get; set; }


    }
}
