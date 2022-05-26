using UnityEngine;
using System.Collections.Generic;
using DilmerGames.Enums;
using DilmerGames.Core.Utilities;
using Photon.Pun;

namespace DilmerGames
{
    public class VRDraw : MonoBehaviourPunCallbacks, IPunObservable
    {
        private int current_index = 1;
        private int previous_index = 0;
        private Vector3 trackPosition;
        private Vector3 previous_trackPosition;
        private int numCapVectices;
        private int previous_numCapVectices;
        private Vector3 linePosition;
        private Vector3 previousLinePosition;
        private Vector3 cameraPosition;
        private Vector3 previousCameraPosition;

        private bool newLine = false;
        private bool updateLine = false;

        [SerializeField]
        private ControlHand controlHand = ControlHand.NoSet;

        [SerializeField]
        private GameObject objectToTrackMovement;

        private Vector3 prevPointDistance = Vector3.zero;

        [SerializeField, Range(0, 1.0f)]
        private float minDistanceBeforeNewPoint = 0.2f;
        private float previousMinDistanceBeforeNewPoint = 0.2f;

        [SerializeField, Range(0, 1.0f)]
        private float minDrawingPressure = 0.8f;

        [SerializeField, Range(0, 1.0f)]
        private float lineDefaultWidth = 0.010f;
        private float previousLineWidth = 0.010f;

        private int positionCount = 0; // 2 by default
        private int previousPositionCount = 0;

        private List<LineRenderer> lines = new List<LineRenderer>();

        private LineRenderer currentLineRender;

        [SerializeField]
        private Color defaultColor = Color.white;

        [SerializeField]
        private GameObject editorObjectToTrackMovement;

        [SerializeField]
        private bool allowEditorControls = true;
       
        void Start()
        {
            if (gameObject.name == "VRDrawLeft(Clone)")
            {
                var trackObject = GameObject.Find("ARContent");
                objectToTrackMovement = trackObject;
            }
            if (gameObject.name == "VRDrawRight(Clone)")
            {
                var trackObject = GameObject.Find("ARContent");
                objectToTrackMovement = trackObject;
            }
            // AddNewLineRenderer();
        }

        void AddNewLineRenderer()
        {
            positionCount = 0;
            GameObject go = new GameObject($"LineRenderer_{controlHand.ToString()}_{lines.Count}");
            go.transform.parent = objectToTrackMovement.transform.parent;
            go.transform.position = objectToTrackMovement.transform.position;
            LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
            goLineRenderer.startWidth = lineDefaultWidth;
            goLineRenderer.endWidth = lineDefaultWidth;
            goLineRenderer.useWorldSpace = false;
            goLineRenderer.material = MaterialUtils.CreateMaterial(defaultColor, $"Material_{controlHand.ToString()}_{lines.Count}");
            goLineRenderer.positionCount = 1;
            goLineRenderer.numCapVertices = 90;
            goLineRenderer.SetPosition(0, objectToTrackMovement.transform.position);

            currentLineRender = goLineRenderer;
            lines.Add(goLineRenderer);
        }

