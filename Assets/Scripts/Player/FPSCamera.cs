using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : BaseGameObject
{

    public static Vector3 CameraForwardVectorOnGround
    {
        get
        {
            Vector3 forwardOnGround = _instance.transform.forward;
            forwardOnGround.y = 0f;
            return forwardOnGround.normalized;
        }
    }

    public static Vector3 CameraRightVectorOnGround
    {
        get
        {
            Vector3 rightOnGround = _instance.transform.right;
            rightOnGround.y = 0f;
            return rightOnGround.normalized;
        }
    }

    public static Vector3 CameraPosition
    {
        get {
            return _instance.transform.position;
        }
    }

    public FPSPlayerController playerController;

    public Vector3 playerPositionOffset;

    public float sensitivityX = 10f;

    public float sensitivityY = 10f;

    public float maxAngleY = 70f;

    public float minAngleY = 70f;

    public bool invertAxisY = false;

    private float _currentX;

    private float _currentY;

    private static FPSCamera _instance;


    void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused && !gameEnded)
        {
            float horizontal = Input.GetAxis("Mouse X"); // deltas
            float vertical = Input.GetAxis("Mouse Y");

            _currentX += sensitivityX * horizontal * Time.deltaTime;
            _currentY += sensitivityY * vertical * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        _currentY = Mathf.Clamp(_currentY, minAngleY, maxAngleY);
        this.transform.rotation = Quaternion.Euler(
                                        (invertAxisY ? -1 : 1) * _currentY,
                                        _currentX,
                                        0f);
        transform.position = playerController.transform.position + playerPositionOffset;
    }
}
