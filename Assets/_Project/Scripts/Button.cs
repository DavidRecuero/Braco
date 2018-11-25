using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	void OnTriggerStay(Collider other)
	{
		if (Input.GetKeyDown ("e") && other.gameObject.CompareTag ("Player")) 
		{
			Destroy (GameObject.FindGameObjectWithTag ("Barrera"));
		}
	}
}
