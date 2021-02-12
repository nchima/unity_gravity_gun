using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleProjectileScript : MonoBehaviour
{

    void Start()
    {
    
    }

    public Vector3 moveDirection;
    public float speed;

    public float projectileGravity;

    Vector3 lastHitPoint;
    Vector3 lastHitNorm;
    float lastHitDist = 999;

    public GameObject hitMarker;

    bool detected;
    void FixedUpdate()
    {
        moveDirection.y -= projectileGravity * Time.fixedDeltaTime;
        transform.forward = moveDirection;
        transform.position += moveDirection * speed * Time.fixedDeltaTime;

        if (Physics.Raycast(transform.position, moveDirection.normalized, out RaycastHit hit))
        {
            detected = true;
            if (hit.distance <= transform.localScale.x / 2)
            {
                SpawnHitMarker(hit.point, hit.normal);
                Destroy(this.gameObject);
            }

            if (hit.distance > lastHitDist) { SpawnHitMarker(lastHitPoint, lastHitNorm); Destroy(this.gameObject); }

            lastHitPoint = hit.point;
            lastHitNorm = hit.normal;
            lastHitDist = hit.distance;
        }
        else if(detected)
        {
            SpawnHitMarker(lastHitPoint, lastHitNorm);
            Destroy(this.gameObject);
        }
    }

    void SpawnHitMarker(Vector3 pos, Vector3 normal)
    {
        GameObject h = Instantiate(hitMarker, pos, Quaternion.identity);
        h.transform.up = normal;
    }
}
