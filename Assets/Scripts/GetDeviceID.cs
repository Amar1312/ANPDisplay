using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class GetDeviceID : MonoBehaviour
{
    public TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
		string deviceid = SystemInfo.deviceUniqueIdentifier;
        _text.text = deviceid;

		try
		{
			File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/devicename.txt", deviceid);
            Camera.main.backgroundColor = Color.green;
		}
		catch
		{
            Camera.main.backgroundColor = Color.red;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
