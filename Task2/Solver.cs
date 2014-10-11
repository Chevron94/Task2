using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        private int K0_BC(int i)
        {
            if (i < l - 1)
                return l - i - 1;
            else
                return 0;
        }

        private int Kn_BC(int i)
        {
            if (i <= n - l)
                return 2 * l - 2;
            else
                return n - 2 - (i - l);
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
                    matr[j,i] =rnd.Next(-10, 10);
                }
                k++;
            }
            for (int i = 0; i<n; i++)
                x[i] =  rnd.Next(-10, 10);

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

            Thread.Sleep(1);
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

        double GetValue(int i, int j)
        {
            if (i >= j)
            {
                return matr[j, i-j];
            }
            return matr[i, j-i];
        }

        private double[] Solve()
        {
            double[,] bc = new double[n,2*l-1];
            // bc - OK!
            for (int j = 0; j < n; j++)
            {
                int kn = Kn(j);
                for (int i = j; i <= kn; i++)
                {
                    double s = 0;
                    int k0 = K0(i);
                    for (int k = k0; k < j ; k++)
                    {
                        s += bc[i, k - i + l - 1] * bc[k, j - k + l - 1];
                    }
                    bc[i,j - i + l - 1] = GetValue(i,j) - s;
                }
                
                for (int i = j+1; i <= kn; i++)
                {
                    double s = 0;
                    int k0 = K0(j);
                    for (int k = k0; k <=j - 1; k++)
                    {
                        if ((l + i - k - 1) < 2 * l - 1)
                            s += bc[j, k - j + l - 1] * bc[k, l + i - k - 1];
                    }
                    if (bc[j, l - 1] == 0)
                        return null;
                    bc[j, i - j + l - 1] = (GetValue(j,i) - s) / bc[j,l - 1];
                }
                 
            }
            // BY = F
            for (int i = 0; i < n; i++)
            {
                for (int j = K0_BC(i); j <= l - 2; j++)
                {
                    f[i] -= f[i - (l - 1) + j] * bc[i, j];
                    bc[i, j] = 0;
                }
                if (bc[i, l - 1] == 0)
                    return null;
                f[i] /= bc[i, l - 1];
                bc[i, l - 1] = 1;
            }
            // AX = Y
            for (int i = n - 2; i >= 0; i--)
                for (int j = l; j <= Kn_BC(i); j++)
                {
                    f[i] -= f[j - l + i + 1] * bc[i, j];
                    bc[i, j] = 0;
                }
            return f;
        }

        public void Form_Answer(int test, int N, int L, ref double avg)
        {
            n = N;
            l = L;
            avg = 0;
            for (int i = 0; i < test; i++)
            {
                Generate(n, l);
                while (Solve() == null)
                {
                    Generate(n, l);
                }
                avg += Get_Avg(f, x);
            }
            avg /= test;
        }
    }
}
