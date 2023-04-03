using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject explosionVFX;


    private void Update()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        Destroy(gameObject);
        Instantiate(explosionVFX, transform.position, transform.rotation);
    }
}
