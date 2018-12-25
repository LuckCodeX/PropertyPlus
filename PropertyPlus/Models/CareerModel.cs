using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class CareerModel
    {
        public int Id { get; set; }
        public string CreatedString { get; set; }
        public int CategoryId { get; set; }
        public int Type { get; set; }
        public string City { get; set; }
        public decimal SalaryMin { get; set; }
        public string Location { get; set; }
        public string ExpiredDate { get; set; }
        public List<CareerContentModel> ContentList { get; set; }
        public CareerContentModel Content { get; set; }
    }

    public class CareerContentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Language { get; set; }
    }
}