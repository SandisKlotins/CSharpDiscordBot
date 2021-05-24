using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CSharpDiscordBot.Modules
{
    public class Commands : ModuleBase
    {
        [Command("ping")]
        public async Task Ping() 
        {
            await Context.Channel.SendMessageAsync("pong!");
        }

        [Command("ssGpus")]
        public async Task GetSsGpuListings() 
        {
            HtmlParser html = new HtmlParser();
            List<List<string>> gpuListings = html.FetchGpuListingFields();
            List<string> desireableGpus = new List<string>(){ "2070", "2080", "3060", "3070", "3080", "6700", "6800", "6900" };
            var builder = new EmbedBuilder()
                .WithDescription("Latest gpu listings from ss.lv: ")
                .AddField("Listing date: ", DateTime.Now.ToString("dd/MM/yyyy, HH:mm"));
            gpuListings.ForEach(delegate (List<string> row)
            {
                // price can come in this form: 1,400    ? soooooooooooo:
                // Remove last symbol of the string, then remove any commas in the string
                string trim = row[7].Remove(row[7].Length - 1, 1).Replace(",", "");
                // Remove any whitespaces from the string and convert to int
                int price = Int16.Parse(Regex.Replace(trim, @"\s", ""));
                string model = Regex.Match(row[3], @"\d+").Value;
                string link = "https://www.ss.lv/"+row[8];
                if (price <= 1500 & desireableGpus.Contains(model))
                {
                    builder.AddField($"{row[3]}", $"{row[6]} - {row[7]} - [Listing link]({link})", false);
                    builder.WithUrl(link);
                }
            });
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        } 
    }
}
