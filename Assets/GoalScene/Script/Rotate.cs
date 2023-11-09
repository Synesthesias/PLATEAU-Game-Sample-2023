using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLATEAU.Samples
{
    public class Rotate : MonoBehaviour
    {
        private Vector3 angle;
        private SceneManage sceneManage;

        void Start()
        {
            angle = this.gameObject.transform.localEulerAngles;
            sceneManage = GameObject.Find("SceneManager").GetComponent<SceneManage>();
        }

        void Update()
        {
            if(sceneManage.SceneName == "GoalCamera")
            {
                RotateCamera();
            }
            
        }
        private void RotateCamera()
        {
            angle.y += Input.GetAxis("Mouse X");

            angle.x -= Input.GetAxis("Mouse Y");

            this.gameObject.transform.localEulerAngles = angle;
        }
    }
}