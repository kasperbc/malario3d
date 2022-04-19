using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;
    Vector3 targetPos;
    public Collider bounds;
    public Vector3 distanceFromTarget;
    public bool[] constraints = new bool[3];
    public Vector3 targetAngle;
    public bool ignoreBounds;
    [SerializeField] Image camIcon;
    [SerializeField] Sprite boundIcon;
    [SerializeField] Sprite freeIcon;
    [SerializeField] GameObject cameraControlIndicator;

    void Update()
    {
        transform.eulerAngles = Vector3.RotateTowards(transform.eulerAngles, targetAngle, 1f, 1f);

        targetPos = target.transform.position;

        Debug.DrawLine(targetPos, targetPos + distanceFromTarget);

        cameraControlIndicator.SetActive(!target.GetComponent<Renderer>().isVisible);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ignoreBounds = !ignoreBounds;
            camIcon.gameObject.GetComponent<Animator>().SetTrigger("CamSwitch");
        }

        // 
        // BOUNDS
        // 
        if (!ignoreBounds)
        {
            camIcon.sprite = boundIcon;
            BoundMovement();
        }
        else
        {
            camIcon.sprite = freeIcon;
            FreeCam();
        }
    }

    void BoundMovement()
    {
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetPos + distanceFromTarget, 0.25f);

        if (!bounds.bounds.Contains(transform.position))
        {
            transform.position = bounds.ClosestPoint(transform.position);
        }
        else if (bounds.bounds.Contains(transform.position) && bounds.bounds.Contains(nextPosition))
        {
            if (constraints[0])
            {
                nextPosition.x = transform.position.x;
            }
            if (constraints[1])
            {
                nextPosition.y = transform.position.y;
            }
            if (constraints[2])
            {
                nextPosition.z = transform.position.z;
            }

            transform.position = Vector3.MoveTowards(transform.position, nextPosition, 0.25f);
        }
    }

    void FreeCam()
    {
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetPos + distanceFromTarget, 0.5f);

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, 0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.5f);
        if (!ignoreBounds)
            Gizmos.DrawCube(bounds.bounds.center, bounds.bounds.extents * 2);
    }
}
