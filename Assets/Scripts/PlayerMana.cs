using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMana : MonoBehaviour
{
	public int startingMana = 100;
	
	public int currentMana;
	public Slider manaSlider;
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
		manaSlider = GetComponent<Slider>();
		//playerShooting = GetComponentInChildren <PlayerShooting> ();
		currentMana = startingMana;
		//currentHealth = 
	}
	
	void Start()
	{
		Player = GameObject.Find ("Character Manager").GetComponent<CharacterManager> ().selectedLeader;
		manaSlider.maxValue = Player.GetComponent<Movements> ().Mana;
	}
	
	void Update ()
	{
		
		//healthSlider.maxValue = 150.05f;
		//Player = GameObject.FindGameObjectWithTag("Leader");
		
		//healthSlider.maxValue = Player.GetComponent<Movements> ().maxHp;
		
		
		Player = GameObject.Find("Character Manager").GetComponent<CharacterManager>().selectedLeader;
		//Player = gameObject.GetComponent<CharacterManager>().selectedLeader;
		//manaSlider.maxValue = Player.GetComponent<Movements> ().Mana;
		manaSlider.value = currentMana;
		currentMana = Player.GetComponent<Movements>().Mana;
		
		
		
		if (Input.GetKeyDown (KeyCode.B)) {
			//TakeDamage(20);
			Player.GetComponent<Movements>().Hurt(20);
		}
	}
	
	
	
	
	public void RegenMana(float amount)
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
	
	
}
