using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using XProject.Core.Models.Zalo;
using XProject.Core.Models.Zalo.ZaloOA;

namespace XProject.WebApi.Ultils
{
    public class OA_Followed
    {
        // remove null property for api call
        public static T RemoveNullProp<T>(T obj)
        {
            var serilaizeJson = JsonConvert.SerializeObject(obj, Formatting.None,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var result = JsonConvert.DeserializeObject<T>(serilaizeJson);

            return result;
        }

        #region Quyền gửi tin và thông báo qua OA

        // Send text message
        public static OAResponse<MessageResponseData> SendMessage(string access_token, string user_id,string text)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var body = new MessageBody();
            body.recipient = new Recipient() { user_id = user_id };
            body.message = new Message() { text = text };

            body = RemoveNullProp<MessageBody>(body);

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<MessageResponseData>>(response.Content);
        }

        // Send attachement message
        // https://developers.zalo.me/docs/api/official-account-api/gui-tin-va-thong-bao-qua-oa/gui-thong-bao-theo-mau-dinh-kem-anh-post-5068
        // https://developers.zalo.me/docs/api/official-account-api/gui-tin-va-thong-bao-qua-oa/gui-thong-bao-theo-mau-dinh-kem-danh-sach-post-5064
        // https://developers.zalo.me/docs/api/official-account-api/gui-tin-va-thong-bao-qua-oa/gui-thong-bao-theo-mau-yeu-cau-thong-tin-nguoi-dung-post-5055
        // https://developers.zalo.me/docs/api/official-account-api/gui-tin-va-thong-bao-qua-oa/gui-thong-bao-dinh-kem-file-post-5049
        public static OAResponse<MessageResponseData> SendMessage(string access_token, string user_id, Attachment attachment, string text = null)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var body = new MessageBody();
            body.recipient = new Recipient() { user_id = user_id };
            body.message = new Message() { text = text, attachment = attachment };

            body = RemoveNullProp<MessageBody>(body);

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<MessageResponseData>>(response.Content);
        }


        // Send response message
        // https://developers.zalo.me/docs/api/official-account-api/gui-tin-va-thong-bao-qua-oa/gui-lenh-phan-hoi-nguoi-dung-post-5027
        public static OAResponse<MessageResponseData> SendResponseMessage(string access_token, string message_id, string text)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var body = new MessageBody();
            body.recipient = new Recipient() { message_id = message_id };
            body.message = new Message() { text = text };

