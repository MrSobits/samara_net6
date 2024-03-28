namespace Bars.GkhGji.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Селектор планов проверки
    /// </summary>
    public class AuditPlanSelectorService : BaseProxySelectorService<AuditPlanProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, AuditPlanProxy> GetCache()
        {
            return new Dictionary<long, AuditPlanProxy>();
        }

        /// <inheritdoc />
        protected override ICollection<AuditPlanProxy> GetAdditionalCache()
        {
            var planJurPersonGjiRepository = this.Container.ResolveRepository<PlanJurPersonGji>();
            var proxySelectorService = this.ProxySelectorFactory.GetSelector<GjiProxy>();

            using (this.Container.Using(planJurPersonGjiRepository))
            {
                var inspectionId = proxySelectorService.ExtProxyListCache.Single().Id;

                return planJurPersonGjiRepository.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.DateStart,
                        x.UriRegistrationNumber,
                        x.DateApproval,
                    })
                    .AsEnumerable()
                    .Select(x => new AuditPlanProxy
                    {
                        Id = x.Id, // 1. Уникальный код
                        IsPlanSigned = 1, // 2. Признак подписания плана проверок для публикации в ГИС ЖКХ.
                        ContragentInspectorId = inspectionId, // 3. Проверяющая организация
                        PlanYear = x.DateStart?.Year, // 4. Год плана проверок
                        AcceptPlanDate = (x.DateApproval ?? x.DateStart), // 5. Дата утверждения плана проверок
                        State = 1, // 7. Статус плана
                        IsNotRegistred = x.UriRegistrationNumber.HasValue ? 2 : 1, // 8. Не должен быть зарегистрирован в едином реестре проверок (Передавать 2)
                        RegistrationNumber = x.UriRegistrationNumber // 9. Регистрационный номер плана в едином реестре проверок
                    })
                    .ToList();
            }
        }
    }
}