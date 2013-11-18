using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LobbyService.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        void AddPlayer(string playerName);

        [OperationContract]
        List<DTO.Player> GetPlayers();

        [OperationContract]
        DTO.Player GetPlayerById(int id);

        [OperationContract]
        DTO.Player GetPlayerByName(string name);

        [OperationContract]
        void CreateLobby(DTO.Player host);

        [OperationContract]
        List<DTO.PlayerLobby> GatAvailablePlayLobbies();

        [OperationContract]
        DTO.Player GetPlayer(String Name);

        [OperationContract]
        DTO.Player GetPlayerId(int id);

        [OperationContract]
        void DeleteAllPlayers();

        [OperationContract]
        void DeleteAllLobbies();

        [OperationContract]
        void DeletePlayer(DTO.Player pl);

        [OperationContract]
        void DeletePlayerLobby(DTO.PlayerLobby pl);

        [OperationContract]
        void JoinLobbyRoom(DTO.Player pl, DTO.Player Host);

        [OperationContract]
        List<DTO.Player> ShowPlayersInLobbyRoom(int host);

        [OperationContract]
        List<int> GetGameUpdate();

        [OperationContract]
        int CheckPlayerCount(string lobby);

        [OperationContract]
        void StartGame(DTO.Player Host);
    }
}
