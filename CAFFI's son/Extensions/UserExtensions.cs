//thanks for MythicalCuddles, his template repository helped me to do UserExtensions.cs
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

        #region Permission Comparing
        public static int GetUserPermissionLevel(this SocketGuildUser user)
        {
            if (user.IsBotOwner())
            {
                return 10;
            }

            if (user.IsTeamMember())
            {
                return 9;
            }

            if (user.IsBot)
            {
                return 8;
            }

            if (user.IsGuildOwner(user.Guild))
            {
                return 7;
            }

            if (user.IsGuildAdministrator())
            {
                return 6;
            }

            if (user.IsGuildModerator())
            {
                return 5;
            }

            return 1;
        }

        public static bool HasHigherPermissionLevel(this IUser contextUser, IUser userMentioned)
        {
            return ((contextUser as SocketGuildUser).GetUserPermissionLevel() >=
                (userMentioned as SocketGuildUser).GetUserPermissionLevel());
        }
        #endregion
    }
}
