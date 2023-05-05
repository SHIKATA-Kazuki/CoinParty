using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public enum Size { Banner, IABBanner, LeaderBoard, MediumRectangle, SmartBanner}
public enum AdType { InterstitialAd, BannerAd}

public class AdmobScript : MonoBehaviour {

    public string AndroidAdID;
    public string IosAdId;

    public AdType _type;
    public Size _size;
    public AdPosition position;
    private BannerView bannerView;


    void Start () {
        RequestBanner();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    private void RequestBanner()
    {
        
#if UNITY_ANDROID
        string adUnitId = AndroidAdID;
#elif UNITY_IPHONE
        string adUnitId = IosAdId;
#else
        string adUnitId = "unexpected_platform";
#endif
        if (_type == AdType.BannerAd)
        {
            switch (_size)
            {
                case Size.Banner:
                    bannerView = new BannerView(adUnitId, AdSize.SmartBanner, position);
                    break;
                case Size.IABBanner:
                    bannerView = new BannerView(adUnitId, AdSize.IABBanner, position);
                    break;
                case Size.LeaderBoard:
                    bannerView = new BannerView(adUnitId, AdSize.Leaderboard, position);
                    break;
                case Size.MediumRectangle:
                    bannerView = new BannerView(adUnitId, AdSize.MediumRectangle, position);
                    break;
                case Size.SmartBanner:
                    bannerView = new BannerView(adUnitId, AdSize.SmartBanner, position);
                    break;

            }

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the banner with the request.
            bannerView.LoadAd(request);
        } else
        {
            // Initialize an InterstitialAd.
            InterstitialAd interstitial = new InterstitialAd(adUnitId);
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            interstitial.LoadAd(request);

        }

    }
}
