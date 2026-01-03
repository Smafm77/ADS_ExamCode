public class Edge : IElementWithKey
{
    private Node _node;
    private int _weight;
    public Edge(Node neighbor, int weight)
    {
        _node = neighbor;
        Weight = weight;
    }
    public Node Node
    {
        get => _node;
        set => _node = value;
    }
    public int Weight
    {
        get => _weight;
        set => _weight = value;
    }
    public int Key => Node.Key;
    public int ChangeableKey
    {
        get => Node.ChangeableKey;
        set => Node.ChangeableKey = value;
    }
    public Action ChangingAction
    {
        set => Node.ChangingAction = value;
    }
}
