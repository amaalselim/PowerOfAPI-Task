using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }= DateTime.Now;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public List<OrderItem> Items { get; set; } = new List<OrderItem>(); 
        

    }
}
