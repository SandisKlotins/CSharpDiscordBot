using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using CSharpDiscordBot.Services;

namespace CSharpDiscordBot
{
    class Program
    {
        public static async Task Main(string[] args)
            => await Boot.RunAsync(args);
	}
}
