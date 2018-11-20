using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameQuit : MonoBehaviour {
	public GameObject quitText;
	private int index=0;
	private bool isStartTimer;
	private float time;
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			quitText.SetActive(true);
			index=1;
			isStartTimer=true;
			quitText.GetComponentInChildren<Text>().text="再按一次退出程序";
		}
		if(isStartTimer){
			time+=Time.deltaTime;
			if(time>=0.05f&&time<=1.5f){
				if(Input.GetKeyDown(KeyCode.Escape))
					index=2;
			}else if(time>1.5f){
				quitText.SetActive(false);
				isStartTimer=false;
				time=0;
			}
		}
		if(index>=2){
			Application.Quit();
			index=0;	
		}
	}
}