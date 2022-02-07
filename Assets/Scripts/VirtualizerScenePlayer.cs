
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace start
{
    public class VirtualizerScenePlayer : NetworkBehaviour
    {
        private GameObject imageTarget;

        public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        public NetworkVariableQuaternion Rotation = new NetworkVariableQuaternion(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        private Vector3 prevPosition = new Vector3(0.0f, 0.0f, 0.0f);
        private void Awake()
        {
            /*
            NetworkObject netObject = GetComponent<NetworkObject>();
            netObject.CheckObjectVisibility = ((clientId) => {
                // return true to show the object, return false to hide it
                if (IsLocalPlayer)
                {
                    // Only show the object to players that are within 5 meters. Note that this has to be rechecked by your own code
                    // If you want it to update as the client and objects distance change.
                    // This callback is usually only called once per client
                    print("This is local player");
                    return false;
                }
                else
                {
                    // Dont show this object
                    return true;
                }
            });
            */
        }

        void Start()
        {
            imageTarget = GameObject.Find("ImageTargetPos");
        }

        public override void NetworkStart()
        {
            GameObject lso = GameObject.Find("LocalScene");
            if (lso != null)
            {
                if (IsLocalPlayer)
                {
                    GameObject go = this.gameObject.transform.GetChild(1).gameObject; 
                    if (go != null)
                    {
                        print("This is player at AR side, disalbe remote player");
                        go.SetActive(false);
                    }
                    this.gameObject.transform.parent = GameObject.Find("Main Camera").transform;
                    this.gameObject.transform.localScale = new Vector3(0, 0, 0);

                }
                else
                {
                    GameObject go = this.gameObject.transform.GetChild(0).gameObject;
                    if (go != null)
                    {
                        print("This is player at VR side, disalbe local player");
                        go.SetActive(false);
                    }
                }
            }
            // Move();
        }

        [ServerRpc(RequireOwnership = false)]
        void GetRemotePlayerPositionServerRpc(ServerRpcParams rpcParams = default)
        {
             // Don't need to implement at client side
        }

        [ClientRpc]
        void GetLocalPlayerPositionClientRpc(ClientRpcParams rpcParams = default)
        {
            print("Called from VR player for getting client player position");
            Transform trans = GameObject.Find("ImageTarget_Oxygen").transform;
            Vector3 pos = transform.localPosition + trans.localPosition;
            Vector3 final_pos = new Vector3(pos.x, 0, pos.z);
            Position.Value = final_pos;
            print("Position.Value of AR player is: " + Position.Value);
        }

        [ServerRpc(RequireOwnership = false)]
        void GetRemotePlayerRotationServerRpc(ServerRpcParams rpcParams = default)
        {
            // Don't need to implement at client side
        }

        [ClientRpc]
        void GetLocalPlayerRotationClientRpc(ClientRpcParams rpcParams = default)
        {
            Transform trans = GameObject.Find("ImageTarget_Oxygen").transform;
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            rotation.eulerAngles = transform.rotation.eulerAngles - trans.rotation.eulerAngles;
            Rotation.Value = rotation;
        }

        void Update()
        {
            Animator anim;

            if (IsLocalPlayer)
            {
                Transform trans = GameObject.Find("Main Camera").transform;
                transform.localPosition = new Vector3(trans.localPosition.x, 0, trans.localPosition.z);
                transform.localRotation = trans.localRotation;
            }
            else if (!NetworkManager.Singleton.IsServer)
            {
                GameObject remotePlayer = this.gameObject.transform.GetChild(1).gameObject;
                anim = remotePlayer.GetComponent(typeof(Animator)) as Animator;
                // anim.SetFloat("VerticalMov", Input.GetAxis("Vertical"));
                // Get coordiante of specified image target
                GetRemotePlayerPositionServerRpc();
                // Transform trans = GameObject.Find("ImageTarget_Oxygen").transform;
                FindImageTargetPosition ImageTargetPosition = imageTarget.GetComponent<FindImageTargetPosition>();
                print("Position.Value of VR player is: " + Position.Value);
                //Vector3 pos = Position.Value + trans.localPosition;
                Vector3 pos = Position.Value + ImageTargetPosition.origin_position;
                Vector3 final_pos = new Vector3(pos.x, ImageTargetPosition.origin_position.y, pos.z);
                // Vector3 final_pos = new Vector3(pos.x, trans.localPosition.y, pos.z);
                // transform.localPosition = final_pos;
                transform.LookAt(final_pos);
                //transform.Translate(final_pos);
                transform.localPosition = final_pos;

                Vector3 diff = Position.Value - prevPosition;
                print("diff is " + diff);
                anim.SetFloat("VerticalMov", 0.2f);
                // anim.SetFloat("VerticalMov", Input.GetAxis("Vertical"));
                if (diff.x > 0.1f || diff.z > 0.1f || diff.x < -0.1f || diff.z < -0.1f)
                {
                    anim.SetFloat("VerticalMov", 0.2f);
                }
                else
                {
                    anim.SetFloat("VerticalMov", 0.0f);
                }
                anim.SetFloat("HorizontalMov", Input.GetAxis("Horizontal"));

                prevPosition = Position.Value;
            }
        }
    }
}