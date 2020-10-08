using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;

using CatBot.Preconditions;

namespace CatBot.Modules
{
    [MinPermissions(PermissionLevel.User)]
    [Name("help")]
    public class HelpCommands : ModuleBase<SocketCommandContext>
    {
        public readonly EmbedBuilder _embed; //постройка embed ссылок
        public readonly CommandService _commandService;
        public HelpCommands()
        {
            _commandService = MainProgram.commands;
            _embed = new EmbedBuilder();
        }

        [Command("help"), Alias("info")]
        [Summary("Команда, показывающая что твой iq < 1")]
        public async Task Help([Remainder] string postcommand = null)
        {
            List<CommandInfo> commands = _commandService.Commands.ToList();
            if (postcommand == null)
            {
                string url = Context.User.GetAvatarUrl();
                if (url == null) url = Context.User.GetDefaultAvatarUrl();

                //начинаем создание embed
                _embed.WithTitle("Доступные команды:");
                foreach (CommandInfo command in commands)
                {
                    string embedField = command.Summary ?? "Описание недоступно. \n";

                    _embed.AddField($"`{command.Name}`", embedField);
                }
                _embed.WithFooter(footer =>
                {
                    footer
                    .WithIconUrl(url)
                    .WithText($"Вызвано {Context.User.Username}");
                });
                _embed.WithCurrentTimestamp();

                Embed embed = _embed.Build();
                //отправка ответа
                await ReplyAsync(embed: embed);
            }
            else
            {
                var result = _commandService.Search(Context, postcommand);
                string url = Context.User.GetAvatarUrl();
                if (url == null) url = Context.User.GetDefaultAvatarUrl();

                _embed.WithFooter(footer =>
                {
                    footer
                    .WithIconUrl(url)
                    .WithText($"Вызвано {Context.User.Username}");
                });
                _embed.WithCurrentTimestamp();

                if (!result.IsSuccess)
                {
                    _embed.WithTitle($"Команда `{postcommand}`");
                    _embed.WithDescription($"*Такой команды не существует.*");
                    _embed.WithColor(255, 000, 000);
                    Embed embed = _embed.Build();
                    //отправка ответа
                    await ReplyAsync(embed: embed);
                    return;
                }

                _embed.WithColor(000, 255, 000);
                _embed.WithTitle($"Команды, похожие на: ** {postcommand} ** ");

                foreach (var match in result.Commands)
                {
                    var cmd = match.Command;

                    _embed.AddField(field =>
                    {
                        field.Name = string.Join(", ", cmd.Aliases);
                        field.Value = $"Описание: {cmd.Summary} \n" +
                        $"Параметры: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                        $"Ремарки dev: {cmd.Remarks}";
                        field.IsInline = false;
                    });
                }
                Embed embedSuccess = _embed.Build();
                await ReplyAsync(embed: embedSuccess);
            }
        }
    }
}
