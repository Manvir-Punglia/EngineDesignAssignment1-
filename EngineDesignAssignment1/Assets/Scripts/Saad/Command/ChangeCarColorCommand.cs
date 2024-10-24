using UnityEngine;

public class ChangeCarColorCommand : ICommand
{
    private Car _car;
    private Color _newColor;
    private Color _oldColor;

    public ChangeCarColorCommand(Car car)
    {
        this._car = car;
        this._oldColor = _car.GetCarColour();
        this._newColor = new Color(Random.value, Random.value, Random.value);
    }

    public void Execute()
    {
        _car.SetCarColour(_newColor);
    }

    public void Undo()
    {
        _car.SetCarColour(_oldColor);
    }
}
