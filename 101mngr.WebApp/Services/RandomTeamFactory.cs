using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using _101mngr.WebApp.Data;

namespace _101mngr.WebApp.Services
{
    public class RandomTeamFactory
    {
        private static readonly PlayerType[] _defaultFormation =
        {
            PlayerType.Goalkeeper,
            PlayerType.Defender, PlayerType.Defender, PlayerType.Defender, PlayerType.Defender,
            PlayerType.Midfielder, PlayerType.Midfielder, PlayerType.Midfielder, PlayerType.Midfielder,
            PlayerType.Forward, PlayerType.Forward
        };

        public RandomTeam Generate(Player player = null)
        {
            var rnd = new Random();
            var minLevel = player != null
                ? player.Level > 10
                    ? player.Level - 10
                    : 0
                : 0;
            var maxLevel = player?.Level + 10 ?? 10;
            var randomTeam = new Player[11];
            var playerPosition = player != null ? _defaultFormation.IndexOf(player.PlayerType) : (int?)null;
            for (int i = 0; i < _defaultFormation.Length; i++)
            {
                if (!playerPosition.HasValue || i != playerPosition)
                {
                    randomTeam[i] = new Player
                    {
                        PlayerType = _defaultFormation[i],
                        FirstName = "Random",
                        LastName = _defaultFormation[i].ToString(),
                        Level = rnd.Next(minLevel, maxLevel)
                    };
                }
                else
                {
                    randomTeam[i] = player;
                }
            }

            return new RandomTeam
            {
                Players = randomTeam,
                Captain = player ?? randomTeam.FirstOrDefault()
            };
        }
    }
}