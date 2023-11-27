// 正解データがある大元
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

namespace PLATEAU.Samples
{
    public class GameManage : MonoBehaviour, InputGameManage.IInputGameActions
    {
        [SerializeField, Tooltip("高さアイテム")] private GameObject measuredheightItem;
        [SerializeField, Tooltip("用途アイテム")] private GameObject UsageItem;
        [SerializeField, Tooltip("ゾンビ")] private GameObject Zombie;
        private InputGameManage inputActions;
        private UIManage UIManageScript;
        private TimeManage TimeManageScript;
        public SampleAttribute correctGMLdata;
        private GameObject goalBuilding;
        private Bounds goalBounds;
        private Vector3 goalPos;
        private System.Random rnd;
        private GameObject[] HintLst;
        private int zombieNum;
        private bool isSetGMLdata;

        public float sonarCount;
        public float distance;
        KeyValuePair<string, PLATEAU.Samples.SampleCityObject> rndBuilding;

        private void Awake()
        {
            inputActions = new InputGameManage();
        }

        // InputSystemに関する関数
        // -------------------------------------------------------------------------------------------------------------
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
            rnd = new System.Random();
            inputActions.InputGame.AddCallbacks(this);
            //SceneManagerからShow.csにアクセスする
            UIManageScript = GameObject.Find("UIManager").GetComponent<UIManage>();
            TimeManageScript = GameObject.Find("TimeManager").GetComponent<TimeManage>();
            //Hintのリストを作る
            HintLst = GameObject.FindGameObjectsWithTag("HintText");
            sonarCount = 5;
            zombieNum = 50;

            for(int i=0; i < zombieNum;i++)
            {
                GenerateZombie();
            }
            //20秒後にCalDistanceを実行
            // Invoke(nameof(CalDistance), 20f);
        }


        /// <summary>
        /// 必要なGMLデータがそろっているか判定する
        /// </summary>
        private bool CheckGMLdata(SampleAttribute buildingData)
        {
            bool isSetData = false;
            
            foreach(GameObject hint in HintLst)
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

        /// <summary>
        /// ランダムな位置にゴールを設置する
        /// </summary>
        public void SelectGoal()
        {
            while(!isSetGMLdata)
            {
                //ランダムに建物を指定
                rndBuilding = UIManageScript.gmls["53394525_bldg_6697_1_op.gml"].CityObjects.ElementAt(rnd.Next(0, UIManageScript.gmls["53394525_bldg_6697_1_op.gml"].CityObjects.Count));
                //ゴールのGMLデータ
                correctGMLdata = rndBuilding.Value.Attribute;
                isSetGMLdata = CheckGMLdata(correctGMLdata);
            }

            //正解の建物の情報を取得
            goalBuilding = GameObject.Find(rndBuilding.Key);
            goalBounds = goalBuilding.GetComponent<MeshCollider>().sharedMesh.bounds;
            //選ばれた建物の位置情報を取得
            goalPos = new Vector3(goalBounds.center.x,goalBounds.center.y+goalBounds.size.y,goalBounds.center.z);
            
            //ゴールカメラの位置を変更
            GameObject.Find("GoalSceneCamera").transform.position = goalPos;
            //Helperの位置を変更
            //★デバッグ終了後元に戻す
            //GameObject.Find("Helper").transform.position = goalPos;
        }

        /// <summary>
        /// ゴールとプレイヤーの距離を計測する
        /// </summary>
        public void CalDistance()
        {
            if(sonarCount > 0)
            {
                Debug.Log("yes");
                sonarCount -= 1;
                float userPosX = GameObject.Find("PlayerArmature").transform.position.x;
                float userPosZ = GameObject.Find("PlayerArmature").transform.position.z;
                distance = Vector2.Distance(new Vector2(userPosX,userPosZ),new Vector2(goalPos.x,goalPos.z));
            }
        }

        /// <summary>
        /// アイテムを拾った時の処理
        /// </summary>
        public void DisplayHint(string itemName)
        {
            UIManageScript.DisplayAnswerGML(itemName);
            TimeManageScript.ColorBuilding(itemName);
        }

        // 生成する処理
        // -----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// アイテムを生成する
        /// </summary>
        public void GenerateHintItem()
        {
            //★GameViewの子として生成
            GameObject hintItem = Instantiate(measuredheightItem, transform.root.gameObject.transform) as GameObject;
            hintItem.name = "measuredheight";
            float itemPosX = Random.Range(-400f,450f);
            float itemPosZ= Random.Range(-200f,200f);
            hintItem.transform.position = new Vector3(itemPosX,100,itemPosZ);

            hintItem = Instantiate(UsageItem, transform.root.gameObject.transform) as GameObject;
            hintItem.name = "Usage";
            itemPosX = Random.Range(-400f,450f);
            itemPosZ= Random.Range(-200f,200f);
            hintItem.transform.position = new Vector3(itemPosX,100,itemPosZ);
        }

        public void GenerateZombie()
        {
            GameObject zombie = Instantiate(Zombie, transform.root.gameObject.transform) as GameObject;
            zombie.name = "zombie";
            float itemPosX = Random.Range(-400f,400f);
            float itemPosZ= Random.Range(-200f,200f);
            zombie.transform.position = new Vector3(itemPosX,100,itemPosZ);
        }

        // InputSystemの入力に対する処理(OnSonar : F)
        // -------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sonarを使う時の処理
        /// </summary> 
        public void OnSonar(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CalDistance();
                UIManageScript.DisplayDistance();
            }
        }
    }
}
