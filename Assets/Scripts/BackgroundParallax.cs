using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform background;
    [SerializeField][Range(0.1f, 10f)] float speed;

    Vector2 cameraUnitSize, particleSystemSize;
    Vector2 lastPlayerPosition, currentPlayerPosition;
    Camera mainCamera;


    void Start()
    {
        mainCamera = Camera.main;
        cameraUnitSize.y = mainCamera.orthographicSize * 2;
        cameraUnitSize.x = cameraUnitSize.y * mainCamera.aspect;
        particleSystemSize = cameraUnitSize * 2.5f;
        

        lastPlayerPosition = player.position;
    }

    private void Update()
    {
        Scroll();
    }

    private void Scroll()
    {
        if (!player) return;
        currentPlayerPosition = player.position;
        Vector2 movementThisFrame = currentPlayerPosition - lastPlayerPosition;
        movementThisFrame *= 1 / (speed - 11);
        background.Rotate(Vector3.right * movementThisFrame.y, Space.World);
        background.Rotate(Vector3.down * movementThisFrame.x, Space.World);
        lastPlayerPosition = currentPlayerPosition;
    }
}
