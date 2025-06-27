using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZone : MonoBehaviour
{
   
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
}
