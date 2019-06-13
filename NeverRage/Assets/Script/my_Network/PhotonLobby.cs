using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;
   

    public GameObject battleButton;
    public GameObject cancleButton;

    public void Awake()
    {
        lobby = this;
    }

  
    public void OnBattleButtonClick()
    {
        battleButton.SetActive(false);
        cancleButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }
    public void OnCancelButtonClicked()
    {

        battleButton.SetActive(true);
        cancleButton.SetActive(false);
        PhotonNetwork.LeaveRoom();

        Debug.Log("Exited To Main Menu");

    }
    //

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    //
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        battleButton.SetActive(true);
        Debug.Log("Player connected");
    }

   

    void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Now In A Room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed To Connect");

        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed To Create Room, duplicate");
        CreateRoom();
    }

    


    void Update()
    {
        
    }
}
