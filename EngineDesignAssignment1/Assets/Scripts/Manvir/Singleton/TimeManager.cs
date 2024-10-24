using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    public float startTimeInSeconds = 10f;  
    private float currentTime;
    private bool isCountingDown = false;
    public Text text;  
    public Car _car;

    void Start()
    {
        ResetTimer();  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
           
            ResetTimer();
        }
    }

    IEnumerator StartDeathCountdown()
    {
        isCountingDown = true;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            text.text = Mathf.Ceil(currentTime).ToString();
           
            yield return null; 
        }

        currentTime = 0;
        text.text = "0";
        isCountingDown = false;

        CountDownEnd();  //execute when the timer hits zero
    }

    //called when the timer hits zero
    void CountDownEnd()
    {
       Destroy(_car);
    }

    public void ResetTimer()
    {
        currentTime = startTimeInSeconds;

        if (!isCountingDown)
        {
            StartCoroutine(StartDeathCountdown());
        }
    }

}

