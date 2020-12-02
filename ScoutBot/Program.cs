using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using ScoutBot.Services;

namespace ScoutBot
{
    public class Program
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        private Program()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                // How much logging do you want to see?
                LogLevel = LogSeverity.Info,

                // If you or another service needs to do anything with messages
                // (eg. checking Reactions, checking the content of edited/deleted messages),
                // you must set the MessageCacheSize. You may adjust the number as needed.
                //MessageCacheSize = 50,

                // If your platform doesn't have native WebSockets,
                // add Discord.Net.Providers.WS4Net from NuGet,
                // add the `using` at the top, and uncomment this line:
                //WebSocketProvider = WS4NetProvider.Instance
            });

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,

                CaseSensitiveCommands = false,
            });

            // Initialize config object and config service
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"../config.json");
            JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));

            // Subscribe the logging handler to both the client and the CommandService.
            _client.Log += LogAsync;
            _commands.Log += LogAsync;

            // Setup your DI container.
            _services = ConfigureServices();
        }

        // Example of a logging handler. This can be re-used by addons
        // that ask for a Func<LogMessage, Task>.
        private static Task LogAsync(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }

            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }

        // Program entry point
        static void Main(string[] args)
        {
            // Call the Program constructor, followed by the
            // MainAsync method and wait until it finishes (which should be never).
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        // If any services require the client, or the CommandService, or something else you keep on hand,
        // pass them as parameters into this method as needed.
        // If this method is getting pretty long, you can seperate it out into another file using partials.
        private IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection()
                // Repeat this for all the service classes
                // and other dependencies that your commands might need.
                .AddSingleton<DatabaseService>()
                .AddSingleton(new InteractiveService(_client));

            // When all your required services are in the collection, build the container.
            // Tip: There's an overload taking in a 'validateScopes' bool to make sure
            // you haven't made any mistakes in your dependency graph.
            return map.BuildServiceProvider(true);
        }

        private async Task MainAsync()
        {
            // Centralize the logic for commands into a separate method.
            CommandHandler commandHandler = new CommandHandler(_client, _commands, _services);
            await commandHandler.InstallCommandsAsync();

            // Login and connect.
            await _client.LoginAsync(TokenType.Bot, Config.DiscordToken);
            await _client.StartAsync();

            // Wait infinitely so the bot stays connected.
            await Task.Delay(Timeout.Infinite);
        }
    }
}