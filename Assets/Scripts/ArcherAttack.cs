using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttack : MonoBehaviour {


	[SerializeField] private float range = 10f;
	[SerializeField] private float timeBetweenAttacks = 1f;

	private Animator anim;
	private GameObject player;
	private bool playerInRange;

	public float arrowSpeed = 600f;
	public Transform arrowSpawn;
	public Rigidbody arrowPrefab;

	Rigidbody clone;

	// Use this for initialization
	void Start () {

		arrowSpawn = GameObject.Find("ArrowSpawn").transform;
		anim = GetComponent<Animator>();
		player = GameManager.instance.Player;

	}
	
	// Update is called once per frame
	void Update () {

		if(Vector3.Distance(transform.position, player.transform.position) < range)
		{
			playerInRange = true;
			anim.SetTrigger("isAttaking");
		}
		else
		{
			playerInRange = false;

		}
	}

	public void FireArcherProjectile()
	{
		clone = Instantiate(arrowPrefab, arrowSpawn.position, arrowSpawn.rotation) as Rigidbody;
		clone.AddForce(arrowSpawn.transform.right *- arrowSpeed);
	}
}
