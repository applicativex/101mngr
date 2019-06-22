using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using _101mngr.Contracts;

namespace _101mngr.WebApp.Hubs
{
    public class MatchRoomService
    {
        private readonly IClusterClient _clusterClient;

        private IHubContext<MatchRoomHub> Hub { get; }

        public MatchRoomService(IClusterClient clusterClient, IHubContext<MatchRoomHub> hub)
        {
            Hub = hub;
            _clusterClient = clusterClient;
        }

        public Task<MatchRoomDto> GetMatchRoom(string matchRoomId)
        {
            var matchRoomGrain = _clusterClient.GetGrain<IMatchRoomGrain>(matchRoomId);
            return matchRoomGrain.GetMatchRoom();
        }

        public Task<MatchRoomDto[]> GetMatchRooms()
        {
            var matchRoomRegistryGrain = _clusterClient.GetGrain<IMatchRoomRegistryGrain>(0);
            return matchRoomRegistryGrain.GetMatchRooms();
        }

        public async Task NewMatchRoom(string matchRoomId, long playerId, string playerName)
        {
            var matchRoomGrain = _clusterClient.GetGrain<IMatchRoomGrain>(matchRoomId);
            await matchRoomGrain.NewRoom(playerId, playerName);
            await Hub.Clients.All.SendAsync("MatchRoomAdded", matchRoomId);
        }

        public async Task StartMatch(string matchRoomId)
        {
            var matchRoomGrain = _clusterClient.GetGrain<IMatchRoomGrain>(matchRoomId);
            await matchRoomGrain.StartMatch();
            await Hub.Clients.All.SendAsync("MatchStarted", matchRoomId);
            await Hub.Clients.All.SendAsync("MatchRoomRemoved", matchRoomId);
        }   

        public async Task JoinRoom(string matchRoomId, long playerId, string playerName)
        {
            var matchRoomGrain = _clusterClient.GetGrain<IMatchRoomGrain>(matchRoomId);
            if (await matchRoomGrain.Join(playerId, playerName))
            {
                await Hub.Clients.All.SendAsync("PlayerJoinedRoom", matchRoomId, playerId, playerName);
            }
        }

        public async Task LeaveRoom(string matchRoomId, long playerId)
        {
            var matchRoomGrain = _clusterClient.GetGrain<IMatchRoomGrain>(matchRoomId);
            if (await matchRoomGrain.Leave(playerId))
            {
                await Hub.Clients.All.SendAsync("PlayerLeftRoom", matchRoomId, playerId);
            }
        }
    }
}