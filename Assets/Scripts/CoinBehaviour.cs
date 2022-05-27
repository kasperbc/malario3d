using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinBehaviour : MonoBehaviour
{
    enum CoinType { Coin, Token, Gem }

    public int value;
    public bool specialItem;
    public bool invisible;
    [SerializeField] Material invisMat;
    [SerializeField] CoinType coinType;
    [SerializeField] float revealRange;
    private void Start()
    {
        if (value == 0 && !specialItem)
        {
            value = 1;
        }

        if (invisible)
        {
            GetComponent<MeshRenderer>().material = invisMat;
        }

        if (revealRange == 0)
        {
            revealRange = 6;
        }
    }

    private void Update()
    {
        if (invisible)
        {
            ChangeColor();
        }
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

        float alpha = 1 - distanceToClosestPlayer / revealRange;
        alpha = Mathf.Clamp(alpha, 0, 1);

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Collect(other.gameObject));
        }
    }
    public IEnumerator Collect(GameObject collector)
    {
        CoinCounter counter = GameObject.Find("CoinCount").GetComponent<CoinCounter>();

        Animator anim = GetComponent<Animator>();

        GetComponent<Collider>().enabled = false;
        counter.AddCoins(value, collector.GetComponent<PlayerHealth>());

        if (specialItem)
        {
            anim.SetBool("Collect", true);

            yield return new WaitForSeconds(0.75f);
        }

        if (coinType == CoinType.Token)
        {
            GameManager.instance.CollectToken();
        }
        if (coinType == CoinType.Gem)
        {
            StartCoroutine(GameManager.instance.LoadLevel("Hub"));
        }

        GetComponent<Renderer>().enabled = false;

        if (transform.childCount > 0)
            transform.GetChild(0).gameObject.SetActive(false);

        GetComponent<ParticleSystem>().Play();

        anim.enabled = false;

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
