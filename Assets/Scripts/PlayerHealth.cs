using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(0, 3)] public int health;
    public bool invincible;
    [SerializeField] Sprite[] lifes = new Sprite[4];
    [SerializeField] Image lifeBar;

    PlayerMovement movement;
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        health = 3;
    }

    private void Update()
    {
        lifeBar.sprite = lifes[health];

        if (transform.position.y < -20)
        {
            Die(false);
        }
    }

    public void TakeDamage()
    {
        if (!invincible)
            health--;

        print("Took damage. Health: " + health);

        if (health <= 0)
        {
            Die(true);
            return;
        }

        StartCoroutine(InvisFrames());

        CoinCounter counter = GameObject.Find("CoinCount").GetComponent<CoinCounter>();
        counter.tookDamageAt = counter.displayCount;

        lifeBar.GetComponent<Animator>().SetTrigger("Bop");
        lifeBar.GetComponent<Animator>().SetInteger("Health", health);
    }

    public void RecoverHealth(int amount)
    {
        if (health >= 3)
            return;

        health += amount;

        lifeBar.GetComponent<Animator>().SetTrigger("Bop");
        lifeBar.GetComponent<Animator>().SetInteger("Health", health);
    }

    public void Die(bool knockBack)
    {
        movement.dead = true;
        health = 0;
        lifeBar.GetComponent<Animator>().SetTrigger("Bop");

        if (knockBack)
            GetComponent<Rigidbody>().AddForce(transform.forward * -3 + Vector3.up * 3, ForceMode.Impulse);

        StartCoroutine(ResetChar());
    }

    IEnumerator ResetChar()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject.Find("Fade").GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.25f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator InvisFrames()
    {
        invincible = true;

        Renderer rend = GetComponent<Renderer>();
        rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 0.5f);

        yield return new WaitForSeconds(1);

        rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 1f);
        invincible = false;
    }
}
