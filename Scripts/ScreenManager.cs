using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {
	
	private int screenWidthFullscreen;
	private int screenHeightFullscreen;
	private int screenResolutionIndex;
	
	// Use this for initialization
	void Start () {
		
		Screen.SetResolution(1280,768,true);
		Screen.showCursor = false;
		
		screenResolutionIndex = 0;
		
		screenWidthFullscreen = Screen.width;
		screenHeightFullscreen = Screen.height;
		
	}
	
	// Update is called once per frame
	void Update () { 
		
		// Switch fullscreen or window mode.
		if(Input.GetKeyDown(KeyCode.F11))
		{
			if(!Screen.fullScreen){
				Screen.SetResolution(screenWidthFullscreen,screenHeightFullscreen,true);
			}
			else{
				Screen.SetResolution(800,600,false);
				Camera.mainCamera.orthographicSize = 300;
			}				
		}
		
//		if(Input.GetKeyDown(KeyCode.Comma))
//		{
//			screenResolutionIndex++;
//			if(screenResolutionIndex == Screen.resolutions.Length)
//			{
//				screenResolutionIndex = Screen.resolutions.Length;
//			}else
//			{
//				Screen.SetResolution(Screen.resolutions[screenResolutionIndex].width,
//									 Screen.resolutions[screenResolutionIndex].height,
//									 Screen.fullScreen);
//			}
//			
//							
//		}
//		if(Input.GetKeyDown(KeyCode.Period))
//		{
//			screenResolutionIndex--;
//			if(screenResolutionIndex == 0)
//			{
//				screenResolutionIndex = 0;
//			}else
//			{
//				Screen.SetResolution(Screen.resolutions[screenResolutionIndex].width,
//									 Screen.resolutions[screenResolutionIndex].height,
//									 Screen.fullScreen);
//			}
//		}
			
	}
}
