using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public EnemyAI enemy;

    [SerializeField] private float projectileSpeed;
    [SerializeField] public GameObject explosionVFX;
    [SerializeField] private Impact impact;
    [SerializeField] private Transform acid;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        enemy = FindObjectOfType<EnemyAI>();
    }
    private void Update()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
        StartCoroutine(DestroyProjectile());
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.collider.GetComponent<EnemyAI>();
            if (enemy)
            {
                enemy.TakeHit(1);
                Destroy(gameObject);
            }
        }
        else
        {
            if (playerMovement.usingAcid)
            {
                if (other.gameObject.tag == "Floor")
                {
                    Destroy(gameObject);
                    Instantiate(explosionVFX, transform.position, Quaternion.Euler(-90, 0, 0));
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
                if (playerMovement.usingIce)
                {
                    Instantiate(explosionVFX, transform.position, transform.rotation);
                }
            }
        }        
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
