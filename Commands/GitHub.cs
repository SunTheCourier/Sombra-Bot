﻿using Discord.Commands;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using Octokit;
using Sombra_Bot.Utils;
using System;
using Discord;

namespace Sombra_Bot.Commands
{
    public class GitHub : ModuleBase<SocketCommandContext>
    {
        [Command("GetRelease"), Summary("Downloads the latest release of a Github repository.")]
        public async Task GetRelease(params string[] args)
        {
            if (args.Length < 2)
            {
                await Error.Send(Context.Channel, "The input text has too few parameters.");
                return;
            }
            string user = null;
            string repo = null;
            string tag = null;
            bool getprerelease = false;

            foreach (KeyValuePair<char, string> pair in GetFlags(args))
            {
                switch (pair.Key)
                {
                    case 't':
                        tag = pair.Value;
                        break;
                    case 'p':
                        string flag = pair.Value.ToLower();
                        if ( flag == "false" || flag == null) getprerelease = false;
                        else if (flag == "true") getprerelease = true;
                        else await Error.Send(Context.Channel, Value: "Flag `p` is neither true or false.");
                        break;
                    default:
                        await Error.Send(Context.Channel, Value: $"Flag `{pair.Value}` does not exist!");
                        return;
                }
            }

            GitHubClient client = new GitHubClient(new ProductHeaderValue("Github"));
            IReadOnlyList<Release> releases;
            try
            {
                releases = await client.Repository.Release.GetAll(user, repo);
            }
            catch (ApiException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) await Error.Send(Context.Channel, Value: "Repository does not exist.");
                else await Error.Send(Context.Channel, Value: "Command failed: error reported!", e: e, et: Error.ExceptionType.Fatal);
                return;
            }


            foreach (Release release in releases)
            {
                await Context.Channel.SendMessageAsync(release.TagName);
                if (getprerelease)
                {
                    //if (release.Prerelease)
                }

            }
            await Context.Channel.SendMessageAsync($"{user}, {repo}");
        }

        private Dictionary<char, String> GetFlags(string[] args)
        {
            string k;

            Dictionary<char, string> argsDict = new Dictionary<char, string>();
            for (int i = 0; i < args.Length; i++)
            {
                
                //if (args.Length % 2 == 0)
                k = args[i];

                if (k.StartsWith('-'))
                {
                    string v;
                    v = args[i + 1];
                    if (!(i + 1 >= args.Length)) argsDict[k[1]] = v;
                    else argsDict[k[1]] = null;
                }
            }
            return argsDict;
        }
    }
}
