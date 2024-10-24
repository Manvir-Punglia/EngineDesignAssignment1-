using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerUI : MonoBehaviour
{
    public CarCustomizer carCustomizer;
    public Car car;

    public void RandomColour()
    {
        ICommand randomColour = new ChangeCarColorCommand(car);
        carCustomizer.AddCommand(randomColour);
    }

    public void UndoColour()
    {
        carCustomizer.UndoCommand();
    }
}
