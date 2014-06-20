using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph grafo = Graph.LoadFromFile("Instancias/gd96a.gra");

            Console.WriteLine("Leu grafo do arquivo: Vertices: {0}, Arestas: {1}",
                grafo.GetNumNodes(), grafo.GetNumEdges());

            DateTime startTime = DateTime.Now;

            Algoritm ag = new Algoritm(grafo);

            TimeSpan timeSpent = DateTime.Now - startTime;

            Console.Write("Tempo consumido (s): {0}", timeSpent.TotalSeconds);
            Console.ReadKey();
        }
    }
}
