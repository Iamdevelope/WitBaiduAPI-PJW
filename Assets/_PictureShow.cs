using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _PictureShow : MonoBehaviour {
public static _PictureShow Instances;
private Dictionary<string,string> picContentAndName=new Dictionary<string,string>();
public Image image;
private Text text;
void Awake(){
	if(Instances==null)
		Instances=this;
}
void Start(){
	text=image.gameObject.GetComponentInChildren<Text>();
	Configure();
	image.gameObject.SetActive(false);
}
public void ShowPictureByString(string msg,string content){
	foreach(var key in picContentAndName.Keys){
		if(msg.Contains(key.ToString()))
			ShowPicture(picContentAndName[key.ToString()],content);
	}
}
private void ShowPicture(string imgName,string content){
	text.text=content;
	Sprite temp=Resources.Load<Sprite>("Texture/"+imgName);
	image.sprite=temp;
	//StartCoroutine(CloseImage());
	image.gameObject.SetActive(true);
}
public IEnumerator CloseImage(){
	if(image.gameObject.activeSelf){
		yield return new WaitForSeconds(10);
		image.gameObject.SetActive(false);
	}
}
private void Configure(){
	picContentAndName.Add("虚拟现实","Logo");
	picContentAndName.Add("人工智能","Logo");
	picContentAndName.Add("增强现实","Logo");
	
}
}
