using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorManager: MonoBehaviour
{

    public GameObject prompt_Box;
    public GameObject playerObject_Box;
    public Transform doorExit_Location;

    bool port;

    void Start(){

        prompt_Box.SetActive(false);
    }

    void Update(){
        if (Input.GetKey(KeyCode.E) & port)
        {

            playerObject_Box.transform.position = doorExit_Location.transform.position;
        }
        else
        {
            port = false;
        }
    }
   

    void OnTriggerStay(Collider other)
    {

        if ((other.gameObject.tag == "Player"))
        {
            
            prompt_Box.SetActive(true);

            port = true;
        }
    }

    void OnTriggerExit()
    {
        prompt_Box.SetActive(false);
        port = false;
    }

}
   


