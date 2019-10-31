using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace DesenhoRota
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Rota NossaRota;
        private void Button1_Click(object sender, EventArgs e)
        {
            int NumeroDePontos = 100;
            
            NossaRota = new Rota(NumeroDePontos);
            NossaRota.GerarCoordenadasAleatorias(2);
            NossaRota.CalculaCustosPorDistanciaEuclidiana();
            Stopwatch Cronometro = new Stopwatch();
            Cronometro.Start();
            NossaRota.InsercaoMaisEconomica();
            //NossaRota.Melhoria2optCiclo();
            //Desenhar(CoordenadasX,CoordenadasY,NossaRota.Sequencia);
            //NossaRota.CicloMelhoriaTrocaSimples2pontos();
            NossaRota.CicloMelhoriaTrocaSimples2pontos();
            NossaRota.Melhoria3optCompleta();
            NossaRota.Melhoria2optCompleta();
            //NossaRota.Melhoria2optCompleta();
            //NossaRota.Melhoria3optCompleta();
            Cronometro.Stop();
            MessageBox.Show("Os números de ciclos e melhorias 2opt foram, respectivamente: " + NossaRota.NumeroDeCiclos2opt.ToString() + " e " + NossaRota.NumeroDeMelhorias2opt.ToString());
            MessageBox.Show("Os números de ciclos e melhorias 3opt foram, respectivamente: " + NossaRota.NumeroDeCiclos3opt.ToString() + " e " + NossaRota.NumeroDeMelhorias3opt.ToString());
            MessageBox.Show("O tempo de resolução foi de " + Cronometro.ElapsedMilliseconds.ToString());
            double CustoTotal = NossaRota.CalculaCustoSequenciaAtual();
            MessageBox.Show("O custo total foi " + CustoTotal.ToString());
            Desenhar(NossaRota.CoordenadasX, NossaRota.CoordenadasY, NossaRota.Sequencia);
        }
        Graphics g;
        public void Desenhar(double[] CoordX, double[] CoordY, int[] Seq)
        {
            
            Pen CanetaNo;
            Pen CanetaAresta;
            Brush Pincel;
            int Raio;
            g = pictureBox1.CreateGraphics();
            //g.Clear(Color.White);
            CanetaNo = new Pen(Color.Blue);
            CanetaAresta = new Pen(Color.Gray);
            Raio = 4;
            for (int i = 0; i < CoordX.GetLength(0); i++)
            {
                //g.DrawEllipse(CanetaNo, (float)CoordX[i] - Raio, (float)CoordY[i] - Raio, Raio * 2, Raio * 2);
                g.FillEllipse(Brushes.Blue, (float)CoordX[i] - Raio, (float)CoordY[i] - Raio, Raio * 2, Raio * 2);
            }
            for (int i = 0; i < Seq.GetLength(0)-1; i++)
            {
                g.DrawLine(CanetaAresta, (float)CoordX[Seq[i]], (float)CoordY[Seq[i]], (float)CoordX[Seq[i+1]], (float)CoordY[Seq[i+1]]);
            }  
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Random Aleatorio = new Random(1);
            Stopwatch Cronometro = new Stopwatch();
            Cronometro.Start();
            for(int i=0;i<10000000;i++)
            {
                int a = Aleatorio.Next(2, 100000);
                double b = Math.Pow(a, 3);
                //double b = a * a * a;
            }
            Cronometro.Stop();
            MessageBox.Show(Cronometro.ElapsedMilliseconds.ToString());
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            CaminhoMinimo aaa = new CaminhoMinimo(20);
            aaa.GerarDistanciasAleatorias(1);
            aaa.InicializarMatrizPredecessor();
            aaa.AlgoritmoFloyd();
            aaa.TrajetoriaEntre(2, 7);
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            lblCoordenadas.Text = e.X.ToString() + " " + e.Y.ToString();
            Desenhar(NossaRota.CoordenadasX, NossaRota.CoordenadasY, NossaRota.Sequencia);
            int PontoMaisProximo=-1;
            double DistanciaMaisProximo = double.MaxValue;
            for (int i = 0; i < NossaRota.TamanhoAtual;i++)
            {
                if(DistanciaMaisProximo>NossaRota.CalculaDistanciaEuclidiana(e.X, NossaRota.CoordenadasX[i],e.Y, NossaRota.CoordenadasY[i]))
                {
                    DistanciaMaisProximo = NossaRota.CalculaDistanciaEuclidiana(e.X, NossaRota.CoordenadasX[i], e.Y, NossaRota.CoordenadasY[i]);
                    PontoMaisProximo = i;
                }
            }
            int Raio = 7;
            //Desenhar();
            g.FillEllipse(Brushes.Red, (float)NossaRota.CoordenadasX[PontoMaisProximo] - Raio, (float)NossaRota.CoordenadasY[PontoMaisProximo] - Raio, Raio * 2, Raio * 2);
        }
    }
}
