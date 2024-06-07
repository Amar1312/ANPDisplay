using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;
using UnityEngine.UI;

using UnityEngine.Windows.Speech;
using TMPro;

public class UnityWinTTS : MonoBehaviour
{
    public TMP_Text txt;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonPress();
        }*/

    }

    public void ButtonPress(string txt)
    {
        SpVoice voice;
        voice = new SpVoice();
        //voice.Speak("Hello.");
        voice.Speak("Next item is.");
        voice.Speak(txt.ToString());
        /*
        char[] myChars = txt.ToCharArray();
        for(int i = 0; i < myChars.Length; i++)
		{
            voice.Speak(myChars[i].ToString());
		}*/
    }

    public void ButtonPressPrevious(string txt)
    {
        SpVoice voice;
        voice = new SpVoice();
        //voice.Speak("Hello.");
        voice.Speak("Previous item is.");
        voice.Speak(txt.ToString());

        /*
        char[] myChars = txt.ToCharArray();
        for (int i = 0; i < myChars.Length; i++)
        {
			try
			{
				voice.Speak(myChars[i].ToString());
			}
			catch
			{
                Debug.Log("cant read");
			}
            
        }*/
    }
}
