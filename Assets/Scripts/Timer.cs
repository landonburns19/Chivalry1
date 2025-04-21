using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public GameOver gameOver;
    [SerializeField] TextMeshProUGUI timerText;
    public float remainingTime;

    void Update()
    {
        if(remainingTime > 0)
        {
            remainingTime  -= Time.deltaTime;
        }
        else if(remainingTime<0)
        {
            remainingTime = 0;
            gameOver.Setup();
            timerText.color = Color.green;
        }
        int min = Mathf.FloorToInt(remainingTime / 60);
        int sec = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
}
