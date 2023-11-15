// 正解データがある大元
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

namespace PLATEAU.Samples
{
    public class GameManage : MonoBehaviour,InputGame.IGameInputActions
    {
        // [SerializeField, Tooltip("ヒント:height")] private GameObject Hint_height;
        // スクリプト名 変数(自由)
        private UIManage UIManageScript;
        public SampleAttribute correctGMLdata;
        private Show ShowScript;
        private bool isInitialiseFinish = false;
        private GameObject goalBuilding;
        private Bounds goalBounds;
        private Vector3 goalPos;
        private System.Random rnd;
        private InputGame inputActions;
        private GameObject[] hintLst;
        private bool isSetGMLdata;
        KeyValuePair<string, PLATEAU.Samples.SampleCityObject> rndBuilding;
        void Awake()
        {
            inputActions = new InputGame();
        }
        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void OnDestroy()
        {
            inputActions.Dispose();
        }

        void Start()
        {
            //SceneManagerからShow.csにアクセスする
            UIManageScript = GameObject.Find("UIManager").GetComponent<UIManage>();
            rnd = new System.Random();
            //Hintのリストを作る
            hintLst = GameObject.FindGameObjectsWithTag("Hint");
            //コルーチン開始
            StartCoroutine(WatiForInitialise());
            //20秒後にCalDistanceを実行
            // Invoke(nameof(CalDistance), 20f);
            inputActions.GameInput.AddCallbacks(this);

            
        }

        /// <summary>
        /// ランダムな位置にゴールを設置する
        /// </summary>
        private void SelectGoal()
        {
            while(!isSetGMLdata)
            {
                //ランダムに建物を指定
                rndBuilding = UIManageScript.gmls["53394525_bldg_6697_1_op.gml"].CityObjects.ElementAt(rnd.Next(0, UIManageScript.gmls["53394525_bldg_6697_1_op.gml"].CityObjects.Count));
                //ゴールのGMLデータ
                correctGMLdata = rndBuilding.Value.Attribute;
                isSetGMLdata = CheckGMLdata(correctGMLdata);
            }

            //選ばれた建物の情報を取得
            goalBuilding = GameObject.Find(rndBuilding.Key);
            goalBounds = goalBuilding.GetComponent<MeshCollider>().sharedMesh.bounds;
            //選ばれた建物の位置情報を取得
            goalPos = new Vector3(goalBounds.center.x,goalBounds.center.y+goalBounds.size.y,goalBounds.center.z);
            
            //ゴールカメラの位置を変更
            GameObject.Find("GoalSceneCamera").transform.position = goalPos;
        }

        private bool CheckGMLdata(SampleAttribute buildingData)
        {
            bool isSetData = false;
            foreach(GameObject hint in hintLst)
            {
                isSetData = false;
                foreach(var t in buildingData.GetKeyValues())
                {
                    if(t.Key.Path.Contains(hint.name))
                    {
                        isSetData = true;
                        break;
                    }
                }
                if(!isSetData)
                {
                    break;
                }
            }
            return isSetData;
        }


        // private void SetHintItem()
        // {
        //     Hint_height.gameObject.SetActive(true);
        // }


        /// <summary>
        /// ゴールとプレイヤーの距離を計測する
        /// </summary>
        private void CalDistance()
        {
            float userPosX = GameObject.Find("PlayerArmature").transform.position.x;
            float userPosZ = GameObject.Find("PlayerArmature").transform.position.z;

            float distance = Vector2.Distance(new Vector2(userPosX,userPosZ),new Vector2(goalPos.x,goalPos.z));
            Debug.Log("ゴールとの距離 : " + distance);
        }

        public void OnFinish(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                CalDistance();
            }
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
            if(UIManageScript.isInitialiseFinish)
            {
                SelectGoal();
                // SetHintItem();
                isInitialiseFinish = true;
            }
            return isInitialiseFinish;
        }
    }
}
