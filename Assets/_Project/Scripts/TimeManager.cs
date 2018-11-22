using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    private float secsForDay;// = 1200f;
    private float secToUpdateSun;// = 0.1f;
    private float gradesForEachUpdate;
    private GameObject player;

	void Start () {

        player = GameObject.FindWithTag("Player");
        secsForDay = player.GetComponent<Personaje>().secsForDay;
        secToUpdateSun = player.GetComponent<Personaje>().secToUpdateSun;

        gradesForEachUpdate = (360f / secsForDay) * secToUpdateSun;

		InvokeRepeating("RotateLight", secToUpdateSun, secToUpdateSun);
    }

    void RotateLight()
    {
        transform.Rotate(gradesForEachUpdate, 0, 0);
    }

	public void RotateLightSuddenly(float seconds)
	{
		float grades = gradesForEachUpdate * seconds / secToUpdateSun;

		GlobalVariables.currentSecond += seconds;

		transform.Rotate(grades, 0, 0);
	}

}
