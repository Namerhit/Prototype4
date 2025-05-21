using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    void Update()
    {
        if (!(transform.position.y < -2)) return;
        
        Destroy(gameObject);
    }
}
