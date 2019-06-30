using System;
using _101mngr.Domain.Enums;

namespace _101mngr.Domain
{
    public class MatchClock
    {
        private readonly Random _random = new Random();
        private MatchPeriod _matchPeriod;
        private int _minute;

        private int _addedTime;
        private int _halfTimeBreak;

        public MatchPeriod MatchPeriod => _matchPeriod;

        public int Minute => _minute;

        public void Tick()
        {
            if (MatchPeriod == MatchPeriod.None)
            {
                _minute = 0;
                _matchPeriod = MatchPeriod.FirstTime;
            }
            else if (MatchPeriod == MatchPeriod.FirstTime)
            {
                if (Minute < 45)
                {
                    _minute = Minute + 1;
                }
                else if (Minute == 45)
                {
                    _addedTime = _random.Next(1, 5);

                    _minute = Minute + 1;
                    _addedTime--;
                }
                else
                {
                    if (_addedTime > 0)
                    {
                        _minute = Minute + 1;
                        _addedTime--;
                    }

                    if (_addedTime == 0)
                    {
                        _minute = 45;
                        _matchPeriod = MatchPeriod.HalfTime;
                        _halfTimeBreak = 15;
                    }
                }
            }
            else if (MatchPeriod == MatchPeriod.HalfTime)
            {
                if (_halfTimeBreak > 0)
                {
                    _halfTimeBreak--;
                }

                if (_halfTimeBreak == 0)
                {
                    _matchPeriod = MatchPeriod.SecondTime;
                }

            }
            else if (MatchPeriod == MatchPeriod.SecondTime)
            {
                if (Minute < 90)
                {
                    _minute = Minute + 1;
                }
                else if (Minute == 90)
                {
                    _addedTime = _random.Next(1, 5);

                    _minute = Minute + 1;
                    _addedTime--;
                }
                else
                {
                    if (_addedTime > 0)
                    {
                        _minute = Minute + 1;
                        _addedTime--;
                    }

                    if (_addedTime == 0)
                    {
                        _minute = 90;
                        _matchPeriod = MatchPeriod.FullTime;
                    }
                }
            }
        }
    }
}