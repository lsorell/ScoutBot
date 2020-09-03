using RiotSharp;
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
        /// Uses tournament stub api.
        /// </summary>
        private static bool _useStub = true;

        /// <summary>
        /// The tournament api instance.
        /// </summary>
        private static TournamentRiotApi _api = TournamentRiotApi.GetInstance(Config.RiotAPIKey, useStub: _useStub);

        /// <summary>
        /// The url for Riot to post completed games.
        /// </summary>
        private static string _resultUrl = "https://www.google.com/"; //TODO: Create endpoint for riot to hit

        /// <summary>
        /// Gets a provider for creating tournaments.
        /// </summary>
        /// <returns>The providerID.</returns>
        public static async Task<int> GetProviderAsync()
        {
            try
            {
                int provider = await _api.CreateProviderAsync(Region.Na, _resultUrl);
                return provider;
            }
            catch (RiotSharpException)
            {
                throw;
            }
        }
    }
}
