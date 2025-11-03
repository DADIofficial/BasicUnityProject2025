using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [Header("Collision/Slide")]
    [SerializeField] private float _wallStopDistance = 0.06f;   // зазор до стены
    [SerializeField] private LayerMask _collisionMask;          // стены/уровень (»— Ћё„»“№ слой Player)
    [SerializeField] private int _maxSlideIterations = 2;       // достаточно 1Ц3

    private PlayerInputController _playerInputController;
    private CapsuleCollider _capsule;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _capsule = GetComponent<CapsuleCollider>();
        if (!_capsule)
            Debug.LogWarning("PlayerController: нужен CapsuleCollider дл€ слайда по стенам.");
    }

    private void Update()
    {
        // 1) ¬вод
        Vector2 input = _playerInputController.MovementInputVector;
        Vector3 moveLocal = new Vector3(input.x, 0f, input.y);

        // 2) ¬ектор в ћ»–≈, нормализуем и масштабируем скоростью
        Vector3 wishMove = transform.TransformDirection(moveLocal);
        if (wishMove.sqrMagnitude > 1e-4f)
            wishMove = wishMove.normalized * _speed * Time.deltaTime;
        else
            return;

        // 3) "”мное" перемещение: подходим к стене и скользим по ней
        Vector3 finalDelta = ComputeSlideDelta(transform.position, wishMove);
        transform.position += finalDelta;
    }

    // ---- helpers ----
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
                // подходим вплотную с безопасным зазором
                float travel = Mathf.Max(0f, hit.distance - _wallStopDistance);
                pos += remainingDir * travel;
                remaining -= travel;

                // нова€ проекци€ по касательной (скольжение)
                Vector3 slideDir = Vector3.ProjectOnPlane(remainingDir, hit.normal);
                if (slideDir.sqrMagnitude < 1e-6f) break; // почти перпендикул€р Ч стоим
                remainingDir = slideDir.normalized;
            }
            else
            {
                // путь чист Ч идЄм до конца
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
        // учитываем скейл и оффсет центра коллайдера
        radius = _capsule.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        float height = Mathf.Max(_capsule.height * transform.lossyScale.y, radius * 2f);

        Vector3 up = transform.up;
        Vector3 worldCenter = atPos + transform.TransformVector(_capsule.center);

        float half = Mathf.Max(0f, height * 0.5f - radius);
        p1 = worldCenter + up * half;
        p2 = worldCenter - up * half;
    }
}
