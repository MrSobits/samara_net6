namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class FinanceSourceViewModel : BaseViewModel<FinanceSource>
    {
        public override IDataResult List(IDomainService<FinanceSource> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId", 0);

            var typeFinance = baseParams.Params.GetAs("type", 0);

            var ids = baseParams.Params.GetAs("Id", string.Empty);

            // Получение в видах работ объекта КР только тех источников финансирования которые есть у программы данного объекта КР
            List<long> finSourceByProgramList = null;
            if (objectCrId > 0)
            {
                // Через переданный Id объекта КР получаем объект по объекту программу по программе получаем таблицу связи источников и из нее тянем источник
                var objectCr = Container.Resolve<IDomainService<Entities.ObjectCr>>().Load(objectCrId);
                if (objectCr != null)
                {
                    var programCr = Container.Resolve<IDomainService<ProgramCr>>().Load(objectCr.ProgramCr.Id);
                    if (programCr != null)
                    {
                        finSourceByProgramList =
                            Container.Resolve<IDomainService<ProgramCrFinSource>>()
                                .GetAll()
                                .Where(x => x.ProgramCr.Id == programCr.Id)
                                .Select(x => x.FinanceSource.Id)
                                .Distinct()
                                .ToList();
                    }
                }
            }

            var listIds = new List<long>();
            if (!string.IsNullOrEmpty(ids))
            {
                if (ids.Contains(','))
                {
                    listIds.AddRange(ids.Split(',').Select(id => id.ToLong()));
                }
                else
                {
                    listIds.Add(ids.ToLong());
                }
            }

            var data = domainService.GetAll()
                .WhereIf(finSourceByProgramList != null, x => finSourceByProgramList.Contains(x.Id))
                .WhereIf(typeFinance > 0, x => x.TypeFinance == typeFinance.To<TypeFinance>())
                .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id)) //Для фильтра в окне мультиселекта (подгружать в сторе только те id которые выбраны)
                .Select(x => new
                {
                    x.Id,
                    x.TypeFinance,
                    x.TypeFinanceGroup,
                    x.Name,
                    x.Code
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
