using System.Text;
public enum Color
{
    White,
    Grey,
    Black
}
public interface IBinaryNode<T> where T : IElementWithKey
{
    public bool IsEmptyNode();
}
public interface IElementWithKey
{
    public int Key { get; }
    public int ChangeableKey { get; set; }
    public Action ChangingAction { set; }
}
public class BinarySearchTree<T> where T : IElementWithKey
{
    public IBinaryNode<T> root;
    public BinarySearchTree(T element)
    {
        root = new BinaryNode<T>(element);
    }
    public BinarySearchTree()
    {
        root = new EmptyNode<T>();
    }
    public void Insert(T element)
    {
      if (root.IsEmptyNode())   // Wenn der Baum leer ist -> einfach Root setzen
        {
            root = new BinaryNode<T>(element);
            return;
        }
        var node = (BinaryNode<T>)root;

        while (true) // Sonst runterlaufen bis ich eine passende freie Stelle finde
        {
            if (element.Key == node.Element.Key)  // Wenn Key schon existiert -> überschreiben
            {
                node.Element = element;
                return;
            }
            else if (element.Key < node.Element.Key)
            {
                if (node.Left is EmptyLeaf<T>) // Wenn links ein EmptyLeaf ist -> da neuen Node hinballern
                {
                    node.Left = new BinaryNode<T>(element, node);
                    return;
                }
                node = (BinaryNode<T>)node.Left;
            }
            else
            {
                if (node.Right is EmptyLeaf<T>)
                {
                    node.Right = new BinaryNode<T>(element, node);
                    return;
                }
                node = (BinaryNode<T>)node.Right;
            }
        }
    }
    public void DisplayNode(IBinaryNode<T> node, string indent = "", bool isLeft = true) //geklauter Code abgeändert mit kleiner Sicherheit
    {
        if (node is not BinaryNode<T> b) return;

        Console.WriteLine(indent + (isLeft ? "└── " : "┌── ") + b.Element.ToString());

        DisplayNode(b.Left, indent + (isLeft ? "    " : "│   "), true);
        DisplayNode(b.Right, indent + (isLeft ? "    " : "│   "), false);
    }
    public void InOrder(Action<T> f) => InOrderHelper(f, root);
    public void PreOrder(Action<T> f) => PreOrderHelper(f, root);
    public void PostOrder(Action<T> f) => PostOrderHelper(f, root);
    private void InOrderHelper(Action<T> f, IBinaryNode<T> node)
    {
        // InOrder = links - node - rechts
        if (node is BinaryNode<T> n)
        {
            InOrderHelper(f, n.Left);
            f(n.Element);
            InOrderHelper(f, n.Right);
        }
    }
    private void PreOrderHelper(Action<T> f, IBinaryNode<T> node)
    {
        // PreOrder = node - links - rechts
        if (node is BinaryNode<T> n)
        {
            f(n.Element);
            PreOrderHelper(f, n.Left);
            PreOrderHelper(f, n.Right);
        }
    }
    private void PostOrderHelper(Action<T> f, IBinaryNode<T> node)
    {
        // PostOrder = links - rechts - node
        if (node is BinaryNode<T> n)
        {
            PostOrderHelper(f, n.Left);
            PostOrderHelper(f, n.Right);
            f(n.Element);
        }
    }
    override public string ToString()
    {
        StringBuilder sb = new StringBuilder();
        InOrder(t => sb.Append(t.ToString()).Append(" "));
        return sb.ToString().TrimEnd(' ');
    }
    public T TreeMinimum() => MinimumNode(root).Element;
    private BinaryNode<T> MinimumNode(IBinaryNode<T> node)
    {
        if (node is BinaryNode<T> n)  // n ist ein echter Knoten
        {
            while (n.Left is BinaryNode<T> next)
            {
                n = next;
            }
            return n;
        }
        else throw new Exception("Oops");
    }
    private BinaryNode<T> FindNode(int key)
    {
        IBinaryNode<T> current = root;

        while (current is BinaryNode<T> node)
        {
            if (key == node.Element.Key)
            {
                return node;
            }
            else if (key < node.Element.Key)
            {
                current = node.Left;
            }
            else
            {
                current = node.Right;
            }
        }
        throw new Exception("Key existiert nicht im Baum.");
    }
    public T Find(int key) => FindNode(key).Element;

