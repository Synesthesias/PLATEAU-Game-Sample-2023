using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using PLATEAU.Samples;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public enum NPCState
    {
        //Stroll,//巡回する
        Wait,//待機する
        Follow,//ついていく
        Goal,//ゴールへ向かう
    };

    [SerializeField]private float NPCSpeed=15f;

    private Animator animator;
    private GameObject player;
    private NavMeshAgent agent;

    //NPCの状態
    private NPCState state;
    //NPCとプレイヤーの現在の距離
    private float currentDistance;
    //プレイヤーがこの距離まで近づいたらついてくるようになる
    private float followDistance=3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        currentDistance = Vector3.Distance(this.transform.position, player.transform.position);

        if (state == NPCState.Follow)//追いかける
        {
            this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
            SetNPCDestination(player.transform.position);
            
            //プレイヤーに十分近づいたら待機状態にする
            if (currentDistance < followDistance)
            {
                SetState(NPCState.Wait);
            }
        }
        else if (state == NPCState.Wait) //待機する
        {
            if (agent.isStopped == true)
            {
                //プレイヤーから離れたらついてくる
                if (currentDistance > followDistance)
                {
                    SetState(NPCState.Follow);
                }
            }
        }
        else if (state == NPCState.Goal) //ゴールに向かう
        {

        }
    }

    //NPCの状態変更メソッド
    public void SetState(NPCState tempState, Transform targetObj = null)
    {
        state = tempState;
        if (tempState == NPCState.Follow)
        {
            animator.SetFloat("MoveSpeed", NPCSpeed);
            agent.isStopped = false;
            animator.SetBool("IsWalking", true);
            //Debug.Log("追いかける状態になった");
        }
        else if (tempState == NPCState.Wait)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            //animator.SetFloat("MoveSpeed", 0f);
            animator.SetBool("IsWalking", false);
            //Debug.Log("待機状態になった");
        }
        else if (tempState == NPCState.Goal)
        {
            animator.SetFloat("MoveSpeed", NPCSpeed);
            agent.isStopped = false;
            animator.SetBool("IsWalking", true);

        }
    }
    //NPCの状態取得メソッド
    public NPCState GetState()
    {
        return state;
    }
    //プレイヤーが範囲に入ったらついていく
    public void OnCharacterEnter(Collider collider)
    {
        //ゴールの建物
        if(collider.CompareTag("Goal"))
        {
            //自身を助けた人数としてカウントさせる
            GameObject.Find("GameManager").GetComponent<GameManage>().AddRescueNum();
            //自身を消す
            Destroy(this);
        }
        //プレイヤーを発見
        else if (collider.CompareTag("Player"))
        {
            //　NPCの状態を取得
            NPCController.NPCState state = GetState();
            //　NPCがついていく状態でなければついていく設定に変更
            if (state != NPCController.NPCState.Follow)
            {
                //Debug.Log("プレイヤー発見");
                SetState(NPCController.NPCState.Follow, collider.transform);
            }

        }
    }
    //NPCの目的地を設定
    public void SetNPCDestination(Vector3 distination)
    {
        agent.SetDestination(distination);

    }
}
