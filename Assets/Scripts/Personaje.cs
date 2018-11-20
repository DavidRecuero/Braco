using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
//using UnityEditor.SceneManagement;

public class Personaje : MonoBehaviour {

	public bool gameEnded;

	public GameObject fpsCamera;
	public Transform target;


	public float horizontalSpeed = 4.0f;
	public float verticalSpeed = 4.0f;

	float h;
	float v;

	public float xSpeed = 2.0f;
	public float ySpeed = 2.0f;

	public float speedH = 2.0f;
	public float speedV = 2.0f;

	float diff;

	public GameObject control;
	public GameObject velocity;
	public GameObject shield;
	public GameObject hammer;
	public GameObject mast;

	float charSpeed=5f;

	float hSpeed=4f; //Sensibilidad del raton en desplazamiento horizontal
	float vSpeed=4f; //Sensibilidad del raton en desplazamiento vertical

	private float boatVelocity = 1.0f;
	private float boatControl  = 1.0f;
	private float boatShield   = 1.0f;

	enum Objects { Empty, Velocity, Control, Shield, Mast };
	private Objects inventory = Objects.Empty;

	private bool hammerPicked = false;
	private bool mastPicked = false;

	private Vector3 spawnPosition;

	public Text message;                  
	private byte messageAlpha = 0;       

	public RawImage hammerImage;          
	public RawImage inventoryImage;         
	public Texture velaTexture;       
	public Texture mastilTexture;        
	public Texture remacheTexture;        
	public Texture remoTexture;        
	public Texture emptyTexture;        

	public Text tutorial;

