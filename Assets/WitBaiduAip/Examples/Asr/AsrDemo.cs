

using UnityEngine;
using UnityEngine.UI;
using Wit.BaiduAip.Speech;
using UnityEngine.EventSystems;
public class AsrDemo : MonoBehaviour
{
    public string APIKey = "";
    public string SecretKey = "";
    public Button StartButton;
    public Button StopButton;
    public Text DescriptionText;

    private AudioClip _clipRecord;
    private Asr _asr;
    public DEMO dd;
    private int isFirst=0;
    //文本框显示时长
    private float textShowTime=10f;
    private bool isShow;
    private float time;
    void Start()
    {
        _asr = new Asr(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

        StartButton.gameObject.SetActive(true);
        StopButton.gameObject.SetActive(false);
        DescriptionText.text = "";
        DescriptionText.transform.parent.gameObject.SetActive(false);

        //StartButton.onClick.AddListener(OnClickStartButton);
        

        //StopButton.onClick.AddListener(OnClickStopButton);
    }

    private void Update(){
        if(isShow){
            time+=Time.deltaTime;
            if(time>=textShowTime){
                DescriptionText.transform.parent.gameObject.SetActive(false);
                time=0;
                isShow=false;
            }
        }
        if(StartButton.GetComponent<_ButtonOnPointer>().isDown&&isFirst<1){
            OnClickStartButton();
            isFirst+=1;
        }
        else if(!StartButton.GetComponent<_ButtonOnPointer>().isDown&&isFirst>=1){
            OnClickStopButton();
            isFirst=0;
        }
    }
    private void OnClickStartButton()
    {
        //StartButton.gameObject.SetActive(false);
        //StopButton.gameObject.SetActive(true);
        DescriptionText.text = "正在获取...";
        //关闭机器人说话
        if(dd.tt!=null)
            StopCoroutine(dd.tt);
        if(dd.subString!=null)
            StopCoroutine(dd.subString);
        dd.temp1=null;
        dd.showText.gameObject.transform.parent.gameObject.SetActive(false);
        _PictureShow.Instances.image.gameObject.SetActive(false);
        dd.ass.clip=null;
        dd.ass.enabled=false;
        dd.isSpeech=true;
        
        _clipRecord = Microphone.Start(null, false, 30, 16000);
    }

    private void OnClickStopButton()
    {
        //StartButton.gameObject.SetActive(false);
        //StopButton.gameObject.SetActive(false);
        //打开机器人说话
        dd.ass.enabled=true;
        dd.isSpeech=false;
        isShow=true;
        time=0;
        DescriptionText.transform.parent.gameObject.SetActive(true);

        DescriptionText.text = "正在识别...";
        Microphone.End(null);
        Debug.Log("end record");
        var data = Asr.ConvertAudioClipToPCM16(_clipRecord);
        
        StartCoroutine(_asr.Recognize(data, s =>
        {
            DescriptionText.text = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";
          
            //StartButton.gameObject.SetActive(true);
            if(s.result!=null)
              dd.SendMessageGG(s.result[0]);
        }));
    }
}