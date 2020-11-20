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
    /// Helper class to reduce method length in ScoutModule.cs
    /// </summary>
    public static class ScoutModuleHelper
    {
        /// <summary>
        /// Builds a list of sheet names.
        /// </summary>
        /// <param name="sheets">The sheet names.</param>
        /// <returns>A list of sheet names.</returns>
        public static string ListSheets(List<string> sheets)
        {
            StringBuilder sb = new StringBuilder("What sheet do you want to give access rights to?\n");
            for (int i = 0; i < sheets.Count; i++)
            {
                sb.AppendLine(string.Format("{0}. {1}", i + 1, sheets[i]));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a prompt for the user to based on a selection.
        /// </summary>
        /// <param name="sheets">The sheet names.</param>
        /// <param name="message">User message.</param>
        /// <param name="selection">The sheet name of the selection.</param>
        /// <returns>A prompt string.</returns>
        public static string PromptForRoles(List<string> sheets, SocketMessage message, out string selection)
        {
            selection = null;
            if (message != null)
            {
                try
                {
                    selection = sheets[Convert.ToInt32(message.Content[0].ToString()) - 1];
                    return string.Format("List the roles you want to give access to sheet {0} seperated by spaces. (i.e. @Gold @Platinum @Diamond, etc...)", selection);
                }
                catch
                {
                    return "Invalid selection.";
                }
            }
            return "";
        }

        /// <summary>
        /// Seperates discord role numbers from the rest of the string.
        /// </summary>
        /// <param name="message">User message.</param>
        /// <returns>An array of role ids.</returns>
        public static ulong[] GetRoleIds(SocketMessage message)
        {
            ulong[] roleIds = null;
            if (message != null)
            {
                try
                {
                    string[] roles = message.Content.Split(' ');
                    roleIds = new ulong[roles.Length];
                    string pattern = @"\d+";
                    for (int i = 0; i < roles.Length; i++)
                    {
                        roleIds[i] = Convert.ToUInt64(Regex.Match(roles[i], pattern).ToString());
                    }
                }
                catch
                {
                    return null;
                }
            }
            return roleIds;
        }
    }
}