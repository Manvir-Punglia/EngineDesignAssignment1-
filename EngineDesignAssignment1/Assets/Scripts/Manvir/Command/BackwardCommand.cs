using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardCommand : ICommand
{
    private Car _car;

    public BackwardCommand(Car car)
    {
        _car = car;
    }

    public  void Execute()
    {
        _car.MoveBackward();
    }
}
