using RiotSharp;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Misc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Runtime;

namespace ScoutBot.Services
{
    public class RiotService
    {
        /// <summary>
        /// The tournament api instance.
        /// </summary>
        private static RiotApi _api = RiotApi.GetDevelopmentInstance(Config.RiotAPIKey);

        public static async Task<Match> GetMatchDetailAsync(long matchId)
        {
            return await _api.Match.GetMatchAsync(Region.Na, matchId);
        }
    }
}
