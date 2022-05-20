using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Core.Models.Zalo.ZaloOA
{
    public class Cover
    {
        public string cover_type { get; set; }

        public string photo_url { get; set; }

        public string cover_view { get; set; }
        public string video_id { get; set; }

        public string status { get; set; }
    }

    public class Content
    {
        public string type { get; set; }
        public long create_date { get; set; }
        public long update_date { get; set; }
        public string content { get; set; }
        public int total_view { get; set; }
        public int total_share { get; set; }
        public int link_view { get; set; }
        public string url { get; set; }
        public string caption { get; set; }
        public string title { get; set; }
        public string video_id { get; set; }
        public string status { get; set; }
        public string id { get; set; }
        public string thumb { get; set; }
    }

    public class ArticalId
    {
        public string id { get; set; }
        public string token { get; set; }
    }

    public class ListVideoArtical
    {
        public List<Content> medias { get; set; }
        public int total { get; set; }
    }

    public class Artical
    {
        public string id { get; set; }
        public string type { get; set; } = "normal";
        public string title { get; set; }
        public string author { get; set; }
        public Cover cover { get; set; }
        public string description { get; set; }
        public List<Content> body { get; set; }
        public List<ArticalId> related_medias { get; set; }
        public string tracking_link { get; set; }
        public string video_id { get; set; }
        public string avatar { get; set; }
        public string status { get; set; } = "show";
        public string comment { get; set; } = "show";
    }

    public class ContentStatus
    {
        public string status_message { get; set; }
        public string video_name { get; set; }
        public long video_size { get; set; }
        public int convert_percent { get; set; }
        public int convert_error_code { get; set; }
        public string video_id { get; set; }
        public int status { get; set; }
    }
}
