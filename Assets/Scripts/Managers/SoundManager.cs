using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioPool audioPool;
    [SerializeField]
    private AudioClip bounceClip;

    private void Awake()
    {
        audioPool = GetComponent<AudioPool>();
    }

    private void PlayBounceSound(string key, float duration)
    {
        audioPool.PlaySound(bounceClip);
    }
}
