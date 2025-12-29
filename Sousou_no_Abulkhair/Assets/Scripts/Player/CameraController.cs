using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;               

    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public float verticalRotationLimit = 80f;

    [Header("Camera Collision")]
    public Vector3 pivotLocalOffset = new Vector3(0f, 1.6f, 0f); 
    public float desiredDistance = 3f; 
    public float minDistance = 0.25f;  
    public float smoothTime = 0.06f;   
    public float safeOffset = 0.02f;
    public LayerMask collisionMask; 

    private float xRotation = 0f;
    private Vector3 _camVel;           
    private Camera _cam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!playerCamera)
            Debug.LogError("CameraController: playerCamera");

        _cam = playerCamera ? playerCamera.GetComponent<Camera>() : null;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalRotationLimit, verticalRotationLimit);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void LateUpdate()
    {
        if (!playerCamera || _cam == null) return;

        Vector3 pivotWorld = transform.TransformPoint(pivotLocalOffset);

        Vector3 idealPos = pivotWorld - playerCamera.forward * desiredDistance;

        float sphereRadius = Mathf.Max(0.1f, _cam.nearClipPlane * 0.95f);

        Vector3 toIdeal = idealPos - pivotWorld;
        float maxDist = Mathf.Max(minDistance, toIdeal.magnitude);
        Vector3 dir = (toIdeal.sqrMagnitude > 1e-6f) ? toIdeal.normalized : -playerCamera.forward;

        if (Physics.SphereCast(
                pivotWorld,
                sphereRadius,
                dir,
                out RaycastHit hit,
                maxDist,
                collisionMask,
                QueryTriggerInteraction.Ignore))
        {
            float clippedDist = Mathf.Max(minDistance, hit.distance - safeOffset);
            Vector3 clippedPos = pivotWorld + dir * clippedDist;
            playerCamera.position = Vector3.SmoothDamp(playerCamera.position, clippedPos, ref _camVel, smoothTime);
        }
        else
        {
            playerCamera.position = Vector3.SmoothDamp(playerCamera.position, idealPos, ref _camVel, smoothTime);
        }
    }
}
