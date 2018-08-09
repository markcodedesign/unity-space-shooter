using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

	public Planet planetToProtect;
	public GameObject arrow;
	public Transform creepyGuy;
	public Transform sexyLady;
	
	private float rotX;
	public bool isLookingUp = false;
	protected bool isCompassOn;
	
	public bool GetIsCompassOn
	{
		get { return isCompassOn;}
	}

	void FixedUpdate () {
		if(!planetToProtect.renderer.isVisible)
		{
			if(!GameObject.Find("HUD").GetComponent<HUD>().newIncomingMessage)
			{
				if(!GameObject.Find("HUD").audio.isPlaying)
				{
					isCompassOn = true;
					arrow.renderer.enabled = true;
					GetAngleOfLineBetweenTwoPoints(creepyGuy.position,sexyLady.position);
					creepyGuy.rotation = Quaternion.Euler(rotX,270,90);
				}else{
					isCompassOn = false;
					arrow.renderer.enabled = false;
				}
				
			}
			else{
					isCompassOn = false;
					arrow.renderer.enabled = false;
				}
		}else
		{
			isCompassOn = false;
			arrow.renderer.enabled = false;
		}
			
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
