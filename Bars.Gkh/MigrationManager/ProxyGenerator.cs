namespace Bars.Gkh.MigrationManager
{
    using System;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Migrations;
    using Bars.Gkh.MigrationManager.Interceptors;

    using Castle.DynamicProxy;

    /// <summary>
    /// Генератор прокси
    /// </summary>
    public class ProxyGenerator
    {
        private static readonly Lazy<ProxyGenerator> instance = new Lazy<ProxyGenerator>(() => new ProxyGenerator());

        private readonly Castle.DynamicProxy.ProxyGenerator generator;
        private readonly ProxyGenerationOptions options;

        /// <summary>
        /// Ключ регистрации хука
        /// </summary>
        public static string MigrationManagerRegistrationKey => $"{nameof(MigrationManager)}Hooked";

        /// <summary>
        /// .ctor
        /// </summary>
        private ProxyGenerator()
        {
            this.generator = new Castle.DynamicProxy.ProxyGenerator();
            this.options = new ProxyGenerationOptions(new MigrationManagerProxyHook());
        }

        /// <summary>
        /// Метод генерирует прокси
        /// </summary>
        /// <param name="manager">
        /// Менеджер миграций
        /// </param>
        /// <returns>
        /// The <see cref="IMigrationManager"/>.
        /// </returns>
        public static IMigrationManager GetProxy(IMigrationManager manager)
        {
            return ProxyGenerator.instance.Value.GetProxyInternal(manager);
        }

        private IMigrationManager GetProxyInternal(IMigrationManager manager)
        {
            var container = ApplicationContext.Current.Container;


            return this.generator.CreateInterfaceProxyWithTargetInterface(
                manager,
                this.options, 
                container.ResolveAll<IInterceptor<IMigrationManager>>().Cast<IInterceptor>().ToArray());
        }
    }
}