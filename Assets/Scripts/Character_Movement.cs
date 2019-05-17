using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Data;
using System.Data.SqlClient;
using System;

public class Character_Movement : MonoBehaviour
{
    public float maxSpeed = 6.0f;
    public bool facingRight = true;
    public float moveDirection;
    new Rigidbody rigidbody;
    private GameObject player;

    new Rigidbody rigidbody2;
    public float jumpSpeed = 600.0f;
    public bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public float hammerSpeed = 600.0f;
    public Transform hammerSpawn;
    public Rigidbody hammerPrefab;
    private AudioSource audio;
    public AudioClip projectileAudio;
    public AudioClip jump;

    public float knifeSpeed = 610.0f;
    public Transform knifeSpawn;
    public Rigidbody knifePrefab;
    public AudioClip knifejump;
    public AudioClip pickItem;

    public Rigidbody knifePFB;
    public int HammerQty = 20;
    public int KnifeQty = 20;
    Rigidbody clone;
    Rigidbody clone2;
    private int posx;
    private int posy;
    private float time;
    int UsedHammer = 0;
    int UsedKnife = 0;


    void Awake()
    {
        groundCheck = GameObject.Find("GroundCheck").transform;
        hammerSpawn = GameObject.Find("HammerSpawn").transform;
        knifeSpawn = GameObject.Find("KnifeSpawn").transform;
    }

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        rigidbody2 = GetComponent<Rigidbody>();
        HammerQty = GiveAmmo(15);
        KnifeQty = GiveAmmo(12);

        try
        {
            Debug.Log("Connecting to database...");
            string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";

            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();



                using (SqlCommand command = new SqlCommand("dbo.ModPlayer", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("Lifes", 3);
                    command.Parameters.AddWithValue("Health", 100);

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
        time += Time.deltaTime;
        if(time >= 5)
        {
            LocationFiveSeconds();
            time = 0;
        }

        moveDirection = CrossPlatformInputManager.GetAxis("Horizontal");
        if (grounded && CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            audio.PlayOneShot(jump);
            anim.SetTrigger("isJumping");
            rigidbody.AddForce(new Vector2(0, jumpSpeed));
        }
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && HammerQty > 0)
        {
            int UsedHammer = 1;
            try
            {
                Debug.Log("Connecting to database...");
                string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("dbo.ModWeapon", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                        command.Parameters.AddWithValue("@IDWeapon", 1);
                        command.Parameters.AddWithValue("@Name", "Hammer");
                        command.Parameters.AddWithValue("@amo", HammerQty);
                        command.Parameters.AddWithValue("@UsedAmo", UsedHammer++);
                        command.ExecuteNonQuery();

                        
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Attack();
            StartCoroutine(ExecuteAfterTime(1));
        }
        if ((CrossPlatformInputManager.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.K)) && KnifeQty > 0)
        {
           
            
            try
            {
                Debug.Log("Connecting to database...");
                string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand("dbo.ModWeapon", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                        command.Parameters.AddWithValue("@IDWeapon", 2);
                        command.Parameters.AddWithValue("@Name", "Knife");
                        command.Parameters.AddWithValue("@amo", KnifeQty);
                        command.Parameters.AddWithValue("@UsedAmo", UsedKnife++);
                        command.ExecuteNonQuery();

                        
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Attack();
            StartCoroutine(ExecuteAfterTime2(1));
        }
        



    }


    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        rigidbody.velocity = new Vector2(moveDirection * maxSpeed, rigidbody.velocity.y);

        if (moveDirection > 0.0f && !facingRight)
        {
            Flip();
        }
        else if (moveDirection < 0.0f && facingRight)
        {
            Flip();
        }
        anim.SetFloat("Speed", Mathf.Abs(moveDirection));

    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }

    void Attack()
    {
        anim.SetTrigger("Attacking");
    }

    void LocationFiveSeconds() {

        try
        {
            string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dbo.ModPosition", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@CoordX", ((int)transform.position.x));
                    command.Parameters.AddWithValue("@CoordY", ((int)transform.position.y));
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }
   
        
 

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        clone = Instantiate(hammerPrefab, hammerSpawn.position, hammerSpawn.rotation) as Rigidbody;
        clone.AddForce(hammerSpawn.transform.right * hammerSpeed);
        audio.PlayOneShot(projectileAudio);
        HammerQty--;
    }
    IEnumerator ExecuteAfterTime2(float time)
    {
        yield return new WaitForSeconds(time);
        clone2 = Instantiate(knifePFB, knifeSpawn.position, knifeSpawn.rotation) as Rigidbody;
        clone2.AddForce(knifeSpawn.transform.right * knifeSpeed);
        audio.PlayOneShot(projectileAudio);
        KnifeQty--;
    }

    public int GiveAmmo(int k)
    {
        int value;
        string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";

        using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SELECT dbo.GiveAmmo(@ammo)", connection))
            {
                command.Parameters.AddWithValue("@ammo", k);


                int valor = (int)command.ExecuteScalar();

                value = valor;
            }
        }
        return value;
    }
}