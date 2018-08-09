using UnityEngine;
using System.Collections;
using System;

public class ManagerMainMenu : MonoBehaviour {
	

	public Material matSpritesheet;

	public AudioClip[] menuClip;
	private const float MAINMENUBUTTONSIZE = 2f;
	private const float VERTICALSPACE = 10f;
	
	private bool isLoadingLevel;
	
	private AsyncOperation loadingProgress;
	private float loadProgress;
	
	
	private MenuButton[] menuButtons;

	private Material skyboxHolder;
	
	void Awake()
	{
		menuButtons = new MenuButton[5];
		
		menuButtons[0] = new MenuButton("Start",135f,40f,matSpritesheet);
		menuButtons[0].SetBottonHighlight(0.305f,0.202f,0.1055f,0.0755f);
		menuButtons[0].SetBottonNeutral(0.305f,0.202f,0.1358f,0.1058f);
		menuButtons[0].SetParent(gameObject.transform);
//		menuButtons[0].SetSize(MAINMENUBUTTONSIZE);
		menuButtons[0].ShowHighlight();
				
		menuButtons[1] = new MenuButton("Load",126f,39f,matSpritesheet);
		menuButtons[1].SetBottonHighlight(0.3f,0.2f,0.044f,0.014f);
		menuButtons[1].SetBottonNeutral(0.3f,0.2f,0.075f,0.045f);
		menuButtons[1].SetParent(gameObject.transform);
//		menuButtons[1].SetSize(MAINMENUBUTTONSIZE);
		menuButtons[1].SetVerticalPosition(-60f);
		
		menuButtons[2] = new MenuButton("Options",203f,48f,matSpritesheet);
		menuButtons[2].SetBottonHighlight(0.464f,0.305f,0.09959f,0.06159f);
		menuButtons[2].SetBottonNeutral(0.464f,0.305f,0.1369f,0.099f);
		menuButtons[2].SetParent(gameObject.transform);
//		menuButtons[2].SetSize(MAINMENUBUTTONSIZE);
		menuButtons[2].SetVerticalPosition(-125f);
		
		menuButtons[3] = new MenuButton("Exit",104f,39f,matSpritesheet);
		menuButtons[3].SetBottonHighlight(0.545f,0.465f,0.1067f,0.0764f);
		menuButtons[3].SetBottonNeutral(0.545f,0.465f,0.137f,0.107f);
		menuButtons[3].SetParent(gameObject.transform);
//		menuButtons[3].SetSize(MAINMENUBUTTONSIZE);
		menuButtons[3].SetVerticalPosition(-190f);
		
		menuButtons[4] = new MenuButton("Credits",191f,40f,matSpritesheet);
		menuButtons[4].SetBottonHighlight(0.73f,0.5822f,0.1676f,0.1377f);
		menuButtons[4].SetBottonNeutral(0.73f,0.5822f,0.1999f,0.17f);
		menuButtons[4].SetParent(gameObject.transform);
//		menuButtons[4].SetSize(MAINMENUBUTTONSIZE);
		menuButtons[4].SetVerticalPosition(-255f);
		
		MenuButton.SetSizeAll(MAINMENUBUTTONSIZE);
		
		for(int i=0;i<menuButtons.Length;i++)
		{
			menuButtons[i].GetButtonObject().tag = gameObject.tag;
		}
	}
	
	void OnDestroy()
	{
		MenuButton.ResetStaticMembers();
	}

	
	void Start () {
		loadProgress = 0f;
		isLoadingLevel = false;

		Screen.showCursor = false;
				
//		Vector3 temp_pos = new Vector3(368f,-40f,1);
		Vector3 temp_pos = new Vector3(0f,150f,1);
		gameObject.transform.position = temp_pos;
	}
	
	

	void Update()
	{
			
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			gameObject.audio.clip = menuClip[0];
			gameObject.audio.Play();
			MenuButton.NextButton();
		}
		
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			gameObject.audio.clip = menuClip[0];
			gameObject.audio.Play();
			MenuButton.PrevButton();
		}
			
		if(Input.GetKeyDown(KeyCode.Return))
		{
			gameObject.audio.clip = menuClip[1];
			gameObject.audio.Play();
			
			switch(MenuButton.GetCurrentButtonIndex())
			{
			case 0: // START BUTTON
				/*DISABLE MAIN MENU ITEMS*/
				skyboxHolder = RenderSettings.skybox;
				RenderSettings.skybox = null;
				
				GameObject[] temp_obj = GameObject.FindGameObjectsWithTag("MenuMainObjects");
				for(int i=0; i < temp_obj.Length;i++)
				{
					if(temp_obj[i].renderer != null)
					{
						temp_obj[i].renderer.enabled = false;
					}
				}
				
				/*LOAD OR DISPLAY STORY INTRO*/
				isLoadingLevel = true;
				loadingProgress = Application.LoadLevelAsync("Level001");
				loadingProgress.allowSceneActivation = false;
				break;
				
			case 1: // LOAD BUTTON
				Debug.Log("LOAD selected");
				
				break;
				
			case 2: // OPTIONS BUTTON
				Debug.Log("OPTIONS selected");
			
				break;
				
			case 3: // EXIT BUTTON
				Debug.Log("EXIT selected");
				Application.Quit();
				break;
				
			case 4: // CREDITS BUTTON
				Debug.Log("CREDITS selected");
				break;
				
			default:
				break;
			}
		}
		
		if(isLoadingLevel)
		{
			loadProgress += 0.2f;
			Debug.Log("Loading %" + loadingProgress.progress);
	
			if(Application.isLoadingLevel)
			{
				if(loadingProgress.progress > 0.89f)
				{
					loadingProgress.allowSceneActivation = true;
					Debug.Log("Level is done loading!");
				}
			}			
			
		}
	}
	
	
	
	
}
