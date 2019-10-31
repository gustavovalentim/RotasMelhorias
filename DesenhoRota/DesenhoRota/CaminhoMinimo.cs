using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesenhoRota
{
    class CaminhoMinimo
    {
        public int Dimensao;
        public double[,] MatrizDistancias;
        public int[,] MatrizPredecessor;
        public CaminhoMinimo(int Tamanho)
        {
            Dimensao = Tamanho;
            MatrizDistancias = new double[Tamanho, Tamanho];
            MatrizPredecessor = new int[Tamanho, Tamanho];
        }
        public void GerarDistanciasAleatorias(int semente)
        {
            Random Aleatorio = new Random(semente);
            for (int i = 0; i < Dimensao; i++)
            {
                for (int j = 0; j < Dimensao; j++)
                {
                    MatrizDistancias[i, j] = Aleatorio.Next(10, 300);
                }
            }
        }
        public void InicializarMatrizPredecessor()
        {
            for (int i = 0; i < Dimensao; i++)
            {
                for (int j = 0; j < Dimensao; j++)
                {
                    if (MatrizDistancias[i, j] < 100000)
                    {
                        MatrizPredecessor[i, j] = i;
                    }
                }
            }
        }
        public void AlgoritmoFloyd()
        {
            for (int k = 0; k < Dimensao; k++)
            {
                for (int i = 0; i < Dimensao; i++)
                {
                    for (int j = 0; j < Dimensao; j++)
                    {
                        if (k != i && k != j && i != j)
                        {
                            if (MatrizDistancias[i, k] + MatrizDistancias[k, j] < MatrizDistancias[i, j])
                            {
                                MatrizDistancias[i, j] = MatrizDistancias[i, k] + MatrizDistancias[k, j];
                                MatrizPredecessor[i, j] = MatrizPredecessor[k, j];
                            }
                        }
                    }
                }
            }
        }
        public int[] TrajetoriaEntre(int pontoA, int pontoB)
        {
            int[] Trajetoria;
            int predecessor = pontoB;
            int QtdPredecessores = 0;
            while(MatrizPredecessor[pontoA,predecessor] != pontoA)
            {
                QtdPredecessores++;
                predecessor = MatrizPredecessor[pontoA, predecessor];
            }
            Trajetoria = new int[QtdPredecessores+2];
            Trajetoria[QtdPredecessores + 1] = pontoB;
            predecessor = pontoB;
            while (MatrizPredecessor[pontoA, predecessor] != pontoA)
            {
                predecessor = MatrizPredecessor[pontoA, predecessor];
                Trajetoria[QtdPredecessores] = predecessor;
                QtdPredecessores--;
            }
            Trajetoria[0] = pontoA;
            return Trajetoria;
        }
    }
}
