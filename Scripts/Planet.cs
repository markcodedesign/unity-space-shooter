using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

	public BoxCollider boxCollider;
	public MeshFilter meshFilter;
	private float quadHeight;
	private float quadWidth;
	private float quadSize;
	private	Vector3[] quadVertices;
	
	protected bool isInView;
	
	private float planetRotationSpeed;
	
	public bool GetIsInView
	{
		get { return isInView;}
	}
	
	void Awake () {
		
		isInView = true;
		planetRotationSpeed = 0.03f;
		quadVertices = new Vector3[4];
		quadSize = 400f;
	

//		AdjustBoxCollider();
		AdjustQuadSize();
		
	}
	

	
	void Update()
	{
		meshFilter.mesh.RecalculateBounds();
		gameObject.transform.RotateAroundLocal(Vector3.forward,-planetRotationSpeed*Time.deltaTime);

	}
	
	void AdjustBoxCollider()
	{
		boxCollider.size = Vector3.zero;
		boxCollider.size = new Vector3(quadSize*2,0,quadSize*2);
	}
	
	void AdjustQuadSize()
	{
		quadVertices[0] = new Vector3(quadSize,0,quadSize);
		quadVertices[1] = new Vector3(-quadSize,0,-quadSize);
		quadVertices[2] = new Vector3(-quadSize,0,quadSize);
		quadVertices[3] = new Vector3(quadSize,0,-quadSize);
		meshFilter.mesh.vertices = quadVertices;
	}
	
	
}
