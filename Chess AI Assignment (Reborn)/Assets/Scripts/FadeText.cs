using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    Text text;
    float alpha = 0.0f;
    bool isIncrease = false;

    void Start()
    {
        text = GetComponent<Text>();
    }
    
    void Update()
    {
        text.color = new Color(255.0f, 255.0f, 255.0f, alpha);

        if (isIncrease)
        {
            alpha += Time.deltaTime;
        }
        else if (!isIncrease)
        {
            alpha -= Time.deltaTime;
        }

        CheckThreshold();
    }

    void CheckThreshold()
    {
        if (alpha <= 0.4f)
        {
            isIncrease = true;
        }
        else if (alpha >= 1.0f)
        {
            isIncrease = false;
        }
    }
}






