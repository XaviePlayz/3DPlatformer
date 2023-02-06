using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Cam Movement
    [SerializeField] private Transform _eyes;
    [SerializeField] private float _sensitivity;
    [Range(-90f, 0f)]
    [SerializeField] private float _camLimitMin;
    [Range(0f, 90f)]
    [SerializeField] private float _camLimitMax;
    private float _camAngle = 0.0f;

    //Movement
    [SerializeField] private float _speed;
    private Rigidbody _rb;

    private void Start()
    {
        _rb.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RotateEyes();
        RotateBody();
    }

    private void FixedUpdate()
    {
        float xDir = Input.GetAxis("Horizontally");
        float zDir = Input.GetAxis("Vertical");

        Vector3 dir = transform.right * xDir + transform.forward * zDir;

        _rb.velocity = new Vector3(0, _rb.velocity.y, 0) + dir.normalized * _speed;
    }

    private void RotateEyes()
    {
        float yMouse = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;
        _camAngle -= yMouse;
        _camAngle = Mathf.Clamp(_camAngle, _camLimitMin, _camLimitMax);
        _eyes.localRotation = Quaternion.Euler(_camAngle, 0, 0);
    }

    private void RotateBody()
    {

    }
}
