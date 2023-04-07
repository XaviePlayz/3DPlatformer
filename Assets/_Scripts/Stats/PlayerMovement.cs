using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Stats _stats;

    [Header("Cam Movement")]
    [SerializeField] private Transform _eyes;
    [SerializeField] private float _sensitivity;
    [Range(-90f, 0f)]
    [SerializeField] private float camLimitMin;
    [Range(0f, 90f)]
    [SerializeField] private float camLimitMax;
    private float _camAngle = 0.0f;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private Slider staminaBar;
    private Rigidbody _rb;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private KeyCode _jumpkey;

    [Header("Interact")]
    [SerializeField] private KeyCode _interactkey;
    [SerializeField] float _interactRange;

    [Header("Abilities")]
    [SerializeField] private KeyCode _abilityKey;
    [SerializeField] private Ability _ability;
    [SerializeField] private Ability _fireAbility;
    [SerializeField] private Ability _iceAbility;
    [SerializeField] private Ability _acidAbility;

    [SerializeField] public bool usingIce = false;
    [SerializeField] public bool usingAcid = false;

    [Header("Canvas")]
    [SerializeField] private Image fireFrame;
    [SerializeField] private Image iceFrame;
    [SerializeField] private Image acidFrame;

    [SerializeField] private Sprite unactiveFrame;
    [SerializeField] private Sprite activefireFrame;
    [SerializeField] private Sprite activeiceFrame;
    [SerializeField] private Sprite activeacidFrame;

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _ability = _fireAbility;
            fireFrame.sprite = activefireFrame;
            iceFrame.sprite = unactiveFrame;
            acidFrame.sprite = unactiveFrame;
            usingIce = false;
            usingAcid = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _ability = _iceAbility;
            fireFrame.sprite = unactiveFrame;
            iceFrame.sprite = activeiceFrame;
            acidFrame.sprite = unactiveFrame;
            usingIce = true;
            usingAcid = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _ability = _acidAbility;
            fireFrame.sprite = unactiveFrame;
            iceFrame.sprite = unactiveFrame;
            acidFrame.sprite = activeacidFrame;
            usingIce = false;
            usingAcid = true;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(Sprinting());
                StopCoroutine(RestoreStaminaWhileRunning());
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
                StartCoroutine(RestoreStaminaWhileRunning());
                speed = 4;
            }
        }
        else
        {
            StopCoroutine(Sprinting());
            StartCoroutine(RestoreStaminaWhileRunning());
            speed = 4;
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            speed = 4;
            StopCoroutine(Sprinting());
            StartCoroutine(RestoreStamina());
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

    private IEnumerator RestoreStaminaWhileRunning()
    {
        staminaBar.value += 0.0004f;
        yield return new WaitForSeconds(0.1f);
    }
}
