using UnityEngine;
using System.Collections;

public class AI_Enemy : MonoBehaviour {
	
	private GameObject playerOne;
	//public GameObject mainBase;
	private Transform sexyLady;
	private float rotX;
	public bool isLookingUp = false;
	
	public Transform weaponMainPosition;
	private Transform weaponMainRotation;

	public GameObject enemyBrain;
	private GameObject radar;
	public EnemyShip _EnemyShip;
	Vector3 adjust_global_z_position;
	private float distanceBetweenEnemyAndPlayer;
	
	protected float radarZPosition;
	
	private float adjustPositionDuration;
	private float adjustPositionSeconds;
	private float timeToReAdjustPositionDuration;
	private float timeToReAdjustPositionSeconds;
	
	
	private bool moveLeft;
	private bool moveRight;
	private bool moveForward;
	private bool moveBackward;
	private bool retreat;
	
	private int isChaseEnabled;
	private float chaseDuration;
	private float chaseSeconds;
	private bool isChaseTimedEnabled;
	private float chaseTimedDuration;
	private float resetChaseSeconds;
	private int higherChaseFrequence;
	private int defaultChaseFrequency;
	
	
	private float fireWeaponDuration;
	private float fireWeaponSeconds;
	
	private float engageDistance;
	
	
	
	public float GetRadarZPosition
	{
		get { return radarZPosition;}
	}
	
	void Awake()
	{
		moveLeft = false;
		moveRight = false;
		moveForward = false;
		moveRight = false;
		retreat = false;
	}
	
	void Start () {
		
		weaponMainRotation = enemyBrain.transform;

		engageDistance = 300f;
		playerOne = GameObject.Find("Spaceship");
		sexyLady = playerOne.transform;
		TurnToEnemy();

		ReAdjustPosition();
		timeToReAdjustPositionDuration = Random.Range(1f,7f);
		timeToReAdjustPositionSeconds = 0;
		
		defaultChaseFrequency = 8;
		ResetChase();
		
		radar = new GameObject("Radar");
		radar.transform.parent = enemyBrain.transform;
		Vector3 adjust_radar_position = enemyBrain.transform.position;
		radarZPosition = 6.0f;
		adjust_radar_position.z = radarZPosition;
		radar.AddComponent<BoxCollider>().name = "Radar";
		BoxCollider radar_collider =  radar.GetComponent<BoxCollider>();
	
		radar_collider.isTrigger = true;
		BoxCollider enemybrain_collider = enemyBrain.GetComponent<BoxCollider>();
		
		Vector3 radar_size = enemybrain_collider.size;
		radar_size *= Random.Range(1.2f,4f);
			
		radar_collider.size = radar_size;
		radar_collider.transform.rotation = enemyBrain.transform.rotation;
		radar_collider.transform.position =	adjust_radar_position;
		
		GetAngleOfLineBetweenTwoPoints(gameObject.transform.position,sexyLady.position);
		gameObject.transform.rotation = Quaternion.Euler(rotX,270,90);
	}
	
	void Update()
	{
		
		if(Globals.isReady)
		{
			distanceBetweenEnemyAndPlayer = Vector3.Distance(playerOne.transform.position,enemyBrain.transform.position);
	
			
			AdjustPosition();
			
			Retreat();
			if(retreat)
			{
				float distance_to_spawn_point = Vector3.Distance(_EnemyShip.spawnPoint.root.transform.position,gameObject.transform.position);
				
				if(distance_to_spawn_point < 10f)
				{
					TurnToEnemy();
					_EnemyShip.Spawn();
					ResetChase();
					ReAdjustPosition();
					ResetLife();
					
					retreat = false;
				}			
				
			}
			
			
			if(timeToReAdjustPositionSeconds < timeToReAdjustPositionDuration)
			{
				ReAdjustPosition();
			}else
			{
				timeToReAdjustPositionSeconds += Time.deltaTime;
				
			}
			
			if(distanceBetweenEnemyAndPlayer > engageDistance)
			{
				GetAngleOfLineBetweenTwoPoints(gameObject.transform.position,sexyLady.position);
				gameObject.transform.rotation = Quaternion.Euler(rotX,270,90);
				
				if(!retreat && isChaseEnabled == 3)
				{
									
					if(chaseSeconds < chaseDuration)
					{
						TurnToEnemy();
						MoveForward();
						FireWeapon(2f);
					}else
					{
						
						ResetChase();
					}
					
					
						chaseSeconds += Time.deltaTime;		
				}
				
				if(!retreat && isChaseTimedEnabled && isChaseEnabled != 3)
				{
					
					if(resetChaseSeconds > chaseTimedDuration)
					{
						ResetChase(higherChaseFrequence);
						resetChaseSeconds = 0f;	
			
					}else
					{
						resetChaseSeconds += Time.deltaTime;
					}
				}
			}else
			{
				if(enemyBrain.renderer.isVisible)
				{
					FireWeapon(0.2f);
				}
				
				MoveRight();
			}
		
		}
	}
	
		
	
	void OnBecameVisible()
	{
		ReAdjustPosition();
		ResetChaseTime(4f);
		
		
	}
	
	void OnBecameInvisible()
	{
		ReAdjustPosition();
		ResetChaseTime(8,.5f);
	}
	
