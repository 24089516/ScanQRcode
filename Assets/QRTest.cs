using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QRTest : MonoBehaviour
{

    WebCamTexture mWebCamTexture;
    public RawImage rawImage;
    public Text txt;

    bool isCheck;
    BarcodeReader mReader;
    Color32[] data;

    float mInterval = 0.3f;
    float last = 0;
    // Start is called before the first frame update
    void Start()
    {
        rawImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheck)
        {
            last += Time.deltaTime;
            if (last >= mInterval)
            {
                last = 0;
                DecodeQR();
            }
        }

    }
    public void BtnCheckOnClick()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        foreach (var item in devices)
        {
            if (!item.isFrontFacing)
            {
                mWebCamTexture = new WebCamTexture(item.name, Screen.width, Screen.height);
                rawImage.texture = mWebCamTexture;
                rawImage.gameObject.SetActive(true);
                mWebCamTexture.Play();
                mReader = new BarcodeReader();
                isCheck = true;
                break;
            }
        }
    }

    void DecodeQR()
    {
        // create a reader with a custom luminance source
        data = mWebCamTexture.GetPixels32();
            // decode the current frame
        var result = mReader.Decode(data, mWebCamTexture.width, mWebCamTexture.height);
        if (result != null)
        {
            txt.text = result.Text;

            mWebCamTexture.Stop();
            mWebCamTexture = null;
            rawImage.texture = null;
            rawImage.gameObject.SetActive(false);
            mReader = null;
            isCheck = false;

        }
        // Sleep a little bit and set the signal to get the next frame
    }

}
