using PastebinAPI.API.APIObject;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PastebinAPI.API
{
    public class Pastebin
    {
        public readonly PasteKey key;

        /// <summary>
        /// Create new pastebin object
        /// </summary>
        /// <param name="key">User and developer keys, can be created using "PasteKeyBuilder"</param>
        public Pastebin(PasteKey key)
        {
            this.key = key;
        }

        /// <summary>
        /// Create new paste
        /// </summary>
        /// <param name="paste">Paste builder with all informations</param>
        /// <returns>Paste url</returns>
        public async Task<string> CreateNewPasteAsync(PasteBuilder paste)
        {
            string data = await paste.BuildAsync(key.UserKey, key.DevKey);
            WebRequest request = WebRequest.Create("https://pastebin.com/api/api_post.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }

            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string url = await reader.ReadToEndAsync();
                return await Task.FromResult(url);
            }
        }

        /// <summary>
        /// Delete paste
        /// </summary>
        /// <param name="url">Paste url</param>
        /// <returns></returns>
        public async Task DeletePasteAsync(string url)
        {
            string[] code = url.Split('/');
            string data = $"api_option=delete&" +
                $"api_user_key={key.UserKey}&" +
                $"api_dev_key={key.DevKey}&" +
                $"api_paste_key={code[code.Length - 1]}";

            WebRequest request = WebRequest.Create("https://pastebin.com/api/api_post.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }

            await request.GetResponseAsync();
        }

        /// <summary>
        /// Read private/unlisted paste
        /// </summary>
        /// <param name="url">Paste url</param>
        /// <returns>Paste raw content</returns>
        public async Task<string> ReadOwnPasteAsync(string url)
        {
            string[] code = url.Split('/');
            string data = $"api_option=show_paste&" +
                $"api_user_key={key.UserKey}&" +
                $"api_dev_key={key.DevKey}&" +
                $"api_paste_key={code[code.Length - 1]}";
            WebRequest request = WebRequest.Create("https://pastebin.com/api/api_raw.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }

            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string text = await reader.ReadToEndAsync();
                return await Task.FromResult(text);
            }
        }

        /// <summary>
        /// Read public paste
        /// </summary>
        /// <param name="url">Paste url</param>
        /// <returns>Paste raw content</returns>
        public static async Task<string> ReadPasteAsync(string url)
        {
            string[] code = url.Split('/');
            WebRequest request = WebRequest.Create($"https://pastebin.com/raw/{code[code.Length - 1]}");
            request.Method = "GET";

            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string text = await reader.ReadToEndAsync();
                return await Task.FromResult(text);
            }
        }

        public async Task<IReadOnlyCollection<Paste>> GetPastesAsync(int limit = 50)
        {
            string data = $"api_option=list&" +
                $"api_user_key={key.UserKey}&" +
                $"api_dev_key={key.DevKey}&" +
                $"api_results_limit={limit}";
            WebRequest request = WebRequest.Create("https://pastebin.com/api/api_post.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }

            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string content = await reader.ReadToEndAsync();
                content = $"<pasteRoot>\n{content}\n</pasteRoot>";

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(content);

                XmlNode root = xml.GetElementsByTagName("pasteRoot")[0];
                XmlNodeList pastes = root.ChildNodes;

                List<Paste> result = new List<Paste>();
                foreach (XmlNode paste in pastes)
                    result.Add(await PasteParser.ParsePasteAsync(paste));

                return await Task.FromResult(result);
            }
        }
    }
}
