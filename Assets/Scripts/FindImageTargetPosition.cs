using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using Vuforia;


public class FindImageTargetPosition : MonoBehaviour
{
    public Vector3 origin_position;
    public bool already_got = false;
    private WorldAnchorManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<WorldAnchorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject marker = GameObject.Find("VuforiaPositionMarker");
        // GameObject camera = GameObject.Find("PositionMarker");
        Vector3 pos = marker.transform.localPosition;
        GameObject child = GameObject.Find("Sphere1");
        GameObject camera = GameObject.Find("Main Camera");
        if (pos != Vector3.zero)
        {
            if (!already_got)
            {
                origin_position = pos;
                already_got = true;
                transform.localPosition = origin_position;
                // child.transform.localPosition = Vector3.zero;
                // child.transform.position = origin_position;
                // Attach world anchor.
                manager.AttachAnchor(marker);
                marker.AddComponent<WorldAnchor>();
                TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();

                // Disable vuforia behavior
                camera.GetComponent<VuforiaBehaviour>().enabled = false;
            }
            manager.AttachAnchor(marker);
            marker.AddComponent<WorldAnchor>();
            TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();

            // Disable vuforia behavior
            camera.GetComponent<VuforiaBehaviour>().enabled = false;
        }
        print("origin_position is: " + origin_position);
    }
}
