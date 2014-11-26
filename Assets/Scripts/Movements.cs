﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum CharacterClass
{
	Knight,
	Fighter,
	Mage,
	Archer
}
public class Movements : MonoBehaviour
{
	public GameObject enemy;
	int ManaCost;
	public CharacterClass characterClass;
	public GameObject EnemyImage;
	public bool isLeader;
	public Animator animator;
	public int Mana;
	public Vector3 direction;
	public CharacterController characterController;
	public Vector3 moveDirection;
	public float speed, originalSpeed;
	public float gravity;
	public float rotationSpeed;
	
	public Transform target;
	
	public CharacterManager characterManager;
	
	public NavMeshAgent navmesh;
	
	public float normalAttackCooldown, normalAttackTimer;
	public int attackOrder = 0;
	
	public GameObject enemyTarget;
	
	public Manager inventoryManager;
	
	#region Tutorials
	public bool isTutorial;
	#endregion
	
	#region NAVMESH AGENT
	public float navmeshAgentDistance;
	#endregion
	
	#region Skill Set Variables for Knight
	public GameObject Sword;
	public GameObject Knight_bloodSurgeParticle;

	private bool Knight_bloodSurgeActive;

	public float Knight_Skill1Timer;

	#endregion
	
	#region Skill Set Variables for Fighter
    public GameObject Fist_PunchParticle;

    private bool Fist_PunchParticleActive;

    public float Fist_PunchTimer;
	
	#endregion
	
	#region Skill Set Variables for Mage
    public GameObject Mage_NormalAttackParticle;
	public GameObject Mage_AlleviateHealParticle;
    public GameObject Mage_GravelSwainParticle;
    public GameObject Mage_OutburstParticle;
    public GameObject Mage_NormalAttackParticleIns;
    public GameObject Mage_NormalBurst;
    public Transform WandPos;

    public Vector3 Mage_Position;

    private bool Mage_GravelSwainActive;
    private bool Mage_OutburstActive;
    private bool Mage_AlleviateHealActive;
    private bool Mage_NormalAttackActive;

    public float Mage_NormalAttackTimer;
    public float Mage_OutburstTimer;
    public float Mage_GravelSwainTimer;
    public float Mage_AlleviateHealTimer;
	#endregion

    #region Skill Set Variables for Hunter


    #endregion

	RawImage EImage;

    [SerializeField]
    float deadZone;

	public GameObject BSUi;

    void Awake()
	{
		Mana = 100;		    
		EnemyImage = GameObject.Find ("EnemyImage");
		enemyHPSlider = GameObject.Find("EnemyHealth").GetComponent<Slider>();
		enemyHealthObject = enemyHPSlider.gameObject;
		originalSpeed = speed;
		BSUi = GameObject.Find("BloodSurge");

	}

    public Transform respawnPoint;

	// Use this for initialization
	void Start()
	{
		Mana = 100; 
		BSUi.SetActive(false);
		enemyHealthObject.SetActive(false);	
		EnemyImage.SetActive (false);

		animator = GetComponent<Animator>();
		characterController = GetComponent<CharacterController>();
		characterManager = GameObject.Find("Character Manager").GetComponent<CharacterManager>();
		navmesh = GetComponent<NavMeshAgent>();
		inventoryManager = GameObject.Find("Inventory Manager").GetComponent<Manager>();
		CharType ();
		playerHp = GameObject.Find ("Health");
		
		HP = maxHp;

        respawnPoint = GameObject.Find("RespawnPoint").transform;
		//Particle Emission of Skills
		//		Mage_AlleviateHealParticle.GetComponent<ParticleSystem>().enableEmission = false;
		//		
        //		Knight_bloodSurgeParticle.GetComponent<ParticleSystem>().enableEmission = false;
        #region Particle System Stopper
        if (characterClass == CharacterClass.Knight)
			Knight_bloodSurgeParticle.GetComponent<ParticleSystem>().Stop ();

        if (characterClass == CharacterClass.Mage)
        {
            Mage_AlleviateHealParticle.GetComponent<ParticleSystem>().Stop();
            Mage_NormalAttackParticle.GetComponent<ParticleSystem>().Stop();
            Mage_GravelSwainParticle.GetComponent<ParticleSystem>().Stop();
            Mage_OutburstParticle.GetComponent<ParticleSystem>().Stop();
        }

		if (characterClass == CharacterClass.Fighter)
			Fist_PunchParticle.GetComponent<ParticleSystem>().Stop ();

        #endregion 

        WandPos = GameObject.Find("Mage_StaffOrigin").transform;
    }
	
