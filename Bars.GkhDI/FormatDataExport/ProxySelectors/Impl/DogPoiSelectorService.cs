namespace Bars.GkhDi.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    public class DogPoiSelectorService : BaseProxySelectorService<DogPoiProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DogPoiProxy> GetCache()
        {
            var infoAboutUseCommonFacilitiesRepository = this.Container.ResolveRepository<InfoAboutUseCommonFacilities>();

            using (this.Container.Using(infoAboutUseCommonFacilitiesRepository))
            {
                var indDict = this.ProxySelectorFactory.GetSelector<IndProxy>().ProxyListCache.Values
                    .Select(x => new
                    {
                        x.Id,
                        x.Surname,
                        x.FirstName,
                        x.SecondName
                    })
                    .AsEnumerable()
                    .GroupBy(x => $"{x.Surname}|{x.FirstName}|{x.SecondName}".ToLower(), x => (long?) x.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var contragentDict = this.ProxySelectorFactory.GetSelector<ContragentProxy>()
                    .ProxyListCache.Values
                    .ToDictionary(x => $"{x.Inn}|{x.Ogrn}", x => (long?) x.Id);

                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();

                return infoAboutUseCommonFacilitiesRepository.GetAll()
                    .Where(x => roQuery.Any(r => r.Id == x.DisclosureInfoRealityObj.RealityObject.Id))
                    .Select(x => new
                    {
                        x.Id,
                        RoId = (long?) x.DisclosureInfoRealityObj.RealityObject.Id,
                        x.Surname,
                        x.Name,
                        x.Patronymic,
                        x.LesseeType,
                        x.ContractNumber,
                        x.SigningContractDate,
                        x.DateStart,
                        x.DateEnd,
                        x.ContractSubject,
                        x.Comment,
                        x.CostByContractInMonth,
                        x.CostContract,
                        x.ContractFile,
                        x.ProtocolFile,
                        x.KindCommomFacilities,
                        x.AppointmentCommonFacilities,
                        x.AreaOfCommonFacilities,
                        x.Inn,
                        x.Ogrn
                    })
                    .AsEnumerable()
                    .Select(x => new DogPoiProxy
                    {
                        Id = x.Id,
                        RealityObjectId = x.RoId,
                        IndividualAccountId = x.LesseeType == LesseeTypeDi.Individual
                            ? indDict.Get($"{x.Surname}|{x.Name}|{x.Patronymic}".ToLower())
                            : null,
                        ContragentId = x.LesseeType == LesseeTypeDi.Legal
                            ? contragentDict.Get($"{x.Inn}|{x.Ogrn}")
                            : null,
                        DocumentNumber = x.ContractNumber,
                        DocumentCreateDate = x.SigningContractDate ?? x.DateStart,
                        DocumentStartDate = x.DateStart,
                        DocumentPlanedEndDate = x.DateEnd,
                        ActionEndDate = x.DateEnd,
                        Subject = x.ContractSubject,
                        Comment = x.Comment,
                        CostContract = x.CostByContractInMonth ?? x.CostContract,
                        Status = 1,

                        ContractFile = x.ContractFile,
                        ProtocolFile = x.ProtocolFile,

                        KindCommomFacilities = x.KindCommomFacilities,
                        AppointmentCommonFacilities = x.AppointmentCommonFacilities,
                        AreaOfCommonFacilities = x.AreaOfCommonFacilities
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}