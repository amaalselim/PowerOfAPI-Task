using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Application.DTO_s
{
    public class ServiceDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ServiceType { get; set; }
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double Rating { get; set; }
        public IFormFile Img { get; set; }
    }
}
