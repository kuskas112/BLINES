using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class AudioPool : MonoBehaviour
{
    [SerializeField] private int poolSize = 5;
    private Queue<AudioSource> availableSources = new();
    private List<AudioSource> allSources = new();
    
    private void Awake()
    {
        // Создаем пул AudioSource
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            allSources.Add(source);
            availableSources.Enqueue(source);
        }
    }
    
    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (availableSources.Count == 0)
        {
            Debug.LogWarning("No available AudioSource in pool!");
            return;
        }
        
        AudioSource source = availableSources.Dequeue();
        
        // Настраиваем и играем
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        
        StartCoroutine(ReturnToPoolWhenFinished(source, clip.length));
    }
    
    private IEnumerator ReturnToPoolWhenFinished(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration + 0.1f);
        
        if (source.isPlaying) source.Stop();
        availableSources.Enqueue(source);
    }
}