using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public struct UV 
{
	private Vector2[] uv_array;

	public Vector2[] GetUV()
	{
		return uv_array;
	}
	
	public UV(float xleft,float xright,float ytop,float ybottom)
	{
		uv_array = new Vector2[4];
		
		uv_array[0].x =xleft;
		uv_array[1].x =xright;
		uv_array[2].x =xright;
		uv_array[3].x =xleft;
		
		uv_array[0].y =ytop;
		uv_array[1].y =ybottom;
		uv_array[2].y =ytop;
		uv_array[3].y =ybottom;			
	}	
	
}

public class MenuButton
{
	
	private static int numberOfButtons=0;
	private static int indexButton=0;
	private static List<MenuButton> listOfMenuButtons;
	
	private int id=0;
	private float width=100f;
	private float height=100f;
	
	private GameObject button = null;
	private Mesh buttonMesh = null;

	private UV buttonNeutral;
	private UV buttonHighlight;
	
	private bool isEnabled=true;
	private bool isHighlighted = false;
	
	
	static MenuButton()
	{
		listOfMenuButtons = new List<MenuButton>();
	}

	public MenuButton(string name,float width,float height,Material mat)
	{
				
		++numberOfButtons;
		if(numberOfButtons-1 == 0)
		{
			id = 0;
		}else
		{
			id = numberOfButtons-1;
		}
			
		listOfMenuButtons.Add(this);
		
		this.width = width;
		this.height = height;
		
		button = new GameObject(name);
		button.AddComponent<MeshFilter>();
		button.AddComponent<MeshRenderer>();
		button.AddComponent<BoxCollider>();
		
		AdjustQuad();	
		AdjustCollider();
		
		buttonMesh = button.GetComponent<MeshFilter>().mesh;
				
		button.transform.Rotate(270f,0,0,Space.Self);
		
		MeshRenderer meshren = button.GetComponent<MeshRenderer>();
		meshren.sharedMaterial = mat;	
		meshren.castShadows = false;
		meshren.receiveShadows = false;
	}
	
	private void AdjustQuad()
	{
		Vector3[] quad_vertices = new Vector3[4];
		quad_vertices[0] = new Vector3(this.width,0,this.height);
		quad_vertices[1] = new Vector3(-this.width,0,-this.height);
		quad_vertices[2] = new Vector3(-this.width,0,this.height);
		quad_vertices[3] = new Vector3(this.width,0,-this.height);

		int[] triangle_indices = new int[6] { 0,1,2,0,3,1};
		
		Mesh temp_mesh = button.GetComponent<MeshFilter>().mesh;
		
		temp_mesh.vertices = quad_vertices;
		temp_mesh.triangles = triangle_indices;
	}
	
	private void AdjustCollider()
	{
		BoxCollider temp_box_collider = button.GetComponent<BoxCollider>();
		temp_box_collider.size = Vector3.zero;
		temp_box_collider.size = new Vector3(this.width,0f,this.height);
	}
	
	public static void ResetStaticMembers()
	{
		indexButton=0;
		numberOfButtons=0;
		listOfMenuButtons.Clear();
	}
	
	public static void NextButton()
	{

		++indexButton;
		if(indexButton > numberOfButtons-1)
		{
			indexButton = 0;
		}
		
		CheckButtonHighlight();
	}
	
	public static void PrevButton()
	{
			
		--indexButton;		
		if(indexButton < 0)
		{
			indexButton = numberOfButtons-1;
		}
		
		CheckButtonHighlight();
	}
	
	public static void ShowButtons()
	{
		for(int x=0;x < listOfMenuButtons.Count;x++)
		{
			listOfMenuButtons[x].EnableButton();
		}
	}
	
	public static void HideButtons()
	{	
		for(int x=0;x < listOfMenuButtons.Count;x++)
		{
			listOfMenuButtons[x].DisableButton();
		}	
		
	}
	
	private static void CheckButtonHighlight()
	{
		for(int i=0;i < numberOfButtons;i++)
		{
			if(listOfMenuButtons[i].GetID() != indexButton)
			{
				listOfMenuButtons[i].ShowNeutral();
			}
			
			if(listOfMenuButtons[i].GetID() == indexButton)
			{
				listOfMenuButtons[i].ShowHighlight();
			}
		}		
	}
	
	public void EnableButton()
	{
		isEnabled = true;
		button.renderer.enabled = true;
	}
	
	public void DisableButton()
	{
		isEnabled = false;
		if(button.renderer)
		{
			button.renderer.enabled = false;
		}
	}
	
	public static int GetNumberOfButtons()
	{
		return numberOfButtons;
	}
	public static int GetCurrentButtonIndex()
	{
		return indexButton;
	}
	
	public int GetID()
	{
		return id;
	}
	
	public GameObject GetButtonObject()
	{
		return button;
	}
	
	public bool IsButtonHighlighted()
	{
		return isHighlighted;
	}
	
	public static void SetTagName(string tag_name)
	{
		for(int x=0;x<listOfMenuButtons.Count;x++)
		{
			listOfMenuButtons[x].GetButtonObject().tag = tag_name;
		}
	}
	
	public void SetVerticalPosition(float y)
	{
		Vector3 pos = button.transform.localPosition;
		pos.y = y;
		button.transform.localPosition = pos;
	}
	
	public void SetBottonNeutral(float uv_xleft,float uv_xright,float uv_ytop,float uv_ybottom)
	{
		buttonNeutral = new UV(uv_xleft,uv_xright,uv_ytop,uv_ybottom);
		buttonMesh.uv = buttonNeutral.GetUV();
	}
	
	public void SetBottonHighlight(float uv_xleft,float uv_xright,float uv_ytop,float uv_ybottom)
	{
		buttonHighlight = new UV(uv_xleft,uv_xright,uv_ytop,uv_ybottom);
		buttonMesh.uv = buttonHighlight.GetUV();
	}

	public void SetParent(Transform parent)
	{
		button.transform.parent = parent;
	}
	
	public void SetSize(float size)
	{
		if(size < this.height)
		{
			this.width /= size;
			this.height /= size;
			AdjustQuad();
		}		
	}
	
	public static void SetSizeAll(float size)
	{
		for(int i=0; i < MenuButton.listOfMenuButtons.Count; i++)
		{
			MenuButton.listOfMenuButtons[i].SetSize(size);
		}
	}
	
	public void ShowHighlight()
	{
		isHighlighted = true;
		buttonMesh.uv = buttonHighlight.GetUV();
	}
	
	public void ShowNeutral()
	{
		isHighlighted = false;
		buttonMesh.uv = buttonNeutral.GetUV();
	}		
	
}