using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace Genetic
{
    class Graph
    {
        int numNodes;
        int numEdges;
        Hashtable nodes;

        public int GetNumNodes()
        {
            return numNodes;
        }
        public int GetNumEdges()
        {
            return numEdges;
        }
        public Hashtable GetNodes()
        {
            return nodes;
        }
        public void SetNumNodes(int num)
        {
            numNodes = num;
        }
        public void SetNumEdges(int num)
        {
            numEdges = num;
        }
        public void SetNodes(Hashtable newNodes)
        {
            nodes = newNodes;
        }
        public Graph()
        {
            nodes = new Hashtable();
            numNodes = numEdges = 0;
        }
        public static Graph LoadFromFile(string filePath)
        {
            //Le grafo do arquivo de entrada
            Graph newGraph = new Graph();

            String buffer;
            StreamReader file = new StreamReader(filePath);

            //Le numero de nodos e arestas
            buffer = file.ReadLine();
            newGraph.SetNumNodes(Convert.ToInt32(buffer));

            buffer = file.ReadLine();
            newGraph.SetNumEdges(Convert.ToInt32(buffer));

            //Le grau dos vertices
            buffer = file.ReadLine();
            String[] degrees = buffer.Split(' ');

            for (int i = 0; i < degrees.Length; i++)
            {
                if (degrees[i].Length == 0)
                    continue;

                Node newNode = new Node();

                newNode.SetIndex(i);
                newNode.SetDegree(Convert.ToInt32(degrees[i]));

                newGraph.GetNodes().Add(i, newNode);
            }


            //Le vizinhos
            buffer = file.ReadLine();
            String[] edgeSplit = buffer.Split(' ');

            int currentNode = 0;
            int currentEdge = 0;

            for (int i = 0; i < edgeSplit.Length; i += 1)
            {
                if (edgeSplit[i].Length == 0)
                    continue;

                if (edgeSplit[i].Equals("-1"))
                    break;

                int from = Convert.ToInt32(edgeSplit[i]);
         

                //TODO: revisar representação
                Neighbour edge = new Neighbour(from);

                if (((Node)newGraph.nodes[currentNode]).GetDegree() == ((Node)newGraph.nodes[currentNode]).GetNeighbours().Count)
                {
                    currentNode++;
                    currentEdge = 0;
                }

                ((Node)newGraph.nodes[currentNode]).GetNeighbours().Add(currentEdge, edge);
                currentEdge++;

            }
            // ultima linha do arquivo contém os indices dos vértices vizinhos de cada vértices --> não sei se pode complicar mas, ali em cima 
            // já tá sendo controlado isso.

            return newGraph;
        }
    }
}
