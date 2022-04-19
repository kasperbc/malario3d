using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{
    public GameObject contains;
    public bool inDestrucable;
    public bool invisible;
    [SerializeField] Material invisibleBrickMat;
    [SerializeField] int coinValue;

    private void Awake()
    {
        if (invisible)
        {
            GetComponent<MeshRenderer>().material = invisibleBrickMat;
        }
        if (coinValue == 0)
        {
            coinValue = 1;
        }
    }

    private void Update()
    {
        if (invisible)
            ChangeColor();
    }

    void ChangeColor()
    {
        Material mat = GetComponent<Renderer>().material;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float distanceToClosestPlayer = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distanceToP = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToP < distanceToClosestPlayer)
            {
                distanceToClosestPlayer = distanceToP;
            }
        }

        float alpha = 1 - distanceToClosestPlayer / 6;
        alpha = Mathf.Clamp(alpha, 0, 1);

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y < transform.position.y - 0.5f)
        {
            Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRB.velocity.y <= 0)
                return;

            playerRB.AddForce(Vector3.down * playerRB.gameObject.GetComponent<PlayerMovement>().jumpForce / 1.75f, ForceMode.Impulse);

            if (!inDestrucable)
                Deconstruct(collision.gameObject);
        }
    }

    void Deconstruct(GameObject destroyer)
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        GetComponent<ParticleSystem>().Play();

        if (contains != null)
        {
            GameObject spawnedObject = Instantiate(contains, transform.position, transform.rotation);

            // COIN
            if (contains.name.Equals("Coin"))
            {
                CoinBehaviour coinBeh = spawnedObject.GetComponent<CoinBehaviour>();

                coinBeh.value = coinValue;

                coinBeh.specialItem = true;
                StartCoroutine(coinBeh.Collect(destroyer.gameObject));
            }
        }
    }
}
