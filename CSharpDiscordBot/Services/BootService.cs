using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDiscordBot.Services
{
    public class BootService
    {
        public static IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public BootService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config) 
        {
            _provider = provider;
            _discord = discord;
            _commands = commands;
            _config = config;
        }

        public async Task StartAsync() 
        {
            string token = _config["token"];
            if (string.IsNullOrEmpty(token)) 
            {
                Console.WriteLine("Failed to read token. Check your json config");
                return;
            }
            await _discord.LoginAsync(TokenType.Bot, token);
            await _discord.StartAsync();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}
