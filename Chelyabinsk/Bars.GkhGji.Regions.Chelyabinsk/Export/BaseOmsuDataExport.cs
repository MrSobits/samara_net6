namespace Bars.GkhGji.Regions.Chelyabinsk.DataExport
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Entities;
    using GkhGji.Entities;
    using System.Collections.Generic;
    using GkhGji.Enums;

    public class BaseOmsuDataExport : BaseDataExportService
    {
        public IDomainService<BaseOMSU> BaseOMSUDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionGjiInspectorDomain { get; set; }


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
            //var appealCitsRealityObject = AppealCitsRealityObjectDomain.GetAll();

            var baseInsp = BaseOMSUDomain.GetAll()
                   .WhereIf(dateStart != DateTime.MinValue, x => x.DateStart >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DateStart <= dateEnd)
                .Select(x => x.Id).ToList();

            Dictionary<long, string> inspDict = new Dictionary<long, string>();
            var dataInspectors = InspectionGjiInspectorDomain.GetAll()
                  .Where(x => baseInsp.Contains(x.Inspection.Id))
                    .ToList();
            if (dataInspectors.Count > 0)
            {
                foreach (InspectionGjiInspector inspinsp in dataInspectors)
                {
                    if (!inspDict.ContainsKey(inspinsp.Inspection.Id))
                    {
                        inspDict.Add(inspinsp.Inspection.Id, inspinsp.Inspector.Fio);
                    }
                    else
                    {
                        inspDict[inspinsp.Inspection.Id] += ", " + inspinsp.Inspector.Fio;
                    }
                }
            }

            Dictionary<TypeFactInspection, string> typeFacts = new Dictionary<TypeFactInspection, string>();
            typeFacts.Add(TypeFactInspection.Cancelled, "Отменена");
            typeFacts.Add(TypeFactInspection.Changed, "Перенесена");
            typeFacts.Add(TypeFactInspection.Done, "Проведена");
            typeFacts.Add(TypeFactInspection.NotDone, "Не проведена");
            typeFacts.Add(TypeFactInspection.NotSet, "Не задано");

            var data = BaseOMSUDomain.GetAll()
                  .Where(x => x.DateStart >= dateStart && x.DateStart <= dateEnd)
                .Select(x => new
                        {
                            x.Id,
                            x.DateStart,
                            Contragent = x.Contragent.Name,
                            x.InspectionYear,
                            Plan = x.Plan.Name,
                            x.Reason,
                            TypeFact = typeFacts.ContainsKey(x.TypeFact)
                                            ? typeFacts[x.TypeFact]
                                                : "Не установлено",
                            State = x.State.Name,
                            Municipality = x.Contragent.Municipality != null 
                                                ? x.Contragent.Municipality.Name
                                                    : "Не указано",
                            OmsuPerson = x.OmsuPerson,
                            InspectorNames = inspDict.ContainsKey(x.Id) ? inspDict[x.Id] : ""
                })
                    .Filter(loadParam, Container)
                    .Order(loadParam)
                    .ToList();

            return data;
        }
    }
}