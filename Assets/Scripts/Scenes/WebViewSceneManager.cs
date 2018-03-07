using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WebViewSceneManager : MonoBehaviour
{
    public void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {

    }

    public void OnDestroy()
    {
        WebViewManager.DestroyInstance();
    }
}
