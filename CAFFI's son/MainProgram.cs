using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;

namespace CatBot
{
    public class MainProgram
    {
        public static void Main(string[] args)
       => new MainProgram().MainAsync().GetAwaiter().GetResult();


        //я заебался 2x - вечно все переделываю
        private static DiscordSocketClient discord;
        public static CommandService commands;
        private static IServiceProvider services;

        /// <summary>
        /// Стартовая точка приложения
        /// </summary>
        /// <returns>выход</returns>
        public async Task MainAsync()
        {
            Console.WriteLine("Нажми Ctrl + C или закрой это окно чтобы закрыть бота");
            Console.Title = DataConstants.Version + " client";

            //инициализация всего
            discord = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });
            commands = new CommandService();

            services = new ServiceCollection()
                .AddSingleton(discord)
                .AddSingleton(commands)
                .BuildServiceProvider();
            var token = ReadFiles();
            discord.Log += DiscordLog;
            //регистрация команд
            await RegisterCommandsAsync();
            //подключение бота
            await discord.LoginAsync(TokenType.Bot, token);
            await discord.StartAsync();
            //статус
            await discord.SetGameAsync("В данный момент в разработке!");

            discord.Ready += () =>
            {
                Console.WriteLine("Бот присоединен!");
                return Task.CompletedTask;
            };
            //удалить - не работает
            //await CatBot.Database.DBConnection.InitializeComponent().ConfigureAwait(true);

            //блокировка выполнения посл.задания, чтобы программу можно было закрыть только пользователем
            await Task.Delay(-1);
        }

        public string ReadFiles()
        {
            StreamReader reader = new StreamReader("config.txt");
            while (!reader.EndOfStream)
            {
                if (reader.ReadLine() == "token:") break;
            }
            string token = reader.ReadLine();

            return token;
        }
        public Task DiscordLog(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            discord.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(discord, message);
            if (message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix(DataConstants.Prefix, ref argPos))
            {
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}
