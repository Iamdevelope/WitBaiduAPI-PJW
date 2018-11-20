using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using YYAI;
using UnityEngine.UI;
using System;

#region  读取类
//读取JSON用的类
[System.Serializable]
public class Data{
	public string session;
	public string answer;
}

[System.Serializable]
public class Res{
	public int ret;
	public string msg;
	public Data data;
}
[System.Serializable]
public class TTSDate{
public string format;
public string speech;
}
public class TTS{
	public int ret;
	public string msg;
	public TTSDate data;
}
[System.Serializable]
public class ASR{
	
	public int ret;
	public string msg;
	public ASRDate data;
}
[System.Serializable]
public class ASRDate {
public string format;
public string rate;

public string text;
}




#endregion

public class DEMO : MonoBehaviour {
    public Text showText;
	public InputField input;
	// Use this for initialization
	string chatAPI="https://api.ai.qq.com/fcgi-bin/nlp/nlp_textchat";
    string ttsAPI="https://api.ai.qq.com/fcgi-bin/aai/aai_tts";
	string asrAPI="https://api.ai.qq.com/fcgi-bin/aai/aai_asr";
    string myword="";
	public string herword="";
    string voiceSign="";
	public AudioSource ass;
	public _RobotAni rr;


	//文本框显示时长
    private float textShowTime=15f;
    private bool isShow;
    private float time;
	public string temp1;
	//人是否正在说话，人如果正在说话，则将前面从网上下载的资源关闭
	public bool isSpeech;
	public Coroutine tt;
	public Coroutine subString;

	void Start () {
       showText.transform.parent.gameObject.SetActive(false);
	}
	void Update(){
		if(isShow){
            time+=Time.deltaTime;
            if(time>=textShowTime){
                showText.transform.parent.gameObject.SetActive(false);
                time=0;
                isShow=false;
            }
        }
	}
     public void MyWordInput()
	 {
		 myword=input.text;
		 SendMessageGG(myword);
	 }
    public void SendMessageGG(string sayingword)
	{

		rr.DoAni(sayingword);
		if(sayingword.Contains("虚拟现实")||sayingword.Contains("增强现实")||sayingword.Contains("人工智能"))
			return;
        string data=QQ_AI.GetOCR(sayingword);
		StartCoroutine(StartPost(chatAPI+"?"+data));		
	}
	IEnumerator StartPost(string url)
	{
       WWWForm from=new WWWForm();
		WWW getData=new WWW(url,from);
		yield return getData;
		if (getData.error!=null)
		{
			Debug.Log(getData.error);
		}else{
			Debug.Log(getData.text);
			string temp;
			Debug.Log(JsonUtility.FromJson<Res>(getData.text).data.answer);
			//对字符进行切割读取
			TextToSpeech(JsonUtility.FromJson<Res>(getData.text).data.answer);
         //  herword=JsonUtility.FromJson<Res>(getData.text).data.answer;
        //    StartCoroutine(TTS());
		}

	}

	public void OnStartButton()
	{
		_RobotAni.GetInstance().LongBreak();
		 ass.Stop();
		Microphone.End(null);
        ass.clip=Microphone.Start(null,false,10,16000);

	}
	public void OnStopButton()
	{
		byte[] clipData=WavUtility.FromAudioClip(ass.clip);
		//ass.clip.SetData();
	    string b64=Convert.ToBase64String(clipData);
			
		voiceSign=QQ_AI.GetVoiceEchoSign(b64);
			
    
		StartCoroutine(ASR(asrAPI,b64)); 
		
			Microphone.End(null);
       
	}
	IEnumerator ASR(string url,string b64) {
          WWWForm form=new WWWForm();
		  form.AddField("app_id",QQ_AI.appId);
		  form.AddField("format","2");
		  form.AddField("rate","16000");
		  form.AddField("speech",b64);
		  form.AddField("time_stamp",QQ_AI.time_stamp);
		  form.AddField("nonce_str",QQ_AI.noce_str);
		  form.AddField("sign",voiceSign);
        UnityWebRequest www = UnityWebRequest.Post(url,form);
	
		www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	
	//	UnityWebRequest.Post();
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log(www.downloadHandler.text);
			myword=JsonUtility.FromJson<ASR>(www.downloadHandler.text).data.text;
			
			SendMessageGG(myword);
        }
    }
	public string aht="0",apc="58",speaker="0";
	public IEnumerator TTS(bool isActive=true) {
		
		

		print(herword+" her");
        UnityWebRequest www = UnityWebRequest.Get(ttsAPI+"?"+QQ_AI.GetT2SSign(herword,aht,apc,speaker));
		www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
       	yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
         	Debug.Log(www.error);
        }
        else {
         	Debug.Log(www.downloadHandler.text);

			string b64=	JsonUtility.FromJson<TTS>(www.downloadHandler.text).data.speech;
			byte[] temp=Convert.FromBase64String(b64);
			AudioClip tempAudio=WavUtility.ToAudioClip(temp);
			showText.transform.parent.gameObject.SetActive(isActive);
			showText.text=herword;
			ass.clip=tempAudio;
			ass.Play();
			//打开计时器，一定时间后关闭文本提示框
			if(!isShow)
				isShow=true;
			time=0;
			//在读完当前文字后，开启协程截取下一段字符进行读取
			subString = StartCoroutine(SubStringToRead(temp1,tempAudio.length-1f,isActive));
    	}
    }
	public void TextToSpeech(string text,bool isActive=true)
	{
		if(text==null)return;
		temp1=text;
		if(temp1.Length>=50){
			herword=temp1.Substring(0,50);
		}
		else{
        	herword=temp1;
			StartCoroutine(_PictureShow.Instances.CloseImage());
		}
		if(!isSpeech)
			tt = StartCoroutine(TTS(isActive));
	}
	public IEnumerator SubStringToRead(string text,float time,bool isActive=true){
		if(!isActive)Debug.Log(text);
		yield return new WaitForSeconds(time);
		if(text!=null){
			if(text.Length>=50)
				TextToSpeech(text.Remove(0,50),isActive);
			else
				TextToSpeech(text.Remove(0,text.Length),isActive);
		}
	}
}
