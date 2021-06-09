using System.Collections.Generic;
using System.Linq;
using ConsoleApplication234;
using LinearEquationNS;
using SimplexMethodNS;

namespace SimplexMethodNS
{
    /// <summary>
    /// ������ ���������, �������������� ������� ��� �������� ������, ������ ��������
    /// </summary>
    public class SimplexSolver
    {
        public List<string> GetVarNames(LinearEquationSystem les)
        {
            var vars = les.Variables.Where(s => s != "max" && s != "min" && s != "").ToList();
            return vars;            
        }

        public void PrepareMatrix(IList<LinearEquation> les, List<string> vars, out double[,] A, out double[] b)
        {
            var cols = vars.Count;
            var rows = les.Count;
            A = new double[rows, cols];
            b = new double[rows];
            
            for (int j = 0; j < rows; j++)
            {
                var eq = les[j];
                b[j] = -eq.GetCoeff("");//��������� ����
                for (int i = 0; i < cols; i++)
                    A[j, i] = les[j].GetCoeff(vars[i]);
            }
        }

        public void PrepareTarget(LinearEquation eq, List<string> vars, out double[] c)
        {
            var cols = vars.Count;
            c = new double[cols];

            for (int i = 0; i < cols; i++)
                c[i] = eq.GetCoeff(vars[i]);
        }

        public Dictionary<string, double> Solve(LinearEquationSystem les)
        {
            //��������� ����� ���� ���������
            var vars = GetVarNames(les);

            //�������������� �������
            double[,] A;//����� �����������
            double[] b;//��������� ���� �����������
            double[] c;//����� ������� �������
            
            PrepareMatrix(les.Where(e=>e.Sign == "=").ToList(), vars, out A, out b);
            PrepareTarget(les.FirstOrDefault(e => e.Sign == "=>" || e.Sign == "=<"), vars, out c);

            //������ ��������
            var simplex = new SimplexMethod();
            var solution = simplex.Solve(A, b, c);

            //�������������� ���������
            if (solution.Status == SimplexMethodResultStatus.Ok)
            {
                var res = new Dictionary<string, double>();
                for (int i = 0; i < vars.Count; i++)
                    res[vars[i]] = solution.Solution[i];

                return res;
            }

            return null;
        }
    }
}