using UnityEngine;
using System.Collections;

public class BulletEnemy : MonoBehaviour {

	enum BulletFrame { red,blue,yellow,green };
	int indexBulletColor;

	public float movementSpeed;
	
	public MeshFilter meshFilter;
	public Renderer spriteSheet;
	private Vector2[] spriteFrame;
	private Vector2 spritePosition;
	
	private float quadHeight;
	private float quadWidth;
	private float quadOrigHeight;
	private float quadOrigWidth;
	private float quadRatio;
	private float quadDesiredSize;
	private	Vector3[] quadVertices;
	
	private GameObject guidanceObject;
	
	
	void Start () {
		
		
		movementSpeed = 600f;
		quadWidth = 64f;
		quadHeight = 256f;
		quadDesiredSize = 12;
		quadRatio = quadWidth/quadHeight;
		
		quadOrigWidth = quadWidth;
		quadOrigHeight = quadHeight;
		for(int x=0;x < (int)(quadHeight-quadDesiredSize);x++)
		{
			quadWidth -= quadRatio;
		}
		quadHeight = quadDesiredSize;
		
		quadVertices = new Vector3[4];
		quadVertices[0] = new Vector3(quadWidth,0,quadHeight);
		quadVertices[1] = new Vector3(-quadWidth,0,-quadHeight);
		quadVertices[2] = new Vector3(-quadWidth,0,quadHeight);
		quadVertices[3] = new Vector3(quadWidth,0,-quadHeight);
		meshFilter.mesh.vertices = quadVertices;
		
		Vector2[] uv = new Vector2[]
        {
			new Vector2(.15f,.2f),	// Upper-right point	
			new Vector2(.20f,.4f), 	// Upper-left point
			new Vector2(.20f,.2f),  // Lower-left point
            new Vector2(.15f,.4f), 	// Upper-right point
          
        };
		
		meshFilter.mesh.uv = uv;
		
//		boxCollider.size = Vector3.zero;
//		boxCollider.size = new Vector3(quadWidth*2,0,quadHeight*2);
		
//		spritePosition = new Vector2(0.05f,0.2f);
//		spriteFrame = new Vector2[4];
//		spriteFrame[(int)BulletFrame.red] = new Vector2(0f,0.2f);
//		spriteFrame[(int)BulletFrame.blue] = new Vector2(0.05f,0.2f);
//		spriteFrame[(int)BulletFrame.yellow] = new Vector2(0.1f,0.2f);
//		spriteFrame[(int)BulletFrame.green] = new Vector2(0.15f,0.2f);
	
//		spriteSheet.material.mainTextureScale = spritePosition;
//		spriteSheet.material.mainTextureOffset = spriteFrame[(int)BulletFrame.green];
		
		Vector3 adjust_z_position = gameObject.transform.position;
		adjust_z_position.z = 5.0f;
		gameObject.transform.position = adjust_z_position;
		
		guidanceObject = new GameObject(gameObject.name);
		guidanceObject.AddComponent<BoxCollider>().size = new Vector3(quadWidth*2,0,quadHeight*2);
		guidanceObject.AddComponent<Rigidbody>().useGravity = false;
		guidanceObject.GetComponent<Rigidbody>().isKinematic = true;
		guidanceObject.transform.rotation = gameObject.transform.rotation;
		guidanceObject.transform.parent = gameObject.transform;
		
		gameObject.renderer.enabled = false;

	
	}
	
	
	void Update()
	{
		if(gameObject.renderer.enabled)
		{
			gameObject.transform.Translate(0,0f,-movementSpeed*Time.deltaTime,Space.Self);
		    Vector3 pos = gameObject.transform.position;
			pos.z = 5.0f;
			guidanceObject.transform.position = pos;
		}
//		
//		if(Time.time % 3 > 10f)
//		{
//			gameObject.renderer.enabled = false;
//		}
	}

	
	void OnBecameInvisible()
	{
		gameObject.renderer.enabled = false;
	}

}
