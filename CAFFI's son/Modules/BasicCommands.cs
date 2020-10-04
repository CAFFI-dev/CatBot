using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
namespace CatBot.Modules
{
    public class BasicCommands : ModuleBase<SocketCommandContext>
    {
        private EmbedBuilder _embed;

        public BasicCommands()
        {
            _embed = new EmbedBuilder(); //постройка embed-ссылок
        }
        [Command("поприветствуй"), Alias("скажипривет", "привет", "дарова", "ку", "здравствуй", "пивет")]
        public async Task Hello([Remainder] IGuildUser user = null)
        {
            if (user == null) await ReplyAsync($"Дарова ебать, {user.Mention}");
            else await ReplyAsync($"Дарова ебать, {Context.User.Mention}");
        }

        [Command("рандом")]
        public async Task Random(int min, int max)
        {
            var rnd = new Random();
            await ReplyAsync($":game_die: Рандомное число: {rnd.Next(min, max+1)}");
        }

        [Command("возраст")]
        public async Task Age([Remainder] IGuildUser user = null)
        {
            if(user == null) await ReplyAsync($"Ваш аккаунт пидора был создан {Context.User.CreatedAt}");
            else await ReplyAsync($"Аккаунт этого дебила был создан {user.CreatedAt}");
        }

        [Command("gachicide"), Alias("suicide")]
        [RequireBotPermission(GuildPermission.KickMembers, ErrorMessage = "**Ошибка:** данный бот не может кикнуть, пока у него нет права ***{kick_members}***")]
        public async Task Suicide() {
            //User.Username.KickAsync("Перестал слушать гачи");
        }

        [Command("ping")]
        [RequireBotPermission(GuildPermission.EmbedLinks, ErrorMessage = "Не могу узнать пинг, так как нету права ***{embed_links}***")]
        public async Task PingAsync()
        {
            _embed.WithTitle($"Информация о пинге для {Context.User.Username}");
            _embed.WithDescription($"{Context.Client.Latency} мс");
            _embed.WithColor(new Color(187, 78, 93));
            await ReplyAsync("", false, _embed.Build());
        }

        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "**Ошибка:** данный бот не может банить, пока у него нет права ***{ban_members}***")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "Да ты еблан, пробовать банить участника без наличия этого права! (нужен ***{ban_members}***")]
        public async Task BanMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null) { await ReplyAsync("Ты забыл указать имя пользователя!");
                return;
            }

            if(user == Context.User)
            {
                await ReplyAsync("Ты не можешь кикнуть себя! (но если так хочется, можешь сделать -gachicide");
                return;
            }

            if (user.IsBot)
            {
                await ReplyAsync("Пока запрещаю банить ботов (в том числе и меня!)");
                return;
            }

            if (user.GuildPermissions.BanMembers)
            {
                await ReplyAsync(user + " не может быть кикнут, т.к он **♂boss of the gym♂** (у него есть право ***ban_members***)");
            }
            if (reason == null) reason = "не указана";
            await Context.Guild.AddBanAsync(user, 1, reason);

            _embed.WithDescription($":white_check_mark: {user.Mention} был забанен.\n **Причина: ** {reason}");
            _embed.WithFooter(footer =>
            {
                footer
                .WithText("Список забаненных и разбаненных")
                .WithIconUrl("https://i.imgur.com/sj57VsU.png");
            });
            Embed embed = _embed.Build();
            await ReplyAsync(embed: embed);
            ITextChannel logChannel = Context.Client.GetChannel(12121) as ITextChannel;

            _embed.WithDescription($":white_check_mark: {user.Mention} был забанен.\n **Причина: ** {reason} " +
                $"\n **Забанил:** {Context.User.Mention}");
            _embed.WithFooter(footer =>
            {
                footer
                .WithText("Список забаненных и разбаненных")
                .WithIconUrl("https://i.imgur.com/sj57VsU.png");
            });
            Embed embedLog = _embed.Build();
            await user.SendMessageAsync(embed: embedLog);
            await logChannel.SendMessageAsync(embed: embedLog);
            await ReplyAsync(embed: embed);
        }

        [Command("unban")]
        [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "**Ошибка:** данный бот не может разбанить участника" +
            ", пока у него нет права ***{ban_members}***")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "Да ты еблан, пробовать разбанить участника без наличия этого права!" +
            " (нужен ***{ban_members}***")]
        public async Task UnbanMember(ulong userId)
        {

            if (userId.ToString() == null)
            {
                await ReplyAsync("Ты забыл указать id пользователя!");
                return;
            }
            IUser bannedUser = Context.Client.GetUser(userId);
            await Context.Guild.RemoveBanAsync(bannedUser);

            string user = bannedUser.Username;
            _embed.WithDescription($":white_check_mark: {user} был разбанен.");
            _embed.WithFooter(footer =>
            {
                footer
                   .WithText("Список забаненных и разбаненных")
                   .WithIconUrl("https://i.imgur.com/sj57VsU.png");
            });
            Embed embed = _embed.Build();
            await ReplyAsync(embed: embed);
            ITextChannel logChannel = Context.Client.GetChannel(12121) as ITextChannel;

            _embed.WithDescription($":white_check_mark: {user} был разбанен.\n **Разбанил:** {Context.User.Mention}.");
            _embed.WithFooter(footer =>
            {
                footer
                .WithText("Список забаненных")
                .WithIconUrl("https://i.imgur.com/sj57VsU.png");
            });
            Embed embedLog = _embed.Build();
            await bannedUser.SendMessageAsync(embed: embedLog);
            await logChannel.SendMessageAsync(embed: embedLog);
            await ReplyAsync(embed: embed);
        }
    }
}
