public class Test : IElementWithKey
{
    private int _changeableKey;
    private Action _changingAction = () => { };// Funktion die nichts tut dann muss man kein nullable machen
    public int Key { get; }
    public int ChangeableKey
    {
        get => _changeableKey;
        set
        {
            _changeableKey = value;
            _changingAction();
        }
    }
    public Action ChangingAction
    {
        set => _changingAction = value;
    }
    public Test(int key, int changeableKey)
    {
        Key = key;
        ChangeableKey = changeableKey;
    }
}
