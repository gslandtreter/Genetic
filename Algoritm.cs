using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

// testar mutação 
namespace Genetic
{
    class Algoritm
    {
        //Configuração
        double taxa_CrossOver = 0.80;
        double taxa_Mutation = 0.10;
        double tamanho_populacao; 

        //Dados
        Hashtable population;
        Graph grafo;
        Gene solution;
        float aptidaoAcumulada=0;

        //
        Hashtable populationBuff;
        
        //Para terminar confere tempo e se solução se mantem constante
        DateTime startTime;
        bool isFirst = false;
        bool EhDiferente = false;
        int SolucaoIgual = 0;
        
        public static Random staticRandom = new Random();

        public Algoritm(Graph grafo)
        {
            // TODO: Complete member initialization
            population = new Hashtable();
            populationBuff = new Hashtable();
            solution = new Gene(grafo.GetNumNodes());
            //tamanho_populacao = grafo.GetNumNodes();
            tamanho_populacao = 50;
            startTime = DateTime.Now;
            this.grafo = grafo;
            GenerateRandomPopulation();
            AvaliateFunction();
            
            //loop principal
            while(CondicaoTermino()==false)
            {
                EhDiferente = false;
                //DateTime start = DateTime.Now;
                SelectCreate();        
                AvaliateFunction();
                //TimeSpan spentTime = DateTime.Now - start;
                //Console.WriteLine("Levei {0}ms.", spentTime.TotalMilliseconds);
            }
            PrintaResultado();
        }

        private void PrintaResultado()
        {
            Console.Write("Solucao:");
            Console.WriteLine(solution.avaliationValue);
            Console.Write("Ordem dos labels:");
            Console.WriteLine(string.Join(",", solution.individuo));
        }

        private bool CondicaoTermino()
        {
            TimeSpan t = DateTime.Now - startTime;
            if (EhDiferente)
                SolucaoIgual = 1;
            else
                SolucaoIgual++;
            return t.TotalMinutes >= 15 || SolucaoIgual >= 500;
        }

        private void SelectCreate()
        {
            List<Gene> listaPop = population.Values.Cast<Gene>().ToList();
            listaPop = listaPop.OrderBy(t => t.avaliationValue).ToList();

            while (populationBuff.Count <= tamanho_populacao)
            {
                int firstIndex, secondIndex;

                firstIndex = staticRandom.Next(listaPop.Count / 3);
                secondIndex = staticRandom.Next(listaPop.Count / 3);

                while (firstIndex == secondIndex)   // garante que os individuos sejam diferentes.
                {
                    secondIndex = staticRandom.Next(listaPop.Count / 3);
                }

                // seleciona dois individuos

                Gene ind1 = listaPop[firstIndex];
                Gene ind2 = listaPop[secondIndex];

                // confere taxa de crossOver -> tá no alg do alemão
                if (staticRandom.NextDouble() <= taxa_CrossOver)
                    CrossOver(ind1, ind2);
                else
                    CopyBestOne(ind1, ind2);

                //confere taxa de mutação -> tbm tá no alg
                if (staticRandom.NextDouble() <= taxa_Mutation)
                    Mutation();               

            }
            // atualiza a população
            //population = populationBuff;

            population.Clear();

            foreach (DictionaryEntry item in populationBuff)
            {
                Gene geneAtual = (Gene)item.Value;
                Gene novoGene = new Gene(geneAtual.individuo.Length);

                geneAtual.individuo.CopyTo(novoGene.individuo, 0);
                novoGene.avaliationValue = geneAtual.avaliationValue;
                novoGene.avaliationValueNormalizado = geneAtual.avaliationValueNormalizado;
                novoGene.valorAcumulado = 0;

                population.Add(population.Count, novoGene);
            }
            populationBuff.Clear();
            aptidaoAcumulada = 0;
        }

        // http://mnemstudio.org/genetic-algorithms-mutation.htm
        private void Mutation()
        {
            int gene = staticRandom.Next(population.Count); // seleciona individuo randomicamente

            Gene indBuffer = (Gene)population[gene];
            Gene ind = new Gene(indBuffer.individuo.Length);
            indBuffer.individuo.CopyTo(ind.individuo, 0);

            gene = staticRandom.Next(ind.individuo.Length); //seleciona genes randomicamente
            int gene2 = staticRandom.Next(ind.individuo.Length);
            while (gene == gene2)
            {
                gene2 = staticRandom.Next(ind.individuo.Length);
            }
            //troca os dois genes;
            int buff = 0;
            buff = ind.individuo[gene];
            ind.individuo[gene] = ind.individuo[gene2];
            ind.individuo[gene2] = buff;
            ind.valorAcumulado = 0;
            ind.avaliationValue = 0;

            //insere na nova população
            populationBuff.Add(populationBuff.Count, ind);
        }

