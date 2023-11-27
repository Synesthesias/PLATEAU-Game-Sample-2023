using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLATEAU.Samples
{
    public class Contact : MonoBehaviour
    {
        private GameManage GameManageScript;
        //★GameViewスクリプトを参照する
        private GameView gameView;
        void Start()
        {
            GameManageScript = GameObject.Find("GameManager").GetComponent<GameManage>();
            //★GameViewコンポーネントを取得
            gameView = transform.root.gameObject.GetComponent<GameView>();
        }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if(hit.gameObject.tag == "Hint")
            {
                //UIManageスクリプトのヒント関数を発動
                GameManageScript.DisplayHint(hit.gameObject.name);
                //アイテムを削除
                Destroy(hit.gameObject);
            }
            if(hit.gameObject.name == "zombie")
            {
                Debug.Log("Game Over");
                //★一番上の親（GameView）にゲームオーバーを通知
                gameView.isGameOver = true ; 

                //#if UNITY_EDITOR
                //    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
                //#else
                //    Application.Quit();//ゲームプレイ終了
                //#endif
            }
            if(hit.gameObject.name == "Helper")
            {
                Debug.Log("Congratuation!!");

                //★一番上の親（GameView）にゲームクリアを通知
                gameView.isGameClear = true;
                //★スコアを加算（例）TODO:直接値を変えるのは望ましくないため、ViewManager側でスコア加算用の関数を作成する
                ViewManager.instance.score += 100;

                //#if UNITY_EDITOR
                //    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
                //#else
                //    Application.Quit();//ゲームプレイ終了
                //#endif

            }

        }
    }
}