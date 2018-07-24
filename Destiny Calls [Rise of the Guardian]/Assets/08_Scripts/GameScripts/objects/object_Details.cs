using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class object_Details : MonoBehaviour {


    public Text infoText_Box;

    private void Start()
    {
       
        infoText_Box.text = "";
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        infoText_Box.text = "Tv Remote!";
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
     
        infoText_Box.text = "";
    }


}
