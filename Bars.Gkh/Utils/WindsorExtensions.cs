namespace Bars.Gkh.Utils
{
    using System;
    using System.Linq;
    using System.Reflection;

    using B4.IoC;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.Nhibernate;
    using Bars.Gkh.Utils.Caching;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using ExecutionAction;
    using Import;

    using NHibernate.Linq.Functions;

    /// <summary>
    /// Методы расширения для WindsorContainer
    /// </summary>
    public static class WindsorExtensions
    {
        /// <summary>
        /// Зарегистрировать класс импорта
        /// </summary>
        /// <typeparam name="TImport">Тип импорта</typeparam>
        /// <param name="container">IoC контейнер</param>
        /// <param name="id">Идентификатор импорта (ключ регистрации)</param>
        public static void RegisterImport<TImport>(this IWindsorContainer container, string id = null)
            where TImport : IGkhImport, ITaskExecutor
        {
            if (id.IsEmpty())
            {
                var idProp = typeof(TImport).GetMember("Id", BindingFlags.Public | BindingFlags.Static);

                if (idProp.IsNotEmpty())
                {
                    var prop = idProp[0];

                    if (prop is PropertyInfo)
                    {
                        id = prop.As<PropertyInfo>().GetValue(null, null).ToStr();
                    }
                    else if (prop is FieldInfo)
                    {
                        id = prop.As<FieldInfo>().GetValue(null).ToStr();
                    }
                }
                else
                {
                    id = typeof (TImport).FullName;
                }
            }

            container.RegisterTransient<IGkhImport, ITaskExecutor, TImport>(id);
        }

        /// <summary>
        /// Зарегистрировать обработчик задачи
        /// </summary>
        /// <typeparam name="TExecutor">Тип обработчика</typeparam>
        /// <param name="container">IoC контейнер</param>
        /// <param name="id">Идентификатор обработчика (ключ регистрации)</param>
        public static void RegisterTaskExecutor<TExecutor>(this IWindsorContainer container, string id)
            where TExecutor : ITaskExecutor
        {
            container.RegisterTransient<ITaskExecutor, TExecutor>(id);
        }

        /// <summary>
        /// Зарегистрировать обработчик обратного вызова для задачи
        /// </summary>
        /// <typeparam name="TCallback">Тип обработчика</typeparam>
        /// <param name="container">IoC контейнер</param>
        /// <param name="id">Идентификатор обработчика (ключ регистрации)</param>
        public static void RegisterTaskCallback<TCallback>(this IWindsorContainer container, string id)
            where TCallback : ITaskCallback
        {
            container.RegisterTransient<ITaskCallback, TCallback>(id);
        }

        /// <summary>
        /// Метод для регистрации корневой секции конфигурации
        /// </summary>
        /// <param name="container">IoC container</param>
        /// <typeparam name="T">Тип секции</typeparam>
        /// <exception cref="Exception">Выбрасывается в случае, если тип секции не помечен атрибутом GkhConfigSectionAttribute</exception>
        public static void RegisterGkhConfig<T>(this IWindsorContainer container) where T : class, IGkhConfigSection
        {
            var type = typeof(T);
            if (!type.HasAttribute<GkhConfigSectionAttribute>(true))
            {
                throw new Exception(
                    "Корневые секции конфигурации должны быть помечены атрибутом GkhConfigSectionAttribute. Для типа {0} это требование не выполнено"
                        .FormatUsing(type.FullName));
            }

            container.RegisterTransient<IGkhConfigSection, T>();
        }

        /// <summary>
        /// Метод получения корневой секции конфигурации
        /// </summary>
        /// <typeparam name="T">Тип секции</typeparam>
        /// <param name="container">IoC контейнер</param>
        /// <returns>Экземпляр секции концигурации, заполненный в соответствии с ранее сохраненными и/или стандартными значениями</returns>
        /// <exception cref="Exception">Выбрасывается в случае, если тип секции не помечен атрибутом GkhConfigSectionAttribute</exception>
        public static T GetGkhConfig<T>(this IWindsorContainer container) where T : class, IGkhConfigSection
        {
            var type = typeof(T);
            if (!type.HasAttribute<GkhConfigSectionAttribute>(true))
            {
                throw new Exception(
                    "Корневые секции конфигурации должны быть помечены атрибутом GkhConfigSectionAttribute. Для типа {0} это требование не выполнено"
                        .FormatUsing(type.FullName));
            }

            var gkhConfigProvider = container.Resolve<IGkhConfigProvider>();
            using (container.Using(gkhConfigProvider))
            {
                return gkhConfigProvider.Get<T>();
            }
        }

        /// <summary>
        /// Зарегистрировать выполнение действий
        /// </summary>
        /// <typeparam name="T">Тип действия</typeparam>
        /// <param name="container">IoC контейнер</param>
        /// <param name="code">Код регистрации</param>
        public static void RegisterExecutionAction<T>(this IWindsorContainer container, string code = null)
            where T : IExecutionAction
        {
            var name = code ?? typeof(T).Name;

            if (typeof(T).Is<IMandatoryExecutionAction>())
            {
                Component.For<IExecutionAction, IMandatoryExecutionAction>()
                    .ImplementedBy(typeof(T))
                    .Named(name)
                    .LifestyleTransient()
                    .RegisterIn(container);
            }
            else
            {
                container.RegisterTransient<IExecutionAction, T>(name);
            }
        }

        /// <summary>
        /// Зарегистрировать представление для <typeparamref name="TEntity"/>, поддерживающее инвалидацию кэша
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <typeparam name="TViewModel">Тип представления</typeparam>
        /// <param name="container">Контейнер</param>
        /// <param name="name">Имя регистрации</param>
        public static void RegisterCacheableViewModel<TEntity, TViewModel>(this IWindsorContainer container, string name = null)
            where TEntity : IEntity
            where TViewModel : ICacheableViewModel<TEntity>
        {
            container.RegisterTransient<IViewModel<TEntity>, ICacheableViewModel<TEntity>, TViewModel>();
        }

        /// <summary>
        /// Получить <see cref="IExecutionAction"/> зарегистрированную по коду nameof(T)
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <typeparam name="T"><see cref="IExecutionAction"/>Тип действия</typeparam>
        public static IExecutionAction ResolveExecutionAction<T>(this IWindsorContainer container) where T : IExecutionAction
        {
            return container.Resolve<IExecutionAction>(nameof(T));
        }

        /// <summary>
        /// Безопасное получение реализации из контейнера
        /// </summary>
        public static T TryResolve<T>(this IWindsorContainer container, T defaultValue = default(T))
        {
            return container.HasComponent<T>() ? container.Resolve<T>() : defaultValue;
        }

        /// <summary>
        /// Зарегистрировать HQL генератор <see cref="BaseGkhMethodHqlGenerator"/>
        /// </summary>
        public static void RegisterMethodHqlGenerator<TGenerator>(this IWindsorContainer container)
            where TGenerator : BaseGkhMethodHqlGenerator
        {
            container.RegisterTransient<IGkhMethodHqlGenerator, TGenerator>();
        }

        /// <summary>
        /// Зарегистрировать HQL генератор
        /// </summary>
        public static void RegisterPropertyHqlGenerator<TGenerator>(this IWindsorContainer container)
            where TGenerator : IHqlGeneratorForProperty
        {
            container.RegisterTransient<IHqlGeneratorForProperty, TGenerator>();
        }
    }
}