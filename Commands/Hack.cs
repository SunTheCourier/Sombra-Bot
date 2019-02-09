﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Sombra_Bot.Utils;

namespace Sombra_Bot.Commands
{
    public class Hack : ModuleBase<SocketCommandContext>
    {
        [Command("Hack"), Summary("Bans or kicks a user.")]
        public async Task ManageUser(int level, IGuildUser user, params string[] reason)
        {
            string fullreason = string.Join(" ", reason);
            IGuildUser cmduser = Context.User as IGuildUser;

            switch (level)
            {
                case 1:
                    if (cmduser.GuildPermissions.KickMembers)
                    {
                        if (Context.User == user)
                        {
                            await Error.Send(Context.Channel, Value: "You cannot kick youself.");
                            return;
                        }

                        try
                        {
                            await user.KickAsync(fullreason);
                            await Context.Channel.SendMessageAsync(Getmessage());
                            await Context.Channel.SendMessageAsync($"{user.Mention} hacked (kicked)!");
                        }
                        catch
                        {
                            await Error.Send(Context.Channel, Value: "User could not be kicked, I do not have enough permissions.");
                        }
                    }
                    else await Error.Send(Context.Channel, Value: "User has insufficent permissions.");
                    break;

                case 2:
                    if (cmduser.GuildPermissions.BanMembers)
                    {
                        if (Context.User == user)
                        {
                            await Error.Send(Context.Channel, Value: "You cannot ban youself.");
                            return;
                        }

                        try
                        {
                            await Context.Guild.AddBanAsync(user, reason: fullreason);
                            await Context.Channel.SendMessageAsync(Getmessage());
                            await Context.Channel.SendMessageAsync($"{user.Mention} hacked! (banned)");
                        }
                        catch
                        {
                            await Error.Send(Context.Channel, Value: "User could not be banned, I do not have enough permissions.");
                        }
                    }
                    else await Error.Send(Context.Channel, Value: "User has insufficent permissions.");
                    break;

                default:
                    await Error.Send(Context.Channel, Key: "Inputted level does not exist");
                    break;
            }
        }
        private string Getmessage()
        {
            Random rng = new Random();
            switch (rng.Next(1, 4))
            {
                case 1:
                    return "Initiating the hack.";
                case 2:
                    return "Iniciando el hackeo.";
                case 3:
                    return "Don't mind me.";
                case 4:
                    return "Let's get started.";
            }
            return null;
        }
    }
}