    public T Delete(int key)
    {
        var node = FindNode(key);
        T deletedElement = node.Element;
        DeleteNode(node);
        return deletedElement;
    }
    private void DeleteNode(BinaryNode<T> node)
    {
        IBinaryNode<T> parent = node.Parent;
        IBinaryNode<T> left = node.Left;
        IBinaryNode<T> right = node.Right;

        bool hasLeftChild = left is BinaryNode<T>;
        bool hasRightChild = right is BinaryNode<T>;

        //Fall 3: zwei Kinder
        if (hasLeftChild && hasRightChild)
        {
            BinaryNode<T> successor = MinimumNode(right);
            node.Element = successor.Element;
            DeleteNode(successor);             // Successor löschen (der ist garantiert Fall 1 oder 2)
            return;
        }
        //Fall 1: keine Kinder -> einfach entfernen
        if (!hasLeftChild && !hasRightChild)
        {
            if (parent is EmptyNode<T>)
            {
                root = new EmptyNode<T>(); // Wenn der Node Root war -> Baum wird komplett leer
            }
            else if (parent is BinaryNode<T> p) // Sonst -> Parent bekommt einen neuen EmptyLeaf als Kind
            {
                if (p.Left == node)
                {
                    p.Left = new EmptyLeaf<T>(p);
                }
                else if (p.Right == node)
                {
                    p.Right = new EmptyLeaf<T>(p);
                }
            }
            return;
        }
        //Fall 2: genau ein Kind -> Kind hochschieben
        IBinaryNode<T> child = hasLeftChild ? left : right;

        if (child is not BinaryNode<T> realChild)
        {
            throw new Exception("Das sollte nie passieren.");
        }

        if (parent is EmptyNode<T>) // Wenn Node die Root war -> Child wird neue Root
        {
            realChild.Parent = new EmptyNode<T>();
            root = realChild;
        }
        else if (parent is BinaryNode<T> p)
        {
            if (p.Left == node)
            {
                p.Left = realChild; // Parent zeigt jetzt auf das Kind statt auf node
            }
            else if (p.Right == node)
            {
                p.Right = realChild;
            }
            realChild.Parent = p; // Parent des Kindes aktualisieren
        }
    }
}
public class EmptyNode<T> : IBinaryNode<T> where T : IElementWithKey
{
    public bool IsEmptyNode() => true;
}
public class EmptyLeaf<T> : IBinaryNode<T> where T : IElementWithKey
{
    private IBinaryNode<T> _parent;
    public IBinaryNode<T> Parent
    {
        get => _parent;
        set => _parent = value;
    }
    public EmptyLeaf(IBinaryNode<T> parent)
    {
        _parent = parent;
    }
    public bool IsEmptyNode() => false;
}
public class BinaryNode<T> : IBinaryNode<T> where T : IElementWithKey
{
    private T _element;
    private IBinaryNode<T> _left;
    private IBinaryNode<T> _right;
    private IBinaryNode<T> _parent;
    public T Element
    {
        get => _element;
        set => _element = value;
    }
    public IBinaryNode<T> Left
    {
        get => _left;
        set => _left = value;
    }
    public IBinaryNode<T> Right
    {
        get => _right;
        set => _right = value;
    }
    public IBinaryNode<T> Parent
    {
        get => _parent;
        set => _parent = value;
    }
    public bool IsEmptyNode() => false;

    public BinaryNode(T element, IBinaryNode<T> left, IBinaryNode<T> right, IBinaryNode<T> parent)
    {
        _element = element;
        _left = left;
        _right = right;
        _parent = parent;
    }
    public BinaryNode(T element, IBinaryNode<T> parent)
    {
        _element = element;
        _parent = parent;
        _left = new EmptyLeaf<T>(this);
        _right = new EmptyLeaf<T>(this);
    }
    public BinaryNode(T element)
    {
        _element = element;
        _left = new EmptyLeaf<T>(this);
        _right = new EmptyLeaf<T>(this);
        _parent = new EmptyNode<T>();
    }
}
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
public class Graph
{
    private Node[] _nodes;
    public Node[] Nodes => _nodes;
    public Graph(int n)
    {
        _nodes = new Node[n];
        for (int i = 0; i < n; i++)
        {
            _nodes[i] = new Node(i);
        }
    }
    public void BFS(Node s)
    {
        foreach (Node n in Nodes)
        {
            n.Color = Color.White;
            n.Distance = -1;
            n.Parent = null;
        }
        s.Color = Color.Grey;
        s.Distance = 0;
        Queue<Node> q = new();
        q.Enqueue(s);
        while (q.Count > 0)
        {
            Node n = q.Dequeue();
            n.IterateThroughNeighbors(node =>
            {
                if (node.Color == Color.White)
                {
                    node.Color = Color.Grey;
                    node.Distance = n.Distance + 1;
                    node.Parent = n;
                    q.Enqueue(node);
                }
            });
            n.Color = Color.Black;
        }
    }
    public void PrintPath(Node s, Node v)
    {
        if (v == s)
        {
            Console.WriteLine(s);
        }
        else if (v.Parent == null)
        {
            Console.WriteLine("Es gibt keinen Pfad von " + s + " nach " + v);
        }
        else
        {
            PrintPath(s, v.Parent);
            Console.WriteLine(v);
        }
    }
    public void DFS()
    {
        foreach (Node n in Nodes)
        {
            n.Color = Color.White;
            n.Distance = -1;
            n.Parent = null;
        }
        foreach (Node n in Nodes)
        {
            if (n.Color == Color.White)
            {
                DFSVisit(n, 0);
            }
        }
    }
    private void DFSVisit(Node n, int distance)
    {
        n.Distance = distance;
        n.Color = Color.Grey;
        n.IterateThroughNeighbors(node =>
        {
            if (node.Color == Color.White)
            {
                node.Parent = n;
                DFSVisit(node, distance + 1);
            }
        });
        n.Color = Color.Black;
    }
    public void Dijkstra(Node s)
    {
        MinPriorityQueue<Node> q = new(Nodes.Length);

        foreach (Node v in Nodes)
        {
            v.Parent = null;
            if (v != s)
            {
                v.ChangeableKey = int.MaxValue;
            }
            else
            {
                v.ChangeableKey = 0;
            }
            q.Insert(v);
        }
        while (q.Count > 0) //gab kein IsEmpty aber Count >0 sollte das selbe tun
        {
            Node x = q.ExtractFirst();
            if (x.Distance == int.MaxValue) // gegen böse böse integer overflow fehler
            {
                continue;
            }
            x.IterateThroughNeighbors(y =>
            {
                int w = x.GetWeight(y);
                int newDist = x.Distance + w;
                if (newDist < y.Distance)
                {
                    y.Parent = x;
                    y.ChangeableKey = newDist;
                }
            });
        }
    }
}
public enum MaxMin
{
    Max,
    Min
}
public class MaxMinPriorityQueue<T> where T : IElementWithKey
{
    private T[] _daten;
    private int _count; //Anzahl der Elemente im Heap
    private MaxMin _mode;
    public MaxMinPriorityQueue(int size, MaxMin auswahl)
    {
        _daten = new T[size];
        _count = 0;
        _mode = auswahl;
    }

