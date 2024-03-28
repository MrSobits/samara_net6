namespace Bars.GkhGji.Regions.Voronezh.DataExport
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Entities;
    using Bars.GkhGji.Entities;

    public class AdmonitionDataExport : BaseDataExportService
    {
        public IDomainService<AppealCitsAdmonition> AppealCitsAdmonitionDomain { get; set; }

        public IDomainService<AppCitAdmonVoilation> AppCitAdmonVoilationDomain { get; set; }

        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */
               
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var appealCitsRealityObject = AppealCitsRealityObjectDomain.GetAll();

            Dictionary<long, string> dict = new Dictionary<long, string>();
            var admViolDomain = AppCitAdmonVoilationDomain.GetAll()
                 .Where(x => x.AppealCitsAdmonition.PerfomanceDate.HasValue
                        ? x.AppealCitsAdmonition.PerfomanceDate.Value >= dateStart && x.AppealCitsAdmonition.PerfomanceDate.Value <= dateEnd
                        : 1 == 1)
                        .Select(x => new
                        {
                            x.AppealCitsAdmonition.Id,
                            x.ViolationGji.Name
                        }).AsEnumerable();
            foreach (var zap in admViolDomain)
            {
                if (!dict.ContainsKey(zap.Id))
                {
                    dict.Add(zap.Id, zap.Name);
                }
                else
                {
                    dict[zap.Id] += "; " + zap.Name;
                }
            }

            var data = AppealCitsAdmonitionDomain.GetAll()
                    .Join(
                        appealCitsRealityObject.AsEnumerable(),
                        x => x.AppealCits.Id,
                        y => y.AppealCits.Id,
                        (x, y) => new
                        {
                            x.Id,
                            x.DocumentName,
                            x.PerfomanceDate,
                            x.PerfomanceFactDate,
                            x.DocumentDate,
                            Contragent = x.Contragent.Name,
                            x.File,
                            Inspector = x.Inspector.Fio,
                            Municipality = y.RealityObject.Municipality.Name,
                            Violations = dict.ContainsKey(x.Id) ? dict[x.Id] : "",
                            Address = y.RealityObject.Address,
                            x.AnswerFile,
                            Executor = x.Executor.Fio,
                            x.DocumentNumber
                        })
                    .Where(x => x.PerfomanceDate.HasValue
                            ? x.PerfomanceDate.Value >= dateStart && x.PerfomanceDate.Value <= dateEnd
                            : 1 == 1)
                    .Filter(loadParam, Container)
                    .Order(loadParam)
                    .ToList();

            return data;
        }
    }
}