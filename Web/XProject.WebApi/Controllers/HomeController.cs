using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XProject.Core.Constants;

namespace XProject.WebApi.Controllers
{    
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route(Endpoints.HomeEndpoint.BaseEndpoint)]
        public string Index()
        {
            return "asdasdad";
        }
    }
}
