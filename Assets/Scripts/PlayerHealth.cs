using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour {

	[SerializeField] int startingHealth = 100;
	[SerializeField] float timeSinceLastHit = 2.0f;
	[SerializeField] int currentHealth;
	[SerializeField] Slider healthSlider;

	private Character_Movement characterMovement;
	private float timer = 0f;
	private Animator anim;
	private AudioSource audio;
	public AudioClip hurtAudio;
	public AudioClip pickItem;
	public LevelManager lman;
	private LifeManager lifeSystem;
	private ParticleSystem particleSystem;
	public bool isDead;

	public int CurrentHealth
	{
		get { return currentHealth; }

		set
		{
			if (value < 0)
				currentHealth = 0;
			else
				currentHealth = value;

		}

	}

	void Awake()
	{
		Assert.IsNotNull(healthSlider);
		particleSystem = GetComponent<ParticleSystem>();
		particleSystem.enableEmission = false;
	}

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();
		currentHealth = startingHealth;
		characterMovement = GetComponent<Character_Movement>();
		audio = GetComponent<AudioSource>();
		lman = FindObjectOfType<LevelManager>();
		lifeSystem = FindObjectOfType<LifeManager>();
		isDead = false;
	}

	public void PlayerKill()
	{
		if(currentHealth <= 0)
		{
			print("kill");
			characterMovement.enabled = false;
			lman.RespawnPlayer();
		}
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		PlayerKill();
		
	}

	private void OnTriggerEnter(Collider other)
	{
		 if(timer >= timeSinceLastHit && !GameManager.instance.GameOver)
		{
			if(other.tag == "Weapon")
			{
				takeHit();
				timer = 0;
			}

		}
	}


	void takeHit()
	{
		if(currentHealth > 0)
		{
			GameManager.instance.PlayerHit(currentHealth);
			anim.Play("Hurt");
			currentHealth -= 10;
			healthSlider.value = currentHealth;
			audio.PlayOneShot(hurtAudio);
		}

		if (currentHealth <= 0)
		{
			GameManager.instance.PlayerHit(currentHealth);
			anim.SetTrigger("isDead");
			characterMovement.enabled = false;


		}
		

	} 

	public void PowerUpHealth()
	{
		if(currentHealth <= 80)
		{
			currentHealth += 20;

		}else if (currentHealth <= startingHealth)
		{
			CurrentHealth = startingHealth;

		}

		healthSlider.value = currentHealth;
		audio.PlayOneShot(pickItem);
	}

	public void KillBox()
	{
		CurrentHealth = 0;
		healthSlider.value = currentHealth;
	}

	public Slider HealthSlider
	{
		get {return healthSlider;}
	}
}
