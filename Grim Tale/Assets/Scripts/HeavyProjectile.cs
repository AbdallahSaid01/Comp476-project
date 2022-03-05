using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyProjectile : MonoBehaviour
{
    [SerializeField] private float timeToDeath = 2f;


    private float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward.normalized * speed * Time.deltaTime;

        if (timeToDeath > 0)
        {
            timeToDeath -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
