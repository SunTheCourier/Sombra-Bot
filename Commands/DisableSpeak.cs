﻿using Discord.Commands;
using Sombra_Bot.Utils;
using System.Collections.Generic;
using System.IO;
using Discord;
using System.Threading.Tasks;
using System.Linq;

namespace Sombra_Bot.Commands
{
    public class DisableSpeak : ModuleBase<SocketCommandContext>
    {
        [Command("DisableMemes"), Summary("Disables Sombra bot's random memes.")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task DisableChat()
        {
            if (Save.DisabledMServers.Contains(Context.Guild.Id.ToString()))
            {
                await Context.Channel.SendMessageAsync("Memes for the server have been disabled.");
                return;
            }

            Save.DisabledMServers.Add(Context.Guild.Id.ToString());
            await Context.Channel.SendMessageAsync("Memes for the server have been disabled.");
        }

        [Command("EnableMemes"), Summary("Enables Sombra bot's random memes.")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task EnableChat()
        {
            if (Save.DisabledMServers.Count != 0)
            {
                if (Save.DisabledMServers.Remove(Context.Guild.Id.ToString()))
                {
                    await Context.Channel.SendMessageAsync("Enabled random memes.");
                    return;
                }
            }
            await Error.Send(Context.Channel, Value: "You already have memes enabled.");
        }
    }
}
