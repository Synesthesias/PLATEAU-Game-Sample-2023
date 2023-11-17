using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Zombie : MonoBehaviour
{
    private Animator animator;
    public GameObject player;
    public float distance;

    public bool search;
    [SerializeField] private SphereCollider searchArea;
    [SerializeField] float searchAngle = 230f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);
        if(distance < 10)
        {
            search = true;
        }
        else
        {
            search = false;
        }
        if(distance < 1f)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.ResetTrigger("Attack");
        }

        if(search)
        {
            animator.SetBool("Run",true);
            transform.position += transform.forward * 0.05f;
            this.transform.LookAt(player.transform);
        }
        else
        {
            animator.SetBool("Run",false);
            animator.SetBool("Walk",true);
            // this.transform.position += transform.forward * 0.03f;
        }
        // Debug.Log(search);
    }
    // プレイヤーが範囲内にいるか
    // private void OnTriggerExit(Collider other)
    // {
    //     if(other.tag == "Player")
    //     {
    //         search = false;
    //     }
    // }
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up,Quaternion.Euler(0f,-searchAngle,0f)*transform.forward,searchAngle*2f,searchArea.radius);
    }
}
