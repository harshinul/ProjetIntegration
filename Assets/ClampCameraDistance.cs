using UnityEngine;
using Unity.Cinemachine;

public class ClampCameraDistance : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float minDistance = 8f;

    void LateUpdate()
    {
        if (vcam == null || vcam.Follow == null) return;

        Transform cam = vcam.VirtualCameraGameObject.transform;
        Transform target = vcam.Follow;

        Vector3 dir = (cam.position - target.position).normalized;
        float dist = Vector3.Distance(cam.position, target.position);

        if (dist < minDistance)
        {
            cam.position = target.position + dir * minDistance;
        }
    }
}
