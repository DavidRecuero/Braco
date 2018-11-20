using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Boat : MonoBehaviour
{
    //public int life = 3;
    public float secondsToDie = 60f;
    public float secondsWhenDamage = 5f;
    private float secondsRunning = 0f;

    public Canvas timeBar;

    public float aceleration  = 0.001f;
    public float desaceleration = 0.0005f;
    public float brake = 0.015f;

    public float maxVel = 0.05f;

    public float angAcel = 0.015f;
    public float angDesacel = 0.005f;

    public float maxAngVel = 0.3f;

    private float velocity = 0.0f;
    private float angVel  = 0.0f;

	public Image bar;   ////////////////////////////////////////////////////////////////////////////////////////////////////////CHRONO

    private bool fromD = false;
    private bool fromA = false;

	public Canvas hud;  //////////////////////////////////////////////////////////////////////////////////////////////////////////Muerte
	public Canvas endGame;
	public Canvas tutorial;
	public Text finalMessage;


	private bool gameEnded; 

    // Use this for initialization
    void Start()
    {
		Cursor.visible = false;
		gameEnded = false;

		aceleration *= BoatStats.boatVelocity;
		maxVel      *= BoatStats.boatVelocity;

		desaceleration  *= BoatStats.boatControl;
		brake           *= BoatStats.boatControl;
		angAcel         *= BoatStats.boatControl;
		angDesacel      *= BoatStats.boatControl;
		maxAngVel       *= BoatStats.boatControl;

		if (BoatStats.boatShield == 2f)
			secondsToDie = 70f;
		else if (BoatStats.boatShield == 3f)
			secondsToDie = 80f;

		if (!BoatStats.showBTutorial) {
			tutorial.gameObject.SetActive (false);
		} 

    }

    // Update is called once per frame
    void Update()
    {
		if (BoatStats.showBTutorial && Input.anyKeyDown) {
			BoatStats.showBTutorial = false;
			tutorial.gameObject.SetActive (false);
		}


		if (!gameEnded) {
			
			if (Input.GetKey ("w")) {
				velocity += aceleration;

				if (velocity > maxVel) {
					velocity = maxVel;
				}
			} else if (Input.GetKey ("s")) {
				velocity -= brake;

				if (velocity < 0f) {
					velocity = 0f;
				}
			} else {
				//desaceleracion barca
				velocity -= desaceleration;

				if (velocity < 0f) {
					velocity = 0f;
				}
			}

			if (Input.GetKeyUp ("d")) {
				fromD = true;
				fromA = false;
			}
			if (Input.GetKeyUp ("a")) {
				fromD = false;
				fromA = true;
			}

			if (Input.GetKey ("a")) {

				angVel -= angAcel;

				if (angVel < -maxAngVel) {
					angVel = -maxAngVel;
				}
			} else if (Input.GetKey ("d")) {
				angVel += angAcel;

				if (angVel > maxAngVel) {
					angVel = maxAngVel;
				}
			} else {
				//inercia

				if (fromA) {
					if (angVel < 0f) {
						angVel += angDesacel;
					} else {
						angVel = 0f;
					}
				} else if (fromD) {
					if (angVel > 0f) {
						angVel -= angDesacel;
					} else {
						angVel = 0f;
					}
				}
			}

			transform.Translate (0.0f, 0.0f, velocity);
			transform.Rotate (0.0f, angVel, 0.0f);

			bar.fillAmount = (secondsToDie - secondsRunning) / secondsToDie;

			if(!BoatStats.showBTutorial)
				secondsRunning += Time.deltaTime;


			if (secondsRunning > secondsToDie) {
				endGame.gameObject.SetActive (true); 
				hud.gameObject.SetActive (false);
				finalMessage.text = "Your boat has broken";
				gameEnded = true;
				Cursor.visible = true;
			}


		}
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Breakable Obstacle"))
        {
            secondsRunning += secondsWhenDamage;
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Unbreakable Obstacle"))
        {
			endGame.gameObject.SetActive(true);
			hud.gameObject.SetActive(false);
			finalMessage.text = "Your boat has broken";
			gameEnded = true;
			Cursor.visible = true;
            
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
			endGame.gameObject.SetActive(true);  
			hud.gameObject.SetActive(false);                            
			finalMessage.text = "Congratulations! You reach Safeland in " + secondsRunning;    
			gameEnded = true; 
			Cursor.visible = true;
        }
    }

}
