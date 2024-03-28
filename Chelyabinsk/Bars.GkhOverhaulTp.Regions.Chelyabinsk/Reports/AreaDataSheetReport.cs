using System;
using System.Collections.Generic;
namespace Bars.GkhOverhaulTp.Regions.Chelyabinsk.Reports
{
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.PassportProvider;
    using Bars.GkhOverhaulTp.Regions.Chelyabinsk.Properties;

    using Castle.Windsor;

    class AreaDataSheetReport : BasePrintForm
    {
        public AreaDataSheetReport()
            : base(new ReportTemplateBinary(Resources.AreaDataSheetReport))
        {
        }

        private long[] municipalityIds;
        private int[] houseTypes;

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Отчет по площадям из техпаспорта"; }
        }

        public override string Desciption
        {
            get { return "Отчет по площадям из техпаспорта"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.AreaDataSheetReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.AreaDataSheetReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            var houseTypesList = baseParams.Params.GetAs("houseTypes", string.Empty);
            this.houseTypes = !string.IsNullOrEmpty(houseTypesList)
                                  ? houseTypesList.Split(',').Select(id => id.ToInt()).ToArray()
                                  : new int[0];
            
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт");
            if (passport == null)
            {
                throw new Exception("Не найден провайдер технического паспорта");
            }

            var realtyObjQuery = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id));

