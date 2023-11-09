using PLATEAU.CityInfo;
using PLATEAU.Util.Async;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Linq;
using System;

namespace PLATEAU.Samples
{
    public class SceneManage : MonoBehaviour, InputScene.ISelectSceneActions
    {
        // Start is called before the first frame update
        [SerializeField, Tooltip("色分け（高さ）の色テーブル")] private Color[] heightColorTable;
        [SerializeField, Tooltip("色分け（使用用途）の色テーブル")] private Color[] usageColorTable;
        [SerializeField] private Camera mainCamera; 
        [SerializeField] private Camera goalCamera; 
        public string SceneName; 
        private InputScene inputActions;


        public bool IsInitialiseFinish = false;
        public readonly Dictionary<string, SampleGml> gmls = new Dictionary<string, SampleGml>();
        private System.Random rnd;
        private PLATEAUInstancedCityModel[] instancedCityModels;

        private string hintFilterName;
        private ColorCodeType colorCodeType;
        private void Awake()
        {
            SceneName = "MainCamera";
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
            
            mainCamera.enabled = true;
            goalCamera.enabled = false;
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

        public void Filter(string filterName)
        {
            hintFilterName = filterName;
            colorCodeType = (ColorCodeType)Enum.Parse(typeof(ColorCodeType), hintFilterName);
            ColorCode(colorCodeType);
        }


        private async Task InitializeAsync()
        {
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
            IsInitialiseFinish = true;
        }




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

            }
        }
    }
}