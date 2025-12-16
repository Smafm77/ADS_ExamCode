namespace ADS
{
    public class ADS
    {
        static void Main()
        {
            var minpq = new MinPriorityQueue<Test>(20);
            Console.WriteLine(minpq.ToString());
            for (int i = 1; i <= 10; i++)
            {
                minpq.Insert(new Test(i, i));
                Console.WriteLine(minpq.ToString());
            }
            minpq.ExtractFirst();
            Console.WriteLine(minpq.ToString());
            Test tmin = new(11, 11);
            minpq.Insert(tmin);
            Console.WriteLine(minpq.ToString());
            tmin.ChangeableKey = 1;
            Console.WriteLine(minpq.ToString());

            var maxpq = new MaxPriorityQueue<Test>(20);
            Console.WriteLine(maxpq.ToString());
            for (int i = 1; i <= 10; i++)
            {
                maxpq.Insert(new Test(i, i));
                Console.WriteLine(maxpq.ToString());
            }
            maxpq.ExtractFirst();
            Console.WriteLine(maxpq.ToString());
            Test tmax = new(11, 11);
            maxpq.Insert(tmax);
            Console.WriteLine(maxpq.ToString());
            tmax.ChangeableKey = 12;
            Console.WriteLine(maxpq.ToString());
            var g = new Graph(6);
            g.Nodes[0].AddEdge(g.Nodes[1], 7);
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


            //BFS Test
            g.BFS(g.Nodes[0]);
            Console.WriteLine("BFS Pfad 0 -> 4:");
            g.PrintPath(g.Nodes[0], g.Nodes[4]);
            Console.WriteLine("Distanz 4 = " + g.Nodes[4].Distance);

            //DFS Test
            g.DFS();
            Console.WriteLine("DFS fertig. Parent von Node 4: " + (g.Nodes[4].Parent?.Key.ToString() ?? "null"));

            //Dijkstra Test
            g.Dijkstra(g.Nodes[0]);
            Console.WriteLine("Dijkstra Pfad 0 -> 4:");
            g.PrintPath(g.Nodes[0], g.Nodes[4]);
            Console.WriteLine("Distanz 4 = " + g.Nodes[4].Distance);


            //Alle Nodes mit distanz zu root und parents
            for (int i = 0; i < g.Nodes.Length; i++)
            {
                Console.WriteLine($"Node {i}: dist={g.Nodes[i].Distance}, parent={(g.Nodes[i].Parent?.Key.ToString() ?? "null")}");
            }

        }
    }
}
