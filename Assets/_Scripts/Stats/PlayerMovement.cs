using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Stats _stats;

    //Cam Movement
    [SerializeField] private Transform _eyes;
    [SerializeField] private float _sensitivity;
    [Range(-90f, 0f)]
    [SerializeField] private float camLimitMin;
    [Range(0f, 90f)]
    [SerializeField] private float camLimitMax;
    private float _camAngle = 0.0f;

    //Movement
    [SerializeField] private float speed;
    [SerializeField] private Slider staminaBar;
    private Rigidbody _rb;

    //Jump
    [SerializeField] private float _jumpForce;
    [SerializeField] private KeyCode _jumpkey;

    //Interact
    [SerializeField] private KeyCode _interactkey;
    [SerializeField] float _interactRange;

    //Ability
    [SerializeField] private KeyCode _abilityKey;
    [SerializeField] private Ability _ability;

    public Rigidbody Rb
    {
        get => _rb;
        private set => _rb = value;
    }

    private void Start()
    {
        staminaBar.value = 1;
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        RotateEyes();
        RotateBody();

        if (Input.GetKeyDown(_jumpkey))
        {
            TryJump();
        }
        if (Input.GetKeyDown(_interactkey))
        {
            TryInteract();
        }
        if (Input.GetKeyDown(_abilityKey))
        {
            _ability.Use(this);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            StartCoroutine(Sprinting());
            StopCoroutine(RestoreStamina());
            if (staminaBar.value == 0)
            {
                speed = 2;
            }
            else
            {
                speed = 7;
            }
        }
        else
        {
            StopCoroutine(Sprinting());
            StartCoroutine(RestoreStamina());
            speed = 4;
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void RotateEyes()
    {
        float yMouse = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;
        _camAngle -= yMouse;
        _camAngle = Mathf.Clamp(_camAngle, camLimitMin, camLimitMax);
        _eyes.localRotation = Quaternion.Euler(_camAngle, 0, 0);
    }
    private void RotateBody()
    {
        float xMouse = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * xMouse);
    }
    private void Move()
    {
        float xDir = Input.GetAxisRaw("Horizontal");
        float zDir = Input.GetAxisRaw("Vertical");

        Vector3 dir = transform.right * xDir + transform.forward * zDir;

        _rb.velocity = new Vector3(0, _rb.velocity.y, 0) + dir.normalized * speed;
    }
    private void TryJump()
    {
        if (IsGrounded())
        {
            Jump(_jumpForce);
        }
    }
    private void Jump(float jumpForce)
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(_eyes.position, _eyes.forward, out hit, _interactRange))
        {
            IInteractable interactable = hit.collider.gameObject.gameObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, -transform.up, out hit, 1.1f);
    }

    private IEnumerator Sprinting()
    {
        staminaBar.value -= 0.004f;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator RestoreStamina()
    {
        staminaBar.value += 0.002f;
        yield return new WaitForSeconds(0.1f);
    }
}
