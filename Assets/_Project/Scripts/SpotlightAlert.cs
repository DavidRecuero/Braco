using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotlightAlert : MonoBehaviour {

	public float radiusCircleMovement;
	public float velocity;

	private Vector3 initPos;
	private Vector3 targetPos;
	private float timeSaw;
	public float timeToDetected;

	public Image deadScreen;

	public Canvas endGame;
	public Canvas hud;
	public Text finalMessage;

	// Use this for initialization
	void Start () {
		initPos = transform.position;

		targetPos = FindNewPosition ();
	}

	// Update is called once per frame
	void Update () {
		
		transform.position = Vector3.MoveTowards (transform.position, targetPos, velocity);

		if(Vector3.Distance(transform.position, targetPos) < 0.1)
		{
			targetPos = FindNewPosition();
		}

	}

	/*void OnTriggerStay(Collider other){

		if (other.gameObject.CompareTag ("Player")) {

			timeSaw += Time.deltaTime;

			float alphaImage = timeSaw / timeToDetected;

			deadScreen.color = new Color (148f / 255f, 28f / 255f, 28f / 255f, alphaImage);

			if (timeSaw > timeToDetected) {

				other.gameObject.GetComponent<Personaje> ().gameEnded = true;

				finalMessage.text = "You have been detected";
				endGame.gameObject.SetActive (true); 
				hud.gameObject.SetActive (false); 
				Cursor.visible = false;

			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {

			timeSaw = 0f;
			deadScreen.color = new Color (148f / 255f, 28f / 255f, 28f / 255f, 0);

		}
	}*/

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			other.GetComponent<Personaje> ().ShowText ("You have been detected");
			other.GetComponent<Personaje> ().transform.position = other.GetComponent<Personaje> ().initPos;
		}
	}

	Vector3 FindNewPosition()
	{
		Vector3 randPosition = Random.insideUnitCircle * radiusCircleMovement;
		randPosition.x += initPos.x;
		randPosition.y += initPos.z;

		return (new Vector3 (randPosition.x, initPos.y, randPosition.y)); 
	}
}
