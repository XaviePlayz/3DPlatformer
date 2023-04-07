using UnityEngine;
using UnityEngine.UI;
public class HealthBarBehaviour : MonoBehaviour
{
    public Camera cameraToFollow;
    public Slider Slider;
    public Color Low;
    public Color High;
    public Vector3 Offset;

    public void SetEnemyHealth(float health, float maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;

        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cameraToFollow.transform.position);
    }
}
