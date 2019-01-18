using System;
using System.Collections.Generic;
using System.Linq;

namespace _101mngr.WebApp.Domain
{
    public class PlayerService
    {
        private List<FootballPlayer> _playerQuery = new List<FootballPlayer>();
        // todo: pagination
        public FootballPlayer[] GetPlayers()
        {
            return _playerQuery.ToArray();
        }

        public FootballPlayer[] GetPlayers(int levelFrom, int levelTo)
        {
            return _playerQuery.Where(x => x.Level >= levelFrom && x.Level <= levelTo)
                .OrderBy(x => Guid.NewGuid())
                .Take(21).ToArray();
        }
    }
}