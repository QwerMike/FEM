using MathNet.Symbolics;
using System;
using System.Collections.Generic;

namespace FEM
{
    static class FemSolve
    {
        public static List<double> Coeficients(
            string strT,
            string strS,
            string strF,
            double q,
            int n)
        {
            Expression T, s, f;

            try
            {
                T = Infix.ParseOrThrow(strT);
                s = Infix.ParseOrThrow(strS);
                f = Infix.ParseOrThrow(strF);
            }
            catch (Exception ex)
            {
                throw new FunctionParseException("Failed to parse function", ex);
            }

            double h = 1 / (double)n;

            var mainDiagonal = MainDiagonal(T, s, n);
            var subDiagonal = SubDiagonal(T, s, n);
            var superDiagonal = new List<double>(subDiagonal);
            subDiagonal.Insert(0, 0);
            superDiagonal.Add(0);
            var constantTerms = ConstantTerms(f, q, n);

            var coefs = TridiagonalMatrixAlgorithm(
                subDiagonal,
                mainDiagonal,
                superDiagonal,
                constantTerms);
            coefs.Add(0);

            return coefs;
        }
        
        public static List<double> MainDiagonal(
            Expression T,
            Expression s,
            int n)
        {
            var result = new List<double>(n);

            result.Add(
                T.Eval(0.5 / n) * n +
                s.Eval(0.5 / n) / (3 * n));

            for (int i = 1; i < n; i++)
            {
                result.Add(
                    (T.Eval((i - 0.5) / n) + T.Eval((i + 0.5) / n)) * n +
                    (s.Eval((i - 0.5) / n) + s.Eval((i + 0.5) / n)) / (3 * n));
            }

            return result;
        }

        public static List<double> SubDiagonal(
            Expression T,
            Expression s,
            int n)
        {
            var result = new List<double>(n-1);

            for (int i = 0; i < n-1; i++)
            {
                result.Add(
                    -T.Eval((i + 0.5) / n) * n +
                    s.Eval((i + 0.5) / n) / (6 * n));
            }

            return result;
        }

        public static List<double> ConstantTerms(
            Expression f,
            double q,
            int n)
        {
            var result = new List<double>(n);

            result.Add(f.Eval(0.5 / n) / (2 * n) + q);

            for (int i = 1; i < n; i++)
            {
                result.Add(
                    (f.Eval((i - 0.5) / n) +
                     f.Eval((i + 0.5) / n)) / (2 * n));
            }

            return result;
        }

        /// <summary>
        /// Tridiagonal Matrix Algorithm
        /// https://en.wikibooks.org/wiki/Algorithm_Implementation/Linear_Algebra/Tridiagonal_matrix_algorithm
        /// </summary>
        /// <param name="a">Sub diagonal </param>
        /// <param name="b">Main diagonal</param>
        /// <param name="c">Super diagonal</param>
        /// <param name="d">Constant Terms</param>
        /// <returns>Modified 'b'- result vector</returns>
        public static List<double> TridiagonalMatrixAlgorithm(
            List<double> a,
            List<double> b,
            List<double> c,
            List<double> d)
        {
            int n = d.Count;
            //n--;
            c[0] /= b[0];
            d[0] /= b[0];

            for (int i = 1; i < n; i++)
            {
                double tmp = b[i] - a[i] * c[i - 1];
                c[i] /= tmp;
                d[i] = (d[i] - a[i] * d[i - 1]) / tmp;
            }
            
            for (int i = n - 2; i >= 0; i--)
            {
                d[i] -= c[i] * d[i + 1];
            }

            return d;
        }

    }

    static class EvalHelper
    {
        public static double Eval(this Expression expr, double x)
        {
            var variables = new Dictionary<string, FloatingPoint>
            {
                { "x", x }
            };

            return Evaluate.Evaluate(variables, expr).RealValue;
        }
    }


    [Serializable]
    public class FunctionParseException : Exception
    {
        public FunctionParseException() { }
        public FunctionParseException(string message) : base(message) { }
        public FunctionParseException(string message, Exception inner) : base(message, inner) { }
        protected FunctionParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
