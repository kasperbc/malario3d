using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce;
    public float minVelocitySustain;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRB = other.gameObject.GetComponent<Rigidbody>();

            print(playerRB.velocity.magnitude);

            if (playerRB.velocity.magnitude < minVelocitySustain)
                playerRB.velocity = Vector3.zero;
            else if (playerRB.velocity.y < 0)
                playerRB.velocity = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);

            GetComponent<Animator>().SetTrigger("Bounce");

            playerRB.AddForce(Vector3.up * bounceForce * 2.5f, ForceMode.VelocityChange);
        }
    }
}
