namespace Bars.Gkh.Formulas
{
    using Bars.B4.IoC;
    using Castle.Windsor;

    public static class FormulaWindsorExtensions
    {
        /// <summary>
        /// Зарегистрировать параметр формулы в контейнер
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        public static void RegisterFormulaParameter<TParameter>(this IWindsorContainer container, string scope, string key)
            where TParameter : IFormulaParameter
        {
            if (!container.HasComponent<FormulaParameterContainer>())
            {
                container.RegisterSingleton<FormulaParameterContainer, FormulaParameterContainer>();
            }

            var prmContainer = container.Resolve<FormulaParameterContainer>();
            using (container.Using(prmContainer))
            {
                container.RegisterTransient<IFormulaParameter, TParameter>(key);
                prmContainer.RegisterParameter(scope, key);
            }
        }
    }
}