            body = RemoveNullProp<MessageBody>(body);

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<MessageResponseData>>(response.Content);
        }

        // Send File [ contentType = image | gif | file ]
        // https://developers.zalo.me/docs/api/official-account-api/upload-hinh-anh/upload-hinh-anh-post-5091
        // https://developers.zalo.me/docs/api/official-account-api/upload-hinh-anh/upload-anh-gif-post-5089
        // https://developers.zalo.me/docs/api/official-account-api/upload-hinh-anh/upload-file-post-5087
        public static OAResponse<MessageResponseData> SendFile(string access_token, string path, string contentType)
        {
            var client = new RestClient($"https://openapi.zalo.me/v2.0/oa/upload/{contentType}");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("access_token", access_token);

            request.AddFile("file", path);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<MessageResponseData>>(response.Content);
        }

        // Get Quota
        // https://developers.zalo.me/docs/api/official-account-api/quan-ly-tin-nhan-nguoi-quan-tam/lay-thong-tin-quota-lenh-chu-dong-mien-phi-post-5149
        public static OAResponse<MessageResponseData> GetQuota(string access_token,string message_id = null)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa/quota/message");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddJsonBody(new Recipient() { message_id = message_id });

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<MessageResponseData>>(response.Content);
        }

        #endregion Quyền gửi tin và thông báo qua OA

        #region Quyền quản lý tin nhắn người quan tâm

        // Get follower message list
        // https://developers.zalo.me/docs/api/official-account-api/quan-ly-tin-nhan-nguoi-quan-tam/lay-danh-sach-cac-hoi-thoai-gan-nhat-post-5144
        public static OAResponse<List<FollowerMessage>> GetFollowerMessages(string access_token,int offset = 0,int count = 10)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa/listrecentchat");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var data = new ListDataRequest() { offset = offset, count = count };

            request.AddParameter("data", request.JsonSerializer.Serialize(data));

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<List<FollowerMessage>>>(response.Content);
        }

        // Get follower message list with userid
        // https://developers.zalo.me/docs/api/official-account-api/quan-ly-tin-nhan-nguoi-quan-tam/lay-danh-sach-cac-hoi-thoai-voi-nguoi-quan-tam-post-5140
        public static OAResponse<List<FollowerMessage>> GetFollowerMessageById(string access_token, string user_id, int offset = 0, int count = 10)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa/conversation");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var data = new ListDataRequest() { user_id = user_id, offset = offset, count = count };

            request.AddParameter("data", request.JsonSerializer.Serialize(data));

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<List<FollowerMessage>>>(response.Content);
        }

        #endregion Quyền quản lý tin nhắn người quan tâm

        #region Quyền quản lý thông tin OA

        // Get OA info
        // https://developers.zalo.me/docs/api/official-account-api/quan-ly-thong-tin-official-account/lay-thong-tin-official-account-post-5135
        public static OAResponse<OAInfo> GetOAInfo(string access_token)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa/getoa");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<OAInfo>>(response.Content);
        }

        // Get followers
        // https://developers.zalo.me/docs/api/official-account-api/quan-ly-thong-tin-official-account/lay-danh-sach-nguoi-quan-tam-post-5133
        public static OAResponse<FollowerResponseData> GetFollowers(string access_token,int offset,int count,string tag_name = null)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa/getfollowers");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var data = new ListDataRequest() { offset = offset, count = count, tag_name = tag_name };
            data = RemoveNullProp<ListDataRequest>(data);

            request.AddParameter("data", request.JsonSerializer.Serialize(data));

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<FollowerResponseData>>(response.Content);
        }

        // Get follower profile by userid
        // https://developers.zalo.me/docs/api/official-account-api/quan-ly-thong-tin-official-account/lay-thong-tin-nguoi-quan-tam-post-5129
        public static OAResponse<UserInfo> GetFollowerProfile(string access_token, string user_id)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa/getprofile");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var data = new ListDataRequest() { user_id = user_id };

            request.AddParameter("data", request.JsonSerializer.Serialize(data));

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<UserInfo>>(response.Content);
        }

        #endregion Quyền quản lý thông tin OA

        #region Quyền quản lý ads

        // Get potential customers
        // https://developers.zalo.me/docs/api/official-account-api/quan-ly-ads/lay-du-lieu-khach-hang-tiem-nang-post-5478
        public static OAResponse<UserResponsesData> GetPotentialCustomers(string access_token, string form_id,int from_time,int to_time,int offset,int limit)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/oa/form/get");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddParameter("form_id", form_id);
            request.AddParameter("from_time", from_time);
            request.AddParameter("to_time", to_time);
            request.AddParameter("offset", offset);
            request.AddParameter("limit", limit);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<UserResponsesData>>(response.Content);
        }

        #endregion Quyền quản lý ads

        #region Quyền quản lý bài viết

        // https://developers.zalo.me/docs/api/article-api-151

        // Create normal artical
        public static OAResponse<ArticalId> CreateArtical(string access_token, Artical model)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/create");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddParameter("access_token", access_token);

            request.AddJsonBody(RemoveNullProp(model));

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ArticalId>>(response.Content);
        }

        // Create video artical
        public static OAResponse<ArticalId> CreateVideoArtical(string access_token, Artical model)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/video/create");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("access_token", access_token);

            request.AddJsonBody(RemoveNullProp(model));

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ArticalId>>(response.Content);
        }

        // Update video artical
        public static OAResponse<ArticalId> UpdateVideoArtical(string access_token, Artical model)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/video/update");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddJsonBody(RemoveNullProp(model));

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ArticalId>>(response.Content);
        }


        // Upload file and get Token
        public static OAResponse<ArticalId> UploadFileAndGetToken(string access_token,string path)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/upload_video/preparevideo");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "form-data/multipart");
            request.AddHeader("access_token", access_token);

            request.AddFile("file", path);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ArticalId>>(response.Content);
        }

        // Upload video
        public static OAResponse<ContentStatus> VerifyVideo(string access_token, string token)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/upload_video/verify");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);
            request.AddHeader("token", token);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ContentStatus>>(response.Content);
        }

        // Get Artical Id
        public static OAResponse<ArticalId> GetArticalId(string access_token, string token)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/verify");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var body = new ArticalId { token=token};
            body = RemoveNullProp(body);

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ArticalId>>(response.Content);
        }

        // Get Video Artical Id
        public static OAResponse<Artical> GetVideoArticalId(string access_token, string id)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/video/getdetail");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddParameter("id", id);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<Artical>>(response.Content);
        }

        // Get Video Artical list
        public static OAResponse<ListVideoArtical> GetVideoArticalId(string access_token, int offset,int limit = 1)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/video/getslice");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddParameter("offset", offset);
            request.AddParameter("limit", limit);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ListVideoArtical>>(response.Content);
        }

        // Get Artical info
        public static OAResponse<Artical> GetArticalInfo(string access_token, string id)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/getdetail");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddParameter("id", id);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<Artical>>(response.Content);
        }

        // Get Artical list
        public static OAResponse<ListVideoArtical> GetArticals(string access_token, int offset,int limit,string type)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/getslice");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddParameter("offset", offset);
            request.AddParameter("limit", limit);
            request.AddParameter("type", type);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ListVideoArtical>>(response.Content);
        }

        // Delete Artical
        public static OAResponse<ArticalId> DeleteArtical(string access_token, string id)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/remove");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var body = new ArticalId() { id=id};
            body = RemoveNullProp(body);

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ArticalId>> (response.Content);
        }

        // Update Artical
        public static OAResponse<ArticalId> UpdateArtical(string access_token, Artical model)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/article/update");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var body = RemoveNullProp<Artical>(model);

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ArticalId>>(response.Content);
        }

        #endregion Quyền quản lý bài viết

        #region Quyền quản lý cửa hàng, đơn hàng

        // https://developers.zalo.me/docs/api/shop-api-new/api/danh-muc-post-6455

        // Get category list
        public static OAResponse<List<ZaloCategory>> GetCategories(string access_token)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/mstore/cate/getcateofoa");
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("access_token", access_token);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<List<ZaloCategory>>>(response.Content);
        }

        // Create category
        public static string CreateCategory(string access_token, ZaloCategoryRequest categoryRequest)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/mstore/cate/create");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddJsonBody(categoryRequest);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ZaloCategory>>(response.Content).data.id;
        }

        // Update category
        public static string UpdateCategory(string access_token, ZaloCategoryRequest categoryRequest)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/mstore/cate/update");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            request.AddJsonBody(categoryRequest);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ZaloCategory>>(response.Content).data.id;
        }

        // Delete category
        public static string DeleteCategory(string access_token, string id)
        {
            var client = new RestClient("https://openapi.zalo.me/v2.0/mstore/cate/remove");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("access_token", access_token);

            var body = new ZaloCategory() { id = id };
            body = RemoveNullProp<ZaloCategory>(body);

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<OAResponse<ZaloCategory>>(response.Content).data.id;
        }

        #endregion Quyền quản lý cửa hàng, đơn hàng
    }
}
