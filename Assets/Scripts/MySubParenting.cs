using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySubParenting : MonoBehaviour
{
    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.Find("SubParent");
        transform.parent = parent.transform;
    }
}