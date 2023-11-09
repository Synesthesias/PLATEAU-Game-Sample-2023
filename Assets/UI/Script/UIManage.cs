using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


namespace PLATEAU.Samples
{
    public class UIManage : MonoBehaviour
    {
        [SerializeField, Tooltip("初期化中")] private UIDocument initializingUi;
        private SceneManage SceneManageScript;
        private bool isInitialiseFinish = false;
        void Awake()
        {
            //ShowGMLからShow.csにアクセスする
            initializingUi.gameObject.SetActive(true);
        }
        void Start()
        {
            SceneManageScript = GameObject.Find("SceneManager").GetComponent<SceneManage>();
            StartCoroutine(WatiForInitialise());
        }

        IEnumerator WatiForInitialise()
        {
            // yield return ->　ある関数が終わるまで待つ
            yield return new WaitUntil(() => IsInitialiseFinished());
        }

        /// <summary>
        /// 都市データがロードされるまで待つ 
        /// </summary>
        private bool IsInitialiseFinished()
        {
            if(SceneManageScript.IsInitialiseFinish)
            {
                initializingUi.gameObject.SetActive(false);
                isInitialiseFinish = true;
            }
            return isInitialiseFinish;
        }
    }
}