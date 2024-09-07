#define dummy
using System.Collections;
using Sonat;
using UnityEngine;

public partial class AdsInstanceMaxLovin
{
#if !((dummy || global_dummy) && !use_max)

    private void RequestBannerNative(bool register = false)
    {
        if(!LoadNative)
            return;
        
        UIDebugLog.Log(nameof(RequestBannerNative));
        if (_initialized && !AdsManager.NoAds)
        {
//            MaxSdk.CreateMRec(NativeBannerId, MaxSdkBase.AdViewPosition.BottomCenter);
            if (register)
            {
                MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnNativeBannerAdLoadedEvent;
                MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnNativeBannerAdLoadFailedEvent;
                MaxSdkCallbacks.Banner.OnAdClickedEvent += OnNativeBannerAdClickedEvent;
                MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnNativeBannerAdRevenuePaidEvent;
            }
            MaxSdk.CreateMRec(NativeBannerId, MaxSdkBase.AdViewPosition.BottomCenter);
            _nativeBannerState = AdsState.Requesting;
        }
    }
    
    private void OnNativeBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log(nameof(OnNativeBannerAdClickedEvent));
    }

    private void OnNativeBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        Debug.Log(nameof(OnNativeBannerAdLoadedEvent));
        _nativeBannerState = AdsState.Loaded;
        nativeAdapter = info.NetworkName; // _bannerView.GetResponseInfo().GetMediationAdapterClassName();
        OnBannerShow.Action.Invoke(true);
    }

    private void OnNativeBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        Debug.Log(nameof(OnNativeBannerAdLoadFailedEvent));
        Debug.Log($"NativeFailed : {errorInfo.Message}");
    }

    private void OnNativeBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        SonatAnalyticTracker.LogRevenue(AdsPlatform.applovinmax, nativeAdapter, adInfo.Revenue, adInfo.RevenuePrecision, AdTypeLog.mrec,
            Kernel.Resolve<FireBaseController>().FirebaseInstanceId,
            "native_banner");
    }

    private IEnumerator IeRequestNativeBanner(float time,bool init = false)
    {
        UIDebugLog.Log("IeRequestNativeBanner",false, LogType.Ads);
        yield return new WaitForSeconds(time);
        RequestBannerNative(init);;
    }
  
    public override bool IsNativeReady()
    {
        #if UNITY_EDITOR
        return true;
        #endif
      return _nativeBannerState == AdsState.Loaded || _nativeBannerState == AdsState.Hidden;
    }

    public override void HideNativeBanner()
    {
//        UIDebugLog.Log($"{nameof(HideNativeBanner)} {_nativeBannerState}");
        if (_nativeBannerState == AdsState.Showing)
        {
            MaxSdk.HideMRec(NativeBannerId);
            _nativeBannerState = AdsState.Hidden;
        }
    }

    public override void ShowNativeBanner()
    {
        #if UNITY_EDITOR
            MaxSdk.ShowMRec(NativeBannerId);
            _nativeBannerState = AdsState.Loaded;
            
            return;
        #endif
        
        UIDebugLog.Log($"{nameof(ShowNativeBanner)} {_nativeBannerState}");
        if (_nativeBannerState == AdsState.Hidden || _nativeBannerState == AdsState.Loaded)
        {
            MaxSdk.ShowMRec(NativeBannerId);
            _nativeBannerState = AdsState.Showing;
        }
    }
    


    public void DestroyNativeBanner()
    {
        _nativeBannerState = AdsState.NotStart;
        OnBannerShow.Action.Invoke(false);
        if(!string.IsNullOrEmpty(NativeBannerId))
            MaxSdk.DestroyMRec(NativeBannerId);
    }
#endif
}