	public bool dead = false;
	// Update is called once per frame
	void Update()
	{
		//enemyHealthObject.SetActive(false);	
		
		if (enemyHPSlider.value == 0) {
			enemyHealthObject.SetActive(false);	
			EnemyImage.SetActive(false);
		}
		
		if (skillActive) {
			//Debug.Log ("counter = " + skillCounter);
			SkillDamage(1);
		}
		
		//Testing Reviving and Removing from Char Pool
		if (isLeader)
		{
			//Debug.Log(HP);
			if (Input.GetKeyDown(KeyCode.K))
			{
				this.HP = this.maxHp;
			}
			if (Input.GetKeyDown(KeyCode.Alpha0)) // return 0 hp
			{
				HP = 0;
			}
			else
			{
				
			}
		}
		
		if (dead == false)
		{
			if (HP <= 0)
			{
				animator.SetTrigger("Dead");
				gameObject.tag = "Dead";
				characterManager.RemoveFromCharacterPool(gameObject);
				dead = true;
			}
		}
		
		if (dead == true)
		{
			if (this.HP > 0)
			{
				gameObject.tag = "Player";
				dead = false;
			}
		}
		
		if (characterManager.selectedLeader != this.gameObject)
			isLeader = false;
		else isLeader = true;
		
		
		if (inventoryManager.isEnabled == false)
		{
			if (dead == false)
			{
				Movement();
				Skills();
			}
			else
			{
				navmesh.enabled = false;
			}
		}
		
		if (isLeader)
		{
			navmesh.enabled = false;
		}
		else
		{
			if (dead)
			{
				navmesh.enabled = false;
			}
			else
			{
				navmesh.enabled = true;
			}
		}
		
		
		
		
		
		if (regenCounter < 5) {
			regenCounter += Time.deltaTime;
			//Debug.Log (regenCounter);
			
		} else {
			regenCounter = 0;
			regenhp = (HP* 0.005f) + (def * 0.002f);
			playerHp.GetComponent<PlayerHealth> ().RegenHP (regenhp);
		}



        #region Knight Particle Condition
        
        

        #endregion

        
		
	}

