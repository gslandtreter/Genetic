using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic
{
    class Gene
    {

        public int[] individuo;
        public float avaliationValue;
        public float avaliationValueNormalizado;
        public float valorAcumulado =0;


        public Gene(int num)
        {
            individuo = new int[num];
            avaliationValueNormalizado = 0;
            avaliationValue = 0;
            SetAll(num);            
        }
        
        public void SetGen(int pos, int num)
        {
            individuo[pos] = num;        
        }

        public bool ContainsNum(int num)
        {

            bool val = individuo.Contains(num);

            return val;
        }
        private void SetAll(int num)
        {
            for (int i = 0; i < num; i++)
            {
                individuo[i] = -1;            
            }
        }
        // fazer as diferenças...
        public void AvaliateGene(Graph grafo)
        {
            SetAvalia(0);
            for(int i=0;i<grafo.GetNodes().Count;i++)
            {
                //TODO: Aqui ta dando indexOutOfRange por causa do -1 no CrossOver
                for (int k = 0; k < ((Node)grafo.GetNodes()[i]).GetNeighbours().Count; k++)
                {
                    avaliationValue += Math.Abs(individuo[i] - ((Node)grafo.GetNodes()[i]).GetNeighbours()[k].neighbour);
                }
            }
        }


        public void SetAvalia(float num)
        {
            this.avaliationValue = num;
        }
    }
}
