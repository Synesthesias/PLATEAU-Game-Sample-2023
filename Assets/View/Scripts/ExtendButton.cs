using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ボタン
/// </summary>
public class ExtendButton : MonoBehaviour
{
    public UnityEngine.UI.Button ButtonInternal;
    public UnityEngine.UI.Image ImageInternal;
    public UnityEngine.UI.Text TextInternal;

    /// <summary>
    /// 表示スプライト
    /// </summary>
    public Sprite Sprite
    {
        get => ImageInternal?.sprite;
        set
        {
            if (ImageInternal != null)
                ImageInternal.sprite = value;
        }
    }

    /// <summary>
    /// 表示テキスト
    /// </summary>
    public string Text
    {
        get => TextInternal?.text;
        set
        {
            if (TextInternal != null)
                TextInternal.text = value;
        }
    }

    /// <summary>
    /// クリックされているか
    /// </summary>
    public bool IsClicked { get; set; }

    void Start()
    {
        if (ButtonInternal != null)
        {
            ButtonInternal.onClick.AddListener(OnClick);
        }
    }

    void Update()
    {
        //IsClicked = false;
    }

    void OnClick()
    {
        IsClicked = true;
        Debug.Log("ボタンクリック");
    }
}
