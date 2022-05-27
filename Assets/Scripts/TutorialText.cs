using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    TextMeshPro textColor;
    [SerializeField] float appearDistance;
    void Start()
    {
        textColor = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    void ChangeColor()
    {
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

        float alpha = 1 - distanceToClosestPlayer / appearDistance;
        alpha = Mathf.Clamp(alpha, 0, 1);

        textColor.color = new Color(textColor.color.r, textColor.color.g, textColor.color.b, alpha);
    }
}
