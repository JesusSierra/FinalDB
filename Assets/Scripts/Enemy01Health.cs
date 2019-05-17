using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Data;
using System.Data.SqlClient;
using System;

public class Enemy01Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private float dissapearSpeed = 2f;
    [SerializeField] private int currentHealth;
    [SerializeField] public int EnemyID = 1;

    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    private bool isAlive;
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private bool dissapearEnemy = false;
    private BoxCollider weaponCollider;
    private AudioSource audio;
    public AudioClip deathSound;
    public AudioClip damageSound;
    private DropItem dropItem;
   

    public bool IsAlive
    {
         get {return isAlive;}
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent <Rigidbody>();
        capsuleCollider = GetComponent <CapsuleCollider>();
        nav = GetComponent <NavMeshAgent>();
        anim = GetComponent <Animator>();
        isAlive = true;
        currentHealth = startingHealth;
        weaponCollider = GetComponentInChildren <BoxCollider>();
        audio = GetComponent <AudioSource>();
        dropItem = GetComponent <DropItem>();
        try
        {
            Debug.Log("Connecting to database...");
            string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";

            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();



                using (SqlCommand command = new SqlCommand("dbo.ModEnemy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@IDEnemy", EnemyID);
                    command.Parameters.AddWithValue("@Health", currentHealth);

                    command.ExecuteNonQuery();

                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (dissapearEnemy)
        {
            transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);
        }
        else { }

        try
        {
            Debug.Log("Connecting to database...");
            string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";

            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();



                using (SqlCommand command = new SqlCommand("dbo.ModEnemy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@IDEnemy", EnemyID);
                    command.Parameters.AddWithValue("@Health", currentHealth);

                    command.ExecuteNonQuery();

                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if (other.tag == "PlayerWeapon")
            {
                takeHit ();
                timer = 0f;
            }
        }
    }

    void takeHit()
    {
        if (currentHealth > 0)
        {
            anim.Play ("Hurt");
            currentHealth -= 10;
            audio.PlayOneShot(damageSound);
        }
        if (currentHealth <= 0)
        {
            isAlive = false;
            audio.PlayOneShot(deathSound);
            KillEnemy();
        }
        

    }

    void KillEnemy()
    {
        capsuleCollider.enabled = false;
        nav.enabled = false;
        anim.SetTrigger ("EnemyDie");
        rigidbody.isKinematic = true;
        weaponCollider.enabled = false;

        StartCoroutine (removeEnemy());
        dropItem.Drop();
        try
        {
            Debug.Log("Connecting to database...");
            string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";

            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dbo.ModDestObj", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.ExecuteNonQuery();

                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    IEnumerator removeEnemy()
    {
        yield return new WaitForSeconds (2f);
        dissapearEnemy = true;
        yield return new WaitForSeconds (1f);
        Destroy (gameObject);
    }
}
