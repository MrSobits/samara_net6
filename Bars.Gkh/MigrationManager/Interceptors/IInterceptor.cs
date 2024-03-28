namespace Bars.Gkh.MigrationManager.Interceptors
{
    using Bars.B4.Utils;

    using Castle.DynamicProxy;

    /// <summary>
    /// Контракт интерцептора
    /// </summary>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    public interface IInterceptor<TService> : IInterceptor
    {
    }

    /// <summary>
    /// Абстрактный интерцептор
    /// </summary>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    public abstract class AbstractInterceptor<TService> : IInterceptor<TService>
        where TService : class
    {
        /// <summary>
        /// Проксируемый сервис
        /// <para>Внимание, использование может побудить зацикливание!</para>
        /// </summary>
        protected TService Service { get; set; }

        /// <summary>
        /// Непроксированный сервис
        /// </summary>
        protected TService UnproxedService { get; set; }

        /// <inheritdoc />
        public void Intercept(IInvocation invocation)
        {
            this.Service = invocation.Proxy as TService;
            this.UnproxedService = invocation.InvocationTarget as TService;

            if (this.Service.IsNull() || this.UnproxedService.IsNull())
            {
                invocation.Proceed();
            }
            else
            {
                this.OnBeforeProceed(invocation);
                invocation.Proceed();
                this.OnAfterProceed(invocation);
            }
        }

        /// <summary>
        /// Метод вызываемый до вызова перехваченного метода
        /// </summary>
        /// <param name="invocation">Описание вызова</param>
        protected virtual void OnBeforeProceed(IInvocation invocation)
        {
        }

        /// <summary>
        /// Метод вызываемый после вызова перехваченного метода
        /// </summary>
        /// <param name="invocation">Описание вызова</param>
        protected virtual void OnAfterProceed(IInvocation invocation)
        {
        }
    }
}