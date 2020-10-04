using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
namespace CatBot.Modules
{
    class MathGame
    {
        //public static bool gameIsActive;
        //public Task PlayGame(SocketMessage message)
        //{
        //    if (!gameIsActive)
        //    {
        //        message.Channel.SendMessageAsync("Начинаю мини-игру... [DEBUG]");
        //        gameIsActive = true;
        //    }
        //    else message.Channel.SendMessageAsync("Мини-игра уже начата! !restart для перезапуска [DEBUG]");
        //    return Task.CompletedTask;
        //}

        //public Task StopGame(SocketMessage message)
        //{
        //    if (gameIsActive)
        //    {
        //        gameIsActive = false;
        //        message.Channel.SendMessageAsync("Спасибо за игру, " + message.Author.Mention + "!");
        //    }
        //    else message.Channel.SendMessageAsync("Ты шо дебил? Ты даже не начинал игру!");
        //    return Task.CompletedTask;
        //}
    }
}
