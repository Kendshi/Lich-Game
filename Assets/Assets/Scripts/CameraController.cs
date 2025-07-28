using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 5f;           // Скорость движения
    [SerializeField] private bool useSmoothMovement = true;  // Плавное движение
    [SerializeField] private float smoothTime = 0.1f;        // Время сглаживания

    [Header("Ссылки")]
    [SerializeField] private Transform playerCamera;         // Ссылка на камеру
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private CinemachineInputAxisController axisController;

    private InputAction m_Rotate;
    private InputAction m_Move;

    // Переменные для плавного движения
    private Vector3 currentVelocity;

    // Кэшированные компоненты
    private Transform cachedTransform;

    void OnEnable()
    {
        inputActions.FindActionMap("CameraControl").Enable();
    }

    void Awake()
    {
        m_Rotate = InputSystem.actions.FindAction("Rotate");
        m_Move = InputSystem.actions.FindAction("Move");
        axisController.Controllers[1].Enabled = true;
    }

    void Start()
    {
        // Получаем компоненты
        cachedTransform = transform;

        // Если камера не назначена, ищем основную камеру
        if (playerCamera == null)
        {
            playerCamera = Camera.main?.transform;
        }

        // Проверка наличия камеры
        if (playerCamera == null)
        {
            Debug.LogError("Камера не найдена! Назначьте камеру в инспекторе или убедитесь, что на сцене есть MainCamera");
        }
    }

    void Update()
    {
        HandleMovement();

        axisController.Controllers[0].Enabled = m_Rotate.IsPressed();
    }

    private void HandleMovement()
    {
        if (playerCamera == null) return;

        float horizontalInput = m_Move.ReadValue<Vector2>().x;
        float verticalInput = m_Move.ReadValue<Vector2>().y;

        // Если есть ввод
        if (Mathf.Abs(horizontalInput) > 0.3f || Mathf.Abs(verticalInput) > 0.3f)
        {
            Vector3 forward = playerCamera.forward;
            Vector3 right = playerCamera.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
            moveDirection.Normalize();

            if (useSmoothMovement)
            {
                Vector3 targetVelocity = moveDirection * moveSpeed;
                currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref currentVelocity, smoothTime);
            }
            else
            {
                currentVelocity = moveDirection * moveSpeed;
            }
        }
        else
        {
            // Быстрая остановка при отсутствии ввода
            currentVelocity = useSmoothMovement ? Vector3.Lerp(currentVelocity, Vector3.zero, Time.deltaTime * (1f / smoothTime)) : Vector3.zero;
        }

        // Применяем движение только если есть скорость
        if (currentVelocity != Vector3.zero)
            cachedTransform.Translate(currentVelocity * Time.deltaTime, Space.World);

    }

    void OnDisable()
    {
        inputActions.FindActionMap("CameraControl").Disable();
    }
}
