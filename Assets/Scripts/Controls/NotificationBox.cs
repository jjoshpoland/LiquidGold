using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class NotificationBox : MonoBehaviour
{
    public TMP_Text message;
    RectTransform rect;
    Image[] images;
    Image image;
    public bool isSingleton;
    public static NotificationBox singleton;
    private void Awake()
    {
        if(isSingleton)
        {
            singleton = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        images = GetComponentsInChildren<Image>();
        image = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Called by unity event
    /// </summary>
    /// <param name="good"></param>
    public void SetSurplusMessage(Good good)
    {
        string text = "<sprite index=" + good.SpriteIndex + "> Surplus";
        message.color = Color.red;
        SetMessage(text);
    }
    /// <summary>
    /// Called by unity event
    /// </summary>
    /// <param name="good"></param>
    public void SetCrashMessage(Good good)
    {
        string text = "<sprite index=" + good.SpriteIndex + "> Shortage";
        message.color = Color.green;
        SetMessage(text);
    }

    public void SetMessage(string text)
    {
        message.text = text;
        Show();
        
    }

    void Show()
    {
        foreach(Image i in images)
        {
            i.color = Color.white;
        }
        image.color = Color.white;
        FadeOut();
    }

    void FadeOut()
    {
        foreach(Image i in images)
        {
            i.DOFade(0, 10);
        }
        image.DOFade(0, 10);
        message.DOFade(0, 10);
    }
}
