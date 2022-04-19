using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFakeObjAI : MonoBehaviour
{
    public Vector3 targetPoint;
    public float speed;
    bool flying;
    bool triggeredAttack;
    void Start()
    {
        if (speed == 0)
        {
            speed = 3;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (flying)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed);
        }

        if (triggeredAttack && !flying)
        {
            transform.position += Vector3.up / 10;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !triggeredAttack)
        {
            StartFlying(other.gameObject, true);
        }
    }

    public void StartFlying(GameObject target, bool chargeUp)
    {
        triggeredAttack = true;

        if (chargeUp)
            StartCoroutine(ChargeUpAnim(target.transform.position));
    }

    IEnumerator ChargeUpAnim(Vector3 target)
    {
        GetComponent<Animator>().SetTrigger("Fly");

        yield return new WaitForSeconds(2);

        targetPoint = target;
        flying = true;
    }
}
