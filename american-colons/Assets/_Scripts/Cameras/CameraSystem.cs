using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;

    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float rotationSpeed = 75f;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float edgeScrollSize = 20f;
    [SerializeField] private float fieldOfViewMax = 50f;
    [SerializeField] private float fieldOfViewMin = 10f;


    [SerializeField] private bool useEdgeScrool = false;
    [SerializeField] private bool useDragPan = false;

    private float targetFieldOfView = 50f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("instance already exists");
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        MoveCamera();
        ScrollOnEdge();
        DragPan();
        RotateCamera();
        Zoom();
    }

    // if the raycast did not hit, then return a negative infinity vector3
    public Vector3 ScreenPointToRay(LayerMask mask)
    {
        Vector3 screenPointPosition = Vector3.negativeInfinity;
        Vector3 mousePosition = GameInputSystem.Instance.GetMousePosition();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
        {
            screenPointPosition = hit.point;
        }

        return screenPointPosition;
    }

    private void MoveCamera()
    {
        Vector2 moveDirNormalized = GameInputSystem.Instance.GetMovementVectorNormalized();
        Vector3 inputDir = new Vector3(moveDirNormalized.x, 0, moveDirNormalized.y);

        Vector3 moveDir = transform.forward * inputDir.z + transform.right*inputDir.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void ScrollOnEdge()
    {
        if (!useEdgeScrool)
            return;

        Vector3 inputDir = new Vector3(0, 0, 0);
        Vector2 mousePosition = GameInputSystem.Instance.GetMousePosition();

        if (mousePosition.x < edgeScrollSize) inputDir.x = -1f;
        if (mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = 1f;

        if (mousePosition.y < edgeScrollSize) inputDir.z = -1f;
        if (mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = 1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void DragPan()
    {
        if (!useDragPan)
            return;

        Vector3 inputDir = new Vector3(0, 0, 0);
        Vector2 dragPanDelta = GameInputSystem.Instance.GetMouseDragDeltaVector();

        inputDir.x = dragPanDelta.x;
        inputDir.z = dragPanDelta.y;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void RotateCamera()
    {
        float rotationDirection = GameInputSystem.Instance.GetRotationDirection();
        transform.eulerAngles += new Vector3(0, rotationDirection * rotationSpeed * Time.deltaTime, 0);
    }

    private void Zoom()
    {
        if (!CanPerformZoom())
        {
            return;
        }

        float scrollValue = GameInputSystem.Instance.GetScrollValue();
        if (scrollValue > 0)
            targetFieldOfView -= 5;
        if (scrollValue < 0)
            targetFieldOfView += 5;

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

        cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.m_Lens.FieldOfView, targetFieldOfView, zoomSpeed * Time.deltaTime);
    }

    private bool CanPerformZoom()
    {
        GameState state = GameSystem.Instance.State;

        return state == GameState.Idle ||
            ((state == GameState.Constructing || state == GameState.Destroying) && GameInputSystem.Instance.ControlIsBeingPressed);
    }
}