            var data = realtyObjQuery
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.TypeHouse))
                .Select(x => new
                {
                    roId = x.Id, 
                    MunName = x.Municipality.Name,
                    x.Address
                })
                .ToList();

            var dataDict = data.GroupBy(x => x.MunName).ToDictionary(x => x.Key, x => x.ToList());

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var section = sectionMu.ДобавитьСекцию("section");


            #region TechPassport
            var roIds = realtyObjQuery.Select(x => x.Id);

            var teсhPassportValueDict = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                .Where(x => roIds.Contains(x.TehPassport.RealityObject.Id)
                                               && (

                                           // Общие сведения
                                               (x.FormCode == "Form_1" && x.CellCode == "6:1") // Площадь здания всего кв.м.
                                               || (x.FormCode == "Form_1" && x.CellCode == "7:1") // в том числе: жилой части здания (кв.м), в т.ч.
                                               || (x.FormCode == "Form_1" && x.CellCode == "8:1") // нежилых помещений функционального назначения
                                               || (x.FormCode == "Form_1_3_2" && x.CellCode == "2:4") //  Места общего пользования.Коридоры мест общего пользования
                                               || (x.FormCode == "Form_1_3_2" && x.CellCode == "1:4") // Места общего пользования.Лестничные марши и площадки
                                               || (x.FormCode == "Form_1_3_2" && x.CellCode == "4:4") // Служебные помещения.Площадь технических помещений
                                               || (x.FormCode == "Form_1_3_3" && x.CellCode == "2:1"))) // Площадь подвалов
                   
                                       .Select(x => new
                                       {
                                           realtyObjId = x.TehPassport.RealityObject.Id,
                                           x.FormCode,
                                           x.CellCode,
                                           x.Value
                                       })
                                       .AsEnumerable()
                                       .GroupBy(x => x.realtyObjId)
                                        .ToDictionary(
                                               x => x.Key,
                                               x => x.GroupBy(y => y.FormCode)
                                                    .ToDictionary(
                                                        y => y.Key,
                                                        y => y.ToDictionary(
                                                                z => z.CellCode,
                                                                z => passport.GetTextForCellValue(y.Key, z.CellCode, z.Value))));

            #endregion

            var itogAreaHouse = 0M;
            var itogareaHouseLive = 0M;
            var itognotLive = 0M;
            var itogcomCorridors = 0M;
            var itogcomLadder = 0M;
            var itogbackOffice = 0M;
            var itogspaceSpecial = 0M;

            foreach (var mun in dataDict.OrderBy(x => x.Key))
            {
                var areaHouse = 0M;
                var areaHouseLive = 0M;
                var notLive = 0M;
                var comCorridors = 0M;
                var comLadder = 0M;
                var backOffice = 0M;
                var spaceSpecial = 0M;

                sectionMu.ДобавитьСтроку();
                sectionMu["MunName"] = mun.Key;

                foreach (var realObj in mun.Value)
                {
                    var dataPassport = new Dictionary<string, Dictionary<string, string>>();

                    if (teсhPassportValueDict.ContainsKey(realObj.roId))
                    {
                        dataPassport = teсhPassportValueDict[realObj.roId];
                    }

                    section.ДобавитьСтроку();

                    section["Mun"] = realObj.MunName;
                    section["Address"] = realObj.Address;
                    section["AreaHouse"] = this.GetValue(dataPassport, "Form_1", "6:1").ToDecimal();
                    section["AreaHouseLive"] = this.GetValue(dataPassport, "Form_1", "7:1").ToDecimal(); 
                    section["NotLive"] = this.GetValue(dataPassport, "Form_1", "8:1").ToDecimal();
                    section["ComCorridors"] = this.GetValue(dataPassport, "Form_1_3_2", "2:4").ToDecimal();
                    section["ComLadder"] = this.GetValue(dataPassport, "Form_1_3_2", "1:4").ToDecimal();
                    section["BackOffice"] = this.GetValue(dataPassport, "Form_1_3_2", "4:4").ToDecimal();
                    section["SpaceSpecial"] = this.GetValue(dataPassport, "Form_1_3_3", "2:1").ToDecimal();

                    areaHouse += this.GetValue(dataPassport, "Form_1", "6:1").ToDecimal();
                    areaHouseLive += this.GetValue(dataPassport, "Form_1", "7:1").ToDecimal();
                    notLive += this.GetValue(dataPassport, "Form_1", "8:1").ToDecimal();
                    comCorridors += this.GetValue(dataPassport, "Form_1_3_2", "2:4").ToDecimal();
                    comLadder += this.GetValue(dataPassport, "Form_1_3_2", "1:4").ToDecimal();
                    backOffice += this.GetValue(dataPassport, "Form_1_3_2", "4:4").ToDecimal();
                    spaceSpecial += this.GetValue(dataPassport, "Form_1_3_3", "2:1").ToDecimal();
                }

                sectionMu["TotalAreaHouse"] = areaHouse;
                sectionMu["TotalAreaHouseLive"] = areaHouseLive;
                sectionMu["TotalNotLive"] = notLive;
                sectionMu["TotalComCorridors"] = comCorridors;
                sectionMu["TotalComLadder"] = comLadder;
                sectionMu["TotalBackOffice"] = backOffice;
                sectionMu["TotalSpaceSpecial"] = spaceSpecial;

                itogAreaHouse += areaHouse;
                itogareaHouseLive += areaHouseLive;
                itognotLive += notLive;
                itogcomCorridors += comCorridors;
                itogcomLadder += comLadder;
                itogbackOffice += backOffice;
                itogspaceSpecial += spaceSpecial; 

            }
            reportParams.SimpleReportParams["ItogAreaHouse"] = itogAreaHouse;
            reportParams.SimpleReportParams["ItogAreaHouseLive"] = itogareaHouseLive;
            reportParams.SimpleReportParams["ItogNotLive"] = itognotLive;
            reportParams.SimpleReportParams["ItogComCorridors"] = itogcomCorridors;
            reportParams.SimpleReportParams["ItogComLadder"] = itogcomLadder;
            reportParams.SimpleReportParams["ItogBackOffice"] = itogbackOffice;
            reportParams.SimpleReportParams["ItogSpaceSpecial"] = itogspaceSpecial;
            
        }

        private string GetValue(Dictionary<string, Dictionary<string, string>> dictData, string codeForm, string codeCell)
        {
            if (dictData.ContainsKey(codeForm))
            {
                if (dictData[codeForm].ContainsKey(codeCell))
                {
                    return dictData[codeForm][codeCell];
                }
            }

            return "0";
        }

    }
}
