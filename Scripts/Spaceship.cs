using UnityEngine;
using System.Collections;

public class Spaceship : MonoBehaviour {
	
	enum AnimationFrame { neutralposition,moveleft,moveright,death};
	public enum ShipState { hit,alive,search,dead,moving_to_position};
	enum Direction { forward,backward,left,right,neutral};
	
	[RangeAttribute(100f, 5000f)]
 	public float worldBoundary;
	
	private GameObject[] bulletPool;
	
	private Vector2[] uv_neutral;
	private Vector2[] uv_left;
	private Vector2[] uv_right;
	
	public AudioClip[] soundEffect;
	
//	public Transform globalsTranform;
	private float maxRoamingDistance;
	private Vector3 limitToRoamingDistance;
	
	public GameObject bullet;
	public Transform weaponMain;
	public float movementSpeed;
	public float cameraSpeed;
	private Vector3 cameraVelocity;
	private Transform cameraTransform;
	public float rotationSpeed;
	public BoxCollider boxCollider;
	public MeshFilter meshFilter;
	public Renderer spriteSheet;
	private Vector2[] spriteFrame;
	private Vector2 spritePosition;
	private float quadHeight;
	private float quadWidth;
	private	Vector3[] quadVertices;
	
	protected static int numberOfDeaths;
	protected static int numberOfHitsTaken;
	protected static int maxNumberOfHits;
	protected static int ai_state;

	private bool isBusy;
	private bool isPositionReached;
	private Vector3 move_to_position;
	private Vector3 spawn_position;
	private Vector3 death_position;
	private Vector3 adjust_global_z_position;
	private float globalZPosition;
	
	private GameObject guidanceObject;

	public delegate void Health();
	public delegate void HealthReset();
	public static HealthReset ResetHealth;
	public static Health DecreaseHealth;
	
	public static int GetShipState
	{
		get { return ai_state;}
	}
	
	public static int GetMaxNumberOfHits
	{
		get { return maxNumberOfHits;}
	}
	
	public static int GetNumberOfHitsTaken
	{
		get { return numberOfHitsTaken;}
	}
	
	public static int GetNumberOfDeaths
	{
		get { return numberOfDeaths;}
	}
	
	
	void Awake()
	{
		maxNumberOfHits = 15;
	}
	
	void Start() {
		
		worldBoundary = 3000;
	
		globalZPosition = 5.0f;
		
		cameraSpeed = 0.5f;
		cameraVelocity = Vector3.zero;
		cameraTransform = Camera.mainCamera.transform;
		
		maxRoamingDistance = 1500f;
		movementSpeed = 300f;
		rotationSpeed = 2f;
		isBusy = false;
		isPositionReached = false;
		numberOfHitsTaken = 0;
		maxNumberOfHits = 15;
		numberOfDeaths = 0;
		
		quadWidth = 32f;
		quadHeight = 32f;
		quadVertices = new Vector3[4];
		quadVertices[0] = new Vector3(quadWidth,0,quadHeight);
		quadVertices[1] = new Vector3(-quadWidth,0,-quadHeight);
		quadVertices[2] = new Vector3(-quadWidth,0,quadHeight);
		quadVertices[3] = new Vector3(quadWidth,0,-quadHeight);
		meshFilter.mesh.vertices = quadVertices;
		
		boxCollider.size = Vector3.zero;
		boxCollider.size = new Vector3(quadWidth*2,0,quadHeight*2);
		
		
		uv_neutral = new Vector2[]
        {
			new Vector2(0f,1.0f),  
			new Vector2(.2f,.8f), 
			new Vector2(.2f,1.0f), 
			new Vector2(0f,.8f),  	
		};		
		
		uv_right = new Vector2[]
        {
			new Vector2(0.2f,1.0f),  
			new Vector2(.4f,.8f),
			new Vector2(.4f,1.0f), 
			new Vector2(0.2f,.8f),  	
		};		
		
		uv_left = new Vector2[]
        {
			new Vector2(0.4f,1.0f),  
			new Vector2(.6f,.8f),
			new Vector2(.6f,1.0f),
			new Vector2(0.4f,.8f),  
		};		
		
		meshFilter.mesh.uv = uv_neutral;
		

		adjust_global_z_position = gameObject.transform.position;
		adjust_global_z_position.z = globalZPosition;
		gameObject.transform.position = adjust_global_z_position;
		
		guidanceObject = new GameObject("Guide Object of " + gameObject.name);
		guidanceObject.AddComponent<AudioSource>();
		guidanceObject.audio.playOnAwake = false;
		guidanceObject.GetComponent<AudioSource>().clip = soundEffect[0];
		
		// Bullet Pooling Initialization
		bulletPool = new GameObject[5];
		for(int i=0;i < bulletPool.Length;i++)
		{
			bulletPool[i] = Instantiate(bullet) as GameObject;
		}
		
		Spawn();
		
	}
	

