using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGround : MonoBehaviour
{
    public float timer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().speed = 1 / timer;

        yield return new WaitForSeconds(timer);

        GetComponent<Collider>().enabled = false;
    }
}
