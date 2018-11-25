using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelBoat : MonoBehaviour {

	public GameObject TravelMenu;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag ("Player") && Input.GetKeyDown ("e")) 
		{
			GameObject.FindGameObjectWithTag ("HUD").SetActive(false);
			TravelMenu.SetActive(true);
			Cursor.visible = true;
			other.GetComponent<Personaje> ().freezeMov = true;
		}
	}
}
