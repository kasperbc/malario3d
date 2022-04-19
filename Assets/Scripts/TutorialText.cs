using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    TextMesh textColor;
    [SerializeField] float appearDistance;
    void Start()
    {
        textColor = GetComponent<TextMesh>();
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
