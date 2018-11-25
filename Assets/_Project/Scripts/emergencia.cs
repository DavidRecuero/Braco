using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class emergencia : MonoBehaviour {

	void OnTriggerStay(Collider other)
	{
		if (Input.GetKeyDown ("e") && other.gameObject.CompareTag ("Player")) {
			SceneManager.LoadScene("MechanicsTest", LoadSceneMode.Single); 
		}

	}
}
