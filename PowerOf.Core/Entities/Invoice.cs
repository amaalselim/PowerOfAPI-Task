using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Core.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }
    }
}
