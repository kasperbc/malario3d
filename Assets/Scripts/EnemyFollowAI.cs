using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyFollowAI : MonoBehaviour
{
    public int behaviourMode;
    // 0: Wandering, 1: Following player, 2: Running away from player, 3: Dead
    Vector3 targetPosition;
    public Transform target;
    public float moveSpeed;
    public float detectionRange;

    Vector3 wanderPos;
    Vector3 spawnPoint;
    Vector3 spawnRot;
    bool wandering;
    bool wanderLoop;

    public bool[] constraints = new bool[3];

    [SerializeField] GameObject turnsIntoAfterDying;
    [SerializeField] bool noDeathAnim;
    [SerializeField] bool immuneToStomp;
    [SerializeField] Material[] materials;
    [SerializeField] int materialID;
    void Start()
    {
        spawnPoint = transform.position;
        spawnRot = transform.eulerAngles;
        GetComponent<Renderer>().material = materials[materialID];
    }

    void Update()
    {
        Vector3 targetRot = transform.eulerAngles;

        if (!constraints[0])
            targetRot.x = spawnRot.x;
        if (!constraints[1])
            targetRot.y = spawnRot.y;
        if (!constraints[2])
            targetRot.z = spawnRot.z;

        transform.eulerAngles = targetRot;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        bool playerInRange = false;
        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            
            if (distanceToPlayer < 15)
            {
                playerInRange = true;
            }
        }

        if (!playerInRange)
        {
            behaviourMode = 3;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        switch (behaviourMode)
        {
            case 0:
                WanderAimlessly();
                break;
            case 1:
                MoveTowardsTarget();
                break;
        }

        if (transform.position.y < -20)
        {
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject.transform;

            behaviourMode = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = null;
            
            behaviourMode = 0;
        }
    }

    //
    // MOVE TOWARDS TARGET
    //
    void MoveTowardsTarget()
    {
        targetPosition = target.position;

        Vector3 targetDir = (targetPosition - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);

        transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 2);
    }

    //
    // WANDER
    //
    void WanderAimlessly()
    {
        if (wandering)
        {
            transform.position = Vector3.MoveTowards(transform.position, wanderPos, moveSpeed / 50);

            transform.LookAt(wanderPos);
            Debug.DrawLine(transform.position, wanderPos, Color.red);
        }

        if (!wanderLoop)
        {
            StartCoroutine(Wander());
        }
    }

    IEnumerator Wander()
    {
        wanderLoop = true;
        wandering = false;

        yield return new WaitForSeconds(Random.Range(1, 3f));

        wandering = true;

        wanderPos = new Vector3(spawnPoint.x + Random.Range(-3, 3), spawnPoint.y, spawnPoint.z + Random.Range(-3, 3));

        if (!Physics.Raycast(wanderPos, Vector3.down * 1, 1))
        {
            wanderPos = new Vector3(spawnPoint.x + Random.Range(-3, 3), spawnPoint.y, spawnPoint.z + Random.Range(-3, 3));
        }

        yield return new WaitForSeconds(Random.Range(1, 5f));

        wanderLoop = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && behaviourMode != 3)
        {

            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            Rigidbody myRb = GetComponent<Rigidbody>();

            if (playerRb.transform.position.y > transform.position.y + 0.5f && !immuneToStomp)
            {
                if (!noDeathAnim)
                    transform.localScale = new Vector3(transform.localScale.z, 0.1f, transform.localScale.z);
                else
                    transform.localScale = Vector3.zero;

                playerRb.AddForce(Vector3.up * 6, ForceMode.Impulse);
                StartCoroutine(Die());
                return;
            }

            playerRb.AddForce(playerRb.transform.forward * -5 + Vector3.up * 3, ForceMode.Impulse);
            myRb.AddForce(transform.forward * -5 + Vector3.up * 3, ForceMode.Impulse);

            playerRb.gameObject.GetComponent<PlayerHealth>().TakeDamage();

            StartCoroutine(Knockback(playerRb.gameObject.GetComponent<PlayerMovement>()));
        }
    }

    IEnumerator Knockback(PlayerMovement moveScript)
    {
        moveScript.canMove = false;

        yield return new WaitForSeconds(0.5f);

        moveScript.canMove = true;
    }

    IEnumerator Die()
    {
        behaviourMode = 3;
        moveSpeed = 0;

        if (turnsIntoAfterDying != null)
        {
            Instantiate(turnsIntoAfterDying, transform.position, transform.rotation);
        }

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.15f);

        Gizmos.DrawSphere(transform.position + transform.forward * detectionRange, detectionRange * 1.2f);
    }
}
