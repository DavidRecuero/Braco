using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

	public GameObject player;
	public GameObject thisHUD;
	public GameObject generalHUD;
	public GameObject jumpText;
	public GameObject jumpButton;
	public GameObject shootText;
	public GameObject shootButton;


	public void buyJump()
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		int woods = 0;

		for (int i = 0; i < player.GetComponent<Personaje> ().bigInventory.Length; i++) 
		{
			if (player.GetComponent<Personaje> ().bigInventory [i] == Personaje.Objects.Shield)
				woods++;
		}

		if (woods >= 2) {
			int deleted = 0;
			int i = 0;

			while (deleted < 2) {
				if (player.GetComponent<Personaje> ().bigInventory [i] == Personaje.Objects.Shield) {
					player.GetComponent<Personaje> ().bigInventory [i] = Personaje.Objects.Empty;
					deleted++;
				}

				i++;
			}

			player.GetComponent<Personaje> ().activeJump = true;

			jumpText.SetActive(false);
			jumpButton.SetActive(false);

		} else {
			//no tienes maderas
		}
	}

	public void buyShoot()
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		int paddle = 0;

		for (int i = 0; i < player.GetComponent<Personaje> ().bigInventory.Length; i++) 
		{
			if (player.GetComponent<Personaje> ().bigInventory [i] == Personaje.Objects.Control)
				paddle++;
		}

		if (paddle >= 1) {
			int deleted = 0;
			int i = 0;

			while (deleted < 1) {
				if (player.GetComponent<Personaje> ().bigInventory [i] == Personaje.Objects.Control) {
					player.GetComponent<Personaje> ().bigInventory [i] = Personaje.Objects.Empty;
					deleted++;
				}

				i++;
			}

			player.GetComponent<Personaje> ().activeShoot = true;

			shootText.SetActive(false);
			shootButton.SetActive(false);

		} else {
			//no tienes maderas
		}
	}

	public void exitShop()
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		generalHUD.SetActive(true);
		thisHUD.SetActive(false);
		Cursor.visible = false;
		player.GetComponent<Personaje> ().freezeMov = false;
	}
}
