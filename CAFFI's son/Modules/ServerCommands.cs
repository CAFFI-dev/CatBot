using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

using CatBot.Preconditions;
namespace CatBot.Modules
{
    [Name("server")]
    public class ServerCommands : ModuleBase<SocketCommandContext>
    {
        private readonly EmbedBuilder _embed; //постройка embed ссылок

        public ServerCommands()
        {
            _embed = new EmbedBuilder();
        }

        #region pingCommands
        [Command("ping")]
        [Summary("Определяет ping между ботом и дискорд сервером.")]
        [MinPermissions(PermissionLevel.ServerMod)]
        [RequireBotPermission(GuildPermission.EmbedLinks, ErrorMessage = "Не могу узнать пинг, так как нету права ***{embed_links}***")]
        public async Task PingAsync()
        {
            _embed.WithTitle($"Информация о пинге для {Context.User.Username}");
            _embed.WithDescription($"{Context.Client.Latency} мс");
            _embed.WithColor(new Color(187, 78, 93));
            await ReplyAsync("", false, _embed.Build());
        }
        #endregion

        #region prefix
        [RequireBotPermission(GuildPermission.EmbedLinks, ErrorMessage = "Не могу изменить префикс, так как нету права ***{embed_links}***")]
        [RequireBotPermission(GuildPermission.SendMessages, ErrorMessage = "Не могу писать сообщения, так как нету права ***{send_messages}***")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "Чтобы пользоваться этой командой, нужно иметь права **администратора**!")]
        [Summary("Изменяет глобальный префикс сервера (добавлю когда будет готова дб)")]
        [Command("prefix"), Alias("addprefix", "mdprefix")]
        public async Task SetPrefixAsync([Remainder] string prefix = null)
        {
            if (prefix == null)
            {
                await ReplyAsync("Чтобы изменить префикс бота, нужно ввести префикс!");
                return;
            }

            //var server = await GetOrAddServerAsync(Context.Guild.Id, Context.Guild.Name, Context.Guild.MemberCount).ConfigureAwait(false);
            ////начинаем создание embed
            //if (!prefix.Contains("*") && !prefix.Contains("`") && !prefix.Contains("~"))
            //{
            //    server.Prefix = prefix;
            //    customPrefixService.StorePrefix(prefix, Context.Guild.Id);
            //    _embed.WithDescription("Successfully added custom prefix!\n" +
            //                           $"Example **{prefix}ping**");}
            _embed.WithColor(new Color(255, 255, 255));
            //отправка ответа
            await ReplyAsync(embed: _embed.Build()).ConfigureAwait(false);
        }

        [RequireBotPermission(GuildPermission.EmbedLinks, ErrorMessage = "Не могу изменить префикс, так как нету права ***{embed_links}***")]
        [RequireBotPermission(GuildPermission.SendMessages, ErrorMessage = "Не могу писать сообщения, так как нету права ***{send_messages}***")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "Чтобы пользоваться этой командой, нужно иметь права **администратора**!")]
        [Summary("Ставит стандартный префикс бота")]
        [Command("defaultprefix"), Alias("removeprefix", "rmprefix")]
        public async Task ClearPrefixAsync()
        {
            //var server = await GetOrAddServerAsync(Context.Guild.Id, Context.Guild.Name, Context.Guild.MemberCount).ConfigureAwait(false);
            ////начинаем создание embed
            //if (!prefix.Contains("*") && !prefix.Contains("`") && !prefix.Contains("~"))
            //{
            //    server.Prefix = prefix;
            //    customPrefixService.StorePrefix(prefix, Context.Guild.Id);
            //    _embed.WithDescription("Successfully added custom prefix!\n" +
            //                           $"Example **{prefix}ping**");}
            _embed.WithColor(new Color(255, 255, 255));
            //отправка ответа
            await ReplyAsync(embed: _embed.Build()).ConfigureAwait(false);
        }
        #endregion
    }
}
