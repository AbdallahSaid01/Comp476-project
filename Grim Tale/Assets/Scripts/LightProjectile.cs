using UnityEngine;

public class LightProjectile : MonoBehaviour
{
    [SerializeField] private float timeToDeath = 2f;
    [SerializeField] private ParticleSystem particleSystem;

    private float speed;

    private void Update()
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
        if (collider.gameObject.CompareTag("Enemy"))
        {
            var positionVector = collider.transform.position;
            Instantiate(particleSystem, positionVector, transform.rotation);
            Destroy(gameObject);
        }
        else if (collider.gameObject.CompareTag("Level"))
        {
            Destroy(gameObject);
        }
    }
}
