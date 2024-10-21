using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private ChestManager manager;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hola");
        if (collision.gameObject.CompareTag("Player"))
        {
            manager.AddCounter();
            Destroy(gameObject);
        }

    }
}