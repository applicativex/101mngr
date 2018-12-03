using System.Threading.Tasks;

namespace _101mngr.WebApp.Data
{
    public interface IPlayerRepository
    {
        Task AddPlayer(Player player);
    }

    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _context;

        public PlayerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPlayer(Player player)
        {
            await _context.AddAsync(player);
            await _context.SaveChangesAsync();
        }
    }
}