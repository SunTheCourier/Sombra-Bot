﻿using Discord.Commands;
using Sombra_Bot.Utils;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using System.Linq;

namespace Sombra_Bot.Commands
{
    public class Suggestions : ModuleBase<SocketCommandContext>
    {
        [Command("Suggest"), Summary("Suggest a feature."), Alias("AddSuggestion")]
        public async Task SaveSuggestion(params string[] suggestion)
        {
            string joined = string.Join(" ", suggestion);
            if (string.IsNullOrWhiteSpace(joined))
            {
                await Error.Send(Context.Channel, Key: "The input text has too few parameters.");
                return;
            }
            Save.Suggestions.Add(new KeyValuePair<ulong, string>(Context.User.Id, joined));
            await Context.Channel.SendMessageAsync("Thank you for your suggestion!");
        }

        [Command("ListSuggestions"), Summary("List saved suggestions."), Alias("Suggestions")]
        [RequireOwner]
        public async Task ListSuggestions()
        {
            if (Save.Suggestions.Count != 0)
            {
                string mod = "";
                int i = 1;
                foreach (KeyValuePair<ulong, string> pair in Save.Suggestions)
                {
                    mod += $"{i} - <@{pair.Key}>: {pair.Value}\n";
                    i++;
                }

                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Suggestions");
                builder.WithColor(Color.Purple);
                builder.WithDescription(mod);
                builder.WithCurrentTimestamp();
                await Context.Channel.SendMessageAsync(embed: builder.Build());
                return;
            }
            await Error.Send(Context.Channel, Value: "No suggestions have been made.");
        }

        [Command("RemoveSuggestion"), Summary("Remove a suggestion.")]
        [RequireOwner]
        public async Task RemoveSuggestion(int element)
        {
            if (Save.Suggestions.Count != 0)
            {
                if (element > Save.Suggestions.Count || element <= 0)
                {
                    await Error.Send(Context.Channel, Value: "Out of range of suggestions.");
                    return;
                }
                Save.Suggestions.RemoveAt(element - 1);
                await Context.Channel.SendMessageAsync("Suggestion removed!");
                return;
            }

            await Error.Send(Context.Channel, Value: "No suggestions have been made");
        }
    }
}
