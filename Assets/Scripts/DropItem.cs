using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour {


	public GameObject[] items;
	int randomInt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Drop()
	{

		randomInt = Random.Range(0, items.Length);
		Instantiate(items[randomInt], transform.position, Quaternion.identity);

	}
}
