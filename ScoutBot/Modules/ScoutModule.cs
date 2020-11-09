using Discord;
using Discord.Commands;
using RiotSharp;
using RiotSharp.Misc;
using ScoutBot.Services;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScoutBot.Modules
{
    /// <summary>
    /// Commands dealing with scouting features can be found in this module.
    /// </summary>
    public class ScoutModule : ModuleBase<SocketCommandContext>
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
            if (await DatabaseService.AddSheet(googleId.Substring(new string("/spreadsheets/d/").Length), name))
                await ReplyAsync("Success!");
            else
                await ReplyAsync("There was an error. The data was not saved.");
        }
    }
}
