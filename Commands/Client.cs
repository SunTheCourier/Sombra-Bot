﻿using Discord.Commands;
using Sombra_Bot.Utils;
using System;
using System.Threading.Tasks;

namespace Sombra_Bot.Commands
{
    public class Client : ModuleBase<SocketCommandContext>
    {
        [Command("Shutdown"), Summary("Shut downs the bot.")]
        [RequireOwner]
        public async Task ShutDown()
        {
            await Context.Channel.SendMessageAsync("Logging out and shutting down.");
            await Context.Client.LogoutAsync();
            await Task.Delay(12000);
            Environment.Exit(0);
        }

        [Command("Say"), Summary("Says the message sent.")]
        [RequireOwner]
        public async Task Say(params string[] input)
        {
            try
            {
                await Context.Message.DeleteAsync();
            }
            catch { }
            await Context.Channel.SendMessageAsync(string.Join(" ", input));
        }

        [Command("Invite"), Summary("Get an invite.")]
        public async Task GetInvite()
        {
            await Context.Channel.SendMessageAsync("Invite link: https://discordbots.org/bot/516009170353258496\nDiscord server: https://discord.gg/jQ8HuWE\nSource code: https://github.com/SunTheCourier/Sombra-Bot");
        }

        [Command("ClearTemp"), Summary("Clears the Temp Directory for Sombra Bot.")]
        [RequireOwner]
        public async Task Clear()
        {
            string size = Misc.ConvertToReadableSize(Program.roottemppath.GetSize());
            await Context.Channel.SendMessageAsync("Clearing temporary directory!");
            try
            {
                Program.roottemppath.DeleteContents();
            }
            catch
            {
                await Error.Send(Context.Channel, Value: "Failed to delete temporary directory.");
                return;
            }
            await Context.Channel.SendMessageAsync($"Done, cleared temporary directory of {size}");
        }
    }
}
