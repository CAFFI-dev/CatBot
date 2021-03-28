using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

using CatBot.Extensions;
namespace CatBot.Preconditions
{
    //класс-атрибут, который проверяет минимальные привилегии в дс канале
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MinPermissionsAttribute : PreconditionAttribute
    {
        private readonly PermissionLevel _level;

        public MinPermissionsAttribute(PermissionLevel level, string ErrorMessage = null)
        {
            _level = level;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider service)
        {
            var permission = GetPermission(context);

            return Task.FromResult(permission >= _level ? PreconditionResult.FromSuccess() : PreconditionResult.FromError("У тебя нет прав чтобы сделать это, ты здесь никто и звать тебя никак!"));
        }

        public PermissionLevel GetPermission(ICommandContext context)
        {
            var user = (SocketGuildUser)context.User;

            if (user.IsBot)
            {
                return PermissionLevel.Bot;
            }
            if (user.IsBotOwner())
            {
                return PermissionLevel.BotOwner;
            }
            if (user.IsTeamMember())
            {
                //0
            }
            if (user.IsGuildOwner(context.Guild))
            {
                return PermissionLevel.ServerOwner;
            }
            if (user.IsGuildAdministrator())
            {
                return PermissionLevel.ServerAdmin;
            }
            if (user.IsGuildModerator())
            {
                return PermissionLevel.ServerMod;
            }
            return PermissionLevel.User;
        }
    }
}
