using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using RiotSharp.Endpoints;
using RiotSharp.Endpoints.MatchEndpoint;
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
            googleId = Regex.Match(googleId, pattern).ToString().Substring(16);

            string errorMsg = "The database had an issue. The data was not saved.";
            await ReplyResultAsync(await DatabaseService.AddSheetAsync(googleId, name), errorMsg);
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
            string googleId = await SelectSpreadsheetAsync();

            if (googleId == null)
                return;

            string error = await GoogleService.AddNewScoutSheetAsync(googleId, opgg, team);
            if (error == null)
            {
                bool result = await DatabaseService.AddTeamAsync(team, googleId);
                await ReplyResultAsync(result, "There was a problem adding the team to the database. Please delete the google sheet that was generated and try again.");
                return;
            }
            await ReplyResultAsync(error == null, error);
        }

        [Command("ScoutGame", RunMode = RunMode.Async)]
        [Summary("Adds match history data to the enemy team's scout sheet.")]
        public async Task ScoutGameAsync(string result, string matchId)
        {
            char resultChar = ScoutModuleHelper.CheckResultInput(result);
            if (resultChar == Char.MinValue)
            {
                await ReplyAsync("Invalid result. Please type win or loss.");
                return;
            }

            string pattern = @"/NA1/([0-9]+)";
            matchId = Regex.Match(matchId, pattern).ToString().Substring(5);

            string googleId = await SelectSpreadsheetAsync();
            if (googleId == null)
                return;

            int teamId = await SelectTeamAsync(googleId);
            var match = await RiotService.GetMatchDetailAsync(Convert.ToInt64(matchId));
            bool success = await DatabaseService.AddMatchAsync(resultChar, teamId, match);
        }

        /// <summary>
        /// Prompts the user to select a google sheet from a list.
        /// </summary>
        /// <returns>The googleId of the selected sheet.</returns>
        private async Task<string> SelectSpreadsheetAsync()
        {
            SocketGuildUser user = (SocketGuildUser)Context.User;
            List<SheetAccess> spreadsheets = await DatabaseService.GetAccessibleSheetsAsync(ScoutModuleHelper.CreateListOfRoles(user));

            string googleId = null;
            string message = ScoutModuleHelper.SheetAccessResponse(spreadsheets, out googleId);
            if (message != null)
            {
                await ReplyAsync(message);
                if (googleId == null)
                    return null;

                SocketMessage selectionMsg = await NextMessageAsync();
                try
                {
                    // Gets the selected google spreadsheet id from the list
                    googleId = spreadsheets[Convert.ToInt32(selectionMsg.Content.Trim()) - 1].Sheet.GoogleId;
                }
                catch
                {
                    await ReplyAsync("Invalid selection.");
                    return null;
                }
            }

            return googleId;
        }

        /// <summary>
        /// Prompts the user to select a team from a list.
        /// </summary>
        /// <param name="googleId">The google Id of the sheet teams are on.</param>
        /// <returns>The teamId</returns>
        private async Task<int> SelectTeamAsync(string googleId)
        {
            List<Teams> teams = await DatabaseService.GetAllTeamsOnSheetAsync(googleId);

            int teamId;
            string message = ScoutModuleHelper.TeamsResponse(teams, out teamId);
            if (message != null)
            {
                await ReplyAsync(message);
                if (teamId == -1)
                    return -1;

                SocketMessage selectionMsg = await NextMessageAsync();
                try
                {
                    // Gets the selected team's SheetId
                    teamId = teams[Convert.ToInt32(selectionMsg.Content.Trim()) - 1].SheetId;
                }
                catch
                {
                    await ReplyAsync("Invalid selection.");
                    return -1;
                }
            }

            return teamId;
        }

        /// <summary>
        /// Responds to the user based on the result of the action.
        /// </summary>
        /// <param name="result">The success of the result.</param>
        /// <param name="errorMsg">Message if result was bad.</param>
        private async Task ReplyResultAsync(bool result, string errorMsg)
        {
            if (result)
                await ReplyAsync("Success!");
            else
                await ReplyAsync(errorMsg);
        }
    }
}
