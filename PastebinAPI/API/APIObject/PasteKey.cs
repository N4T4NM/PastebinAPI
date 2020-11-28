using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPI.API.APIObject
{
    /// <summary>
    /// Pastebin user keys
    /// </summary>
    public struct PasteKey
    {
        public string UserKey;
        public string DevKey;
    }

    public class PasteKeyBuilder
    {
        private PasteKeyBuilder()
        {

        }

        /// <summary>
        /// Create new pastebin key object
        /// </summary>
        /// <param name="devKey">Your develepor key, can be found at: https://pastebin.com/doc_api</param>
        /// <param name="username">Your profile name</param>
        /// <param name="password">Your profile password</param>
        /// <returns>New PasteKey instance</returns>
        public static async Task<PasteKey> GenerateKeyAsync(string devKey, string username, string password)
        {
            WebRequest request = WebRequest.Create("https://pastebin.com/api/api_login.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string data = $"api_dev_key={devKey}&" +
                $"api_user_password={WebUtility.UrlEncode(password)}&" +
                $"api_user_name={WebUtility.UrlEncode(username)}";

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                await stream.WriteAsync(buffer, 0, buffer.Length);
                stream.Close();
            }


            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string userKey = await reader.ReadToEndAsync();
                return await Task.FromResult(new PasteKey() { UserKey = userKey, DevKey = devKey });
            }
        }
    }

}
