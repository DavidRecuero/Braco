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

    [Header("Jump")]    //privatizar cosas despues de debug
    public bool activeJump;
    public float jumpForce;
    public bool isGrounded;        //must be colliding with an object tagged as "ground"

    [Header("Day & Time")]
    public float secsForDay;
    public float secToUpdateSun;
    public float daysToEnd;
    [HideInInspector] public int currentDay; 

    [Header("Inventory")]   //privatizar cosas despues de debug
    public bool bigInventoryActive;

    [HideInInspector] public enum Objects { Empty, Velocity, Control, Shield, Mast };
    private Objects inventory;
    private bool hammerPicked = false;
    private bool mastPicked = false;

    public Objects[] bigInventory = new Objects[5] { Objects.Empty, Objects.Empty, Objects.Empty, Objects.Empty, Objects.Empty };
    public int inventoryCurrentPosition;

    [Header("Items GameObjects")]
    public GameObject control;
	public GameObject velocity;
	public GameObject shield;
	public GameObject hammer;
	public GameObject mast;
    
    [Header("HUD & UI")]
    public Text message;                  
	private byte messageAlpha = 0;

    public Text tutorial;

    public RawImage hammerImage;          
	public RawImage inventoryImage;

    public RawImage[] bigInventoryHUD;
    public RawImage[] bigInventorySelectedHUD;

    public Texture velaTexture;       
	public Texture mastilTexture;        
	public Texture remacheTexture;        
	public Texture remoTexture;        
	public Texture emptyTexture;        

    [HideInInspector] public bool gameEnded;

    private Camera fpsCamera;
    
    void Start ()
    {
		gameEnded = false;

		Cursor.visible = false;

        currentDay = 0;

        inventory = Objects.Empty;

        fpsCamera = Camera.main;

        BoatStats.boatShield = 1.0f;
		BoatStats.boatVelocity = 1.0f;
		BoatStats.boatControl = 1.0f;

        if (bigInventoryActive)
            inventoryImage.enabled = false;
        else
            bigInventoryHUD[0].transform.parent.gameObject.SetActive(false);

		if(!BoatStats.showTutorial)
			tutorial.gameObject.SetActive(false);               //HUD
	}
	
	void Update ()
    {
		if (!gameEnded)
        {
            PlayerMovement();

            if(Input.GetKeyDown(KeyCode.Space) && isGrounded && activeJump)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                isGrounded = false;
            }

            if (bigInventoryActive)
            {
                SelectInventoryPosition();

                if (Input.GetKeyDown("q"))
                {
                    ThrowAction(bigInventory[inventoryCurrentPosition]);

                    inventoryCurrentPosition++;
                    if (inventoryCurrentPosition == bigInventory.Length)
                        inventoryCurrentPosition = 0;
                }
            }
            else
            {
                if (Input.GetKeyDown("q"))
                    ThrowAction(inventory);
            }

            if (currentDay == daysToEnd)
                ShowText("Time ended");

            InventoryHudManager();

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
			if (Input.GetKeyDown("e"))
			{
				hammerPicked = true;
				other.gameObject.SetActive(false);
                ShowText("Hammer picked");
                hammerImage.color = new Color32(255, 255, 255, 255); 
			}
		}
		else if (other.gameObject.CompareTag("Mast"))
		{
			if (Input.GetKeyDown("e"))
			{
                if (bigInventoryActive)
                {
                    PickSomething(Objects.Mast);
                    other.gameObject.SetActive(false);
                }
                else
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
		}
		else if (other.gameObject.CompareTag("Shield"))
		{

			if (Input.GetKeyDown("e"))
			{
                if (bigInventoryActive)
                {
                    PickSomething(Objects.Shield);
                    other.gameObject.SetActive(false);
                }
                else
                {
                    if (inventory == Objects.Empty)
                    {
                        other.gameObject.SetActive(false);
                        inventory = Objects.Shield;
                        ShowText("Shield picked");
                    }
                    else
                        ShowText("You are carrying something");
                }
            }
		}
		else if (other.gameObject.CompareTag("Control"))
		{

			if (Input.GetKeyDown("e"))
			{
                if (bigInventoryActive)
                {
                    PickSomething(Objects.Control);
                    other.gameObject.SetActive(false);
                }
                else
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
		}
		else if (other.gameObject.CompareTag("Velocity"))
		{

			if (Input.GetKeyDown("e"))
			{
                if (bigInventoryActive)
                {
                    PickSomething(Objects.Velocity);
                    other.gameObject.SetActive(false);
                }
                else
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
		}
		else if (other.gameObject.CompareTag("Boat"))
		{

			if (Input.GetKeyDown("e"))
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

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    public void ShowText(string text_)
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

    void ThrowAction(Objects item)
    {
        Vector3 spawnPosition = transform.position + (transform.forward * 2);
        spawnPosition.y += 3f;

        if (item == Objects.Shield)
            Instantiate(shield, spawnPosition, transform.rotation);
        else if (item == Objects.Control)
            Instantiate(control, spawnPosition, transform.rotation);
        else if (item == Objects.Velocity)
            Instantiate(velocity, spawnPosition, transform.rotation);
        else if (item == Objects.Mast)
            Instantiate(mast, spawnPosition, transform.rotation);
        

        if(bigInventoryActive)
        {
            bigInventory[inventoryCurrentPosition] = Objects.Empty;
        }
        else
        {
            if(item == Objects.Empty)
                ShowText("Nothing to throw");

            inventory = Objects.Empty;
        }
    }

    void InventoryHudManager()
    {
        if (bigInventoryActive)
        {
            for (int i = 0; i < bigInventory.Length; i++)
            {
                switch (bigInventory[i])                                        //fills the image with the correspondent object
                {
                    case Objects.Velocity:
                        bigInventoryHUD[i].texture = velaTexture;
                        break;
                    case Objects.Control:
                        bigInventoryHUD[i].texture = remoTexture;
                        break;
                    case Objects.Shield:
                        bigInventoryHUD[i].texture = remacheTexture;
                        break;
                    case Objects.Mast:
                        bigInventoryHUD[i].texture = mastilTexture;
                        break;
                    default:
                        bigInventoryHUD[i].texture = emptyTexture;
                        break;
                }

                if (inventoryCurrentPosition == i)
                    bigInventorySelectedHUD[i].enabled = true;
                else
                    bigInventorySelectedHUD[i].enabled = false;
            }

        }
        else
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
    }

    void PickSomething(Objects item)
    {
        bool itemStacked = false;

        for(int i = 0; i < bigInventory.Length; i++)
        {
            if(bigInventory[i] == Objects.Empty && !itemStacked)
            {
                bigInventory[i] = item;
                itemStacked = true;
            }
        }

        if(!itemStacked)
        {
            ThrowAction(bigInventory[inventoryCurrentPosition]);

            bigInventory[inventoryCurrentPosition] = item;
        }

    }

    void SelectInventoryPosition()
    {
        if      (Input.GetKeyDown("1"))
            inventoryCurrentPosition = 0;
        else if (Input.GetKeyDown("2"))
            inventoryCurrentPosition = 1;
        else if (Input.GetKeyDown("3"))
            inventoryCurrentPosition = 2;
        else if (Input.GetKeyDown("4"))
            inventoryCurrentPosition = 3;
        else if (Input.GetKeyDown("5"))
            inventoryCurrentPosition = 4;
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
