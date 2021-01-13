using Discord.WebSocket;
using ScoutBot.Database.Model;
using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScoutBot.Modules
{
    /// <summary>
    /// Helper class to reduce method length in ScoutModule.cs
    /// </summary>
    public static class ScoutModuleHelper
    {
        /// <summary>
        /// Formats a string to list objects with a number after a prompt.
        /// Example:
        ///     What color is your favorite?
        ///     1. Red
        ///     2. Orange
        ///     3. Blue
        /// </summary>
        /// <param name="prompt">The question to ask.</param>
        /// <param name="list">The list of options.</param>
        /// <returns></returns>
        public static string FormatListPrompt(string prompt, List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(prompt);
            for (int i = 0; i < list.Count; i++)
            {
                sb.AppendLine(string.Format("{0}. {1}", i + 1, list[i]));
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
                    selection = sheets[Convert.ToInt32(message.Content.Trim()) - 1];
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

        /// <summary>
        /// Makes a list based on user roles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A list of roles.</returns>
        public static List<ulong> CreateListOfRoles(SocketGuildUser user)
        {
            List<ulong> roles = new List<ulong>();
            foreach (SocketRole role in user.Roles)
            {
                roles.Add(role.Id);
            }
            return roles;
        }

        /// <summary>
        /// Creates a response based on the spreadsheets the user has access to.
        /// </summary>
        /// <param name="spreadsheets">The spreadsheets the user has access to.</param>
        /// <param name="googleId">The google id of the spreadsheet.</param>
        /// <returns></returns>
        public static string SheetAccessResponse(List<SheetAccess> spreadsheets, out string googleId)
        {
            googleId = null;
            if (spreadsheets.Count == 0)
            {
                return "You don't have access to any sheets. Ask an admin to give your role access.";
            }
            else if (spreadsheets.Count > 1)
            {
                List<string> names = new List<string>();
                foreach (SheetAccess sheet in spreadsheets)
                {
                    names.Add(sheet.Sheet.Name);
                }
                googleId = "";
                return FormatListPrompt("What file do you want to add this to?", names);
            }
            else
            {
                googleId = spreadsheets[0].Sheet.GoogleId;
                return null;
            }
        }
    }
}