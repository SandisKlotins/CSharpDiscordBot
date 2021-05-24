using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Linq;

namespace CSharpDiscordBot.Modules
{
    class HtmlParser
    {
        public string Url { get; set; }
        private async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var html = client.GetStringAsync(fullUrl);
            return await html;
        }

        public List<List<string>> FetchGpuListingFields()
        {
            Url = "https://www.ss.lv/lv/electronics/computers/completing-pc/video/sell/";
            string html = CallUrl(Url).Result;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            List<List<string>> formatedListing = new List<List<string>>();

            var gpuListings = htmlDoc.DocumentNode.SelectNodes("//tr[contains(@id, 'tr_')]");
            foreach (var gpuListing in gpuListings) 
            {
                List<string> list = new List<string>();
                string link = String.Empty;
                HtmlNodeCollection row = gpuListing.SelectNodes(".//td[contains(@class, 'msg')]");
                if (row != null)
                {
                    foreach (var field in row)
                    {
                        var linkFound = field.SelectSingleNode(".//a[@class='am']");
                        if (string.IsNullOrEmpty(link) && linkFound != null && linkFound.Attributes["href"] != null)
                        {
                            link = linkFound.Attributes["href"].Value;
                        }
                        list.Add(field.InnerText);
                    }
                    list.Add(link);
                    formatedListing.Add(list);
                }
            }
            return formatedListing;
        }
    }
}
