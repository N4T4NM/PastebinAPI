using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

namespace PastebinAPI.API.APIObject
{
    public class PasteBuilder
    {
        /// <summary>
        /// Paste title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Paste content
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Paste visibility
        /// </summary>
        public Visibility Visibility { get; set; }
        /// <summary>
        /// Paste expire time
        /// </summary>
        public ExpireTime ExpireTime { get; set; }
        /// <summary>
        /// Paste laguage, ex: csharp, php, vbnet...
        /// </summary>
        public string Format { get; set; }

        internal async Task<string> BuildAsync(string userKey, string devKey)
        {
            string date = (ExpireTime.GetType().GetField(ExpireTime.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[])[0].Description;

            string requestData = $"api_option=paste&" +
                $"api_user_key={userKey}&" +
                $"api_paste_private={(int)Visibility}&" +
                $"api_paste_name={WebUtility.UrlEncode(Title)}&" +
                $"api_paste_expire_date={date}&" +
                $"api_paste_format={Format}&" +
                $"api_dev_key={devKey}&" +
                $"api_paste_code={WebUtility.UrlEncode(Text)}";

            return await Task.FromResult(requestData);
        }
    }
}
