using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using System;

public class LifeItem : MonoBehaviour
{
    private GameObject player;
    private int posx;
    private int posy;
    private LifeManager lifeManager;
    

    private SpriteRenderer spriteRenderer;
    public GameObject pickUpEffect;
    private PowerItemExplode powerItemExplode;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        lifeManager = FindObjectOfType<LifeManager>();
        spriteRenderer = GetComponentInChildren <SpriteRenderer>();

        powerItemExplode = GetComponent <PowerItemExplode>();
        boxCollider = GetComponent <BoxCollider>();

    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
        {
            PickLife();

            

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
                        command.Parameters.AddWithValue("@Type", "Life");
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




            print("Life Collected");
        }
    }

    public void PickLife()
    {
        lifeManager.GiveLife();
        powerItemExplode.Pickup();
        spriteRenderer.enabled = false;
        Destroy (gameObject);
    }    
    // Update is called once per frame
    void Update()
    {
        posx = (int)player.transform.position.x;
        posy = (int)player.transform.position.y;
    }
}
