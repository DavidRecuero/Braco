using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour {

	public GameObject shopHUD;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag ("Player") && Input.GetKeyDown ("e")) 
		{
			GameObject.FindGameObjectWithTag ("HUD").SetActive(false);
			shopHUD.SetActive(true);
			Cursor.visible = true;
			other.GetComponent<Personaje> ().freezeMov = true;
		}
	}

}
