using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraBounds : MonoBehaviour
{
    public bool constrainX, constrainY, constrainZ;
    public Vector3 angle;
    public Vector3 position;

    void Awake()
    {
        if (angle.y == 0)
        {
            angle.y = 90;
        }
        if (angle.x == 0)
        {
            angle.x = 20;
        }

        if (position.x == 0)
            position.x = -10;
        if (position.y == 0)
            position.y = 6;
    }

    public void OnDrawGizmos()
    {
        Collider coll = GetComponent<Collider>();

        Gizmos.color = new Color(0.411f, 0, 1, 0.15f);
        Gizmos.DrawCube(coll.bounds.center, coll.bounds.extents * 2);
    }
}
