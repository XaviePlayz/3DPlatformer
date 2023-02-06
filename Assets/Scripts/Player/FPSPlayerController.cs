using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FPSPlayerController : BaseGameObject
{
    public static FPSPlayerController FPSPlayerInstance
    {
        get
        {
            return _instance;
        }
    }

    public static event Action<int> LifeUpdate;

    public CharacterController characterController;

    public ShootController shootController;

    public float movementSpeed;

    public float gravity;

    public int life;

    public AudioSource audioSource;

    public AudioClip damageClip;

    private static FPSPlayerController _instance;


    void Awake()
    {
        _instance = this;
        if (LifeUpdate != null)
        {
            LifeUpdate(life);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused && !gameEnded)
        {
            float forward = Input.GetAxis("Vertical");
            float sidestep = Input.GetAxis("Horizontal");
            Vector3 movement = forward * FPSCamera.CameraForwardVectorOnGround +
                                sidestep * FPSCamera.CameraRightVectorOnGround;

            transform.forward = FPSCamera.CameraForwardVectorOnGround;

            if (Input.GetMouseButton(0))
            {
                shootController.Shoot();
            }

            characterController.Move(Time.deltaTime * (movementSpeed * movement - gravity * Vector3.up));
        }
    }

    public void Damage(int damage)
    {
        if (!gameEnded && !gamePaused)
        {
            life = Mathf.Clamp(life - damage, 0, life);
            audioSource.PlayOneShot(damageClip);
            if (LifeUpdate != null)
            {
                LifeUpdate(life);
            }
            if (life <= 0)
            {
                GameManager.EndGame(false);
            }
        }
    }

    public void AddHealth(int amount)
    {
        if (!gameEnded && !gamePaused)
        {
            life += amount;
            if (LifeUpdate != null)
            {
                LifeUpdate(life);
            }
        }
    }
}
