using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProjectile", 5);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