    public T[] Daten => _daten;
    public int Count => _count;
    public MaxMin Mode => _mode;

    private void Swap(int i, int j)
    {
        (_daten[i], _daten[j]) = (_daten[j], _daten[i]);
        _daten[i].ChangingAction = CreateChangingKeyAction(i);
        _daten[j].ChangingAction = CreateChangingKeyAction(j);
    }
    private bool Better(int i, int j)
    {
        return (_mode == MaxMin.Max && Greater(_daten[i], _daten[j])) || (_mode == MaxMin.Min && Less(_daten[i], _daten[j]));
    }
    public bool Less(T object1, T object2) => object1.ChangeableKey < object2.ChangeableKey;

    public bool Greater(T object1, T object2) => object1.ChangeableKey > object2.ChangeableKey;

    public void Heapify(int index)
    {
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int best = index; //"Bester" Knoten -> Größter bei Max, Kleinster bei Min

            if (left < _count) //Wenn linker Knoten existiert, prüfe ob "besser" als best
            {
                if (Better(left, best))
                {
                    best = left;
                }
            }

            if (right < _count)
            {
                if (Better(right, best))
                {
                    best = right;
                }
            }

            if (best == index)
                break;

            Swap(index, best);
            index = best;
        }
    }
    public T First()
    {
        if (_count == 0)
        {
            throw new InvalidOperationException("PriorityQueue ist leer");
        }
        return Daten[0];
    }
    public T ExtractFirst()
    {
        if (_count == 0)
        {
            throw new InvalidOperationException("PriorityQueue ist leer");
        }
        T result = _daten[0];
        _count--; //Letzes Element logisch entfernen (Größe verringern)

        if (_count > 0)
        {
            _daten[0] = _daten[_count]; //root aktualisieren
            _daten[0].ChangingAction = CreateChangingKeyAction(0); //Auch hier wird pos geändert
            Heapify(0); //sortieren
        }
        return result;
    }
    public void Insert(T element)
    {
        if (_count == _daten.Length)
        {
            throw new InvalidOperationException("PriorityQueue ist voll.");
        }
        int i = _count;
        _daten[i] = element;
        _count++;

        _daten[i].ChangingAction = CreateChangingKeyAction(i);

        IncreaseKey(i);
    }
    private void IncreaseKey(int pos)
    {
        while (pos > 0) //Bubble up
        {
            int parent = (pos - 1) / 2;

            if (Better(pos, parent))
            {
                Swap(pos, parent);
                pos = parent;
            }
            else
                break;
        }
    }
    private Action CreateChangingKeyAction(int pos)
    {
        return () => IncreaseKey(pos);
    }
    override public string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _count; i++)
        {
            sb.Append(_daten[i].ChangeableKey).Append(" ");
        }
        return sb.ToString().TrimEnd(' ');
    }
}
public class MaxPriorityQueue<T> : MaxMinPriorityQueue<T> where T : IElementWithKey
{
    public MaxPriorityQueue(int size) : base(size, MaxMin.Max) // Aufruf des Parent-Constructors mit MaxMin = Max
    {
    }
    public T Maximum() => First();
}
public class MinPriorityQueue<T> : MaxMinPriorityQueue<T> where T : IElementWithKey
{
    public MinPriorityQueue(int size) : base(size, MaxMin.Min)
    {
    }
    public T Minimum() => First();
}
