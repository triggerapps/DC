using System.Collections;
using System.Collections.Generic;
//
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//
using UnityEngine;

public class LoadNewScene_Scene1 : MonoBehaviour {
	

		/*
	 * this script loads the next scene, the transition is uncontrolable for now.
	 * this script also fades the image of our logo
	 * */

		public Image splashImage;
		//public Image splashImage2;
		//public Image splashImage3;

		public string loadLevel;

	    IEnumerator LOAD()
		{
			splashImage.canvasRenderer.SetAlpha(0.0f);
			//splashImage2.canvasRenderer.SetAlpha(0.0f);
			//splashImage3.canvasRenderer.SetAlpha(0.0f);


			fadeIn();
			yield return new WaitForSeconds(3.0f);
			fadeOut();
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene(loadLevel);
		}

		void fadeIn()
		{
			splashImage.CrossFadeAlpha(1.0f, 1.5f, false);	
			//splashImage2.CrossFadeAlpha(1.0f, 1.5f, false);
			//splashImage3.CrossFadeAlpha(1.0f, 1.5f, false);


		}

		void fadeOut()
		{
			splashImage.CrossFadeAlpha(0.0f, 2.5f, false);
			//splashImage2.CrossFadeAlpha (0.0f, 2.5f, false);
			//splashImage3.CrossFadeAlpha(0.0f, 2.5f, false);
		}


	}
