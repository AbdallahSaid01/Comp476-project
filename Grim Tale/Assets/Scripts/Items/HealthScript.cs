using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    private int value;

    void Start()
    {
        value = Random.Range(1, 5);
    }

    public void setValue(int val)
    {
        value = val;
    }

    private void increment()
    {
        PlayerController.playerHealth += value;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            increment();
            Destroy(gameObject.transform.parent.parent.gameObject);
        }
    }

}
