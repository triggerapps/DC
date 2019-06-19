
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;


namespace Com.TriggerAppsProduction.NeverRage
{

    public class Launcher : MonoBehaviourPunCallbacks
    {
        bool isConnecting;

        /*Testing My Network Application */
        #region Private Serializabe Fields

        [Tooltip("The maximum number of players per room. When a room is full, " +
            "it can't be joined by new players," +
            " and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion

        #region Private Field

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        /// <summary>
        /// this client's version number. Users are seperated from each other by gameversion/patches 
        /// </summary>

        /* Stages:  pre-alpha, beta, Alpha */

        string gameVersion = "1";

        #endregion
        #region MonoBehaviour call Backs

       
        void Awake()
        {
            // #Critical
            /*
             * Our game will have a resizable arena based on the number of players,
             * and to make sure that the loaded scene is the same for every connected player,
             * we'll make use of the very convenient feature provided by Photon: PhotonNetwork.AutomaticallySyncScene When this is true, 
             * the MasterClient can call PhotonNetwork.LoadLevel() and all connected players will automatically load that same level.
             */

            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            #region Progress Button Enactive
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            #endregion
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        /// 
        public void Connect()
        {
            isConnecting = true;

            /*
             * Buttons
             */
            #region ProgressButton Active
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            #endregion
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {

                Debug.Log("Connected");
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();

                /* Callback for room Not_connected
                */
            }
            else
            {
                Debug.Log("Dis_connected");
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
         
        }
        #region MonoBehaviorPunCallbacks Callbacks
        /*
         * inform dev and client about networking to Master... connection status
         */
        public override void OnConnectedToMaster()
        {
            Debug.Log("Photon 2.0 : OnConnectedToMaster() was called");

            /*
             * When the game start - we run the Connect(); after connecting to the master-this override function OnConnectedToMaster();
             *Will auto run, and in there We added the JoinRandomRoom(); and if this joinrandomroom fails, another override function
             * OnJoinedRandomFailed(); which is below this section, will run automatically
             */
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
            }
            
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            #region Progress Button Enactive
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            #endregion

            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);

        }

        #endregion

        /* FindingRoom
         * the script below will auto run, if the MasterServer function fails to execute PhotonNetwork.JoinRandomRoom();
         */
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was " +
                "called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
     
        }

        /*
         * the function below will execute after photon created a new room and join us in
         */
        public override void OnJoinedRoom()
        {

            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");


                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("Room for 1");
            }

        }
        #endregion
        #region Quit Game
        public void QuitRoom()
        {
            Application.Quit();
        }
        #endregion
    }
}
    