using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    public Transform player_Box;
    public Transform PauseGame_canvas_Box;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }


	}
    public void QuitApplication()
    {
       
            Application.Quit();
       
    }
    public void Pause()
    {
        if (PauseGame_canvas_Box.gameObject.activeInHierarchy == false)
        {
            PauseGame_canvas_Box.gameObject.SetActive(true);
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            PauseGame_canvas_Box.gameObject.SetActive(false);
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

