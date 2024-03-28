namespace Bars.Gkh.RegOperator.DomainModelServices.MassUpdater
{
    using System;
    using System.Collections.Generic;

    using Castle.Windsor;

    using NHibernate.Util;

    /// <summary>
    /// Контекст для работы с массовыми изменениями в системе
    /// </summary>
    /// <remarks> контекст нужно создавать внутри транзакции</remarks>
    public class MassUpdateContext : IDisposable
    {
        private readonly IWindsorContainer container;
        private readonly IDictionary<Type, IMassOperationExecutor> executors;
        private bool disposed;

        [ThreadStatic]
        private static Stack<MassUpdateContext> currentContextStack;

        public static MassUpdateContext CurrentContext
        {
            get
            {
                if (MassUpdateContext.currentContextStack == null || MassUpdateContext.currentContextStack.Count == 0)
                {
                    return null;
                }

                return MassUpdateContext.currentContextStack.Peek();
            }
        }

        /// <summary>
        /// Использовать сессию без хранения состояния
        /// </summary>
        public bool UseStatelessSession { get; set; }

        protected MassUpdateContext(IWindsorContainer container)
        {
            this.container = container;
            this.executors = new Dictionary<Type, IMassOperationExecutor>();

            if (MassUpdateContext.currentContextStack == null)
            {
                MassUpdateContext.currentContextStack = new Stack<MassUpdateContext>();
            }
            MassUpdateContext.currentContextStack.Push(this);
        }

        /// <summary>
        /// Создать контекст массового изменения в системе
        /// </summary>
        public static MassUpdateContext CreateContext(IWindsorContainer container)
        {
            var useStatelessSession = MassUpdateContext.CurrentContext?.UseStatelessSession ?? false;
            return new MassUpdateContext(container) {UseStatelessSession = useStatelessSession};
        }

        /// <summary>
        /// Создать контекст массового изменения в системе
        /// </summary>
        public static MassUpdateContext CreateContext(IWindsorContainer container, bool useStatelessSession)
        {
            return new MassUpdateContext(container) { UseStatelessSession = useStatelessSession };
        }

        /// <summary>
        /// Добавить оброботчик для массового изменения в системе 
        /// </summary>
        /// <param name="executor">Исполнитель</param>
        public void AddHandler(IMassOperationExecutor executor)
        {
            this.ThrowIsDisposed();

            this.executors[executor.GetType()] = executor;
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Неверное состояние контекста массовой обработки</exception>
        /// <exception cref="InvalidOperationException">Нарушена очередность освобождения контекста</exception>
        public void Dispose()
        {
            this.ThrowIsDisposed();
            this.ThrowIsNotHead();

            MassUpdateContext.currentContextStack.Pop();

            this.executors.ForEach(x => x.Value.ProcessChanges(this.UseStatelessSession));

            this.disposed = true;
        }

        private void ThrowIsNotHead()
        {
            if (MassUpdateContext.currentContextStack.Peek() != this)
            {
                throw new InvalidOperationException("Нарушена очередность освобождения контекста");
            }
        }

        private void ThrowIsDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("Неверное состояние контекста массовой обработки");
            }
        }
    }
}