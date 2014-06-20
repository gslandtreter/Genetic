using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic
{
    class Neighbour
    {
        public int neighbour;
        
        public Neighbour()
        {
            neighbour = 0;
        }

        public Neighbour(int newFrom)
        {
            neighbour = newFrom;            
        }
    }
}
