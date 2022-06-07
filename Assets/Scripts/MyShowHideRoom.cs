using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyShowHideRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowRoom()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
    public void HideRoom()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
