using UnityEngine;
using System.Collections;

public class EnemyShip : MonoBehaviour {
	
	public static int NumberOfEnemies;
	protected static float movementSpeed;

	
	enum AnimationFrame { greenenemycolor,redenemycolor};
	enum ShipState { hit,alive,search,dead,moving_to_position};
	
	
	public AudioClip[] soundEffect;
	public Transform spawnPoint;
	public BoxCollider boxCollider;
	public MeshFilter meshFilter;
	
	private float quadHeight;
	private float quadWidth;
	private	Vector3[] quadVertices;
	
	protected int numberOfHitsTaken;
	protected int maxNumberOfHits;
	private bool isBusy;
	private bool isPositionReached;
	private int ai_state;
	private Vector3 move_to_position;
	private Vector3 spawn_position;
	protected Vector3 adjust_z_position;
	protected static float globalZPosition;
	
	private GameObject guidanceObject;
	
	public int GetNumberOfHitsTaken
	{
		get { return numberOfHitsTaken;}
	}
	
	public int GetMaxNumberOfHits
	{
		get { return maxNumberOfHits;}
	}
	public float GetGlobalZPosition
	{
		get { return globalZPosition;}
	}
	
	public Vector3 GetAdjustedZPosition
	{
		get { return adjust_z_position;}
	}
	
	public float GetMovementSpeed
	{
		get { return movementSpeed;}
	}
	
	public int ResetNumberOfHitsTaken
	{
		set { numberOfHitsTaken = value;}
	}
	
	
	void Awake () {
		
		spawnPoint = GameObject.Find("Mothership Spawnpoint").transform;

		globalZPosition = 5.0f;
		++NumberOfEnemies;
		movementSpeed = 200f;

		quadWidth = 32f;
		quadHeight = 22f;
		quadVertices = new Vector3[4];
		quadVertices[0] = new Vector3(quadWidth,0,quadHeight);
		quadVertices[1] = new Vector3(-quadWidth,0,-quadHeight);
		quadVertices[2] = new Vector3(-quadWidth,0,quadHeight);
		quadVertices[3] = new Vector3(quadWidth,0,-quadHeight);
		
		meshFilter.mesh.vertices = quadVertices;
				
		boxCollider.size = Vector3.zero;
		boxCollider.size = new Vector3(quadWidth*2,0,quadHeight*2);
			
		Vector2[] uv = new Vector2[]
        {
			new Vector2(0f,.6f),  // Lower-Right point	
			new Vector2(.2f,.8f), // Upper-Left point
			new Vector2(.2f,.6f), // Lower-Lef point
            new Vector2(0f,0.799f),  // Upper-Right point
          
        };		
		
		meshFilter.mesh.uv = uv;
		
	
		adjust_z_position = gameObject.transform.position;
		adjust_z_position.z = globalZPosition;
		gameObject.transform.position = adjust_z_position;
		
		guidanceObject = new GameObject("Guide Object of " + gameObject.name);
		guidanceObject.AddComponent<AudioSource>().volume = gameObject.audio.volume;
		guidanceObject.audio.playOnAwake = false;
		guidanceObject.GetComponent<AudioSource>().clip = soundEffect[0];
					
		Spawn();
		
		}

	void OnTriggerEnter(Collider wtf_hit_me)
	{
	
		if(wtf_hit_me.name.Equals("Bullet(Clone)"))
		{
			wtf_hit_me.transform.parent.gameObject.renderer.enabled = false;
			Hit ();
		}
		
	}
	
	
//	void OnBecameInvisible()
//	{
//		if(gameObject.activeInHierarchy)
//		{
//			StartCoroutine("DestroyObjectAfterTime",10f);
//		}
//	}
	
//	void OnBecameVisible()
//	{
//		StopCoroutine("DestroyObjectAfterTime");
//	}
	
