using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonAI : MonoBehaviour
{
    public float fireRate;
    public GameObject projectile;
    [SerializeField] GameObject target;
    void Start()
    {
        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos;
        if (target != null)
        {
            targetPos = target.transform.position;
        }
        else
        {
            return;
        }

        transform.LookAt(target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = null;
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireRate);

        if (target != null)
            Instantiate(projectile, transform.position + transform.forward * 1, transform.rotation, null);

        StartCoroutine(Fire());
    }
}
