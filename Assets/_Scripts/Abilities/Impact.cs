using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private void Update()
    {
        StartCoroutine(DestroyImpact());
    }
    IEnumerator DestroyImpact()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
