namespace Fordummies
{
    public class Fordummies
    {
        static void Main()
        {
            //var minpq = new MinPriorityQueue<Test>(20);
            //Console.WriteLine(minpq.ToString());
            //for (int i = 1; i <= 10; i++)
            //{
                //minpq.Insert(new Test(i, i));
                //Console.WriteLine(minpq.ToString());
            //}
            //minpq.ExtractFirst();
            //Console.WriteLine(minpq.ToString());
            //Test tmin = new(11, 11);
            //minpq.Insert(tmin);
            //Console.WriteLine(minpq.ToString());
            //tmin.ChangeableKey = 1;
            //Console.WriteLine(minpq.ToString());
//
            //var maxpq = new MaxPriorityQueue<Test>(20);
            //Console.WriteLine(maxpq.ToString());
            //for (int i = 1; i <= 10; i++)
            //{
                //maxpq.Insert(new Test(i, i));
                //Console.WriteLine(maxpq.ToString());
            //}
            //maxpq.ExtractFirst();
            //Console.WriteLine(maxpq.ToString());
            //Test tmax = new(11, 11);
            //maxpq.Insert(tmax);
            //Console.WriteLine(maxpq.ToString());
            //tmax.ChangeableKey = 12;
            //Console.WriteLine(maxpq.ToString());
            var g = new Graph(6); //Graph mit 6 Nodes
            
            //Kanten erzeugen

            g.Nodes[0].AddEdge(g.Nodes[1], 7);  //Kante 0 auf 1 mit Gewicht 7
            g.Nodes[1].AddEdge(g.Nodes[0], 7);

            g.Nodes[0].AddEdge(g.Nodes[2], 9);
            g.Nodes[2].AddEdge(g.Nodes[0], 9);

            g.Nodes[0].AddEdge(g.Nodes[5], 14);
            g.Nodes[5].AddEdge(g.Nodes[0], 14);

            g.Nodes[1].AddEdge(g.Nodes[2], 10);
            g.Nodes[2].AddEdge(g.Nodes[1], 10);

            g.Nodes[1].AddEdge(g.Nodes[3], 15);
            g.Nodes[3].AddEdge(g.Nodes[1], 15);

            g.Nodes[2].AddEdge(g.Nodes[3], 11);
            g.Nodes[3].AddEdge(g.Nodes[2], 11);

            g.Nodes[2].AddEdge(g.Nodes[5], 2);
            g.Nodes[5].AddEdge(g.Nodes[2], 2);

            g.Nodes[3].AddEdge(g.Nodes[4], 6);
            g.Nodes[4].AddEdge(g.Nodes[3], 6);

            g.Nodes[4].AddEdge(g.Nodes[5], 9);
            g.Nodes[5].AddEdge(g.Nodes[4], 9);

            
            //Testnummern
            int a = 1;
            int b = 5;

            //BFS Test
            g.BFS(g.Nodes[a].Key);
            Console.WriteLine("BFS Pfad "+a+ " -> " +b+":");
            g.PrintPath(g.Nodes[a], g.Nodes[b]);
            Console.WriteLine("Distanz " +b+ "  = " + g.Nodes[b].Distance);
            DistanzDruck(g, "BFS");

            //DFS Test
            g.DFS(a);
            Console.WriteLine("DFS fertig. Parent von Node " +b+ " : " + (g.Nodes[b].Parent?.Key.ToString() ?? "null"));
            DistanzDruck(g, "DFS");

            ////Dijkstra Test
            g.Dijkstra(g.Nodes[a].Key);
            Console.WriteLine("Dijkstra Pfad " +a+ " -> " +b+ ":");
            g.PrintPath(g.Nodes[a], g.Nodes[b]);
            Console.WriteLine("Distanz " +b+  " = " + g.Nodes[b].Distance);
            DistanzDruck(g, "Dijkstra");

            //Alle Nodes mit distanz zu root und parents
            static void DistanzDruck(Graph g, string name)
            {
              for (int i = 0; i < g.Nodes.Length; i++)
              {
                  Console.WriteLine($"Node {i}: dist={g.Nodes[i].Distance}, parent={(g.Nodes[i].Parent?.Key.ToString() ?? "null")}");
              }
              Console.WriteLine();
            }
        }
    }
}
