using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTeleProjectile : MonoBehaviour
{

    public Camera firstPersonCamera; //Assign the first 
    public KeyCode shootKey = KeyCode.Mouse0; //Assign in editor what key you use to shoot the projectile.

    public GameObject gunModel;
    public GameObject projectile;

    public float projectileSpeed;

    Transform hand;

    void Start()
    {
        GameObject g = Instantiate(gunModel, firstPersonCamera.transform);
        hand = g.transform;
    }

    void Update()
    {

        for (int i = 0; i < 64; i++)
        {

        }

        if (Input.GetKeyDown(shootKey)) //runs when the assigend 'shoot' key is pressed
        {
            ShootProjectile(projectileSpeed);
        }
    }

    public void ShootProjectile(float spd)
    {
        GameObject p = Instantiate(projectile, hand.position, Quaternion.identity);
        p.GetComponent<TeleProjectileScript>().moveDirection = ((firstPersonCamera.transform.position + (firstPersonCamera.transform.forward * 99999)) - p.transform.position).normalized;
        //print(((firstPersonCamera.transform.position + (firstPersonCamera.transform.forward * 99999)) - p.transform.position).normalized);
        p.GetComponent<TeleProjectileScript>().speed = spd;
    }
}