	public void Spawn()
	{
		numberOfHitsTaken = 0;
		maxNumberOfHits = Random.Range(2,20);
		spawn_position = spawnPoint.position;
		spawn_position.y = Random.Range(250f,200f);
		spawn_position.x = Random.Range(-1000f,1000f);
		spawn_position.z = gameObject.transform.position.z;
		gameObject.transform.position = spawn_position;
		ai_state = (int) ShipState.alive;
	}
	
//	void Spawn(bool is_position_only = false)
//	{
//		if(is_position_only)
//		{
//			isPositionReached = false;
//		}
//		else 
//		{
//			isBusy = false;
//			isPositionReached = false;		
//			numberOfHitsTaken = 0;
//			maxNumberOfHits = Random.Range(2,10);
//		}
//		
//		spawn_position = new Vector3((float) Random.Range(-(Screen.width/2),Screen.width/2),450f,gameObject.transform.position.z);
//		
//		
//		gameObject.transform.position = spawn_position;
//		move_to_position = gameObject.transform.position;
//		move_to_position.y = (float)Random.Range(100,300);
//		
//	}
//	
		
	void Hit()
	{
		++numberOfHitsTaken;
		int index = Random.Range(1,soundEffect.Length);
		
		if(numberOfHitsTaken == maxNumberOfHits)
		{	
			guidanceObject.audio.PlayOneShot(soundEffect[index]);
			guidanceObject.transform.position = gameObject.transform.position;
			ai_state = (int) ShipState.dead;

		}else
		{
			gameObject.audio.clip = soundEffect[index];
			gameObject.audio.Play();
		}
	}
	
	void OnDestroy()
	{
		--NumberOfEnemies;
		Debug.Log("Enemy Dead. Initialize Explosion Animation");
				
	}
	
//	IEnumerator AI()
//	{
//		isBusy = true;
//		ai_state = (int) ShipState.moving_to_position;
//		yield return new WaitForSeconds(2f);
//		ai_state = (int) ShipState.alive;
//		Debug.Log("AI Entering State 1");
//		
//		yield return new WaitForSeconds(1f);
//		Debug.Log("AI Entering State 2");
//		
//		yield return new WaitForSeconds(1f);
//		Debug.Log("AI Entering State 3");
//	
//	}

//	IEnumerator DestroyObjectAfterTime(float time_in_seconds)
//	{
//		Debug.Log("Destroying Object in " + time_in_seconds + " seconds.");
//		yield return new WaitForSeconds(time_in_seconds);
//		Debug.Log("Oject Destroyed");
//		DestroyObject(guidanceObject);
//		DestroyObject(gameObject);
//	}
		
		
	void AI_MoveToPosition()
	{
		if(!isPositionReached)
		{
			gameObject.transform.Translate(0,-movementSpeed*Time.deltaTime,0,Space.World);
			if(gameObject.transform.position.y <= move_to_position.y)
			{
				isPositionReached = true;
				Debug.Log("Position Reached");
			}
		}
	}
	
//	public void AI_FireWeapon(bool is_pooled)
//	{
//		
//		if(Spaceship.GetShipState == (int)ShipState.alive)
//		{
//			
//			if(!is_pooled)
//			{
//				GameObject temp_bullet_object;
//				temp_bullet_object = Instantiate(bullet,weaponMain.position,gameObject.transform.rotation) as GameObject;
//			}else{
//			
//				foreach(GameObject bullet_available in bulletPool)
//				{
//					if(!bullet_available.renderer.enabled)
//					{
//						bullet_available.transform.position = weaponMain.position;
//						bullet_available.transform.rotation = gameObject.transform.rotation;
//						bullet_available.renderer.enabled = true;
//						break;
//					}
//					
//				}
//			}
//		
//		}
//	}
	
	void Update()
	{
		
//		if(!isBusy)
//		{
//			StartCoroutine(AI());
//		}
//		if(ai_state == (int) ShipState.moving_to_position)
//		{
//			AI_MoveToPosition();
//		}
		
		if(ai_state == (int) ShipState.dead)
		{
			if(!guidanceObject.audio.isPlaying)
			{
				guidanceObject.audio.Play();
				Spawn();
				//DestroyObject(guidanceObject,4f);
				//DestroyObject(gameObject);
			}
		}
					
	}
}
