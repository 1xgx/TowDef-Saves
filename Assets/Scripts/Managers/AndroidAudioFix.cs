using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidAudioFix : MonoBehaviour
{
    private static AndroidAudioFix _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("setVolumeControlStream", 3); // 3 = STREAM_MUSIC
        }
#endif
    }
}
