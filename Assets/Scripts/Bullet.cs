using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public ulong clientID;

    private void OnCollisionEnter(Collision collision)
    {
        if(IsServer)
        {
            PlayerDamage other = collision.gameObject.GetComponent<PlayerDamage>();

            if(other != null && clientID != other.OwnerClientId)
            {
                other.DealDamage();
                Debug.Log(clientID + " " + other.OwnerClientId);

                //Update Score
                GameManager.Instance.AddScore(clientID);
            }

            Destroy(gameObject);
        }
    }
}
