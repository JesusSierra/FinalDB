using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public float timer = 0f;
    public float waitTime =2.0f;
    public GameObject currentCheckpoint;
    private GameObject player;
    private PlayerHealth playerHealth;
    public PlayerHealth playerSlider;
    private Character_Movement characterMovement;
    public Animator anim;
    private LifeManager lifeSystem;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        playerHealth = player.GetComponent<PlayerHealth>();
        currentCheckpoint = GameManager.instance.gameObject;
        playerSlider = player.GetComponent<PlayerHealth>();
        characterMovement = player.GetComponent<Character_Movement>();
        anim = player.GetComponent<Animator>();
        lifeSystem = FindObjectOfType<LifeManager>();
        
    }

    public void RespawnPlayer()
    {
        print ("Player Respawn");
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            lifeSystem.TakeLife();
            player.transform.position = currentCheckpoint.transform.position;
            playerHealth.CurrentHealth = 100;
            timer = 0f;
            playerHealth.HealthSlider.value = playerHealth.CurrentHealth;
            characterMovement.enabled = true;
            anim.Play("Blend Tree");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
