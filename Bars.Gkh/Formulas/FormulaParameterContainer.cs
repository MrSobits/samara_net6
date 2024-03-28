namespace Bars.Gkh.Formulas
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Castle.Windsor;

    /// <summary>
    /// Контейнер для параметров формул
    /// </summary>
    public class FormulaParameterContainer
    {
        private readonly IWindsorContainer _container;
        private Dictionary<string, HashSet<string>> _prmContainer = new Dictionary<string, HashSet<string>>();

        public FormulaParameterContainer(IWindsorContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Зарегистрировать ключ компонента параметра формулы в определенном скоупе
        /// </summary>
        /// <param name="scope">Имя скоупа</param>
        /// <param name="componentKey">Ключ компонента</param>
        public void RegisterParameter(string scope, string componentKey)
        {
            ArgumentChecker.NotNullOrEmptyOrWhitespace(scope, "scope");
            ArgumentChecker.NotNullOrEmptyOrWhitespace(componentKey, "componentKey");

            HashSet<string> cmps;
            if (!_prmContainer.TryGetValue(scope, out cmps))
            {
                cmps = new HashSet<string>();
            }

            if (!cmps.Contains(componentKey))
            {
                cmps.Add(componentKey);
            }

            _prmContainer[scope] = cmps;
        }

        /// <summary>
        /// Получить параметры из скоупа
        /// </summary>
        /// <param name="scope">Имя скоупа</param>
        public List<ParameterInfo> ListParametersByScope(string scope)
        {
            var keys = _prmContainer.Get(scope);
            List<ParameterInfo> result;

            if (keys.IsNotEmpty())
            {
                var prms = keys.Select(key => _container.Resolve<IFormulaParameter>(key)).ToList();
                using (_container.Using(prms))
                {
                    result = prms.Select(
                        x => new ParameterInfo
                        {
                            Code = x.Code,
                            DisplayName = x.DisplayName
                        }).ToList();
                }
            }
            else
            {
                result = new List<ParameterInfo>(0);
            }

            return result;
        }
    }

    public struct ParameterInfo
    {
        public string DisplayName { get; set; }

        public string Code { get; set; }
    }
}