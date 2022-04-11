using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{

    public GameObject goldPrefab;
    public GameObject hpPrefab;
    public GameObject manaPrefab;

    void Start()
    {
       
        //Debug instantiations. TO be spawned when enemy defeated.
        GameObject stuff = Instantiate(goldPrefab, new Vector3(2f, 0f, 0f), Quaternion.identity) as GameObject;
        GameObject stuff2 = Instantiate(goldPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        GameObject stuff3 = Instantiate(goldPrefab, new Vector3(0f, 0f, -2f), Quaternion.identity) as GameObject;

    }

    
}
