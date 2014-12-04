using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FighterSkill2 : MonoBehaviour {
	
	public float speed = 0.5f;
	public float damage;
	public float fighterskilldmg;
	public GameObject EnemyImage;
	private GameObject enemyHealthObject;
	public Slider enemyHPSlider;
	public GameObject Fighter;
	
	// Use this for initialization
	void Awake()
	{
		Fighter = GameObject.Find ("Fighter");
		
	}
	void Start () {
		//Mage.GetComponent<Movements> ().Hit();
		//EnemyImage = GameObject.Find ("EnemyImage");
		EnemyImage = Fighter.GetComponent<Movements> ().EnemyImage;
		//enemyHPSlider = GameObject.Find("EnemyHealth").GetComponent<Slider>();
		enemyHPSlider = Fighter.GetComponent<Movements>().enemyHPSlider;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Enemy")
		{
			enemyHPSlider.gameObject.SetActive(true);
			EnemyImage.SetActive (true);
			EnemyImage.GetComponent<RawImage>().GetComponent<EnemyImage>().EnemyPortrait(col.gameObject.name);
			col.gameObject.GetComponent<EnemyChar>().inflictDamage(fighterskilldmg);      
			enemyHPSlider.value = col.transform.gameObject.GetComponent<EnemyChar>().HP;
            Destroy(gameObject);
		}
		
	}	
	
	// Update is called once per frame
	void Update () {
		/*
		if (enemyHPSlider.value == 0) {
			enemyHealthObject.SetActive(false);	
			EnemyImage.SetActive(false);
		}

		*/
		//transform.Translate(transform.forward * speed);
		fighterskilldmg = GameObject.Find ("Fighter").GetComponent<Movements> ().SkillDamage(5);
	}
}
