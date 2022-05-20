using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Core.Models.Tracking
{
    public class TrackingRequest
    {
        public string brandid { get; set; }
        public string ext { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
    }

    public class TrackingDetailModel
    {
        public string BrandId { get; set; }
        public string Ext { get; set; }
        public int Duration { get; set; }
    }
}