	void OnTriggerStay(Collider wtf_is_in_my_way)
	{
		if(wtf_is_in_my_way.name.Equals("Radar"))
		{
			
			if(wtf_is_in_my_way.transform.position.x < gameObject.transform.position.x)
			{
				moveRight = true;
			}else if(wtf_is_in_my_way.transform.position.x > gameObject.transform.position.x)
			{
				moveLeft = true;
			}
		}
		
		if(wtf_is_in_my_way.name.Equals("Spaceship"))
		{
			MoveBackward();
			ReAdjustPosition();
		}
		
		if(wtf_is_in_my_way.name.Equals("Enemy Ship(Clone)"))
		{
			ReAdjustPosition();
		}
	}
	
	void OnTriggerEnter(Collider wtf_is_in_my_way)
	{
		if(wtf_is_in_my_way.name.Equals("Spaceship"))
		{
			if(isChaseEnabled == 1)
			{
				ReAdjustPosition();
			}
		}
	}
		
	void OnTriggerExit()
	{
		
		if(moveRight)
		{
			moveRight = false;
		}
		
		if(moveLeft)
		{
			moveLeft = false;
		}
		
	}
	
	void TurnToBase()
	{
		GameObject mothership;
		mothership = GameObject.Find("Enemy Mothership");
		sexyLady = mothership.transform;
		
	}
	
	void TurnToEnemy()
	{
		sexyLady = playerOne.transform;
	}
	
	void Retreat()
	{
		if(_EnemyShip.GetNumberOfHitsTaken > _EnemyShip.GetMaxNumberOfHits/3f | Spaceship.GetShipState ==  (int) Spaceship.ShipState.dead)
		{ 			
				
			TurnToBase();
			MoveForward();
			retreat = true;
			engageDistance = 50f;
		
			
		}else 
		{
			retreat = false;
			engageDistance = 300f;
		}
		
	
	}
	
	void MoveLeft()
	{
		gameObject.transform.Translate(-_EnemyShip.GetMovementSpeed*Time.deltaTime,0,0);

	}
		
	void MoveRight()
	{
		gameObject.transform.Translate(_EnemyShip.GetMovementSpeed*Time.deltaTime,0,0);

	}
		
	void MoveBackward()
	{
		gameObject.transform.Translate(0,0,_EnemyShip.GetMovementSpeed*Time.deltaTime,Space.Self);
		adjust_global_z_position = gameObject.transform.position;
		adjust_global_z_position.z = _EnemyShip.GetGlobalZPosition;
		gameObject.transform.position = adjust_global_z_position;
	}
	
	void MoveForward()
	{
		gameObject.transform.Translate(0,0,-_EnemyShip.GetMovementSpeed*Time.deltaTime,Space.Self);
		adjust_global_z_position = gameObject.transform.position;
		adjust_global_z_position.z = _EnemyShip.GetGlobalZPosition;
		gameObject.transform.position = adjust_global_z_position;	
	}
	
	
	void ResetLife()
	{
		_EnemyShip.ResetNumberOfHitsTaken = 0;
		engageDistance = 300f;
	}
	
	void ResetChaseTime(int higher_frequency=6, float wait_time=7f)
	{
		higherChaseFrequence = higher_frequency;
		chaseSeconds = 0;
		isChaseTimedEnabled = true;
		chaseTimedDuration = wait_time;		
	}
	
	void ResetChaseTime(float wait_time=7f)
	{
		higherChaseFrequence = defaultChaseFrequency;
		chaseSeconds = 0;
		isChaseTimedEnabled = true;
		chaseTimedDuration = wait_time;		
	}
	
	void ResetChase(int higher_frequency)
	{
		chaseSeconds = 0f;
		isChaseEnabled = Random.Range(1,higher_frequency);
		chaseDuration = Random.Range(3f,10f);
		
	}
	
	void ResetChase()
	{
		chaseSeconds = 0f;
		isChaseEnabled = Random.Range(1,defaultChaseFrequency);
		chaseDuration = Random.Range(3f,10f);
		
	}
	
	void ReAdjustPosition()
	{
		timeToReAdjustPositionSeconds = 0;
		adjustPositionSeconds = 0;
		adjustPositionDuration = 0;
		adjustPositionDuration = Random.Range(3f,8f);
	}
	
	void AdjustPosition()
	{
	
		adjustPositionSeconds += Time.deltaTime;
		if(adjustPositionSeconds < adjustPositionDuration)
		{
			if(moveLeft)
			{
				gameObject.transform.Translate(-_EnemyShip.GetMovementSpeed*Time.deltaTime,0,0);
	
			}
			
			if(moveRight)
			{
				gameObject.transform.Translate(_EnemyShip.GetMovementSpeed*Time.deltaTime,0,0);
			}
						
		}
	}
	
	void FireWeapon(float time_interval=1f)
	{
		if(fireWeaponSeconds > time_interval)
		{
			EnemyPooledBullet.FirePooledBullet(weaponMainPosition.position,weaponMainRotation.rotation);
			fireWeaponSeconds = 0f;	

		}
		
		fireWeaponSeconds += Time.deltaTime;
		
	}
	
	private void GetAngleOfLineBetweenTwoPoints(Vector3 p1, Vector3 p2) 
	{ 		
		float xDiff = p2.x - p1.x; 
		float yDiff = p2.y - p1.y;
		
		if(isLookingUp)
		{
			rotX = Mathf.Atan2(-yDiff, -xDiff) * Mathf.Rad2Deg;
		}else
		{
			rotX = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;
		}
	}
}
