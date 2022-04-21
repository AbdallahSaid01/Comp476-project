using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip lightCast;
    [SerializeField] private AudioClip heavyCast;
    [SerializeField] private AudioClip projectileDestroy;
    [SerializeField] private AudioClip enemyHitByLightAttack;
    [SerializeField] private AudioClip enemyHitByHeavyAttack;
    [SerializeField] private AudioClip enemyKilled;
    [SerializeField] private AudioClip goldInteract;
    [SerializeField] private AudioClip playerHit;
    
    private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource ??= GetComponent<AudioSource>();
    }

    public void PlayOneShot(Clip clip)
    {
        audioSource.pitch = Random.Range(0.75f, 1.25f);
        audioSource.PlayOneShot(GetClip(clip));
    }

    private AudioClip GetClip(Clip clip)
    {
        switch (clip)
        {
            case Clip.LightCast:
                return lightCast;
            case Clip.HeavyCast:
                return heavyCast;
            case Clip.ProjectileDestroy:
                return projectileDestroy;
            case Clip.EnemyHitByLightAttack:
                return enemyHitByLightAttack;
            case Clip.EnemyHitByHeavyAttack:
                return enemyHitByHeavyAttack;
            case Clip.EnemyKilled:
                return enemyKilled;
            case Clip.GoldInteract:
                return goldInteract;
            case Clip.PlayerHit:
                return playerHit;
            default:
                return lightCast;
        }
    }
}

public enum Clip { LightCast, HeavyCast, ProjectileDestroy, EnemyHitByLightAttack, EnemyHitByHeavyAttack, EnemyKilled, GoldInteract, PlayerHit }
