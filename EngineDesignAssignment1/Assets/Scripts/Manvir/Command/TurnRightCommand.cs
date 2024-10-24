using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRightCommand : ICommand
{
    private Car _car;

    public TurnRightCommand(Car car)
    {
        _car = car;
    }

    public  void Execute()
    {
        _car.TurnRight();
    }
}
