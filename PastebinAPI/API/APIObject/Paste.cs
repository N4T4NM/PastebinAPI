using System;
using System.Threading.Tasks;
using System.Xml;

namespace PastebinAPI.API.APIObject
{
    public struct Paste
    {
        public string Key;
        public DateTime CreationDate;
        public string Title;
        public int Size;
        public DateTime ExpireDate;
        public Visibility Visibility;
        public string FormatShort;
        public string FormatLong;
        public string Url;
        public int Hits;
    }

    public class PasteParser
    {
        private PasteParser()
        {

        }

        public static async Task<Paste> ParsePasteAsync(XmlNode paste)
        {
            string key = paste.SelectSingleNode("paste_key").InnerText;
            int creationTimestamp = int.Parse(paste.SelectSingleNode("paste_date").InnerText);
            string title = paste.SelectSingleNode("paste_title").InnerText;
            int size = int.Parse(paste.SelectSingleNode("paste_size").InnerText);
            int expireTimestamp = int.Parse(paste.SelectSingleNode("paste_expire_date").InnerText);
            int visibility = int.Parse(paste.SelectSingleNode("paste_private").InnerText);
            string @short = paste.SelectSingleNode("paste_format_short").InnerText;
            string @long = paste.SelectSingleNode("paste_format_long").InnerText;
            string url = paste.SelectSingleNode("paste_url").InnerText;
            int hits = int.Parse(paste.SelectSingleNode("paste_hits").InnerText);

            DateTime timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            Paste parsed = new Paste();
            parsed.Key = key;
            parsed.CreationDate = timestamp.AddSeconds(creationTimestamp).ToLocalTime();
            parsed.Title = title;
            parsed.Size = size;
            parsed.ExpireDate = timestamp.AddSeconds(expireTimestamp).ToLocalTime();
            parsed.Visibility = (Visibility)visibility;
            parsed.FormatShort = @short;
            parsed.FormatLong = @long;
            parsed.Url = url;
            parsed.Hits = hits;

            return await Task.FromResult(parsed);
        }
    }
}
