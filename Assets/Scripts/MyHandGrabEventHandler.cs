using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyHandGrabEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnHandTouchEvent()
    {
        var pv = GetComponent<PhotonView>();
        pv.RequestOwnership();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
