using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
//using UnityEditor.SceneManagement;

public class Water : MonoBehaviour {

	public GameObject boat;
	public GameObject pj;
	public float fuerza = 500;
	public float marea = 0.0003f;
	public float multMarea = 30f;
	public float tiempoParaMuerte = 3.0f;
	private float tiempoSumergido = 0f;
	public Image deadScreen;
	private Vector3 direccion;

	public Canvas endGame;
	public Canvas hud;
	public Text finalMessage;

	private float alphaImage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y > 7.15) {
			SceneManager.LoadScene("Boat", LoadSceneMode.Single);
		} 
		else if (transform.position.y > 3.75) {
			transform.Translate (0f, marea * multMarea, 0f);
		} 
		else {
			transform.Translate (0f, marea, 0f);
		}
	}


	/*void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			//direccion = boat.transform.position - other.transform.position; 
			//pj.GetComponent<Rigidbody> ().AddForce (direccion.normalized * fuerza);
		
		}
	
	}*/

	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			tiempoSumergido += Time.deltaTime;

			alphaImage = tiempoSumergido / tiempoParaMuerte;

			deadScreen.color = new Color (148f / 255f, 28f / 255f, 28f / 255f, alphaImage);

			if (tiempoSumergido > tiempoParaMuerte) {
				other.gameObject.GetComponent<Personaje> ().gameEnded = true;

				endGame.gameObject.SetActive (true); 
				hud.gameObject.SetActive (false); 
				finalMessage.text = "You have drowned, learn to swim";
				Cursor.visible = false;
			}
		}

	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			//pj.GetComponent<Rigidbody> ().AddForce (direccion.normalized * -fuerza);
			alphaImage = 0f;
			tiempoSumergido = 0f;

			deadScreen.color = new Color (148f / 255f, 28f / 255f, 28f / 255f, alphaImage);
		}
	}
}
