using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Contract.Repository.Models
{
    public class Log : Entity
    {
        public string URL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserAccount { get; set; }
        public string IP { get; set; }
        public string MessageError {get;set;}
        public int Status { get; set; }
        public string Type { get; set; }
        public string NameFile { get; set; }
    }
}