	void OnTriggerEnter(Collider wtf_hit_me)
	{
		if(wtf_hit_me.name.Equals("Bullet Enemy(Clone)"))
		{
			wtf_hit_me.transform.parent.gameObject.renderer.enabled = false;

//			DestroyObject(wtf_hit_me.transform.parent.gameObject);
			Hit ();
		}

	}
	
	void Boundary()
	{
		if(transform.position.x > worldBoundary | transform.position.x < -worldBoundary)
			{
				if(Mathf.Sign(transform.position.x) == -1)
				{
					Vector3 temp = transform.position;
					temp.x = -worldBoundary;
					transform.position = temp;
				}else 
				{
					Vector3 temp = transform.position;
					temp.x = worldBoundary;
					transform.position = temp;
				}
				
			}
			
			if(transform.position.y > worldBoundary | transform.position.y < -worldBoundary)
			{
				if(Mathf.Sign(transform.position.y) == -1)
				{
					Vector3 temp = transform.position;
					temp.y = -worldBoundary;
					transform.position = temp;
				}else 
				{
					Vector3 temp = transform.position;
					temp.y = worldBoundary;
					transform.position = temp;
				}
				
			}
	}

	void Hit()
	{
		++numberOfHitsTaken;
		int index = Random.Range(1,soundEffect.Length);
		DecreaseHealth();
		if(numberOfHitsTaken >= maxNumberOfHits)
		{	
			numberOfHitsTaken = 0;
			++numberOfDeaths;
			guidanceObject.audio.PlayOneShot(soundEffect[index]);
			guidanceObject.transform.position = gameObject.transform.position;
			death_position = gameObject.transform.position;
			ai_state = (int) ShipState.dead;
			ResetHealth();

		}else
		{
			gameObject.audio.clip = soundEffect[index];
			gameObject.audio.Play();
		}
	}
	
	void Spawn()
	{
		ai_state = (int) ShipState.moving_to_position;
		
		if(numberOfDeaths >= 1)
		{
			spawn_position = new Vector3(death_position.x,-500f,globalZPosition);
			gameObject.transform.position = spawn_position;
			
			move_to_position = death_position;
			move_to_position.y = 200;	
		}
		else
		{
			spawn_position = new Vector3(0f,-500f,globalZPosition);
			gameObject.transform.position = spawn_position;
			move_to_position = gameObject.transform.position;
			move_to_position.y = 200;	
		}
			
		foreach(GameObject bullet_available in bulletPool)
			{
				if(bullet_available.renderer.enabled)
				{
					bullet_available.renderer.enabled = false;
				}
				
			}
	}
	
	IEnumerator AI()
	{
		isBusy = true;
		ai_state = (int) ShipState.moving_to_position;
		yield return new WaitForSeconds(2f);
		ai_state = (int) ShipState.alive;
		
	
	}
	
	void AI_MoveToPosition()
	{
		if(!isPositionReached)
		{
			gameObject.transform.Translate(0,movementSpeed*Time.deltaTime,0,Space.World);
			if(Mathf.Abs(gameObject.transform.position.y) <= move_to_position.y)
			{
				isPositionReached = false;
				ai_state = (int) ShipState.alive;
			}
		}
	}
	
