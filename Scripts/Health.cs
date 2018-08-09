using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public MeshFilter meshFilter;
	private float quadHeight;
	private float quadWidth;
	private	Vector3[] quadVertices;
	private Vector2[] uv;
	
	private float decreaseLife;
	private float temp;
	private float new_width;
	
	void Start() {
		
		quadVertices = new Vector3[4];
		quadWidth = 128f;
		quadHeight = 8f;
		
		decreaseLife = 2*quadWidth/Spaceship.GetMaxNumberOfHits;
		Debug.Log("Decrease life by: " + decreaseLife);
		
		uv = new Vector2[]
        {
			new Vector2(0.3f,.4f),  
			new Vector2(.35f,0.35f),
			new Vector2(.35f,.4f),
			new Vector2(0.3f,.35f),  
		};		
		
		meshFilter.mesh.uv = uv;
				
		AdjustQuadSize();
		
		new_width = quadWidth;
		
		Spaceship.DecreaseHealth = DecreaseLife;
		Spaceship.ResetHealth = ResetLife;	
		
		Debug.Log("Health Loaded");
		
	}
	
	
	void DecreaseLife()
	{
		new_width = new_width - decreaseLife;
		Debug.Log("Decrease life by: " + decreaseLife + " Width: " + new_width);
		
		quadVertices[0] = new Vector3(new_width,0,quadHeight);
		quadVertices[1] = new Vector3(-quadWidth,0,-quadHeight);
		quadVertices[2] = new Vector3(-quadWidth,0,quadHeight);
		quadVertices[3] = new Vector3(new_width,0,-quadHeight);
		meshFilter.mesh.vertices = quadVertices;
	}
	
	void ResetLife()
	{
		new_width = quadWidth;
		AdjustQuadSize();
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
