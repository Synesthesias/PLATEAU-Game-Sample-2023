using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �{�^��
/// </summary>
public class ExtendButton : MonoBehaviour
{
    public UnityEngine.UI.Button ButtonInternal;
    public UnityEngine.UI.Image ImageInternal;
    public UnityEngine.UI.Text TextInternal;

    /// <summary>
    /// �\���X�v���C�g
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
    /// �\���e�L�X�g
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
    /// �N���b�N����Ă��邩
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
        Debug.Log("�{�^���N���b�N");
    }
}
