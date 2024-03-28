namespace Bars.Gkh.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    public class WorkViewModel : BaseViewModel<Work>
    {
        public override IDataResult List(IDomainService<Work> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var um = loadParams.Filter.GetAs<long>("UnitMeasure");

            var isAdditionalWorks = baseParams.Params.GetAs("isAdditionalWorks", false);
            var isActual = baseParams.Params.GetAs("isActual", false);
            var isConstructionWorks = baseParams.Params.GetAs("isConstructionWorks", false);

            var workIds = baseParams.Params.ContainsKey("ids") ? baseParams.Params["ids"].ToStr() : string.Empty;
            var onlyByWorkId = baseParams.Params.ContainsKey("onlyByWorkId")
                               && baseParams.Params["onlyByWorkId"].ToBool(); // флаг получения работ по переданным Id
            var onlyWorks = baseParams.Params.ContainsKey("onlyWorks") && baseParams.Params["onlyWorks"].ToBool();
            var listWorkIds = new List<long>();
            if (!string.IsNullOrEmpty(workIds))
            {
                if (workIds.Contains(','))
                {
                    listWorkIds.AddRange(workIds.Split(',').Select(id => id.ToLong()));
                }
                else
                {
                    listWorkIds.Add(workIds.ToInt());
                }
            }

            return domain.GetAll()
                .WhereIf(isAdditionalWorks, x => x.IsAdditionalWork || x.TypeWork == TypeWork.Service)
                .WhereIf(isConstructionWorks, x => x.IsConstructionWork)
                .WhereIf(um > 0, x => x.UnitMeasure.Id == um)
                .WhereIf(onlyByWorkId, x => listWorkIds.Contains(x.Id)) // Флаг нужен для того что бы точно знать что мы получаем работы по переданным Id, например если мы в справочнике то список пустой флаг false и видим всех. Если затягиваем работы по id из реестра обекта КР и в фильтре нет работ то не видим ничего
                .WhereIf(onlyWorks, x => x.TypeWork == TypeWork.Work)
                .WhereIf(isActual, x => x.IsActual)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.ReformCode,
                    x.GisCode,
                    x.WorkAssignment,
                    x.Description,
                    UnitMeasureName = x.UnitMeasure.Name,
                    x.IsAdditionalWork,
                    x.Consistent185Fz,
                    x.IsPSD,
                    x.TypeWork,
                    x.IsActual
                })
                .ToListDataResult(loadParams);
        }
    }
}