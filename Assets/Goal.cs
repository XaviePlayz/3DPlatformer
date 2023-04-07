using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Goal : MonoBehaviour
{
    public int totalJesterCount;
    public int jestersKilled;
    public TextMeshProUGUI counter;
    public GameObject Victory;

    public Player player;

    void Start()
    {
        var foundObjects = Object.FindObjectsOfType<EnemyAI>();
        int count = foundObjects.Length;

        totalJesterCount = count;
        counter.text = jestersKilled.ToString() + "/" + totalJesterCount.ToString();
    }

    void Update()
    {
        if (jestersKilled == totalJesterCount)
        {
            Victory.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Player>().enabled = false;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
