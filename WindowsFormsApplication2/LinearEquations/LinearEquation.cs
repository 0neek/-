using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LinearEquationNS
{
    /// <summary>
    /// Линейное уравнение/неравенство.
    /// Содержит пары переменная-коэффициент.
    /// Свободный члнен хранится как перменная с пустым именем.
    /// Уравнение автоматически приводится к каноническому виду - слева перменные и свободный член, справа - ноль.
    /// Одноименные перменные приводятся к одному члену.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class LinearEquation : Dictionary<string, double>
    {
        public string Sign { get; private set; }

        //добавление члена уравнения
        public new void Add(string varName, double coeff)
        {
            this[varName] = GetCoeff(varName) + coeff;
        }

        public double GetCoeff(string varName)
        {
            var c = 0d;
            TryGetValue(varName, out c);
            return c;
        }

        //парсим уравнение
        public static LinearEquation Parse(string str)
        {
            var result = new LinearEquation();
            var m = Regex.Match(str, @"^([^\!<>=]+?)([\!<>=]+)([^\!<>=]+?)$");
            if (!m.Success)
                throw new Exception("Syntax error");
            Parse(result, m.Groups[1].Value, 1);//left part
            Parse(result, m.Groups[3].Value, -1);//right part
            result.Sign = m.Groups[2].Value;

            return result;
        }

        //парсим выражение
        private static void Parse(LinearEquation result, string str, int coeff)
        {
            foreach (Match m in Regex.Matches(str, @"([+\-]?)\s*([\w*\.\s]+)"))
            {
                var s = m.Groups[2].Value.Trim();

                if (s == "") continue;

                var v = s;
                var c = 1d;

                //выделяем коэффициент и имя переменной
                var cm = Regex.Match(s, @"(?<coeff>\d+)\s*\*\s*(?<var>[a-z0-9]*)|(?<coeff>\d+)(?<var>[a-z0-9]*)|(?<var>[a-z0-9][a-z0-9]*)");
                if (cm.Success && cm.Groups["var"].Value != "")
                {
                    v = cm.Groups["var"].Value;
                    if(cm.Groups["coeff"].Value != "")
                        c = double.Parse(cm.Groups["coeff"].Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    if (double.TryParse(s, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out c))
                        v = "";
                    else
                        c = 1d;
                }

                //обрабатываем знак
                if (m.Groups[1].Value == "-")
                    c *= -1;

                c *= coeff;

                result.Add(v, c);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var pair in this)
            {
                var mult = string.IsNullOrEmpty(pair.Key) ? "" : "*";
                sb.AppendFormat("{0}{1:G}{3}{2}", Math.Sign(pair.Value) == -1 ? "-" : "+", Math.Abs(pair.Value), pair.Key, mult);
            }

            sb.AppendFormat(" {0} 0", Sign);

            return sb.ToString().Trim('+');
        }

        public void Invert()
        {
            switch (Sign)
            {
                case ">=": Sign = "<="; break;
                case "<=": Sign = ">="; break;
                case ">": Sign = "<"; break;
                case "<": Sign = ">"; break;
                case "=>": Sign = "=<"; break;
            }

            foreach (var v in Keys.ToArray())
                this[v] *= -1;
        }
    }
}