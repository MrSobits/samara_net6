namespace Bars.Gkh.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;    
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class ControlActivityDatOfContractByUK : BasePrintForm
    {
        public ControlActivityDatOfContractByUK() : base(new ReportTemplateBinary(Properties.Resources.ControlActivityDatOfContractByUK))
        {
        }

        #region Входящие параметры
        private long[] municipalityIds;
        #endregion

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Контроль заполнения сроков действия договора с УК";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Контроль заполнения сроков действия договора с УК";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Формы программы";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ControlActivityDatOfContractByUK";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.ControlActivityDatOfContractByUK";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var servceData = Container.Resolve<IDomainService<ManOrgBaseContract>>();
            var serviceRealObj = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            var queryManOrgContractRealityObjectMu = serviceRealObj.GetAll().WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id));

            var queryManOrgContrRoIds = queryManOrgContractRealityObjectMu.Select(x => x.ManOrgContract.Id).Distinct();

            var dictRoInfo = queryManOrgContractRealityObjectMu
                .Select(x => new
                                {
                                    MuName = x.RealityObject.Municipality.Name,
                                    x.RealityObject.Address,
                                    ManOrgContractId = x.ManOrgContract.Id
                                })
                .AsEnumerable()
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.Address)
                .GroupBy(x => x.Address)
                .Select(x => new
                                {
                                    Address = x.Key,
                                    ListManOrgContractId = x.Select(y => y.ManOrgContractId).ToList(),
                                    MuName = x.Select(y => y.MuName).First()
                                })
                .ToDictionary(x => x.Address, v => v);

             var dictContractInfo =
                servceData.GetAll()
                           .Where(x => queryManOrgContrRoIds.Contains(x.Id))
                           .Select(x => new
                                        {
                                            ManOrgContractId = x.Id,
                                            ManOrg = x.ManagingOrganization != null ? x.ManagingOrganization.Contragent.Name : "Непосредственное управление",
                                            TypeContractString = x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj ? "Передача управления" : "Основной",
                                            TypeContractManOrgRealObj = x.TypeContractManOrgRealObj.GetEnumMeta().Display,
                                            DateStart = x.StartDate,
                                            DateEnd = x.EndDate
                                        })
                           .ToDictionary(
                               x => x.ManOrgContractId,
                               v => new
                                   {
                                       v.ManOrg,
                                       v.TypeContractString,
                                       v.TypeContractManOrgRealObj,
                                       v.DateStart,
                                       v.DateEnd
                                   });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("SectionData");

            foreach (var ro in dictRoInfo)
            {
                var data = ro.Value;
                var listManOrgContractId = data.ListManOrgContractId;
                var listManOrgContractIdCount = listManOrgContractId.Count;

                foreach (var id in listManOrgContractId)
                {
                    section.ДобавитьСтроку();

                    section["MuName"] = data.MuName;
                    section["Address"] = ro.Key;
                    section["CountManOrg"] = listManOrgContractIdCount;

                    if (!dictContractInfo.ContainsKey(id))
                    {
                        continue;
                    }

                    var contractInfo = dictContractInfo[id];
                    section["ManOrgName"] = contractInfo.ManOrg;
                    section["TypeContract"] = contractInfo.TypeContractString;
                    section["TypeContractManOrg"] = GetTypeContractManOrg(contractInfo.TypeContractManOrgRealObj);
                    section["DateStart"] = contractInfo.DateStart;
                    section["DateEnd"] = contractInfo.DateEnd;
                }                
            }
        }

        private string GetTypeContractManOrg(string typeContractManOrgRealObj)
        {
            var result = string.Empty;
            switch (typeContractManOrgRealObj)
            {
                case "ТСЖ/ЖСК":
                    result = "ТСЖ/ЖСК";
                    break;
                case "УК с ТСЖ/ЖСК":
                    result = "Договор между ТСЖ и УК";
                    break;
                case "УК с собственниками":
                    result = "Договор между собственниками и УК";
                    break;
                default:
                    result = typeContractManOrgRealObj;
                    break;
            }

            return result;
        }
    }
}