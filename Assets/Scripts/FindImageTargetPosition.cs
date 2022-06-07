using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;


public class FindImageTargetPosition : MonoBehaviour
{
    public Vector3 origin_position;
    private GameObject marker;
    private WorldAnchorManager manager;
    private bool position_locked = false;

    void Awake()
    {
        marker = GameObject.Find("TrackerHandler");
        if (XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale))
        {
            // RoomScale mode was set successfully.  App can now assume that y=0 in Unity world coordinate represents the floor.
            print("Successfully set the tracking type!");
        }
        else
        {
            // RoomScale mode was not set successfully.  App cannot make assumptions about where the floor plane is.
            print("Failed to set the tracking type!");
            var scale = XRDevice.GetTrackingSpaceType();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        marker = GameObject.Find("TrackerHandler");
        manager = FindObjectOfType<WorldAnchorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // GameObject camera = GameObject.Find("PositionMarker");
        if (marker != null)
        {
            Vector3 pos = marker.transform.localPosition;
            if ((pos != Vector3.zero) && (!position_locked))
            {
                // origin_position = pos - new Vector3(0.0f, 0.05f, 0.0f);
                origin_position = pos - new Vector3(0.0f, 0.0f, 0.0f);
                transform.localPosition = origin_position;
                transform.localRotation = marker.transform.localRotation;

                manager.AttachAnchor(this.gameObject);
                marker.AddComponent<WorldAnchor>();

                position_locked = true;
            }
            print("origin_position is: " + origin_position);
        }
        else
        {
            origin_position = new Vector3(10.0f, 5.05f, 3.0f);
            transform.localPosition = origin_position;
            transform.localRotation = Quaternion.Euler(-83.369f, 185.838f, -10.511f);;
        }
    }
}
