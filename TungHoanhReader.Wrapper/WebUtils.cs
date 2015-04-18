using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TungHoanhReader.Wrapper
{
    class WebUtils
    {
        public  async static Task<WebResult> DoRequestSimpleGet(string requestUri, CookieContainer cookie = null,string useragent = "",string referer="")
        {
            var result = new WebResult();
            result.Status = false;
            try
            {
                var handler = new HttpClientHandler();
                if (cookie != null) handler.CookieContainer=cookie;
                var client = new HttpClient(handler);
                if (!string.IsNullOrEmpty(useragent))
                {
                    client.DefaultRequestHeaders.UserAgent.Clear();
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(useragent));
                }
                client.BaseAddress = new Uri(requestUri);
                if (!string.IsNullOrEmpty(referer))
                {
                    client.DefaultRequestHeaders.Referrer = new Uri(referer);
                }
                result.Data = await client.GetStringAsync(requestUri);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Data = ex.Message;
                return result;
            }
            return result;
        }


        internal class WebResult
        {
            public bool Status { get; set; }
            public string Data { get; set; }
        }
    }
}
