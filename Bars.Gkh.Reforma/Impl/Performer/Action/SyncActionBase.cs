namespace Bars.Gkh.Reforma.Impl.Performer.Action
{
    using System;
    using System.ServiceModel;

    using B4;
    using B4.IoC;
  
    using Exceptions;
    using Interface;
    using Interface.Performer;
    using ReformaService;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     Базовый класс для выолнения действий сихронизации с Реформой ЖКХ
    /// </summary>
    /// <typeparam name="TParams">Тип входных параметров действия</typeparam>
    /// <typeparam name="TResult">Тип результат действия</typeparam>
    public abstract class SyncActionBase<TParams, TResult> : ISyncAction<TParams, TResult>
    {
        #region Explicit Interface Properties

        /// <summary>
        ///     Параметры действия
        /// </summary>
        object ISyncAction.Parameters
        {
            get
            {
                return Parameters;
            }

            set
            {
                Parameters = (TParams)value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Выполнение действия
        /// </summary>
        /// <returns>Результат действия</returns>
        public virtual SyncActionResult<TResult> Perform()
        {
            var result = new SyncActionResult<TResult>();
            try
            {
                result.Data = Execute();
                result.Success = true;
            }
            catch (FaultException e)
            {
                result.ErrorDetails = new ErrorDetails(e);
                result.Success = false;
            }
            catch (SyncException e)
            {
                result.ErrorDetails = e.Details;
                result.Success = false;
            }
            catch (Exception e)
            {
                result.ErrorDetails = new ErrorDetails(e);
                result.Success = false;

                Container.UsingForResolved<ILogger>((container, manager) => manager.LogError(e, e.ToString()));
            }

            return result;
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        ///     Выполнение действия
        /// </summary>
        /// <returns>Результат действия</returns>
        SyncActionResult ISyncAction.Perform()
        {
            return Perform();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Реализация действия
        /// </summary>
        /// <returns>Результат действия</returns>
        protected abstract TResult Execute();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncProvider">
        ///     Провайдер синхронизации
        /// </param>
        /// <param name="parameters">
        ///     Параметры действия
        /// </param>
        protected SyncActionBase(IWindsorContainer container, ISyncProvider syncProvider, TParams parameters)
        {
            Container = container;
            SyncProvider = syncProvider;
            Parameters = parameters;
        }

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncProvider">
        ///     Провайдер синхронизации
        /// </param>
        protected SyncActionBase(IWindsorContainer container, ISyncProvider syncProvider)
            : this(container, syncProvider, default(TParams))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        ///     Параметры действия
        /// </summary>
        public TParams Parameters { get; set; }

        /// <summary>
        ///     Сериализованные в JSON параметры. Геттер сериализует текущие параметры, сеттер - десериализует
        /// </summary>
        public virtual string SerializedParameters
        {
            get
            {
                return JsonNetConvert.SerializeObject(Container, Parameters);
            }

            set
            {
                Parameters = string.IsNullOrEmpty(value) ? default(TParams) : JsonNetConvert.DeserializeObject<TParams>(Container, value);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Клиент Реформы ЖКХ
        /// </summary>
        protected ApiSoapPortClient Client
        {
            get
            {
                return SyncProvider.Client;
            }
        }

        /// <summary>
        ///     Планировщик действий синхронизации
        /// </summary>
        protected ISyncActionPerformer Performer
        {
            get
            {
                return SyncProvider.Performer;
            }
        }

        /// <summary>
        ///     IoC контейнер
        /// </summary>
        protected IWindsorContainer Container { get; set; }

        /// <summary>
        ///     Провайдер синхронизации
        /// </summary>
        protected ISyncProvider SyncProvider { get; set; }

        #endregion
    }
}
