using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    public string levelName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animator anim = other.gameObject.GetComponent<Animator>();

            other.GetComponent<PlayerMovement>().enabled = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<ConstantForce>().enabled = false;

            anim.enabled = true;
            anim.SetTrigger("Warp");

            StartCoroutine(GameManager.instance.LoadLevel(levelName));
        }
    }
}
