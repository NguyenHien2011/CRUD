using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Models;
namespace XProject.Contract.Service.Interface
{
    public interface ILogService
    {
        void Create(Log log , IFormFile file);
        void Delete(Log log, string id);
        void Update(Log log, string id);
        List<Log> Get();
        //IAsyncResult UploadFile(IList<IFormFile> files, Log log);
        //void Apply(OpenApiOperation operation, OperationFilterContext context);
        void PostFiles(List<IFormFile> files, Log log);
    }
}
