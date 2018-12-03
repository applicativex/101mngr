using System.Threading.Tasks;
using _101mngr.WebApp.Data;

namespace _101mngr.WebApp.Services
{
    public class RandomMatchService
    {
        private readonly RandomTeamFactory _randomTeamFactory;

        public RandomMatchService(RandomTeamFactory randomTeamFactory)
        {
            _randomTeamFactory = randomTeamFactory;
        }

        public void ScheduleMatch(RandomTeamFactory randomTeamFactory)
        {

        }

        public async Task GetMatch(Player player)
        {
            var team1 = _randomTeamFactory.Generate(player);
            var team2 = _randomTeamFactory.Generate();
        }
    }

    // New Match ->  Random team generation -> Captains pick -> Players pick -> Match start

        // New Match (Real players can join match)
        // Random team generation (Team completed with random players)
        // Captains pick (Captains picked from real players randomly)
        // Chat (It is possible to chat with players during pick)
        // Players pick (Player can pick all players or choose random pick)
}