        private void CopyBestOne(Gene ind1, Gene ind2)
        {
            if (ind1.avaliationValue > ind2.avaliationValue)
            {
                Gene novoGene = new Gene(ind2.individuo.Length);
                ind2.individuo.CopyTo(novoGene.individuo, 0);
                novoGene.avaliationValue = ind2.avaliationValue;
                populationBuff.Add(populationBuff.Count, novoGene);
            }
            else
            {
                Gene novoGene = new Gene(ind1.individuo.Length);
                ind1.individuo.CopyTo(novoGene.individuo, 0);
                novoGene.avaliationValue = ind1.avaliationValue;
                populationBuff.Add(populationBuff.Count, novoGene);
            }            
        }

        //http://creationwiki.org/pt/Algoritmo_gen%C3%A9tico#Order_crossover_.28OX.29
        // me basiei no crossOver OX qe é o q o guilherme implementou.
        private void CrossOver(Gene ind1, Gene ind2)
        {
            Gene son = new Gene(this.grafo.GetNumNodes());
            Gene son2 = new Gene(this.grafo.GetNumNodes());

            int cut = staticRandom.Next(grafo.GetNumNodes());
            int cut2 = staticRandom.Next(grafo.GetNumNodes());

            while (cut >= cut2) // faz com que os cortes não sejam no msm ponto
            {
                cut = staticRandom.Next(grafo.GetNumNodes());
                cut2 = staticRandom.Next(grafo.GetNumNodes());
            }

            //atribui os valores de cada um dos filhos na faixa.            
            if (cut2 > cut)
            {
                int[] buff1 = new int[this.grafo.GetNumNodes()];
                int[] buff2 = new int[this.grafo.GetNumNodes()];
                for (int i = cut; i < cut2; i++)
                {
                    son.individuo[i] = ind1.individuo[i];
                    son2.individuo[i] = ind2.individuo[i];
                }

                int coun = 0;
                for (int k = cut2; k < this.grafo.GetNumNodes(); k++)   // registra a lista auxiliar para completar o filho , depois do corte
                {
                    buff1[coun] = ind1.individuo[k];
                    buff2[coun] = ind2.individuo[k];
                    coun++;
                }

                for (int k = 0; k < cut2; k++)   // antes do primeiro corte 
                {
                    buff1[coun] = ind1.individuo[k];
                    buff2[coun] = ind2.individuo[k];
                    coun++;
                }

                //completa os filhos
                int coun2 = cut2;
                int coun3 = cut2;

                for (int k = 0; k < buff2.Length; k++)
                {
                    if (son.individuo.Contains(buff2[k]) == false)
                    {
                        son.individuo[coun2] = buff2[k];
                        coun2++;
                    }
                    if (son2.individuo.Contains(buff1[k]) == false)
                    {
                        son2.individuo[coun3] = buff1[k];
                        coun3++;
                    }
                    if (coun2 >= buff2.Length)
                    {
                        coun2 = 0;
                    }
                    if (coun3 >= buff2.Length)
                    {
                        coun3 = 0;
                    }
                }

            }
            //adiciona elementos na população
            populationBuff.Add(populationBuff.Count, son);
            populationBuff.Add(populationBuff.Count, son2);
        }        


        // gera primeira população randomica, está gerando número randomico para cada vértice. Esse número randomico varia conforme o número de vértices disponíveis.
        public void GenerateRandomPopulation()
        {
            for (int k = 0; k < this.tamanho_populacao; k++) // tamanho da população --> rever
            {
                Gene newInd = new Gene(this.grafo.GetNumNodes());

                newInd.InitializeIndividuo();
                newInd.ShuffleIndividuo();

                population.Add(k, newInd);                
            } 
        }

        //Avalia toda a população        
        public void AvaliateFunction()
        {         
            foreach (DictionaryEntry gene in this.population)
            {
                ((Gene)gene.Value).AvaliateGene(this.grafo);
                if (isFirst == false) // seta solução arbitraria
                {
                    Gene novaSolucao = ((Gene)gene.Value);
                    solution = new Gene(novaSolucao.individuo.Length);
                    novaSolucao.individuo.CopyTo(solution.individuo, 0);
                    solution.avaliationValue = novaSolucao.avaliationValue;
                    
                    isFirst=true;
                }
                if (((Gene)gene.Value).avaliationValue < solution.avaliationValue)  // guarda o melhor gene
                {
                    Gene novaSolucao = ((Gene)gene.Value);
                    solution = new Gene(novaSolucao.individuo.Length);
                    novaSolucao.individuo.CopyTo(solution.individuo, 0);
                    solution.avaliationValue = novaSolucao.avaliationValue;
                    EhDiferente = true;
                }
            }
        } 
    }
}
