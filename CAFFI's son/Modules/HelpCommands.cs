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
                Embed embed = _embed.Build();
                //отправка ответа
                await ReplyAsync($"Команда в разработке, будет сделана после реализации банов, киков, мутов и разбанов \n" +
                    $"Доступные команды: -help, -ban, -kick, -random, -unban, -привет, -возраст, -ping, -prefix(в разработке), -gachicide(в разработке)");
            }
        }
    }
}
