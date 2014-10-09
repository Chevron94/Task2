using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task2
{
    class Solver
    {
        double[,] matr;
        double[] result;
        double[] x;
        double[] f;
        int n;
        int l;
        public Solver()
        {
        }
        public double[] Solve()
        {
            return null;
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
            result = new double[n];
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
                    if (tmp < 5)
                    {
                        f[i] += matr[i, j] * x[tmp];
                        tmp++;
                    }
                }
                int q = 0;
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
        public void Form_Answer(int test, int N, int L, ref double avg)
        {
            n = 5;
            l = 2;
            Generate(n, l);
        }
    }
}
