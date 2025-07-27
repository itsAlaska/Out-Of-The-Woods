using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private int pixelsPerUnit = 32;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z;

        float unitsPerPixel = 1f / pixelsPerUnit;
        targetPosition.x = Mathf.Round(targetPosition.x / unitsPerPixel) * unitsPerPixel;
        targetPosition.y = Mathf.Round(targetPosition.y / unitsPerPixel) * unitsPerPixel;

        transform.position = targetPosition;
    }
}