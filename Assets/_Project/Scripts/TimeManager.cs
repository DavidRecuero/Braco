using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    private float secsForDay;// = 1200f;
    private float secToUpdateSun;// = 0.1f;
    private float gradesForEachUpdate;
    private GameObject player;
	private GameObject clock;

	void Start () {
		player = GameObject.FindWithTag("Player");
		clock = GameObject.FindWithTag("Clock");

		transform.eulerAngles = new Vector3 (GlobalVariables.currentSunRot, transform.rotation.y, transform.rotation.z);
		clock.transform.eulerAngles = new Vector3 (0, 0, GlobalVariables.currentSunRot);

		secsForDay = player.GetComponent<Personaje>().secsForDay;
		secToUpdateSun = player.GetComponent<Personaje>().secToUpdateSun;

		gradesForEachUpdate = (360f / secsForDay) * secToUpdateSun;

		InvokeRepeating("RotateLight", secToUpdateSun, secToUpdateSun);
    }

    void RotateLight()
    {
        transform.Rotate(gradesForEachUpdate, 0, 0);
		clock.transform.Rotate(0, 0, gradesForEachUpdate);
		GlobalVariables.currentSunRot = transform.eulerAngles.x;
		//Debug.Log (GlobalVariables.currentSunRot);
    }

	public void RotateLightSuddenly(float seconds)
	{
		float grades = gradesForEachUpdate * seconds / secToUpdateSun;

		GlobalVariables.currentSecond += seconds;

		transform.Rotate(grades, 0, 0);
		clock.transform.Rotate(0, 0, grades);
	}

}
