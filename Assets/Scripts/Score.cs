using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    public RawImage theImage;
    public Texture[] myscore = new Texture[4];

    private int currentItem = 0;
    void Start()
    {
        theImage.texture = myscore[currentItem];
    }

}
