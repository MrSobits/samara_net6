namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.GroupAction;

    using Castle.Windsor;

    /// <summary>
    /// Сервис действий в модуле раскрытия информации
    /// </summary>
    public class GroupDiService : IGroupDiService
    {
        /// <summary>
        /// Действия только по 988
        /// </summary>
        private readonly string[] only988Actions =
        {
            "ReformaManorgManualIntegration",
            "ReformaRealityObjManualIntegration"
        };

        private readonly DateTime period988Date = new DateTime(2015, 1, 1);

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис "Деятельность управляющей организации в периоде раскрытия информации"
        /// </summary>
        public IDomainService<DisclosureInfo> DisclosureInfoDomain { get; set; }

        /// <summary>
        /// Метод возвращает доступные действия
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список действий</returns>
        public IDataResult GetGroupActions(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoId = baseParams.Params.GetAs("disclosureInfoId", (long)0);
                var typeAction = baseParams.Params.GetAs("typeAction", TypeDiTargetAction.RealityObject);

                var groupActionList = this.Container.ResolveAll<IDiGroupAction>();

                var service = this.Container.ResolveAll<IAuthorizationService>().FirstOrDefault();

                if (service == null)
                {
                    return new BaseDataResult();
                }

                var userIdentity = this.Container.Resolve<IUserIdentity>();

                var disclosureInfo = this.DisclosureInfoDomain.GetAll().FirstOrDefault(x => x.Id == disclosureInfoId);


                var allowedActions = groupActionList
                    .Where(x => x.TypeDiTargetAction == typeAction)
                    .WhereIf(disclosureInfo.PeriodDi.DateStart < this.period988Date, x => !this.only988Actions.Contains(x.Code))
                    .Where(x => service.Grant(userIdentity, x.PermissionName, disclosureInfo))
                    .ToList();
                
                return new BaseDataResult(allowedActions)
                    {
                        Success = true
                    };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}
