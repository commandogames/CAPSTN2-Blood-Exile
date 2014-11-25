using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyImage : MonoBehaviour {

	public RawImage EnemyIcon;
	public Texture EnemyTexture;
	public string enemyType ;

	// Use this for initialization
	void Start () {
		EnemyIcon = GetComponent<RawImage> ();
	



	}
	
	// Update is called once per frame
	void Update () {

		EnemyPortrait (enemyType);
		EnemyIcon.texture = EnemyTexture;

	}

	public void EnemyPortrait(string type)
	{
		Debug.Log (type);
		enemyType = type;

		switch (enemyType) {

		case "Enemy_Golem": //Golem
			EnemyTexture = Resources.Load<Texture>("Golem");
			break;

		case "MudGolem 1": //Taong Lupa
			EnemyTexture = Resources.Load<Texture>("MudGolem1");
			break;

		case "MudGolem 2": //Taong Lupa
			EnemyTexture = Resources.Load<Texture>("MudGolem2");
			break;
		
		case "MudGolem 3": //Taong Lupa
			EnemyTexture = Resources.Load<Texture>("MudGolem3");
			break;

		case "SpikeMushroom": //Spikes
			EnemyTexture = Resources.Load<Texture>("SpikeMushroom");
			break;

		case "PurpleMushroom": //Poison
			EnemyTexture = Resources.Load<Texture>("PoisonMushroom");
			break;

		
		}

	}
}
