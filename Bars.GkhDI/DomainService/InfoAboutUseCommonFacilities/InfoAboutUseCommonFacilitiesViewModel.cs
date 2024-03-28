namespace Bars.GkhDi.DomainService
{
    using B4;
    using Entities;
    using System;
    using System.Linq;

    public class InfoAboutUseCommonFacilitiesViewModel : BaseViewModel<InfoAboutUseCommonFacilities>
    {
        public override IDataResult List(IDomainService<InfoAboutUseCommonFacilities> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
            var disclosureInfoRealityObj =
                this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>()
                    .GetAll()
                    .Where(x => x.Id == disclosureInfoRealityObjId)
                    .Select(x => new { x.Id, x.RealityObject, x.PeriodDi })
                    .FirstOrDefault();

            if (disclosureInfoRealityObj == null)
            {
                return new ListDataResult();
            }

            var periodDi = disclosureInfoRealityObj.PeriodDi;

            var data = domainService.GetAll()
                           .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id)
                           /* Фильтрация по датам документа. Период дата начала - дата конца документа должен пересекаться
                           с периодом дата начала - дата конца периода раскрытия*/
                           .Where(x => periodDi != null
                               && (((periodDi.DateStart.HasValue && x.DateStart >= periodDi.DateStart.Value
                                     || !periodDi.DateStart.HasValue)
                                    && (periodDi.DateEnd.HasValue && periodDi.DateEnd.Value >= x.DateStart
                                        || !periodDi.DateEnd.HasValue))
                                   || ((periodDi.DateStart.HasValue && periodDi.DateStart.Value >= x.DateStart)
                                       && (periodDi.DateStart.HasValue && x.DateEnd >= periodDi.DateStart.Value
                                           || x.DateEnd <= DateTime.MinValue))))
                           .Select(
                               x => new
                               {
                                   x.Id,
                                   x.KindCommomFacilities,
                                   x.From,
                                   x.Number,
                                   x.DateStart,
                                   x.DateEnd,
                                   x.Lessee,
                                   x.TypeContract,
                                   x.CostContract,
                                   ProtocolFileName = x.ProtocolFile != null ? x.ProtocolFile.Name : string.Empty,
                                   x.ProtocolFile,
                                   x.LesseeType,
                                   x.Surname,
                                   x.Name,
                                   x.Gender,
                                   x.BirthDate,
                                   x.BirthPlace,
                                   x.Snils,
                                   x.Ogrn,
                                   x.Inn,
                                   ContractFileName = x.ContractFile != null ? x.ContractFile.Name : string.Empty,
                                   x.ContractFile,
                                   x.ContractSubject
                               })
                           .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}