using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTimer : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(timer());
    }


    IEnumerator timer()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
