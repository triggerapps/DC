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
    #region GameManager
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Object Reference Field


        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

     

        #region gameManager take damage instance initation
        //instance
        public static GameManager Instance;
        //
        #endregion


        #endregion

        #region Start Metods 
        void Start()
        {
           #region gamemanager taking dmg instance
            //instance
            Instance = this;
            #endregion



            #region Photon Pun: Instantiating Player 

            //If the PLAYER GameObject is not Available


            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }

            if (playerPrefab = null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
                #region Photon Pun: Instantiate Player if PlayerManager Don't Have Access to that Version of the Player

                //Else, Don't instantiate that clone of the player
                #endregion
            }
       
    }

        #endregion
        //
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
    #endregion
}