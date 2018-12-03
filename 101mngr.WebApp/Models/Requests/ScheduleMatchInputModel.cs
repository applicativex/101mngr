using System;

namespace _101mngr.WebApp.Controllers
{
    public class ScheduleMatchInputModel
    {
        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public DateTime StartDate { get; set; }
    }
}