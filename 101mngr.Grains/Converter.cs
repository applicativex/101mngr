using System;
using System.Linq;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using _101mngr.Domain;
using _101mngr.Domain.Enums;
using _101mngr.Domain.Events;
using Team = _101mngr.Domain.Team;

namespace _101mngr.Grains
{
    public static class Converter
    {
        public static PlayerDto ToDto(this Player value)
        {
            return new PlayerDto
            {
                Id = long.Parse(value.Id),
                PlayerType = (int)value.PlayerType,
                BirthDate = value.DateOfBirth,
                CountryCode = value.CountryCode,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Height = value.Height,
                Weight = value.Weight,
                AcquiredSkills = value.AcquiredSkills.ToDto(),
            };
        }

        public static MatchDto ToDto(this Match value)
        {
            return new MatchDto
            {
                Id = value.Id,
                Name = value.Name,
                StartTime = value.StartTime,
                Minute = value.Minute,
                MatchPeriod = (int)value.MatchPeriod,
                Goals = value.Goals.ToDto(),
                YellowCards = value.YellowCards.ToDto(),
                RedCards = value.RedCards.ToDto(),
                HomeTeam = value.HomeTeam.ToDto(),
                AwayTeam = value.AwayTeam.ToDto(),
                MatchEvents = value.MatchEvents
                    .Select(x => x.ToDto())
                    .OrderByDescending(x => x.MatchPeriod)
                    .ThenByDescending(x => x.Minute)
                    .ToList()
            };
        }

        public static TrainingDto ToDto(this Training value)
        {
            return new TrainingDto
            {
                CoverageDelta = value.CoverageDelta,
                DribblingDelta = value.DribblingDelta,
                EnduranceDelta = value.EnduranceDelta,
                HittingAccuracyDelta = value.HittingAccuracyDelta,
                HittingPowerDelta = value.HittingPowerDelta,
                PassingDelta = value.PassingDelta,
                ReceivingDelta = value.ReceivingDelta,
                TackleDelta = value.TackleDelta
            };
        }

        public static GoalEvent ToGoalEvent(this MatchEventDto value)
        {
            return new GoalEvent
            {
                Id = value.Id,
                MatchPeriod = (MatchPeriod)value.MatchPeriod,
                Minute = value.Minute,
                Home = value.Home.Value,
                PlayerId = value.PlayerId
            };
        }

        public static YellowCardEvent ToYellowCardEvent(this MatchEventDto value)
        {
            return new YellowCardEvent
            {
                Id = value.Id,
                MatchPeriod = (MatchPeriod)value.MatchPeriod,
                Minute = value.Minute,
                Home = value.Home.Value,
                PlayerId = value.PlayerId
            };
        }

        public static RedCardEvent ToRedCardEvent(this MatchEventDto value)
        {
            return new RedCardEvent
            {
                Id = value.Id,
                MatchPeriod = (MatchPeriod)value.MatchPeriod,
                Minute = value.Minute,
                Home = value.Home.Value,
                PlayerId = value.PlayerId
            };
        }

        public static SubstitutionEvent ToSubstitutionEvent(this MatchEventDto value)
        {
            return new SubstitutionEvent
            {
                Id = value.Id,
                MatchPeriod = (MatchPeriod)value.MatchPeriod,
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
                Id = value.Id,
                MatchPeriod = (MatchPeriod)value.MatchPeriod,
                Minute = value.Minute
            };
        }

        private static AcquiredSkillsDto ToDto(this Domain.AcquiredSkills value)
        {
            return new AcquiredSkillsDto
            {
                Dribbling = value.Dribbling,
                Endurance = value.Endurance,
                Coverage = value.Coverage,
                Tackle = value.Tackle,
                Passing = value.Passing,
                HittingAccuracy = value.HittingAccuracy,
                HittingPower = value.HittingPower,
                Receiving = value.Receiving
            };
        }

        private static ScoreDto ToDto(this Score value)
        {
            return new ScoreDto
            {
                Home = value.Home,
                Away = value.Away
            };
        }

        private static MatchPlayerDto ToDto(this MatchPlayer value)
        {
            return new MatchPlayerDto
            {
                Id = value.Id,
                Name = value.Name,
                Bench = value.Bench,
                PlayerType = (int)value.PlayerType
            };
        }

        private static TeamDto ToDto(this Team value)
        {
            return new TeamDto
            {
                Id = value.Id,
                Name = value.Name,
                Players = value.Players.Select(x => ToDto((MatchPlayer)x)).ToList()
            };
        }

        private static MatchEventDto ToDto(this IMatchEvent @event)
        {
            switch (@event)
            {
                case GoalEvent goalEvent:
                    return new MatchEventDto
                    {
                        Id = goalEvent.Id,
                        MatchEventType = (int)MatchEventType.Goal,
                        MatchPeriod = (int)goalEvent.MatchPeriod,
                        Minute = goalEvent.Minute,
                        Home = goalEvent.Home,
                        PlayerId = goalEvent.PlayerId
                    };
                case RedCardEvent redCardEvent:
                    return new MatchEventDto
                    {
                        Id = @event.Id,
                        MatchEventType = (int)MatchEventType.RedCard,
                        MatchPeriod = (int)redCardEvent.MatchPeriod,
                        Minute = redCardEvent.Minute,
                        Home = redCardEvent.Home,
                        PlayerId = redCardEvent.PlayerId
                    };
                case SubstitutionEvent substitutionEvent:
                    return new MatchEventDto()
                    {
                        Id = @event.Id,
                        MatchEventType = (int)MatchEventType.Substitution,
                        MatchPeriod = (int)substitutionEvent.MatchPeriod,
                        Minute = substitutionEvent.Minute,
                        Home = substitutionEvent.Home,
                        PlayerId = substitutionEvent.PlayerId,
                        SubstitutionPlayerId = substitutionEvent.SubstitutionPlayerId
                    };
                case TimeEvent timeEvent:
                    return new MatchEventDto
                    {
                        Id = @event.Id,
                        MatchEventType = (int)MatchEventType.Time,
                        Minute = timeEvent.Minute,
                        MatchPeriod = (int)timeEvent.MatchPeriod
                    };
                case YellowCardEvent yellowCardEvent:
                    return new MatchEventDto
                    {
                        Id = @event.Id,
                        MatchEventType = (int)MatchEventType.YellowCard,
                        MatchPeriod = (int)yellowCardEvent.MatchPeriod,
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