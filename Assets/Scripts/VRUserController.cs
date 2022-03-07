using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VRUserController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Fields
    // Find the Main Camera's position
    private GameObject parent_camera;
    private GameObject tracker;
    private Vector3 position = new Vector3(0, 0, 0);
    private Vector3 prevPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 relativeCameraPos;
    private Animator animator;
    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>

    void Start()
    {
        if (photonView.IsMine)
        {
            parent_camera = GameObject.Find("CenterEyeAnchor");
            if (parent_camera != null)
            {
                var camera_position = parent_camera.transform.position;
                var lookPos = camera_position - transform.position;
                lookPos.y = 0;
                // var rotation = Quaternion.LookRotation(lookPos);
                // rotation *= Quaternion.Euler(0, 90, 0); // this adds a 90 degrees Y rotation
                // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                transform.LookAt(lookPos);
                transform.position = new Vector3(position.x, 0.0f, position.z);
            }
        }
        else
        {
            tracker = GameObject.Find("TrackerHandler");
            relativeCameraPos = transform.position - tracker.transform.position;
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {
        if (photonView.IsMine)
        {
            var camera_position = parent_camera.transform.position;
            var lookPos = camera_position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            // rotation *= Quaternion.Euler(0, 90, 0); // this adds a 90 degrees Y rotation
            // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*2);
            //transform.rotation = rotation;
            transform.LookAt(lookPos);
            transform.position = new Vector3(camera_position.x, 0.0f, camera_position.z);

            position = transform.localPosition;  // Use the world position since the localPosition is not changed
        }
        else
        {
            Vector3 diff = position - prevPosition;
            // anim.SetFloat("VerticalMov", Input.GetAxis("Vertical"));
            if (diff.x > 0.05f || diff.z > 0.05f || diff.x < -0.05f || diff.z < -0.05f)
            {
                var new_position = new Vector3(position.x, 0, position.z);
                var lookat_pos = new Vector3(position.x, tracker.transform.position.y, position.z);
                transform.LookAt(lookat_pos);
                transform.localPosition = new_position;
                // animator.SetFloat("VerticalMov", 0.2f);
                animator.SetFloat("Speed", 0.3f);
                // animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
            }
            else
            {
                var new_position = new Vector3(position.x, 0, position.z);
                var lookat_pos = new Vector3(position.x, tracker.transform.position.y, position.z);
                transform.LookAt(lookat_pos);
                transform.localPosition = new_position;
                animator.SetFloat("Speed", 0.0f);
            }
            // anim.SetFloat("HorizontalMov", Input.GetAxis("Horizontal"));

            prevPosition = position;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(position);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// MonoBehaviour method called when the Collider 'other' enters the trigger.
    /// Affect Health of the Player if the collider is a beam
    /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
    /// One could move the collider further away to prevent this or check if the beam belongs to the player.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    /// <summary>
    /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
    /// We're going to affect health while the beams are touching the player
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerStay(Collider other)
    {
        // we dont' do anything if we are not the local player.
        if (!photonView.IsMine)
        {
            return;
        }
    }

    #endregion

    #region Custom

    /// <summary>
    /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
    /// </summary>
    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
        }
        if (Input.GetButtonUp("Fire1"))
        {
        }
    }

    #endregion
}
