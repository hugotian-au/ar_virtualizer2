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

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            // LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            // LoadArena();
        }
    }

    public void OnDestroy()
    {
        Debug.Log("Leave the room OnDestroy");
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
