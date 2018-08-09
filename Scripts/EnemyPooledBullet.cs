using UnityEngine;
using System.Collections;

public class EnemyPooledBullet : MonoBehaviour {

	private static GameObject[] bulletPool;
	public GameObject bullet;
	private static AudioSource clip;
	
	void Start () {
	
		// Bullet Pooling Initialization
		bulletPool = new GameObject[15];
		for(int i=0;i < bulletPool.Length;i++)
		{
			bulletPool[i] = Instantiate(bullet) as GameObject;
		}
		
		clip = audio;
		
	}
	
	
	public static void FirePooledBullet(Vector3 bulletposition,Quaternion bulletrotation)
	{	
		foreach(GameObject bullet_available in bulletPool)
		{
			if(!bullet_available.renderer.enabled)
			{				
				bullet_available.renderer.enabled = true;
				bullet_available.transform.position = bulletposition;
				bullet_available.transform.rotation = bulletrotation;
			
				clip.Play();
				
				break;
			}
		}						
	}
}
