using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Models;

namespace XProject.Contract.Repository.Interface
{
    public interface ILogRepository : IRepository<Log>
    {
        //void Create(Log log);
    }
}
