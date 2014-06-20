using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Genetic
{
    class Node
    {
        Hashtable neighbours;
        int index;
        int degree;

        public void SetIndex(int idx)
        {
            index = idx;
        }

        public int GetIndex()
        {
            return index;
        }

        public void SetDegree(int deg)
        {
            degree = deg;
        }

        public int GetDegree()
        {
            return degree;
        }

        public void SetNeighbours(Hashtable newEdge)
        {
            neighbours = newEdge;
        }

        public Hashtable GetNeighbours()
        {
            return neighbours;
        }

        public Node()
        {
            neighbours = new Hashtable();
            degree = 0;
        }
    }
}
