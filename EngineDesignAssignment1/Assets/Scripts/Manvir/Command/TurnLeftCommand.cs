using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeftCommand : ICommand
{
    private Car _car;

    public TurnLeftCommand(Car car)
    {
        _car = car;
    }

    public  void Execute()
    {
        _car.TurnLeft();
    }

    public void Undo()
    {

    }
}
