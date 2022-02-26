using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VRUserController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Fields
    // Find the Main Camera's position
    private GameObject parent;
    private Vector3 position = new Vector3(0, 0, 0);
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
        }
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {
        if (photonView.IsMine)
        {
            position = transform.localPosition;
        }
        else
        {
            transform.localPosition = position;
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
