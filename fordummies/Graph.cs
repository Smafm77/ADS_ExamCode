public enum Color
{
    White,
    Grey,
    Black
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
    private Node FindNodeByKey(int id){
      return Nodes[id];
    } 
    public void BFS(int id)
    {
        Node s = FindNodeByKey(id);
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
    public void DFS(int id)
    {
      foreach (Node n in Nodes)
        {
            n.Color = Color.White;
            n.Distance = -1;
            n.Parent = null;
        }
      Node start = FindNodeByKey(id);
      DFSVisit(start,0);
    }
    private void DFSVisit(Node n, int distance)
    {
        n.Distance = distance;
        n.Color = Color.Grey;
        n.IterateThroughNeighbors(nei =>
        {
            if (nei.Color == Color.White)
            {
                nei.Parent = n;
                DFSVisit(nei, distance + 1);
            }
        });
        n.Color = Color.Black;
    }
    public void Dijkstra(int id)
    {
        MinPriorityQueue<Node> q = new(Nodes.Length);
        Node s = FindNodeByKey(id);
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
            if (x.Distance == int.MaxValue) //wenn kleinstes schon unendlich, dann ist rest unerreichbar
            {
                break;
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
