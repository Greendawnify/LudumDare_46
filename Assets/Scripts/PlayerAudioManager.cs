using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] Transform playerRef;

    [Header("Audio Clips")]
    [SerializeField] AudioClip[] steps;
    [SerializeField] AudioClip sniperBullet;
    [SerializeField] AudioClip zoom;
   
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;

    [Header("Differenent Sources")]
    [SerializeField] GameObject sourcePrefab;
    [SerializeField] AudioSource walkingSource;
    [SerializeField] AudioSource shootingSource;
    [SerializeField] AudioSource quipSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SniperSound() {
        float newFloat = Random.Range(minPitch, maxPitch);
        shootingSource.clip = sniperBullet;
        shootingSource.loop = false;
        shootingSource.pitch = newFloat;

        shootingSource.PlayOneShot(sniperBullet);
        Debug.Log("play snipe");
    }

    public void ZoomIn() {
        GameObject go = Instantiate(sourcePrefab);
        AudioSource newSource = go.GetComponent<AudioSource>();
        
        go.transform.SetParent(playerRef);


        float newFloat = Random.Range(minPitch, maxPitch);
        newSource.clip = zoom;
        newSource.loop = false;
        newSource.pitch = newFloat;
        newSource.Play();
        go.GetComponent<DestroyAudioIn>().SetTimer(newSource.clip.length);
        Debug.Log("play zoom");



    }
}
