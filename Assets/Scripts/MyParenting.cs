using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyParenting : MonoBehaviour
{
    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.Find("ARContent");
        transform.parent = parent.transform;
        transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