    [SerializeField]
    float vert, hori;

    
	void Movement()
	{

        
        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("Hurt");
        }
        vert = Input.GetAxis("ZoomIn");
        //hori = Input.GetAxis("Horizontal");
		if (isLeader)
		{
			//this.gameObject.tag = "Leader";
			
			
			AttackMovements();
			speed = originalSpeed;
			navmesh.enabled = false;
            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
			{
				direction.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			}
			else
			{
				direction.Set(0, 0, 0);
			}
			
			animator.SetFloat("Speed", direction.sqrMagnitude);
			
			
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") == false &&
			    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2") == false &&
			    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 3") == false 
			    &&
			    animator.GetCurrentAnimatorStateInfo(0).IsName("Skill 1") == false &&
			    animator.GetCurrentAnimatorStateInfo(0).IsName("Skill 2") == false &&
			    animator.GetCurrentAnimatorStateInfo(0).IsName("Skill 3") == false &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt") == false
			    )
			{
				if(characterClass == CharacterClass.Knight)
					Sword.GetComponent<TrailRenderer>().time = 0;
				
				
				if (characterController.isGrounded)
				{
					
					moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
					moveDirection *= speed;
                    if ((Input.GetAxis("Horizontal") > deadZone || Input.GetAxis("Horizontal") < -deadZone) || (Input.GetAxis("Vertical") > deadZone || Input.GetAxis("Vertical") < -deadZone))
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
				}
				moveDirection.y -= gravity;
			}
			else //NOT ATTACKING
			{
				#region Knight  //knight attributes
				if (characterClass == CharacterClass.Knight)
				{
					Sword.GetComponent<TrailRenderer>().time = 0.3f;
                    //Sword.transform.GetChild(1).GetComponent<XWeaponTrailDemo>().enabled = false;
				}
				#endregion
				
				moveDirection = new Vector3(0, 0, 0);
				speed = 0;
                //if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                //{
                //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10.0f);
                //}
			}
			characterController.Move(moveDirection * Time.deltaTime);
		}
		
		else//NOT LEADER *********************
		{
			//this.gameObject.tag = "Player";
			navmesh.enabled = true;
			float dist = Vector3.Distance(this.transform.position, characterManager.selectedLeader.transform.position);
			
			if (dist >= 10.0f)
			{
				navmesh.stoppingDistance = 5.0f;
                if(dead == false)
				    navmesh.SetDestination(characterManager.selectedLeader.transform.position);
			}
			else //search for enemies using Physics.OverlapSphere
			{
				SearchForEnemies();
			}
			//---
			
			if (navmesh.velocity.magnitude == 0)
			{
				animator.SetFloat("Speed", 0);
				
			}
			
			else if(navmesh.velocity.sqrMagnitude > 1)
			{
				animator.SetFloat("Speed", 1);
				//transform.rotation = Quaternion.LookRotation(characterManager.selectedLeader.transform.position * Time.deltaTime * 5.0f);
			}
		}
	}
	
	void Skills()
	{
		#region Knight Skills
		if (characterClass == CharacterClass.Knight)
		{
			//skill conditions here
			//sample skill
			
			if (isLeader)
			{

                // 1st Skill
                if (Knight_bloodSurgeActive == true)
                {

                    Knight_Skill1Timer += Time.deltaTime;
                    if (Knight_Skill1Timer >= 20f)
                    {
                        Knight_bloodSurgeActive = false;
                    }
                }

                if (Knight_bloodSurgeActive == false)
                {
                    Knight_bloodSurgeParticle.GetComponent<ParticleSystem>().Stop();
                    Knight_Skill1Timer = 0f;
                }

				if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("Skill 1")) ///skill 1
				{
					ManaCost = 30;
					if(Mana >= ManaCost)
					{
					Knight_bloodSurgeParticle.GetComponent<ParticleSystem>().Play();
					animator.SetTrigger("Skill 1");
					Knight_bloodSurgeActive = true;
					Mana -= ManaCost;
					skillActive = true;
					}
					
					
				}
				if (Input.GetKeyDown(KeyCode.Alpha2)) ///skill 2
				{
					SkillAttack(this.transform.position, 0.9f, 2);
					animator.SetTrigger("Skill 2");
				}
				if (Input.GetKeyDown(KeyCode.Alpha3)) ///skill 3
				{
					SkillAttack(this.transform.position, 0.9f, 3);
					animator.SetTrigger("Skill 3");
				}
			}
			//Knight Particle Condition

		}
		#endregion
		
		#region Fighter Skills
		else if (characterClass == CharacterClass.Fighter)
		{
			//skill conditions here
			if (isLeader)
			{
                #region Fighter Particle Condition

                //Normal Attack
                if (Fist_PunchParticleActive == true)
                {
                    Fist_PunchTimer += Time.deltaTime;

                    if (Fist_PunchTimer >= 1.5f)
                    {
                        Fist_PunchParticleActive = false;
                    }
                }

                if (Fist_PunchParticleActive == false)
                {
                    Fist_PunchParticle.GetComponent<ParticleSystem>().Stop();
                    Fist_PunchTimer = 0f;
                }
                #endregion 

				if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("Skill 1")) ///skill 1
				{
					//Debug.Log("Skill 1");
					SkillAttack(this.transform.position, 0.9f, 4);
					animator.SetTrigger("Skill 1");
				}
				if (Input.GetKeyDown(KeyCode.Alpha2)) ///skill 2
				{
					SkillAttack(this.transform.position, 0.9f, 5);
					animator.SetTrigger("Skill 2");
				}
				if (Input.GetKeyDown(KeyCode.Alpha3)) ///skill 3
				{
					SkillAttack(this.transform.position, 0.9f, 6);
					animator.SetTrigger("Skill 3");
				}
			}
		}
		#endregion
		
		#region Mage Skills
		else if (characterClass == CharacterClass.Mage)
		{
			//skill conditions here
			if (isLeader)
			{
                #region Mage Particle Condition

                //Normal Attack
                if (Mage_NormalAttackActive == true)
                {
                    Mage_NormalAttackTimer += Time.deltaTime;

                    if (Mage_NormalAttackTimer >= 0.8f)
                    {
                        Mage_NormalAttackActive = false;
                        //  Mage_NormalAttackTimer = 0f;
                    }
                }

                if (Mage_NormalAttackActive == false)
                {
                    Mage_NormalAttackParticle.GetComponent<ParticleSystem>().Stop();
                    Mage_NormalAttackTimer = 0f;
                }

                //1st Skill
                if (Mage_GravelSwainActive == true)
                {
                    Mage_GravelSwainTimer += Time.deltaTime;

                    if (Mage_GravelSwainTimer >= 4f)
                    {
                        Mage_GravelSwainActive = false;
                    }
                }

                if (Mage_GravelSwainActive == false)
                {
                    Mage_GravelSwainParticle.GetComponent<ParticleSystem>().Stop();
                    Mage_GravelSwainTimer = 0f;
                }

                //2nd Skill
                if (Mage_OutburstActive == true)
                {
                    Mage_OutburstTimer += Time.deltaTime;

                    if (Mage_OutburstTimer >= 10f)
                    {
                        Mage_OutburstActive = false;
                    }
                }

                if (Mage_OutburstActive == false)
                {
                    Mage_OutburstParticle.GetComponent<ParticleSystem>().Stop();
                    Mage_OutburstTimer = 0f;
                }

                //3rd Skill
                if (Mage_AlleviateHealActive == true)
                {

                    Mage_AlleviateHealTimer += Time.deltaTime;

                    if (Mage_AlleviateHealTimer >= 5f)
                    {
                        Mage_AlleviateHealActive = false;
                    }
                }

                if (Mage_AlleviateHealActive == false)
                {
                    Mage_AlleviateHealParticle.GetComponent<ParticleSystem>().Stop();
                    Mage_AlleviateHealTimer = 0f;
                }

                #endregion
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("Skill 1"))///skill 1
				{
                    Mage_GravelSwainParticle.GetComponent<ParticleSystem>().Play();
					SkillAttack(this.transform.position, 0.9f, 7);
					animator.SetTrigger("Skill 1");
                    Mage_GravelSwainActive = true;	
				}
				if (Input.GetKeyDown(KeyCode.Alpha2)) ///skill 2
				{
                    Mage_OutburstParticle.GetComponent<ParticleSystem>().Play();
					SkillAttack(this.transform.position, 5, 8);
					animator.SetTrigger("Skill 2");
                    Mage_OutburstActive = true;
				}
				if (Input.GetKeyDown(KeyCode.Alpha3)) ///skill 3
				{
                    Mage_AlleviateHealParticle.GetComponent<ParticleSystem>().Play();
					SkillAttack(this.transform.position, 0.9f, 9);
					animator.SetTrigger("Skill 3");
                    Mage_AlleviateHealActive = true;
				}
			}
		}
		#endregion
		
		#region Hunter Skills
		else if (characterClass == CharacterClass.Archer)
		{
			//skill conditions here
			if (isLeader)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1)) ///skill 1
				{
					animator.SetTrigger("Skill 1");
				}
				if (Input.GetKeyDown(KeyCode.Alpha2)) ///skill 2
				{
					animator.SetTrigger("Skill 2");
				}
				if (Input.GetKeyDown(KeyCode.Alpha3)) ///skill 3
				{
					animator.SetTrigger("Skill 3");
				}
			}
		}
		#endregion
		
		else { }
		
	}

    IEnumerator mageNormalAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject burst = (GameObject)Instantiate(Mage_NormalBurst, WandPos.transform.position, transform.rotation);
        burst.AddComponent<MoveForward>();
        Destroy(burst, 0.3f);
    }
	void AttackMovements()
	{
		#region Fighter Attacks
		if (characterClass == CharacterClass.Fighter)
		{
			if (isLeader)
			{
				normalAttackTimer += Time.deltaTime;
				if (Input.GetButtonDown("Attack") || Input.GetKeyDown(KeyCode.G))
				{
					
					Fist_PunchParticle.GetComponent<ParticleSystem>().Play();
                    Fist_PunchParticleActive = true;
					InflictDamage(100, this.transform.position, 0.9f);
					animator.SetFloat("Fighter_AttackCooldown", normalAttackTimer);
					animator.SetInteger("Fighter_AttackCombo", attackOrder);
					attackOrder += 1;
					if (attackOrder >= 3)
					{
						attackOrder = 0;
						normalAttackTimer = 0;
					}
					
					if (attackOrder != 0)
					{
						if (normalAttackTimer >= 3.0f)
						{
							attackOrder = 0;
							normalAttackTimer = 0;
						}
					}
					
					
					animator.SetTrigger("Attack");
				}
			}
			else
			{
				normalAttackTimer = 0;
				attackOrder = 0;
			}
		}
		#endregion
		
		#region Knight Attacks
		if (characterClass == CharacterClass.Knight)
		{
			if (isLeader)
			{
				normalAttackTimer += Time.deltaTime;
				if (Input.GetButtonDown("Attack") || Input.GetKeyDown(KeyCode.G))
				{
					InflictDamage(100, this.transform.position, 1.0f);
					animator.SetFloat("Fighter_AttackCooldown", normalAttackTimer);
					animator.SetInteger("Fighter_AttackCombo", attackOrder);
					attackOrder += 1;
					if (attackOrder >= 3)
					{
						attackOrder = 0;
						normalAttackTimer = 0;
					}
					
					if (attackOrder != 0) //reset starting combo
					{
						if (normalAttackTimer >= 3.0f)
						{
							attackOrder = 0;
							normalAttackTimer = 0;
						}
					}
					animator.SetTrigger("Attack");
				}
			}
			else
			{
				normalAttackTimer = 0;
				attackOrder = 0;
			}
		}
		
		#endregion
		
		#region Mage Attacks
		if (characterClass == CharacterClass.Mage)
		{
			if (isLeader)
			{
				normalAttackTimer += Time.deltaTime;
				if (Input.GetButtonDown("Attack") || Input.GetKeyDown(KeyCode.G))
				{
                    //StartCoroutine(mageNormalAttack(0.6f));
                    GameObject burst = (GameObject)Instantiate(Mage_NormalAttackParticle, WandPos.transform.position, transform.rotation);
                    Destroy(burst, 1.9f);

                    //Mage_NormalAttackParticle.GetComponent<ParticleSystem>().Play();
                    //Mage_NormalAttackActive = true;
                   
                    //Mage_Position = transform.position;
                //    Instantiate(Mage_NormalAttackParticleIns, Mage_Position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
					//Debug.Log("GG");
					InflictDamage(100, this.transform.position, 0.9f);
					animator.SetFloat("Fighter_AttackCooldown", normalAttackTimer);
					animator.SetInteger("Fighter_AttackCombo", attackOrder);
					attackOrder += 1;
					if (attackOrder >= 3)
					{
						attackOrder = 0;
						normalAttackTimer = 0;
					}
					
					if (attackOrder != 0) //reset starting combo
					{
						if (normalAttackTimer >= 3.0f)
						{
							attackOrder = 0;
							normalAttackTimer = 0;
						}
					}
					animator.SetTrigger("Attack");
				}
			}
			else
			{
				normalAttackTimer = 0;
				attackOrder = 0;
			}
		}
		#endregion
		
		#region Hunter Attacks
		if (characterClass == CharacterClass.Archer)
		{
			if (isLeader)
			{
				normalAttackTimer += Time.deltaTime;
				if (Input.GetButtonDown("Attack") || Input.GetKeyDown(KeyCode.G))
				{
					//Debug.Log("GG");
					InflictDamage(100, this.transform.position, 0.9f);
					animator.SetFloat("Fighter_AttackCooldown", normalAttackTimer);
					animator.SetInteger("Fighter_AttackCombo", attackOrder);
					attackOrder += 1;
					if (attackOrder >= 3)
					{
						attackOrder = 0;
						normalAttackTimer = 0;
					}
					
					if (attackOrder != 0) //reset starting combo
					{
						if (normalAttackTimer >= 3.0f)
						{
							attackOrder = 0;
							normalAttackTimer = 0;
						}
					}
					animator.SetTrigger("Attack");
				}
			}
			else
			{
				normalAttackTimer = 0;
				attackOrder = 0;
			}
		}
		#endregion
	}
	
	void SearchForEnemies()
	{
		GameObject newTarget = null;
		Collider[] col = Physics.OverlapSphere(transform.position, 5.0f);
		foreach (Collider collide in col)
		{
			if(collide.gameObject.tag == "Enemy")
				newTarget = collide.gameObject;
		}
		
		if (newTarget != null)
		{
			enemyTarget = newTarget.gameObject;
			navmesh.stoppingDistance = 2.0f;
			navmesh.SetDestination(enemyTarget.transform.position);
		}
	}

	void InflictDamage(float dmg, Vector3 center, float radius)
	{
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);
		foreach (Collider collide in hitColliders)
		{
			if (collide.gameObject.tag == "Dummy")
				collide.transform.gameObject.GetComponent<Tutorial_DummyScript>().InflictDamage(100);
			
			
			if (collide.gameObject.tag == "Enemy"){

				enemyHealthObject.SetActive(true);
				NormalDamage();
				EnemyImage.SetActive (true);
				EnemyImage.GetComponent<RawImage>().GetComponent<EnemyImage>().EnemyPortrait(collide.gameObject.name);

				if(collide.gameObject.name == "MudGolem 1")
				{
					collide.transform.gameObject.GetComponent<MudGolem1>().inflictDamage(dmgAtk);
					enemyHPSlider.value =  collide.transform.gameObject.GetComponent<MudGolem1>().HP;
				}

				else if(collide.gameObject.name == "MudGolem 2(Clone)")
				{
					collide.transform.gameObject.GetComponent<MudGolem2>().inflictDamage(dmgAtk);
					enemyHPSlider.value =  collide.transform.gameObject.GetComponent<MudGolem2>().HP;
				}

				else if(collide.gameObject.name == "MudGolem 3(Clone)")
				{
					collide.transform.gameObject.GetComponent<MudGolem3>().inflictDamage(dmgAtk);
					enemyHPSlider.value =  collide.transform.gameObject.GetComponent<MudGolem3>().HP;
				}

				else{
				collide.transform.gameObject.GetComponent<EnemyChar>().inflictDamage(dmgAtk);
				enemyHPSlider.value =  collide.transform.gameObject.GetComponent<EnemyChar>().HP;
				//Debug.Log("DAMAGE: " + enemyHPSlider.value);
				}

				



			}		
				            		
				        }		
			    }		
				
			    void SkillAttack(Vector3 center, float radius, int skill)		
		    {		
			        Collider[] hitColliders = Physics.OverlapSphere(center, radius);		
			        foreach (Collider collide in hitColliders)		
			        {		

		
				            if (collide.gameObject.tag == "Dummy")		
					                collide.transform.gameObject.GetComponent<Tutorial_DummyScript>().InflictDamage(100);		
				            		
					            		
				            if (collide.gameObject.tag == "Enemy"){	
							
								
						                enemyHealthObject.SetActive(true);		
					                EnemyImage.SetActive (true);		
									
						                SkillDamage(skill);		
					                		
						                if(collide.gameObject.name == "MudGolem 1")		
						                {		
							                    collide.transform.gameObject.GetComponent<MudGolem1>().inflictDamage(skillDmg);		
							                    enemyHPSlider.value = collide.transform.gameObject.GetComponent<MudGolem1>().HP;		
							                }		
					                		
						                else if(collide.gameObject.name == "MudGolem 2(Clone)")		
						                {		
							                    collide.transform.gameObject.GetComponent<MudGolem2>().inflictDamage(skillDmg);		
							                    enemyHPSlider.value = collide.transform.gameObject.GetComponent<MudGolem2>().HP;		
							                }		
					                		
						                else if(collide.gameObject.name == "MudGolem 3(Clone)")		
						                {		
							                    collide.transform.gameObject.GetComponent<MudGolem3>().inflictDamage(skillDmg);		
							                    enemyHPSlider.value = collide.transform.gameObject.GetComponent<MudGolem3>().HP;		
							                }		
					                		
					                else{		
						                    collide.transform.gameObject.GetComponent<EnemyChar>().inflictDamage(skillDmg);		
						                    enemyHPSlider.value = collide.transform.gameObject.GetComponent<EnemyChar>().HP;		
						                    //Debug.Log("DAMAGE: " + enemyHPSlider.value);		
						                }

				//collide.transform.gameObject.GetComponent<EnemyMovement>().Die();
				
				
				
			}


			
		}
	}
	
	//------------------------------------------------------------------------------------Damage Calculation-----------------------------------------------------------------------
	public float HP;
	public float baseDmg = 0f;
	public float magicDmg;
	public float agi;
	public float def;
	
	public bool skillActive;
	
	float skillCounter;
	
	public float maxHp;
	
	public Slider enemyHPSlider;
	private GameObject enemyHealthObject;
	public GameObject playerHp;
	
	float regenCounter = 0;
	public float wepDmg;
	public float dmgSkill;
	public float dmgAtk;
	public float skillType;
	public float skillDmg;
	public float skillTotal;
	public float currentDmg ;
	
	public int skills = 2;
	public int type = 1;
	
	public int level = 1; 
	
	public float regenhp;
	
	void CharType()
	{
		//Class
		switch (type) {
		case 1: //Knight
		{
			maxHp = 420;
			baseDmg = 7;
			magicDmg = 4;
			def = 9;
			agi = 5;
			
			currentDmg = baseDmg;
			
			break;
		}
			
		case 2: //Fist
		{
			maxHp = 380;
			baseDmg = 9;
			magicDmg = 3;
			def = 6;
			agi = 7;
			
			currentDmg = baseDmg;
			
			break;
		}
			
		case 3: //Mage
		{
			maxHp = 290;
			baseDmg = 5;
			magicDmg = 10;
			def = 4;
			agi = 6;
			
			currentDmg = baseDmg;
			
			break;
		}
			
		case 4: //Hunter
		{
			maxHp = 355;
			baseDmg = 6;
			magicDmg = 6;
			def = 5;
			agi = 8;
			
			currentDmg = baseDmg;
			
			break;	
		}
		}
	}
	
	public float SkillDamage(int skilltype)
	{	int skill = skilltype;
		switch (skill) 
		{
			//Knight Skills
		case 1: //Blood Surge
		{	
			BSUi.SetActive(true);
			skillCounter += Time.deltaTime;
			
			if(skillCounter < 1)
			{
				baseDmg = currentDmg + (baseDmg * 0.20f);
			}
			
			if(skillCounter > 20)
			{
				baseDmg = currentDmg;
				skillCounter = 0;
				skillActive = false;
				BSUi.SetActive(false);
			}
			break;
		}
		case 2: //Cyclone Pass
		{
			skillDmg = 70 + (agi * 0.25f);
			break;
		}
		case 3: //Defile Slash
		{
			skillDmg = 150 + (magicDmg * 0.30f) + (agi * 0.50f);
			break;
		}
			
			//Fist Skills
		case 4: //Alacrity Spritz
		{
			skillDmg = 50 + (agi * 0.28f);
			break;
		}
		case 5: //Dynamic Jab
		{
			skillDmg = 65 + (magicDmg * 0.15f);
			break;
		}
		case 6: //Myriad Strike
		{
			skillDmg = 105 + (magicDmg * 0.40f) + (agi * 0.45f);
			break;
		}
			
			//Mage Skills
		case 7: //Gravel Swain
		{
			//Summon Golem
			break;
		}
		case 8: //Outburst
		{
			skillDmg = 80 + (magicDmg * 0.20f) + (agi * 0.09f);
			break;
		}
		case 9: //Alleviate Heal
		{
			//Heal 		
			break;
		}
			
			//Hunter Skills
		case 10: //Daze Arrow
		{
			skillDmg = 30 + (magicDmg * 0.20f);
			break;
		}
		case 11: //Shaft
		{
			skillDmg = 60 + (agi * 0.35f);
			break;
		}
		case 12: //Rain Shower
		{
			skillDmg = 90 + (magicDmg * 0.35f) + (agi * 0.55f);
			break;
		}
			
		}
		
		dmgSkill = ((level + 2) * magicDmg + skillDmg)/4;
		return skillDmg;
		//Debug.Log ("Skill Damage " + dmgSkill);
	}
	
	void RegenHP()
	{
		regenCounter += Time.deltaTime;
		
		if ( regenCounter < 5 )
		{
			regenhp = (HP* 0.05f) + (def * 0.02f);
		}
		playerHp.GetComponent<PlayerHealth> ().RegenHP (regenhp);
		regenCounter = 0;
		
	}
	
	public void Hurt(float dmg)
	{
		HP -= dmg;
		//animator.SetTrigger("Hurt");
	}
	
	public void NormalDamage()
	{
		dmgAtk = ((level + 2)* 2 +baseDmg)/ 4 ;
		//Debug.Log ("Normal Damage : " + dmgAtk);
		
	}
}