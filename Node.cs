using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Genetic
{
    class Node
    {
        List<Neighbour> neighbours;
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

        public void SetNeighbours(List<Neighbour> newEdge)
        {
            neighbours = newEdge;
        }

        public List<Neighbour> GetNeighbours()
        {
            return neighbours;
        }

        public Node()
        {
            neighbours = new List<Neighbour>();
            degree = 0;
        }
    }
}
