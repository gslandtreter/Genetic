﻿using System;
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
        double taxa_CrossOver = 0.65;
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
            tamanho_populacao = grafo.GetNumNodes();
            startTime = DateTime.Now;
            this.grafo = grafo;
            GenerateRandomPopulation();
            AvaliateFunction();
            

            //loop principal
            while(CondicaoTermino()==false)
            {
                EhDiferente = false;
                SelectCreate();
                AvaliateFunction();
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
            return t.TotalMinutes >= 60 || SolucaoIgual > 500;
        }

        private void SelectCreate()
        {
            while (populationBuff.Count <= tamanho_populacao)
            {
                double buffer, buffer2;
                buffer = staticRandom.NextDouble() * (aptidaoAcumulada);
                buffer2 = staticRandom.NextDouble() * (aptidaoAcumulada);

                while (buffer == buffer2)   // garante que os individuos sejam diferentes.
                {
                    buffer2 = staticRandom.NextDouble() * (aptidaoAcumulada);
                }
                // seleciona dois individuos
                double limiteInferior = 0;
                Gene ind1 = new Gene(this.grafo.GetNumNodes());
                Gene ind2 = new Gene(this.grafo.GetNumNodes());

                foreach (DictionaryEntry ind in population)
                {
                    if (((Gene)ind.Value).valorAcumulado >= buffer && buffer >= limiteInferior)
                    {
                        ind1 = (Gene)ind.Value;
                    }
                    if (((Gene)ind.Value).valorAcumulado >= buffer2 && buffer2 >= limiteInferior)
                    {
                        ind2 = (Gene)ind.Value;
                    }
                    limiteInferior = ((Gene)ind.Value).valorAcumulado;
                }


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
            population = populationBuff;
        }

        // http://mnemstudio.org/genetic-algorithms-mutation.htm
        private void Mutation()
        {
            int gene = staticRandom.Next(population.Count); // seleciona individuo randomicamente

            Gene ind = (Gene)population[gene];
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
                populationBuff.Add(populationBuff.Count, ind2);
            }
            else
            {
                populationBuff.Add(populationBuff.Count, ind1);
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
                int coun2 = 0;
                for (int k = cut2; k < this.grafo.GetNumNodes(); k++)
                { 
                  if(son.individuo.Contains(buff2[coun2])==false)
                  {
                      son.individuo[k] = buff2[coun2];
                  }
                  if (son2.individuo.Contains(buff1[coun2]) == false)
                  {
                      son2.individuo[k] = buff1[coun2];
                  }
                  coun2++;
                }
                for (int k = 0; k < cut; k++)    
                {
                    if (son.individuo.Contains(buff2[coun2]) == false)
                    {
                        son.individuo[k] = buff2[coun2];
                    }
                    if (son2.individuo.Contains(buff1[coun2]) == false)
                    {
                        son2.individuo[k] = buff1[coun2];
                    }
                    coun2++;                    
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
                int buffer;

                for (int i = 0; i < this.grafo.GetNumNodes(); i += 1)
                {

                    bool NotFounded = false;
                    while (NotFounded == false)
                    {
                        buffer = staticRandom.Next(grafo.GetNumNodes());
                        if (newInd.ContainsNum(buffer) == false)
                        {
                            newInd.SetGen(i, buffer);
                            NotFounded = true;
                        }
                    }
                }
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
                    solution = ((Gene)gene.Value);
                    isFirst=true;
                }
                if (((Gene)gene.Value).avaliationValue < solution.avaliationValue)  // guarda o melhor gene
                {
                    solution = ((Gene)gene.Value);
                    EhDiferente = true;
                }
            }
          
            // normaliza a aptidão
            foreach (DictionaryEntry gene in this.population)
            {
                ((Gene)gene.Value).avaliationValueNormalizado = ((Gene)gene.Value).avaliationValue / solution.avaliationValue;
                aptidaoAcumulada = aptidaoAcumulada + ((Gene)gene.Value).avaliationValueNormalizado;   // faz o somatório de tudo (usado para seleção)   
                ((Gene)gene.Value).valorAcumulado = aptidaoAcumulada;
            }
            // ordenar hastable conforme avaliationValue --> acho q n precisa
            
           
        } 
    }
}