using System;
using System.Collections.Generic;
using System.Text;

namespace CatBot.Preconditions
{
    //контейнер хранения всех уровней привилегий сервера
    public enum PermissionLevel
    {
        Bot,
        User,
        ServerMod,
        ServerAdmin,
        ServerOwner,
        TeamMember,
        BotOwner
    }
}
