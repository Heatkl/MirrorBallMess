using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Tooltip("Cannon controller from Player")]
    [SerializeField] CannonController controller;

    private BallInputControl input; //Созданные Input Actions
    private PlayerCursorController playerCursorController; //Контроллер для показа/скрытия курсора через Esc

    private void Awake()
    {
        input = new BallInputControl();
        playerCursorController = new PlayerCursorController();

    }

    private void Start()
    {
        input.BallMap.MouseMove.performed += RotateCannon;
        input.BallMap.MouseShoot.performed += Shoot;
        input.BallMap.CursorLock.performed += ChangeCursorState;
    }

    private void Update()
    {
        if (input.BallMap.UnstoppableShoot.IsPressed())
        {
            Shoot(default);
        }
    }


    /// <summary>
    /// Вызов метода по событию нажатия на Esc
    /// </summary>
    /// <param name="context">Контекст InputActions</param>
    private void ChangeCursorState(InputAction.CallbackContext context)
    {
        playerCursorController.ShowHideCursor();
    }

    /// <summary>
    /// Вызов метода по событию перемещения мыши
    /// </summary>
    /// <param name="context">Контекст InputActions</param>
    private void RotateCannon(InputAction.CallbackContext context)
    {
        controller.RotateCannon(context.ReadValue<Vector2>());
    }

    /// <summary>
    /// Вызов метода по событию нажатия на клавиши
    /// </summary>
    /// <param name="context">Контекст InputActions</param>
    private void Shoot(InputAction.CallbackContext context)
    {
        controller.SpawnBall();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.BallMap.MouseMove.performed -= RotateCannon;
        input.BallMap.MouseShoot.performed -= Shoot;
        input.Disable();
    }
}


