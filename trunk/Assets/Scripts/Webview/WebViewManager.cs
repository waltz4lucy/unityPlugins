using System.Collections;
using UnityEngine;

public partial class WebViewManager : UnitySingleton<WebViewManager>
{
    public bool IsOpened { get { return null != webViewObject; } }

    void Update()
    {
        if (!IsOpened)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWebView();
            return;
        }
    }

    void OnGUI()
    {
        if (IsOpened)
        {
            int closeButtonMargin = (headerHeight - closeButtonSize) / 2;

            // header
            GUI.Box(new Rect(0, 0, Screen.width, headerHeight), string.Empty);

            // close button
            if (GUI.Button(new Rect(Screen.width - closeButtonSize - closeButtonMargin, closeButtonMargin, closeButtonSize, closeButtonSize), closeButtonTexture, GUIStyle.none))
            {
                CloseWebView();
            }
        }
    }

    public void OpenWebView(string url)
    {
        StartCoroutine(_OpenWebView(url, top: headerHeight));
    }

    private IEnumerator _OpenWebView(string url, int left = 0, int top = 0, int right = 0, int bottom = 0)
    {
#if UNITY_EDITOR
        Application.OpenURL(url);
        yield break;
#else
        CloseWebView();

        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

        webViewObject.Init(
            cb: (msg) =>
            {
                Debug.Log(string.Format("CallFromJS[{0}]", msg));
            },
            err: (msg) =>
            {
                Debug.Log(string.Format("CallOnError[{0}]", msg));
            },
            ld: (msg) =>
            {
                Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
#if !UNITY_ANDROID
                webViewObject.EvaluateJS(@"
                  window.Unity = {
                    call: function(msg) {
                      window.location = 'unity:' + msg;
                    }
                  }
                ");
#endif
                webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
            },
            enableWKWebView: true);

        webViewObject.SetMargins(left, top, right, bottom);
        webViewObject.SetVisibility(true);

#if !UNITY_WEBPLAYER
        if (url.EndsWith(".pdf"))
        {
#if UNITY_ANDROID
            webViewObject.LoadURL("https://drive.google.com/viewerng/viewer?embedded=true&url=" + url.Replace(" ", "%20"));
#else
            webViewObject.LoadURL(url.Replace(" ", "%20"));
#endif
        }
        else if (url.StartsWith("http"))
        {
            webViewObject.LoadURL(url.Replace(" ", "%20"));
        }
        else
        {
            var exts = new string[]
            {
                ".jpg",
                ".html"  // should be last
            };
            foreach (var ext in exts)
            {
                var uri = url.Replace(".html", ext);
                var src = System.IO.Path.Combine(Application.streamingAssetsPath, uri);
                var dst = System.IO.Path.Combine(Application.persistentDataPath, uri);
                byte[] result = null;
                if (src.Contains("://"))
                {
                    // for Android
                    var www = new WWW(src);
                    yield return www;
                    result = www.bytes;
                }
                else
                {
                    result = System.IO.File.ReadAllBytes(src);
                }
                System.IO.File.WriteAllBytes(dst, result);
                if (ext == ".html")
                {
                    webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
                    break;
                }
            }
        }
#else
        if (Url.StartsWith("http")) {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        } else {
            webViewObject.LoadURL("StreamingAssets/" + Url.Replace(" ", "%20"));
        }
        webViewObject.EvaluateJS(
            "parent.$(function() {" +
            "   window.Unity = {" +
            "       call:function(msg) {" +
            "           parent.unityWebView.sendMessage('WebViewObject', msg)" +
            "       }" +
            "   };" +
            "});");
#endif
        yield break;
#endif
    }

    public void CloseWebView()
    {
        GameObject.Destroy(webViewObject);
    }
}

#region Implements

public partial class WebViewManager
{
    [SerializeField]
    int headerHeight;

    [SerializeField]
    int closeButtonSize;

    [SerializeField]
    Texture closeButtonTexture;

    WebViewObject webViewObject;
}

#endregion
