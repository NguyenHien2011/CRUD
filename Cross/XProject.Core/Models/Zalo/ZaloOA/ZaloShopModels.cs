using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Core.Models.Zalo.ZaloOA
{
    public class ZaloCategory
    {
        public string id { get; set; }
        public string name { get; set; }
        public int sequence { get; set; }
        public List<string> product_ids { get; set; }
        public int product_quantity { get; set; }
        public long created_at { get; set; }
        public long updated_at { get; set; }
    }

    public class ZaloCategoryRequest
    {
        public string name { get; set; }
        public int sequence { get; set; }
        public List<string> product_ids { get; set; }
        public int status { get; set; }
    }
}
