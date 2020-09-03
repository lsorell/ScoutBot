using Discord.Commands;
using RiotSharp;
using RiotSharp.Misc;
using ScoutBot.Services;
using System.Threading.Tasks;

namespace ScoutBot.Modules
{
    /// <summary>
    /// Commands dealing with riot api can be found in this module.
    /// </summary>
    public class RiotModule : ModuleBase<SocketCommandContext>
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
    }
}
