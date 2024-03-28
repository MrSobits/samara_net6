namespace Bars.Gkh.Formulas.Impl
{
    using System.Collections.Generic;
    using B4;
    using B4.Utils.Annotations;
    using Formulas;
    using NCalc;
    using NCalc.Domain;

    public class FormulaService : IFormulaService
    {
        private readonly IFormulaTranslator translator;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="translator"></param>
        public FormulaService(IFormulaTranslator translator)
        {
            this.translator = translator;
        }

        public IDataResult GetParamsList(string formula)
        {
            ArgumentChecker.NotNullOrEmptyOrWhitespace(formula, nameof(formula));

            formula = this.translator.Translate(formula);

            var expr = new Expression(formula, EvaluateOptions.IterateParameters);
            var hasErrors = expr.HasErrors();

            if (!hasErrors)
            {
                var paramsList = new HashSet<string>();
                this.ExtractIdentifiers(expr.ParsedExpression, paramsList);

                return new ListDataResult(paramsList, paramsList.Count);
            }

            return new BaseDataResult(false, "Проверьте правильность задания формулы!");
        }

        public IDataResult CheckFormula(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
            {
                return new BaseDataResult(false, "Формула отсутствует");
            }

            formula = this.translator.Translate(formula);

            var expr = new Expression(formula, EvaluateOptions.IterateParameters);
            var hasErrors = expr.HasErrors();

            return new BaseDataResult(!hasErrors, hasErrors ? "Формула задана неверно!" : "Формула задана верно!");
        }

        private void ExtractIdentifiers(LogicalExpression expression, HashSet<string> identifiers)
        {
            if (expression is UnaryExpression)
            {
                var ue = expression as UnaryExpression;

                this.ExtractIdentifiers(ue.Expression, identifiers);
            }
            else if (expression is BinaryExpression)
            {
                var be = expression as BinaryExpression;

                this.ExtractIdentifiers(be.LeftExpression, identifiers);
                this.ExtractIdentifiers(be.RightExpression, identifiers);
            }
            else if (expression is TernaryExpression)
            {
                var te = expression as TernaryExpression;

                this.ExtractIdentifiers(te.LeftExpression, identifiers);
                this.ExtractIdentifiers(te.MiddleExpression, identifiers);
                this.ExtractIdentifiers(te.RightExpression, identifiers);
            }
            else if (expression is Function)
            {
                var fn = expression as Function;

                var expressions = fn.Expressions;
                if (expressions != null && expressions.Length > 0)
                {
                    for (int i = 0; i < expressions.Length; i++)
                    {
                        this.ExtractIdentifiers(expressions[i], identifiers);
                    }
                }
            }
            else if (expression is Identifier)
            {
                var identifier = expression as Identifier;
                identifiers.Add(identifier.Name);
            }
        }
    }
}