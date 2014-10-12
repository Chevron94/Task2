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
        int L;
        public Solver()
        {
        }
        private int K0(int i)
        {
            if (i < L)
                return 0;
            return i - L+1;
        }

        private int Kn(int i)
        {
            if (i > n - L)
                return n - 1;
            return i + L - 1;
        }

        private int K0_BC(int i)
        {
            if (i < L - 1)
                return L - i - 1;
            else
                return 0;
        }

        private int Kn_BC(int i)
        {
            if (i <= n - L)
                return 2 * L - 2;
            else
                return n - 2 - (i - L);
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
            double[,] bc = new double[n,2*L-1];
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
                        if ((k - i + L - 1) >= 0)
                            s += bc[i, k - i + L - 1] * bc[k, j - k + L - 1];
                    }
                    bc[i,j - i + L - 1] = GetValue(i,j) - s;
                }
                
                for (int i = j+1; i <= kn; i++)
                {
                    double s = 0;
                    int k0 = K0(j);
                    for (int k = k0; k <=j - 1; k++)
                    {
                        if ((L + i - k - 1) < 2 * L - 1)
                            s += bc[j, k - j + L - 1] * bc[k, L + i - k - 1];
                    }
                    if (bc[j, L - 1] == 0)
                        return null;
                    bc[j, i - j + L - 1] = (GetValue(j,i) - s) / bc[j,L - 1];
                }
                 
            }
            // BY = F
            for (int i = 0; i < n; i++)
            {
                for (int j = K0_BC(i); j <= L - 2; j++)
                {
                    f[i] -= f[i - (L - 1) + j] * bc[i, j];
                    bc[i, j] = 0;
                }
                if (bc[i, L - 1] == 0)
                    return null;
                f[i] /= bc[i, L - 1];
                bc[i, L - 1] = 1;
            }
            // AX = Y
            for (int i = n - 2; i >= 0; i--)
                for (int j = L; j <= Kn_BC(i); j++)
                {
                    f[i] -= f[j - L + i + 1] * bc[i, j];
                    bc[i, j] = 0;
                }
            return f;
        }

        double[] SolveSqardMatrix()
        {
            double[,] bc = new double[n,n];
            // BC
            for(int j = 0; j < n; j++)
            {
                for (int i = j; i < n; i++)
                {
                    double s = matr[i, j];
                    for (int k = 0; k <= j - 1; k++)
                    {
                        s -= bc[i, k] * bc[k, j];
                    }
                    bc[i, j] = s;
                }
                for (int i = j + 1; i < n; i++)
                {
                    double s = matr[j, i];
                    for (int k = 0; k < j - 1; k++)
                    {
                        s -= bc[j, k] * bc[k, i];
                    }
                    bc[j, i] = s/bc[j,j];
                }

            }
            //B*y = f
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    f[i] -= f[j] * bc[i, j];
                }
                if (bc[i, i] == 0)
                    return null;
                f[i] /= bc[i, i];
                bc[i, i] = 1;
            }
            // Ax = Y
            for (int i = n-1; i >=0; i--)
            {
                for (int j = i + 1; j < n; j++)
                {
                    f[i] -= f[j] * bc[i, j];
                }
            }
            if (f.Contains(double.NaN) || f.Contains(double.NegativeInfinity) || f.Contains(double.PositiveInfinity))
                return null;
            return f;
        }

        public void Form_Answer(int test, int N, int _L, ref double avg)
        {
            n = N;
            L = _L;
            avg = 0;
            for (int i = 0; i < test; i++)
            {
                Generate(n, L);
                while (Solve() == null)
                {
                    Generate(n, L);
                }
                avg += Get_Avg(f, x);
            }
            avg /= test;
        }

        private void RandomMatr(int n)
        {
            Random rand = new Random();
            matr = new double[n, n];
            for(int i = 0; i<n; i++)
                for(int j = 0; j< n; j++)
                    matr[i, j] =  rand.Next(-10, 10);
        }

        private void Generate_F_and_X()
        {
            Random rand = new Random();
            x = new double[n];
            for (int i = 0; i < n; i++)
                x[i] = rand.Next(-10, 10);
            f = new double[n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    f[i] += matr[i, j]*x[j];
        }

        private double[,] RandomUpTreugolMatr(int n)
        {
            Random rand = new Random();
            double[,] res = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = i; j <= Kn(i); j++)
                    res[i, j] = (rand.NextDouble() - 0.5) * 2 * rand.Next(-10,10);
            return res;
        }

        private double[,] RandomDownTreugolMatr(int n)
        {
            Random rand = new Random();
            double[,] res = new double[n,n];
            for (int i = 0; i < n; i++)
                for (int j = K0(i); j <= i; j++)
                    res[i, j] = (rand.NextDouble() - 0.5) * 2 * rand.Next(-10, 10);
            return res;
        }

        private void Generate_Bad(int k)
        {
            GetBadMatr(RandomDownTreugolMatr(n), RandomUpTreugolMatr(n), n, k);
            Generate_F_and_X();
        }

        private void Generate_Good()
        {
            RandomMatr(n);
            Generate_F_and_X();
        }

        private void GetBadMatr(double[,] a, double[,] b, int n, int r)
        {
            matr = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < n; k++)
                        matr[i, j] += a[i, k] * b[k, j];
            for (int i = 0; i < n; i++)
                matr[i, i] /= Math.Pow(10, r);
        }

        public void Form_Good_And_Bad_Answer(int test, int N, ref double avg, int k = 1)
        {
            n = N;
            avg = 0;
            for (int i = 0; i < test; i++)
            {
                if (k > 1)
                    Generate_Bad(k);
                else Generate_Good();
                while (SolveSqardMatrix() == null)
                {
                    if (k > 1)
                        Generate_Bad(k);
                    else Generate_Good();
                }
                avg += Get_Avg(f, x);
            }
            avg /= test;
        }
    }
}
