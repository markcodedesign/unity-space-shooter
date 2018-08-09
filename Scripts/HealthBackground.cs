using UnityEngine;
using System.Collections;

public class HealthBackground : MonoBehaviour {
	
public MeshFilter meshFilter;
	private float quadHeight;
	private float quadWidth;
	private	Vector3[] quadVertices;
	private Vector2[] uv;
	
	void Start()
	{
		quadVertices = new Vector3[4];
		quadWidth = 128f;
		quadHeight = 8f;
		
		uv = new Vector2[]
        {
			new Vector2(0.35f,.4f),  
			new Vector2(.4f,0.35f),
			new Vector2(.4f,.4f),
			new Vector2(0.35f,.35f),  
		};		
		
		meshFilter.mesh.uv = uv;
				
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
