public class Edge : IElementWithKey
{
    private Node _node;
    private int _weight;
    public Edge(Node neighbor, int weight)
    {
        _node = neighbor;
        Weight = weight;
    }
    public Node Neighbor => _node;
    public int Weight
    {
        get => _weight;
        set => _weight = value;
    }
    public int Key => _node.Key;
    public int ChangeableKey
    {
        get => _node.ChangeableKey;
        set => _node.ChangeableKey = value;
    }
    public Action ChangingAction
    {
        set => _node.ChangingAction = value;
    }  
}
