using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Enums;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IPlayerGrain : IGrainWithIntegerKey
    {
        Task Create(string userName, string email);

        Task UpdateProfileInfo(string firstName, string lastName, DateTime dateOfBirth, string countryCode, PlayerType playerType, double height, double weight);

        Task<PlayerDto> GetPlayerInfo();

        Task StartTraining();

        Task<TrainingResultDto> GetCurrentTraining();

        Task<TrainingResultDto> TrainPassing();

        Task<TrainingResultDto> TrainEndurance();

        Task<TrainingResultDto> TrainDribbling();

        Task<TrainingResultDto> TrainCoverage();

        Task FinishTraining();
    }
}
    