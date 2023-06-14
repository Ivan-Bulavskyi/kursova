using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models
{
    public class OptimalData
    {
        public string nameElem { get; set; }
        public List<Range> badRanges { get; set; }
        public List<Range> averageRanges { get; set; }
        public Range optimalRange { get; set; }
    }

    public class Range
    {
        public double b { get; set; }
        public double e { get; set; }
    }
}