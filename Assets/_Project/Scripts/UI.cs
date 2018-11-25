using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UI : MonoBehaviour {

	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
            Pause();
	}

    public void Pause()
    {

    }

    public void Quit()
    {
        //#if UNITY_EDITOR
        //EditorApplication.isPlaying = false;
       // #else
        Application.Quit();
       //#endif
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Rachmaninov_01", LoadSceneMode.Single);
    }
}
