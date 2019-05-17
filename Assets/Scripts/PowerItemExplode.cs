using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerItemExplode : MonoBehaviour {


	public GameObject pickupEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Pickup()
	{
		GameObject newPickupEffect = (GameObject)Instantiate(pickupEffect, transform.position, transform.rotation);
		Destroy(newPickupEffect, 1);


	}
}
