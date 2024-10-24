using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCustomizer : MonoBehaviour
{
    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    public void AddCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
    }

    public void UndoCommand()
    {
        if (commandHistory.Count > 0)
        {
            ICommand command = commandHistory.Pop();
            command.Undo();
        }
    }
}
