using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLATEAU.Samples
{
    public class Contact : MonoBehaviour
    {
        private GameManage GameManageScript;
        void Start()
        {
            GameManageScript = GameObject.Find("GameManager").GetComponent<GameManage>();
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
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
                #else
                    Application.Quit();//ゲームプレイ終了
                #endif
            }
            if(hit.gameObject.name == "Helper")
            {
                Debug.Log("Congratuation!!");
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
                #else
                    Application.Quit();//ゲームプレイ終了
                #endif

            }

        }
    }
}