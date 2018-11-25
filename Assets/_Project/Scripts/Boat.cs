using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [Header("Linear Velocity")]
    public float aceleration  = 0.001f;
    public float desaceleration = 0.0005f;
    public float brake = 0.015f;
    public float maxVel = 0.05f;
    public float secondsImprovementByShelds = 10f;

    private float velocity = 0.0f;
    

    [Header("Angular Velocity")]
    public float angAcel = 0.015f;
    public float angDesacel = 0.005f;
    public float maxAngVel = 0.3f;

    private bool fromD = false;     //last key pressed D (between A and D)
    private bool fromA = false;     //opposite ^^^^^^^^

    private float angVel = 0.0f;

    [Header("Game Time")]
    public float secondsToDie = 60f;
    public float secondsPenalizedWhenDamage = 5f;
    private float secondsRunning = 0f;

    [Header("HUD")]
    public Image bar;   //Chrono
    public Canvas hud;  //Muerte

    [Header("Menus")]
    public Canvas endGame;
    public Canvas tutorial;
    public Text finalMessage;


    private bool gameEnded;
    

    void Start()
    {
		Cursor.visible = false;
		gameEnded = false;

        //Apply improvements from first scene
		aceleration *= BoatStats.boatVelocity;
		maxVel      *= BoatStats.boatVelocity;

		desaceleration  *= BoatStats.boatControl;
		brake           *= BoatStats.boatControl;
		angAcel         *= BoatStats.boatControl;
		angDesacel      *= BoatStats.boatControl;
		maxAngVel       *= BoatStats.boatControl;

		if (BoatStats.boatShield == 2f)
			secondsToDie += secondsImprovementByShelds;
		else if (BoatStats.boatShield == 3f)
			secondsToDie += secondsImprovementByShelds * 2f;

        //If first game, show tutorial
		if (!BoatStats.showBTutorial) 
			tutorial.gameObject.SetActive (false);

    }
    
    void Update()
    {
        if (!gameEnded && !BoatStats.showBTutorial)
        {
            BoatMovement();

            bar.fillAmount = (secondsToDie - secondsRunning) / secondsToDie;

            secondsRunning += Time.deltaTime;

            if (secondsRunning > secondsToDie)
                GameOverMessage();
        }

        if (BoatStats.showBTutorial && Input.anyKeyDown)
        {
			BoatStats.showBTutorial = false;
			tutorial.gameObject.SetActive (false);
		}
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Breakable Obstacle"))
        {
            secondsRunning += secondsPenalizedWhenDamage;
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Unbreakable Obstacle"))
            GameOverMessage();

        else if (other.gameObject.CompareTag("Finish"))
			SceneManager.LoadScene ("MechanicsTest");
    }

    void BoatMovement()
    {
        if (Input.GetKey("w"))
        {
            velocity += aceleration;

            if (velocity > maxVel)
                velocity = maxVel;

        }
        else if (Input.GetKey("s"))
        {
            velocity -= brake;

            if (velocity < 0f)
                velocity = 0f;

        }
        else  //desaceleration without brake
        {

            velocity -= desaceleration;

            if (velocity < 0f)
                velocity = 0f;

        }

        if (Input.GetKeyUp("d"))
        {
            fromD = true;
            fromA = false;
        }
        if (Input.GetKeyUp("a"))
        {
            fromD = false;
            fromA = true;
        }

        if (Input.GetKey("a"))
        {

            angVel -= angAcel;

            if (angVel < -maxAngVel)
                angVel = -maxAngVel;

        }
        else if (Input.GetKey("d"))
        {
            angVel += angAcel;

            if (angVel > maxAngVel)
                angVel = maxAngVel;

        }
        else    //inertia
        {

            if (fromA)
            {
                if (angVel < 0f)
                    angVel += angDesacel;
                else
                    angVel = 0f;

            }
            else if (fromD)
            {
                if (angVel > 0f)
                    angVel -= angDesacel;
                else
                    angVel = 0f;
            }
        }

        transform.Translate(0.0f, 0.0f, velocity);
        transform.Rotate(0.0f, angVel, 0.0f);
    }

    void GameOverMessage()
    {
        endGame.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
        finalMessage.text = "Your boat has broken";
        gameEnded = true;
        Cursor.visible = true;
    }

    void WinMessage()
    {
        endGame.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
        finalMessage.text = "Congratulations! You reach Safeland in " + Mathf.Floor(secondsRunning) + "s";
        gameEnded = true;
        Cursor.visible = true;
    }
}
