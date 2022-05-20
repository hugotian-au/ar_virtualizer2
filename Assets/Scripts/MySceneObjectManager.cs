using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MySceneObjectManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab to use for representing scene object located in the room")]
    public GameObject sceneBlock1;
    public GameObject sceneBlock2;
    public GameObject sceneBlock3;
    public GameObject sceneBlock4;
    public GameObject sceneBlock5;
    public GameObject sceneBlock6;
    public GameObject sceneBlock7;


    // Start is called before the first frame update
    void Start()
    {
        // PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
        if (!PhotonNetwork.IsMasterClient)
        {
            var sceneobject_name1 = this.sceneBlock1.name + "(Clone)";
            var scene_object1 = GameObject.Find(sceneobject_name1);
            if (scene_object1 == null)
            {
                var pv = GetComponent<PhotonView>();
                pv.RPC("RPC_InstantiateSceneObject", RpcTarget.All,
                    this.sceneBlock1.name, new Vector3(1f, 0.01f, 0.5f), Quaternion.identity);
                // Instantiate(sceneObject1, new Vector3(0, 0, 0), Quaternion.identity);
            }
            var sceneobject_name2 = this.sceneBlock2.name + "(Clone)";
            var scene_object2 = GameObject.Find(sceneobject_name2);
            if (scene_object2 == null)
            {
                var pv = GetComponent<PhotonView>();
                pv.RPC("RPC_InstantiateSceneObject", RpcTarget.All,
                    this.sceneBlock2.name, new Vector3(1f, 0.01f, 0.5f), Quaternion.identity);
                // Instantiate(sceneObject1, new Vector3(0, 0, 0), Quaternion.identity);
            }
            var sceneobject_name3 = this.sceneBlock3.name + "(Clone)";
            var scene_object3 = GameObject.Find(sceneobject_name3);
            if (scene_object3 == null)
            {
                var pv = GetComponent<PhotonView>();
                pv.RPC("RPC_InstantiateSceneObject", RpcTarget.All,
                    this.sceneBlock3.name, new Vector3(1f, 0.01f, 0.5f), Quaternion.identity);
                // Instantiate(sceneObject1, new Vector3(0, 0, 0), Quaternion.identity);
            }
            var sceneobject_name4 = this.sceneBlock4.name + "(Clone)";
            var scene_object4 = GameObject.Find(sceneobject_name4);
            if (scene_object4 == null)
            {
                var pv = GetComponent<PhotonView>();
                pv.RPC("RPC_InstantiateSceneObject", RpcTarget.All,
                    this.sceneBlock4.name, new Vector3(1f, 0.01f, 0.5f), Quaternion.identity);
                // Instantiate(sceneObject1, new Vector3(0, 0, 0), Quaternion.identity);
            }
            var sceneobject_name5 = this.sceneBlock5.name + "(Clone)";
            var scene_object5 = GameObject.Find(sceneobject_name5);
            if (scene_object5 == null)
            {
                var pv = GetComponent<PhotonView>();
                pv.RPC("RPC_InstantiateSceneObject", RpcTarget.All,
                    this.sceneBlock5.name, new Vector3(1f, 0.01f, 0.5f), Quaternion.identity);
                // Instantiate(sceneObject1, new Vector3(0, 0, 0), Quaternion.identity);
            }
            var sceneobject_name6 = this.sceneBlock6.name + "(Clone)";
            var scene_object6 = GameObject.Find(sceneobject_name6);
            if (scene_object6 == null)
            {
                var pv = GetComponent<PhotonView>();
                pv.RPC("RPC_InstantiateSceneObject", RpcTarget.All,
                    this.sceneBlock6.name, new Vector3(1f, 0.01f, 0.5f), Quaternion.identity);
                // Instantiate(sceneObject1, new Vector3(0, 0, 0), Quaternion.identity);
            }
            var sceneobject_name7 = this.sceneBlock7.name + "(Clone)";
            var scene_object7 = GameObject.Find(sceneobject_name3);
            if (scene_object7 == null)
            {
                var pv = GetComponent<PhotonView>();
                pv.RPC("RPC_InstantiateSceneObject", RpcTarget.All,
                    this.sceneBlock7.name, new Vector3(1f, 0.01f, 0.5f), Quaternion.identity);
                // Instantiate(sceneObject1, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }
    }

    [PunRPC]
    void RPC_InstantiateSceneObject(string name, Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.InstantiateSceneObject(name, position, rotation, 0);
    }
}
