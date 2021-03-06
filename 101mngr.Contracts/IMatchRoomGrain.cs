﻿using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IMatchRoomGrain : IGrainWithStringKey
    {
        Task<MatchRoomDto> GetMatchRoom();

        Task NewRoom(long playerId, string playerName);

        Task<bool> Join(long playerId, string playerName);

        Task<bool> Leave(long playerId); 

        Task StartMatch();
    }
}