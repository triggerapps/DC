﻿using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DelayStartroomController : MonoBehaviourPunCallbacks
{
    //scene navigation index
    [SerializeField]
    private int WaitingRoomSceneIndex;
    public override void OnEnable()
    {
        //register to photon callback functions
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        //unregister to photon callback functions
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public override void OnJoinedRoom() //Callback function for when we successfully create or join a room.
    {
        // called when our player joins the room
        // load into waiting room scene
        SceneManager.LoadScene(WaitingRoomSceneIndex);
    }
}