using Invedia.DI.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Interface;
using XProject.Contract.Repository.Models;
using XProject.Contract.Service.Interface;
using XProject.Core.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace XProject.Service
{
    [ScopedDependency(ServiceType = typeof(ILogService))]
    public class LogService : Base.Service, ILogService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogRepository _logRepo;
        private readonly IUnitOfWork _uof;
        public LogService(IServiceProvider serviceProvider, IWebHostEnvironment env) : base(serviceProvider)
        {
            _logRepo = serviceProvider.GetRequiredService<ILogRepository>();
            _uof = serviceProvider.GetRequiredService<IUnitOfWork>();
            _env = env;
        }
        public List<Log> Get()
        {
            _logRepo.Get();
            _uof.SaveChanges();
            return _logRepo.Get().ToList();
        }
        private async Task<bool> WriteFile(IFormFile file)
        {
            bool isSaveSuccess = false;
            string fileName;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
                   fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                isSaveSuccess = true;
            }
            catch (Exception e)
            {
                //log error
            }

            return isSaveSuccess;
        }
        public void Create([FromForm]List<IFormFile> files, Log log)
        {


            //string contentRootPath = _env.ContentRootPath;

            //List<Contract.Repository.Models.Log> f = new List<Contract.Repository.Models.Log>();
            //string path1 = _env.WebRootPath + "Files"; ;
            //string path2 = Path.Combine(path1, log.Id.ToString());
            //Directory.CreateDirectory(path2);


            //if (files.Count > 0)
            //{
            //    foreach (var file in files)
            //    {
            //        string s = file.FileName;
            //        var FilePath = path2 + "\\" + s;
            //        //var FilePath = file.FileName;
            //        //guardo en la carpeta
            //        using (var stream = System.IO.File.Create(FilePath))
            //        {
            //            file.CopyToAsync(stream);
            //        }
            //        Contract.Repository.Models.Log ff = new Contract.Repository.Models.Log();
            //        //var rootFolder = Path.Combine(Directory.GetCurrentDirectory(),"File");
            //        //var orgFolderPath = Path.Combine(log.Id);
            //        //ff.nombre = Path.GetFileNameWithoutExtension(file.FileName);
            //        //ff.tamanio = tamanio;
            //        log.NameFile = Path.GetFileName(file.FileName).ToString();
            //        //ff.ubicacion = FilePath;
            //        //inserto en la lista
            //        f.Add(ff);
            //    }
            //    //inserto en la db

            //}
            _logRepo.AddRange(log);
            _uof.SaveChanges();
        }
        public void Delete(Log log, string id)
        {
            //_logRepo.Delete(log);
            //_uof.SaveChanges();
            if (id == log.Id)
            {
                _logRepo.Delete(log);
                _uof.SaveChanges();
            }
        }
        public void Update(Log log, string id)
        {
            if (id == log.Id)
            {
                _logRepo.Update(log);
                _uof.SaveChanges();
            }
        }

        public void Create(Log log, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public void PostFiles(List<IFormFile> files, Log log)
        {
            throw new NotImplementedException();
        }

        //public void PostFiles([FromForm] List<IFormFile> files,Log log)
        //{

        //    string contentRootPath = _env.ContentRootPath;

        //    List<Contract.Repository.Models.Log> f = new List<Contract.Repository.Models.Log>();
        //    string path1 = _env.WebRootPath + "Files"; ;
        //    string path2 = Path.Combine(path1, log.Id.ToString());
        //    Directory.CreateDirectory(path2);


        //    if (files.Count > 0)
        //    {
        //        foreach (var file in files)
        //        {
        //            string s = file.FileName;
        //            var FilePath = path2 + "\\" + s;

        //            using (var stream = System.IO.File.Create(FilePath))
        //            {
        //                file.CopyToAsync(stream);
        //            }
        //            Contract.Repository.Models.Log ff = new Contract.Repository.Models.Log();
        //            ff.NameFile = Path.GetExtension(file.FileName).Substring(1);
        //            log.NameFile = Path.GetFileName(file.FileName).ToString();
        //            f.Add(ff);
        //        }
        //        //inserto en la db
        //        _logRepo.AddRange();
        //        _uof.SaveChanges();
        //    }
        //    _logRepo.AddRange();
        //    _uof.SaveChanges();
        //    //return OK(f);
        //}
        //public void groupAPI()
        //{

        //}
















        //}
        //public void Apply(Log log, AppContext context)
        //{
        //    if (log.Id.ToLower() == "apivaluesuploadpost")
        //    {
        //        log.NameFile.Clear();
        //        log.NameFile.Add(new NonBodyParameter
        //        {
        //            Name = "uploadedFile",
        //            In = "formData",
        //            Description = "Upload File",
        //            Required = true,
        //            Type = "file"
        //        });
        //        log.Consumes.Add("multipart/form-data");
        //    }
        //}





        //public void Apply(OpenApiOperation operation, OperationFilterContext context)
        //{
        //    var fileUploadMime = "multipart/form-data";
        //    if (operation.RequestBody == null || !operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMime, StringComparison.InvariantCultureIgnoreCase)))
        //        return;
        //    var fileParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType == typeof(IFormFile));
        //    operation.RequestBody.Content[fileUploadMime].Schema.Properties =
        //        fileParams.ToDictionary(k => k.Name, v => new OpenApiSchema()
        //        {
        //            Type = "string",
        //            Format = "binary"
        //        });
        //}
    }
}