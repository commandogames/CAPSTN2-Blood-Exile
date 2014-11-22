using UnityEngine;
using System.Collections;

public class FallOff : MonoBehaviour {

    public CharacterManager cM;
	// Use this for initialization
	void Start () {

        cM = GameObject.Find("Character Manager").GetComponent<CharacterManager>();
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log(this.gameObject.name);
            if (col.gameObject.GetComponent<Movements>().characterClass ==  CharacterClass.Knight)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            else
            {
                cM.RemoveFromCharacterPool(col.gameObject);  
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
