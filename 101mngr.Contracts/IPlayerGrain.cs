using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IPlayerGrain : IGrainWithIntegerKey
    {
        Task Create(string userName, string email);

        Task UpdateProfileInfo(string firstName, string lastName, DateTime dateOfBirth, string countryCode, int playerType, double height, double weight);

        Task<PlayerDto> GetPlayerInfo();

        Task StartTraining();

        Task<TrainingDto> GetCurrentTraining();

        Task<TrainingDto> TrainPassing();

        Task<TrainingDto> TrainEndurance();

        Task<TrainingDto> TrainDribbling();

        Task<TrainingDto> TrainCoverage();

        Task FinishTraining();
    }
}
    