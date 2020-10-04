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
    public class HelpCommands : ModuleBase<SocketCommandContext>
    {
        private EmbedBuilder _embed; //постройка embed ссылок

        public HelpCommands()
        {
            _embed = new EmbedBuilder();
        }
        [Command("help"), Alias("info")]
        public async Task Help([Remainder] string command = null)
        {
            if (command == null)
            {
                //начинаем создание embed
                //_embed.
                ////отправка ответа
                //await ReplyAsync(embed: embed);
            }
        }
    }
}
