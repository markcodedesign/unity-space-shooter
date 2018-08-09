using UnityEngine;
using System.Collections;

public class CircleBlinking : MonoBehaviour {
	
	public GameObject circleBlinking;
	
	public MeshFilter meshFilter;
	private float quadHeight;
	private float quadWidth;
	private	Vector3[] quadVertices;
	
	public Renderer spriteSheet;
	private Vector2[] spriteFrame;
	private Vector2 spritePosition;
	
	private bool startBlinking;
	private float blinkSpeed;
	private float blinkOnTimeTemp;
	private float blinkOffTimeTemp;
	private bool isBlink;
		
		
	public enum BlinkCircleColor : int { red,green,blue};
	
	void Start () {
		startBlinking = false;
		blinkSpeed = 0f;
		blinkOnTimeTemp = 0f;
		blinkOffTimeTemp = 0f;
				
		quadVertices = new Vector3[4];
		quadWidth = 16f;
		quadHeight = 16f;
				
		AdjustQuadSize();
		
		circleBlinking.renderer.enabled = false;
		
		spritePosition = new Vector2(0.05f,-0.05f);
		spriteFrame = new Vector2[2];
		spriteFrame[(int)BlinkCircleColor.red] = new Vector2(0.2499f,0.249f);
		spriteFrame[(int)BlinkCircleColor.green] = new Vector2(0.1999f,0.249f);
		spriteSheet.material.mainTextureScale = spritePosition;
		spriteSheet.material.mainTextureOffset = spriteFrame[(int)BlinkCircleColor.green];
		
		//BlinkStart(0.5f,BlinkCircleColor.green);
		
	}
	
	void Update()
	{
		if(startBlinking)
		{
			if(Time.time % 1 > blinkSpeed)
			{
				circleBlinking.renderer.enabled = true;
			}else
			{
				circleBlinking.renderer.enabled = false;					
			}
			
//			if(blinkOnTimeTemp < blinkSpeed)
//			{
//				circleBlinking.renderer.enabled = true;
//				blinkOnTimeTemp +=Time.deltaTime;									
//			}	
//			else if(blinkOffTimeTemp < blinkSpeed)
//			{
//				circleBlinking.renderer.enabled = false;
//				blinkOffTimeTemp += Time.deltaTime;
//			}else
//			{
//				blinkOnTimeTemp = 0;
//				blinkOffTimeTemp = blinkOnTimeTemp;
//			}
			
			
		}			
	}
	
	public void BlinkSpeed(float blink_speed=0.5f)
	{
		blinkSpeed = blink_speed;
		
	}
	
	public void BlinkStart(float blink_speed=0.5f, BlinkCircleColor blink_color=BlinkCircleColor.green)
	{
		spriteSheet.material.mainTextureOffset = spriteFrame[(int)blink_color];
		blinkSpeed = blink_speed;

		circleBlinking.renderer.enabled = true;
		startBlinking = true;
	}
	
	public void BlinkStop()
	{
		blinkOnTimeTemp = 0;
		circleBlinking.renderer.enabled = false;
		startBlinking = false;
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
