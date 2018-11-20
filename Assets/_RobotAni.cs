using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _RobotAni : MonoBehaviour {

	 private  _RobotAni (){}
	 private static  _RobotAni mInstance;

	 public  float idleBreakTime=10;
	 public float longBreakTime=20;
	 private float idleTimer;

     private bool isIdel=true;
	 public GameObject robotConDo;
	 public DEMO demo;
	 public static  _RobotAni GetInstance()
	 {
		 
		 if (mInstance==null)
		 {
			 mInstance=new _RobotAni();
		 }
		 return mInstance;
	 }
	
	  void Update()
	  {
		if (isIdel&&!demo.ass.isPlaying)
		{  
			print("@@@");
			idleTimer-=Time.deltaTime;
			if (idleTimer<=0)
			{
				isIdel=false;
				IdleState();
			}
		} 
	  }

	  public void BreakIdelState()
	  {
		  isIdel=true;
		  idleTimer=idleBreakTime;
	  }
	public   void LongBreak()
	  {
		  ani.Stop();
		  demo.ass.Stop();
		  isIdel=true;
		  idleBreakTime=longBreakTime;
	  }
	private string[] selfComponentContent;
public Animation ani;
	void Start () {
		print(ani.gameObject.name);
		 idleTimer=longBreakTime;
	}
	public void DoAni(string action)
	{
		bool haveAni=false;
 
        foreach (AnimationState item in ani)
		{
			if (action.Contains(item.name))
			{
				ani.Play(item.name);
				if(robotConDo.activeSelf)
		   			robotConDo.SetActive(false);
                //LongBreak();
				haveAni=true;
			}
		}
		if(action.Contains("虚拟现实")||action.Contains("增强现实")||action.Contains("人工智能")){
			//显示图片
			_PictureShow.Instances.ShowPictureByString(action,"\t\t北京智同创科技有限公司是一家专业从事虚拟现实、增强现实、体感"+
			"交互和人工智能业务的高科技公司，致力于为各相关行业提供专业的VR、AR、体感互动和人工智能等系统的技术开发和产品定制服务，业务范围涵盖了教育、工业、医疗、商业和军事等多个领域。");
			demo.TextToSpeech("北京智同创科技有限公司是一家专业从事虚拟现实、增强现实、体感交互和人工智能业务的高科技公司，致力于为"+
			"各相关行业提供专业的VR、AR、体感互动和人工智能等系统的技术开发和产品定制服务，业务范围涵盖了教育、工业、医疗、商业和军事等多个领域。",false);
			ani.Play("介绍");
			if(robotConDo.activeSelf)
		   		robotConDo.SetActive(false);
			haveAni=false;
		}
		if (!haveAni)
		{
			if(robotConDo.activeSelf)
		   		robotConDo.SetActive(false);
			ani.Play("介绍");
		   	//LongBreak();
		}
	}
	private int lateRandom=-1;
	void IdleState()
	{
    BreakIdelState();
      //0引导，1询问。
	  int idleIndex=Random.Range(0,3);
	  if(idleIndex==lateRandom) return;
	  lateRandom=idleIndex;
	  print("State!!!：  "+idleIndex);
      switch (idleIndex)
	  {
		  case 0:{
           demo.TextToSpeech("Hello，我是聊天机器人，按下语音按钮可以和我聊天哦");
		   ani.Play("抬右手");
		    if(robotConDo.activeSelf)
		   		robotConDo.SetActive(false);
		  }
		  break;
		   case 1:{
           demo.TextToSpeech("你在干嘛呢？和我一起玩吧");
		   	if(robotConDo.activeSelf)
		   		robotConDo.SetActive(false);
		   ani.Play("疑问");
		  }
		  break;
		  case 2:{
		   demo.TextToSpeech("我知道的可不少呢，上知天文下知地理，背唐诗、讲故事，更是拿手好戏！",false);
		   ani.Play("跳舞");
		   robotConDo.SetActive(true);
		  }
		  break;
	  }

	}
}
