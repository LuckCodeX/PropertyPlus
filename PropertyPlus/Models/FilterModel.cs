using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class FilterModel
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public string Search { get; set; }
        public FilterPriceModel FilterPrice { get; set; }
        public FilterAreaModel FilterArea { get; set; }
        public FilterRoomModel FilterRoom { get; set; }
        public FilterFacilityModel FilterFacility { get; set; }
    }

    public class FilterPriceModel
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }

    public class FilterAreaModel
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }

    public class FilterRoomModel
    {
        public int NoBedRoom { get; set; }
        public int NoBathRoom { get; set; }
    }

    public class FilterFacilityModel
    {
        public List<int> FacilityIds { get; set; }
    }
}