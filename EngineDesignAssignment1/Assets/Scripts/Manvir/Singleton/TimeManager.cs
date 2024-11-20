using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    private float startTimeInSeconds ;  
    private float currentTime;
    private bool isCountingDown = false;
    public Text text;  
    public Car _car;
    public ConfigParser _configParser;

  
    void Start()
    {
        
        
        
        ResetTimer();

        if (_configParser == null)
        {
            _configParser = FindObjectOfType<ConfigParser>();
        }
            

        if (_car == null)
        {
            _car = FindObjectOfType<Car>();
        }
        startTimeInSeconds = _configParser.timerLength;
        //Debug.Log(startTimeInSeconds);
        currentTime = startTimeInSeconds;
        //Debug.Log(currentTime);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
           
           // ResetTimer();
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

        CountDownEnd();  
    }

    
    void CountDownEnd()
    {
        _car.enabled = false;
        _car.isDrift = false;
        StartCoroutine(RestartAfterDelay(5));
    }

    IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _car.enabled = true;
        _car.RespawnCar();
        ResetTimer();
        CheckpointManager.Instance.ResetCheckpoints();
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

