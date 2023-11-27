using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : ViewBase
{
    public bool isGameClear = false;  //�Q�[���N���A�t���O
    public bool isGameOver = false;   //�Q�[���I�[�o�[�t���O

    [SerializeField] private Canvas gameEndCanvas;
    [SerializeField] private ExtendButton retryButton;  //���g���C�{�^��
    [SerializeField] private ExtendButton toTitleButton;  //�^�C�g���ɖ߂�{�^��
    [SerializeField] private Text gameEndText;  //�Q�[���I���e�L�X�g
    [SerializeField] private Text scoreText;  //�X�R�A�e�L�X�g



    void Start()
    {
        gameEndCanvas.enabled=false;
    }
    public override IEnumerator Wait()
    {
        while (true)
        {
            //�Q�[���I��
            if(isGameOver||isGameClear)
            {
                //��UI���\���ɂ���
                //���]���r��A�C�e��������
                //���v���C���[�𑀍�ł��Ȃ�����


                //�Q�[���I��UI��\��
                gameEndCanvas.enabled=true;

                //�Q�[���I�[�o�[�e�L�X�g
                gameEndText.text = "Game Over";

                if (isGameClear)
                {
                    //�N���A�e�L�X�g
                    gameEndText.text = "Game Clear!";

                    //�X�R�A�\�����X�R�A�擾�p�̊֐����쐬����
                    scoreText.text = "SCORE�@"+ViewManager.instance.score;
                }

                //�{�^�����͑҂���Ԃɂ���
                while (true)
                {
                    if (retryButton.IsClicked)  //�Ē���{�^��
                    {
                        //�Q�[��������������

                    }
                    else if (toTitleButton.IsClicked)  //�^�C�g���{�^��
                    {
                        //�Q�[���I��
                        yield break;
                    }

                    yield return null;
                }

            }
            yield return null;
        }
    }
}