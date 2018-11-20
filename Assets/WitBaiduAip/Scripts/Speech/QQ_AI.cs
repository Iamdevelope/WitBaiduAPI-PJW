using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
//using System.Drawing;

namespace YYAI
{
	public class QQ_AI
	{
		//在腾讯AI平台注册应用,在下面填入应用的Appid与AppKey即可使用，地址:https://ai.qq.com/product/nlptrans.shtml#text
		public  static string appId = "1106914164"; //AppID
		public  static string appKey = "eTz8BXMeMvtLQ20a"; //AppKey
		public static string session="10000"; 
		public static string time_stamp="";
		public static string noce_str="";
		public static string sign="";



      public  static string GetVoiceEchoSign(string speech64)
	  {
		  Debug.Log("  voice");
          time_stamp=GetTimeStamp();
		  noce_str=GetNonceStr();

       	SortedDictionary<string, string> di = new SortedDictionary<string, string>();
			di.Add("app_id", appId);
			di.Add("format","2");
			di.Add("rate", "16000");
			di.Add("speech",speech64);
			di.Add("time_stamp", time_stamp);
			di.Add("nonce_str", noce_str);
			string signStr = GetSign(di);
			di.Add("sign", signStr);
			string retdata = GetDiValue(di);

			return signStr;
		//	return retdata;
	  }
	  public static string GetT2SSign(string text,string aht,string apc,string speaker )
	  {
		  time_stamp=GetTimeStamp();
		  noce_str=GetNonceStr();
        	SortedDictionary<string, string> di = new SortedDictionary<string, string>();
              di.Add("app_id",appId);
			  di.Add("time_stamp",time_stamp);
			   di.Add("nonce_str",noce_str);
                di.Add("speaker",speaker);
				di.Add("format","2");
				di.Add("volume","0");
				di.Add("speed","100");
				di.Add("text",text);
				di.Add("aht",aht);
				di.Add("apc",apc);
				Debug.Log(aht+"  "+apc+"  "+speaker);
				string t2ssign=GetSign(di);
				di.Add("sign",t2ssign);
				string retdata=GetDiValue(di);
				Debug.Log(" re!!:  "+retdata);
				return retdata;
			
	  }
		public static string GetOCR(string text)
		{
			Debug.Log("OCR");
			SortedDictionary<string, string> di = new SortedDictionary<string, string>();
			di.Add("app_id", appId);
			di.Add("session", session);
			di.Add("question", text);
			di.Add("time_stamp", GetTimeStamp());
			di.Add("nonce_str", GetNonceStr());
		
		//	di.Add("target", target);
			string signStr = GetSign(di);
			di.Add("sign", signStr);
			string retdata = GetDiValue(di);
			 time_stamp=GetTimeStamp();
			 noce_str=GetNonceStr();
			 sign=signStr;
			return retdata;
		}

		public static string GetTimeStamp()
		{
			System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
			return ((int)(DateTime.Now - startTime).TotalSeconds).ToString();
		}

		public static string GetNonceStr()
		{
			return Guid.NewGuid().ToString("N");
		}

		public static string GetDiValue(SortedDictionary<string, string> di)
		{
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, string> kv in di)
			{
				if (sb.Length > 0)
					sb.Append('&');
				sb.Append(URLdi(kv.Key, kv.Value));
			}
			return sb.ToString();
		}

		public static string GetSign(SortedDictionary<string, string> di)
		{
			string str = GetDiValue(di);
			str += "&app_key=" + appKey;
			
			var md5 = MD5.Create();
			var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
			StringBuilder sb = new StringBuilder();
			foreach (byte b in bs)
			{
				sb.Append(b.ToString("x2"));
			}
			//所有字符转为大写
			Debug.Log(sb.ToString().ToUpper());
			return sb.ToString().ToUpper();
		}

		public static string URLdi(string key, string value)
		{
			return string.Format("{0}={1}", key, URL_BM(value));
		}

		public static string URL_BM(string text)
		{
			byte[] utf;
			Encoding ed = Encoding.GetEncoding("utf-8");
			utf = ed.GetBytes(text);
			text = "";
			text = "%" + BitConverter.ToString(utf, 0).Replace("-", "%");
						return URL还原(text);
		}

		/// <summary>
		/// 将大小写字母跟数字以及一些特殊字符进行URL解码
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string URL还原(string text)
		{
			text = System.Text.RegularExpressions.Regex.Replace(text, "%2E", ".");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%2D", "-");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%5F", "_");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%20", "+");

			text = System.Text.RegularExpressions.Regex.Replace(text, "%61", "a");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%62", "b");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%63", "c");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%64", "d");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%65", "e");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%66", "f");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%67", "g");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%68", "h");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%69", "i");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%6A", "j");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%6B", "k");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%6C", "l");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%6D", "m");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%6E", "n");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%6F", "o");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%70", "p");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%71", "q");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%72", "r");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%73", "s");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%74", "t");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%75", "u");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%76", "v");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%77", "w");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%78", "x");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%79", "y");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%7A", "z");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%41", "A");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%42", "B");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%43", "C");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%44", "D");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%45", "E");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%46", "F");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%47", "G");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%48", "H");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%49", "I");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%4A", "J");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%4B", "K");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%4C", "L");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%4D", "M");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%4E", "N");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%4F", "O");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%50", "P");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%51", "Q");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%52", "R");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%53", "S");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%54", "T");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%55", "U");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%56", "V");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%57", "W");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%58", "X");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%59", "Y");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%5A", "Z");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%30", "0");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%31", "1");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%32", "2");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%33", "3");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%34", "4");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%35", "5");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%36", "6");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%37", "7");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%38", "8");
			text = System.Text.RegularExpressions.Regex.Replace(text, "%39", "9");

			return text;
		}


	}
}