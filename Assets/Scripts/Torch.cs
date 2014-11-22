using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour
{
    public GameObject TorchFlames_Particle;
    public GameObject FirstPathBoss;
    public bool TorchFlame_Active;

    // Use this for initialization
    void Start()
    {

        TorchFlames_Particle.GetComponent<ParticleSystem>().enableEmission = false;

        MeshRenderer MeshComponent = gameObject.GetComponent<MeshRenderer>();


        FirstPathBoss.renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       

        if (TorchFlame_Active == true)
        {
            TorchFlames_Particle.GetComponent<ParticleSystem>().enableEmission = true;
            FirstPathBoss.renderer.enabled = true;
         
        }

    }
    void OnTriggerEnter(Collider collision)
    {
            if (collision.gameObject.tag == "Player")
            {
                TorchFlame_Active = true;
            }
        
    }
}
