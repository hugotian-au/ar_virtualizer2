using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using Vuforia;
using Photon.Realtime;
using Photon.Pun;

public class TrackerHandler : MonoBehaviour
{
    public Vector3 origin_position;
    private WorldAnchorManager manager;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<WorldAnchorManager>();
    }
    // On Image Target Tracker Found
    public void OnTrackerFound()
    {
        GameObject marker = GameObject.Find("VuforiaPositionMarker");
        Vector3 pos = marker.transform.localPosition;
        // GameObject child = GameObject.Find("Sphere1");
        GameObject camera = GameObject.Find("Main Camera");
        if (pos != Vector3.zero)
        {
            origin_position = pos;
            transform.localPosition = origin_position;
            // child.transform.localPosition = Vector3.zero;
            // child.transform.position = origin_position;
            // Attach world anchor.
            manager.AttachAnchor(marker);
            marker.AddComponent<WorldAnchor>();
            TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();

            // Disable vuforia behavior
            camera.GetComponent<VuforiaBehaviour>().enabled = false;
            marker.SetActive(false);

            PhotonNetwork.LoadLevel("Room");
        }
    }
}
