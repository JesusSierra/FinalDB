using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.Data.SqlClient;
using System;
using UnityStandardAssets.CrossPlatformInput;

public class PauseScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject audioManager;
    private AudioSource audioSource;
    private AudioSource pauseAudio;
    public AudioClip pause;
    public AudioClip pause2;
    public GameObject Components;
    public Text hammerName;
    public Text knifeName;
    public Text hammerAmmo;
    public Text knifeAmmo;
    public Text playerLifes;
    public Text playerHealth;
    public Text killedEnemies;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = audioManager.GetComponentInChildren<AudioSource>();
        pauseAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.P))
        {
            if (GameIsPaused)
            {
                Components.SetActive(false);
                Resume();
                
            }
            else
            {
                hammerName.text = "Weapon Name: " + ReturnWeaponName(1);
                knifeName.text = "Knife Name: " + ReturnWeaponName(2);
                hammerAmmo.text = "Current Ammo: ";
                knifeAmmo.text = "Current Ammo: ";
                playerLifes.text = "Lifes: ";
                playerHealth.text = "Health: ";
                killedEnemies.text = "Killed Enemies: ";

                Components.SetActive(true);
                Pause();
               


            }
        }
    }

    void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        audioSource.Play();
        pauseAudio.Stop();
        pauseAudio.PlayOneShot(pause2);
         
    }
    void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
        audioSource.Pause();
        pauseAudio.PlayOneShot(pause2);
        pauseAudio.PlayOneShot (pause);
    }
    public string ReturnWeaponName(int W)
    {
        string name;
        string conn = @"Data Source = 127.0.0.1; user id = Ness; password = memo; Initial Catalog = VideojuegoBD;";
        using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT dbo.GiveWeapon(@NW)", connection))
            {
                command.Parameters.AddWithValue("@NW", W);
                string valor = (string)command.ExecuteScalar();
                name = valor;
            }
        }
        return name;
    }

}
