using PLATEAU.CityInfo;
using PLATEAU.Util.Async;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

namespace PLATEAU.Samples
{
    public class UIManage : MonoBehaviour, InputScene.ISelectSceneActions
    {
        // Start is called before the first frame update
        [SerializeField, Tooltip("初期化中")] private UIDocument initializingUi;
        [SerializeField, Tooltip("色分け（高さ）の色テーブル")] private Color[] heightColorTable;
        [SerializeField, Tooltip("色分け（使用用途）の色テーブル")] private Color[] usageColorTable;
        [SerializeField] private Camera mainCamera; 
        [SerializeField] private Camera goalCamera;


        private PLATEAUInstancedCityModel[] instancedCityModels;
        private ColorCodeType colorCodeType;
        private InputScene inputActions;
        private Canvas GMLCanvas;
        private GameManage GameManageScript;
        private GameObject[] FilterContents;
        private GameObject[] HintTexts;
        private int filterIndex;
        private bool isSetColorCodeType;

        public readonly Dictionary<string, SampleGml> gmls = new Dictionary<string, SampleGml>();
        public string SceneName; 
        public bool isInitialiseFinish = false;


        private void Awake()
        {
            inputActions = new InputScene();
            InitializeAsync().ContinueWithErrorCatch();  
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
            inputActions.SelectScene.AddCallbacks(this);
            StartCoroutine(WatiForInitialise());

            filterIndex = 0;
            SceneName = "MainCamera";
            mainCamera.enabled = true;
            goalCamera.enabled = false;
            GameManageScript = GameObject.Find("GameManager").GetComponent<GameManage>();
            HintTexts = GameObject.FindGameObjectsWithTag("HintText");

            FilterContents = GameObject.FindGameObjectsWithTag("FilterContent");
            foreach(GameObject FilterContent in FilterContents)
            {
                if(FilterContent.name != "None")
                {
                    FilterContent.SetActive(false);
                }
                else
                {
                    TextMeshProUGUI  filterMesh = FilterContent.GetComponent<TextMeshProUGUI>();
                    filterMesh.color = Color.red;
                    filterIndex = 0;
                }
                
            }

        }


        public void Filter(string filterName)
        {
            //正解値　表示
            foreach(GameObject text in HintTexts)
            {
                if(text.name.Contains(filterName))
                {
                    foreach(var t in GameManageScript.correctGMLdata.GetKeyValues())
                    {
                        if(t.Key.Path.Contains(filterName))
                        {
                            TextMeshProUGUI hintText = text.GetComponent<TextMeshProUGUI>();
                            hintText.text = filterName + " : " + t.Value;
                        }
                    }    
                }
            }

            //フィルター　表示
            foreach(GameObject FilterContent in FilterContents)
            {
                if(FilterContent.name.Contains(filterName))
                {
                    FilterContent.SetActive(true);
                }
            }
        }

        private void ColorCode(ColorCodeType type)
        {
            foreach (var keyValue in gmls)
            {
                Color[] colorTable = null;
                switch (type)
                {
                    case ColorCodeType.measuredheight:
                        colorTable = heightColorTable;
                        break;
                    case ColorCodeType.Usage:
                        colorTable = usageColorTable;
                        break;
                    default:
                        break;
                }
                keyValue.Value.ColorCode(type, colorTable);
            }
        }


        /// <summary>
        /// Inport PLATEAU data
        /// </summary> 
        private async Task InitializeAsync()
        {
            initializingUi.gameObject.SetActive(true);
            instancedCityModels = FindObjectsOfType<PLATEAUInstancedCityModel>();
            

            if (instancedCityModels == null || instancedCityModels.Length == 0)
            {
                return;
            }
            foreach(var instancedCityModel in instancedCityModels)
            {
                var rootDirName = instancedCityModel.name;

                for (int i = 0; i < instancedCityModel.transform.childCount; ++i)
                {
                    var go = instancedCityModel.transform.GetChild(i).gameObject;
                    // サンプルではdemを除外します。
                    if (go.name.Contains("dem")) continue;
                    var cityModel = await PLATEAUCityGmlProxy.LoadAsync(go, rootDirName);
                    if (cityModel == null) continue;
                    var gml = new SampleGml(cityModel, go);
                    gmls.Add(go.name, gml);
                }
            }
            isInitialiseFinish = true;
        }



        /// <summary>
        /// Change to Camera Scene
        /// </summary> 
        private void ChangeCameraScene()
        {
            if(mainCamera.enabled)
            {
                SceneName = "GoalCamera";
                mainCamera.enabled = false;
                goalCamera.enabled = true;
            }
            else
            {
                SceneName = "MainCamera";
                mainCamera.enabled = true;
                goalCamera.enabled = false;
            }
        }

        private void ChangeFilter()
        {
            if(FilterContents.Length == filterIndex + 1)
            {
                filterIndex = 0;
            }
            else
            {
                filterIndex += 1;
            }
            isSetColorCodeType = false;
            while(!isSetColorCodeType)
            {
                if(FilterContents[filterIndex].activeSelf)
                {
                    colorCodeType = (ColorCodeType)Enum.Parse(typeof(ColorCodeType), "None");
                    ColorCode(colorCodeType);
                    colorCodeType = (ColorCodeType)Enum.Parse(typeof(ColorCodeType), FilterContents[filterIndex].name);
                    ColorCode(colorCodeType);

                    foreach(GameObject FilterContent in FilterContents)
                    {
                        TextMeshProUGUI  filterM = FilterContent.GetComponent<TextMeshProUGUI>();
                        filterM.color = Color.black;
                    }
                    TextMeshProUGUI  filterMesh = FilterContents[filterIndex].GetComponent<TextMeshProUGUI>();
                    filterMesh.color = Color.red;
                    break;
                    // isSetColorCodeType = true;
                }
                if(FilterContents.Length == filterIndex + 1)
                {
                    filterIndex = 0;
                }
                else
                {
                    filterIndex += 1;
                }
            }
        }


        IEnumerator WatiForInitialise()
        {
            // yield return ->　ある関数が終わるまで待つ
            yield return new WaitUntil(() => IsInitialiseFinished());
        }
        private bool IsInitialiseFinished()
        {
            if(isInitialiseFinish)
            {
                initializingUi.gameObject.SetActive(false);
            }
            return isInitialiseFinish;
        }

        /// <summary>
        /// Function of Input Scene
        /// </summary> 
        public void OnChangeCameraScene(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                ChangeCameraScene();
            }

        }
        public void OnChangeFilterScene(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                ChangeFilter();
            }
        }
    }
}