using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _takeDistance;
    [SerializeField] private float _holdDistance;
    [SerializeField] private float _throwForce;

    private PlayerInput _playerInput;

    private Vector2 _direction;
    private Vector2 _rotate;

    private Vector2 _rotation;

    private GameObject _currentObjct;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Player.Move.performed += ctx => OnMove();
        _playerInput.Player.Look.performed += ctx => OnLook();
        _playerInput.Player.PickUp.performed += ctx => TryPickUp();
        _playerInput.Player.Drop.performed += ctx => Throw(true);
        _playerInput.Player.Throw.performed += ctx => Throw();
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

    private void TryPickUp()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, _takeDistance) && !hitInfo.collider.gameObject.isStatic)
        {
            _currentObjct = hitInfo.collider.gameObject;

            _currentObjct.transform.position = default;
            _currentObjct.transform.SetParent(transform, false);
            _currentObjct.transform.localPosition += new Vector3(0, 0, _holdDistance);

            _currentObjct.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void Throw(bool drop = false)
    {
        _currentObjct.transform.SetParent(null);

        Rigidbody rigidbody = _currentObjct.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;

        if (!drop)
        {
            rigidbody.AddForce(transform.forward * _throwForce, ForceMode.Impulse);
        }
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
