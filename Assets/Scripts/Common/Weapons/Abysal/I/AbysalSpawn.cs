using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbysalSpawn : MonoBehaviour
{

    private GameObject player;
    private const string PLAYER_TAG = "Player";

    private void Awake()
    {
        player = GameObject.FindWithTag(PLAYER_TAG);
    }

    void Start()
    {
        FlyOnTheHeadPlayer();
    }

    void FlyOnTheHeadPlayer()
    {
        if (player != null)
        {
            Vector3 offset = new Vector3(0, 1f, 0);
            transform.position = player.transform.position + offset;
            transform.SetParent(player.transform);
        }
    }


}
