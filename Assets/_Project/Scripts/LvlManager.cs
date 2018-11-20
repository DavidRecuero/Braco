using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using UnityEngine;

public class LvlManager : MonoBehaviour {
    
	public void CargaNivel( string pNombreNivel){
		SceneManager.LoadScene (pNombreNivel);
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
