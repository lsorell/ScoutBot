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
            await ReplyResultAsync(await DatabaseService.AddSheetAsync(googleId.Substring(new string("/spreadsheets/d/").Length), name));
        }

        [RequireUserPermissionAttribute(GuildPermission.Administrator)]
        [Command("GiveAccess", RunMode = RunMode.Async)]
        [Summary("Process to give roles access to a google sheet.")]
        public async Task GiveAccessAsync()
        {
            List<string> sheets = await DatabaseService.GetSheetNamesAsync();
            await ReplyAsync(ScoutModuleHelper.ListSheets(sheets));

            SocketMessage selectionMsg = await NextMessageAsync();
            string selection = null;
            await ReplyAsync(ScoutModuleHelper.PromptForRoles(sheets, selectionMsg, out selection));
            if (selection == null)
                return;

            ulong[] roleIds = ScoutModuleHelper.GetRoleIds(await NextMessageAsync());
            if (roleIds == null)
            {
                await ReplyAsync("Roles not formatted correctly.");
                return;
            }

            await ReplyResultAsync(await DatabaseService.AddSheetAccessAsync(roleIds, selection));
        }

        private async Task ReplyResultAsync(bool result)
        {
            if (result)
                await ReplyAsync("Success!");
            else
                await ReplyAsync("There was an error. The data was not saved.");
        }
    }
}
