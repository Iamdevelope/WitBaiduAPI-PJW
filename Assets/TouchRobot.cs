using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchRobot : MonoBehaviour {
	private Ray ray;
	private RaycastHit hit;
	private Animation ani;
	private float time;
	private bool isStartTime;
	public bool isFirst;
	public GameObject robotConDo;
	public DEMO demo;
	private void Start(){
		ani=GetComponent<Animation>();
	}
	private void Update(){
#if UNITY_ANDROID||UNITY_IOS
		ray=Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#else
		ray=Camera.main.ScreenPointToRay(Input.mousePosition);
#endif
		if(Physics.Raycast(ray,out hit,100)){
			if(hit.collider.tag=="Robot"){
				PlayAnim();
				isStartTime=true;
			}
		}else{
				isFirst=false;
		}
		if(isStartTime){
			time+=Time.deltaTime;
			if(time>=0.5f){
				if(isFirst)
					isFirst=false;
				isStartTime=false;
				time=0;
			}
		}
	}
	private void PlayAnim(){
		if(isFirst)return;
		
		int random=Random.Range(0,4);
		switch (random)
	  {
		  case 0:{
			demo.TextToSpeech("你好，想要跟我一起玩吗？");
		   	if(robotConDo.activeSelf)
		   		robotConDo.SetActive(false);
		   ani.Play("抬右手");
		  }
		  break;
		   case 1:{
		   ani.Play("疑问");
		  }
		  break;
		  case 2:{
			demo.TextToSpeech("来跳支舞吧");
		   	if(robotConDo.activeSelf)
		   		robotConDo.SetActive(false);
		   ani.Play("跳舞");
		  }
		  break;
		  case 3:{
			demo.TextToSpeech("讨厌了，不要摸我。");
		   	if(robotConDo.activeSelf)
		   		robotConDo.SetActive(false);
		   ani.Play("偷笑");
		  }
		  break;
	  }
	  isFirst=true;
	}
}
