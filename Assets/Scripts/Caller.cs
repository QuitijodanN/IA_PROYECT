using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caller : MonoBehaviour
{
    private GameObject[] mushrooms;
    private Transform target;

    void Start()
    {
        mushrooms = GameObject.FindGameObjectsWithTag("Mushroom");
        target = GameObject.FindWithTag("Player").transform;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target.transform)
        {
            foreach (GameObject mush in mushrooms)
            {
                mush.GetComponent<PatrollRayCast>().seek = true;
                mush.GetComponent<PatrollRayCast>().caller = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform == target.transform)
        {
            foreach (GameObject mush in mushrooms)
            {
                mush.GetComponent<PatrollRayCast>().caller = false;
            }
        }
    }
}
