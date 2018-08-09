using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	public MeshFilter meshFilter;
	public Material backgroundMaterial;

	private float quadHeight;
	private float quadWidth;
	private float desiredSize;
	private	Vector3[] quadVertices;
	
	void Awake () {
		
		quadVertices = new Vector3[4];
		desiredSize = 10f;
		quadWidth = 9725f/desiredSize;
		quadHeight = 4862f/desiredSize;
				
		AdjustQuadSize();
		
		gameObject.renderer.material= backgroundMaterial;
			
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
