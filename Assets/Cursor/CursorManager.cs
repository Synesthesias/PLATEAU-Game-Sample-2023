using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CursorManager: MonoBehaviour
{
    public Image cursorImage;              // UI Image
    public Sprite defaultCursorSprite;     // �ʏ�̃J�[�\���̃X�v���C�g
    public Sprite highlightCursorSprite;   // ���点�����J�[�\���̃X�v���C�g

    public void OnEnable()
    {
        // Mouse�f�o�C�X�̃g���b�L���O��L���ɂ���
        InputSystem.EnableDevice(Mouse.current);
         Cursor.visible = false;
    }

    public void OnDisable()
    {
        // Mouse�f�o�C�X�̃g���b�L���O�𖳌��ɂ���
        InputSystem.DisableDevice(Mouse.current);
    }

    public void Update()
    {
        // Mouse�̈ʒu���擾���ăJ�[�\���̈ʒu���X�V
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        cursorImage.rectTransform.position = mousePosition;
        cursorImage.sprite = highlightCursorSprite;
        
  
    }

    bool ShouldHighlightCursor()
    {
        // ���点���������������ɒǉ�
        // ��: ����̃I�u�W�F�N�g�Ƀ}�E�X���d�Ȃ��Ă���ꍇ�Ȃ�
        return false;
    }
}

