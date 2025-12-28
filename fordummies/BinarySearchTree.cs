using System.Text;
public interface IBinaryNode<T> where T : IElementWithKey
{
    public bool IsEmptyNode();
}
public class BinarySearchTree<T> where T : IElementWithKey
{
    private IBinaryNode<T> _root = new EmptyNode<T>(); //privater setter weil Aufgabenblatt 10 sagt kein public setter aber ohne setter geht nicht und direkt new EmptyNode implementiert wg non nullable
    public IBinaryNode<T> Root{
      get => _root; 
      private set => _root = value;
    }
    public BinarySearchTree(T element)
    {
        Root = new BinaryNode<T>(element);
    }
    public BinarySearchTree()
    {
        Root = new EmptyNode<T>();
    }
    public void Insert(T element)
    {
        if (Root.IsEmptyNode())   // Wenn der Baum leer ist -> einfach Root setzen
        {
            Root = new BinaryNode<T>(element);
            return;
        }
        var node = (BinaryNode<T>)Root;

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
    public void InOrder(Action<T> f) => InOrderHelper(f, Root);
    public void PreOrder(Action<T> f) => PreOrderHelper(f, Root);
    public void PostOrder(Action<T> f) => PostOrderHelper(f, Root);
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
    public T TreeMinimum() => MinimumNode(Root).Element;
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
        IBinaryNode<T> current = Root;

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
                Root = new EmptyNode<T>(); // Wenn der Node Root war -> Baum wird komplett leer
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
            Root = realChild;
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
