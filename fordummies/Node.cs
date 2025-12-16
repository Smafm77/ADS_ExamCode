public class Node : IElementWithKey
{
    private int _id;
    private Node? _parent;
    private Color _color;
    private int _distance;
    private BinarySearchTree<Edge> _adjacent;
    private Action _changingAction;
    public int Distance { get => _distance; set => _distance = value; }
    public Node? Parent //muss noch abgefangen werden
    {
        get => _parent;
        set => _parent = value;
    }
    public Color Color
    {
        get => _color;
        set => _color = value;
    }
    public BinarySearchTree<Edge> Adjacent
    {
        get => _adjacent;
        set => _adjacent = value;
    }
    public override string ToString() => _id.ToString();
    public int Key => _id;
    public int ChangeableKey
    {
        get => _distance;
        set
        {
            _distance = value;
            _changingAction();
        }
    }
    public Node(int id)
    {
        _id = id;
        _parent = null;
        _adjacent = new BinarySearchTree<Edge>();
        _changingAction = () => { }; // Funktion die nichts tut dann muss man kein nullable machen
    }
    public Action ChangingAction { set => _changingAction = value; }
    public void AddEdge(Node neighbor, int weight)
    {
        Edge newEdge = new(neighbor, weight);
        Adjacent.Insert(newEdge);
    }
    public void DeleteEdge(Node neighbor)
    {
        Adjacent.Delete(neighbor.Key);
    }
    public void IterateThroughNeighbors(Action<Node> action)
    {
        Adjacent.InOrder(edge =>
    {
        action(edge.Neighbor);
    });
    }
    public int GetWeight(Node neighbor) => Adjacent.Find(neighbor.Key).Weight;
}
