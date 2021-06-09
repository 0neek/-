using System.Collections.Generic;
using System.Linq;

namespace LinearEquationNS
{
    /// <summary>
    /// Система линейных уравнений/неравенств
    /// </summary>
    public class LinearEquationSystem : List<LinearEquation>
    {
        public HashSet<string> Variables { get; } = new HashSet<string>();

        public new void Add(LinearEquation eq)
        {
            foreach (var v in eq.Keys)
                Variables.Add(v);

            base.Add(eq);
        }

        //парсим систему уравнений
        public static LinearEquationSystem Parse(string str)
        {
            var res = new LinearEquationSystem();

            foreach (var line in str.Split('\r', '\n').Where(s => !string.IsNullOrWhiteSpace(s)))
                res.Add(LinearEquation.Parse(line));

            return res;
        }
    }
}