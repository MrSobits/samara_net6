namespace Bars.Gkh.Reforma.Utils.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4;

    /// <summary>
    /// Посетитель выражения, который обходит дерево выражения и пытается по цепочке получить значение по селектору
    /// </summary>
    internal class ExpressionSelectorVisitor : ExpressionVisitor
    {
        private readonly IList<string> descriptions;
        private readonly Stack<Func<object, object>> propertyValueSelectors;
        private readonly IMetaDescriptionContainer container;

        /// <summary>
        /// .ctor   
        /// </summary>
        /// <param name="container">Контейнер мета-описания</param>
        public ExpressionSelectorVisitor(IMetaDescriptionContainer container)
        {
            this.propertyValueSelectors = new Stack<Func<object, object>>();
            this.descriptions = new List<string>();
            this.container = container;
        }

        /// <summary>
        /// Метод обходит дерево выражения и пытается по цепочке получить значение
        /// </summary>
        /// <param name="value">Параметр для лямбда-аргумента</param>
        /// <param name="propertySelector">Проверяемое конечное свойство, может быть цепочка x => x.Prop1.Prop2.Prop3</param>
        /// <returns>Результат обхода дерева</returns>
        public TreeVisitResult GetResult<TValue>(TValue value, Expression propertySelector)
        {
            this.descriptions.Clear();
            this.propertyValueSelectors.Clear();

            this.Visit(propertySelector);

            var typeDescription = this.container.GetTypeDescription(typeof(TValue));
            this.descriptions.Add(typeDescription?.Name);

            var path = string.Join(" -> ", this.descriptions.Reverse().Where(x => x != null));

            object result;
            var success = this.TryGetFinalValue(value, out result);

            return new TreeVisitResult
            {
                Path = path,
                Data = result,
                Success = success
            };
        }

        /// <inheritdoc />
        protected override Expression VisitMember(MemberExpression node)
        {
            var propertyInfo = (PropertyInfo)node.Member;
            var propertyType = propertyInfo.PropertyType;
            var typeDescription = this.container.GetTypeDescription(propertyType);
            var propertyDescription = this.container.GetPropertyDescription(node.Member.DeclaringType, node.Member.Name);

            this.propertyValueSelectors.Push(x => propertyInfo.GetValue(x));
            this.descriptions.Add(
                propertyDescription?.Name                       // 1. Первым идёт описание свойства
                    ?? typeDescription?.Name                    // 2. Затем описание типа
                        ?? node.Member.DeclaringType?.Name);    // 3. В конечном счёте выведем название типа

            return base.VisitMember(node);
        }

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            this.propertyValueSelectors.Push(x => node.Method.Invoke(x, null));
            this.descriptions.Add(node.Method.Name);

            return base.VisitMethodCall(node);
        }

        private bool TryGetFinalValue(object firstObject, out object result)
        {
            if (firstObject == null)
            {
                result = null;
                return false;
            }

            if (this.propertyValueSelectors.Count > 0)
            {
                var selectorFunc = this.propertyValueSelectors.Pop();

                return this.TryGetFinalValue(selectorFunc(firstObject), out result);
            }

            result = firstObject;
            return true;
        }

        /// <summary>
        /// Результат обхода дерева
        /// </summary>
        public class TreeVisitResult : GenericDataResult<object>
        {
            /// <summary>
            /// Путь до поля
            /// </summary>
            public string Path { get;set; }
        }
    }
}