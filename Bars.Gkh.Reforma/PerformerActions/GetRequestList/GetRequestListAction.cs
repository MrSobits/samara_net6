namespace Bars.Gkh.Reforma.PerformerActions.GetRequestList
{
    using Bars.Gkh.Domain;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.ReformaService;

    using Castle.Windsor;

    /// <summary>
    ///     Действие получения списка запросов на получение доступа на раскрытие информации
    ///     по управляющим организациям
    /// </summary>
    public class GetRequestListAction : LoggableSyncActionBase<object, RequestState[]>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "GetRequestList";

        #endregion

        #region Public Properties

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public override string Id
        {
            get
            {
                return ActionId;
            }
        }

        #endregion

        #region Methods

        protected override RequestState[] Execute()
        {
            var service = Container.Resolve<IManOrgService>();
            try
            {
                var requests = Client.GetRequestList();
                Container.InTransaction(
                    () =>
                        {
                            foreach (var request in requests)
                            {
                                service.SetRequestState(request.inn, request.create_date, request.update_date, (RequestStatus)request.status);
                            }
                        });

                return requests;
            }
            finally
            {
                Container.Release(service);
            }
        }

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
        public GetRequestListAction(IWindsorContainer container, ISyncProvider syncProvider, object parameters)
            : base(container, syncProvider, parameters)
        {
        }

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncProvider">
        ///     Провайдер синхронизации
        /// </param>
        public GetRequestListAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}