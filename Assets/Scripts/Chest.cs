using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private ChestManager manager;
    
    private AudioSource audioSource;

    [SerializeField] private AudioClip chestsound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("hola");
        if (collision.gameObject.CompareTag("Player"))
        {
            manager.AddCounter();

            audioSource.Play();
            
            Destroy(gameObject);
        }

    }
}