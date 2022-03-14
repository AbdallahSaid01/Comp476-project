using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaScript : MonoBehaviour
{
    private int value;

    void Start()
    {
        value = Random.Range(1, 10);
    }

    public void setValue(int val)
    {
        value = val;
    }

    private void increment()
    {
        PlayerController.playerMana += value;
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
