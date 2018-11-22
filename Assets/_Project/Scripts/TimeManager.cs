using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    private float secsForDay;// = 1200f;
    private float secToUpdateSun;// = 0.1f;
    private float gradesForEachUpdate;
    private GameObject player;

	// Use this for initialization
	void Start () {

        player = GameObject.FindWithTag("Player");
        secsForDay = player.GetComponent<Personaje>().secsForDay;
        secToUpdateSun = player.GetComponent<Personaje>().secToUpdateSun;

        gradesForEachUpdate = (360f / secsForDay) * secToUpdateSun;

        InvokeRepeating("RotateLight", 0, secToUpdateSun);
        InvokeRepeating("DayPassed", secsForDay, secsForDay);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void RotateLight()
    {
        transform.Rotate(gradesForEachUpdate, 0, 0);
    }

    void DayPassed()
    {
        player.GetComponent<Personaje>().currentDay++;
        player.GetComponent<Personaje>().ShowText(player.GetComponent<Personaje>().currentDay.ToString());
    }
}
