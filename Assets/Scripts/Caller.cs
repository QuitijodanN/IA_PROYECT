using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caller : MonoBehaviour
{
    public GameObject[] mushrooms;
    public Transform target;

    void Start()
    {
        mushrooms = GameObject.FindGameObjectsWithTag("Mushroom");
        target = GameObject.FindWithTag("Player").transform;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.transform == target.transform)
        {
            foreach (GameObject mush in mushrooms) {
                mush.GetComponent<PatrollRayCast>().seek = true;
                mush.GetComponent<PatrollRayCast>().next = false;
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target.transform)
        {
            foreach (GameObject mush in mushrooms)
            {
                mush.GetComponent<PatrollRayCast>().seek = true;
                mush.GetComponent<PatrollRayCast>().next = false;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform == target.transform)
        {
            foreach (GameObject mush in mushrooms)
            {
                mush.GetComponent<PatrollRayCast>().next = true;
            }
        }
    }
}
