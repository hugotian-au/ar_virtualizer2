using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ARUserController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Fields
    // Find the Main Camera's position
    private GameObject parent;
    private GameObject tracker;
    private Vector3 position = new Vector3(0, 0, 0);
    private Vector3 trackerPosition = new Vector3(0, 0, 0);
    private Vector3 prevPosition = new Vector3(0.0f, 0.0f, 0.0f);
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
            parent = GameObject.Find("Main Camera");
            transform.parent = parent.transform;
            transform.localPosition = new Vector3(0.0f, -1.8f, 0.0f);

            tracker = GameObject.Find("TrackerHandler");
        }
        else
        {
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
            if (tracker != null)
            {
                position = transform.position - tracker.transform.position;
            }
            else
            {
                position = transform.position;
            }
        }
        else
        {
            transform.LookAt(position);
            transform.localPosition = position;
            Vector3 diff = transform.localPosition - prevPosition;
            // anim.SetFloat("VerticalMov", Input.GetAxis("Vertical"));
            if (diff.x > 0.1f || diff.z > 0.1f || diff.x < -0.1f || diff.z < -0.1f)
            {
                // animator.SetFloat("VerticalMov", 0.2f);
                animator.SetFloat("Speed", 0.5f);
                // animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
            }
            else
            {
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
