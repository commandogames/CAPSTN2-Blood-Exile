using UnityEngine;
using System.Collections;

public class MageNormalAttack : MonoBehaviour {

    public float speed = 0.5f;
    public float damage;
	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log(col.gameObject.name);
            col.gameObject.GetComponent<EnemyChar>().inflictDamage(3);
            
        }
    }
	// Update is called once per frame
	void Update () {

        //transform.Translate(transform.forward * speed);
	}
}
