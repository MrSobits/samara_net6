namespace Bars.Gkh.Reforma.PerformerActions.GetReportingPeriodList
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.ReformaService;

    using Castle.Windsor;

    /// <summary>
    ///     Действие получения списка отчетных периодов Реформы
    /// </summary>
    public class GetReportingPeriodListAction : LoggableSyncActionBase<object, ReportingPeriod[]>
    {
        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "GetReportingPeriodList";

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
        public GetReportingPeriodListAction(IWindsorContainer container, ISyncProvider syncProvider, object parameters)
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
        public GetReportingPeriodListAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

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

        protected override ReportingPeriod[] Execute()
        {
            var service = Container.ResolveDomain<ReportingPeriodDict>();
            try
            {
                var result = Client.GetReportingPeriodList();
                Container.InTransaction(
                    () =>
                        {
                            foreach (var period in result)
                            {
                                var entity = service.GetAll().FirstOrDefault(x => x.ExternalId == period.id) ?? new ReportingPeriodDict { ExternalId = period.id };

                                entity.DateStart = period.date_start;
                                entity.DateEnd = period.date_end;
                                entity.Name = period.name;
                                entity.State = (ReportingPeriodStateEnum)period.state;
                                entity.Is_988 = period.is_988;

                                if (entity.Id > 0)
                                {
                                    service.Update(entity);
                                }
                                else
                                {
                                    service.Save(entity);
                                }
                            }
                        });

                return result;
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}