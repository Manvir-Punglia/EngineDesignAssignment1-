using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRightCommand : Command
{
    private Car _car;

    public TurnRightCommand(Car car)
    {
        _car = car;
    }

    public override void Execute()
    {
        _car.TurnRight();
    }
}
