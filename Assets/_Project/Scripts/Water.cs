using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
//using UnityEditor.SceneManagement;

public class Water : MonoBehaviour {
    
	public float marea = 0.0003f;
	public float multMarea = 30f;
	public float tiempoParaMuerte = 3.0f;
	private float tiempoSumergido = 0f;

    public Image deadScreen;

	public Canvas endGame;
	public Canvas hud;
	public Text finalMessage;
	

	void Update () {

        if (!BoatStats.showTutorial)
        {
            if (transform.position.y > 7.15)
                SceneManager.LoadScene("Boat", LoadSceneMode.Single);   //Scene end
            else if (transform.position.y > 3.75)
                transform.Translate(0f, marea * multMarea, 0f);        //More velocity
            else
                transform.Translate(0f, marea, 0f);                    //Initial state
        }

	}

	void OnTriggerStay(Collider other){

		if (other.gameObject.CompareTag ("Player")) {

			tiempoSumergido += Time.deltaTime;

			float alphaImage = tiempoSumergido / tiempoParaMuerte;

			deadScreen.color = new Color (148f / 255f, 28f / 255f, 28f / 255f, alphaImage);

			if (tiempoSumergido > tiempoParaMuerte) {

				other.gameObject.GetComponent<Personaje> ().gameEnded = true;

                finalMessage.text = "You have drowned, learn to swim";
                endGame.gameObject.SetActive (true); 
				hud.gameObject.SetActive (false); 
				Cursor.visible = false;

			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
            
			tiempoSumergido = 0f;
			deadScreen.color = new Color (148f / 255f, 28f / 255f, 28f / 255f, 0);

		}
	}
}
