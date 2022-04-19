using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAI : MonoBehaviour
{
    [SerializeField] GameObject target;
    public float speed;
    private void Start()
    {
        if (speed == 0)
        {
            speed = 5;
        }

        StartCoroutine(DeathClock());
    }
    void Update()
    {
        target = FindNearestPlayer();

        if (target == null)
            return;

        Vector3 targetPosition = target.transform.position;

        Vector3 targetDir = (targetPosition - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        targetRot.x = 0;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 0.5f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    GameObject FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(transform.position, player.transform.position);
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            Rigidbody myRb = GetComponent<Rigidbody>();

            if (playerRb.transform.position.y > transform.position.y)
            {
                playerRb.AddForce(Vector3.up * 8, ForceMode.Impulse);
                StartCoroutine(Die());
                return;
            }

            playerRb.AddForce(playerRb.transform.forward * -5 + Vector3.up * 3, ForceMode.Impulse);
            StartCoroutine(Die());

            playerRb.gameObject.GetComponent<PlayerHealth>().TakeDamage();

            StartCoroutine(Knockback(playerRb.gameObject.GetComponent<PlayerMovement>()));
        }
    }

    IEnumerator Knockback(PlayerMovement moveScript)
    {
        moveScript.canMove = false;

        yield return new WaitForSeconds(0.75f);

        moveScript.canMove = true;
    }

    IEnumerator Die()
    {
        speed = 0;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(Vector3.down * 3, ForceMode.Impulse);
        GetComponent<Collider>().enabled = false;
        GetComponent<ParticleSystem>().Stop();

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }

    IEnumerator DeathClock()
    {
        yield return new WaitForSeconds(5);

        StartCoroutine(Die());
    }
}
