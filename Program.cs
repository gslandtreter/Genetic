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
            Graph grafo = Graph.LoadFromFile("Instancias/small.gra");

            Console.WriteLine("Leu grafo do arquivo: Vertices: {0}, Arestas: {1}",
                grafo.GetNumNodes(), grafo.GetNumEdges());

            Algoritm ag = new Algoritm(grafo);


            Console.ReadKey();
        }
    }
}
