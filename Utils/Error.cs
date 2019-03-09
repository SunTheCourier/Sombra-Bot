﻿using Discord;
using System.Threading.Tasks;
using Discord.WebSocket;
using System;

namespace Sombra_Bot.Utils
{
    public class Error
    {
        public enum ExceptionType { User, Fatal }
        public static async Task Send(ISocketMessageChannel SendLocation, string Key = "An error has occured.", string Value = "View the help menu for help.", Exception e = null, ExceptionType et = ExceptionType.User)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Error");
            builder.AddField(Key, Value);
            builder.WithColor(Color.Red);
            builder.WithCurrentTimestamp();
            await SendLocation.SendMessageAsync(embed: builder.Build());
            if (et == ExceptionType.Fatal)
            {
                EmbedBuilder errorbuilder = new EmbedBuilder();
                errorbuilder.WithCurrentTimestamp();
                errorbuilder.WithColor(Color.Red);
                //should we send anything else?
                errorbuilder.WithDescription($"```{e.Message}\n\n{e.StackTrace}```");
                //Should we send this to App Owner or Repo Owner?
                await Program.AppInfo.Owner.SendMessageAsync(embed: errorbuilder.Build());
            }
        }
    }
}
