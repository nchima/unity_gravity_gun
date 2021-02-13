using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTeleProjectile : MonoBehaviour
{
    public int projectileBounces;

    public Transform player;

    public Camera firstPersonCamera; //Assign the first 
    public KeyCode shootKey = KeyCode.Mouse0; //Assign in editor what key you use to shoot the projectile.

    public GameObject gunModel;
    public GameObject projectile;

    public float projectileSpeed = 30;
    public float projectileGravity = .8f;
    public float projectileDestroyTime = 2;

    Transform hand;

    int layerMask = 1 << 9;

    public bool ableToShoot = true;

    void Start()
    {
        //If the Camera is not assigned, check to see if this is attached to a camera.
        if (firstPersonCamera==null)
        {
            print($"[{this}]: No Camera Assigned, Assigning Camera '{GetComponentInChildren<Camera>().name}' as FPS Camera...");
            firstPersonCamera = this.gameObject.GetComponentInChildren<Camera>(); 
        }
        if (player == null)
        {
            print($"[{this}]: No Player Transform Assigned, Assigning Transform '{GetComponentInChildren<Transform>()}'");
            player = GetComponentInChildren<Transform>();
        }
        //spawn in the 'gun' in the lower right hand side
        GameObject g = Instantiate(gunModel, firstPersonCamera.transform);
        hand = g.transform;
    }

    List<Vector3> trajPoints = new List<Vector3>();
    Vector3 aimDir;

    public Vector3 trajDest;

    void Update()
    {
        if (!firstPersonCamera) { print($"No Camera Assigned to [{this}]!"); return; }
        aimDir = ((firstPersonCamera.transform.position + (firstPersonCamera.transform.forward * 99999)) - hand.transform.position).normalized;
        if (Input.GetKeyDown(shootKey) && ableToShoot) //runs when the assigend 'shoot' key is pressed
        {           
            ShootProjectile(projectileSpeed);
        }
    }


    private void LateUpdate()
    {
        if (!firstPersonCamera) { print($"No Camera Assigned to [{this}]!"); return; }
        Vector3 pos = hand.position;
        Vector3 vel = aimDir * projectileSpeed;
        Vector3 grav = new Vector3(0, -projectileGravity, 0) * projectileSpeed;
        trajPoints.Clear();

        Vector3 lastHit = Vector3.zero;
        float lastDist = Mathf.Infinity;

        int i = 0;

        while (true)
        {
            trajPoints.Add(pos);
            vel = vel + grav * Time.fixedDeltaTime;
            pos = pos + vel * Time.fixedDeltaTime;
            if (i > 0)
            {
                Debug.DrawLine(trajPoints[i], trajPoints[i - 1]);
            }
            if (Physics.Raycast(pos, vel.normalized, out RaycastHit trajHit))
            {
                if (lastDist < trajHit.distance)
                {
                    trajDest = lastHit;
                    break;
                }
                lastHit = trajHit.point;
                lastDist = trajHit.distance;
            }
            else
            {
                if (lastDist != Mathf.Infinity)
                {
                    trajDest = lastHit;
                    break;
                }
            }
            i++;
            if (i > 128)
            {
                print("reaching limit...");
                break;
            }
        }

    }

    public void ShootProjectile(float spd)
    {
        ableToShoot = false;

        GameObject p = Instantiate(projectile, hand.position, Quaternion.identity);
        p.GetComponent<TeleProjectileScript>().moveDirection = aimDir;
        p.GetComponent<TeleProjectileScript>().moveDirection = aimDir;
        p.GetComponent<TeleProjectileScript>().speed = spd;
        p.GetComponent<TeleProjectileScript>().player = player;
        p.GetComponent<TeleProjectileScript>().projectileGravity = projectileGravity;
        p.GetComponent<TeleProjectileScript>().projectileBounces = projectileBounces;
        p.GetComponent<TeleProjectileScript>().projectileDestroyTime = projectileDestroyTime;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(trajDest,Vector3.one*.2f);
    }
}
