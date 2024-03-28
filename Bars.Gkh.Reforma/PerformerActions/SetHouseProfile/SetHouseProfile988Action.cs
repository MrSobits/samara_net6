namespace Bars.Gkh.Reforma.PerformerActions.SetHouseProfile
{
    using System;
    using System.Linq;
    using System.ServiceModel;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.Gkh.Reforma.Utils;
    using Bars.Gkh.Reforma.Utils.Validation;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    /// <summary>
    ///     Действие обновления профиля дома
    /// </summary>
    public class SetHouseProfile988Action : LoggableSyncActionBase<SetHouseProfileParams, object>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "SetHouseProfile988";

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
        /// Запуск действия
        /// </summary>
        /// <returns></returns>
        protected override object Execute()
        {
            var service = this.Container.Resolve<IRobject988DataCollector>();
            var domain = this.Container.ResolveDomain<RefRealityObject>();
            var periodService = this.Container.ResolveDomain<PeriodDi>();
            var repPeriodService = this.Container.ResolveDomain<ReportingPeriodDict>();
            try
            {
                this.Logger.SetActionDetails($"Идентификатор = {this.Parameters.RobjectId} Период = {this.Parameters.PeriodDiId}");
                var refRobjects = domain.GetAll().Where(x => x.RealityObject.Id == this.Parameters.RobjectId).ToArray();
                if (refRobjects.Length == 0)
                {
                    throw new Exception("Не найден указанный дом");
                }

                var refRobject = refRobjects[0];
                var externalIds = refRobjects.Select(x => x.ExternalId).ToArray();
                var externalIdsStr = string.Join(", ", externalIds);

                this.Logger.SetActionDetails(
                    $"Адрес = {refRobject.RealityObject.Address} Внешние идентификаторы = {externalIdsStr} Период = {this.Parameters.PeriodDiId}");

                var period = periodService.Get(this.Parameters.PeriodDiId);
                if (period == null)
                {
                    throw new ReformaValidationException("Не найден указанный период раскрытия");
                }

                var reportingPeriod = repPeriodService.GetAll().FirstOrDefault(x => x.PeriodDi == period);

                this.Logger.SetActionDetails($"Адрес = {refRobject.RealityObject.Address} Внешние идентификаторы = {externalIdsStr} Период = {period.Name}");


                foreach (var externalId in externalIds)
                {
                    HouseProfileData988 currentProfile;
                    try
                    {
                        currentProfile = this.Client.GetHouseProfile988(externalId, reportingPeriod.ExternalId).house_profile_data;
                    }
                    catch (FaultException exception) when(exception.Message == "Missing company house in this reporting period")
                    {
                        throw new ReformaValidationException("Отсутствует анкета МКД за указанный отчетный период", exception);
                    }

                    var result = service.CollectHouseProfile988Data(currentProfile, refRobject, period, this.Parameters.ManOrgId);
                    if (!result.Success)
                    {
                        throw new ReformaValidationException(result.Message);
                    }

                    var collectionResult = result.Data;
                    if (collectionResult.CollectedFiles.Length > 0)
                    {
                        foreach (var collectedFile in collectionResult.CollectedFiles)
                        {
                            collectedFile.Process(collectionResult.ProfileData, this.SyncProvider);
                        }
                    }

                    this.Client.SetHouseProfile988(externalId, reportingPeriod.ExternalId, collectionResult.ProfileData);
                }

                return this.Parameters.RobjectId;
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
        public SetHouseProfile988Action(IWindsorContainer container, ISyncProvider syncProvider, SetHouseProfileParams parameters)
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
        public SetHouseProfile988Action(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}