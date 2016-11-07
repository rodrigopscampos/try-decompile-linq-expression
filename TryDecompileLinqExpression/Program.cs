using System;
using System.Linq.Expressions;

namespace TryDecompileLinqExpression
{
    class Program
    {
        static void Main()
        {
            Expression<Func<int, int, int, int, int, int>> e1 = (a, b, c, d, e) => a + b - c * d / e;
            DoDecompile(e1.Body);

            Expression<Func<int, bool>> e2 = i => (i < 100 && i > 90) || i == -1;
            DoDecompile(e2.Body);

            Expression<Action> e4 = () => Console.WriteLine("Wello World");
            DoDecompile(e4.Body);

            const string s = "teste";
            Expression<Action> e5 = () => Console.WriteLine($"Writing external variable: {s}");
            DoDecompile(e5.Body);

            Console.ReadLine();

            /*
            RESULT:

            ----------
((a + b) - ((c * d) / e)):
-(a + b): a Add b
-((c * d) / e): --(c * d): c Multiply d
(c * d) Divide e

----------
(((i < 100) AndAlso (i > 90)) OrElse (i == -1)):
-((i < 100) AndAlso (i > 90)): --(i < 100): i LessThan 100
--(i > 90): i GreaterThan 90
-(i == -1): i Equal -1

----------
WriteLine("Wello World")

----------
WriteLine(Format("Writing external variable: {0}", "teste"))
            */
        }

        static void DoDecompile(Expression e)
        {
            Console.WriteLine(new string('-', 10));
            Decompile(e);
            Console.WriteLine();
        }

        static bool Decompile(Expression e, int depth = 0)
        {
            if (e.NodeType == ExpressionType.Parameter)
                return false;

            if (e.NodeType == ExpressionType.Call)
            {
                Console.WriteLine(e);
                return false;
            }

            var be = (BinaryExpression)e;

            if (depth == 0)
                Console.WriteLine(e + ": ");
            else
                Console.Write(new string('-', depth) + e + ": ");

            depth++;

            if (!Decompile(be.Left, depth))
            {
                Console.WriteLine(string.Format($"{be.Left} {be.NodeType} {be.Right}"));
            }

            else if (!Decompile(be.Right, depth))
            {
                Console.WriteLine(string.Format($"{be.Left} {be.NodeType} {be.Right}"));
            }

            return true;
        }
    }
}
