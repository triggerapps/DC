using System.Collections;
using System.Collections.Generic;
//
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class menu : MonoBehaviour {

//	public Player playerScript;
//	public GameObject playerObject;

    public GameObject Main_Menu_Box;
    public GameObject Credit_Menu_Box;
    public GameObject Options_Menu_Box;
    public GameObject LoadTeam_Credits_Box;


	//Resolutions
	public Toggle[] ScreenRezToggles;
	public int[] screenWidths;
	int activeScreenResIndex;

	// Use this for initialization
	void Awake()
	{
	//	if (playerObject != null) {
	//		playerObject.SetActive (false);
	//	}
	//	if (playerScript.enabled == true) 
	//	{
//		playerScript.enabled = false;
	}

	void Start ()
    {
        Load_MainMenu();
    }

    //
    bool optionMenu_isOn;
    bool aboutMenu_isOn;
    bool mainMenu_isOn;
    bool LoadTeam_Credits_isON;

    // Update is called once per frame
    void Update ()
    {

		if(optionMenu_isOn == true)
        {

            Main_Menu_Box.gameObject.SetActive(false);


            Credit_Menu_Box.gameObject.SetActive(false);


            Options_Menu_Box.gameObject.SetActive(true);

		
        }
        else
        {
            Options_Menu_Box.gameObject.SetActive(false);
        }

        if (aboutMenu_isOn == true)
        {

            Main_Menu_Box.gameObject.SetActive(false);


            Options_Menu_Box.gameObject.SetActive(false);


            Credit_Menu_Box.gameObject.SetActive(true);

            //
         
        }
        else
        {

           Credit_Menu_Box.gameObject.SetActive(false);
          //
         
        }

        if (LoadTeam_Credits_isON == true)
        {

            Options_Menu_Box.gameObject.SetActive(false);

            Main_Menu_Box.gameObject.SetActive(false);

            Credit_Menu_Box.gameObject.SetActive(false);


            LoadTeam_Credits_Box.gameObject.SetActive(true);

        }
        else
        {
            LoadTeam_Credits_Box.gameObject.SetActive(false);

        }


        if (mainMenu_isOn == true)
        { 

            Credit_Menu_Box.gameObject.SetActive(false);
    
            Options_Menu_Box.gameObject.SetActive(false);


            Main_Menu_Box.gameObject.SetActive(true);

        }
        else
        {
           Main_Menu_Box.gameObject.SetActive(false);
        }

	    }

    public void LoadIntro_Scene()
    {
        SceneManager.LoadScene ("LoadingScreen02");
    }

    public void Load_OptionMenu()
    {
        aboutMenu_isOn = false;
        mainMenu_isOn = false;
        optionMenu_isOn = true;
    }
    public void Load_CreditMenu()
    {
       
        mainMenu_isOn = false;
        optionMenu_isOn = false;
        aboutMenu_isOn = true;
        LoadTeam_Credits_isON = false;
    }
    public void LoadTeam_Credits()
    {

        mainMenu_isOn = false;
        optionMenu_isOn = false;
        aboutMenu_isOn = false;
        LoadTeam_Credits_isON = true;

    }
    public void Load_MainMenu()
    {
        aboutMenu_isOn = false;
        optionMenu_isOn = false;
        mainMenu_isOn = true;
    }
    public void QuitGame_Scene()
    {
        Application.Quit();
    }

	//Resolutions
	public void SetScreenRez (int i) 
	{
		if (ScreenRezToggles [i].isOn) {
			activeScreenResIndex = i;
			float aspectRatio = 16 / 9f;
			Screen.SetResolution (screenWidths [i], (int)(screenWidths [i] / aspectRatio), false);
			PlayerPrefs.SetInt ("screen res index", activeScreenResIndex);
			PlayerPrefs.Save ();
		}
	}
	public void SetFullscreen(bool isFullscreen) {
		for (int i = 0; i < ScreenRezToggles.Length; i++) {
			ScreenRezToggles[i].interactable = !isFullscreen;
		}

		if (isFullscreen) {
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions [allResolutions.Length - 1];
			Screen.SetResolution (maxResolution.width, maxResolution.height, true);
		} else {
			SetScreenRez (activeScreenResIndex);
		}

		PlayerPrefs.SetInt ("fullscreen", ((isFullscreen) ? 1 : 0));
		PlayerPrefs.Save ();
	}
}
