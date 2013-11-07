using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LobbyService.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService2" in both code and config file together.
    [ServiceContract]
    public interface IService2
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        void CreateLobby(DTO.Player host, int lobby);

        [OperationContract]
        List<DTO.PlayerLobby> GatAvailablePlayLobbies();

        [OperationContract]
        DTO.Player GetPlayer(string name);

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
    }
}
