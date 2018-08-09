using UnityEngine;
using System.Collections;
using System;

public class Globals : MonoBehaviour {

	public GameObject prefabEnemyShip;
	public GameObject prefabPlayerShip;
	private GameObject playerOne;
	public static int MaxNumberOfEnemies;
	
	public AudioClip[] menuClip;
	public Material matSpritesheet;
		
	public static bool isPause;
	public static bool isReady;
	public static bool isMenuInGame;
	
	private MenuButton[] menuButtonsInGame; 
	private const int MAXNUMBEROFENEMIES = 5;
	private const float MAINMENUBUTTONSIZE = 2f;
	
	
	void Start()
	{
		menuButtonsInGame = new MenuButton[4];
		
		menuButtonsInGame[0] = new MenuButton("Resume",209f,39f,matSpritesheet);
		menuButtonsInGame[0].SetBottonHighlight(0.581f,0.419f,0.168f,0.138f);
		menuButtonsInGame[0].SetBottonNeutral(0.581f,0.419f,0.1985f,0.1685f);
		menuButtonsInGame[0].SetSize(MAINMENUBUTTONSIZE);
		menuButtonsInGame[0].SetParent(Camera.mainCamera.transform);
		menuButtonsInGame[0].SetVerticalPosition(120f);
		menuButtonsInGame[0].ShowHighlight();
		
		menuButtonsInGame[1] = new MenuButton("Load",126f,39f,matSpritesheet);
		menuButtonsInGame[1].SetBottonHighlight(0.3f,0.2f,0.044f,0.014f);
		menuButtonsInGame[1].SetBottonNeutral(0.3f,0.2f,0.075f,0.045f);
		menuButtonsInGame[1].SetParent(Camera.mainCamera.transform);
		menuButtonsInGame[1].SetSize(MAINMENUBUTTONSIZE);
		menuButtonsInGame[1].SetVerticalPosition(60f);

		menuButtonsInGame[2] = new MenuButton("Options",203f,48f,matSpritesheet);
		menuButtonsInGame[2].SetBottonHighlight(0.464f,0.305f,0.09959f,0.06159f);
		menuButtonsInGame[2].SetBottonNeutral(0.464f,0.305f,0.1369f,0.099f);
		menuButtonsInGame[2].SetParent(Camera.mainCamera.transform);
		menuButtonsInGame[2].SetSize(MAINMENUBUTTONSIZE);
		menuButtonsInGame[2].SetVerticalPosition(-10f);
		
		menuButtonsInGame[3] = new MenuButton("Quit",110f,43f,matSpritesheet);
		menuButtonsInGame[3].SetBottonHighlight(0.55f,0.464f,0.0425f,0.0095f);
		menuButtonsInGame[3].SetBottonNeutral(00.55f,0.464f,0.076f,0.043f);
		menuButtonsInGame[3].SetParent(Camera.mainCamera.transform);
		menuButtonsInGame[3].SetSize(MAINMENUBUTTONSIZE);
		menuButtonsInGame[3].SetVerticalPosition(-80f);
		
		MenuButton.SetTagName("Menu");
		MenuButton.HideButtons();
		
		isReady = false;
		isMenuInGame = false;
		isPause = false;
		
		playerOne = Instantiate(prefabPlayerShip) as GameObject;
		playerOne.name = "Spaceship";
		
		for(int x=0;x < MAXNUMBEROFENEMIES; x++)
		{
			GameObject temp2 = Instantiate(prefabEnemyShip) as GameObject;
		}		
		

	}
	
	
	void OnDestroy()
	{
		Time.timeScale = 1;
		MenuButton.ResetStaticMembers();
	}
	
						
	void Update()
	{
		if(!isReady)
		{
			if(Input.anyKeyDown)
			{
				isReady = true;
			}	
		}
		
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(!isMenuInGame)
			{
				isMenuInGame = true;
				MenuButton.ShowButtons();
				PauseGame();
				
			}else
			{
				isMenuInGame = false;
				MenuButton.HideButtons();
				PauseGame();
			}
		}
		
		if(isMenuInGame)
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
			case 0: // RESUME BUTTON
				isMenuInGame = false;
				MenuButton.HideButtons();
				PauseGame();
		
				break;
				
			case 1: // LOAD BUTTON
				
				break;
				
			case 2: // OPTIONS BUTTON
			
				break;
				
			case 3: // QUIT BUTTON
				Application.LoadLevel("MainMenu");
				break;
				
			default:
				break;
			}
		}
		
		}
	}
	

	void PauseGame()
	{
		GameObject[] obj = (GameObject[]) GameObject.FindObjectsOfType(typeof(GameObject));
		if(!isPause)
		{
			foreach(GameObject o in obj)
			{
				if(o.audio)
				{
					if(o.name != "Globals")
					{
						o.audio.mute = true;
					}
				}
			}

			Time.timeScale = 0;
			isPause = true;
		}else
		{
			
			foreach(GameObject o in obj)
			{
				if(o.audio)
				{
					if(o.name != "Globals")
					{
						o.audio.mute = false;
					}
				}
			}
			
			Time.timeScale = 1;
			isPause = false;
		}
	}
	
	
}
