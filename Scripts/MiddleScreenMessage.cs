using UnityEngine;
using System.Collections;

public class MiddleScreenMessage : MonoBehaviour {

	private float quadHeight;
	private float quadWidth;
	private float quadOrigHeight;
	private float quadOrigWidth;
	private float quadRatio;
	private float quadDesiredSize;
	private	Vector3[] quadVertices;
	
	public MeshFilter meshFilter;
	public Renderer spriteSheet;
	private Vector2[] spriteFrame;
	private Vector2 spritePosition;
	
	private bool startBlinking;
	private float blinkSpeed;
	private float blinkOnTimeTemp;
	private float blinkOffTimeTemp;
	
	enum Message : int { paused,ready};
	
	void Awake () {
		
		blinkSpeed = .5f;
		blinkOnTimeTemp = 0f;
		blinkOffTimeTemp = 0f;
		
		quadVertices = new Vector3[4];
		quadWidth = 256f;
		quadHeight = 32f;
		quadDesiredSize = 16f;
		quadRatio = quadWidth/quadHeight;
		
		quadOrigWidth = quadWidth;
		quadOrigHeight = quadHeight;
		for(int x=0;x < (int)(quadHeight-quadDesiredSize);x++)
		{
			quadWidth -= quadRatio;
		}
		quadHeight = quadDesiredSize;
		
		AdjustQuadSize();
		
		spritePosition = new Vector2(-0.2f,-0.03f);
		spriteFrame = new Vector2[2];
		spriteFrame[(int)Message.paused] = new Vector2(0.4f,0.199f);
		spriteFrame[(int)Message.ready] = new Vector2(0.04f,0.168f);
		
		spriteSheet.material.mainTextureScale = spritePosition;
		
			
		gameObject.renderer.enabled = false;
	}
	
	
	void Update()
	{
		
		if(Globals.isPause)
		{
	
			spriteSheet.material.mainTextureOffset = spriteFrame[(int)Message.paused];
		    gameObject.renderer.enabled = true;
			
//			if(startBlinking)
//			{
//				if(blinkOnTimeTemp < blinkSpeed)
//				{
//					gameObject.renderer.enabled = true;
//					blinkOnTimeTemp +=Time.deltaTime;									
//				}
//				
//				else if(blinkOffTimeTemp < blinkSpeed)
//				{
//					gameObject.renderer.enabled = false;
//					blinkOffTimeTemp += Time.deltaTime;
//				}else
//				{
//					blinkOnTimeTemp = 0;
//					blinkOffTimeTemp = blinkOnTimeTemp;
//				}
//				
//				
//			}			
		}else
		{
			gameObject.renderer.enabled = false;
		}
	}
		
	void AdjustQuadSize()
	{
		quadVertices[0] = new Vector3(quadWidth,0,quadHeight);
		quadVertices[1] = new Vector3(-quadWidth,0,-quadHeight);
		quadVertices[2] = new Vector3(-quadWidth,0,quadHeight);
		quadVertices[3] = new Vector3(quadWidth,0,-quadHeight);
		meshFilter.mesh.vertices = quadVertices;
	}
}
