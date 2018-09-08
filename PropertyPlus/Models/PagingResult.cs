using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class PagingResult<T>
    {
        public int total { get; set; }
        public List<T> data { get; set; }
    }
}