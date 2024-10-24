using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeftCommand : Command
{
    private Car _car;

    public TurnLeftCommand(Car car)
    {
        _car = car;
    }

    public override void Execute()
    {
        _car.TurnLeft();
    }
}
