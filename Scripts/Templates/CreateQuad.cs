using UnityEngine;
using System.Collections;

public class CreateQuad : MonoBehaviour {
	
	
	public BoxCollider boxCollider;
	public MeshFilter meshFilter;
	private float quadHeight;
	private float quadWidth;
	private	Vector3[] quadVertices;
	
	void Awake () {
		
		quadVertices = new Vector3[4];
		quadWidth = 64f;
		quadHeight = 64f;
				
		AdjustBoxCollider();
		AdjustQuadSize();
		
		Mesh temp_mesh = gameObject.GetComponent<MeshFilter>().mesh;
		
		
	}
	
	void AdjustBoxCollider()
	{
		boxCollider.size = Vector3.zero;
		boxCollider.size = new Vector3(quadWidth*2,0,quadHeight*2);
	}
	
	void AdjustQuadSize()
	{
		quadVertices[0] = new Vector3(quadWidth,0,quadHeight);
		quadVertices[1] = new Vector3(-quadWidth,0,-quadHeight);
		quadVertices[2] = new Vector3(-quadWidth,0,quadHeight);
		quadVertices[3] = new Vector3(quadWidth,0,-quadHeight);
		meshFilter.mesh.vertices = quadVertices;
	}
	
	public Vector3[] Create(float width, float height)
	{
		Vector3[] _quadVertices = new Vector3[4];
				
		quadVertices[0] = new Vector3(width,0,height);
		quadVertices[1] = new Vector3(-width,0,-height);
		quadVertices[2] = new Vector3(-width,0,height);
		quadVertices[3] = new Vector3(width,0,-height);
		
		return _quadVertices;
	
	}
	
	public void Create(Mesh mesh,float width,float height)
	{
		
		if(mesh == null)
		{
			
			mesh.vertices = Create (width,height);			
		}
	}

}
