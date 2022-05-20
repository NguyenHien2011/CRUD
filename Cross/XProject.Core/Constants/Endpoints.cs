using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Core.Constants
{
    public class Endpoints
    {
        public const string ApiLog = "log/";
        public static class HomeEndpoint
        {
            public const string Area = "";

            public const string BaseEndpoint = "~/" + Area;
            public const string ControllergetLog = "~/log" + Area;
            public const string ControllercreateLog = "~/create" + Area;
            public const string ControllereditLog = "~/edit" + Area;
            public const string ControllerdeleteLog = "~/delete" + Area;
            public const string ControllerupdateLog = "~/update" + Area;
            public const string ControllerdetailsLog = "~/details" + Area;
        }
        public static class LogEndpoint
        {
            public const string Area = "" + ApiLog;
            public const string GetLog = "~/" + Area + "get-log/";
            public const string CreateLog = "~/" + Area + "create-log/";
            public const string DeleteLog = "~/" + Area + "delete-log/";
            public const string UpdateLog = "~/" + Area + "update-log/";
            public const string UploadFile = "~/" + Area + "uploadfile-log/";
        }
    }
}
