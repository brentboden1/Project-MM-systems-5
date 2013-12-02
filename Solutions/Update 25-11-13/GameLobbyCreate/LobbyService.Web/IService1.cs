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
        #region Player()

        [OperationContract]
        void AddPlayer(string playerName);

        [OperationContract]
        List<DTO.Player> GetPlayers();

        [OperationContract]
        DTO.Player GetPlayerById(int id);

        [OperationContract]
        DTO.Player GetPlayerByName(string name);

        [OperationContract]
        DTO.Player GetPlayer(String Name);

        [OperationContract]
        DTO.Player GetPlayerId(int id);

        #endregion

        #region Delete()

        [OperationContract]
        void DeleteAllPlayers();

        [OperationContract]
        void DeleteAllLobbies();

        [OperationContract]
        void DeletePlayer(DTO.Player pl);

        [OperationContract]
        void DeletePlayerLobby(DTO.PlayerLobby pl);

        #endregion

        #region Lobby()

        [OperationContract]
        void CreateLobby(DTO.Player host);

        [OperationContract]
        List<DTO.PlayerLobby> GatAvailablePlayLobbies();

        #endregion

        #region Update()

        [OperationContract]
        List<int> GetGameUpdate(DTO.Player host);

        [OperationContract]
        int CheckPlayerCount(string lobby);

        [OperationContract]
        int GetUpdate(DTO.Player host);

        [OperationContract]
        int GetPlayerLocation(DTO.Player Host, DTO.Player Player);

        #endregion

        #region Join()

        [OperationContract]
        void JoinLobbyRoom(DTO.Player pl, DTO.Player Host);

        [OperationContract]
        List<DTO.Player> ShowPlayersInLobbyRoom(int host);

        #endregion

        #region Gamestart()

        [OperationContract]
        void StartGame(DTO.Player Host);

        #endregion

        #region GameFunction()

        [OperationContract]
        void BuyTile();

        #endregion

        [OperationContract]
        void DoWork();
    }
}
