using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : ViewBase
{
    public bool isGameClear = false;  //ゲームクリアフラグ
    public bool isGameOver = false;   //ゲームオーバーフラグ

    [SerializeField] private Canvas gameEndCanvas;
    [SerializeField] private ExtendButton retryButton;  //リトライボタン
    [SerializeField] private ExtendButton toTitleButton;  //タイトルに戻るボタン
    [SerializeField] private Text gameEndText;  //ゲーム終了テキスト
    [SerializeField] private Text scoreText;  //スコアテキスト

    private GameObject cursor;

    private CursorManager cursorManage;

    void Awake()
    {
        cursor = GameObject.Find("Cursor");
        cursorManage = cursor.GetComponent<CursorManager>();
    }

    void Start()
    {
        
        cursorManage.OnInvisible();
        Cursor.lockState = CursorLockMode.Confined;
        gameEndCanvas.enabled=false;
    }
    public override IEnumerator Wait()
    {
        while (true)
        {
            //ゲーム終了
            if(isGameOver||isGameClear)
            {
                //★UIを非表示にする
                //★ゾンビやアイテムを消す
                //★プレイヤーを操作できなくする


                //ゲーム終了UIを表示
                gameEndCanvas.enabled=true;
                cursorManage.OnVisible();

                //ゲームオーバーテキスト
                gameEndText.text = "Game Over";

                if (isGameClear)
                {
                    //クリアテキスト
                    gameEndText.text = "Game Clear!";

                    //スコア表示※スコア取得用の関数を作成する
                    scoreText.text = "SCORE　"+ViewManager.instance.score;
                }

                //ボタン入力待ち状態にする
                while (true)
                {
                    if (retryButton.IsClicked)  //再挑戦ボタン
                    {
                        //ゲームを初期化する

                    }
                    else if (toTitleButton.IsClicked)  //タイトルボタン
                    {
                        //ゲーム終了
                        yield break;
                    }

                    yield return null;
                }

            }
            yield return null;
        }
    }
}