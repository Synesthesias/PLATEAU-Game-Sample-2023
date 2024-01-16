using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Start()
    {
        player.transform.position = transform.position;
        this.enabled = false;
    }

}