        void Update()
        {
            // primary left controller
            if(controlHand == ControlHand.Left && updateLine)
            {
                //VRStats.Instance.firstText.text = $"Axis1D.PrimaryIndexTrigger: {OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)}";
                UpdateLine();
                updateLine = false;

                previous_trackPosition = trackPosition;
                previousLineWidth = lineDefaultWidth;
                previousPositionCount = positionCount;
                previous_numCapVectices = numCapVectices;
                previousLinePosition = linePosition;
                previousMinDistanceBeforeNewPoint = minDistanceBeforeNewPoint;
                previousCameraPosition = cameraPosition;
            }
            else if(controlHand == ControlHand.Left && newLine)
            {
                //VRStats.Instance.secondText.text = $"Button.PrimaryIndexTrigger: {Time.deltaTime}";
                AddNewLineRenderer();
                newLine = false;
                previous_index = current_index;
            }

            // secondary right controller
            if(controlHand == ControlHand.Right && updateLine)
            {
                //VRStats.Instance.firstText.text = $"Axis1D.SecondaryIndexTrigger: {OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger)}";
                UpdateLine();
                updateLine = false;
                previous_trackPosition = trackPosition;
                previousLineWidth = lineDefaultWidth;
                previousPositionCount = positionCount;
                previous_numCapVectices = numCapVectices;
                previousLinePosition = linePosition;
                previousMinDistanceBeforeNewPoint = minDistanceBeforeNewPoint;
                previousCameraPosition = cameraPosition;
            }
            else if(controlHand == ControlHand.Right && newLine)
            {
                //VRStats.Instance.secondText.text = $"Button.SecondaryIndexTrigger: {Time.deltaTime}";
                AddNewLineRenderer();
                newLine = false;
                previous_index = current_index;
            }

        }

        void UpdateLine()
        {
            if(prevPointDistance == null)
            {
                prevPointDistance = trackPosition;
            }

            if(prevPointDistance != null && Mathf.Abs(Vector3.Distance(prevPointDistance, trackPosition)) >= minDistanceBeforeNewPoint)
            {
                Vector3 dir = (trackPosition - cameraPosition).normalized;
                prevPointDistance = trackPosition;
                AddPoint(prevPointDistance, dir);
            }
        }

        void AddPoint(Vector3 position, Vector3 direction)
        {
            currentLineRender.SetPosition(positionCount, position);
            positionCount++;
            currentLineRender.positionCount = positionCount + 1;
            currentLineRender.SetPosition(positionCount, position);
            
            // send position
            // TCPControllerClient.Instance.UpdateLine(position);
        }

        public void UpdateLineWidth(float newValue)
        {
            currentLineRender.startWidth = newValue;
            currentLineRender.endWidth = newValue;
            lineDefaultWidth = newValue;
        }

        public void UpdateLineColor(Color color)
        {
            // in case we haven't drawn anything
            if(currentLineRender.positionCount == 1)
            {
                currentLineRender.material.color = color;
                currentLineRender.material.EnableKeyword("_EMISSION");
                currentLineRender.material.SetColor("_EmissionColor", color);
            }
            defaultColor = color;
        }

        public void UpdateLineMinDistance(float newValue)
        {
            minDistanceBeforeNewPoint = newValue;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(current_index);
                stream.SendNext(trackPosition);
                stream.SendNext(lineDefaultWidth);
                stream.SendNext(positionCount);
                stream.SendNext(numCapVectices);
                stream.SendNext(linePosition);
                //stream.SendNext(defaultColor);
                stream.SendNext(minDistanceBeforeNewPoint);

            }
            else
            {
                current_index = (int)stream.ReceiveNext();
                trackPosition = (Vector3)stream.ReceiveNext();
                lineDefaultWidth = (float)stream.ReceiveNext();
                positionCount = (int)stream.ReceiveNext();
                numCapVectices = (int)stream.ReceiveNext();
                linePosition = (Vector3)stream.ReceiveNext();
                cameraPosition = (Vector3)stream.ReceiveNext();
                //defaultColor = (Color)stream.ReceiveNext();
                minDistanceBeforeNewPoint = (float)stream.ReceiveNext();

                if (previous_index != current_index)
                {
                    newLine = true;
                }
                if((trackPosition != previous_trackPosition) ||
                    (lineDefaultWidth != previousLineWidth) ||
                    (positionCount != previousPositionCount) ||
                    (numCapVectices != previous_numCapVectices) ||
                    (linePosition != previousLinePosition) ||
                    (minDistanceBeforeNewPoint != previousMinDistanceBeforeNewPoint) ||
                    (cameraPosition != previousCameraPosition))
                {
                    updateLine = true;
                }
            }
        }
    }
}