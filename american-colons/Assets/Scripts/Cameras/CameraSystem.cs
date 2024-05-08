using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{

    [SerializeField] private GameInputSystem gameInputSystem;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float edgeScrollSize = 20f;
    [SerializeField] private bool useEdgeScrool = false;

    private void FixedUpdate()
    {
        MoveCamera();
        RotateCamera();
    }

    private void MoveCamera()
    {
        Vector2 moveDirNormalized = gameInputSystem.GetMovementVectorNormalized();
        Vector3 inputDir = new Vector3(moveDirNormalized.x, 0, moveDirNormalized.y);

        inputDir = ScrollOnEdge(inputDir);

        Vector3 moveDir = transform.forward * inputDir.z + transform.right;
        transform.position += inputDir * moveSpeed * Time.deltaTime;
    }

    private void RotateCamera()
    {
        float rotationDirection = gameInputSystem.GetRotationDirection();
        transform.eulerAngles += new Vector3(0, rotationDirection*rotationSpeed, 0);
    }

    private Vector3 ScrollOnEdge(Vector3 inputDir)
    {
        if (!useEdgeScrool)
            return inputDir;


        Vector2 mousePosition = gameInputSystem.GetMousePosition();

        if (mousePosition.x < edgeScrollSize) inputDir.x = -1f;
        if (mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = 1f;

        if (mousePosition.y < edgeScrollSize) inputDir.z = -1f;
        if (mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = 1f;

        return inputDir;
    }
}
