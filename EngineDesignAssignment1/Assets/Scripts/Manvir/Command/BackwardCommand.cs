using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardCommand : Command
{
    private Car _car;

    public BackwardCommand(Car car)
    {
        _car = car;
    }

    public override void Execute()
    {
        _car.MoveBackward();
    }
}
