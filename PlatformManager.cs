using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SingletonPattern;

public class PlatformManager : Singleton<PlatformManager>
{
    #if (UNITY_ANDROID)
        public static Platform_Android   Platform { get; private set; }
    #elif (UNITY_IPHONE)
        public static Platform_IOS       Platform { get; private set; }
    #else
        public static Platform_Window    Platform { get; private set; }
    #endif

    public void Init()
    {
        Debug.Assert(!isInitiated, "This class cannot be initiate more than once!");
        isInitiated = true;
        
        #if (UNITY_ANDROID)
            Platform = Platform_Android.GetInstance;
        #elif (UNITY_IPHONE)
            Platform = Platform_IOS.GetInstance;
        #else
            Platform = Platform_Window.GetInstance;
        #endif
    }
}