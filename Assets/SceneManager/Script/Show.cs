using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Linq;
using TMPro;

namespace PLATEAU.Samples
{
    public class Show : MonoBehaviour, GISSampleInputActions.IGISSampleActions
    {
        private UIManage UIManageScript;
        private GameManage GameManageScript;
        [SerializeField, Tooltip("選択中オブジェクトの色")] private Color selectedColor;
        private SampleCityObject selectCityObject;
        private SampleCityObject selectedCityObject;
        private GameObject selectedHintItem;
        private GISSampleInputActions inputActions;
        private bool isInitialiseFinish;
        private string GMLText;
        private GameObject displaySelectGML;

        private void Awake()
        {
            UIManageScript = GameObject.Find("UIManager").GetComponent<UIManage>();
            GameManageScript = GameObject.Find("GameManager").GetComponent<GameManage>();
            inputActions = new GISSampleInputActions();
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
        private void Start()
        {
            inputActions.GISSample.AddCallbacks(this);
            selectedCityObject = null;
        }

        /// <summary>
        /// 建物をクリックした時にGMLの情報を返す
        /// </summary>
        private void SelectObject()
        {
            if(selectedCityObject != null)
            {
                selectedCityObject.SetMaterialColor(Color.white);
            }
            if(selectedHintItem != null)
            {
                Debug.Log(selectedHintItem);
                selectedHintItem.GetComponent<Renderer>().material.color = Color.white;
            }
            var trans = Lookforward();
            if(trans == null)
            {
                selectedCityObject = null;
                selectedHintItem = null;
                return;
            }
            else if(trans.parent.name == "Hint")
            {
                SelectHintItem(trans);
            }
            else
            {
                SelectCityObject(trans);
            }
        }
        private void SelectHintItem(Transform trans)
        {
            GameObject.Find(trans.name).GetComponent<Renderer>().material.color = selectedColor;
            var tmpvalue = GameManageScript.correctGMLdata.GetKeyValues();
            foreach(var t in tmpvalue)
            {
                if(t.Key.Path.Contains(trans.name))
                {
                    Debug.Log("ヒント : " + t.Key.Path + "  :  " + t.Value);
                }
            }
            selectedHintItem = GameObject.Find(trans.name);
        }
        private void SelectCityObject(Transform trans)
        {
            selectCityObject = UIManageScript.gmls[trans.parent.parent.name].CityObjects[trans.name];
            // 選択した建物の色を変更する
            if(selectCityObject != selectedCityObject)
            {
                selectCityObject.SetMaterialColor(selectedColor);
                selectedCityObject = UIManageScript.gmls[trans.parent.parent.name].CityObjects[trans.name];
            }

            GMLText = "";
            //対象の建物のGMLデータを取得
            var data = GetAttribute(trans.parent.parent.name, trans.name);
            var tmpV = data.GetKeyValues();
            foreach(var t in tmpV)
            {
                GMLText += t.Key.Path + "  :  " + t.Value + "/n";
            }
            displaySelectGML = GameObject.Find("SelectBuildingGML");
            TextMeshProUGUI  displayMesh = displaySelectGML.GetComponent<TextMeshProUGUI>();
            displayMesh.text = GMLText;

        }

        /// <summary>
        /// 目の前にある建物を返す
        /// </summary>
        private Transform Lookforward()
        {
            // thirdPerson = GameObject.Find("PlayerArmature");
            var camera = Camera.main;
            var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            float nearestDistance = float.MaxValue;
            Transform nearestTransform = null;
            foreach (var hit in Physics.RaycastAll(ray))
            {
                if (hit.distance <= nearestDistance && hit.transform.name != "PlayerArmature" && !hit.transform.name.Contains("dem"))
                {
                    nearestDistance = hit.distance;
                    nearestTransform = hit.transform;
                }
            }
            return nearestTransform;
        }

        /// <summary>
        /// gmlsから目的の建物を選ぶ
        /// </summary>
        public SampleAttribute GetAttribute(string gmlFileName, string cityObjectID)
        {
            if (UIManageScript.gmls.TryGetValue(gmlFileName, out SampleGml gml))
            {
                if (gml.CityObjects.TryGetValue(cityObjectID, out SampleCityObject city))
                {
                    return city.Attribute;
                }
            }
            return null;
        }
        
        public void OnSelectObject(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if(UIManageScript.isInitialiseFinish)
                {
                    SelectObject();
                }
            }

        }
    }
}
