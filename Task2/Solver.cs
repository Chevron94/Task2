using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task2
{
    class Solver
    {
        double[,] matr;
        double[] x;
        double[] f;
        int n;
        int l;
        public Solver()
        {
        }
        private int K0(int i)
        {
            if (i < l)
                return 0;
            return i - l+1;
        }

        private int Kn(int i)
        {
            if (i > n - l)
                return n - 1;
            return i + l - 1;
        }

        public void Generate(int n, int l)
        {
            matr = new double[n, l];
            x = new double[n];
            f = new double[n];
            Random rnd = new Random();
            int k = 0;
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < n - k; j++)
                {
                    matr[j,i] =  rnd.Next(-10, 10);
                }
                k++;
            }
            for (int i = 0; i<n; i++)
                x[i] = rnd.Next(-10, 10);

            for (int i = 0; i < n; i++)
            {
                int k0 = K0(i);
                int kn = Kn(i);
                int tmp = k0;
                for (int t = i - l + 1; t < i; t++)
                {
                    if (t >= 0)
                    {
                        f[i] += matr[t, l + t - i] * x[tmp];
                        tmp++;
                    }
                }
                for (int j = 0; j < l; j++)
                {
                    if (tmp < n)
                    {
                        f[i] += matr[i, j] * x[tmp];
                        tmp++;
                    }
                }
            }

        }
        private double Get_Avg(double[] solution, double[] x) // Вычисление погрешности
        {
            double res = 0;
            for (int i = 0; i < n; i++)
            {
                if (x[i] != 0)
                    res = Math.Max(res, Math.Abs(solution[i] - x[i]) / x[i]);
                else res = Math.Max(res, Math.Abs(solution[i] - x[i]));
            }
            return res;
        }

        private double[] Solve()
        {
            double[,] bc = new double[n,2*l-1];
            for (int i = 0; i< n; i++)
            {

            }
            for (int j = 0; j < n; j++)
            {
                for (int i = j; j <= Kn(j); j++)
                {
                    double s = matr[i, i + j];
                    for (int k = K0(i); k < j - 1; j++)
                    {
                       
                    }
                }
            }
            return null;
        }

        public void Form_Answer(int test, int N, int L, ref double avg)
        {
            n = N;
            l = L;
            avg = 0;
            for (int i = 0; i < test; i++)
            {
                Generate(n, 10);
                while (Solve() == null)
                {
                    Generate(n, 10);
                }
                avg += Get_Avg(f, x);
            }
            avg /= test;
        }
    }
}
