using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class laodingSliderController : MonoBehaviour {
    [SerializeField] private string LevelOne_Loader;

    [SerializeField] private Slider SceneLoadingSlider;

    public Text waitingText;
    float count;


	public Image splashImage;


    void Start()
    {
        count = 0f;
        SetWaitingText();
    }

    void Update()
    {
        SceneLoadingSlider.value += Time.deltaTime * 50;

        if (SceneLoadingSlider.value >= 100)
        {
            SceneManager.LoadScene("Intro_");
        }

        if(SceneLoadingSlider.value >= 35)
        {
            waitingText.text = "."+".";

            SetWaitingText();
        }
        if(SceneLoadingSlider.value >= 50)
        {
            waitingText.text = "."+ "." + ".";
            SetWaitingText();
        }
        if(SceneLoadingSlider.value >= 60)
        {
            waitingText.text = ".";
            SetWaitingText();
        }
        if(SceneLoadingSlider.value >= 70)
        {
            waitingText.text = "."+ "." ;
            SetWaitingText();
        }
        if(SceneLoadingSlider.value >= 80)
        {
            waitingText.text = "...";
            SetWaitingText();
        }
      
    }





        void SetWaitingText()
        {
            if (SceneLoadingSlider.value <= 20)
            {
                waitingText.text = ".";
            }
        }

    }

