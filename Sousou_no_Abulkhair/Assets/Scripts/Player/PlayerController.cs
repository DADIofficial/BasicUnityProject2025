using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    [Header("Collision/Slide")]
    [SerializeField] private float _wallStopDistance = 0.06f;
    [SerializeField] private LayerMask _collisionMask;
    [SerializeField] private int _maxSlideIterations = 2;

    [Header("SFX")]
    [SerializeField] private SFXManager _sfxManager; 
    [SerializeField] private float stepDistance = 1.6f; 
    [SerializeField] private float minMoveSpeedForSteps = 0.05f; 

    [Header("Ground check")]
    [SerializeField] private float groundCheckDistance = 0.2f;

    private PlayerInputController _playerInputController;
    private CapsuleCollider _capsule;

    private Vector3 _lastStepPos;
    private bool _wasMoving;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _capsule = GetComponent<CapsuleCollider>();
        if (!_capsule)
            Debug.LogWarning("PlayerController: ����� CapsuleCollider ��� ������ �� ������.");

        if (_sfxManager == null)
            _sfxManager = FindFirstObjectByType<SFXManager>(FindObjectsInactive.Include);
    }

    private void Start()
    {
        _lastStepPos = transform.position;
    }

    private void Update()
    {
        Vector2 input = _playerInputController.MovementInputVector;
        Vector3 moveLocal = new Vector3(input.x, 0f, input.y);

        Vector3 wishMove = transform.TransformDirection(moveLocal);

        
        if (wishMove.sqrMagnitude <= 1e-4f)
        {
            _wasMoving = false;
            _lastStepPos = transform.position; 
            return;
        }

        wishMove = wishMove.normalized * _speed * Time.deltaTime;

        Vector3 finalDelta = ComputeSlideDelta(transform.position, wishMove);
        transform.position += finalDelta;

        HandleFootsteps(finalDelta);
    }

    private void HandleFootsteps(Vector3 finalDelta)
    {
        if (_sfxManager == null) return;

        
        float speedNow = finalDelta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        if (speedNow < minMoveSpeedForSteps)
        {
            _wasMoving = false;
            _lastStepPos = transform.position;
            return;
        }

        
        if (!IsGrounded())
        {
            _wasMoving = false;
            _lastStepPos = transform.position;
            return;
        }

        if (!_wasMoving)
        {
            _wasMoving = true;
            _lastStepPos = transform.position;
        }

        float dist = Vector3.Distance(transform.position, _lastStepPos);
        if (dist >= stepDistance)
        {
            _sfxManager.PlaySFX(SFXType.Footstep);
            _lastStepPos = transform.position;
        }
    }

    private bool IsGrounded()
    {
        
        if (_capsule == null) return true;

        Vector3 origin = transform.position + transform.TransformVector(_capsule.center);
        float radius = _capsule.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);

        
        return Physics.SphereCast(origin, radius * 0.9f, Vector3.down, out _,
            groundCheckDistance, _collisionMask, QueryTriggerInteraction.Ignore);
    }

    private Vector3 ComputeSlideDelta(Vector3 startPos, Vector3 desiredDelta)
    {
        if (!_capsule) return desiredDelta;

        Vector3 remainingDir = desiredDelta.normalized;
        float remaining = desiredDelta.magnitude;
        Vector3 pos = startPos;

        for (int i = 0; i < _maxSlideIterations && remaining > 1e-5f; i++)
        {
            if (CapsuleCast(pos, remainingDir, remaining + _wallStopDistance, out RaycastHit hit))
            {
                float travel = Mathf.Max(0f, hit.distance - _wallStopDistance);
                pos += remainingDir * travel;
                remaining -= travel;

                Vector3 slideDir = Vector3.ProjectOnPlane(remainingDir, hit.normal);
                if (slideDir.sqrMagnitude < 1e-6f) break;
                remainingDir = slideDir.normalized;
            }
            else
            {
                pos += remainingDir * remaining;
                remaining = 0f;
                break;
            }
        }

        return pos - startPos;
    }

    private bool CapsuleCast(Vector3 atPos, Vector3 dir, float distance, out RaycastHit hit)
    {
        GetCapsuleWorld(atPos, out Vector3 p1, out Vector3 p2, out float radius);
        return Physics.CapsuleCast(p1, p2, radius, dir, out hit, distance, _collisionMask, QueryTriggerInteraction.Ignore);
    }

    private void GetCapsuleWorld(Vector3 atPos, out Vector3 p1, out Vector3 p2, out float radius)
    {
        radius = _capsule.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        float height = Mathf.Max(_capsule.height * transform.lossyScale.y, radius * 2f);

        Vector3 up = transform.up;
        Vector3 worldCenter = atPos + transform.TransformVector(_capsule.center);

        float half = Mathf.Max(0f, height * 0.5f - radius);
        p1 = worldCenter + up * half;
        p2 = worldCenter - up * half;
    }
}
