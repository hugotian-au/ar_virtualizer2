using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyParenting : MonoBehaviour
{
    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.Find("MixedRealityPlayspace");
        transform.parent = parent.transform;
    }
}
