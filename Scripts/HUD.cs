using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	public AudioClip[] malekAudioClip; 
	
	private GameObject hudDisplay;
	public Transform hudPosition;
	public Compass compass;
	public GameObject blinkCircleObjectTemplate;
	private GameObject blinkCircle;
			
	public MeshFilter meshFilter;
	public Renderer spriteSheet;
	private Vector2[] spriteFrame;
	private Vector2 spritePosition;
	
	private float quadSize;
	private float quadHeight;
	private float quadWidth;
	private	Vector3[] quadVertices;
	
	private Vector3 adjustZPosition;
	
	public bool newIncomingMessage;
	
	enum ComHud : int { blank,hit,don,malek};
	
	void Start () {
		
		newIncomingMessage = false;
		
		quadSize = 128f;
		quadVertices = new Vector3[4];
		quadWidth = quadSize;
		quadHeight = quadSize;
		AdjustQuadSize(quadWidth,quadHeight);
		
		spritePosition = new Vector2(-0.2f,-0.2f);
		spriteFrame = new Vector2[4];
		spriteFrame[(int)ComHud.blank] = new Vector2(1f,0.4f);
		spriteFrame[(int)ComHud.malek] = new Vector2(0.601f,0.4f);
		spriteFrame[(int)ComHud.don] = new Vector2(0.8f,0.4f);
		

		spriteSheet.material.mainTextureScale = spritePosition;
		spriteSheet.material.mainTextureOffset = spriteFrame[(int)ComHud.blank];
		
		blinkCircle = Instantiate(blinkCircleObjectTemplate,new Vector3(0f,0f,0f),blinkCircleObjectTemplate.transform.rotation) as GameObject;
		blinkCircle.transform.parent = gameObject.transform.root;
		adjustZPosition = new Vector3(352f,-68f,1f);
		blinkCircle.transform.position = adjustZPosition;
		
	
	}
	
	void Update()
	{
		if(newIncomingMessage)
		{		
			if(!gameObject.audio.isPlaying)
			{
				blinkCircle.GetComponent<CircleBlinking>().BlinkStart(0.5f,CircleBlinking.BlinkCircleColor.green);
				spriteSheet.material.mainTextureOffset = spriteFrame[(int)ComHud.malek];
				gameObject.audio.clip = malekAudioClip[Random.Range(0,malekAudioClip.Length-1)];
				gameObject.audio.Play();
			}
			
		}
		else
		{
			spriteSheet.material.mainTextureOffset = spriteFrame[(int)ComHud.blank];
			blinkCircle.GetComponent<CircleBlinking>().BlinkStop();
		}
			
		if(compass.GetIsCompassOn)
		{
			if(!newIncomingMessage)
			{
				blinkCircle.GetComponent<CircleBlinking>().BlinkStop();
				spriteSheet.material.mainTextureOffset = spriteFrame[(int)ComHud.blank];
			}

		}
		
	
		if(Spaceship.GetNumberOfHitsTaken < Spaceship.GetMaxNumberOfHits/2)
		{
			newIncomingMessage = false;
					
		}else if(Spaceship.GetNumberOfHitsTaken > Spaceship.GetMaxNumberOfHits/2)
		{
			newIncomingMessage = true;
		}
		
			
	}
	
	
		
	void AdjustQuadSize(float width,float height)
	{
		quadVertices[0] = new Vector3(width,0,height);
		quadVertices[1] = new Vector3(-width,0,-height);
		quadVertices[2] = new Vector3(-width,0,height);
		quadVertices[3] = new Vector3(width,0,-height);
		meshFilter.mesh.vertices = quadVertices;
	}
	

}