	// Use this for initialization
	void Start () {
		gameEnded = false;

		Cursor.visible = false;

		BoatStats.boatShield = 1.0f;
		BoatStats.boatVelocity = 1.0f;
		BoatStats.boatControl = 1.0f;

		if(!BoatStats.showTutorial)
		{
			tutorial.gameObject.SetActive(false);///////////////////////////////////////HUD
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameEnded) {
			if (Input.anyKeyDown) {
				tutorial.gameObject.SetActive (false);
				BoatStats.showTutorial = false;
			}

			if (messageAlpha > 5) {
				messageAlpha -= 2;
				message.color = new Color32 (255, 225, 225, messageAlpha);
			} else
				message.color = new Color32 (0, 0, 0, 0);

			/////////////////////////////////////////////////////////////////////////////////////////////////MOVIMIENTO
			/// 
			h = horizontalSpeed * Input.GetAxis ("Mouse X");
			v = verticalSpeed * Input.GetAxis ("Mouse Y");


			//////////////////////////////////////////horizontal rotation
			transform.Rotate (0, h, 0);
			/////////////////////////////////////////////////////////////

			////////////Vertical Rotation limited between 90 and -90 degrees
			if (fpsCamera.transform.eulerAngles.x - v > 90f && fpsCamera.transform.eulerAngles.x - v < 360f - 90f) {
				diff = 90f - fpsCamera.transform.eulerAngles.x;

				if (diff < 0)
					fpsCamera.transform.eulerAngles = new Vector3 (270f, fpsCamera.transform.eulerAngles.y, fpsCamera.transform.eulerAngles.z);
				else
					fpsCamera.transform.eulerAngles = new Vector3 (90f, fpsCamera.transform.eulerAngles.y, fpsCamera.transform.eulerAngles.z);
			} else
				fpsCamera.transform.Rotate (-v, 0, 0);
			///////////////////////////////////////////////////////////////

			///////////////////////////////////////////////////////////WASD
			if (Input.GetKey (KeyCode.W))
				transform.Translate (0, 0, ySpeed);
			else if (Input.GetKey (KeyCode.S))
				transform.Translate (0, 0, -ySpeed);

			if (Input.GetKey (KeyCode.D))
				transform.Translate (xSpeed, 0, 0);
			else if (Input.GetKey (KeyCode.A))
				transform.Translate (-xSpeed, 0, 0);

			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	


			if (Input.GetKeyDown ("q")) {
				spawnPosition = transform.position + (transform.forward * 2);
				spawnPosition.y += 3f;

				if (inventory == Objects.Shield) {
					inventory = Objects.Empty;
					Instantiate (shield, spawnPosition, transform.rotation);
				} else if (inventory == Objects.Control) {
					inventory = Objects.Empty;
					Instantiate (control, spawnPosition, transform.rotation);
				} else if (inventory == Objects.Velocity) {
					inventory = Objects.Empty;
					Instantiate (velocity, spawnPosition, transform.rotation);
				} else if (inventory == Objects.Mast) {
					inventory = Objects.Empty;
					Instantiate (mast, spawnPosition, transform.rotation);
				} else if (inventory == Objects.Empty) {
					message.text = "Nothing to throw";
					messageAlpha = 255;  
				}
			}
		

			switch (inventory) {
			case Objects.Velocity:
				inventoryImage.texture = velaTexture;
				break;
			case Objects.Control:
				inventoryImage.texture = remoTexture;
				break;
			case Objects.Shield:
				inventoryImage.texture = remacheTexture;
				break;
			case Objects.Mast:
				inventoryImage.texture = mastilTexture;
				break;
			default:
				inventoryImage.texture = emptyTexture;
				break;
			}

		} else {
			Cursor.visible = true;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Hammer"))
		{
			if (Input.GetKeyDown("space") || Input.GetKeyDown("e"))
			{
				hammerPicked = true;
				other.gameObject.SetActive(false);
				message.text = "Hammer picked";                 
				messageAlpha = 255;                             
				hammerImage.color = new Color32(255, 255, 255, 255); 
			}
		}
		else if (other.gameObject.CompareTag("Mast"))
		{
			if (Input.GetKeyDown("space") || Input.GetKeyDown("e"))
			{
				if (inventory == Objects.Empty)
				{
					other.gameObject.SetActive(false);
					inventory = Objects.Mast;
					message.text = "Mast picked";
					messageAlpha = 255;   
				}
				else
				{
					message.text = "You are carrying something";    
					messageAlpha = 255;  
				}
			}
		}
		else if (other.gameObject.CompareTag("Shield"))
		{

			if (Input.GetKeyDown("space") || Input.GetKeyDown("e"))
			{ 
				if(inventory == Objects.Empty)
				{
					other.gameObject.SetActive(false);
					inventory = Objects.Shield;
					message.text = "Shield picked";
					messageAlpha = 255;
				}
				else
				{
					message.text = "You are carrying something";
					messageAlpha = 255;  
				} 
			}
		}
		else if (other.gameObject.CompareTag("Control"))
		{

			if (Input.GetKeyDown("space") || Input.GetKeyDown("e"))
			{
				if (inventory == Objects.Empty)
				{
					other.gameObject.SetActive(false);
					inventory = Objects.Control;
					message.text = "Control picked";
					messageAlpha = 255; 
				}
				else
				{
					message.text = "You are carrying something";
					messageAlpha = 255;  
				}
			}
		}
		else if (other.gameObject.CompareTag("Velocity"))
		{

			if (Input.GetKeyDown("space") || Input.GetKeyDown("e"))
			{
				if (inventory == Objects.Empty)
				{
					other.gameObject.SetActive(false);
					inventory = Objects.Velocity;
					message.text = "Velocity picked";
					messageAlpha = 255; 
				}
				else
				{
					message.text = "You are carrying something";
					messageAlpha = 255;  
				}
			}
		}
		else if (other.gameObject.CompareTag("Boat"))
		{

			if (Input.GetKeyDown("space") || Input.GetKeyDown("e"))
			{
				if (hammerPicked)
				{
					if (inventory == Objects.Mast)
					{
						inventory = Objects.Empty;
						mastPicked = true;
						other.transform.Find("Mástil").gameObject.SetActive(true);
						message.text = "Mast fixed"; 
						messageAlpha = 255; 
					}
					else if (inventory == Objects.Shield)
					{
						inventory = Objects.Empty;
						BoatStats.boatShield++;

						if(BoatStats.boatShield == 2f)
							other.transform.Find("Roto_boat01").gameObject.SetActive(false);
						else
							other.transform.Find("Roto_boat02").gameObject.SetActive(false);
						
						message.text = "Shield fixed"; 
						messageAlpha = 255;
						BoatStats.boatShield++;
					}
					else if (inventory == Objects.Control)
					{
						inventory = Objects.Empty;
						BoatStats.boatControl++;

						if(BoatStats.boatControl == 2f)
							other.transform.Find("Remo01").gameObject.SetActive(true);
						else
							other.transform.Find("Remo02").gameObject.SetActive(true);

						message.text = "Control fixed";
						messageAlpha = 255; 

					}
					else if (inventory == Objects.Velocity)
					{
						if (mastPicked)
						{
							inventory = Objects.Empty;
							BoatStats.boatVelocity++;

							if(BoatStats.boatVelocity == 2f)
								other.transform.Find("Vela01").gameObject.SetActive(true);
							else
								other.transform.Find("Vela02").gameObject.SetActive(true);

							message.text = "Velocity fixed"; 
							messageAlpha = 255;  


						}
						else
						{
							message.text = "You need to fix the mast!"; 
							messageAlpha = 255; 
						}
					}
					else if (inventory == Objects.Empty)
					{
						message.text = "You are carrying nothing"; 
						messageAlpha = 255;  
					}
				}
				else
				{
					message.text = "You need to pick the hammer!"; 
					messageAlpha = 255;  
				}
			}
		}
	}  
}
