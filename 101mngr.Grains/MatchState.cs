using System;
using System.Collections.Generic;
using System.Linq;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    public class MatchState
    {
        public MatchState()
        {
            Goals = new Score();
            YellowCards = new Score();
            RedCards = new Score();
            MatchEvents = new List<IMatchEvent>();
        }
        
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime StartTime { get; set; }

        public int Minute { get; set; }

        public MatchPeriod MatchPeriod { get; set; }

        public Score Goals { get; set; }

        public Score YellowCards { get; set; }

        public Score RedCards { get; set; }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }

        public List<PlayerDataDto> Players { get; set; }

        public List<IMatchEvent> MatchEvents { get; set; }

        public void Apply(GoalEvent @event)
        {
            Goals = @event.Home ? Goals.HomeIncrement() : Goals.AwayIncrement();
            MatchEvents.Add(@event);
        }

        public void Apply(YellowCardEvent @event)
        {
            YellowCards = @event.Home ? YellowCards.HomeIncrement() : YellowCards.AwayIncrement();
            MatchEvents.Add(@event);
        }

        public void Apply(RedCardEvent @event)
        {
            RedCards = @event.Home ? RedCards.HomeIncrement() : RedCards.AwayIncrement();
            MatchEvents.Add(@event);
        }

        public void Apply(SubstitutionEvent @event)
        {
            var playerId = @event.PlayerId;
            var substitutionPlayerId = @event.SubstitutionPlayerId;

            if (@event.Home)
            {
                var player = HomeTeam.Players.SingleOrDefault(x => x.Id == playerId);
                var substitutionPlayer = HomeTeam.Players.SingleOrDefault(x => x.Id == substitutionPlayerId);
                player.Bench = true;
                substitutionPlayer.Bench = false;
            }
            else
            {
                var player = HomeTeam.Players.SingleOrDefault(x => x.Id == playerId);
                var substitutionPlayer = HomeTeam.Players.SingleOrDefault(x => x.Id == substitutionPlayerId);
                player.Bench = true;
                substitutionPlayer.Bench = false;
            }

            MatchEvents.Add(@event);
        }

        public void Apply(TimeEvent @event)
        {
            Minute = @event.Minute;
            MatchPeriod = @event.MatchPeriod;
        }
    }

    public class GoalEvent : IMatchEvent
    {
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
        public bool Home { get; set; }
        public string PlayerId { get; set; }
    }

    public class YellowCardEvent : IMatchEvent
    {
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
        public bool Home { get; set; }
        public string PlayerId { get; set; }
    }

    public class RedCardEvent : IMatchEvent
    {
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
        public bool Home { get; set; }
        public string PlayerId { get; set; }
    }

    public class SubstitutionEvent : IMatchEvent
    {
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
        public bool Home { get; set; }
        public string PlayerId { get; set; }
        public string SubstitutionPlayerId { get; set; }
    }

    public class TimeEvent : IMatchEvent
    {
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
    }

    public interface IMatchEvent
    {
        MatchPeriod MatchPeriod { get; set; }

        int Minute { get; set; }
    }
}