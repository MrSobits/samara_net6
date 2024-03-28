namespace Bars.Gkh.Reforma.PerformerActions.SetHouseProfile
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;

    using Castle.Windsor;

    /// <summary>
    ///     Действие обновления профиля дома
    /// </summary>
    public class SetHouseProfileAction : LoggableSyncActionBase<SetHouseProfileParams, object>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "SetHouseProfile";

        #endregion

        #region Public Properties

        /// <summary>
        /// Идентификатор действия
        /// </summary>
        public override string Id
        {
            get
            {
                return SetHouseProfileAction.ActionId;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Выполнить действия
        /// </summary>
        /// <returns></returns>
        protected override object Execute()
        {
            var service = this.Container.Resolve<IRobjectDataCollector>();
            var domain = this.Container.ResolveDomain<RefRealityObject>();
            var periodService = this.Container.ResolveDomain<PeriodDi>();
            var repPeriodService = this.Container.ResolveDomain<ReportingPeriodDict>();
            try
            {
                this.Logger.SetActionDetails(string.Format("Идентификатор = {0} Период = {1}", this.Parameters.RobjectId, this.Parameters.PeriodDiId));
                var refRobjects = domain.GetAll().Where(x => x.RealityObject.Id == this.Parameters.RobjectId).ToArray();
                if (refRobjects.Length == 0)
                {
                    throw new Exception("Не найден указанный дом");
                }

                var refRobject = refRobjects[0];
                var externalIds = refRobjects.Select(x => x.ExternalId).ToArray();
                var externalIdsStr = string.Join(", ", externalIds);

                this.Logger.SetActionDetails(string.Format("Адрес = {0} Внешние идентификаторы = {1} Период = {2}", refRobject.RealityObject.Address, externalIdsStr, this.Parameters.PeriodDiId));

                var period = periodService.Get(this.Parameters.PeriodDiId);
                if (period == null)
                {
                    throw new Exception("Не найден указанный период раскрытия");
                }

                var reportingPeriodId =
                    repPeriodService.GetAll().Where(x => x.PeriodDi == period).Select(x => x.ExternalId).FirstOrDefault();

                this.Logger.SetActionDetails(string.Format("Адрес = {0} Внешние идентификаторы = {1} Период = {2}", refRobject.RealityObject.Address, externalIdsStr, period.Name));


                foreach (var externalId in externalIds)
                {
                    // мы можем передать не все поля. однако, реформа не может избирательно изменять значение полей,
                    // что вынуждает нас сначала получить текущий профиль дома, а потом "пропатчить" его нашими значениями.
                    // но упоротость логики здесь не заканчивается...
                    HouseProfileData currentProfile;
                    try
                    {
                        currentProfile = this.Client.GetHouseProfile(externalId, reportingPeriodId).house_profile_data;
                    }
                    catch
                    {
                        currentProfile = new HouseProfileData();
                    }

                    // ...патчим мы тоже хитрожопо: с проверкой на пустые значения. мы не можем
                    // затирать заполненное поле пустым значением (класс ClassMerger).
                    // всю эту порнографию надо выпилить при первом удобном случае
                    var result = service.CollectHouseProfileData(currentProfile, refRobject.RealityObject, period);
                    if (!result.Success)
                    {
                        throw new Exception(result.Message);
                    }

                    this.Client.SetHouseProfile(externalId, reportingPeriodId, result.Data);
                }

                return refRobject.RealityObject.Id;
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(domain);
                this.Container.Release(periodService);
                this.Container.Release(repPeriodService);
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
        public SetHouseProfileAction(IWindsorContainer container, ISyncProvider syncProvider, SetHouseProfileParams parameters)
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
        public SetHouseProfileAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}