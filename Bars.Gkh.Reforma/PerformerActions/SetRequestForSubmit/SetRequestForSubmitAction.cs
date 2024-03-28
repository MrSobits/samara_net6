namespace Bars.Gkh.Reforma.PerformerActions.SetRequestForSubmit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    ///     Действие отправки заявок на получения доступа к раскрытию информации по
    ///     управляющим организациям
    /// </summary>
    public class SetRequestForSubmitAction : LoggableSyncActionBase<object, SetRequestForSubmitInnStatus[]>
    {
        #region Static Fields

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "SetRequestForSubmit";

        #endregion

        #region Public Properties

        public override string Id
        {
            get
            {
                return ActionId;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Выполнение действия
        /// </summary>
        /// <returns>
        ///     Результат подачи запроса
        /// </returns>
        protected override SetRequestForSubmitInnStatus[] Execute()
        {
            var service = Container.Resolve<IManOrgService>();
            var statuses = new List<SetRequestForSubmitInnStatus>();
            try
            {
                var inns = service.GetUnrequestedInns();
                foreach (var innsPart in inns.SplitArray(200))
                {
                    var result = Client.SetRequestForSubmit(innsPart.ToArray());
                    Container.InTransaction(
                        () =>
                            {
                                var successInns = result.Where(x => x.status == (int)SetRequestForSubmitInnStatusEnum.Success).Select(x => x.inn).ToArray();
                                foreach (var inn in successInns)
                                {
                                    service.SetRequestState(inn, DateTime.UtcNow, null, RequestStatus.pending);
                                }
                            });

                    statuses.AddRange(result);
                }

                return statuses.ToArray();
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
        public SetRequestForSubmitAction(IWindsorContainer container, ISyncProvider syncProvider, object parameters)
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
        public SetRequestForSubmitAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}