using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    private PlayerInput _playerInput;

    private Vector2 _direction;
    private Vector2 _rotate;

    private Vector2 _rotation;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Player.Move.performed += ctx => OnMove();
        _playerInput.Player.Look.performed += ctx => OnLook();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        _rotate = _playerInput.Player.Look.ReadValue<Vector2>();
        _direction = _playerInput.Player.Move.ReadValue<Vector2>();

        Look(_rotate);
        Move(_direction);
    }

    private void OnMove()
    {
        _direction = _playerInput.Player.Move.ReadValue<Vector2>();
    }

    private void OnLook()
    {
        _rotate = _playerInput.Player.Look.ReadValue<Vector2>();
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.1f)
        {
            return;
        }

        float scaledMoveSpeed = _moveSpeed * Time.deltaTime;

        Vector3 move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);

        transform.position += move * scaledMoveSpeed;
    }

    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.1f)
        {
            return;
        }

        float scaledRotateSpeed = _rotateSpeed * Time.deltaTime;

        _rotation.y += rotate.x * scaledRotateSpeed;
        _rotation.x = Mathf.Clamp(_rotation.x - rotate.y * scaledRotateSpeed, -90, 90);
        transform.localEulerAngles = _rotation;
    }
}
