using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public partial class UnityNative
{
#if !UNITY_EDITOR && UNITY_IPHONE
    [DllImport("__Internal")]
    static extern string IOSNativeGetMcc();
#endif
    public static string GetMcc()
    {
#if UNITY_EDITOR
        return "450";
#elif UNITY_ANDROID
        return AndroidAction(javaClass =>
        {
            return javaClass.CallStatic<string>("GetMcc");
        });
#elif UNITY_IPHONE
        return IOSNativeGetMcc();
#endif
    }
}

#region Implements

public partial class UnityNative
{
#if UNITY_ANDROID
    static void AndroidAction(System.Action<AndroidJavaClass> action)
    {
        using (var javaClass = GetJavaClass())
        {
            action(javaClass);
        }
    }

    static T AndroidAction<T>(System.Func<AndroidJavaClass, T> action)
    {
        using (var javaClass = GetJavaClass())
        {
            return action(javaClass);
        }
    }

    static AndroidJavaClass GetJavaClass()
    {
        return new AndroidJavaClass(NexusPersonalConfig.Instance.bundleIdentifier + ".library.NexusAndroidNative");
    }
#endif
}

#endregion
