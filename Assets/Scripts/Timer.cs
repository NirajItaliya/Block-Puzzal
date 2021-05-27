using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private string TIMER_LABEL = "Time: ";
    private IEnumerator coroutine;

    public int startTime = 5;  
    public int timeLeft;       
    public Text timeDisplay;   

    void Awake() {
        GameManager.instance.GameOverEvent += OnGameOverEvent;
        coroutine = Countdown();

        SetTime(startTime);
    }

    
    public void AddTime(int delta)
    {
        timeLeft += delta;
        UpdateTimerText();
    }

   
    public void SetTime(int time)
    {
        timeLeft = time;
        UpdateTimerText();
    }

    
    public void SetDisplay(bool display)
    {
        timeDisplay.gameObject.SetActive(display);
    }

   
    public void StartTimer()
    {
        StartCoroutine(coroutine);
    }

  
    public void StopTimer()
    {
        StopCoroutine(coroutine);
    }

    
    private void OnGameOverEvent(object sender, System.EventArgs e)
    {
        StopTimer();
    }

    
    private IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            AddTime(-1);
        }

        GameManager.instance.GameOver();
    }

    private void UpdateTimerText()
    {
        timeDisplay.text = TIMER_LABEL + timeLeft.ToString();
    }
}