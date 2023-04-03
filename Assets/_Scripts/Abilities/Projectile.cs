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
    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other);
        Destroy(gameObject);
        Instantiate(explosionVFX, transform.position, transform.rotation);
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
