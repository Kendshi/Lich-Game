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
        axisController.Controllers.ForEach(control => control.Enabled = false);
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

        if (m_Rotate.IsPressed())
        {
            axisController.Controllers[0].Enabled = true;
        }
        else
        { 
            axisController.Controllers[0].Enabled = false;
        }
    }

    private void HandleMovement()
    {
        if (playerCamera == null) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

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
            if (useSmoothMovement)
            {
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.deltaTime * (1f / smoothTime));
            }
            else
            {
                currentVelocity = Vector3.zero;
            }
        }

        // Применяем движение только если есть скорость
        if (currentVelocity != Vector3.zero)
        {
            cachedTransform.Translate(currentVelocity * Time.deltaTime, Space.World);
        }
    }

    void OnDisable()
    {
        inputActions.FindActionMap("CameraControl").Disable();
    }
}
