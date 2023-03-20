using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public static Crosshair Instance;
    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite zoomOut;
    [SerializeField] private Sprite zoomIn;

    public void Start()
    {
        Instance = this;
    }
    public void ZoomIn()
    {
        crosshair.sprite = zoomIn;
        crosshair.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
    public void ZoomOut()
    {
        crosshair.sprite = zoomOut;
        crosshair.transform.localScale = new Vector3(1, 1, 1);
    }
}
