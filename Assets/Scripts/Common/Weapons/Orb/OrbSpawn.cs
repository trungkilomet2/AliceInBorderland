using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawn : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        SetChildOfPlayer();
    }

    void SetChildOfPlayer()
    {
        if (player != null)
        {
            Vector3 offset = new Vector3(0, -0.07f, 0);
            transform.position = player.transform.position + offset;
            transform.SetParent(player.transform);
        }
    }
}
