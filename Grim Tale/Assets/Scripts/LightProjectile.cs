using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightProjectile : MonoBehaviour
{
    [SerializeField] private float timeToDeath = 2f;
    [SerializeField] private ParticleSystem particleSystem;

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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Enemy")
        {
            Vector3 positionVector = collider.transform.position;
            Instantiate(particleSystem, positionVector, transform.rotation);
            Destroy(gameObject);
        }
        else if (collider.transform.tag == "Level")
        {
            Destroy(gameObject);
        }
    }
}
