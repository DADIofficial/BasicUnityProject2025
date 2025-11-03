using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        // Ввод с клавиатуры (или геймпада)
        Vector2 input = _playerInputController.MovementInputVector;

        // Создаем вектор движения в локальных координатах
        Vector3 move = new Vector3(input.x, 0f, input.y);

        // Преобразуем локальный вектор в мировой — с учётом поворота игрока
        Vector3 moveRelativeToRotation = transform.TransformDirection(move);

        // Нормализуем (чтобы диагональ не была быстрее) и умножаем на скорость
        transform.position += moveRelativeToRotation.normalized * _speed * Time.deltaTime;
    }
}
