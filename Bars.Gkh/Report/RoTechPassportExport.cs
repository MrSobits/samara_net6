namespace Bars.Gkh.Report
{
    using System;
    using Bars.Gkh.Enums;
    using Bars.Gkh.PassportProvider;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Entities;
    using Properties;
    using Castle.Windsor;


    public class RoTechPassportExport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }
        public long[] MunicipalityIds;

        private bool typeManyApartments;
        private bool typeSocialBehavior;
        private bool typeIndividual;
        private bool typeBlockedBuilding;
        private bool typeNotSet;

        private string[] exeptions = 
            {
                "Form_1_1", "Form_1_2", "Form_1_2_2", "Form_1_2_3", "Form_3_1_3", "Form_3_2_3", "Form_3_3_3", "Form_3_4_2",
                "Form_4_1_1", "Form_4_2", "Form_4_2_1"
            };
        
        public RoTechPassportExport()
            : base(new ReportTemplateBinary(Resources.RoTechPassportExport))
        {
        }

        public override string Name
        {
            get
            {
                return "Экспорт технических паспортов домов";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Экспорт технических паспортов домов";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Технический паспорт дома";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.RoTechPassportExport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.RoTechPassportExport";
            }
        }

        private IEnumerable<long> GetChildrenMoIds(long[] parentMo)
        {
            if (!parentMo.Any()) return new List<long>();

            var muService = Container.Resolve<IDomainService<Municipality>>();

            try
            {
                var childrenListIds = muService.GetAll()
                .Where(x => x.ParentMo != null && parentMo.Contains(x.ParentMo.Id))
                .Select(x => x.Id)
                .ToList();

                if (childrenListIds.Any())
                {
                    childrenListIds.AddRange(GetChildrenMoIds(childrenListIds.ToArray()));
                }

                return childrenListIds;
            }
            finally 
            {
                Container.Release(muService);
            }
            
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            var municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToList()
                                  : new List<long>();

            if (municipalityIds.Count() > 0)
            {
                municipalityIds.AddRange(GetChildrenMoIds(municipalityIds.ToArray()));    
            }
            
            MunicipalityIds = municipalityIds.ToArray();

            typeManyApartments = baseParams.Params.GetAs("typeManyApartments", false);
            typeSocialBehavior = baseParams.Params.GetAs("typeSocialBehavior", false);
            typeBlockedBuilding = baseParams.Params.GetAs("typeBlockedBuilding", false);
            typeIndividual = baseParams.Params.GetAs("typeIndividual", false);
            typeNotSet = baseParams.Params.GetAs("typeNotSet", false);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            // получаем провайдер реализующий необходимый паспорт
            var passportProvider = Container.ResolveAll<IPassportProvider>();
            var roService = Container.Resolve<IDomainService<RealityObject>>();
            var valuesService = Container.Resolve<IRepository<TehPassportValue>>();

            try
            {
                var passport = passportProvider.FirstOrDefault(x => x.Name == "Техпаспорт");
                if (passport == null)
                {
                    throw new Exception("Не найден провайдер технического паспорта");
                }

                var roQuery = roService.GetAll()
                    .WhereIf(MunicipalityIds.Length > 0, x => MunicipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(!typeManyApartments, x => x.TypeHouse != TypeHouse.ManyApartments)
                    .WhereIf(!typeSocialBehavior, x => x.TypeHouse != TypeHouse.SocialBehavior)
                    .WhereIf(!typeIndividual, x => x.TypeHouse != TypeHouse.Individual)
                    .WhereIf(!typeBlockedBuilding, x => x.TypeHouse != TypeHouse.BlockedBuilding)
                    .WhereIf(!typeNotSet, x => x.TypeHouse != TypeHouse.NotSet);

                var roData = roQuery
                    .Select(x => new
                    {
                        x.Id,
                        muName = x.Municipality.Name,
                        x.Address,
                        x.FiasAddress.StreetGuidId,
                        x.FiasAddress.House,
                        x.FiasAddress.Housing,
                        x.TypeHouse,
                    })
                    .OrderBy(x => x.TypeHouse)
                    .ThenBy(x => x.muName)
                    .ThenBy(x => x.Address)
                    .ToDictionary(x => x.Id);

                var data = valuesService.GetAll()
                    .WhereIf(MunicipalityIds.Length > 0,
                        x => roQuery.Any(y => y.Id == x.TehPassport.RealityObject.Id))
                    .Where(x => !exeptions.Contains(x.FormCode))
                    .Select(x => new
                    {
                        x.TehPassport.RealityObject.Id,
                        x.FormCode,
                        x.CellCode,
                        x.Value
                    }).ToArray();

                var columnData = data.Select(x => new
                {
                    ColumnCode = x.FormCode + ":" + x.CellCode,
                    CellName = passport.GetLabelForFormElement(x.FormCode, x.CellCode)
                })
                .Where(x => !string.IsNullOrEmpty(x.CellName))
                .OrderBy(x => x.ColumnCode)
                .Distinct(x => x.ColumnCode)
                .Take(251)
                .ToArray();

                var tpData = data
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => new
                    {
                        ColumnCode = z.FormCode + ":" + z.CellCode,
                        z.Value
                    }));

                var column = reportParams.ComplexReportParams.ДобавитьСекцию("колонка");

                foreach (var col in columnData)
                {
                    column.ДобавитьСтроку();

                    column["code"] = col.CellName;
                    column["template"] = "$" + col.ColumnCode.Replace(':', 'z') + "$";
                }

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

                foreach (var item in roData)
                {
                    section.ДобавитьСтроку();

                    section["StreetGuidId"] = item.Value.StreetGuidId;
                    section["House"] = item.Value.House;
                    section["Housing"] = item.Value.Housing;
                    section["TypeHouse"] = item.Value.TypeHouse.GetEnumMeta().Display;

                    if (tpData.ContainsKey(item.Value.Id))
                    {
                        var oneRoTp = tpData[item.Value.Id];

                        foreach (var val in oneRoTp)
                        {
                            try
                            {
                                section[val.ColumnCode.Replace(':', 'z')] = val.Value;
                            }
                            catch (ReportParamsException ex)
                            {
                            }
                        }
                    }
                }
            }
            finally 
            {
                Container.Release(passportProvider);
                Container.Release(roService);
                Container.Release(valuesService);
            }
        }
    }
}
