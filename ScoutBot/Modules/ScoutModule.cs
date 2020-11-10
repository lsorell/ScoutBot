using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using RiotSharp;
using RiotSharp.Misc;
using ScoutBot.Services;
using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScoutBot.Modules
{
    /// <summary>
    /// Commands dealing with scouting features can be found in this module.
    /// </summary>
    public class ScoutModule : InteractiveBase
    {
        [Command("echo")]
        [Summary("Echos back the message.")]
        public async Task EchoAsync([Remainder] string echo)
        {
            await ReplyAsync(echo);
        }

        [Command("prefix")]
        [Summary("Prints the command prefix.")]
        public async Task PrefixAsync()
        {
            await ReplyAsync(Config.CommandPrefix.ToString());
        }

        [RequireUserPermissionAttribute(GuildPermission.Administrator)]
        [Command("AddSheet")]
        [Summary("Adds a google sheet with a common name to the database.")]
        public async Task AddSheetAsync(string googleId, [Remainder] string name)
        {
            string pattern = @"/spreadsheets/d/([a-zA-Z0-9-_]+)";
            googleId = Regex.Match(googleId, pattern).ToString();
            if (await DatabaseService.AddSheetAsync(googleId.Substring(new string("/spreadsheets/d/").Length), name))
                await ReplyAsync("Success!");
            else
                await ReplyAsync("There was an error. The data was not saved.");
        }

        [RequireUserPermissionAttribute(GuildPermission.Administrator)]
        [Command("GiveAccess", RunMode = RunMode.Async)]
        [Summary("Process to give roles access to a google sheet.")]
        public async Task GiveAccessAsync()
        {
            List<string> sheets = await DatabaseService.GetSheetNamesAsync();
            StringBuilder sb = new StringBuilder("What sheet do you want to give access rights to?\n");
            for (int i = 0; i < sheets.Count; i++)
            {
                sb.AppendLine(string.Format("{0}. {1}", i + 1, sheets[i]));
            }
            await ReplyAsync(sb.ToString());
            SocketMessage selectionMsg = await NextMessageAsync();
            string selection = null;
            if (selectionMsg != null)
            {
                try
                {
                    selection = sheets[Convert.ToInt32(selectionMsg.Content[0].ToString()) - 1];
                    await ReplyAsync(string.Format("List the roles you want to give access to sheet {0} seperated by spaces. (i.e. @Gold @Platinum @Diamond, etc...)", selection));
                }
                catch
                {
                    await ReplyAsync("Invalid selection.");
                    return;
                }
            }
            SocketMessage rolesMsg = await NextMessageAsync();
            ulong[] roleIds = null;
            if (rolesMsg != null)
            {
                try
                {
                    string[] roles = rolesMsg.Content.Split(' ');
                    roleIds = new ulong[roles.Length];
                    string pattern = @"\d+";
                    for (int i = 0; i < roles.Length; i++)
                    {
                        roleIds[i] = Convert.ToUInt64(Regex.Match(roles[i], pattern).ToString());
                    }
                }
                catch
                {
                    await ReplyAsync("Roles not formatted correctly.");
                    return;
                }

            }
            if (await DatabaseService.AddSheetAccessAsync(roleIds, selection))
                await ReplyAsync("Success!");
            else
                await ReplyAsync("There was an error. The data was not saved.");
        }
    }
}
