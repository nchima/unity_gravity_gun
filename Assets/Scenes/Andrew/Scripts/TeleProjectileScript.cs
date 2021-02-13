using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleProjectileScript : MonoBehaviour
{
    public float speed;
    public float projectileGravity;
    float lastHitDist = 999;
    public float projectileDestroyTime;
    float destroyTimer;

    Vector3 lastHitPoint;
    Vector3 lastHitNorm;
    public Vector3 moveDirection;

    public GameObject hitMarker;

    public Transform player;

    bool detected;
    public bool showDebugHitMarkers;    

    void FixedUpdate()
    {
        moveDirection.y -= projectileGravity * Time.fixedDeltaTime;
        transform.forward = moveDirection;
        transform.position += moveDirection * speed * Time.fixedDeltaTime;

        if (Physics.Raycast(transform.position, moveDirection.normalized, out RaycastHit hit)) //detects if the projectile has hit something in front of it, then teleports player, and allows them to shoot again
        {
            detected = true; // shows if the projectile has ever detected a collider, so that if it suddenly doesnt any longer, it will destroy itself, as it has passes through a collider in this case.
            if (hit.distance <= transform.localScale.x / 2) // if hitting a wall within the radius of the projectile, teleport and destroy
            {
                TeleportMe(lastHitPoint, lastHitNorm);
                if (showDebugHitMarkers)
                    SpawnHitMarker(lastHitPoint, lastHitNorm);
                player.GetComponent<ShootTeleProjectile>().ableToShoot = true;
                Destroy(this.gameObject);
            }

            if (hit.distance > lastHitDist)// if suddenly the distance between the course of trajectory increases instead of decreases, it has passed through a collider, and should destory itself, and teleport the player to the last point of contact
            {
                TeleportMe(lastHitPoint, lastHitNorm);
                if (showDebugHitMarkers)
                    SpawnHitMarker(lastHitPoint, lastHitNorm);
                player.GetComponent<ShootTeleProjectile>().ableToShoot = true;
                Destroy(this.gameObject);
            }

            lastHitPoint = hit.point;
            lastHitNorm = hit.normal;
            lastHitDist = hit.distance;
        }
        else if (detected) // if the projectile detects a collider, then suddenly detects only the void, it has passed through a collider, and thouls teleport the player to the last detected point on the trajectory.
        {
            TeleportMe(lastHitPoint, lastHitNorm);
            if (showDebugHitMarkers)
                SpawnHitMarker(lastHitPoint, lastHitNorm);
            player.GetComponent<ShootTeleProjectile>().ableToShoot = true;
            Destroy(this.gameObject);
        }
        else // if the projectile was shot into a void with no colliders, it sould wait a designated amount of time, then destroy itself.
        {
            destroyTimer += Time.fixedDeltaTime;
            if (destroyTimer>=projectileDestroyTime) {
                player.GetComponent<ShootTeleProjectile>().ableToShoot = true;
                Destroy(this.gameObject);
            }
        }
    }

    void TeleportMe(Vector3 pos, Vector3 normal)
    {
            player.position = pos + normal;
    }

    void SpawnHitMarker(Vector3 pos, Vector3 normal) //leaves a mark of where the projectile lands after destroyed [debugging purposes, but can be used to cool effect maybe]
    {
        GameObject h = Instantiate(hitMarker, pos, Quaternion.identity);
        h.transform.up = normal;
    }
}
