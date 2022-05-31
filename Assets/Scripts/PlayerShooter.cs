using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Player.Shoot.performed += ctx => OnShoot();
        _playerInput.Player.Move.performed += ctx => OnMove();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    public void OnShoot()
    {
        Debug.Log("Shoot");
    }

    public void OnMove()
    {
        Vector2 moveDirDirection = _playerInput.Player.Move.ReadValue<Vector2>();

        Debug.Log(moveDirDirection);
    }
}
