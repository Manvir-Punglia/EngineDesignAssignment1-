using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : Observer
{
    public int maximum = 100;
    public int current = 50;
    public Image mask; 
    public Image fill; 
    public Color backgroundColor = Color.red;
    private Car _Car;
    
    void GetCurrentFill()
    {
        float fillAmount = (float)current/(float)maximum;
        mask.fillAmount = fillAmount;
        fill.color = backgroundColor;
    }

    private void Update()
    {
        GetCurrentFill();
    }

    public override void Notify(Subject subject)
    {
        
        _Car = subject.GetComponent<Car>();
        current = (int)_Car.GetSpeed();
    }
}
