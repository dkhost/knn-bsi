﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.Kfixo
{
    class Program
    {
        public class Individuo
        {
            private string _classe;
            private double _a, _b, _c, _d;
            private bool _trocado;
            private bool _usado;
            private bool _errado;

            public Individuo(string classe, double a, double b, double c, double d)
            {
                _a = a;
                _b = b;
                _c = c;
                _d = d;
                _classe = classe;
                _trocado = false;
            }
            public double a {
                get
                {
                    return _a;
                }
                set
                {
                    _a = value;
                }
            }
            public double b
            {
                get
                {
                    return _b;
                }
                set
                {
                    _b = value;
                }
            }
            public double c
            {
                get
                {
                    return _c;
                }
                set
                {
                    _c = value;
                }
            }
            public double d
            {
                get
                {
                    return _d;
                }
                set
                {
                    _d = value;
                }
            }
            public string classe
            {
                get
                {
                    return _classe;
                }
                set
                {
                    _classe = value;
                }
            }
            public bool trocado
            {
                get
                {
                    return _trocado;
                }
                set
                {
                    _trocado = value;
                }
            }
            public bool usado
            {
                get
                {
                    return _usado;
                }
                set
                {
                    _usado = value;
                }
            }
            public bool errado
            {
                get
                {
                    return _errado;
                }
                set
                {
                    _errado = value;
                }
            }
        }

        public class Indicadores
        {
            private int _acertos;
            private int _erros;
            private double _percentAcerto;

            public Indicadores(int acertos, int erros, double percentAcerto)
            {
                _acertos = acertos;
                _erros = erros;
                _percentAcerto = percentAcerto;
            }

            public int acertos
            {
                get
                {
                    return _acertos;
                }
                set
                {
                    _acertos = value;
                }
            }
            public int erros
            {
                get
                {
                    return _erros;
                }
                set
                {
                    _erros = value;
                }
            }
            public double percentAcerto
            {
                get
                {
                    return _percentAcerto;
                }
                set
                {
                    _percentAcerto = value;
                }
            }
            
        }

        public static string[] CarregarDataBase()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\databaseGLM.txt");

            return lines;
        }

        public static List<Individuo> Colunas(string[] dataBase)
        {
            List<Individuo> individuos = new List<Individuo>();

            foreach (var dados in dataBase)
            {
                string[] colunas = dados.Split(',');
                string classe = colunas[0];
                double a = Convert.ToDouble(colunas[1]), b = Convert.ToDouble(colunas[2]), c = Convert.ToDouble(colunas[3]), d = Convert.ToDouble(colunas[4]);

                Individuo individuo = new Individuo(classe, a, b, c, d);
                individuos.Add(individuo);
            }
            return individuos;
        }
        public static double DistanciaEuclidiana(Individuo ind1, Individuo ind2)
        {
            
            double soma = Math.Pow((ind1.a - ind2.a), 2)
                + Math.Pow((ind1.b - ind2.b), 2)
                + Math.Pow((ind1.c - ind2.c), 2)
                + Math.Pow((ind1.d - ind2.d), 2);
            return Math.Sqrt(soma);
        }
        public static string[] ClassificarAmostras(List<Individuo> C1, List<Individuo> C2 , int k)
        {
            if(k%2 == 0)
            {
                k--;
                k = k <= 0 ? 1 : k;
            }
            var tam = C1.Count();
            string[] classesC1 = new string[tam];
            int posicao = 0;
            foreach (var elemento1 in C1)
            {
                var distanciaIndividuos = new List<KeyValuePair<double, Individuo>>();
                foreach (var elemento2 in C2)
                {
                    double distancia = DistanciaEuclidiana(elemento1, elemento2);
                    distanciaIndividuos.Add(new KeyValuePair<double, Individuo>(distancia, elemento2));
                }
                distanciaIndividuos.Sort((x,y) => x.Key.CompareTo(y.Key));

                int contadorL = 0, contadorR = 0, contadorB = 0;
                for (int i = 0; i < k; i++)
                {
                    string classe = distanciaIndividuos[i].Value.classe;

                    if (classe == "L")
                        contadorL++;
                    if (classe == "M")
                        contadorR++;
                    if (classe == "G")
                        contadorB++;
                }
                string classeClassificacao;
                if (contadorL >= contadorR && contadorL >= contadorB)
                    classeClassificacao = "L";
                else if (contadorR >= contadorL && contadorR >= contadorB)
                    classeClassificacao = "M";
                else
                    classeClassificacao = "G";
                classesC1[posicao] = classeClassificacao;
                posicao++;
                distanciaIndividuos = null;
            }
            return classesC1;
        }

        static void Main(string[] args)
        {
            List<Indicadores> ResultadoTestes = new List<Indicadores>();
            List<Individuo> individuos = new List<Individuo>();
            string[] database = CarregarDataBase();
            individuos = Colunas(database);
            int quantidadeIndividuos = individuos.Count();
            int K = 1;

            List<Individuo> ListL = new List<Individuo>();
            List<Individuo> ListM = new List<Individuo>();
            List<Individuo> ListG = new List<Individuo>();
            List<Individuo> Z1 = new List<Individuo>();
            List<Individuo> Z2 = new List<Individuo>();
            List<Individuo> Z3 = new List<Individuo>();

            Console.WriteLine("Database: GLM utilizando K fixo.\n");
            for (int contador = 1; contador <= 20; contador++)
            { 
            int acertos = 0, erros = 0;
            double percentAcerto = 0;

                foreach (var indv in individuos)
            {
                if (indv.classe == "L")
                {
                    ListL.Add(indv);
                    continue;
                }
                if (indv.classe == "M")
                {
                    ListM.Add(indv);
                    continue;
                }
                if (indv.classe == "G")
                {
                    ListG.Add(indv);
                    continue;
                }
            }
            
            Random numRand = new Random();
            Individuo AuxAdd;
            
                while (Z1.Count() < 72)
            {
                AuxAdd = ListL.ElementAt(numRand.Next(ListL.Count() - 1));
                if (!AuxAdd.usado)
                {
                    AuxAdd.usado = true;
                    Z1.Add(AuxAdd);
                }
            }
             
            while (Z2.Count() < 72)
            {
                
                AuxAdd = ListL.ElementAt(numRand.Next(ListL.Count() - 1));
                if (!AuxAdd.usado)
                {
                    AuxAdd.usado = true;
                    Z2.Add(AuxAdd);
                }
            }
                
            while (Z3.Count() < 144)
            {
                AuxAdd = ListL.Where(c => c.usado == false).First();
                AuxAdd.usado = true;
                Z3.Add(AuxAdd);
            }
           
            while (Z1.Count() < 84)
            {
                AuxAdd = ListG.ElementAt(numRand.Next(ListG.Count() - 1));
                if (!AuxAdd.usado)
                {
                    AuxAdd.usado = true;
                    Z1.Add(AuxAdd);
                }
            }
            
            while (Z2.Count() < 84)
            {
                AuxAdd = ListG.ElementAt(numRand.Next(ListG.Count() - 1));
                if (!AuxAdd.usado)
                {
                    AuxAdd.usado = true;
                    Z2.Add(AuxAdd);
                }
            }
            
            while (Z3.Count() < 169)
            {
                AuxAdd = ListG.Where(c => c.usado == false).First();
                AuxAdd.usado = true;
                Z3.Add(AuxAdd);
            }
          
            while (Z1.Count() < 156)
            {
                AuxAdd = ListM.ElementAt(numRand.Next(ListM.Count() - 1));
                if (!AuxAdd.usado)
                {
                    AuxAdd.usado = true;
                    Z1.Add(AuxAdd);
                }
            }
            
            while (Z2.Count() < 156)
            {
                AuxAdd = ListM.ElementAt(numRand.Next(ListM.Count() - 1));
                if (!AuxAdd.usado)
                {
                    AuxAdd.usado = true;
                    Z2.Add(AuxAdd);
                }
            }
            
            while (Z3.Count() < 313)
            {
                    AuxAdd = ListM.Where(c => c.usado == false).First();
                    AuxAdd.usado = true;
                    Z3.Add(AuxAdd);
            }
            
            string[] classeObtida = ClassificarAmostras(Z1, Z2, K);
            int a = 0;
            Individuo auxTroca = null, auxTroca2 = null;

            foreach (var qntAcertos in Z1)
            {
                if (qntAcertos.classe != classeObtida[a])
                {
                    qntAcertos.errado = true;
                }
                a++;
            }
            
            while (Z1.Any(e => e.errado == true))
            {
                auxTroca = Z1.Where(e => e.errado == true).First();
                auxTroca2 = Z2.Where(c => c.classe == auxTroca.classe).First();
                Z1.Remove(auxTroca);
                Z2.Remove(auxTroca2);
                auxTroca.trocado = true;
                auxTroca.errado = false;
                auxTroca2.trocado = true;
                Z1.Add(auxTroca2);
                Z2.Add(auxTroca);
            }
            
            foreach (var limpZ1 in Z1)
            {
                    limpZ1.trocado = false;
                    limpZ1.usado = false;
            }

            foreach (var limpZ2 in Z2)
            {
                    limpZ2.trocado = false;
                    limpZ2.usado = false;
            }

            classeObtida = ClassificarAmostras(Z3, Z2, K);

             a = 0;
            foreach (var qntAcertos in Z3)
            {
                if (qntAcertos.classe == classeObtida[a])
                {
                    acertos++;
                }
                else
                {
                    erros++;
                }

                a++;
            }
                
             percentAcerto = (acertos * 100) / Z3.Count();
            Indicadores indicador = new Indicadores(acertos, erros, percentAcerto);
            ResultadoTestes.Add(indicador);


                Console.WriteLine("Tentativa: " + contador + "\n" + "Porcentagem de acerto: " + percentAcerto + "%");
            }
            Console.WriteLine("\n");

            double soma = 0, media, desvioPadrao;
            foreach(var baseDeCalculo in ResultadoTestes)
            {
                soma += baseDeCalculo.percentAcerto;
            }
            media = soma / ResultadoTestes.Count();
            soma = 0;
            foreach(var baseDeCalculo in ResultadoTestes)
            {
                soma += Math.Pow((baseDeCalculo.percentAcerto - media), 2);
            }
            desvioPadrao = Math.Sqrt(soma / ResultadoTestes.Count());
            Console.WriteLine("Media:" + media + "%");
            Console.WriteLine("Desvio Padrão:" + desvioPadrao);

            Console.ReadKey();

        }
    }
}