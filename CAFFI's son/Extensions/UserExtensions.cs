using System;
using System.Collections.Generic;
using System.Text;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
namespace CatBot.Extensions
{
    public static class UserExtensions
    {
        #region User Extensions
        public static bool IsBotOwner(this IUser user)
        {
            return user.Id == DataConstants.OwnerId;
        }

        public static bool IsTeamMember(this IUser user)
        {
            //to do
            return false;
        }

        public static bool IsGuildOwner(this SocketGuildUser user, IGuild guild)
        {
            return guild.OwnerId == user.Id;
        }

        public static bool IsGuildAdministrator(this SocketGuildUser user)
        {
            return user.GuildPermissions.Administrator;
        }

        public static bool IsGuildModerator(this SocketGuildUser user)
        {
            return (user.GuildPermissions.KickMembers || user.GuildPermissions.BanMembers || user.GuildPermissions.ManageMessages || user.GuildPermissions.ManageChannels);
        }
        //to do
        //joindate createdate
        #endregion
    }
}
