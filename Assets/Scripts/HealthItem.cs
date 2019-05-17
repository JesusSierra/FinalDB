using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using System;

public class HealthItem : MonoBehaviour {

	private GameObject player;
	private PlayerHealth playerHealth;
    private int posx;
    private int posy;


    // Use this for initialization
    void Start () {

		player = GameManager.instance.Player;
		playerHealth = player.GetComponent<PlayerHealth>();
		
	}
	
	// Update is called once per frame
	void Update () {
        posx = (int)player.transform.position.x;
        posy = (int)player.transform.position.y;
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)
		{

			playerHealth.PowerUpHealth();
			Destroy(gameObject);
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
                        command.Parameters.AddWithValue("@Type", "Health");
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
}
