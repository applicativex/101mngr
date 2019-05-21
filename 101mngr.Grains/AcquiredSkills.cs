using System;
using System.Collections.Generic;

namespace _101mngr.Grains
{
    public class AcquiredSkills
    {
        public int Tackle { get; set; }

        public int Coverage { get; set; }

        public int Dribbling { get; set; }

        public int Receiving { get; set; }
            
        public int Passing { get; set; }    

        public int Endurance { get; set; }

        public int HittingPower { get; set; }

        public int HittingAccuracy { get; set; }

        public void Apply(PlayerAcquiredSkillsChanged acquiredSkillsChanged)
        {
            Tackle += acquiredSkillsChanged.TackleDelta;
            Coverage += acquiredSkillsChanged.CoverageDelta;
            Dribbling += acquiredSkillsChanged.DribblingDelta;
            Receiving += acquiredSkillsChanged.ReceivingDelta;
            Passing += acquiredSkillsChanged.PassingDelta;
            Endurance += acquiredSkillsChanged.EnduranceDelta;
            HittingPower += acquiredSkillsChanged.HittingPowerDelta;
            HittingAccuracy += acquiredSkillsChanged.HittingAccuracyDelta;
        }
    }

    public class PlayerTest
    {
        public static void Test()
        {
            //tackle
            //    coverage
            //dribbling
            //    receiving
            //endurance
            //    passing
            //hitting power
            //hitting accuracy


        }
    }
}