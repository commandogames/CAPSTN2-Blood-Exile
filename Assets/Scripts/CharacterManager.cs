using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{

    public GameObject[] Characters;
    public int leader_indexer = 0;
    public GameObject selectedLeader;
    public GameObject indicator;
    public List<GameObject> myChars = new List<GameObject>();

    public List<GameObject> deadCharacaters = new List<GameObject>();
    public int mycharscount;
    public bool allDead = false;
    // Use this for initialization
    void Start()
    {

    }


    public void RemoveFromCharacterPool(GameObject character) //patayin
    {
        if (myChars.Count >= 1)
        {
            //moves selected leader to dead character pool
            character.GetComponent<Movements>().navmesh.enabled = false;
            character.GetComponent<Movements>().dead = true;
            character.GetComponent<Movements>().HP = 0;
            if (character == myChars[myChars.Count - 1])
            {
                leader_indexer = 0;
            }
            deadCharacaters.Add(character);
            myChars.Remove(character);
            
        }

    }

    public void ReturnToCharacterPool() //ibalik
    {
        Debug.Log(deadCharacaters[0].name.ToString());
        deadCharacaters[0].GetComponent<Movements>().HP = deadCharacaters[0].GetComponent<Movements>().maxHp;
        deadCharacaters[0].GetComponent<Movements>().dead = false;
        deadCharacaters[0].GetComponent<Movements>().navmesh.enabled = true;
        //returns latest player who died back to character selection
        myChars.Add(deadCharacaters[0]);
        deadCharacaters.Remove(deadCharacaters[0]);
        

    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Application.LoadLevel(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Application.LoadLevel(1);
        }

        //selectedLeader = Characters[leader_indexer];
        selectedLeader = myChars[leader_indexer];

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Change Leader"))
        {
            if (myChars.Count > 1)
                leader_indexer += 1;
        }

        if (leader_indexer > myChars.Count - 1)
            leader_indexer = 0;

        indicator.transform.position = selectedLeader.transform.position + new Vector3(0, 3, 0);
        indicator.transform.rotation = Quaternion.identity;

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {

        }

        if (myChars.Count == 0)
            allDead = true;
        if (allDead) Debug.Log("GG");

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            RemoveFromCharacterPool(selectedLeader);
        }

        if (Input.GetKeyDown(KeyCode.R)) //revive last person who died
        {
            ReturnToCharacterPool();

        }

        mycharscount = myChars.Count;
    }
}
