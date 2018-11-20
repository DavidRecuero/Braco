using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
//using UnityEditor.SceneManagement;

public class Personaje : MonoBehaviour {
    
    [Header("Movement")]
    public float horizontalSpeed;
	public float verticalSpeed;

    [Header("Rotation")]
    public float xSpeed;
    public float ySpeed;
    private float h;
	private float v;

    [Header("Items GameObjects")]
    public GameObject control;
	public GameObject velocity;
	public GameObject shield;
	public GameObject hammer;
	public GameObject mast;

    public Text message;                  
	private byte messageAlpha = 0;

    public Text tutorial;

    public RawImage hammerImage;          
	public RawImage inventoryImage;         
	public Texture velaTexture;       
	public Texture mastilTexture;        
	public Texture remacheTexture;        
	public Texture remoTexture;        
	public Texture emptyTexture;        

    [HideInInspector] public bool gameEnded;

    private Camera fpsCamera;

    enum Objects { Empty, Velocity, Control, Shield, Mast };
    private Objects inventory;
    private bool hammerPicked = false;
    private bool mastPicked = false;

    void Start ()
    {
		gameEnded = false;

		Cursor.visible = false;

        inventory = Objects.Empty;

        fpsCamera = Camera.main;

        BoatStats.boatShield = 1.0f;
		BoatStats.boatVelocity = 1.0f;
		BoatStats.boatControl = 1.0f;

		if(!BoatStats.showTutorial)
			tutorial.gameObject.SetActive(false);               //HUD
	}
	
	void Update ()
    {
		if (!gameEnded)
        {
            PlayerMovement();

			if (Input.GetKeyDown ("q"))
                ThrowAction();

            InventoryManager();

            TextAnimation();
        }
        else 
			Cursor.visible = true;

        if (BoatStats.showTutorial && Input.anyKeyDown)
        {
            tutorial.gameObject.SetActive(false);
            BoatStats.showTutorial = false;
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
                ShowText("Hammer picked");
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
                    ShowText("Mast picked");
                }
				else
                    ShowText("You are carrying something");
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
                    ShowText("Shield picked");
                }
				else
                    ShowText("You are carrying something");
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
                    ShowText("Control picked");
                }
				else
                    ShowText("You are carrying something");
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
                    ShowText("Velocity picked");
				}
				else
                    ShowText("You are carrying something");
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
						other.transform.Find("Mast").gameObject.SetActive(true);

                        ShowText("Mast fixed");
                    }
					else if (inventory == Objects.Shield)
					{
						inventory = Objects.Empty;
						BoatStats.boatShield++;

						if(BoatStats.boatShield == 2f)
							other.transform.Find("Broken 1").gameObject.SetActive(false);
						else
							other.transform.Find("Broken 2").gameObject.SetActive(false);

                        ShowText("Shield fixed");
					}
					else if (inventory == Objects.Control)
					{
						inventory = Objects.Empty;
						BoatStats.boatControl++;

						if(BoatStats.boatControl == 2f)
							other.transform.Find("Paddle 1").gameObject.SetActive(true);
						else
							other.transform.Find("Paddle 2").gameObject.SetActive(true);

                        ShowText("Control fixed");

                    }
					else if (inventory == Objects.Velocity)
					{
                        if (mastPicked)
                        {
                            inventory = Objects.Empty;
                            BoatStats.boatVelocity++;

                            if (BoatStats.boatVelocity == 2f)
                                other.transform.Find("Flag 1").gameObject.SetActive(true);
                            else
                                other.transform.Find("Flag 2").gameObject.SetActive(true);

                            ShowText("Velocity fixed");

                        }
                        else
                            ShowText("You need to fix the mast!");
					}
					else if (inventory == Objects.Empty)
                        ShowText("You are carrying nothing");
				}
				else
                    ShowText("You need to pick the hammer!");
			}
		}
	}  

    void ShowText(string text_)
    {
        message.text = text_;
        messageAlpha = 255;
    }

    void TextAnimation()
    {
        if (messageAlpha > 5)
        {
            messageAlpha -= 2;
            message.color = new Color32(255, 225, 225, messageAlpha);
        }
        else
            message.color = new Color32(0, 0, 0, 0);
    }

    void ThrowAction()
    {
        Vector3 spawnPosition = transform.position + (transform.forward * 2);
        spawnPosition.y += 3f;

        if (inventory == Objects.Shield)
        {
            inventory = Objects.Empty;
            Instantiate(shield, spawnPosition, transform.rotation);
        }
        else if (inventory == Objects.Control)
        {
            inventory = Objects.Empty;
            Instantiate(control, spawnPosition, transform.rotation);
        }
        else if (inventory == Objects.Velocity)
        {
            inventory = Objects.Empty;
            Instantiate(velocity, spawnPosition, transform.rotation);
        }
        else if (inventory == Objects.Mast)
        {
            inventory = Objects.Empty;
            Instantiate(mast, spawnPosition, transform.rotation);
        }
        else if (inventory == Objects.Empty)
            ShowText("Nothing to throw");
    }

    void InventoryManager()
    {
        switch (inventory)
        {
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
    }

    void PlayerMovement()
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////MOVIMIENTO

        h = horizontalSpeed * Input.GetAxis("Mouse X");
        v = verticalSpeed * Input.GetAxis("Mouse Y");

        transform.Rotate(0, h, 0);                                                                                 //Horizontal rotation

        if (fpsCamera.transform.eulerAngles.x - v > 90f && fpsCamera.transform.eulerAngles.x - v < 360f - 90f)      //Vertical Rotation limited between 90 and -90 degrees
        {
            float diff = 90f - fpsCamera.transform.eulerAngles.x;

            if (diff < 0)
                fpsCamera.transform.eulerAngles = new Vector3(270f, fpsCamera.transform.eulerAngles.y, fpsCamera.transform.eulerAngles.z);
            else
                fpsCamera.transform.eulerAngles = new Vector3(90f, fpsCamera.transform.eulerAngles.y, fpsCamera.transform.eulerAngles.z);
        }
        else
            fpsCamera.transform.Rotate(-v, 0, 0);

        ///////////////////////////////////////////////////////////WASD

        if (Input.GetKey(KeyCode.W))
            transform.Translate(0, 0, ySpeed);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(0, 0, -ySpeed);

        if (Input.GetKey(KeyCode.D))
            transform.Translate(xSpeed, 0, 0);
        else if (Input.GetKey(KeyCode.A))
            transform.Translate(-xSpeed, 0, 0);
    }
}
