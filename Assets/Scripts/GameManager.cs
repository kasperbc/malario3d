using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool paused;
    public static GameManager instance;
    [SerializeField] GameObject pauseCam;
    GameObject coinCount;
    GameObject healthCount;
    GameObject pauseText;
    public bool tokenCollected;
    GameObject tokenIndicator;
    [SerializeField] Sprite tokenSprite;
    void Start()
    {
        paused = false;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        coinCount = GameObject.Find("CoinCount");
        healthCount = GameObject.Find("HealthIndicator");
        pauseText = GameObject.Find("PauseText");
        tokenIndicator = GameObject.Find("TokenIndicator");

        tokenCollected = false;
    }

    // Update is called once per frame
    void Update()
    {
        //
        // PAUSE
        //

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }

        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        pauseCam.SetActive(paused);

        coinCount.SetActive(!paused);
        healthCount.GetComponent<Image>().enabled = !paused;
        pauseText.SetActive(paused);
        tokenIndicator.GetComponent<Image>().enabled = paused;
    }

    public void CollectToken()
    {
        tokenCollected = true;

        tokenIndicator.GetComponent<Image>().sprite = tokenSprite;
    }

    public IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject.Find("Fade").GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Level2");
    }
}
