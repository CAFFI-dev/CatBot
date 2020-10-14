using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CatBot.Modules
{
    public class ProfileCommands : ModuleBase<SocketCommandContext>
    {
        [RequireUserPermission(GuildPermission.ChangeNickname, ErrorMessage = "Не могу изменить тебе никнейм, т.к у тебя нет на это права: ***{change_nickname}***")]
        [Name("profile")]
        public class EditProfile : ModuleBase<SocketCommandContext>
        {
            [Command("profile info")]
            public async Task InfoSyntax()
            {
                await ReplyAsync("в разработке");
            }

            [Command("profile nickname")]
            public async Task ChangeNickname()
            {
                await ReplyAsync("пиздец в какой разработке");
            }
        }
    }
}
