using _101mngr.Domain.Enums;

namespace _101mngr.Domain.Events
{
    public interface IMatchEvent
    {
        string Id { get; set; }  

        MatchPeriod MatchPeriod { get; set; }

        int Minute { get; set; }
    }

    public class GoalEvent : IMatchEvent
    {
        public string Id { get; set; }
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
        public bool Home { get; set; }
        public string PlayerId { get; set; }
    }

    public class YellowCardEvent : IMatchEvent
    {
        public string Id { get; set; }
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
        public bool Home { get; set; }
        public string PlayerId { get; set; }
    }

    public class RedCardEvent : IMatchEvent
    {
        public string Id { get; set; }
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
        public bool Home { get; set; }
        public string PlayerId { get; set; }
    }

    public class SubstitutionEvent : IMatchEvent
    {
        public string Id { get; set; }
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
        public bool Home { get; set; }
        public string PlayerId { get; set; }
        public string SubstitutionPlayerId { get; set; }
    }

    public class TimeEvent : IMatchEvent
    {
        public string Id { get; set; }
        public MatchPeriod MatchPeriod { get; set; }
        public int Minute { get; set; }
    }
}