using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] public Camera playerCam;

    [SerializeField] private LayerMask pickableLayerMask;

    [SerializeField] private Transform playerCameraTransform;

    [SerializeField] private GameObject pickUpUI;

    internal void AddHealth(int healthBoost)
    {
        Debug.Log($"Health boosted by {healthBoost}");
    }

    [SerializeField] [Min(1)] private float hitRange = 3;

    [SerializeField] private Transform pickUpParent;

    [SerializeField] private GameObject inHandItem;

    [SerializeField] private InputActionReference interactionInput, dropInput, useInput;

    private RaycastHit hit;
    [SerializeField] public GameObject _bulletSpawn;

    [SerializeField] private AudioSource pickUpSource;

    [Header("Zoom Parameters")]
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 15f;
    private float defaultFOV;
    private Coroutine zoomRoutine;

    private bool alreadyCrouching;

    void Awake()
    {
        defaultFOV = playerCam.fieldOfView;
    }
    void Start()
    {
        Instance = this;
        interactionInput.action.performed += PickUp;
        dropInput.action.performed += Drop;
        useInput.action.performed += Use;
    }

    private void Use(InputAction.CallbackContext obj)
    {
        if (inHandItem != null)
        {
            IUsable usable = inHandItem.GetComponent<IUsable>();
            if (usable != null)
            {
                usable.Use(this.gameObject);
            }
        }
    }

    private void Drop(InputAction.CallbackContext obj)
    {
        inHandItem.GetComponent<BoxCollider>().enabled = true;
        inHandItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        inHandItem.GetComponent<Transform>().transform.rotation = Quaternion.Euler(0, 0, 0);

        if (inHandItem != null)
        {
            inHandItem.transform.SetParent(null);
            inHandItem = null;
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }

    private void PickUp(InputAction.CallbackContext obj)
    {
        if(hit.collider != null && inHandItem == null)
        {
            IPickable pickableItem = hit.collider.GetComponent<IPickable>();
            if (pickableItem != null)
            {
                pickUpSource.Play();
                inHandItem = pickableItem.PickUp();
                inHandItem.transform.SetParent(pickUpParent.transform, pickableItem.KeepWorldPosition);
                inHandItem.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
    private void Open(InputAction.CallbackContext obj)
    {
        if (hit.collider != null && inHandItem == null)
        {
            IPickable pickableItem = hit.collider.GetComponent<IPickable>();
            if (pickableItem != null)
            {
                pickUpSource.Play();
                inHandItem = pickableItem.PickUp();
                inHandItem.transform.SetParent(pickUpParent.transform, pickableItem.KeepWorldPosition);
                inHandItem.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            Crosshair.Instance.ZoomIn();
            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            Crosshair.Instance.ZoomOut();
            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!alreadyCrouching)
            {
                //playerCam.transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
            }
            alreadyCrouching = true;
        }
        else
        {
            //playerCam.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            alreadyCrouching = false;
        }


        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
        }

        if (inHandItem != null)
        {
            return;
        }

        if (Physics.Raycast(
            playerCameraTransform.position, 
            playerCameraTransform.forward, 
            out hit, 
            hitRange, 
            pickableLayerMask))
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
            pickUpUI.SetActive(true);
        }
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCam.fieldOfView;
        float timeElapsed = 0;

        while(timeElapsed < timeToZoom)
        {
            playerCam.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCam.fieldOfView = targetFOV;
        zoomRoutine = null;
    }
}
