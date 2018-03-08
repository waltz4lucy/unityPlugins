using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

#region iOS

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

#endregion

#region Android

public partial class UnityNative
{
#if UNITY_ANDROID

#endif
}

#endregion
