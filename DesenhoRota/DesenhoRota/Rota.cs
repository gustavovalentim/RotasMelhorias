using System;
using System.Collections.Generic;
using System.Text;

namespace DesenhoRota
{
    class Rota
    {
        public int TamanhoMaximo;
        public int TamanhoAtual;
        public int[] Sequencia;
        public double[,] MatrizCustos;
        public double[,] MatrizTempos;
        public List<int> ForaDaSequencia;
        public bool TeveMelhoria2opt;
        public int NumeroDeCiclos2opt;
        public int NumeroDeMelhorias2opt;
        public bool TeveMelhoria3opt;
        public int NumeroDeCiclos3opt;
        public int NumeroDeMelhorias3opt;
        public double[] CoordenadasX;
        public double[] CoordenadasY;
        public Rota(int tamMaximo)
        {
            TamanhoMaximo = tamMaximo;
            MatrizCustos = new double[tamMaximo, tamMaximo];
            Sequencia = new int[tamMaximo + 1];
            ForaDaSequencia = new List<int>();
            NumeroDeCiclos2opt = 0;
            NumeroDeMelhorias2opt = 0;
            NumeroDeCiclos3opt = 0;
            NumeroDeMelhorias3opt = 0;
            for (int i=0;i<tamMaximo;i++)
            {
                ForaDaSequencia.Add(i);
            }
        }
        public void GerarCoordenadasAleatorias(int semente)
        {
            CoordenadasX = new double[TamanhoMaximo];
            CoordenadasY = new double[TamanhoMaximo];
            Random Aleatorio = new Random(semente);
            for (int i = 0; i < TamanhoMaximo; i++)
            {
                CoordenadasX[i] = Aleatorio.Next(20, 380);
                CoordenadasY[i] = Aleatorio.Next(20, 380);
            }
        }
        public void CalculaCustosPorDistanciaEuclidiana()
        {
            for (int i=0;i<TamanhoMaximo;i++)
            {
                for (int j=0;j<TamanhoMaximo;j++)
                {
                    MatrizCustos[i, j] = CalculaDistanciaEuclidiana(CoordenadasX[j], CoordenadasX[i], CoordenadasY[j], CoordenadasY[i]);
                }
            }
        }
        public double CalculaDistanciaEuclidiana(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
        public void IniciarSequencia()
        {
            string ArcoInicial = EncontraMenorArco();
            string[] s = ArcoInicial.Split('_');
            Sequencia[0] = int.Parse(s[0]);
            Sequencia[1] = int.Parse(s[1]);
            Sequencia[2] = int.Parse(s[0]);
            TamanhoAtual = 2;
            ForaDaSequencia.Remove(int.Parse(s[0]));
            ForaDaSequencia.Remove(int.Parse(s[1]));
        }
        public string EncontraMenorArco()
        {
            double MenorDistancia = double.MaxValue;
            string ArcoMenorDistancia = "";
            for (int i=0;i<TamanhoMaximo;i++)
            {
                for (int j=0;j<TamanhoMaximo;j++)
                {
                    if(i!=j)
                    {
                        if (MatrizCustos[i, j] < MenorDistancia)
                        {
                            MenorDistancia = MatrizCustos[i, j];
                            ArcoMenorDistancia = i.ToString() + "_" + j.ToString();
                        }
                    }
                }
            }
            return ArcoMenorDistancia;
        }
        public void InsercaoPonto(int ponto, int posicao)
        {
            for(int i=TamanhoAtual;i>=posicao;i--)
            {
                Sequencia[i + 1] = Sequencia[i];
            }
            Sequencia[posicao] = ponto;
            TamanhoAtual++;
            ForaDaSequencia.Remove(ponto);
        }
        public double CustoInsercao(int ponto, int posicao)
        {
            // 1 - 4 - 8 - 3 - 1
            // ponto = 6; posicao = 2;
            // 1 - 4 - 6 - 8 - 3 - 1
            // Custo[4,6]+Custo[6,8]-Custo[4,8]
            return MatrizCustos[Sequencia[posicao - 1], ponto] + MatrizCustos[ponto, Sequencia[posicao]] - MatrizCustos[Sequencia[posicao - 1], Sequencia[posicao]];
        }
        public int MelhorLugarParaInsercao(int ponto)
        {
            double MenorCustoInsercao=double.MaxValue;
            int PosicaoInserir = -1;
            double CustoAtual;
            for (int i=0;i<TamanhoAtual;i++)
            {
                CustoAtual = CustoInsercao(ponto, i);
                if (CustoAtual<MenorCustoInsercao)
                {
                    MenorCustoInsercao = CustoAtual;
                    PosicaoInserir = i;
                }
            }
            return 0;
        }
        public int MelhorLugarParaInsercaoRemovendoP(int ponto, int p)
        {
            double MenorCustoInsercao = double.MaxValue;
            int PosicaoInserir = -1;
            double CustoAtual;
            for (int i = 0; i < TamanhoAtual; i++)
            {
                CustoAtual = CustoInsercao(ponto, i);
                if (CustoAtual < MenorCustoInsercao)
                {
                    MenorCustoInsercao = CustoAtual;
                    PosicaoInserir = i;
                }
            }
            return 0;
        }
        public void InsercaoMaisEconomica()
        {
            IniciarSequencia();
            while (ForaDaSequencia.Count>0)
            {
                int PontoParaInsercao=-1;
                int PosicaoParaInsercao=-1;
                double CustoAtualInsercao = double.MaxValue;
                double MaisEconomica = double.MaxValue;
                foreach (int elemento in ForaDaSequencia)
                {
                    for(int i=1;i<TamanhoAtual;i++)
                    {
                        CustoAtualInsercao = CustoInsercao(elemento, i); 
                        if (CustoAtualInsercao<MaisEconomica)
                        {
                            MaisEconomica = CustoAtualInsercao;
                            PontoParaInsercao = elemento;
                            PosicaoParaInsercao = i;
                        }
                    }
                }
                InsercaoPonto(PontoParaInsercao, PosicaoParaInsercao);
            }
        }
        public void Melhoria2opt(int PosicaoA, int PosicaoB)
        {
            InverteSentido(PosicaoA + 1, PosicaoB);
            TeveMelhoria2opt = true;
            NumeroDeMelhorias2opt++;
        }
        public void InverteSentido(int posicaoInicio, int posicaoFim)
        {
            Array.Reverse(Sequencia, posicaoInicio, posicaoFim-posicaoInicio+1);
        }
        public double Economia2opt(int posicaoA, int posicaoB)
        {
            double CustoAntigo = 0;
            double CustoNovo = 0;
            CustoAntigo = CustoCaminho(posicaoA, posicaoB + 1);
            CustoNovo += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoB]];
            CustoNovo += CustoCaminhoReverso(posicaoA + 1, posicaoB);
            CustoNovo += MatrizCustos[Sequencia[posicaoA + 1], Sequencia[posicaoB + 1]];
            return CustoAntigo - CustoNovo-0.0001;
        }
        public double CustoCaminho(int posicaoInicio, int posicaoFim)
        {
            double Custo = 0;
            for (int i = posicaoInicio; i < posicaoFim; i++)
            {
                Custo += MatrizCustos[Sequencia[i], Sequencia[i + 1]];
            }
            return Custo;
        }
        public double CustoCaminhoReverso(int posicaoInicio, int posicaoFim)
        {
            double Custo = 0;
            for (int i = posicaoFim; i > posicaoInicio; i--)
            {
                Custo += MatrizCustos[Sequencia[i], Sequencia[i - 1]];
            }
            return Custo;
        }
        public void Melhoria2optCiclo()
        {
            for (int i=0;i<TamanhoAtual;i++)
            {
                for(int j=i+1;j<TamanhoAtual;j++)
                {
                    if(Economia2opt(i, j)>0)
                    {
                        Melhoria2opt(i, j);
                    }
                }
            }
        }
        public void Melhoria2optCompleta()
        {
            TeveMelhoria2opt = true;
            while(TeveMelhoria2opt)
            {
                TeveMelhoria2opt = false;
                Melhoria2optCiclo();
                NumeroDeCiclos2opt++;
            }
        }
        public double Economia3optCaso1(int posicaoA, int posicaoB, int posicaoC)
        {
            double CustoAntigo = 0;
            double CustoNovo = 0;
            CustoAntigo += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoA + 1]];
            CustoAntigo += CustoCaminho(posicaoA + 1, posicaoB);
            CustoAntigo += MatrizCustos[Sequencia[posicaoB], Sequencia[posicaoB + 1]];
            CustoAntigo += MatrizCustos[Sequencia[posicaoC], Sequencia[posicaoC + 1]];
            CustoNovo += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoB + 1]];
            CustoNovo += MatrizCustos[Sequencia[posicaoC], Sequencia[posicaoB]];
            CustoNovo += CustoCaminhoReverso(posicaoA + 1, posicaoB);
            CustoNovo += MatrizCustos[Sequencia[posicaoA+1], Sequencia[posicaoC + 1]];
            return CustoAntigo - CustoNovo - 0.0001;
        }
        public double Economia3optCaso2(int posicaoA, int posicaoB, int posicaoC)
        {
            double CustoAntigo = 0;
            double CustoNovo = 0;
            CustoAntigo += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoA + 1]];
            CustoAntigo += CustoCaminho(posicaoA + 1, posicaoB);
            CustoAntigo += MatrizCustos[Sequencia[posicaoB], Sequencia[posicaoB + 1]];
            CustoAntigo += CustoCaminho(posicaoB + 1, posicaoC);
            CustoAntigo += MatrizCustos[Sequencia[posicaoC], Sequencia[posicaoC + 1]];
            CustoNovo += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoB]];
            CustoNovo += CustoCaminhoReverso(posicaoA + 1, posicaoB);
            CustoNovo += MatrizCustos[Sequencia[posicaoA+1], Sequencia[posicaoC]];
            CustoNovo += CustoCaminhoReverso(posicaoB + 1, posicaoC);
            CustoNovo += MatrizCustos[Sequencia[posicaoB + 1], Sequencia[posicaoC + 1]];
            return CustoAntigo - CustoNovo - 0.0001;
        }
        public double Economia3optCaso3(int posicaoA, int posicaoB, int posicaoC)
        {
            double CustoAntigo = 0;
            double CustoNovo = 0;
            CustoAntigo += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoA + 1]];
            CustoAntigo += MatrizCustos[Sequencia[posicaoB], Sequencia[posicaoB + 1]];
            CustoAntigo += CustoCaminho(posicaoB + 1, posicaoC);
            CustoAntigo += MatrizCustos[Sequencia[posicaoC], Sequencia[posicaoC + 1]];
            CustoNovo += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoC]];
            CustoNovo += CustoCaminhoReverso(posicaoB + 1, posicaoC);
            CustoNovo += MatrizCustos[Sequencia[posicaoB + 1], Sequencia[posicaoA+1]];
            CustoNovo += MatrizCustos[Sequencia[posicaoB], Sequencia[posicaoC + 1]];
            return CustoAntigo - CustoNovo - 0.0001;
        }
        public double Economia3optCaso4(int posicaoA, int posicaoB, int posicaoC)
        {
            double CustoAntigo = 0;
            double CustoNovo = 0;
            CustoAntigo += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoA + 1]];
            CustoAntigo += MatrizCustos[Sequencia[posicaoB], Sequencia[posicaoB + 1]];
            CustoAntigo += MatrizCustos[Sequencia[posicaoC], Sequencia[posicaoC + 1]];
            CustoNovo += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoB+1]];
            CustoNovo += MatrizCustos[Sequencia[posicaoC], Sequencia[posicaoA + 1]];
            CustoNovo += MatrizCustos[Sequencia[posicaoB], Sequencia[posicaoC + 1]];
            return CustoAntigo - CustoNovo - 0.0001;
        }
        public void Melhoria3optCaso1(int posicaoA, int posicaoB, int posicaoC)
        {
            InverteSentido(posicaoA + 1, posicaoB);
            int[] SequenciaDeslocada = new int[posicaoC - posicaoB];
            for(int i=0;i<posicaoC-posicaoB;i++)
            {
                SequenciaDeslocada[i] = Sequencia[posicaoB + 1 + i];
            }
            for(int i=0;i<posicaoB-posicaoA;i++)
            {
                Sequencia[posicaoC-i] = Sequencia[posicaoB-i];
            }
            for(int i=0;i<posicaoC-posicaoB;i++)
            {
                Sequencia[posicaoA + 1 + i] = SequenciaDeslocada[i];
            }
        }
        public void Melhoria3optCaso2(int posicaoA, int posicaoB, int posicaoC)
        {
            InverteSentido(posicaoA + 1, posicaoB);
            InverteSentido(posicaoB + 1, posicaoC);
        }
        public void Melhoria3optCaso3(int posicaoA, int posicaoB, int posicaoC)
        {
            InverteSentido(posicaoB + 1, posicaoC);
            int[] SequenciaDeslocada = new int[posicaoC - posicaoB];
            for (int i = 0; i < posicaoC - posicaoB; i++)
            {
                SequenciaDeslocada[i] = Sequencia[posicaoB + 1 + i];
            }
            for (int i = 0; i < posicaoB - posicaoA; i++)
            {
                Sequencia[posicaoC - i] = Sequencia[posicaoB - i];
            }
            for (int i = 0; i < posicaoC - posicaoB; i++)
            {
                Sequencia[posicaoA + 1 + i] = SequenciaDeslocada[i];
            }
        }
        public void Melhoria3optCaso4(int posicaoA, int posicaoB, int posicaoC)
        {
            int[] SequenciaDeslocada = new int[posicaoC - posicaoB];
            for (int i = 0; i < posicaoC - posicaoB; i++)
            {
                SequenciaDeslocada[i] = Sequencia[posicaoB + 1 + i];
            }
            for (int i = 0; i < posicaoB - posicaoA; i++)
            {
                Sequencia[posicaoC - i] = Sequencia[posicaoB - i];
            }
            for (int i = 0; i < posicaoC - posicaoB; i++)
            {
                Sequencia[posicaoA + 1 + i] = SequenciaDeslocada[i];
            }
        }
        public void ProcessoMelhoria3optCaso1(int i, int j, int k)
        {
            if (Economia3optCaso1(i, j, k) > 0)
            {
                Melhoria3optCaso1(i, j, k);
                NumeroDeMelhorias3opt++;
                TeveMelhoria3opt = true;
                //Melhoria2optCompleta();
            }
        }
        public void ProcessoMelhoria3optCaso2(int i, int j, int k)
        {
            if (Economia3optCaso2(i, j, k) > 0)
            {
                Melhoria3optCaso2(i, j, k);
                NumeroDeMelhorias3opt++;
                TeveMelhoria3opt = true;
                //Melhoria2optCompleta();
            }
        }
        public void ProcessoMelhoria3optCaso3(int i, int j, int k)
        {
            if (Economia3optCaso3(i, j, k) > 0)
            {
                Melhoria3optCaso3(i, j, k);
                NumeroDeMelhorias3opt++;
                TeveMelhoria3opt = true;
                //Melhoria2optCompleta();
            }
        }
        public void ProcessoMelhoria3optCaso4(int i, int j, int k)
        {
            if (Economia3optCaso4(i, j, k) > 0)
            {
                Melhoria3optCaso4(i, j, k);
                NumeroDeMelhorias3opt++;
                TeveMelhoria3opt = true;
                //Melhoria2optCompleta();
            }
        }
        public void Melhoria3optCiclo()
        {
            double bbbb = CalculaCustoSequenciaAtual();
            for (int i = 0; i < TamanhoAtual; i++)
            {
                for (int j = i + 2; j < TamanhoAtual; j++)
                {
                    for(int k=j+2;k<TamanhoAtual;k++)
                    {
                        double MaiorEconomia = -double.MaxValue;
                        ProcessoMelhoria3optCaso1(i, j, k);
                        ProcessoMelhoria3optCaso2(i, j, k);
                        ProcessoMelhoria3optCaso3(i, j, k);
                        ProcessoMelhoria3optCaso4(i, j, k);
                    }
                }
            }
        }
        public void Melhoria3optCompleta()
        {
            TeveMelhoria3opt = true;
            while (TeveMelhoria3opt)
            {
                TeveMelhoria3opt = false;
                Melhoria3optCiclo();
                NumeroDeCiclos3opt++;
            }
        }
        public double EconomiaTrocaSimples2pontos(int posicaoA, int posicaoB)
        {
            double Economia = 0;
            Economia += MatrizCustos[Sequencia[posicaoA - 1], Sequencia[posicaoA]];
            Economia += MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoA+1]];
            Economia += MatrizCustos[Sequencia[posicaoB - 1], Sequencia[posicaoB]];
            Economia += MatrizCustos[Sequencia[posicaoB], Sequencia[posicaoB+1]];
            Economia -= MatrizCustos[Sequencia[posicaoA - 1], Sequencia[posicaoB]];
            Economia -= MatrizCustos[Sequencia[posicaoB], Sequencia[posicaoA+1]];
            Economia -= MatrizCustos[Sequencia[posicaoB - 1], Sequencia[posicaoA]];
            Economia -= MatrizCustos[Sequencia[posicaoA], Sequencia[posicaoB+1]];
            //Economia -= 0.0001;
            return Economia;
        }
        public void MelhoriaTrocaSimples2pontos(int posicaoA, int posicaoB)
        {
            int PontoCopia = Sequencia[posicaoA];
            Sequencia[posicaoA] = Sequencia[posicaoB];
            Sequencia[posicaoB] = PontoCopia;
        }
        public void CicloMelhoriaTrocaSimples2pontos()
        {
            for(int i=1; i<TamanhoAtual;i++)
            {
                for(int j=i+2; j<TamanhoAtual;j++)
                {
                    if(EconomiaTrocaSimples2pontos(i,j)>0)
                    {
                        MelhoriaTrocaSimples2pontos(i, j);
                    }
                }
            }
        }
        public double CalculaCustoSequenciaAtual()
        {
            double CustoTotal = 0;
            for (int i = 0; i < TamanhoAtual; i++)
            {
                CustoTotal += MatrizCustos[Sequencia[i], Sequencia[i + 1]];
            }
            return CustoTotal;
        }
    }
}
