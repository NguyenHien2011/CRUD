using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Core.Models.Zalo
{
    public class OAResponse<T>
    {
        public int error { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
