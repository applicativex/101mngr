using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IPlayerGrain : IGrainWithIntegerKey
    {
        Task<long> GetPlayer();

        Task Create(CreatePlayerDto request);

        Task ProfileInfo(ProfileInfoDto dto);

        Task<PlayerDto> GetPlayerInfo();

        Task<string> NewMatch(string matchName);

        Task JoinMatch(string matchId);

        Task LeaveMatch(string matchId);

        Task<MatchDto[]> GetMatchHistory();

        Task AddMatchHistory(MatchDto match);

        Task<string> RandomMatch();

        Task StartTraining();

        Task<TrainingResultDto> GetCurrentTraining();

        Task FinishTraining();

        Task<TrainingResultDto> TrainPassing();

        Task<TrainingResultDto> TrainEndurance();

        Task<TrainingResultDto> TrainDribbling();

        Task<TrainingResultDto> TrainCoverage();
    }
}
