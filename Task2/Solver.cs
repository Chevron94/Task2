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
            if (i <= L-1)
                return 0;
            else
                return i - L + 1;
        }

        private int Kn_BC(int i)
        {
            return i;
        }

        private void Generate_X_F(int n, int l)
        {
            x = new double[n];
            f = new double[n];
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
                x[i] = rnd.Next(-10, 10) * rnd.NextDouble();
     //       x = new double[4]  {0, -5, 2, -8};
            for (int i = 0; i < n; i++)
            {
                
                double sum = 0;
                int pos_x = Kn(i);
                for (int j = l - 1; j >= 0; j--)
                {
                    if (matr[i, j] != Double.MaxValue)
                    {
                        sum += matr[i, j] * x[pos_x];
                        pos_x--;
                    }
                }
                int m = 1;
                for (int j = 1; j < l; j++)
                {
                    if (i - m < 0)
                        break;
                    sum += matr[i - m, j] * x[pos_x];
                    m++;
                    pos_x--;
                }
                f[i] = sum;
            }
        }

        public void Generate(int n, int l)
        {
            matr = new double[n, l];
            Random rnd = new Random();
            int k = 0;
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < n - k; j++)
                {
                    matr[j, i] = rnd.Next(-10, 10) * rnd.NextDouble(); 
                }
                for (int j = n - k; j < n; j++)
                {
                    matr[j, i] = Double.MaxValue;
                }
                k++;
            }
   //         matr = new double[,] { { -4, 3 }, { 0, 3 }, { 8, 0 }, { -5, double.MaxValue } };
            Generate_X_F(n, l);
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

        double[] SolveSqardMatrix()
        {
            double[,] bc = new double[n, n];
            // BC
            for (int j = 0; j < n; j++)
            {
                for (int i = j; i < n; i++)
                {
                    double s = matr[i, j];
                    for (int k = 0; k < j; k++)
                    {
                        s -= bc[i, k] * bc[k, j];
                    }
                    bc[i, j] = s;
                }
                for (int i = j + 1; i < n; i++)
                {
                    double s = matr[j, i];
                    for (int k = 0; k < j; k++)
                    {
                        s -= bc[j, k] * bc[k, i];
                    }
                    bc[j, i] = (s / bc[j, j]);
                }

            }
            //B*y = f
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < j; i++)
                {
                    f[j] -= f[i] * bc[j, i];
                }
                if (bc[j, j] == 0)
                    return null;
                f[j] /= bc[j, j];
                //bc[i, i] = 1;
            }
            // Ax = Y
            for (int i = n - 2; i >= 0; i--)
            {
                for (int j = i+1; j < n; j++)
                {
                    f[i] -= f[j] * bc[i, j];
                }
                //base[]
            }
            if (f.Contains(double.NaN) || f.Contains(double.NegativeInfinity) || f.Contains(double.PositiveInfinity))
                return null;
            return f;
        }



        //private double[] Solve()
        //{
        //    double[,] bc = new double[n,2*L-1];
        //    // bc - OK!
        //    for (int j = 0; j < n; j++)
        //    {
        //        int kn = Kn(j);
        //        for (int i = j; i <= kn; i++)
        //        {
        //            double s = 0;
        //            int k0 = K0(i);
        //            for (int k = k0; k < j ; k++)
        //            {
        //                if ((k - i + L - 1) >= 0)
        //                    s += bc[i, k - i + L - 1] * bc[k, j - k + L - 1];
        //            }
        //            bc[i,j - i + L - 1] = GetValue(i,j) - s;
        //        }
                
        //        for (int i = j+1; i <= kn; i++)
        //        {
        //            double s = 0;
        //            int k0 = K0(j);
        //            for (int k = k0; k <=j - 1; k++)
        //            {
        //                if ((L + i - k - 1) < 2 * L - 1)
        //                    s += bc[j, k - j + L - 1] * bc[k, L + i - k - 1];
        //            }
        //            if (bc[j, L - 1] == 0)
        //                return null;
        //            bc[j, i - j + L - 1] = (GetValue(j,i) - s) / bc[j,L - 1];
        //        }
                 
        //    }
        //    // BY = F
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = K0_BC(i); j <= L - 2; j++)
        //        {
        //            f[i] -= f[i - (L - 1) + j] * bc[i, j];
        //            bc[i, j] = 0;
        //        }
        //        if (bc[i, L - 1] == 0)
        //            return null;
        //        f[i] /= bc[i, L - 1];
        //        bc[i, L - 1] = 1;
        //    }
        //    // AX = Y
        //    for (int i = n - 2; i >= 0; i--)
        //        for (int j = L; j <= Kn_BC(i); j++)
        //        {
        //            f[i] -= f[j - L + i + 1] * bc[i, j];
        //            bc[i, j] = 0;
        //        }
        //    return f;
        //}

        void SetBCValue(ref double[,] bc, double val, int i, int j)
        {
            bc[i, L - 1 - i + j] = val;
        }
        double GetBCValue(double[,] bc, int i, int j)
        {
            if (j <= i && j >= K0_BC(i) && j <= Kn_BC(i))
                return bc[i, L - 1 - i + j];
            else return 0;
        }

        private double[] Solve()
        {
            double[,] bc = new double[n,  L];
            // bc - OK!
            for (int j = 0; j < n; j++)
            {
                int kn = Kn(j);
                for (int i = j; i <= kn; i++)
                {
                    double s = 0;
                    int k0 = K0(i);
                    for (int k = k0; k < j; k++)
                    {
                            s += GetBCValue(bc,i,k) * GetBCValue(bc,j,k)/GetBCValue(bc,k,k);
                    }
                    SetBCValue(ref bc, GetValue(i, j) - s,i,j);
                }

            }
            // BY = F
            for (int i = 0; i < n; i++)
            {
                for (int j = K0_BC(i); j < i; j++)
                {
                    f[i] -= f[j] * GetBCValue(bc, i, j);
                    //bc[i, j] = 0;
                }
                if (GetBCValue(bc,i,i) == 0)
                    return null;
                f[i] /= GetBCValue(bc,i,i);
               // bc[i, L - 1] = 1;
            }
            // AX = Y
            for (int i = n - 2; i >= 0; i--)
                for (int j = i+1; j < n; j++)
                {
                    f[i] -= f[j] * GetBCValue(bc, j, i) / GetBCValue(bc, i, i);
                   // bc[i, j] = 0;
                }
            return f;
        }
        public void Form_Answer(int test, int N, int _L, ref double avg, int k=0)
        {
            n = N;
            L = _L;
            avg = 0;
            for (int i = 0; i < test; i++)
            {
                if (k == 0)
                {
                    Generate(n, L);
                    while (Solve() == null)
                    {
                        Generate(n, L);
                    }
                }
                else
                {
                    Generate_Bad(k);
                    while (SolveSqardMatrix() == null)
                    {
                        Generate_Bad(k);
                    }
                }
                avg += Get_Avg(f, x);
            }
            avg /= test;
        }

        private double[,] RandomUpTreugolMatr(int n, int k)
        {
            Random rand = new Random();
            double _k = Math.Pow(10, -k);
            double[,] res = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = i; j < n; j++)
                    res[i, j] = rand.Next(-10, 10) *(rand.NextDouble() - 0.5) * 2;
            for (int i=0; i<n; i++)
                res[i,i] *= _k;
            return res;
        }

        private double[,] RandomDownTreugolMatr(int n, int k)
        {
            Random rand = new Random();
            double _k = Math.Pow(10, -k);
            double[,] res = new double[n,n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j <= i; j++)
                    res[i, j] = rand.Next(-10, 10)*(rand.NextDouble() - 0.5) * 2;
            for (int i = 0; i < n; i++)
                res[i, i] *= _k;
            return res;
        }

        private void Generate_SQARD_F_X(int n)
        {
            Random rand = new Random();
            x = new double[n];
            for(int i = 0; i<n; i++)
                x[i] = (rand.NextDouble() - 0.5) * 2 * rand.Next(-10, 10);

            f = new double[n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    f[i] += matr[i, j] * x[j];
        }

        private void Generate_Bad(int k)
        {
            GetBadMatr(RandomDownTreugolMatr(n,k), RandomUpTreugolMatr(n,k), n, k);
            Generate_SQARD_F_X(n);
        }

        private void GetBadMatr(double[,] b, double[,] a, int n, int r)
        {
            matr = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < n; k++)
                        matr[i, j] += a[i, k] * b[k, j];
        }
    }
}
