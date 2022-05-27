using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField] string[] beginStrings;
    void Start()
    {
        RandomBeginText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            StartGame();
        }
    }

    void RandomBeginText()
    {
        TextMeshProUGUI beginText = GameObject.Find("PressKey").GetComponent<TextMeshProUGUI>();

        beginText.text = "Press " + beginStrings[Random.Range(0, beginStrings.Length)] + " to begin";
    }

    void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
