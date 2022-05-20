using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XProject.Contract.Repository.Models;
using XProject.Contract.Service.Interface;
using XProject.Core.Constants;
using XProject.WebApi.Extension;

namespace XProject.WebApi.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogService _logService;
        public LogController(IServiceProvider serviceProvider)
        {
            _logService = serviceProvider.GetRequiredService<ILogService>();
        }
        
        [HttpPost("upload")]
        public IActionResult Upload([FromBody]IFormFile file)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "/Upload", file.FileName);
                var stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);
                return Ok(new { length = file.Length, name = file.FileName });
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route(Endpoints.LogEndpoint.GetLog)]
        public List<Log> GetLog()
        {
            return _logService.Get().ToList();
        }

        [HttpPost]
        [Route(Endpoints.LogEndpoint.CreateLog)]
        //public void create(List<IFormFile> files, Log log)
        //{
        //    CreateLog(log);

        //    PostFiles(files, log);
        //}
        public void CreateLog(string jsonData , IFormFile file)
        {
            Log json = JsonConvert.DeserializeObject<Log>(jsonData);
            //_logService.Create(files);
            _ = UploadService.WriteFile(file);
            _logService.Create(json, file);
        }
        public void PostFiles( [FromHeader] List<IFormFile> files, Log log)
        {
            _logService.PostFiles(files,log);
        }
        [HttpPost]
        [Route(Endpoints.LogEndpoint.DeleteLog)]
        public void DeleteLog([FromBody] Log log, string id)
        {
            _logService.Delete(log, id);
        }
        [HttpPost]
        [Route(Endpoints.LogEndpoint.UpdateLog)]
        public void UpdateLog([FromBody] Log log, string id)
        {
            _logService.Update(log, id);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}