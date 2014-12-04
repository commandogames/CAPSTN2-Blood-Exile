using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KnightSkill2 : MonoBehaviour {
	
	public float speed = 0.5f;
	public float damage;
	public float knightskilldmg;
	public GameObject EnemyImage;
	private GameObject enemyHealthObject;
	public Slider enemyHPSlider;
	public GameObject Knight;
	
	// Use this for initialization
	void Awake()
	{
		Knight = GameObject.Find ("Knight");
		
	}
	void Start () {
		//Mage.GetComponent<Movements> ().Hit();
		//EnemyImage = GameObject.Find ("EnemyImage");
		EnemyImage = Knight.GetComponent<Movements> ().EnemyImage;
		//enemyHPSlider = GameObject.Find("EnemyHealth").GetComponent<Slider>();
		enemyHPSlider = Knight.GetComponent<Movements>().enemyHPSlider;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Enemy")
		{
			enemyHPSlider.gameObject.SetActive(true);
			EnemyImage.SetActive (true);
			EnemyImage.GetComponent<RawImage>().GetComponent<EnemyImage>().EnemyPortrait(col.gameObject.name);
			col.gameObject.GetComponent<EnemyChar>().inflictDamage(knightskilldmg);      
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
		knightskilldmg = GameObject.Find ("Knight").GetComponent<Movements> ().SkillDamage(2);
	}
}
