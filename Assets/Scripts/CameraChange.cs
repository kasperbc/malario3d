using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public Collider nextCollider;
    public Collider prevCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collider camTargeted = other.GetComponent<PlayerMovement>().cam.GetComponent<CameraMovement>().bounds;
            CameraMovement camMov = other.GetComponent<PlayerMovement>().cam.GetComponent<CameraMovement>();

            if (camTargeted != nextCollider)
            {
                other.GetComponent<PlayerMovement>().cam.GetComponent<CameraMovement>().bounds = nextCollider;
                CameraBounds boundProp = nextCollider.GetComponent<CameraBounds>();

                camMov.constraints[0] = boundProp.constrainX;
                camMov.constraints[1] = boundProp.constrainY;
                camMov.constraints[2] = boundProp.constrainZ;

                camMov.targetAngle = boundProp.angle;
                camMov.distanceFromTarget = boundProp.position;
                camMov.gameObject.transform.position = boundProp.gameObject.GetComponent<Collider>().ClosestPoint(camMov.gameObject.transform.position);
            }
            else
            {
                other.GetComponent<PlayerMovement>().cam.GetComponent<CameraMovement>().bounds = prevCollider;
                CameraBounds boundProp = prevCollider.GetComponent<CameraBounds>();

                camMov.constraints[0] = boundProp.constrainX;
                camMov.constraints[1] = boundProp.constrainY;
                camMov.constraints[2] = boundProp.constrainZ;

                camMov.targetAngle = boundProp.angle;
                camMov.distanceFromTarget = boundProp.position;

                camMov.gameObject.transform.position = boundProp.gameObject.GetComponent<Collider>().ClosestPoint(camMov.gameObject.transform.position);
            }
        }
    }
}
