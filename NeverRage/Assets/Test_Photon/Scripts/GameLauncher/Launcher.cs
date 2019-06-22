﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Com.TriggerApps.NeverRage
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [Tooltip("The maximum number of players per room. When a room is full,new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        

        #endregion


        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";
        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;

        #endregion

        #region Public Fields
        //Ui username field
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        //loading text
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        #endregion

        #region MonoBehaviour CallBacks



        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        void Start()
        {
            //buttons Activity
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion


        #region Public Methods


        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            //buttons Activity
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // keep track of the will to join a room, because when we come back from the game we will get
            //a callback that we are connected, so we need to know what to do then
            isConnecting = true;

            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }


        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        /// <summary>
        /// Call backs are functions that gets data on thier own, from photon
        /// </summary>


        public override void OnConnectedToMaster()
        {
            Debug.Log("ConnectToMaster() Was Called from Launcher Script");

            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
            }
            
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

           // #Critical: We can Set This value in the Inspector; how many player do we need (max) amount, before sending players to other room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });

        }

        public override void OnJoinedRoom()
        {
            // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");


                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("Room for 1");
            }
            Debug.Log("Now this client is in a room.");
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }


        #endregion


    }
}