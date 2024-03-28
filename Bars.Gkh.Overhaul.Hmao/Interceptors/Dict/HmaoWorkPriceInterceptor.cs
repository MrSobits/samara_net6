namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    // заменил интерцептор поскольку сущность расширелась полями
    public class HmaoWorkPriceInterceptor : Bars.Gkh.Overhaul.Interceptors.WorkPriceInterceptor<HmaoWorkPrice>
    {
        public override IDataResult CheckWorkPrice(IDomainService<HmaoWorkPrice> service, HmaoWorkPrice entity)
        {
            var capitalGroupId = entity.CapitalGroup != null ? entity.CapitalGroup.Id : 0;
            var realEstTypeId = entity.RealEstateType != null ? entity.RealEstateType.Id : 0;

            var checkMunicipality =
                service.GetAll()
                        .Where(x => x.Municipality.Id == entity.Municipality.Id  && x.Job.Id == entity.Job.Id && x.Year == entity.Year)
                        .WhereIf(capitalGroupId > 0, x => x.CapitalGroup.Id == capitalGroupId)
                        .WhereIf(capitalGroupId == 0, x => x.CapitalGroup == null)
                        .WhereIf(realEstTypeId > 0, x => x.RealEstateType.Id == realEstTypeId)
                        .WhereIf(realEstTypeId == 0, x => x.RealEstateType == null)
                       .Any(x => x.Id != entity.Id);

            if (checkMunicipality)
            {
                return new BaseDataResult(false, string.Format("Стоимость работы ('{0}') для текущей комбинации полей уже существует: Год={1}; МО={2}; Группа капитальности={3}; Тип дома={4}",
                                                                        entity.Job.Name, 
                                                                        entity.Year, 
                                                                        entity.Municipality.Name, 
                                                                        entity.CapitalGroup != null ? entity.CapitalGroup.Name : "Не задано",
                                                                        entity.RealEstateType != null ? entity.RealEstateType.Name : "Не задано"));
            }

            return Success();
        }
    }
}