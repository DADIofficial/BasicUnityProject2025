using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private bool lockX = false;

    private void Awake()
    {
        if (!targetCamera) targetCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (!targetCamera) return;

        Vector3 dir = transform.position - targetCamera.transform.position;

        if (lockX)
            dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) return;

        transform.rotation = Quaternion.LookRotation(dir);
    }
}

