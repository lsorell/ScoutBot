using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using ScoutBot.Database.Model;
using ScoutBot.Services;
using System;
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
            await ReplyResultAsync(
                await DatabaseService.AddSheetAsync(googleId.Substring(new string("/spreadsheets/d/").Length), name),
                "The database had an issue. The data was not saved.");
        }

        [RequireUserPermissionAttribute(GuildPermission.Administrator)]
        [Command("GiveAccess", RunMode = RunMode.Async)]
        [Summary("Give roles access to a google sheet.")]
        public async Task GiveAccessAsync()
        {
            List<string> sheets = await DatabaseService.GetSheetNamesAsync();
            await ReplyAsync(ScoutModuleHelper.FormatListPrompt("What sheet do you want to give access rights to?", sheets));

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

            await ReplyResultAsync(
                await DatabaseService.AddSheetAccessAsync(roleIds, selection),
                "The database had an issue. The data was not saved.");
        }

        [Command("NewScout", RunMode = RunMode.Async)]
        [Summary("Adds a new scout sheet to the google spreadsheet.")]
        public async Task NewScoutAsync(string opgg, [Remainder] string team)
        {
            SocketGuildUser user = (SocketGuildUser)Context.User;
            List<SheetAccess> spreadsheets = await DatabaseService.GetAccessibleSheetsAsync(ScoutModuleHelper.CreateListOfRoles(user));

            string googleId = null;
            string message = ScoutModuleHelper.SheetAccessResponse(spreadsheets, out googleId);
            if (message != null)
            {
                await ReplyAsync(message);
                if (googleId == null)
                    return;

                SocketMessage selectionMsg = await NextMessageAsync();
                try
                {
                    // Gets the selected google spreadsheet id from the list
                    googleId = spreadsheets[Convert.ToInt32(selectionMsg.Content.Trim()) - 1].Sheet.GoogleId;
                }
                catch
                {
                    await ReplyAsync("Invalid selection.");
                }
            }

            string error = await GoogleService.AddNewScoutSheetAsync(googleId, opgg, team);
            await ReplyResultAsync(error == null, error);
        }

        private async Task ReplyResultAsync(bool result, string errorMsg)
        {
            if (result)
                await ReplyAsync("Success!");
            else
                await ReplyAsync(errorMsg);
        }
    }
}
