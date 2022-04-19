using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float platformSpeed;
    public float waitingTime;
    public bool moving;
    public bool startMovingOnContact;
    bool movingTowardsEnd;
    bool madeContact;
    [SerializeField] bool relativeEndPoint;
    [SerializeField] float timeEnteredPlatform;
    void Start()
    {
        if (startPoint == Vector3.zero)
        {
            startPoint = transform.position;
        }
        else
        {
            transform.position = startPoint;
        }

        if (relativeEndPoint)
            endPoint = startPoint + endPoint;

        relativeEndPoint = false;

        if (!startMovingOnContact)
        {
            StartCoroutine(Wait(waitingTime));
        }
    }

    void FixedUpdate()
    {
        if (moving)
        {
            if (movingTowardsEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPoint, platformSpeed / 10);
                if (transform.position == endPoint)
                {
                    StartCoroutine(Wait(waitingTime));
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPoint, platformSpeed / 10);
                if (transform.position == startPoint)
                {
                    StartCoroutine(Wait(waitingTime));
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            if (startMovingOnContact && !madeContact)
            {
                madeContact = true;
                timeEnteredPlatform = Time.timeSinceLevelLoad;
                StartCoroutine(Wait(1));
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            collision.transform.localScale = new Vector3(1, 1.5f, 1);
            if (Time.timeSinceLevelLoad >= timeEnteredPlatform + 0.33f)
            {
                Vector3 targetDir;
                if (movingTowardsEnd)
                    targetDir = (endPoint - transform.position).normalized;
                else
                    targetDir = (startPoint - transform.position).normalized;

                if (moving)
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(targetDir * (platformSpeed * 3), ForceMode.Impulse);

                timeEnteredPlatform = Mathf.Infinity;
            }
        }
    }

    IEnumerator Wait(float seconds)
    {
        moving = false;
        movingTowardsEnd = !movingTowardsEnd;

        if (!(startMovingOnContact && !madeContact))
        {
            yield return new WaitForSeconds(seconds);
        }

        moving = true;
    }

    private void OnDrawGizmos()
    {
        Vector3 offSet;

        if (relativeEndPoint)
        {
            offSet = startPoint + endPoint;
        }
        else
        {
            offSet = endPoint;
        }

        Gizmos.color = new Color(1, 0.5f, 0);
        Gizmos.DrawLine(startPoint, offSet);

        if (startPoint == Vector3.zero)
        {
            startPoint = transform.position;
        }
    }
}
