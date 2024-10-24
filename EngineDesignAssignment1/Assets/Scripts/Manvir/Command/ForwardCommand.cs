using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardCommand :Command 
{
    private Car _car;

    public ForwardCommand(Car car)
    {
        _car = car;
    }

    public override void Execute()
    {
        _car.MoveForward();
    }
}
