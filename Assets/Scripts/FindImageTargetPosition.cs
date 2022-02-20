using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using Photon.Pun;
using Photon.Realtime;


public class FindImageTargetPosition : MonoBehaviourPunCallbacks
{
    public Vector3 origin_position;
    private GameObject marker;
    // Start is called before the first frame update
    void Start()
    {
        marker = GameObject.Find("TrackerHandler");
    }

    // Update is called once per frame
    void Update()
    {
        // GameObject camera = GameObject.Find("PositionMarker");
        if (marker != null)
        {
            Vector3 pos = marker.transform.localPosition;
            if (pos != Vector3.zero)
            {
                origin_position = pos;
                transform.localPosition = origin_position;
            }
            print("origin_position is: " + origin_position);
        }
    }

    public void OnDestroy()
    {
        Debug.Log("Leave the room");
        // PhotonNetwork.LeaveRoom();
    }

    #region Photon Callbacks
    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        print("Leave the room!");
    }
    #endregion
}
