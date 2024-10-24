using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardCommand :ICommand 
{
    private Car _car;

    public ForwardCommand(Car car)
    {
        _car = car;
    }

    public  void Execute()
    {
        _car.MoveForward();
    }

    public void Undo()
    {

    }
}
