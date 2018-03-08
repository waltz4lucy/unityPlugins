using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WebViewSceneManager : MonoBehaviour
{
    public Button button;
    public InputField inputField;
    public Text text;

    private string url = "https://github.com/waltz4lucy/unityPlugins";

    public void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        button.onClick.AddListener(delegate {
            OnClick();
        });

        inputField.onEndEdit.AddListener(delegate
        {
            OnEndEdit(text.text);
        });
    }

    public void OnDestroy()
    {
        WebViewManager.DestroyInstance();
    }

    public void OnClick()
    {
        WebViewManager.Instance.OpenWebView(url);
    }

    public void OnEndEdit(string str)
    {
        url = str;
    }
}
