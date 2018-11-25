using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Vector3 _initPos;
	private float _velocity;
	private float _distance;

	// Use this for initialization
	void Start () {
		_initPos = transform.position;
		_velocity = 0.5f;
		_distance = 50f;
	}

	// Update is called once per frame
	void Update () {
		transform.position += transform.forward.normalized * 0.5f;

		if (Vector3.Distance(transform.position, _initPos) > 50f)
			Destroy(this.gameObject);
	}

	void OnTriggerEnter(Collider other) //Collision needs a rigidbody in other?????
	{
		if (other.gameObject.CompareTag ("Bullet Target"))
			other.GetComponent<Rigidbody> ().useGravity = true;

		Destroy(this.gameObject);
	}
}
