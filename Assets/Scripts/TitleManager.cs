using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
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
        
    }

    void RandomBeginText()
    {
        TextMeshProUGUI beginText = GameObject.Find("PressKey").GetComponent<TextMeshProUGUI>();

        beginText.text = "Press " + beginStrings[Random.Range(0, beginStrings.Length)] + " to begin";
    }
}
