using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class loadimage : MonoBehaviour
{
	// Start is called before the first frame update

	public RawImage _image;

	// Update is called once per frame
	void Update()
	{

	}


	private void Start()
	{
        byte[] byteArray = File.ReadAllBytes(@"D:\Personal\Images\1.JPG");
        //create a texture and load byte array to it
        // Texture size does not matter 
        Texture2D sampleTexture = new Texture2D(2, 2);
        // the size of the texture will be replaced by image size
        bool isLoaded = sampleTexture.LoadImage(byteArray);
        // apply this texure as per requirement on image or material

        _image.texture = sampleTexture;

        /*
        GameObject image = GameObject.Find("RawImage");
        if (isLoaded)
        {
            image.GetComponent<RawImage>().texture = sampleTexture;
        }*/
    }


	/*
    IEnumerator Start()
    {
        WWW www = new WWW("file:///D://Personal//Images//1.jpg");
        while (!www.isDone)
            yield return null;
        //GameObject image = GameObject.Find("RawImage");
        _image.texture = www.texture;
    }*/
}
