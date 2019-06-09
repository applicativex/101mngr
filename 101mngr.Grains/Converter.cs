using System;
using System.Linq;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    public static class Converter
    {
        public static ScoreDto ToDto(this Score value)
        {
            return new ScoreDto
            {
                Home = value.Home,
                Away = value.Away
            };
        }

        public static MatchPlayerDto ToDto(this MatchPlayer value)
        {
            return new MatchPlayerDto
            {
                Id = value.Id,
                Name = value.Name,
                Bench = value.Bench,
                PlayerType = value.PlayerType
            };
        }

        public static TeamDto ToDto(this Team value)
        {
            return new TeamDto
            {
                Id = value.Id,
                Name = value.Name,
                Players = value.Players.Select(x => ToDto((MatchPlayer) x)).ToList()
            };
        }

        public static GoalEvent ToGoalEvent(this MatchEventDto value)
        {
            return new GoalEvent
            {
                MatchPeriod = value.MatchPeriod,
                Minute = value.Minute,
                Home = value.Home.Value,
                PlayerId = value.PlayerId
            };
        }

        public static YellowCardEvent ToYellowCardEvent(this MatchEventDto value)
        {
            return new YellowCardEvent
            {
                MatchPeriod = value.MatchPeriod,
                Minute = value.Minute,
                Home = value.Home.Value,
                PlayerId = value.PlayerId
            };
        }

        public static RedCardEvent ToRedCardEvent(this MatchEventDto value)
        {
            return new RedCardEvent
            {
                MatchPeriod = value.MatchPeriod,
                Minute = value.Minute,
                Home = value.Home.Value,
                PlayerId = value.PlayerId
            };
        }

        public static SubstitutionEvent ToSubstitutionEvent(this MatchEventDto value)
        {
            return new SubstitutionEvent
            {
                MatchPeriod = value.MatchPeriod,
                Minute = value.Minute,
                Home = value.Home.Value,
                PlayerId = value.PlayerId,
                SubstitutionPlayerId = value.SubstitutionPlayerId
            };
        }

        public static TimeEvent ToTimeEvent(this MatchEventDto value)
        {
            return new TimeEvent
            {
                MatchPeriod = value.MatchPeriod,
                Minute = value.Minute
            };
        }

        public static MatchEventDto ToDto(this IMatchEvent @event)
        {
            switch (@event)
            {
                case GoalEvent goalEvent:
                    return new MatchEventDto
                    {
                        MatchEventType = MatchEventType.Goal,
                        MatchPeriod = goalEvent.MatchPeriod,
                        Minute = goalEvent.Minute,
                        Home = goalEvent.Home,
                        PlayerId = goalEvent.PlayerId
                    };
                case RedCardEvent redCardEvent:
                    return new MatchEventDto
                    {
                        MatchEventType = MatchEventType.RedCard,
                        MatchPeriod = redCardEvent.MatchPeriod,
                        Minute = redCardEvent.Minute,
                        Home = redCardEvent.Home,
                        PlayerId = redCardEvent.PlayerId
                    };
                case SubstitutionEvent substitutionEvent:
                    return new MatchEventDto()
                    {
                        MatchEventType = MatchEventType.Substitution,
                        MatchPeriod = substitutionEvent.MatchPeriod,
                        Minute = substitutionEvent.Minute,
                        Home = substitutionEvent.Home,
                        PlayerId = substitutionEvent.PlayerId,
                        SubstitutionPlayerId = substitutionEvent.SubstitutionPlayerId
                    };
                case TimeEvent timeEvent:
                    return new MatchEventDto
                    {
                        MatchEventType = MatchEventType.Time,
                        Minute = timeEvent.Minute,
                        MatchPeriod = timeEvent.MatchPeriod
                    };
                case YellowCardEvent yellowCardEvent:
                    return new MatchEventDto
                    {
                        MatchEventType = MatchEventType.YellowCard,
                        MatchPeriod = yellowCardEvent.MatchPeriod,
                        Minute = yellowCardEvent.Minute,
                        Home = yellowCardEvent.Home,
                        PlayerId = yellowCardEvent.PlayerId
                    };
                default:
                    throw new NotSupportedException();
            }
        }
    }
}