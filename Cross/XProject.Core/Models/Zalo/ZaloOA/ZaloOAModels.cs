using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XProject.Core.Models.Zalo.ZaloOA
{
    public class Recipient
    {
        public string user_id { get; set; }
        public string message_id { get; set; }
    }

    public class Message
    {
        public string text { get; set; }
        public Attachment attachment { get; set; }
    }

    public class MessageBody
    {
        public Recipient recipient { get; set; }
        public Message message { get; set; }
    }

    public class MessageResponseData
    {
        public Quota quota { get; set; }
        public string message_id { get; set; }
        public string user_id { get; set; }
        public string attachment_id { get; set; }
        public string token { get; set; }
        public string type { get; set; }
        public int remain { get; set; }
        public int total { get; set; }
    }

    public class Element
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string image_url { get; set; }
        public DefaultAction default_action { get; set; }
    }

    public class DefaultAction
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Payload
    {
        public string template_type { get; set; }
        public List<Element> elements { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public Payload payload { get; set; }
    }

    public class Quota
    {
        public int total { get; set; }
        public int remain { get; set; }
        public string type { get; set; }
    }

    public class FollowerMessage
    {
        public int src { get; set; }
        public long time { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public string message_id { get; set; }
        public string from_id { get; set; }
        public string to_id { get; set; }
        public string from_display_name { get; set; }
        public string from_avatar { get; set; }
        public string to_display_name { get; set; }
        public string to_avatar { get; set; }
    }

    public class OAInfo
    {
        public long oa_id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string cover { get; set; }
        public bool is_verified { get; set; }
    }

    public class FollowerInfo
    {
        public string user_id { get; set; }
    }

    public class FollowerResponseData
    {
        public int total { get; set; }
        public List<FollowerInfo> followers { get; set; }
    }

    public class Avatars
    {
        public string _120 { get; set; }
        public string _240 { get; set; }
    }

    public class TagsAndNotesInfo
    {
        public List<string> tag_names { get; set; }
        public List<string> notes { get; set; }
    }

    public class UserInfo
    {
        public int user_gender { get; set; }
        public long user_id { get; set; }
        public long user_id_by_app { get; set; }
        public string avatar { get; set; }
        public Avatars avatars { get; set; }
        public string display_name { get; set; }
        public TagsAndNotesInfo tags_and_notes_info { get; set; }
        public string shared_info { get; set; }
    }

    public class ListDataRequest
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public int count { get; set; }
        public int from_time { get; set; }
        public int to_time { get; set; }
        public string user_id { get; set; }
        public string form_id { get; set; }
        public string tag_name { get; set; }
    }

    public class Question
    {
        public long questionId { get; set; }
        public string title { get; set; }
    }

    public class Answer
    {
        public long questionId { get; set; }
        public List<string> responses { get; set; }
    }

    public class UserResponses
    {
        public int submitTime { get; set; }
        public List<Answer> answers { get; set; }
    }

    public class UserResponsesData
    {
        public int total { get; set; }
        public List<Question> questions { get; set; }
        public List<UserResponses> responses { get; set; }
    }
}
