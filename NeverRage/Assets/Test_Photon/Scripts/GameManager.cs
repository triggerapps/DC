using System;
using System.Collections;

#region List Libraries
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
#endregion

namespace Com.TriggerAppsProduction.NeverRage
{//the above is a container for this script, the domain that own it
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region public Object Reference
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
 #endregion

        #region Start Metods 
        void Start()
        {
            if (playerPrefab = null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate;
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }

        }
#endregion


        #region Photon Callbacks


            /// <summary>
            /// Called when the local player left the room. We need to load the launcher scene.
            /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion

        #region Private Methods

        #region Photon Callbacks
        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        #endregion

        #endregion


        /// <summary>
        /// Below you can load level based on how many players are available
        /// </summary>
        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }


        #endregion

        #region Public Methods  Room 


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
      
          public void QuitRoom()
        {
            Application.Quit();
        }
            
        
    


    #endregion
}
}