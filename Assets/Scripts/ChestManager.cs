using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private Score score;

    private int counter;


    private void Awake()
    {
        counter = 0;
    }

    public void AddCounter(int sum = 1)
    {
        counter += sum;
        score.ChangeImage(counter);
        if (counter >= 3)
        {
            SceneManager.LoadScene("Win");
        }
    }

}