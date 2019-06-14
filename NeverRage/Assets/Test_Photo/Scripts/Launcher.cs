
using UnityEngine;
using Photon.Pun;


namespace Com.TriggerAppsProduction.NeverRage
{

    public class Launcher : MonoBehaviour
    {
        /*Testing My Network Application */
        #region Private Serializabe Fields
        #endregion

        #region Private Field


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
            Connect();
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
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Connected");
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                Debug.Log("Dis_connected");
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion
    }
}
    