using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MageNormalAttack : MonoBehaviour {

    public float speed = 0.5f;
    public float damage;
	public float mageDmg;
	public GameObject EnemyImage;
	private GameObject enemyHealthObject;
	public Slider enemyHPSlider;
	public GameObject Mage;

	// Use this for initialization
	void Awake()
	{
		Mage = GameObject.Find ("Mage");
	
	}
	void Start () {
		//Mage.GetComponent<Movements> ().Hit();
		//EnemyImage = GameObject.Find ("EnemyImage");
		EnemyImage = Mage.GetComponent<Movements> ().EnemyImage;
		//enemyHPSlider = GameObject.Find("EnemyHealth").GetComponent<Slider>();
		enemyHPSlider = Mage.GetComponent<Movements>().enemyHPSlider;
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            Debug.Log(col.gameObject.name);
			enemyHPSlider.gameObject.SetActive(true);
			EnemyImage.SetActive (true);
			EnemyImage.GetComponent<RawImage>().GetComponent<EnemyImage>().EnemyPortrait(col.gameObject.name);
            col.gameObject.GetComponent<EnemyChar>().inflictDamage(mageDmg);      
			Debug.Log(mageDmg);
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
		mageDmg = GameObject.Find ("Mage").GetComponent<Movements> ().NormalDamage ();
	}
}
