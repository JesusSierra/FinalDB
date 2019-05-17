using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using System;

public class PowerItem : MonoBehaviour {

	private GameObject player;
    private int posx;
    private int posy;
    private AudioSource audio;
	private PlayerHealth playerHealth;
	private ParticleSystem particleSystem;

	private MeshRenderer meshRenderer;
	private ParticleSystem brainParticles;

	public GameObject pickupEffect;

	private PowerItemExplode powerItemExplode;
	private SphereCollider sphereCollider;

	// Use this for initialization
	void Start () {


		player = GameManager.instance.Player;
		playerHealth = player.GetComponent<PlayerHealth>();
		playerHealth.enabled = true;

		particleSystem = player.GetComponent<ParticleSystem>();
		//particleSystem.enableEmission = false;

		meshRenderer = GetComponentInChildren<MeshRenderer>();
		brainParticles = GetComponent<ParticleSystem>();

		powerItemExplode = GetComponent<PowerItemExplode>();
		sphereCollider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        posx = (int)player.transform.position.x;
        posy = (int)player.transform.position.y;
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player)
		{
			
			StartCoroutine(InvincibleRoutine());
			meshRenderer.enabled = false;
            try
            {
                Debug.Log("Connecting to database...");
                string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();
                    Console.WriteLine("Done.");


                    using (SqlCommand command = new SqlCommand("dbo.ModObject", connection))
                    { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                        command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                        command.Parameters.AddWithValue("@Type", "Power");
                        command.Parameters.AddWithValue("@CoordX", posx);
                        command.Parameters.AddWithValue("@CoordY", posy);

                        command.ExecuteNonQuery();
                        Console.WriteLine("Lista escritura con un SP.");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

        }	
	}

	public IEnumerator InvincibleRoutine()
	{
		powerItemExplode.Pickup();
		print("pick PowerItem");
		particleSystem.enableEmission = true;
		playerHealth.enabled = false;
		brainParticles.enableEmission = false;
		sphereCollider.enabled = false;


		yield return new WaitForSeconds(10f);
		print("no more invencible");
		particleSystem.enableEmission = false;
		playerHealth.enabled = true;
		Destroy(gameObject);



	}

	void Pickup()
	{
		Instantiate(pickupEffect, transform.position, transform.rotation);

	}

}
