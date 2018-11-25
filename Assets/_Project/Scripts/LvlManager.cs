using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using UnityEngine;

public class LvlManager : MonoBehaviour {
    
	public GameObject player;
	public GameObject thisHUD;
	public GameObject generalHUD;

	public void CargaNivel( string pNombreNivel){
		SceneManager.LoadScene (pNombreNivel);
	}

	public void ExitTravelMenu()
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		generalHUD.SetActive(true);
		thisHUD.SetActive(false);
		Cursor.visible = false;
		player.GetComponent<Personaje> ().freezeMov = false;
	}

	public void QuitGame ()
	{
        //#if UNITY_EDITOR
        //EditorApplication.isPlaying = false;
        // #else
        Application.Quit();
        //#endif
    }
}
