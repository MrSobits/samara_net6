namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сервис для получения управляемых домов УО
    /// </summary>
    public class DiRealityObjectViewModelService : IDiRealityObjectViewModelService
    {
        /// <summary>
        /// Домен-сервис контрактор
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }

        /// <summary>
        /// Получить список домов для раскрытия информации, находящихся в управлении УО в период раскрытия информации
        /// </summary>
        /// <param name="disclosureInfo">Деятельность управляющей организации в периоде раскрытия информации</param>
        /// <param name="disclosureInfoRealityObjId">Идентификатор дома для исключения</param>
        /// <returns></returns>
        public IQueryable<long> GetManagedRealityObjects(DisclosureInfo disclosureInfo, long disclosureInfoRealityObjId = 0)
        {
            if (disclosureInfo == null)
            {
                return Enumerable.Empty<long>().AsQueryable();
            }

            var disInfoRoObj = this.ManOrgContractRealityObjectDomain.GetAll().FirstOrDefault(x => x.Id == disclosureInfoRealityObjId);

            return this.ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id)
                .Where(x => x.RealityObject.TypeHouse != TypeHouse.BlockedBuilding
                         && x.RealityObject.TypeHouse != TypeHouse.Individual)
                .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable
                         || x.RealityObject.ConditionHouse == ConditionHouse.Emergency
                         || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated
                         && !x.RealityObject.ResidentsEvicted)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.ManOrgContract.StartDate,
                    x.ManOrgContract.EndDate
                })
                .Where(x => x.StartDate <= disclosureInfo.PeriodDi.DateEnd)
                .Where(x => !x.EndDate.HasValue || x.EndDate >= disclosureInfo.PeriodDi.DateStart)
                .WhereIf(disInfoRoObj != null, x => x.Id != disInfoRoObj.RealityObject.Id)
                .Select(x => x.Id);
        }
    }
}