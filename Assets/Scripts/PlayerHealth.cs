using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public int startingHealth = 100;
	public float currentHealth;
	public Slider healthSlider;
	public Image damageImage;
	public AudioClip deathClip;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public GameObject Player;
	
	
	Animator anim;
	AudioSource playerAudio;
	
	//PlayerShooting playerShooting;
	bool isDead;
	bool damaged;
	
	
	void Awake ()
	{
		anim = GetComponent <Animator> ();
		
		playerAudio = GetComponent <AudioSource> ();
		
		//playerShooting = GetComponentInChildren <PlayerShooting> ();
		currentHealth = startingHealth;
		//currentHealth = 
	}
	
	void Start()
	{
		Player = GameObject.Find ("Character Manager").GetComponent<CharacterManager> ().selectedLeader;
		healthSlider.maxValue = Player.GetComponent<Movements>().maxHp;
	}
	
	void Update ()
	{
		
		//healthSlider.maxValue = 150.05f;
		//Player = GameObject.FindGameObjectWithTag("Leader");
		
		//healthSlider.maxValue = Player.GetComponent<Movements> ().maxHp;
		
		
		Player = GameObject.Find("Character Manager").GetComponent<CharacterManager>().selectedLeader;
		//Player = gameObject.GetComponent<CharacterManager>().selectedLeader;
		healthSlider.maxValue = Player.GetComponent<Movements> ().maxHp;
		healthSlider.value = currentHealth;
		
		
		currentHealth = Player.GetComponent<Movements>().HP;
		
		
		/*
		if(damaged)
		{
			damageImage.color = flashColour;
		}
		else
		{
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		*/
		damaged = false;
		
		if (Input.GetKeyDown (KeyCode.B)) {
			//TakeDamage(20);
			Player.GetComponent<Movements>().Hurt(20);
		}
	}
	
	
	public void TakeDamage (float amount)
	{
		
		damaged = true;
		
		currentHealth -= amount;
		
		healthSlider.value = currentHealth;
		
		//playerAudio.Play ();
		
		if(currentHealth <= 0 && !isDead)
		{
			Death ();
		}
	}
	
	public void RegenHP(float amount)
	{
		/*
		if ((currentHealth + amount) < healthSlider.maxValue) {
						currentHealth += amount; 
						healthSlider.value = currentHealth;
		} else if ((currentHealth + amount) > healthSlider.maxValue) { 
				healthSlider.value = healthSlider.maxValue;
		}
		*/
	}
	
	
	void Death ()
	{
		isDead = true;
		
		//playerShooting.DisableEffects ();
		
		anim.SetTrigger ("Die");
		
		playerAudio.clip = deathClip;
		playerAudio.Play ();
		
		
		//playerShooting.enabled = false;
	}
}