	void FireWeapon(bool is_pooled)
	{
		if(!is_pooled)
		{
			GameObject temp_bullet_object;
			temp_bullet_object = Instantiate(bullet,weaponMain.position,gameObject.transform.rotation) as GameObject;
		}else{
		
			foreach(GameObject bullet_available in bulletPool)
			{
				if(!bullet_available.renderer.enabled)
				{
					bullet_available.transform.position = weaponMain.position;
					bullet_available.transform.rotation = gameObject.transform.rotation;
					bullet_available.renderer.enabled = true;
					break;
				}
				
			}
		}
	}
	

//	bool IsMaxDistanceReached()
//	{
//		float distance = Vector3.Distance(globalsTranform.position,gameObject.transform.position);
//		
//		if(distance > maxRoamingDistance)
//		{
//					
//			return true;				
//		}
//		
//		return false;
//	}
	
	void CameraFollow()
	{
		cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position,
		new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,0),ref cameraVelocity,cameraSpeed);
	}
	
#if UNITY_STANDALONE
	void ControllerPC()
	{
		if(Input.GetKeyDown(KeyCode.Period))
			{
				FireWeapon(true);					
			}
			
			if(Input.GetKey(KeyCode.S))
			{
				
				gameObject.transform.Translate(0,0,-movementSpeed*Time.deltaTime,Space.Self);
				adjust_global_z_position = gameObject.transform.position;
				adjust_global_z_position.z = globalZPosition;
				gameObject.transform.position = adjust_global_z_position;
				
				
			}
			
			if(Input.GetKey(KeyCode.W))
			{
				
				gameObject.transform.Translate(0,0,movementSpeed*Time.deltaTime,Space.Self);
				adjust_global_z_position = gameObject.transform.position;
				adjust_global_z_position.z = globalZPosition;
				gameObject.transform.position = adjust_global_z_position;
				
					
			}		
			
			if(Input.GetKey(KeyCode.D))
			{
				
				meshFilter.mesh.uv = uv_right;
				gameObject.transform.Translate(movementSpeed*Time.deltaTime,0,0,Space.Self);
				adjust_global_z_position = gameObject.transform.position;
				adjust_global_z_position.z = globalZPosition;
				gameObject.transform.position = adjust_global_z_position;
				
				
			}else if(Input.GetKeyUp(KeyCode.D))
			{
				meshFilter.mesh.uv = uv_neutral;
			}
			
			if(Input.GetKey(KeyCode.A))
			{
				
				meshFilter.mesh.uv = uv_left;
				gameObject.transform.Translate(-movementSpeed*Time.deltaTime,0,0,Space.Self);
				adjust_global_z_position = gameObject.transform.position;
				adjust_global_z_position.z = globalZPosition;
				gameObject.transform.position = adjust_global_z_position;
				
				
			}else if(Input.GetKeyUp(KeyCode.A))			
			{
				meshFilter.mesh.uv = uv_neutral;
			}
			
			if(Input.GetKey(KeyCode.Comma))
			{
				//Rotate Left
				gameObject.transform.RotateAround(Vector3.forward,rotationSpeed*Time.deltaTime);
			}
			
			if(Input.GetKey(KeyCode.Slash))
			{
				//Rotate Right
				gameObject.transform.RotateAround(Vector3.forward,-rotationSpeed*Time.deltaTime);

			}
		
	}
#endif
	
#if UNITY_ANDROID
	
	void ControllerAndroid()
	{
		
	}
	
	
#endif
	
	void Update () {
				
		Boundary();
		
		if(!isBusy)
		{
			StartCoroutine(AI());
		}
		
		if(ai_state == (int) ShipState.moving_to_position)
		{
			AI_MoveToPosition();
		}
		
		if(ai_state == (int) ShipState.dead)
		{
			if(!guidanceObject.audio.isPlaying)
			{
				guidanceObject.audio.Play();
				Spawn();
				

			}
		}
		

		if(ai_state == (int) ShipState.alive & !Globals.isPause)
		{
			
			CameraFollow();
#if UNITY_STANDALONE
			ControllerPC();			
#endif	
		}
	}
}
