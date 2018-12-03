using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _101mngr.WebApp.Data;

namespace _101mngr.WebApp.Services
{
    public class MatchHistoryService
    {
        private readonly ApplicationDbContext _context;

        public MatchHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<PlayerMatchHistory>> GetPlayerHistory(long playerId)
        {
            return await _context.MatchHistory.Where(x => x.PlayerId == playerId).ToListAsync();
        }
    }
}