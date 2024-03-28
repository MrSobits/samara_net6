namespace Bars.Gkh1468.DomainService
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Domain;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    public class PassportStructViewModel : BaseViewModel<PassportStruct>
    {
        public override IDataResult Get(IDomainService<PassportStruct> domainService, BaseParams baseParams)
        {
            var passportStructId = baseParams.Params.GetAs<long>("id");

            var hasPassports = Container.Resolve<IDomainService<HouseProviderPassport>>().GetAll().Any(x => x.PassportStruct.Id == passportStructId)
                || Container.Resolve<IDomainService<OkiProviderPassport>>().GetAll().Any(x => x.PassportStruct.Id == passportStructId);

            var passportStruct = domainService.Get(passportStructId);
            
            return new BaseDataResult(new
                                          {
                                              passportStruct.Id,
                                              passportStruct.Name,
                                              passportStruct.PassportType,
                                              passportStruct.ValidFromMonth,
                                              passportStruct.ValidFromYear,
                                              hasPassports
                                          });
        }

        public override IDataResult List(IDomainService<PassportStruct> domainService, BaseParams baseParams)
        {
            var roId = baseParams.Params.Get("roId", (long)0);

            var loadParams = this.GetLoadParam(baseParams);

            var ro = Container.Resolve<IDomainService<RealityObject>>().Get(roId);
            if (roId == 0 || ro == null)
            {
                var dateTimeFormat = CultureInfo.GetCultureInfo("Ru-ru").DateTimeFormat;

                var res = domainService.GetAll()
                    .Where(x => x.ValidFromMonth > 0 && x.ValidFromMonth < 13)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.PassportType,
                        x.ValidFromYear,
                        ValidFromMonth = dateTimeFormat.GetMonthName(x.ValidFromMonth)
                    })
                    .AsQueryable();

                var result = res.Order(loadParams).Paging(loadParams).ToArray();
                return new ListDataResult(result, res.Count());
            }

            // Фильтруем по типу дома
            var data =
                domainService.GetAll()
                             .WhereIf(
                                      IsRoOrMkdType(ro.TypeHouse.To1468RealObjType()),
                                 x => x.PassportType == GetPassportType(ro.TypeHouse.To1468RealObjType()))
                             .Filter(loadParams, Container)
                             .Order(loadParams);

            return new ListDataResult(data.Paging(loadParams), data.Count());
        }

        private bool IsRoOrMkdType(TypeRealObj typeRealObj)
        {
            return typeRealObj == TypeRealObj.Mkd || typeRealObj == TypeRealObj.RealityObject;
        }

        private PassportType GetPassportType(TypeRealObj typeRealObj)
        {
            switch (typeRealObj)
            {
                case TypeRealObj.Mkd:
                    return PassportType.Mkd;
                case TypeRealObj.RealityObject:
                    return PassportType.House;
            }

            return PassportType.Nets;
        }
    }
}