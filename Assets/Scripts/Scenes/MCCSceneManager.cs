using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MCCSceneManager : MonoBehaviour
{
    public Button button;
    public Text text;

    public void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        button.onClick.AddListener(delegate {
            OnClick();
        });
    }

    public void OnClick()
    {
        text.text = UnityNative.GetMcc();
    }
}
