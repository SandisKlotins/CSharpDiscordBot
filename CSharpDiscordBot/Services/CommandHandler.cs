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
    public class CommandHandler
    {
        public static IServiceProvider _provider;
        public static DiscordSocketClient _discord;
        public static CommandService _commands;
        public static IConfigurationRoot _config;
        public CommandHandler(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config)
        {
            _provider = provider;
            _discord = discord;
            _commands = commands;
            _config = config;

            _discord.Ready += OnReady;
            _discord.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message.Author.IsBot) return;
            var context = new SocketCommandContext(_discord, message);
            int argPos = 0;
            if (message.HasStringPrefix(_config["prefix"], ref argPos) || message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) 
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);
                if (!result.IsSuccess) 
                {
                    await context.Channel.SendMessageAsync($"The following shit happened: \n {result.Error}");
                }
            }

        }

        private Task OnReady()
        {
            Console.WriteLine($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
            return Task.CompletedTask;
        }
    }
}
