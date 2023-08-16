using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootSpeed;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private KeyCode shootInput;
    private Rigidbody playerRb;

    public override void OnNetworkSpawn()
    {
        playerRb = GetComponent<Rigidbody>();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(shootInput))
        {
            Shoot();
        }
    }

    [ServerRpc]
    public void RequestShootServerRPC()
    {
        if (Input.GetKeyDown(shootInput)) ;
            {

            if (!IsServer && IsLocalPlayer)
            {
                Shoot(); 
            }
            else if (!IsClient && IsLocalPlayer)
            {
                RequestShootServerRPC(); 
            }
        }
    }

    private void Shoot()
    {
        GameObject tempBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        tempBullet.GetComponent<NetworkObject>().Spawn();

        tempBullet.GetComponent<Rigidbody>().AddForce(playerRb.velocity + tempBullet.transform.forward * shootSpeed, ForceMode.VelocityChange);
        Destroy(tempBullet, 5f);
    }
}
