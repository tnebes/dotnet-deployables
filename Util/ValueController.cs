namespace dotnet_deployables.Controllers;

public class ValueController
{
    private readonly bool _increment;
    private readonly int _goal;
    private int _value;

    public ValueController(int value, bool increment)
    {
        _increment = increment;
        _value = increment ? 1 : value;
        _goal = increment ? value : 1;
    }

    public int GetValue()
    {
        return _increment ? _value++ : _value--;
    }

    public bool IsDone()
    {
        return _increment ? _value > _goal : _value < _goal;
    }
}