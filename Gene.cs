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

        static Random random = new Random();

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

        public void ShuffleIndividuo()
        {
            for (int i = individuo.Length; i > 1; i--)
            {
                // Pick random element to swap.
                int j = Gene.random.Next(i); // 0 <= j <= i-1
                // Swap.
                int tmp = individuo[j];
                individuo[j] = individuo[i - 1];
                individuo[i - 1] = tmp;
            }
        }

        public void InitializeIndividuo()
        {
            for(int i = 0; i < individuo.Length; i++)
                individuo[i] = i;
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
            if (avaliationValue > 0)
            {
                //Item ja avaliado
                return;
            }

            for(int i=0;i<grafo.GetNodes().Count;i++)
            {
                //TODO: Aqui ta dando indexOutOfRange por causa do -1 no CrossOver
                for (int k = 0; k < ((Node)grafo.GetNodes()[i]).GetNeighbours().Count; k++)
                {
                    avaliationValue += Math.Abs(individuo[i] - individuo[((Neighbour)((Node)grafo.GetNodes()[i]).GetNeighbours()[k]).neighbour]);
                }
            }

            avaliationValue /= 2;
        }


        public void SetAvalia(float num)
        {
            this.avaliationValue = num;
        }
    }
}
