using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class NativeAdHandler : MonoBehaviour
{
    public string NativeAdUnitId;
    NativeAd nativeAd;

    public RawImage AdIconTexture;
    public RawImage AdChoicesLogo;
    public Text AdHeadline;
    public Text AdDescription;
    public GameObject AdLoaded;
    public GameObject AdLoading;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            RequestNativeAd();
        });
    }

    private void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder(NativeAdUnitId)
            .ForNativeAd()
            .Build();
        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        adLoader.LoadAd(new AdRequest.Builder().Build());
    }
    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Native ad failed to load: " + args.LoadAdError.GetMessage());
    }


    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    {
        Debug.Log("Native ad loaded.");
        this.nativeAd = args.nativeAd;

        //set textures and details
        AdIconTexture.texture = nativeAd.GetIconTexture();
        AdHeadline.text = nativeAd.GetHeadlineText();
        AdDescription.text = nativeAd.GetBodyText();
        AdChoicesLogo.texture = nativeAd.GetAdChoicesLogoTexture();
        //register gameobjects with native ads api
        if (!nativeAd.RegisterIconImageGameObject(AdIconTexture.gameObject))
        {
            Debug.Log("error registering icon");
        }
        if (!nativeAd.RegisterHeadlineTextGameObject(AdHeadline.gameObject))
        {
            Debug.Log("error registering headline");
        }
        if (!nativeAd.RegisterBodyTextGameObject(AdDescription.gameObject))
        {
            Debug.Log("error registering description");
        }

        //disable loading and enable ad object
        AdLoaded.gameObject.SetActive(true);
        AdLoading.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

    }
}