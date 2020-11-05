﻿using Discord;
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
        [Command("Register")]
        [Summary("Links a google sheet with a discord role.")]
        public async Task RegisterSheetAsync(string sheetId, string roleId, [Remainder] string teamName)
        {
            string pattern = @"\d+"; //Get all digits from role
            roleId = Regex.Match(roleId, pattern).ToString();
            if (await DatabaseService.AddSheetAccess(sheetId, Convert.ToUInt64(roleId), teamName))
                await ReplyAsync("Success!");
            else
                await ReplyAsync("There was an error. The data was not saved.");
        }
    }
}