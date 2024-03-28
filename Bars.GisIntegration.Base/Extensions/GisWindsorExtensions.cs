namespace Bars.GisIntegration.Base.Extensions
{
    using Bars.B4.IoC;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.GisServiceProvider;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    /// <summary>
    /// Методы расширения контейнера для ГИС
    /// </summary>
    public static class GisWindsorExtensions
    {
        /// <summary>
        /// Регистрация поставщика сервиса
        /// </summary>
        public static void RegisterGisServiceProvider<TClient, TType>(this IWindsorContainer container, string name) 
            where TType : IGisServiceProvider<TClient>
        {
            container.RegisterTransient<IGisServiceProvider, IGisServiceProvider<TClient>, TType>(name);
        }

        /// <summary>
        /// Метод регистрации в контейнере справочника
        /// </summary>
        /// <typeparam name="T">Тип справочника</typeparam>
        /// <param name="container">Экземпляр контейнера</param>
        public static void RegisterDictionary<T>(this IWindsorContainer container) where T : IDictionary
        {
            var dictionaryType = typeof(T);
            container.Register(Component.For<IDictionary>().Named(dictionaryType.Name).ImplementedBy(dictionaryType).LifeStyle.Transient);
        }
    }
}