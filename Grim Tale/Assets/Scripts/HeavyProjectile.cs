using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyProjectile : MonoBehaviour
{
    private float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward.normalized * speed * Time.deltaTime;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
